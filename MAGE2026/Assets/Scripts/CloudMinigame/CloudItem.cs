using UnityEngine;

public class CloudItem : MonoBehaviour
{
    [Header("Cloud")]
    public CloudType cloudType;

    [HideInInspector]
    public CloudLine cloudLine;

    [HideInInspector]
    public Camera minigameCamera;

    [HideInInspector]
    public float conveyorDistance;

    public bool IsDragging { get; private set; }

    private Vector3 dragOffset;
    private float distanceWhenPickedUp;
    private float dragStartTime;
    private float dragZ;

    void OnMouseDown()
    {
        IsDragging = true;

        distanceWhenPickedUp = conveyorDistance;
        dragStartTime = Time.time;
        dragZ = transform.position.z;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        dragOffset = transform.position - mouseWorldPosition;
    }

    void OnMouseDrag()
    {
        if (!IsDragging)
            return;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        Vector3 newPosition = mouseWorldPosition + dragOffset;
        newPosition.z = dragZ;

        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        if (!IsDragging)
            return;

        IsDragging = false;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPosition);

        CloudFolder folder = null;

        foreach (Collider2D hit in hits)
        {
            CloudFolder possibleFolder = hit.GetComponentInParent<CloudFolder>();

            if (possibleFolder != null)
            {
                folder = possibleFolder;
                break;
            }
        }

        if (folder != null)
        {
            bool accepted = folder.Accepts(cloudType);

            if (accepted)
            {
                cloudLine.ItemSorted(this);
            }
            else
            {
                ReturnToLine();
            }
        }
        else
        {
            ReturnToLine();
        }
    }

    void ReturnToLine()
    {
        float dragTime = Time.time - dragStartTime;
        cloudLine.ReturnItemToLine(
            this,
            distanceWhenPickedUp,
            dragTime
        );
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        float distanceFromCamera =
            Mathf.Abs(
                minigameCamera.transform.position.z -
                dragZ
            );

        mousePosition.z = distanceFromCamera;

        Vector3 worldPosition =
            minigameCamera.ScreenToWorldPoint(mousePosition);

        worldPosition.z = dragZ;

        return worldPosition;
    }
}