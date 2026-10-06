using UnityEngine;

public class DesktopCamController : MonoBehaviour
{
    [Header("Main Room")]
    public Camera main;
    public PlayerCamera playerCamera;

    [Header("Minigame Cameras")]
    public Camera[] cams;

    [Header("Submission")]
    public GameObject checkbutton;

    [Header("Minigame Back Button")]
    public GameObject backButton;

    private int currentCam = -1;
    private bool inMinigame;

    void Start()
    {
        inMinigame = false;
        currentCam = -1;

        if (main != null)
            main.gameObject.SetActive(true);

        if (checkbutton != null)
            checkbutton.SetActive(true);

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

        currentCam = index;
        inMinigame = true;

        if (checkbutton != null)
            checkbutton.SetActive(false);

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

        if (checkbutton != null)
            checkbutton.SetActive(true);

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