using UnityEngine;

public class SliderButton : MonoBehaviour
{
    public ObjectSlider slider;

    void OnMouseDown()
    {
        slider.Select();
    }
}