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

    [Header("Amount Of Each")]
    public List<int> cloudAmounts = new List<int>();

    [Header("Settings")]
    public bool randomizeOrder = true;
    public bool reverse = false;

    private List<CloudItem> items = new List<CloudItem>();
    private float lineLength;

    void Start()
    {
        lineLength = endX - startX;
        SpawnClouds();
    }

    void Update()
    {
        MoveItems();
    }

    void SpawnClouds()
    {
        items.Clear();

        List<CloudItem> spawnList = new List<CloudItem>();

        int count = Mathf.Min(cloudPrefabs.Count, cloudAmounts.Count);

        for (int i = 0; i < count; i++)
        {
            if (cloudPrefabs[i] == null)
                continue;

            for (int j = 0; j < cloudAmounts[i]; j++)
            {
                spawnList.Add(cloudPrefabs[i]);
            }
        }

        if (randomizeOrder)
        {
            ShuffleList(spawnList);
        }

        for (int i = 0; i < spawnList.Count; i++)
        {
            CloudItem newItem = Instantiate(spawnList[i], transform);

            newItem.cloudLine = this;
            newItem.minigameCamera = minigameCamera;
            newItem.conveyorDistance = i * itemSpacing;

            items.Add(newItem);

            PositionItem(newItem);
        }
    }

    void MoveItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CloudItem item = items[i];

            if (item == null || item.IsDragging)
                continue;

            item.conveyorDistance += speed * Time.deltaTime;

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

        if(reverse == false)
        {
            position.x = transform.position.x + startX + item.conveyorDistance;
        }else if(reverse == true)
        {
            position.x = transform.position.x - startX - item.conveyorDistance;
        }
        
        position.y = transform.position.y;
        position.z = transform.position.z;

        item.transform.position = position;
    }

    public void ReturnItemToLine(CloudItem item, float originalDistance, float dragTime)
    {
        item.conveyorDistance = originalDistance + speed * dragTime;
        item.conveyorDistance %= lineLength;

        PositionItem(item);
    }

    public void ItemSorted(CloudItem item)
    {
        items.Remove(item);
        item.gameObject.SetActive(false);
        Destroy(item.gameObject);
    }

    void ShuffleList(List<CloudItem> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            CloudItem temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
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