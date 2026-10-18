
using UnityEngine;
using DG.Tweening;
using KinoGlitch;

public class GlitchTransition : MonoBehaviour
{
    public DigitalGlitchController glitchController;
    public float duration = 0.3f;
    private Tween glitchTween;
    private AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayGlitchTransition()
    {
        audioManager.playGlitchSFX();
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
