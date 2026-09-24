using UnityEngine;

public class SliderMinigame : MonoBehaviour
{
    [Header("Sliders")]
    public ObjectSlider[] sliders;

    void Start()
    {
        GenerateNewRound();
    }

    public void SelectSlider(ObjectSlider selectedSlider)
    {
        int[] values = new int[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            values[i] = sliders[i].Value;
        }

        System.Array.Sort(values);

        int medianValue = values[1];

        if (selectedSlider.Value == medianValue)
        {
            Debug.Log("Correct!");
            GenerateNewRound();
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

    void GenerateNewRound()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].SetValue(Random.Range(0, 101));
        }
    }
}