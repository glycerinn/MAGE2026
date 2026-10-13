using TMPro;
using UnityEngine;
using DG.Tweening;

public class CluePopup : MonoBehaviour
{
    public TMP_Text clueText;
    [Header("Popup Animation")]
    public float popDuration = 0.35f;
    public Ease popEase = Ease.OutBack;
    private Transform popupTransform;
    private Vector3 originalScale;
    private Tween popTween;

    void Awake()
    {
        popupTransform = transform;
        originalScale = popupTransform.localScale;
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
        popTween?.Kill();
        gameObject.SetActive(true);
        popupTransform.localScale = Vector3.zero;
        popTween = popupTransform.DOScale(originalScale, popDuration).SetEase(popEase);
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
                return "Humidity: " + value;
            case "Rainfall":
                return "Rainfall: " + value;
            default:
                return source + ": " + value;
        }
    }

    public void HideClue()
    {
        popTween?.Kill();
        popTween = null;
        gameObject.SetActive(false);
        popupTransform.localScale = originalScale;
    }

    void OnDestroy()
    {
        popTween?.Kill();
    }
}