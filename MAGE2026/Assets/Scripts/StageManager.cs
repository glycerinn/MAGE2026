using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Minigames")]
    public GameObject[] minigames;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera[] minigameCameras;

    [Header("Stage Complete UI")]
    public GameObject stageCompleteScreen;

    private int currentStage;
    private int currentMinigame;
    private bool stageComplete;

    public int CurrentStage => currentStage + 1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        currentStage = PlayerPrefs.GetInt("SelectedStage", 1) - 1;
        currentStage = Mathf.Clamp(currentStage, 0, minigames.Length - 1);

        currentMinigame = 0;
        stageComplete = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        StartStage();
    }

    void SwitchToMainCamera()
    {
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        for (int i = 0; i < minigameCameras.Length; i++)
        {
            if (minigameCameras[i] != null)
                minigameCameras[i].gameObject.SetActive(false);
        }
    }

    void StartStage()
    {
        currentMinigame = 0;

        DisableAllMinigames();

        Debug.Log("Starting Stage " + CurrentStage);

        ActivateCurrentMinigame();
    }

    void ActivateCurrentMinigame()
    {
        if (currentMinigame > currentStage)
        {
            CompleteStage();
            return;
        }

        GameObject minigame = minigames[currentMinigame];

        if (minigame != null)
            minigame.SetActive(true);

        Debug.Log("Starting minigame: " + minigame.name);
    }

    public void MinigameWon(string minigameName)
    {
        if (stageComplete)
            return;

        GameObject currentObject = minigames[currentMinigame];

        if (currentObject == null)
            return;

        if (currentObject.name != minigameName)
        {
            Debug.LogWarning(
                "Wrong minigame completed. Expected: " +
                currentObject.name +
                " | Received: " +
                minigameName
            );

            return;
        }

        Debug.Log("Minigame complete: " + minigameName);

        currentObject.SetActive(false);

        currentMinigame++;

        if (currentMinigame > currentStage)
        {
            CompleteStage();
            return;
        }

        ActivateCurrentMinigame();
    }

    void CompleteStage()
    {
        stageComplete = true;

        SwitchToMainCamera();

        Debug.Log("STAGE " + CurrentStage + " COMPLETE!");

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);
    }

    public void NextStage()
    {
        if (!stageComplete)
            return;

        if (currentStage >= minigames.Length - 1)
        {
            Debug.Log("ALL STAGES COMPLETE!");
            return;
        }

        currentStage++;
        stageComplete = false;

        PlayerPrefs.SetInt("SelectedStage", CurrentStage);
        PlayerPrefs.Save();

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        ResetMinigames();
        StartStage();
    }

    void ResetMinigames()
    {
        for (int i = 0; i < minigames.Length; i++)
        {
            if (minigames[i] == null)
                continue;

            minigames[i].SetActive(false);

            if (minigames[i].TryGetComponent(out IMinigame minigame))
                minigame.ResetMinigame();
        }
    }

    void DisableAllMinigames()
    {
        for (int i = 0; i < minigames.Length; i++)
        {
            if (minigames[i] != null)
                minigames[i].SetActive(false);
        }
    }

    public void Quit()
    {
        Debug.Log("Quit pressed.");
    }
}