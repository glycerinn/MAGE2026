using System.Collections.Generic;
using UnityEngine;

public class DiagnosisManager : MonoBehaviour
{
    public static DiagnosisManager Instance;

    [Header("Stage Diagnosis Data")]
    public DiagnosisSO[] stageDiagnoses;

    private DiagnosisSO currentDiagnosisData;

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

        int index = stage - 1;

        if (index < 0 || index >= stageDiagnoses.Length)
        {
            Debug.LogError("No DiagnosisSO found for Stage " + stage);
            currentDiagnosisData = null;
            return;
        }

        currentDiagnosisData = stageDiagnoses[index];

        Debug.Log(
            "Loaded diagnosis data for Stage " +
            stage +
            ": " +
            currentDiagnosisData.name
        );
    }

    public void SetObservation(string source, string value)
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
        if (observations.TryGetValue(source, out string value))
            return value;

        return "";
    }

    public string GetRequiredObservation(string source)
    {
        if (currentDiagnosisData == null)
        {
            Debug.LogError("No DiagnosisSO is loaded.");
            return "";
        }

        return currentDiagnosisData.GetRequiredObservation(source);
    }

    public bool SubmitDiagnosis(string diagnosis)
    {
        if (currentDiagnosisData == null)
        {
            Debug.LogError("No DiagnosisSO is loaded.");
            return false;
        }

        string correctDiagnosis =
            currentDiagnosisData.GetDiagnosis(observations);

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
    }
}