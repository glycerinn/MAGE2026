using UnityEngine;

public class SliderMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "SliderMinigame";

    [Header("Sliders")]
    public ObjectSlider[] sliders;

    [Header("Win Condition")]
    public int winAmount = 3;

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

        int[] values = new int[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            values[i] = sliders[i].Value;
        }

        System.Array.Sort(values);

        int medianIndex = values.Length / 2;
        int medianValue = values[medianIndex];

        if (selectedSlider.Value == medianValue)
        {
            completedRounds++;

            Debug.Log(
                "Rainfall round complete! " +
                completedRounds + "/" + winAmount
            );

            if (completedRounds >= winAmount)
            {
                hasWon = true;

                ReportDiagnosisClue();

                Debug.Log("RAINFALL MINIGAME WON!");

                StageManager.Instance.MinigameWon(MinigameName);
                return;
            }

            GenerateNewRound();
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

    void GenerateNewRound()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].SetValue(Random.Range(0, 101));
        }
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
            return;

        string rainfallClue =
            DiagnosisManager.Instance.GetRequiredObservation("Rainfall");

        if (string.IsNullOrEmpty(rainfallClue))
        {
            Debug.LogError("No Rainfall diagnosis clue found.");
            return;
        }

        DiagnosisManager.Instance.SetObservation(
            "Rainfall",
            rainfallClue
        );
    }

    public void ResetMinigame()
    {
        completedRounds = 0;
        hasWon = false;

        GenerateNewRound();
    }
}