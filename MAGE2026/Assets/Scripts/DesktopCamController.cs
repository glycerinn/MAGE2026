
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

    [Header("Monitor Button Interaction")]
    public CanvasGroup monitorButtonsGroup;

    [Header("Pixelation Display")]
    public GameObject pixelationCanvas;

    public GlitchTransition[] cameraGlitches;
    public GlitchTransition mainCameraGlitch;

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

        for (int i = 0; cams != null && i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inMinigame)
        {
            BackToMain();
        }
    }

    public void OpenMinigame(int index)
    {
        if (index < 0 || index >= cams.Length)
        {
            Debug.LogWarning("Invalid minigame camera index: " + index);
            return;
        }

        if (pixelationCanvas != null)
        pixelationCanvas.SetActive(false);

        if (cams[index] == null)
        {
            Debug.LogWarning("Minigame camera at index " + index + " is not assigned.");
            return;
        }

        if (monitorButtonsGroup != null)
        {
            monitorButtonsGroup.interactable = false;
            monitorButtonsGroup.blocksRaycasts = false;
        }

        if (main != null)
            main.gameObject.SetActive(false);

        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }

        cams[index].gameObject.SetActive(true);

        if (cameraGlitches != null &&
            index < cameraGlitches.Length &&
            cameraGlitches[index] != null)
        {
            cameraGlitches[index].PlayGlitchTransition();
        }

        currentCam = index;
        inMinigame = true;

        if (playerCamera != null)
            playerCamera.SetInteractionModePublic(true);

        if (backButton != null)
            backButton.SetActive(true);

        Debug.Log("Opened minigame camera: " + cams[index].name);
    }

    public void BackToMain()
    {
        if (!inMinigame)
            return;

        if (mainCameraGlitch != null)
            mainCameraGlitch.PlayGlitchTransition();

        ResetToMainCamera();
        Debug.Log("Returned to main room and reset camera direction.");
    }

    public void ResetToMainCamera()
    {
        for (int i = 0; cams != null && i < cams.Length; i++)
        {
            if (cams[i] != null)
                cams[i].gameObject.SetActive(false);
        }

        if (pixelationCanvas != null)
            pixelationCanvas.SetActive(true);

        if (main != null)
            main.gameObject.SetActive(true);

        if (playerCamera != null)
        {
            playerCamera.ResetCameraRotation();
            playerCamera.ForceGameplayCursorState();
        }

        if (monitorButtonsGroup != null)
        {
            monitorButtonsGroup.interactable = true;
            monitorButtonsGroup.blocksRaycasts = true;
        }

        currentCam = -1;
        inMinigame = false;

        if (backButton != null)
            backButton.SetActive(false);
    }

    public bool IsInMinigame()
    {
        return inMinigame;
    }
}
