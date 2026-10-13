using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject crosshair;
    public float mouseSensitivity = 500f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private float yRotation = 0f;
    private float xRotation = 0f;
    private bool canLook = true;
    private bool interactionMode = false;

    void Start()
    {
        SetInteractionMode(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            SetInteractionMode(!interactionMode);
        }

        if (!canLook || interactionMode)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void SetInteractionMode(bool enabled)
    {
        interactionMode = enabled;
        Cursor.lockState = enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enabled;
        if (crosshair != null)
            crosshair.SetActive(!enabled);
    }

    public void SetCameraLook(bool enabled)
    {
        canLook = enabled;
        if (!enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!interactionMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ResetCameraRotation()
    {
        xRotation = 0f;
        yRotation = 0f;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}