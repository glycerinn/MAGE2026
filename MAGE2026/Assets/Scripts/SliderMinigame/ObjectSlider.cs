using UnityEngine;

public class ObjectSlider : MonoBehaviour
{
    [Header("Handle")]
    public Transform handle;

    [Header("Slider Range")]
    public float bottomY = -2f;
    public float topY = 2f;

    public int Value { get; private set; }

    private SliderMinigame minigame;

    void Awake()
    {
        minigame = GetComponentInParent<SliderMinigame>();
    }

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, 0, 100);

        float normalized = Value / 100f;
        float y = Mathf.Lerp(bottomY, topY, normalized);

        Vector3 position = handle.localPosition;
        position.y = y;
        handle.localPosition = position;
    }

    public void Select()
    {
        minigame.SelectSlider(this);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 bottom = transform.position;
        bottom.y += bottomY;

        Vector3 top = transform.position;
        top.y += topY;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bottom, 0.08f);
        Gizmos.DrawSphere(top, 0.08f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottom, top);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(bottom + Vector3.left * 0.15f, bottom + Vector3.right * 0.15f);
        Gizmos.DrawLine(top + Vector3.left * 0.15f, top + Vector3.right * 0.15f);
    }
}