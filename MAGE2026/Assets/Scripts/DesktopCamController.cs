using UnityEngine;

public class DesktopCamController : MonoBehaviour
{
    [Header("Main Room")]
    public Camera main;
    public PlayerCamera playerCamera;

    [Header("Minigame Cameras")]
    public Camera[] cams;

    [Header("Minigame Back Button")]
    public GameObject backButton;

    [Header("Minigame Glitch Effects")]
    public KinoGlitch.DigitalGlitchController[] cameraGlitches;

    private int currentCam = -1;
    private bool inMinigame;

    void Start()
    {
        inMinigame = false;
        currentCam = -1;

        if (main != null)
            main.gameObject.SetActive(true);

        if (backButton != null)
            backButton.SetActive(false);

        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }
    }

    public void OpenMinigame(int index)
    {
        if (index < 0 || index >= cams.Length)
        {
            Debug.LogWarning(
                "Invalid minigame camera index: " +
                index
            );
            return;
        }

        if (cams[index] == null)
        {
            Debug.LogWarning(
                "Minigame camera at index " +
                index +
                " is not assigned."
            );
            return;
        }

        if (main != null)
            main.gameObject.SetActive(false);

        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }

        cams[index].gameObject.SetActive(true);

        if (cameraGlitches != null && index < cameraGlitches.Length && cameraGlitches[index] != null)
        {
            cameraGlitches[index].PlayGlitchTransition(0.3f);
        }

        currentCam = index;
        inMinigame = true;

        if (backButton != null)
            backButton.SetActive(true);

        Debug.Log(
            "Opened minigame camera: " +
            cams[index].name
        );
    }

    public void BackToMain()
    {
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }

        if (main != null)
            main.gameObject.SetActive(true);

        if (playerCamera != null)
            playerCamera.ResetCameraRotation();

        currentCam = -1;
        inMinigame = false;
        
        if (backButton != null)
            backButton.SetActive(false);

        Debug.Log(
            "Returned to main room and reset camera direction."
        );
    }

    public bool IsInMinigame()
    {
        return inMinigame;
    }
}