using UnityEngine;

public class CameraParallaxMenu : MonoBehaviour
{
    [Header("Mouse Parallax Settings")]
    public float moveAmount = 0.3f;     
    public float rotateAmount = 2.0f;   
    public float smoothSpeed = 3.0f; 

    [Header("Idle Auto-Move Settings")]
    public float idleSwayAmount = 0.1f; 
    public float idleSwaySpeed = 1.0f;  
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;
        
        mouseX = Mathf.Clamp(mouseX, -1f, 1f);
        mouseY = Mathf.Clamp(mouseY, -1f, 1f);

        float swayX = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmount;
        float swayY = Mathf.Cos(Time.time * idleSwaySpeed * 0.9f) * idleSwayAmount;

        Vector3 targetPos = startPosition + new Vector3(mouseX * moveAmount + swayX, mouseY * moveAmount + swayY, 0f);
        Quaternion targetRot = startRotation * Quaternion.Euler(-mouseY * rotateAmount, mouseX * rotateAmount, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}