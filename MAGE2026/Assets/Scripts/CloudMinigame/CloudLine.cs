using System.Collections.Generic;
using UnityEngine;

public class CloudLine : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float startX = -8f;
    public float endX = 8f;
    public float itemSpacing = 1.2f;

    [Header("Minigame Camera")]
    public Camera minigameCamera;

    [Header("Cloud Prefabs")]
    public List<CloudItem> cloudPrefabs = new List<CloudItem>();

    [Header("Total Clouds")]
    public int totalCloudAmount = 10;

    [Header("Settings")]
    public bool randomizeOrder = true;
    public bool reverse = false;

    private List<CloudItem> items = new List<CloudItem>();
    private Dictionary<CloudType, int> generatedCloudCounts =
        new Dictionary<CloudType, int>();

    private float lineLength;
    private CloudMinigame minigame;
    private int totalClouds;

    public int TotalClouds => totalClouds;

    void Awake()
    {
        lineLength = endX - startX;
        minigame = GetComponentInParent<CloudMinigame>();
    }

    void Update()
    {
        MoveItems();
    }

    public void GenerateClouds(CloudType requiredType)
    {
        ClearClouds();

        generatedCloudCounts.Clear();

        if (cloudPrefabs == null || cloudPrefabs.Count == 0)
        {
            Debug.LogError(
                "CloudLine has no Cloud Prefabs assigned!"
            );
            return;
        }

        List<CloudItem> otherPrefabs =
            new List<CloudItem>();

        CloudItem requiredPrefab = null;

        for (int i = 0; i < cloudPrefabs.Count; i++)
        {
            if (cloudPrefabs[i] == null)
                continue;

            if (cloudPrefabs[i].cloudType == requiredType)
            {
                requiredPrefab = cloudPrefabs[i];
            }
            else
            {
                otherPrefabs.Add(cloudPrefabs[i]);
            }
        }

        if (requiredPrefab == null)
        {
            Debug.LogError(
                "No cloud prefab exists for required type: " +
                requiredType
            );
            return;
        }

        int amount = Mathf.Max(1, totalCloudAmount);

        int requiredAmount =
            Mathf.FloorToInt(amount / 2f) + 1;

        int otherAmount =
            amount - requiredAmount;

        List<CloudItem> spawnList =
            new List<CloudItem>();

        for (int i = 0; i < requiredAmount; i++)
        {
            spawnList.Add(requiredPrefab);

            if (!generatedCloudCounts.ContainsKey(requiredType))
                generatedCloudCounts[requiredType] = 0;

            generatedCloudCounts[requiredType]++;
        }

        for (int i = 0; i < otherAmount; i++)
        {
            if (otherPrefabs.Count == 0)
                break;

            CloudItem prefab =
                otherPrefabs[
                    Random.Range(0, otherPrefabs.Count)
                ];

            spawnList.Add(prefab);

            if (!generatedCloudCounts.ContainsKey(prefab.cloudType))
                generatedCloudCounts[prefab.cloudType] = 0;

            generatedCloudCounts[prefab.cloudType]++;
        }

        if (randomizeOrder)
            ShuffleList(spawnList);

        totalClouds = spawnList.Count;

        for (int i = 0; i < spawnList.Count; i++)
        {
            CloudItem newItem =
                Instantiate(
                    spawnList[i],
                    transform
                );

            newItem.cloudLine = this;
            newItem.minigameCamera = minigameCamera;
            newItem.conveyorDistance =
                i * itemSpacing;

            items.Add(newItem);

            PositionItem(newItem);
        }

        Debug.Log(
            name +
            " generated " +
            totalClouds +
            " clouds. Required majority: " +
            requiredType
        );
    }

    public Dictionary<CloudType, int> GetCloudCounts()
    {
        return new Dictionary<CloudType, int>(
            generatedCloudCounts
        );
    }

    void MoveItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CloudItem item = items[i];

            if (item == null || item.IsDragging)
                continue;

            item.conveyorDistance +=
                speed * Time.deltaTime;

            if (item.conveyorDistance > lineLength)
            {
                item.conveyorDistance -= lineLength;
            }

            PositionItem(item);
        }
    }

    void PositionItem(CloudItem item)
    {
        Vector3 position = item.transform.position;

        if (!reverse)
        {
            position.x =
                transform.position.x +
                startX +
                item.conveyorDistance;
        }
        else
        {
            position.x =
                transform.position.x -
                startX -
                item.conveyorDistance;
        }

        position.y = transform.position.y;
        position.z = transform.position.z;

        item.transform.position = position;
    }

    public void ReturnItemToLine(
        CloudItem item,
        float originalDistance,
        float dragTime)
    {
        item.conveyorDistance =
            originalDistance +
            speed * dragTime;

        item.conveyorDistance %= lineLength;

        PositionItem(item);
    }

    public void ItemSorted(CloudItem item)
    {
        items.Remove(item);

        item.gameObject.SetActive(false);
        Destroy(item.gameObject);

        if (minigame != null)
            minigame.CloudSorted();
    }

    void ClearClouds()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] != null)
                Destroy(items[i].gameObject);
        }

        items.Clear();
        totalClouds = 0;
    }

    void ShuffleList(List<CloudItem> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex =
                Random.Range(0, i + 1);

            CloudItem temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void ResetLine()
    {
        CloudType requiredType =
            CloudType.Cumulus;

        if (minigame != null)
        {
            if (DiagnosisManager.Instance != null)
            {
                string requiredCloud =
                    DiagnosisManager.Instance
                        .GetRequiredObservation("Cloud");

                if (System.Enum.TryParse(
                    requiredCloud,
                    out CloudType parsedType))
                {
                    requiredType = parsedType;
                }
            }
        }

        GenerateClouds(requiredType);
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;
        start.x += startX;

        Vector3 end = transform.position;
        end.x += endX;

        Gizmos.color = Color.white;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.15f);
        Gizmos.DrawWireSphere(end, 0.15f);
    }
}