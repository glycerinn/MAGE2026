using UnityEngine;

public class SliderMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "SliderMinigame";

    [Header("Sliders")]
    public ObjectSlider[] sliders;

    [Header("Rainfall Clue")]
    public CluePopup cluePopup;

    [Header("Win Condition")]
    public int winAmount = 5;

    private int completedRounds;
    private bool hasWon;

    void Start()
    {
        ResetMinigame();
    }

    public void SelectSlider(ObjectSlider selectedSlider)
    {
        if (hasWon)
            return;

        if (sliders == null || sliders.Length == 0)
        {
            Debug.LogError("Rainfall: No sliders are assigned.");
            return;
        }

        if (selectedSlider == null)
        {
            Debug.LogError("Rainfall: Selected slider is null.");
            return;
        }

        int[] values = new int[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null)
            {
                Debug.LogError("Rainfall: Slider at index " + i + " is not assigned.");
                return;
            }

            values[i] = sliders[i].Value;
        }

        System.Array.Sort(values);

        int medianValue = values[values.Length / 2];

        if (selectedSlider.Value != medianValue)
        {
            Debug.Log("Rainfall: Wrong slider selected.");
            return;
        }

        completedRounds++;

        Debug.Log("Rainfall round complete! " + completedRounds + "/" + winAmount);

        if (completedRounds < winAmount)
        {
            GenerateNewRound();
            return;
        }

        if (!ReportDiagnosisClue())
        {
            Debug.LogWarning("Rainfall: Could not report clue. Minigame remains active.");
            return;
        }

        hasWon = true;

        Debug.Log("RAINFALL MINIGAME WON!");

        if (StageManager.Instance != null)
            StageManager.Instance.MinigameWon(MinigameName);
        else
            Debug.LogError("Rainfall: StageManager.Instance is null.");
    }

    void GenerateNewRound()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            int value;
            bool valid;
            int attempts = 0;

            do
            {
                value = Random.Range(0, 101);
                valid = true;

                for (int j = 0; j < i; j++)
                {
                    if (Mathf.Abs(value - sliders[j].Value) < 10)
                    {
                        valid = false;
                        break;
                    }
                }

                attempts++;
            }
            while (!valid && attempts < 100);

            if (valid)
            {
                sliders[i].SetValue(value);
            }
            else
            {
                Debug.LogWarning(
                    "Rainfall: Could not generate a value with a 10-point minimum difference."
                );
                sliders[i].SetValue(value);
            }
        }
    }

    bool ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError("Rainfall: DiagnosisManager.Instance is null.");
            return false;
        }

        if (cluePopup == null)
        {
            Debug.LogError("Rainfall: CluePopup is not assigned.");
            return false;
        }

        string rainfallClue =
            DiagnosisManager.Instance.GetRequiredObservation("Rainfall");

        if (string.IsNullOrWhiteSpace(rainfallClue))
        {
            Debug.LogError(
                "Rainfall: No clue returned. Check the selected diagnosis rule " +
                "and when it is initialized."
            );
            return false;
        }

        DiagnosisManager.Instance.SetObservation("Rainfall", rainfallClue);
        cluePopup.ShowClue("Rainfall", rainfallClue);

        Debug.Log("Rainfall: Clue successfully retrieved and displayed: " + rainfallClue);

        return true;
    }

    public void ResetMinigame()
    {
        completedRounds = 0;
        hasWon = false;

        if (cluePopup != null)
            cluePopup.HideClue();

        GenerateNewRound();
    }
}