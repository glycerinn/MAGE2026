using TMPro;
using UnityEngine;

public class DiagnosisGuide : MonoBehaviour
{
    public static DiagnosisGuide Instance;

    [Header("3D Text")]
    public TMP_Text diagnosisText;
    public TMP_Text ruleText;

    [Header("Arrows")]
    public GameObject previousButton;
    public GameObject nextButton;

    private DiagnosisSO currentDiagnosisData;
    private int currentIndex;
    private bool ready;

    void Awake()
    {
        Instance = this;
        ready = false;
    }

    public void SetupForStage(int stage)
    {
        ready = false;
        currentIndex = 0;
        currentDiagnosisData = null;

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError(
                "DiagnosisGuide: DiagnosisManager.Instance is null."
            );
            return;
        }

        DiagnosisSO[] stageDiagnoses =
            DiagnosisManager.Instance.stageDiagnoses;

        int stageIndex = stage - 1;

        if (stageIndex < 0 ||
            stageIndex >= stageDiagnoses.Length)
        {
            Debug.LogError(
                "DiagnosisGuide: Invalid stage index: " +
                stage
            );
            return;
        }

        currentDiagnosisData =
            stageDiagnoses[stageIndex];

        if (currentDiagnosisData == null)
        {
            Debug.LogError(
                "DiagnosisGuide: No DiagnosisSO assigned for Stage " +
                stage
            );
            return;
        }

        if (currentDiagnosisData.rules == null ||
            currentDiagnosisData.rules.Count == 0)
        {
            Debug.LogError(
                "DiagnosisGuide: DiagnosisSO for Stage " +
                stage +
                " has no rules."
            );
            return;
        }

        ready = true;

        ShowCurrentDiagnosis();
    }

    public void NextDiagnosis()
    {
        if (!ready)
            return;

        if (currentDiagnosisData == null)
            return;

        if (currentIndex >=
            currentDiagnosisData.rules.Count - 1)
        {
            return;
        }

        currentIndex++;

        ShowCurrentDiagnosis();
    }

    public void PreviousDiagnosis()
    {
        if (!ready)
            return;

        if (currentDiagnosisData == null)
            return;

        if (currentIndex <= 0)
            return;

        currentIndex--;

        ShowCurrentDiagnosis();
    }

    void ShowCurrentDiagnosis()
    {
        if (currentDiagnosisData == null)
            return;

        if (currentDiagnosisData.rules == null ||
            currentDiagnosisData.rules.Count == 0)
        {
            return;
        }

        DiagnosisSO.DiagnosisRule rule =
            currentDiagnosisData.rules[currentIndex];

        if (diagnosisText != null)
        {
            diagnosisText.text =
                rule.diagnosis;
        }

        if (ruleText != null)
        {
            ruleText.text =
                BuildRuleText(rule);
        }

        UpdateButtons();

        Debug.Log(
            "Diagnosis Guide: Showing " +
            (currentIndex + 1) +
            "/" +
            currentDiagnosisData.rules.Count +
            " - " +
            rule.diagnosis
        );
    }

    string BuildRuleText(
        DiagnosisSO.DiagnosisRule rule)
    {
        if (rule.conditions == null ||
            rule.conditions.Count == 0)
        {
            return "No rules listed.";
        }

        string text = "";

        for (int i = 0;
            i < rule.conditions.Count;
            i++)
        {
            DiagnosisSO.Condition condition =
                rule.conditions[i];

            if (i > 0)
                text += "\n";

            text +=
                GetReadableSource(condition.source) +
                ": " +
                condition.value;
        }

        return text;
    }

    string GetReadableSource(string source)
    {
        switch (source)
        {
            case "Cloud":
                return "Cloud";

            case "Satellite":
                return "Satellite";

            case "Humidity":
                return "Humidity";

            case "Rainfall":
                return "Rainfall";

            case "Headline":
                return "Headline";

            default:
                return source;
        }
    }

    void UpdateButtons()
    {
        if (previousButton != null)
        {
            previousButton.SetActive(
                currentIndex > 0
            );
        }

        if (nextButton != null)
        {
            nextButton.SetActive(
                currentIndex <
                currentDiagnosisData.rules.Count - 1
            );
        }
    }

    public void ResetGuide()
    {
        ready = false;
        currentDiagnosisData = null;
        currentIndex = 0;

        if (diagnosisText != null)
            diagnosisText.text = "";

        if (ruleText != null)
            ruleText.text = "";

        if (previousButton != null)
            previousButton.SetActive(false);

        if (nextButton != null)
            nextButton.SetActive(false);
    }
}