using UnityEngine;

public class DiagnosisChoice : MonoBehaviour
{
    public string diagnosis;

    private bool selected;

    void OnMouseDown()
    {
        Select();
    }

    public void Select()
    {
        DiagnosisUI.Instance.SelectDiagnosis(this);
    }

    public void SetSelected(bool value)
    {
        selected = value;

        Debug.Log(
            diagnosis +
            " selected: " +
            selected
        );
    }
}