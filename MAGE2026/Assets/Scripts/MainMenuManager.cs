using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // public GameObject Credits;
    // public LevelLoader levelLoader;
    // private AudioManager audioManager;
    private int sceneToContinue;
    public GameObject StageMenu;

    public void Awake()
    {
        StageMenu.SetActive(false);
        // GameObject audioObj = GameObject.FindGameObjectWithTag("AudioManager");

        // if (audioObj != null)
        // {
        //     audioManager = audioObj.GetComponent<AudioManager>();
        //     Debug.Log("found");
        // }
        // else
        // {
        //     Debug.LogError("AudioManager not found in scene!");
        // }
            
    } 

    public void Start()
    {
        // AudioManager.instance.playMainMenuBGM();
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        // StartCoroutine(levelLoader.PlayTutorialTransition());
        // audioManager.playButtonSFX();
        StageMenu.SetActive(true);
    }

    public void Back()
    {
        StageMenu.SetActive(false);
    }


    public void QuitGame()
    {
        // if(audioManager != null)
        // {
        //     audioManager.playButtonSFX();
        //     Debug.Log("click2");
        // }
        
        Application.Quit();
    }

    // public void ContinueButton()
    // {
    //     sceneToContinue = PlayerPrefs.GetInt("SavedScene");
    //     if(sceneToContinue != 0)
    //     {
    //         StartCoroutine(levelLoader.PlayContinueTransition(sceneToContinue));
    //     }
    //     else
    //     {
    //         return;
    //     }

    // }
}