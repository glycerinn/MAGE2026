using UnityEngine;

public class SliderMinigame : MonoBehaviour
{
    [Header("Sliders")]
    public ObjectSlider[] sliders;

    [Header("Win Condition")]
    public int winAmount = 3;

    private int completedRounds;
    private bool hasWon;

    void Start()
    {
        completedRounds = 0;
        hasWon = false;

        GenerateNewRound();
    }

    public void SelectSlider(ObjectSlider selectedSlider)
    {
        if (hasWon)
            return;

        int[] values = new int[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            values[i] = sliders[i].Value;
        }

        System.Array.Sort(values);

        int medianValue = values[1];

        if (selectedSlider.Value == medianValue)
        {
            completedRounds++;

            Debug.Log(
                "Slider round complete! " +
                completedRounds + "/" + winAmount
            );

            if (completedRounds >= winAmount)
            {
                hasWon = true;

                Debug.Log("SLIDER MINIGAME WON!");

                StageManager.Instance.MinigameWon("Slider");
                return;
            }

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