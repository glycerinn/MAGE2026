using UnityEngine;

public class DiagnosisSubmit : MonoBehaviour
{
    void OnMouseDown()
    {
        if (DiagnosisUI.Instance == null)
        {
            Debug.LogError(
                "DiagnosisUI.Instance is null."
            );

            return;
        }

        DiagnosisUI.Instance.SubmitDiagnosis();
    }
}