using UnityEngine;

public class DiagnosisUI : MonoBehaviour
{
    public static DiagnosisUI Instance;

    [Header("Diagnosis Choices")]
    public DiagnosisChoice[] choices;

    [Header("Choices Per Stage")]
    public int[] choicesPerStage =
    {
        3,
        5,
        6,
        6,
        6
    };

    private DiagnosisChoice selectedChoice;
    private bool ready;

    void Awake()
    {
        Instance = this;
        ready = false;
    }

    public void SetupForStage(int stage)
    {
        selectedChoice = null;
        ready = false;

        Debug.Log(
            "Setting up diagnosis for Stage " +
            stage
        );

        int stageIndex =
            stage - 1;

        if (stageIndex < 0 ||
            stageIndex >= choicesPerStage.Length)
        {
            Debug.LogError(
                "Invalid diagnosis stage: " +
                stage
            );

            return;
        }

        int activeChoiceCount =
            choicesPerStage[stageIndex];

        for (int i = 0;
            i < choices.Length;
            i++)
        {
            if (choices[i] == null)
                continue;

            choices[i].SetSelected(false);

            bool shouldBeActive =
                i < activeChoiceCount;

            choices[i].SetAvailable(
                shouldBeActive
            );
        }

        if (HeadlineClue.Instance != null)
        {
            HeadlineClue.Instance.ShowForStage(
                stage
            );
        }

        if (DiagnosisGuide.Instance != null)
        {
            DiagnosisGuide.Instance.SetupForStage(
                stage
            );
        }

        ready = true;

        Debug.Log(
            "========== DIAGNOSIS SETUP COMPLETE =========="
        );
    }

    public void SelectDiagnosis(
        DiagnosisChoice choice)
    {
        if (!ready)
        {
            Debug.LogWarning(
                "Diagnosis UI is not ready yet."
            );

            return;
        }

        if (choice == null)
            return;

        selectedChoice = choice;

        for (int i = 0;
            i < choices.Length;
            i++)
        {
            if (choices[i] != null)
            {
                choices[i].SetSelected(
                    choices[i] == selectedChoice
                );
            }
        }

        Debug.Log(
            "Selected diagnosis: " +
            selectedChoice.diagnosis
        );
    }

    public void SubmitDiagnosis()
    {
        if (!ready)
        {
            Debug.LogWarning(
                "Diagnosis UI is not ready yet."
            );

            return;
        }

        if (selectedChoice == null)
        {
            Debug.LogWarning(
                "No diagnosis selected."
            );

            return;
        }

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError(
                "DiagnosisManager.Instance is null."
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

        ready = false;

        if (correct)
        {
            Debug.Log(
                "Diagnosis correct."
            );

            StageManager.Instance.CompleteDiagnosis(
                true
            );
        }
        else
        {
            Debug.Log(
                "Diagnosis incorrect."
            );

            StageManager.Instance.CompleteDiagnosis(
                false
            );
        }
    }

    public void ResetUI()
    {
        selectedChoice = null;
        ready = false;

        for (int i = 0;
            i < choices.Length;
            i++)
        {
            if (choices[i] != null)
                choices[i].SetSelected(false);
        }

        if (HeadlineClue.Instance != null)
            HeadlineClue.Instance.Hide();

        if (DiagnosisGuide.Instance != null)
            DiagnosisGuide.Instance.ResetGuide();
    }
}