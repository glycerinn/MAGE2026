using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Stage Minigames")]
    public GameObject[] stageMinigames;

    [Header("Stage Complete UI")]
    public GameObject stageCompleteScreen;

    private HashSet<string> completedMinigames = new HashSet<string>();
    private int currentStage;
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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentStage = 0;
        stageComplete = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        ActivateCurrentStage();
    }

    void ActivateCurrentStage()
    {
        for (int i = 0; i < stageMinigames.Length; i++)
        {
            if (stageMinigames[i] != null)
                stageMinigames[i].SetActive(i <= currentStage);
        }

        Debug.Log("Starting Stage " + CurrentStage);
    }

    public void MinigameWon(string minigameName)
    {
        if (completedMinigames.Contains(minigameName))
            return;

        completedMinigames.Add(minigameName);

        Debug.Log("Minigame won: " + minigameName);

        CheckStageComplete();
    }

    void CheckStageComplete()
    {
        for (int i = 0; i <= currentStage; i++)
        {
            string requiredMinigame = stageMinigames[i].GetComponent<IMinigame>().MinigameName;

            if (!completedMinigames.Contains(requiredMinigame))
                return;
        }

        CompleteCurrentStage();
    }

    void CompleteCurrentStage()
    {
        if (stageComplete)
            return;

        stageComplete = true;

        Debug.Log("STAGE " + CurrentStage + " COMPLETE!");

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(true);
    }

    public void NextStage()
    {
        if (!stageComplete)
            return;

        if (currentStage >= stageMinigames.Length - 1)
        {
            Debug.Log("ALL STAGES COMPLETE!");
            return;
        }

        currentStage++;
        stageComplete = false;

        if (stageCompleteScreen != null)
            stageCompleteScreen.SetActive(false);

        ActivateCurrentStage();
    }

    public void Quit()
    {
        Debug.Log("Quit pressed.");
    }
}