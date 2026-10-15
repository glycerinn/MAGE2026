using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButtons : MonoBehaviour
{
    public int stageNumber;
    private bool isTransitioning;
    public string gameSceneName = "SampleScene";

    public void SelectStage()
    {
        if (isTransitioning)
        return;

        isTransitioning = true;
        PlayerPrefs.SetInt("SelectedStage", stageNumber);
        PlayerPrefs.Save();

        StartCoroutine(GameLevel());
    }

    IEnumerator GameLevel()
    {
        yield return StartCoroutine(Transition.Instance.PlayTransition());

        SceneManager.LoadScene(gameSceneName);

        yield return StartCoroutine(Transition.Instance.EndTransition());
    }
}