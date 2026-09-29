using UnityEngine;

public class DiagnosisUI : MonoBehaviour
{
    public static DiagnosisUI Instance;

    [Header("Diagnosis Choices")]
    public DiagnosisChoice[] choices;

    private DiagnosisChoice selectedChoice;

    void Awake()
    {
        Instance = this;
    }

    public void SelectDiagnosis(DiagnosisChoice choice)
    {
        selectedChoice = choice;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] != null)
                choices[i].SetSelected(choices[i] == selectedChoice);
        }

        Debug.Log(
            "Selected diagnosis: " +
            selectedChoice.diagnosis
        );
    }

    public void SubmitDiagnosis()
    {
        if (selectedChoice == null)
        {
            Debug.LogWarning("No diagnosis selected.");
            return;
        }

        string diagnosis = selectedChoice.diagnosis;

        Debug.Log(
            "Submitting diagnosis: " +
            diagnosis
        );

        bool correct =
            DiagnosisManager.Instance.SubmitDiagnosis(diagnosis);

        if (correct)
        {
            StageManager.Instance.CompleteDiagnosis();
        }
        else
        {
            StageManager.Instance.RestartCurrentStage();
        }
    }

    public void ResetUI()
    {
        selectedChoice = null;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] != null)
                choices[i].SetSelected(false);
        }
    }
}