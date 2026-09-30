using System.Collections.Generic;
using UnityEngine;

public class DiagnosisManager : MonoBehaviour
{
    public static DiagnosisManager Instance;

    [Header("Stage Diagnosis Data")]
    public DiagnosisSO[] stageDiagnoses;

    private DiagnosisSO currentDiagnosisData;

    private DiagnosisSO.DiagnosisRule currentRule;

    private Dictionary<string, string> observations =
        new Dictionary<string, string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartStageDiagnosis(int stage)
    {
        observations.Clear();
        currentRule = null;

        int index = stage - 1;

        if (index < 0 || index >= stageDiagnoses.Length)
        {
            Debug.LogError(
                "No DiagnosisSO found for Stage " +
                stage
            );

            currentDiagnosisData = null;
            return;
        }

        currentDiagnosisData = stageDiagnoses[index];

        if (currentDiagnosisData.rules == null ||
            currentDiagnosisData.rules.Count == 0)
        {
            Debug.LogError(
                "DiagnosisSO for Stage " +
                stage +
                " has no diagnosis rules."
            );

            return;
        }

        currentRule =
            currentDiagnosisData.rules[
                Random.Range(
                    0,
                    currentDiagnosisData.rules.Count
                )
            ];

        Debug.Log(
            "Selected diagnosis for Stage " +
            stage +
            ": " +
            currentRule.diagnosis
        );

        for (int i = 0;
            i < currentRule.conditions.Count;
            i++)
        {
            DiagnosisSO.Condition condition =
                currentRule.conditions[i];

            Debug.Log(
                "Selected clue: " +
                condition.source +
                " = " +
                condition.value
            );
        }
    }

    public void SetObservation(
        string source,
        string value)
    {
        observations[source] = value;

        Debug.Log(
            "Diagnosis observation: " +
            source +
            " = " +
            value
        );
    }

    public string GetObservation(string source)
    {
        if (observations.TryGetValue(
            source,
            out string value))
        {
            return value;
        }

        return "";
    }

    public string GetRequiredObservation(string source)
    {
        if (currentRule == null)
        {
            Debug.LogError(
                "No diagnosis rule has been selected."
            );

            return "";
        }

        for (int i = 0;
            i < currentRule.conditions.Count;
            i++)
        {
            DiagnosisSO.Condition condition =
                currentRule.conditions[i];

            if (condition.source == source)
                return condition.value;
        }

        Debug.LogWarning(
            "No clue for source: " +
            source
        );

        return "";
    }

    public bool SubmitDiagnosis(string diagnosis)
    {
        if (currentRule == null)
        {
            Debug.LogError(
                "No diagnosis rule has been selected."
            );

            return false;
        }

        string correctDiagnosis =
            currentRule.diagnosis;

        Debug.Log(
            "Player diagnosis: " +
            diagnosis +
            " | Correct diagnosis: " +
            correctDiagnosis
        );

        if (diagnosis == correctDiagnosis)
        {
            Debug.Log("DIAGNOSIS CORRECT!");
            return true;
        }

        Debug.Log("DIAGNOSIS WRONG!");
        return false;
    }

    public void ResetDiagnosis()
    {
        observations.Clear();
        currentRule = null;
    }
}