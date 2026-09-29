using UnityEngine;

public class DiagnosisSubmit : MonoBehaviour
{
    void OnMouseDown()
    {
        DiagnosisUI.Instance.SubmitDiagnosis();
    }
}