using UnityEngine;

public class SliderButton : MonoBehaviour
{
    public ObjectSlider slider;
    private AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void OnMouseDown()
    {
        audioManager.playClickSFX();
        slider.Select();
    }
}