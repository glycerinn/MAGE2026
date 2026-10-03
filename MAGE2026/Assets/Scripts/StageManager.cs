using UnityEngine;
using Yarn.Unity;

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

    [Header("Stage Complete Animation")]
    public StageFinishAnimation stageAnimation;

    [Header("Yarn Dialogue")]
    public DialogueRunner dialogueRunner;
    public string[] stageDialogueNodes =
    {
        "Stage1",
        "Stage2",
        "Stage3",
        "Stage4",
        "Stage5"
    };

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
        currentStage =
            PlayerPrefs.GetInt("SelectedStage", 1) - 1;

        currentStage =
            Mathf.Clamp(
                currentStage,
                0,
                4
            );

        currentMinigame = 0;
        stageComplete = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        StartStage();
    }

    void StartStage()
    {
        currentMinigame = 0;

        DisableAllMinigames();

        if (DiagnosisManager.Instance != null)
        {
            DiagnosisManager.Instance.StartStageDiagnosis(
                CurrentStage
            );
        }

        Debug.Log(
            "Starting Stage " +
            CurrentStage
        );

        StartStageDialogue();
    }

    int GetMinigameCountForStage()
    {
        return Mathf.Min(
            CurrentStage,
            minigames.Length
        );
    }

    void StartStageDialogue()
    {
        if (dialogueRunner == null)
        {
            Debug.LogWarning(
                "No DialogueRunner assigned. " +
                "Starting minigame immediately."
            );

            ActivateCurrentMinigame();
            return;
        }

        int dialogueIndex =
            CurrentStage - 1;

        if (dialogueIndex < 0 ||
            dialogueIndex >= stageDialogueNodes.Length)
        {
            Debug.LogWarning(
                "No dialogue node assigned for Stage " +
                CurrentStage +
                ". Starting minigame."
            );

            ActivateCurrentMinigame();
            return;
        }

        string nodeName =
            stageDialogueNodes[dialogueIndex];

        if (string.IsNullOrEmpty(nodeName))
        {
            Debug.LogWarning(
                "Dialogue node is empty for Stage " +
                CurrentStage +
                ". Starting minigame."
            );

            ActivateCurrentMinigame();
            return;
        }

        Debug.Log(
            "Starting stage dialogue: " +
            nodeName
        );

        dialogueRunner.onDialogueComplete.AddListener(
            OnStageDialogueComplete
        );

        dialogueRunner.StartDialogue(nodeName);
    }

    void OnStageDialogueComplete()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(
            OnStageDialogueComplete
        );

        Debug.Log(
            "Stage " +
            CurrentStage +
            " opening dialogue complete."
        );

        ActivateCurrentMinigame();
    }

    void ActivateCurrentMinigame()
    {
        int minigameCount =
            GetMinigameCountForStage();

        if (currentMinigame >= minigameCount)
        {
            StartDiagnosis();
            return;
        }

        GameObject minigameObject =
            minigames[currentMinigame];

        if (minigameObject == null)
            return;

        minigameObject.SetActive(true);

        if (minigameObject.TryGetComponent(
            out CloudMinigame cloudMinigame))
        {
            cloudMinigame.StartCloudMinigame();
        }

        if (minigameObject.TryGetComponent(out HumidityMinigame humidityMinigame))
        {
            humidityMinigame.StartHumidityMinigame();
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

        if (currentMinigame >=
            GetMinigameCountForStage())
        {
            return;
        }

        GameObject currentObject =
            minigames[currentMinigame];

        if (currentObject == null)
            return;

        if (!currentObject.TryGetComponent(
            out IMinigame minigame))
        {
            return;
        }

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

        Debug.Log(
            "Minigame complete: " +
            minigameName
        );

        currentMinigame++;

        if (currentMinigame >=
            GetMinigameCountForStage())
        {
            StartDiagnosis();
            return;
        }

        ActivateCurrentMinigame();
    }

    void StartDiagnosis()
    {
        Debug.Log(
            "ALL MINIGAMES COMPLETE. " +
            "STARTING DIAGNOSIS."
        );

        if (DiagnosisUI.Instance != null)
        {
            DiagnosisUI.Instance.SetupForStage(
                CurrentStage
            );
        }
    }

    public void CompleteDiagnosis()
    {
        CompleteStage();
    }

    void CompleteStage()
    {
        stageComplete = true;

        Debug.Log(
            "STAGE " +
            CurrentStage +
            " COMPLETE!"
        );

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);

        if (stageAnimation != null)
            stageAnimation.Play();
    }

    public void RestartCurrentStage()
    {
        Debug.Log(
            "WRONG DIAGNOSIS. " +
            "RESTARTING STAGE " +
            CurrentStage
        );

        stageComplete = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        if (DiagnosisManager.Instance != null)
            DiagnosisManager.Instance.ResetDiagnosis();

        if (DiagnosisUI.Instance != null)
            DiagnosisUI.Instance.ResetUI();

        if (stageAnimation != null)
            stageAnimation.Hide();

        ResetMinigames();

        StartStage();
    }

    public void NextStage()
    {
        if (!stageComplete)
            return;

        if (currentStage >= 4)
        {
            Debug.Log(
                "ALL STAGES COMPLETE!"
            );

            return;
        }

        currentStage++;
        stageComplete = false;

        PlayerPrefs.SetInt(
            "SelectedStage",
            CurrentStage
        );

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
        for (int i = 0;
            i < minigames.Length;
            i++)
        {
            if (minigames[i] == null)
                continue;

            minigames[i].SetActive(false);

            if (minigames[i].TryGetComponent(
                out IMinigame minigame))
            {
                minigame.ResetMinigame();
            }
        }
    }

    void DisableAllMinigames()
    {
        for (int i = 0;
            i < minigames.Length;
            i++)
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