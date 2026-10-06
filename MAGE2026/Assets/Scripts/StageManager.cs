using TMPro;
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
    public TMP_Text stageCompleteText;
    public GameObject nextStageButton;
    public GameObject retryButton;

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
    private bool[] minigameCompleted;
    private bool stageComplete;
    private bool diagnosisStarted;

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

        stageComplete = false;
        diagnosisStarted = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        StartStage();
    }

    void StartStage()
    {
        stageComplete = false;
        diagnosisStarted = false;

        int minigameCount =
            GetMinigameCountForStage();

        minigameCompleted =
            new bool[minigameCount];

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
                "Starting minigames immediately."
            );

            ActivateAllMinigames();
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
                ". Starting minigames."
            );

            ActivateAllMinigames();
            return;
        }

        string nodeName =
            stageDialogueNodes[dialogueIndex];

        if (string.IsNullOrEmpty(nodeName))
        {
            Debug.LogWarning(
                "Dialogue node is empty for Stage " +
                CurrentStage +
                ". Starting minigames."
            );

            ActivateAllMinigames();
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

        ActivateAllMinigames();
    }

    void ActivateAllMinigames()
    {
        int minigameCount =
            GetMinigameCountForStage();

        for (int i = 0;
            i < minigameCount;
            i++)
        {
            if (minigames[i] == null)
                continue;

            minigames[i].SetActive(true);

            if (minigames[i].TryGetComponent(
                out CloudMinigame cloudMinigame))
            {
                cloudMinigame.StartCloudMinigame();
            }

            if (minigames[i].TryGetComponent(
                out HumidityMinigame humidityMinigame))
            {
                humidityMinigame.StartHumidityMinigame();
            }

            Debug.Log(
                "Activated minigame: " +
                minigames[i].name
            );
        }
    }

    public void MinigameWon(string minigameName)
    {
        if (stageComplete)
            return;

        int minigameCount =
            GetMinigameCountForStage();

        for (int i = 0;
            i < minigameCount;
            i++)
        {
            if (minigames[i] == null)
                continue;

            if (!minigames[i].TryGetComponent(
                out IMinigame minigame))
            {
                continue;
            }

            if (minigame.MinigameName != minigameName)
                continue;

            if (minigameCompleted[i])
            {
                Debug.LogWarning(
                    "Minigame already completed: " +
                    minigameName
                );

                return;
            }

            minigameCompleted[i] = true;

            Debug.Log(
                "Minigame complete: " +
                minigameName
            );

            CheckAllMinigamesComplete();

            return;
        }

        Debug.LogWarning(
            "Could not find minigame: " +
            minigameName
        );
    }

    void CheckAllMinigamesComplete()
    {
        int minigameCount =
            GetMinigameCountForStage();

        for (int i = 0;
            i < minigameCount;
            i++)
        {
            if (!minigameCompleted[i])
                return;
        }

        StartDiagnosis();
    }

    void StartDiagnosis()
    {
        if (diagnosisStarted)
            return;

        diagnosisStarted = true;

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

    public void CompleteDiagnosis(bool correct)
    {
        if (correct)
        {
            ShowCorrectResult();
        }
        else
        {
            ShowIncorrectResult();
        }
    }

    void ShowCorrectResult()
    {
        stageComplete = true;

        Debug.Log(
            "STAGE " +
            CurrentStage +
            " COMPLETE!"
        );

        if (stageCompleteText != null)
        {
            stageCompleteText.text =
                "STAGE " +
                CurrentStage +
                " COMPLETE!";
        }

        if (nextStageButton != null)
            nextStageButton.SetActive(true);

        if (retryButton != null)
            retryButton.SetActive(false);

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);

        if (stageAnimation != null)
            stageAnimation.Play();
    }

    void ShowIncorrectResult()
    {
        stageComplete = false;

        Debug.Log(
            "DIAGNOSIS INCORRECT!"
        );

        if (stageCompleteText != null)
            stageCompleteText.text = "INCORRECT!";

        if (nextStageButton != null)
            nextStageButton.SetActive(false);

        if (retryButton != null)
            retryButton.SetActive(true);

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);

        if (stageAnimation != null)
            stageAnimation.Play();
    }

    public void RestartCurrentStage()
    {
        Debug.Log(
            "RETRYING STAGE " +
            CurrentStage
        );

        stageComplete = false;
        diagnosisStarted = false;

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
        diagnosisStarted = false;

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

    public bool IsMinigameCompleted(int index)
    {
        if (minigameCompleted == null)
            return false;

        if (index < 0 ||
            index >= minigameCompleted.Length)
            return false;

        return minigameCompleted[index];
    }

    public void Quit()
    {
        Debug.Log("Quit pressed.");
    }
}