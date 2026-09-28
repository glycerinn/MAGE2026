using UnityEngine;

public class CloudMinigame : MonoBehaviour, IMinigame
{
    [Header("Cloud Line")]
    public CloudLine cloudLine;

    public string MinigameName => "CloudMinigame";

    private int sortedClouds;
    private bool hasWon;

    public void CloudSorted()
    {
        if (hasWon)
            return;

        sortedClouds++;

        Debug.Log(
            "Cloud sorted: " +
            sortedClouds + "/" +
            cloudLine.TotalClouds
        );

        if (sortedClouds >= cloudLine.TotalClouds)
        {
            hasWon = true;

            Debug.Log("CLOUD MINIGAME WON!");

            StageManager.Instance.MinigameWon(MinigameName);
        }
    }

    public void ResetMinigame()
    {
        sortedClouds = 0;
        hasWon = false;

        cloudLine.ResetLine();
    }
}