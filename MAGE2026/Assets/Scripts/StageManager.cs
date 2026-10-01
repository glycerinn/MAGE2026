using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public GameObject diagnosisScreen;

    [Header("Minigames")]
    public GameObject[] minigames;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera[] minigameCameras;

    [Header("Stage Complete UI")]
    public GameObject stageCompleteScreen;

    [Header("Stage Complete Animation")]
    public StageFinishAnimation stageAnimation;

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

        if (diagnosisScreen != null)
            diagnosisScreen.SetActive(false);

        StartStage();
    }

    void StartStage()
    {
        currentMinigame = 0;

        DisableAllMinigames();

        if (DiagnosisManager.Instance != null)
            DiagnosisManager.Instance.StartStageDiagnosis(CurrentStage);

        Debug.Log("Starting Stage " + CurrentStage);

        ActivateCurrentMinigame();
    }

    void ActivateCurrentMinigame()
    {
        if (currentMinigame > currentStage)
        {
            StartDiagnosis();
            return;
        }

        GameObject minigameObject = minigames[currentMinigame];

        if (minigameObject == null)
            return;

        minigameObject.SetActive(true);

        if (minigameObject.TryGetComponent(
            out CloudMinigame cloudMinigame))
        {
            cloudMinigame.StartCloudMinigame();
        }

        Debug.Log(
            "Starting minigame: " +
            minigameObject.name
        );
    }

    public void MinigameWon(string minigameName)
    {
        if (stageComplete)
            return;

        GameObject currentObject = minigames[currentMinigame];

        if (currentObject == null)
            return;

        if (!currentObject.TryGetComponent(out IMinigame minigame))
            return;

        if (minigame.MinigameName != minigameName)
        {
            Debug.LogWarning(
                "Wrong minigame completed. Expected: " +
                minigame.MinigameName +
                " | Received: " +
                minigameName
            );

            return;
        }

        Debug.Log("Minigame complete: " + minigameName);

        currentMinigame++;

        if (currentMinigame > currentStage)
        {
            StartDiagnosis();
            return;
        }

        ActivateCurrentMinigame();
    }

    void StartDiagnosis()
    {
        Debug.Log("ALL MINIGAMES COMPLETE. STARTING DIAGNOSIS.");

        if (DiagnosisUI.Instance != null)
            DiagnosisUI.Instance.SetupForStage(CurrentStage);

        if (diagnosisScreen != null)
            diagnosisScreen.SetActive(true);
    }

    public void CompleteDiagnosis()
    {
        if (diagnosisScreen != null)
            diagnosisScreen.SetActive(false);

        CompleteStage();
    }

    void CompleteStage()
    {
        stageComplete = true;

        Debug.Log("STAGE " + CurrentStage + " COMPLETE!");

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);

        if (stageAnimation != null)
            stageAnimation.Play();
    }

    public void RestartCurrentStage()
    {
        Debug.Log("WRONG DIAGNOSIS. RESTARTING STAGE " + CurrentStage);

        stageComplete = false;

        if (diagnosisScreen != null)
            diagnosisScreen.SetActive(false);

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        if (DiagnosisManager.Instance != null)
            DiagnosisManager.Instance.ResetDiagnosis();

        if (stageAnimation != null)
            stageAnimation.Hide();

        ResetMinigames();

        StartStage();
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

        if (stageAnimation != null)
            stageAnimation.Hide();

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