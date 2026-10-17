using UnityEngine;
using DG.Tweening;

public enum CloudType
{
    Cumulus,
    Nimbostratus,
    Stratus
}

public class CloudItem : MonoBehaviour
{
    [Header("Cloud")]
    public CloudType cloudType;
    public Sprite[] possibleSprites;
    public SpriteRenderer spriteRenderer;

    [Header("Drag Shake")]
    public float shakeAngle = 5f;
    public float shakeDuration = 0.15f;
    private Tween shakeTween;
    private Quaternion originalRotation;

    [Header("Hover Scale")]
    public float hoverScaleMultiplier = 1.15f;
    public float hoverScaleDuration = 0.2f;
    private Vector3 originalScale;
    private Tween scaleTween;

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
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        ChooseRandomSprite();
    }

    void StartShake()
    {
        shakeTween?.Kill();
        transform.rotation = originalRotation;
        shakeTween = transform.DOLocalRotate(
            new Vector3(0f, 0f, shakeAngle),
            shakeDuration
        )
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

    public void ChooseRandomSprite()
    {
        if (possibleSprites == null || possibleSprites.Length == 0)
            return;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("CloudItem has no SpriteRenderer!", this);
            return;
        }

        spriteRenderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
    }

    void OnMouseEnter()
    {
        if (IsDragging)
            return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(
            originalScale * hoverScaleMultiplier,
            hoverScaleDuration
        ).SetEase(Ease.OutQuad);
    }

    void OnMouseExit()
    {
        if (IsDragging)
            return;

        ResetScale();
    }

    void ResetScale()
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(
            originalScale,
            hoverScaleDuration
        ).SetEase(Ease.OutQuad);
    }

    void OnMouseDown()
    {
        audioManager.playGrabSFX();
        IsDragging = true;
        scaleTween?.Kill();
        transform.localScale = originalScale;
        distanceWhenPickedUp = conveyorDistance;
        dragStartTime = Time.time;
        dragZ = transform.position.z;
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        dragOffset = transform.position - mouseWorldPosition;
        StartShake();
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
                cloudLine.ItemSorted(this);
            else
                ReturnToLine();
        }
        else
        {
            ReturnToLine();
        }
    }

    void ReturnToLine()
    {
        shakeTween?.Kill();
        shakeTween = null;
        transform.localRotation = originalRotation;
        float dragTime = Time.time - dragStartTime;
        cloudLine.ReturnItemToLine(this, distanceWhenPickedUp, dragTime);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        float distanceFromCamera = Mathf.Abs(minigameCamera.transform.position.z - dragZ);
        mousePosition.z = distanceFromCamera;
        Vector3 worldPosition = minigameCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = dragZ;
        return worldPosition;
    }

    void OnDestroy()
    {
        shakeTween?.Kill();
        scaleTween?.Kill();
    }
}