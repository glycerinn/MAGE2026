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
        ResetMinigame();
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

            DetermineCloudObservation();

            Debug.Log("CLOUD MINIGAME WON!");

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
        totalClouds = 0;
        hasWon = false;

        CloudType requiredType = GetRequiredCloudType();

        for (int i = 0; i < cloudLines.Length; i++)
        {
            if (cloudLines[i] == null)
                continue;

            cloudLines[i].GenerateClouds(requiredType);
            totalClouds += cloudLines[i].TotalClouds;
        }

        Debug.Log(
            "Cloud minigame generated with majority type: " +
            requiredType +
            " | Total clouds: " +
            totalClouds
        );
    }

    CloudType GetRequiredCloudType()
    {
        if (DiagnosisManager.Instance == null)
        {
            return CloudType.Cumulus;
        }

        string requiredCloud =
            DiagnosisManager.Instance.GetRequiredObservation("Cloud");

        if (System.Enum.TryParse(
            requiredCloud,
            out CloudType cloudType))
        {
            return cloudType;
        }

        return CloudType.Cumulus;
    }
}