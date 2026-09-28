using UnityEngine;

public class SliderMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "SliderMinigame";

    [Header("Sliders")]
    public ObjectSlider[] sliders;

    [Header("Win Condition")]
    public int winAmount = 3;

    private int completedRounds;
    private bool hasWon;

    void Start()
    {
        ResetMinigame();
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

                StageManager.Instance.MinigameWon(MinigameName);
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

    public void ResetMinigame()
    {
        completedRounds = 0;
        hasWon = false;

        GenerateNewRound();
    }
}