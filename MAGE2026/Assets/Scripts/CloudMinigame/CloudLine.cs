using System.Collections.Generic;
using UnityEngine;

public class CloudLine : MonoBehaviour
{
    [Header("Line")]
    public float speed = 2f;

    public float startX = -8f;
    public float endX = 8f;

    public float itemSpacing = 1.2f;

    [Header("Items")]
    public List<CloudItem> items = new List<CloudItem>();

    void Start()
    {
        SetupItems();
    }

    void Update()
    {
        MoveItems();
    }

    void SetupItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CloudItem item = items[i];

            if (item == null)
                continue;

            item.cloudLine = this;
            item.conveyorDistance = i * itemSpacing;

            PositionItem(item);
        }
    }

    void MoveItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CloudItem item = items[i];

            if (item == null)
                continue;

            if (item.IsDragging)
                continue;

            item.conveyorDistance += speed * Time.deltaTime;

            if (item.conveyorDistance > endX - startX)
            {
                item.conveyorDistance = 0f;
            }

            PositionItem(item);
        }
    }

    void PositionItem(CloudItem item)
    {
        Vector3 position = item.transform.position;

        position.x = transform.position.x + startX + item.conveyorDistance;
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
            originalDistance + speed * dragTime;

        PositionItem(item);
    }

    public void ItemSorted(CloudItem item)
    {
        items.Remove(item);

        Destroy(item.gameObject);
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