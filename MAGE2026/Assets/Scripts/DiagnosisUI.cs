using UnityEngine;

public class DiagnosisUI : MonoBehaviour
{
    public static DiagnosisUI Instance;

    [Header("Diagnosis Choices")]
    public DiagnosisChoice[] choices;

    [Header("Choices Per Stage")]
    public int[] choicesPerStage = { 3, 5, 6, 6 };

    private DiagnosisChoice selectedChoice;

    void Awake()
    {
        Instance = this;
    }

    public void SetupForStage(int stage)
    {
        selectedChoice = null;

        Debug.Log(
            "Stage received: " +
            stage
        );

        int stageIndex = stage - 1;

        if (stageIndex < 0 || stageIndex >= choicesPerStage.Length)
        {
            return;
        }

        int activeChoiceCount = choicesPerStage[stageIndex];

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == null)
            {
                continue;
            }

            bool shouldBeActive =
                i < activeChoiceCount;

            choices[i].SetSelected(false);
            choices[i].SetAvailable(shouldBeActive);
        }

        Debug.Log(
            "========== DIAGNOSIS SETUP COMPLETE =========="
        );
    }

    public void SelectDiagnosis(DiagnosisChoice choice)
    {
        if (choice == null)
            return;

        selectedChoice = choice;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] != null)
                choices[i].SetSelected(
                    choices[i] == selectedChoice
                );
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
            Debug.LogWarning(
                "No diagnosis selected."
            );
            return;
        }

        string diagnosis =
            selectedChoice.diagnosis;

        Debug.Log(
            "Submitting diagnosis: " +
            diagnosis
        );

        bool correct =
            DiagnosisManager.Instance.SubmitDiagnosis(
                diagnosis
            );

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