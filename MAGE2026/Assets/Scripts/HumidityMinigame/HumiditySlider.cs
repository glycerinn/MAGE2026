using TMPro;
using UnityEngine;

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

    private HumidityMinigame minigame;

    void Awake()
    {
        minigame = GetComponentInParent<HumidityMinigame>();
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

    public void ButtonPressed()
    {
        if (IsLocked)
            return;

        minigame.SliderButtonPressed(this);
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