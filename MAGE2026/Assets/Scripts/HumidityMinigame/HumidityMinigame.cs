using UnityEngine;

public class HumidityMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "HumidityMinigame";

    [Header("Sliders")]
    public HumiditySlider[] sliders;

    [Header("Allowed Values")]
    public int[] allowedValues = { 35, 50, 80 };

    [Header("Win Condition")]
    public int winAmount = 3;

    private int majorityValue;
    private int completedRounds;

    void Start()
    {
        ResetMinigame();
    }

    public void SliderButtonPressed(HumiditySlider slider)
    {
        if (slider.IsLocked)
            return;

        int newValue = allowedValues[Random.Range(0, allowedValues.Length)];

        slider.SetValue(newValue);

        if (newValue == majorityValue)
        {
            slider.Lock();
        }

        CheckComplete();
    }

    void GenerateNewRound()
    {
        majorityValue =
            allowedValues[Random.Range(0, allowedValues.Length)];

        int[] majoritySlots = new int[sliders.Length];

        for (int i = 0; i < majoritySlots.Length; i++)
        {
            majoritySlots[i] = i;
        }

        for (int i = majoritySlots.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            int temp = majoritySlots[i];
            majoritySlots[i] = majoritySlots[randomIndex];
            majoritySlots[randomIndex] = temp;
        }

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].Unlock();

            bool isMajority = false;

            for (int j = 0; j < 3 && j < majoritySlots.Length; j++)
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
            }
            else
            {
                int startingValue;

                do
                {
                    startingValue = Random.Range(0, 301);
                }
                while (startingValue == majorityValue);

                sliders[i].SetValue(startingValue);
            }
        }
    }

    void CheckComplete()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (!sliders[i].IsLocked)
                return;
        }

        completedRounds++;

        Debug.Log(
            "Humidity round complete! " +
            completedRounds + "/" + winAmount
        );

        if (completedRounds >= winAmount)
        {
            ReportDiagnosisClue();

            Debug.Log("HUMIDITY MINIGAME WON!");

            StageManager.Instance.MinigameWon(MinigameName);
            return;
        }

        GenerateNewRound();
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
            return;

        string humidityClue =
            DiagnosisManager.Instance.GetRequiredObservation("Humidity");

        if (string.IsNullOrEmpty(humidityClue))
        {
            Debug.LogError("No Humidity diagnosis clue found.");
            return;
        }

        DiagnosisManager.Instance.SetObservation(
            "Humidity",
            humidityClue
        );
    }

    public void ResetMinigame()
    {
        completedRounds = 0;
        majorityValue = 0;

        GenerateNewRound();
    }
}