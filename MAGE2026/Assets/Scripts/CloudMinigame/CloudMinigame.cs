using System.Collections.Generic;
using UnityEngine;

public class CloudMinigame : MonoBehaviour, IMinigame
{
    [Header("Cloud Lines")]
    public CloudLine[] cloudLines;

    [Header("Clue")]
    public CluePopup cluePopup;

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

            StageManager.Instance.MinigameWon(
                MinigameName
            );
        }
    }

    void DetermineCloudObservation()
    {
        Dictionary<CloudType, int> cloudCounts =
            new Dictionary<CloudType, int>();

        foreach (CloudType type in System.Enum.GetValues(
            typeof(CloudType)))
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
                cloudCounts[pair.Key] += pair.Value;
        }

        CloudType mostCommonType =
            CloudType.Cumulus;

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
            string cloudObservation =
                mostCommonType.ToString();

            DiagnosisManager.Instance.SetObservation(
                "Cloud",
                cloudObservation
            );

            if (cluePopup != null)
            {
                cluePopup.ShowClue(
                    "Cloud",
                    cloudObservation
                );
            }
        }
    }

    public void ResetMinigame()
    {
        sortedClouds = 0;
        totalClouds = 0;
        hasWon = false;

        if (cluePopup != null)
            cluePopup.HideClue();

        Debug.Log(
            "Cloud minigame reset. " +
            "Cloud generation will happen when the stage starts."
        );
    }

    public void StartCloudMinigame()
    {
        sortedClouds = 0;
        totalClouds = 0;
        hasWon = false;

        if (cluePopup != null)
            cluePopup.HideClue();

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError(
                "CloudMinigame: DiagnosisManager.Instance is null."
            );

            return;
        }

        string requiredCloud =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Cloud"
            );

        if (string.IsNullOrEmpty(requiredCloud))
        {
            Debug.LogError(
                "CloudMinigame: No Cloud clue was selected."
            );

            return;
        }

        if (!System.Enum.TryParse(
            requiredCloud,
            out CloudType requiredType))
        {
            Debug.LogError(
                "CloudMinigame: Could not parse Cloud clue: " +
                requiredCloud
            );

            return;
        }

        Debug.Log(
            "Cloud minigame using selected diagnosis cloud: " +
            requiredType
        );

        for (int i = 0; i < cloudLines.Length; i++)
        {
            if (cloudLines[i] == null)
                continue;

            cloudLines[i].GenerateClouds(
                requiredType
            );

            totalClouds +=
                cloudLines[i].TotalClouds;
        }

        Debug.Log(
            "Cloud minigame generated with majority type: " +
            requiredType +
            " | Total clouds: " +
            totalClouds
        );
    }
}