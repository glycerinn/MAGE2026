using UnityEngine;

public class HumidityButton : MonoBehaviour
{
    public HumiditySlider slider;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void OnMouseDown()
    {
        if (slider == null || slider.IsLocked)
            return;

        slider.ButtonPressed();
    }

    void Update()
    {
        if (slider == null || spriteRenderer == null)
            return;

        if (slider.IsLocked)
        {
            Color color = originalColor;
            color.a = 0.3f;
            spriteRenderer.color = color;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }
}