
using UnityEngine;
using DG.Tweening;

public class ClickRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAmount = new Vector3(0f, 0f, 10f);
    public float duration = 0.2f;
    public Ease ease = Ease.OutSine;

    [Header("Input Settings")]
    public Camera inputCamera;
    public LayerMask clickableLayers = ~0;

    private Quaternion originalRotation;
    private Tween rotationTween;

    void Awake()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (inputCamera == null || !Input.GetMouseButtonDown(0))
            return;

        Ray ray = inputCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayers))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                RotateOnClick();
        }
    }

    void RotateOnClick()
    {
        rotationTween?.Kill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotateQuaternion(
            originalRotation * Quaternion.Euler(rotationAmount),
            duration
        ).SetEase(ease));
        sequence.Append(transform.DOLocalRotateQuaternion(
            originalRotation,
            duration
        ).SetEase(Ease.OutBack));

        rotationTween = sequence;
    }

    void OnDisable()
    {
        rotationTween?.Kill();
        if (transform != null)
            transform.localRotation = originalRotation;
    }

    void OnDestroy()
    {
        rotationTween?.Kill();
    }
}
