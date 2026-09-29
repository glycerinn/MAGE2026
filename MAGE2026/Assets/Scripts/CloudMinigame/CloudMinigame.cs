using System.Collections.Generic;
using UnityEngine;

public class CloudMinigame : MonoBehaviour, IMinigame
{
    [Header("Cloud Lines")]
    public CloudLine[] cloudLines;

    public string MinigameName => "CloudMinigame";

    private int sortedClouds;
    private int totalClouds;
    private bool hasWon;

    void Start()
    {
        CalculateTotalClouds();
    }

    void CalculateTotalClouds()
    {
        totalClouds = 0;

        for (int i = 0; i < cloudLines.Length; i++)
        {
            if (cloudLines[i] != null)
                totalClouds += cloudLines[i].TotalClouds;
        }
    }

    public void CloudSorted()
    {
        if (hasWon)
            return;

        sortedClouds++;

        Debug.Log(
            "Cloud sorted: " +
            sortedClouds + "/" +
            totalClouds
        );

        if (sortedClouds >= totalClouds)
        {
            hasWon = true;

            Debug.Log("CLOUD MINIGAME WON!");

            DetermineCloudObservation();

            StageManager.Instance.MinigameWon(MinigameName);
        }
    }

    void DetermineCloudObservation()
    {
        Dictionary<CloudType, int> cloudCounts =
            new Dictionary<CloudType, int>();

        foreach (CloudType type in System.Enum.GetValues(typeof(CloudType)))
        {
            cloudCounts[type] = 0;
        }

        for (int i = 0; i < cloudLines.Length; i++)
        {
            if (cloudLines[i] == null)
                continue;

            Dictionary<CloudType, int> lineCounts =
                cloudLines[i].GetCloudCounts();

            foreach (KeyValuePair<CloudType, int> pair in lineCounts)
            {
                cloudCounts[pair.Key] += pair.Value;
            }
        }

        CloudType mostCommonType = CloudType.Cumulus;
        int highestCount = -1;

        foreach (KeyValuePair<CloudType, int> pair in cloudCounts)
        {
            Debug.Log(
                "Total " +
                pair.Key +
                ": " +
                pair.Value
            );

            if (pair.Value > highestCount)
            {
                highestCount = pair.Value;
                mostCommonType = pair.Key;
            }
        }

        Debug.Log(
            "MOST COMMON CLOUD: " +
            mostCommonType +
            " (" +
            highestCount +
            ")"
        );

        if (DiagnosisManager.Instance != null)
        {
            DiagnosisManager.Instance.SetObservation(
                "Cloud",
                mostCommonType.ToString()
            );
        }
    }

    public void ResetMinigame()
    {
        sortedClouds = 0;
        hasWon = false;

        CalculateTotalClouds();

        for (int i = 0; i < cloudLines.Length; i++)
        {
            if (cloudLines[i] != null)
                cloudLines[i].ResetLine();
        }
    }
}