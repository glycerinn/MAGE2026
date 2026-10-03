using TMPro;
using UnityEngine;

public class HeadlineClue : MonoBehaviour
{
    public static HeadlineClue Instance;

    [Header("Text")]
    public TMP_Text clueText;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowForStage(int stage)
    {
        if (stage != 5)
        {
            Hide();
            return;
        }

        if (clueText == null)
        {
            Debug.LogError(
                "HeadlineClue: Clue Text is not assigned."
            );

            return;
        }

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError(
                "HeadlineClue: DiagnosisManager.Instance is null."
            );

            return;
        }

        string headlineValue =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Headline"
            );

        if (string.IsNullOrEmpty(headlineValue))
        {
            Debug.LogError(
                "HeadlineClue: No Headline observation was found in the selected diagnosis rule."
            );

            return;
        }

        clueText.text = headlineValue;

        DiagnosisManager.Instance.SetObservation(
            "Headline",
            headlineValue
        );

        gameObject.SetActive(true);

        Debug.Log(
            "Stage 5 headline shown: " +
            headlineValue
        );
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}