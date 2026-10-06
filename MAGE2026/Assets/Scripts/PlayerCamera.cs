using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float yRotation = 0f;
    float xRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void ResetCameraRotation()
    {
        xRotation = 0f;
        yRotation = 0f;

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}