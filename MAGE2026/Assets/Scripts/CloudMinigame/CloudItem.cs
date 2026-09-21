using UnityEngine;

public class CloudItem : MonoBehaviour
{
    [Header("Cloud")]
    public CloudType cloudType;

    [Header("Minigame Camera")]
    public Camera minigameCamera;

    [HideInInspector]
    public CloudLine cloudLine;

    [HideInInspector]
    public float conveyorDistance;

    public bool IsDragging { get; private set; }

    private Vector3 dragOffset;
    private float distanceWhenPickedUp;
    private float dragStartTime;
    private float dragZ;

    void OnMouseDown()
    {
        if (cloudLine == null)
            return;

        if (minigameCamera == null)
            return;

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

        CloudFolder folder = FindFolderUnderMouse();

        if (folder != null && folder.Accepts(cloudType))
        {
            cloudLine.ItemSorted(this);
        }
        else
        {
            float dragTime = Time.time - dragStartTime;

            cloudLine.ReturnItemToLine(
                this,
                distanceWhenPickedUp,
                dragTime
            );
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        float distanceFromCamera =
            Mathf.Abs(minigameCamera.transform.position.z - dragZ);

        mousePosition.z = distanceFromCamera;

        Vector3 worldPosition =
            minigameCamera.ScreenToWorldPoint(mousePosition);

        worldPosition.z = dragZ;

        return worldPosition;
    }

    CloudFolder FindFolderUnderMouse()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        Collider2D hit =
            Physics2D.OverlapPoint(mouseWorldPosition);

        if (hit == null)
            return null;

        return hit.GetComponent<CloudFolder>();
    }
}