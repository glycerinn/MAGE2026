using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public Sprite image;
        [TextArea(3, 8)]
        public string text;
    }

    [Header("Tutorial Pages")]
    public TutorialPage[] pages;

    [Header("Display")]
    public Image tutorialImage;
    public TMP_Text tutorialText;

    [Header("Navigation")]
    public GameObject previousButton;
    public GameObject nextButton;
    public GameObject continueButton;

    [Header("Game Scene")]
    public string gameSceneName = "SampleScene";

    private int currentPage;

    void Start()
    {
        currentPage = 0;

        if (pages == null || pages.Length == 0)
        {
            Debug.LogError(
                "TutorialManager: No tutorial pages assigned."
            );
            return;
        }

        ShowPage();
    }

    public void NextPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        if (currentPage >= pages.Length - 1)
            return;

        currentPage++;

        ShowPage();
    }

    public void PreviousPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        if (currentPage <= 0)
            return;

        currentPage--;

        ShowPage();
    }

    void ShowPage()
    {
        TutorialPage page =
            pages[currentPage];

        if (tutorialImage != null)
        {
            tutorialImage.sprite =
                page.image;
        }

        if (tutorialText != null)
        {
            tutorialText.text =
                page.text;
        }

        UpdateButtons();
    }

    void UpdateButtons()
    {
        bool isFirstPage =
            currentPage == 0;

        bool isLastPage =
            currentPage == pages.Length - 1;

        if (previousButton != null)
        {
            previousButton.SetActive(
                !isFirstPage
            );
        }

        if (nextButton != null)
        {
            nextButton.SetActive(
                !isLastPage
            );
        }

        if (continueButton != null)
        {
            continueButton.SetActive(
                isLastPage
            );
        }
    }

    public void ContinueToGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError(
                "TutorialManager: Game scene name is empty."
            );
            return;
        }

        SceneManager.LoadScene(
            gameSceneName
        );
    }
}