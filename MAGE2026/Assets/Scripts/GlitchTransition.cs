
using UnityEngine;
using DG.Tweening;
using KinoGlitch;

public class GlitchTransition : MonoBehaviour
{
    public DigitalGlitchController glitchController;
    public float duration = 0.3f;
    private Tween glitchTween;

    public void PlayGlitchTransition()
    {
        if (glitchController == null)
        {
            Debug.LogWarning("GlitchTransition: No glitch controller assigned.");
            return;
        }

        glitchTween?.Kill();
        glitchController.Intensity = 1f;

        glitchTween = DOTween.To(
            () => glitchController.Intensity,
            value => glitchController.Intensity = value,
            0f,
            duration
        ).SetEase(Ease.OutQuad);
    }

    void OnDestroy()
    {
        glitchTween?.Kill();
    }
}
