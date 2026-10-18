
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
    private bool interactionModeForced = false;

    void Start()
    {
        interactionMode = false;
        interactionModeForced = false;
        ApplyCursorState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.RightControl))
        {
            if (canLook && !interactionModeForced)
            {
                interactionMode = !interactionMode;
                ApplyCursorState();
            }
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

    void ApplyCursorState()
    {
        bool showCursor = !canLook || interactionMode;

        Cursor.lockState = showCursor
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = showCursor;

        if (crosshair != null)
            crosshair.SetActive(canLook && !interactionMode);
    }

    public void SetCameraLook(bool enabled)
    {
        canLook = enabled;
        ApplyCursorState();
    }

    public void SetInteractionModePublic(bool enabled)
    {
        interactionMode = enabled;
        interactionModeForced = enabled;
        ApplyCursorState();
    }

    public void ForceGameplayCursorState()
    {
        canLook = true;
        interactionMode = false;
        interactionModeForced = false;
        ApplyCursorState();
    }

    public void ResetCameraRotation()
    {
        xRotation = 0f;
        yRotation = 0f;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
