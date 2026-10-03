using UnityEngine;

public class HumidityMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "HumidityMinigame";

    [Header("Sliders")]
    public HumiditySlider[] sliders;

    [Header("Allowed Values")]
    public int[] allowedValues = { 35, 50, 80 };

    [Header("Clue")]
    public CluePopup cluePopup;

    private int majorityValue;
    private bool hasWon;

    public void StartHumidityMinigame()
    {
        GenerateNewRound();
    }

    public void SliderButtonPressed(HumiditySlider slider)
    {
        if (hasWon)
            return;

        if (slider == null || slider.IsLocked)
            return;

        int newValue =
            allowedValues[
                Random.Range(0, allowedValues.Length)
            ];

        slider.SetValue(newValue);

        if (newValue == majorityValue)
            slider.Lock();

        CheckComplete();
    }

    void GenerateNewRound()
    {
        hasWon = false;

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError(
                "HumidityMinigame: DiagnosisManager.Instance is null."
            );

            return;
        }

        string requiredHumidity =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Humidity"
            );

        if (string.IsNullOrEmpty(requiredHumidity))
        {
            Debug.LogError(
                "HumidityMinigame: No Humidity diagnosis clue was selected."
            );

            return;
        }

        string cleanHumidity = requiredHumidity.Replace("%", "").Trim();

        if (!int.TryParse(
            cleanHumidity,
            out majorityValue))
        {
            Debug.LogError(
                "HumidityMinigame: Could not parse Humidity clue: " +
                requiredHumidity
            );

            return;
        }

        Debug.Log(
            "Humidity minigame using selected diagnosis humidity: " +
            majorityValue
        );

        if (sliders == null || sliders.Length == 0)
        {
            Debug.LogError(
                "HumidityMinigame: No sliders assigned."
            );

            return;
        }

        int[] majoritySlots =
            new int[sliders.Length];

        for (int i = 0; i < majoritySlots.Length; i++)
            majoritySlots[i] = i;

        for (int i = majoritySlots.Length - 1; i > 0; i--)
        {
            int randomIndex =
                Random.Range(0, i + 1);

            int temp = majoritySlots[i];
            majoritySlots[i] = majoritySlots[randomIndex];
            majoritySlots[randomIndex] = temp;
        }

        int majorityCount =
            Mathf.Min(3, sliders.Length);

        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null)
                continue;

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
            }
            else
            {
                int startingValue;

                do
                {
                    startingValue =
                        Random.Range(0, 301);
                }
                while (startingValue == majorityValue);

                sliders[i].SetValue(startingValue);
            }
        }

        Debug.Log(
            "Humidity round generated. " +
            "Majority value: " +
            majorityValue
        );
    }

    void CheckComplete()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == null)
                continue;

            if (!sliders[i].IsLocked)
                return;
        }

        hasWon = true;

        ReportDiagnosisClue();

        Debug.Log(
            "HUMIDITY MINIGAME WON! " +
            "Correct humidity: " +
            majorityValue
        );

        StageManager.Instance.MinigameWon(
            MinigameName
        );
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
            return;

        string humidityClue =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Humidity"
            );

        if (string.IsNullOrEmpty(humidityClue))
        {
            Debug.LogError(
                "No Humidity diagnosis clue found."
            );

            return;
        }

        DiagnosisManager.Instance.SetObservation(
            "Humidity",
            humidityClue
        );

        if (cluePopup != null)
        {
            cluePopup.ShowClue(
                "Humidity",
                humidityClue
            );
        }
    }

    public void ResetMinigame()
    {
        hasWon = false;
        majorityValue = 0;

        if (cluePopup != null)
            cluePopup.HideClue();

        GenerateNewRound();
    }
}