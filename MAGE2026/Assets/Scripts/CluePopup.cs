using TMPro;
using UnityEngine;

public class CluePopup : MonoBehaviour
{
    public TMP_Text clueText;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowClue(string source, string value)
    {
        if (clueText == null)
        {
            Debug.LogError("CluePopup: Clue Text is not assigned.");
            return;
        }

        clueText.text = GetClueMessage(source, value);
        gameObject.SetActive(true);

        Debug.Log("CLUE SHOWN: " + clueText.text);
    }

    string GetClueMessage(string source, string value)
    {
        switch (source)
        {
            case "Cloud":
                return "Cloud Observation: " + value;
            case "Satellite":
                return "Satellite Imagery: " + value;

            case "Humidity":
                return "Humidity: " + value + "%";

            case "Rainfall":
                return "Rainfall: " + value + "%";

            default:
                return source + ": " + value;
        }
    }

    public void HideClue()
    {
        gameObject.SetActive(false);
    }
}