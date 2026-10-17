
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // public GameObject Credits;
    // public LevelLoader levelLoader;
    private AudioManager audioManager;
    private int sceneToContinue;
    public GameObject StageMenu;
    public TutorialPopUp TutorialPrompt;
    public TutorialPopUp StageMenuPopup;
    private bool isTransitioning;
    public string TutorialSceneName = "Tutorial";

    public void Awake()
    {
        StageMenu.SetActive(false);
        GameObject audioObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioObj != null)
        {
            audioManager = audioObj.GetComponent<AudioManager>();
            Debug.Log("found");
        }
        else
        {
            Debug.LogError("AudioManager not found in scene!");
        }
            
    }

    public void Start()
    {
        AudioManager.instance.playMainMenuBGM();
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        audioManager.playClickSFX();
        TutorialPrompt.ShowPopUp();
    }

    public void TutorialYes()
    {
        if (isTransitioning)
        return;

        isTransitioning = true;
        audioManager.playYesSFX();
        StartCoroutine(TutorialLevel());
    }

    public void TutorialNo()
    {
        audioManager.playClickSFX();
        TutorialPrompt.HidePopUp();
        StageMenuPopup.ShowPopUp();
    }

    public void Back()
    {
        audioManager.playClickSFX();
        StageMenuPopup.HidePopUp();
    }

    public void QuitGame()
    {
        audioManager.playClickSFX();
        Application.Quit();
    }

    IEnumerator TutorialLevel()
    {
        yield return StartCoroutine(Transition.Instance.PlayTransition());

        SceneManager.LoadScene(TutorialSceneName);

        yield return StartCoroutine(Transition.Instance.EndTransition());
    }
}
