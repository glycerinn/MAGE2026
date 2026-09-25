using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public RectTransform[] buttons;
    public float slideDuration = 0.6f;
    public float delayDuration = 0.2f;

    private void Start()
    {
        slideIn();
    }

    void slideIn()
    {
        for(int i = 0; i<buttons.Length; i++)
        {
            RectTransform btns = buttons[i];
            Vector2 target = btns.anchoredPosition;
            btns.anchoredPosition = new Vector2(-500f, target.y);

            btns.DOAnchorPos(target, slideDuration).SetEase(Ease.OutBack).SetDelay(i*delayDuration);
        }
    }
}
