using UnityEngine;

// Menambahkan AudioSource otomatis untuk Manager
[RequireComponent(typeof(AudioSource))]
public class HumidityMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "HumidityMinigame";

    [Header("Sliders")]
    public HumiditySlider[] sliders;

    [Header("Allowed Values")]
    public int[] allowedValues = { 35, 50, 85 };

    [Header("Loading")]
    public float loadingDuration = 13f;

    [Header("Clue")]
    public CluePopup cluePopup;

    [Header("Audio Settings")]
    public AudioClip successSound; // Suara saat berhasil
    public AudioClip failSound;    // Suara saat gagal
    public AudioClip fixingSound;  // Suara saat slider sedang diproses (looping)

    private int majorityValue;
    private bool hasWon;
    
    // Variabel Audio
    private AudioSource audioSource;
    private int activeLoadingCount = 0; // Menghitung berapa slider yang sedang di-fix

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void StartHumidityMinigame()
    {
        if (StageManager.Instance == null ||
            StageManager.Instance.CurrentStage < 2)
            return;

        GenerateNewRound();
    }

    public void SliderButtonPressed(HumiditySlider slider)
    {
        if (hasWon || slider == null || slider.IsLocked || slider.IsLoading)
            return;

        // Memulai suara fixing jika ini adalah slider pertama yang ditekan
        activeLoadingCount++;
        if (activeLoadingCount == 1 && fixingSound != null)
        {
            audioSource.clip = fixingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        slider.PlayLoadingPulse(loadingDuration, () =>
        {
            // Mengurangi jumlah slider yang aktif
            activeLoadingCount--;
            
            // Hentikan suara fixing jika tidak ada lagi slider yang sedang di-fix
            if (activeLoadingCount <= 0)
            {
                activeLoadingCount = 0;
                audioSource.Stop();
            }

            if (hasWon || slider == null || slider.IsLocked)
                return;

            if (allowedValues == null || allowedValues.Length == 0)
            {
                Debug.LogError("HumidityMinigame: No allowed values assigned.");
                return;
            }

            int newValue = allowedValues[Random.Range(0, allowedValues.Length)];
            slider.SetValue(newValue);

            if (newValue == majorityValue)
            {
                slider.Lock();
                // Mainkan suara sukses (PlayOneShot agar tidak memotong suara fixing jika ada slider lain)
                if (successSound != null) audioSource.PlayOneShot(successSound);
            }
            else
            {
                // Mainkan suara gagal
                if (failSound != null) audioSource.PlayOneShot(failSound);
            }

            CheckComplete();
        });
    }

    // ... (Sisa fungsi GenerateNewRound, CheckComplete, ReportDiagnosisClue, ResetMinigame tetap sama seperti aslinya, 
    // hanya hapus pemanggilan audioManager.playHumSuccessSFX() dan playHumFailSFX() yang ada di dalam GenerateNewRound aslinya)

    void GenerateNewRound()
    {
        hasWon = false;
        activeLoadingCount = 0; // Reset counter audio saat ronde baru
        audioSource.Stop();

        if (DiagnosisManager.Instance == null) return;
        string requiredHumidity = DiagnosisManager.Instance.GetRequiredObservation("Humidity");
        if (string.IsNullOrEmpty(requiredHumidity)) return;
        string cleanHumidity = requiredHumidity.Replace("%", "").Trim();
        if (!int.TryParse(cleanHumidity, out majorityValue)) return;

        int[] majoritySlots = new int[sliders.Length];
        for (int i = 0; i < majoritySlots.Length; i++) majoritySlots[i] = i;
        for (int i = majoritySlots.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = majoritySlots[i];
            majoritySlots[i] = majoritySlots[randomIndex];
            majoritySlots[randomIndex] = temp;
        }

        int majorityCount = Mathf.Min(3, sliders.Length);
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null) continue;

            sliders[i].CancelLoadingPulse();
            sliders[i].Unlock();

            bool isMajority = false;
            for (int j = 0; j < majorityCount; j++)
            {
                if (majoritySlots[j] == i)
                {
                    isMajority = true;
                    break;
                }
            }

            if (isMajority)
            {
                sliders[i].SetValue(majorityValue);
                sliders[i].Lock();
                // Dihapus: audioManager.playHumSuccessSFX();
            }
            else
            {
                int startingValue;
                do { startingValue = Random.Range(0, 301); } while (startingValue == majorityValue);
                sliders[i].SetValue(startingValue);
                // Dihapus: audioManager.playHumFailSFX();
            }
        }
    }

    void CheckComplete()
    {
        if (sliders == null || sliders.Length == 0) return;
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null) return;
            if (!sliders[i].IsLocked) return;
        }
        hasWon = true;
        ReportDiagnosisClue();
        if (StageManager.Instance != null) StageManager.Instance.MinigameWon(MinigameName);
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null) return;
        string humidityClue = DiagnosisManager.Instance.GetRequiredObservation("Humidity");
        if (string.IsNullOrEmpty(humidityClue)) return;
        DiagnosisManager.Instance.SetObservation("Humidity", humidityClue);
        if (cluePopup != null) cluePopup.ShowClue("Humidity", humidityClue);
    }

    public void ResetMinigame()
    {
        hasWon = false;
        majorityValue = 0;
        activeLoadingCount = 0;
        if (audioSource != null) audioSource.Stop();
        
        if (cluePopup != null) cluePopup.HideClue();
        if (sliders == null) return;
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null) continue;
            sliders[i].CancelLoadingPulse();
            sliders[i].Unlock();
        }
    }
}