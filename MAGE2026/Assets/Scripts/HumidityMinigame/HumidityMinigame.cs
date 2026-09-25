using UnityEngine;

public class HumidityMinigame : MonoBehaviour
{
    [Header("Sliders")]
    public HumiditySlider[] sliders;

    [Header("Allowed Values")]
    public int[] allowedValues = { 35, 50, 80 };

    private int majorityValue;

    void Start()
    {
        GenerateNewRound();
    }

    public void SliderButtonPressed(HumiditySlider slider)
    {
        if (slider.IsLocked)
            return;

        int newValue = allowedValues[Random.Range(0, allowedValues.Length)];
        slider.SetValue(newValue);

        if (newValue == majorityValue)
        {
            slider.Lock();
        }

        CheckComplete();
    }

    void GenerateNewRound()
    {
        majorityValue = allowedValues[Random.Range(0, allowedValues.Length)];

        int[] majoritySlots = new int[8];

        for (int i = 0; i < majoritySlots.Length; i++)
        {
            majoritySlots[i] = i;
        }

        for (int i = majoritySlots.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = majoritySlots[i];
            majoritySlots[i] = majoritySlots[randomIndex];
            majoritySlots[randomIndex] = temp;
        }

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].Unlock();

            bool isMajority = false;

            for (int j = 0; j < 3; j++)
            {
                if (majoritySlots[j] == i)
                {
                    isMajority = true;
                    break;
                }
            }

            if (isMajority)
            {
                sliders[i].SetValue(majorityValue);
                sliders[i].Lock();
            }
            else
            {
                int startingValue;

                do
                {
                    startingValue = Random.Range(0, 301);
                }
                while (startingValue == majorityValue);

                sliders[i].SetValue(startingValue);
            }
        }
    }

    void CheckComplete()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (!sliders[i].IsLocked)
                return;
        }

        Debug.Log("All sliders matched!");

        GenerateNewRound();
    }
}