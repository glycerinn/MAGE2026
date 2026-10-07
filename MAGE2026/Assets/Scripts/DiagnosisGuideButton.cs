using UnityEngine;

public class DiagnosisGuideButton : MonoBehaviour
{
    public enum ButtonType
    {
        Previous,
        Next
    }

    public ButtonType buttonType;

    void OnMouseDown()
    {
        if (DiagnosisGuide.Instance == null)
            return;

        if (buttonType == ButtonType.Previous)
        {
            DiagnosisGuide.Instance.PreviousDiagnosis();
        }
        else
        {
            DiagnosisGuide.Instance.NextDiagnosis();
        }
    }
}