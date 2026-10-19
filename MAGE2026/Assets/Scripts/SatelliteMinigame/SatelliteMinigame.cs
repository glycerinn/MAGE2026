using UnityEngine;

// Memastikan objek memiliki AudioSource
[RequireComponent(typeof(AudioSource))]
public class SatelliteMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "SatelliteMinigame";

    [Header("Line")]
    public Transform linePivot;
    public Transform line;
    public float rotationSpeed = 90f;

    [Header("Target")]
    public Transform targetPivot;
    public Transform targetBar;

    [Header("Angles")]
    public float targetMinAngle = -90f;
    public float targetMaxAngle = 90f;

    [Header("Alignment")]
    public float acceptableAngle = 5f;
    public float requiredHoldTime = 2f;

    [Header("Input")]
    public KeyCode rotateLeft = KeyCode.A;
    public KeyCode rotateRight = KeyCode.D;

    [Header("Clue")]
    public CluePopup cluePopup;

    [Header("Win Condition")]
    public int winAmount = 5;

    [Header("Audio Settings")]
    public AudioClip engineSound; // Suara saat satelit bergerak
    public AudioClip fixSound;    // Suara saat satelit sejajar (berhasil di-lock)

    private float lineAngle;
    private float targetAngle;
    private int completedRounds;
    private float alignmentTimer;
    private bool hasWon;
    
    // Variabel Audio
    private AudioSource audioSource;
    private bool isFixSFXPlaying;

    public void Awake()
    {
        // Mengambil referensi AudioSource
        audioSource = GetComponent<AudioSource>();
        // Memastikan audio tidak otomatis dimainkan dan bisa di-loop jika perlu
        audioSource.playOnAwake = false; 
    }

    void Start()
    {
        ResetMinigame();
    }

    void Update()
    {
        if (hasWon)
            return;

        RotateLine();
        CheckAlignment();
    }

    void RotateLine()
    {
        float direction = 0f;

        if (Input.GetKey(rotateLeft))
            direction += 1f;

        if (Input.GetKey(rotateRight))
            direction -= 1f;

        // Jika ada input pergerakan
        if (direction != 0f)
        {
            lineAngle += direction * rotationSpeed * Time.deltaTime;

            lineAngle = Mathf.Clamp(
                lineAngle,
                targetMinAngle,
                targetMaxAngle
            );

            linePivot.localRotation = Quaternion.Euler(0f, 0f, lineAngle);

            // Handle suara mesin
            if (engineSound != null && !isFixSFXPlaying) 
            {
                // Jika audioSource tidak sedang memainkan suara mesin, mainkan
                if (audioSource.clip != engineSound || !audioSource.isPlaying)
                {
                    audioSource.clip = engineSound;
                    audioSource.loop = true; // Loop suara mesin selama bergerak
                    audioSource.Play();
                }
            }
        }
        else
        {
            // Jika tidak ada input dan suara yang sedang diputar adalah suara mesin, hentikan
            if (audioSource.clip == engineSound && audioSource.isPlaying && !isFixSFXPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void CheckAlignment()
    {
        float difference = Mathf.Abs(
            Mathf.DeltaAngle(lineAngle, targetAngle)
        );

        if (difference <= acceptableAngle)
        {
            alignmentTimer += Time.deltaTime;

            if (!isFixSFXPlaying)
            {
                // Saat mulai sejajar, hentikan suara mesin dan mainkan suara "Fix"
                audioSource.Stop(); 
                audioSource.clip = fixSound;
                audioSource.loop = true; // Loop suara fix selama ditahan
                audioSource.Play();
                
                isFixSFXPlaying = true;
            }

            if (alignmentTimer >= requiredHoldTime)
            {
                CompleteRound();
                audioSource.Stop();
                isFixSFXPlaying = false;
            }
        }
        else
        {
            alignmentTimer = 0f;

            if (isFixSFXPlaying)
            {
                // Jika keluar dari area sejajar, hentikan suara "Fix"
                audioSource.Stop();
                isFixSFXPlaying = false;
            }
        }
    }

    void CompleteRound()
    {
        alignmentTimer = 0f;
        completedRounds++;

        Debug.Log(
            "Satellite round complete! " +
            completedRounds + "/" + winAmount
        );

        if (completedRounds >= winAmount)
        {
            hasWon = true;

            ReportDiagnosisClue();

            Debug.Log("SATELLITE MINIGAME WON!");

            StageManager.Instance.MinigameWon(MinigameName);
            return;
        }

        GenerateNewTarget();
    }

    void GenerateNewTarget()
    {
        targetAngle = Random.Range(
            targetMinAngle,
            targetMaxAngle
        );

        targetPivot.localRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        alignmentTimer = 0f;
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
            return;

        string satelliteClue =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Satellite"
            );

        if (string.IsNullOrEmpty(satelliteClue))
        {
            Debug.LogError(
                "No Satellite diagnosis clue found."
            );
            return;
        }

        DiagnosisManager.Instance.SetObservation(
            "Satellite",
            satelliteClue
        );

        if (cluePopup != null)
        {
            cluePopup.ShowClue(
                "Satellite",
                satelliteClue
            );
        }
    }

    public void ResetMinigame()
    {
        completedRounds = 0;
        hasWon = false;
        alignmentTimer = 0f;
        lineAngle = 0f;

        if (cluePopup != null)
            cluePopup.HideClue();

        linePivot.localRotation =
            Quaternion.identity;

        targetPivot.localRotation =
            Quaternion.identity;

        // Pastikan suara berhenti saat minigame di-reset
        if (audioSource != null)
        {
             audioSource.Stop();
        }
        isFixSFXPlaying = false;

        GenerateNewTarget();
    }
}