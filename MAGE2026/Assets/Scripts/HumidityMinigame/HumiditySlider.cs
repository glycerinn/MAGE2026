using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HumiditySlider : MonoBehaviour
{
    [Header("Fill")]
    public Transform fill;

    [Header("Percentage Text")]
    public TMP_Text percentageText;

    [Header("Value Range")]
    public float minValue = 0f;
    public float maxValue = 300f;
    public int Value { get; private set; }
    public bool IsLocked { get; private set; }
    public bool IsLoading { get; private set; }

    [Header("Fill Padding")]
    public float leftPadding = 0.005f;
    public float rightPadding = 0.005f;
    private float originalFillWidth;

    [Header("Loading Animation")]
    public float shakeDistance = 0.1f;
    public float shakeSpeed = 5f;
    private Vector3 originalFillScale;
    private Vector3 originalFillPosition;
    private Tween fillTween;
    private Tween delayTween;
    private HumidityMinigame minigame;

    // HAPUS: private AudioManager audioManager;

    void Awake()
    {
        // HAPUS: audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        minigame = GetComponentInParent<HumidityMinigame>();
        if (fill != null)
        {
            originalFillScale = fill.localScale;
            originalFillPosition = fill.localPosition;
            originalFillWidth = originalFillScale.x;
        }
    }

    // ... (Fungsi SetValue, Lock, Unlock tetap sama persis) ...

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, (int)minValue, (int)maxValue);
        float normalized = Mathf.InverseLerp(minValue, maxValue, Value);

        if (fill != null)
        {
            float usableWidth = Mathf.Max(0f, originalFillWidth - leftPadding - rightPadding);
            float fillWidth = usableWidth * normalized;

            Vector3 scale = originalFillScale;
            scale.x = fillWidth;
            fill.localScale = scale;

            Vector3 position = originalFillPosition;
            position.x = originalFillPosition.x - originalFillWidth / 2f + leftPadding + fillWidth / 2f;
            fill.localPosition = position;
        }

        if (percentageText != null)
            percentageText.text = Value + "%";
    }

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }


    public void PlayLoadingPulse(float duration, Action onComplete)
    {
        // HAPUS: audioManager.playFix2SFX();
        
        if (IsLoading || fill == null)
            return;

        IsLoading = true;
        fillTween?.Kill();
        delayTween?.Kill();

        float startValue = minValue;
        float endValue = maxValue;
        float animatedValue = startValue;
        SetValue((int)startValue);

        fillTween = DOTween.To(() => animatedValue, value =>
        {
            animatedValue = value;
            SetValue(Mathf.RoundToInt(value));
        }, endValue, duration / 2f)
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);

        delayTween = DOVirtual.DelayedCall(duration, () =>
        {
            fillTween?.Kill();
            fillTween = null;
            delayTween = null;
            IsLoading = false;
            SetValue(Value);
            onComplete?.Invoke();
        });
    }

    // ... (Fungsi CancelLoadingPulse, ButtonPressed, dll. tetap sama persis) ...

    public void CancelLoadingPulse()
    {
        fillTween?.Kill();
        delayTween?.Kill();
        fillTween = null;
        delayTween = null;
        IsLoading = false;
        SetValue(Value);
    }

    public void ButtonPressed()
    {
        if (IsLocked || IsLoading || minigame == null)
            return;

        minigame.SliderButtonPressed(this);
    }

    void OnDisable()
    {
        CancelLoadingPulse();
    }

    void OnDestroy()
    {
        fillTween?.Kill();
        delayTween?.Kill();
    }
}