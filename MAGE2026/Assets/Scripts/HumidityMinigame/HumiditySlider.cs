using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HumiditySlider : MonoBehaviour
{
    [Header("Handle")]
    public Transform handle;

    [Header("Percentage Text")]
    public TMP_Text percentageText;

    [Header("Slider Range")]
    public float leftX = -2f;
    public float rightX = 2f;

    [Header("Value Range")]
    public float minValue = 0f;
    public float maxValue = 300f;

    public int Value { get; private set; }
    public bool IsLocked { get; private set; }
    public bool IsLoading { get; private set; }

    [Header("Loading Shake")]
    public float shakeDistance = 0.1f;
    public float shakeSpeed = 5f;

    private Vector3 originalPosition;
    private Tween shakeTween;
    private Tween delayTween;

    private HumidityMinigame minigame;

    void Awake()
    {
        minigame = GetComponentInParent<HumidityMinigame>();
        originalPosition = transform.localPosition;
    }

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, (int)minValue, (int)maxValue);
        float normalized = Mathf.InverseLerp(minValue, maxValue, Value);
        float x = Mathf.Lerp(leftX, rightX, normalized);
        Vector3 position = handle.localPosition;
        position.x = x;
        handle.localPosition = position;
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
        if (IsLoading || handle == null)
            return;
        IsLoading = true;
        shakeTween?.Kill();
        delayTween?.Kill();
        float startX = leftX;
        float endX = rightX;
        Vector3 position = handle.localPosition;
        position.x = startX;
        handle.localPosition = position;
        float halfDuration = duration / 2f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(handle.DOLocalMoveX(endX, halfDuration).SetEase(Ease.InOutSine));
        sequence.Append(handle.DOLocalMoveX(startX, halfDuration).SetEase(Ease.InOutSine));
        sequence.SetLoops(-1, LoopType.Restart);
        shakeTween = sequence;
        delayTween = DOVirtual.DelayedCall(duration, () =>
        {
            shakeTween?.Kill();
            shakeTween = null;
            delayTween = null;
            if (handle != null)
                SetValue(Value);
            IsLoading = false;
            onComplete?.Invoke();
        });
    }

    public void CancelLoadingPulse()
    {
        shakeTween?.Kill();
        delayTween?.Kill();
        shakeTween = null;
        delayTween = null;
        IsLoading = false;
        if (handle != null)
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
        shakeTween?.Kill();
        delayTween?.Kill();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 left = transform.TransformPoint(new Vector3(leftX, 0f, 0f));
        Vector3 right = transform.TransformPoint(new Vector3(rightX, 0f, 0f));
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(left, 0.08f);
        Gizmos.DrawSphere(right, 0.08f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(left, right);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(left + transform.up * -0.15f, left + transform.up * 0.15f);
        Gizmos.DrawLine(right + transform.up * -0.15f, right + transform.up * 0.15f);
    }
}