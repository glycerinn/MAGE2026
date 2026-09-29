using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButtons : MonoBehaviour
{
    public int stageNumber;
    public string gameSceneName = "SampleScene";

    public void SelectStage()
    {
        PlayerPrefs.SetInt("SelectedStage", stageNumber);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }
}