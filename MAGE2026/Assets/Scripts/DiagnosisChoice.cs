using UnityEngine;

public class DiagnosisChoice : MonoBehaviour
{
    [Header("Diagnosis")]
    public string diagnosis;

    private bool selected;
    private bool available;

    public void SetAvailable(bool value)
    {
        available = value;
        gameObject.SetActive(value);
    }

    void OnMouseDown()
    {
        if (!available)
        {
            return;
        }

        Select();
    }

    public void Select()
    {
        if (!available)
        {
            return;
        }

        if (DiagnosisUI.Instance == null)
        {
            Debug.LogError("DiagnosisUI.Instance is null.");
            return;
        }

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