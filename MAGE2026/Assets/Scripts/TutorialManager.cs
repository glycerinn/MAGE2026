
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Slide Animation")]
    public float slideDistance = 600f;
    public float slideDuration = 0.25f;

    [Header("Game Scene")]
    public string gameSceneName = "SampleScene";

    private int currentPage;
    private bool isTransitioning;
    private RectTransform imageRect;
    private Vector2 imageStartPosition;
    private bool isAnimating;
    private AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentPage = 0;

        if (pages == null || pages.Length == 0)
        {
            Debug.LogError("TutorialManager: No tutorial pages assigned.");
            return;
        }

        if (tutorialImage != null)
        {
            imageRect = tutorialImage.rectTransform;
            imageStartPosition = imageRect.anchoredPosition;
        }

        ShowPage();
    }

    public void NextPage()
    {
        audioManager.playClickSFX();
        if (isAnimating || pages == null || currentPage >= pages.Length - 1)
            return;

        currentPage++;
        ShowPage(-1);
    }

    public void PreviousPage()
    {
        audioManager.playClickSFX();
        if (isAnimating || pages == null || currentPage <= 0)
            return;

        currentPage--;
        ShowPage(1);
    }

    void ShowPage(int slideDirection = 0)
    {
        TutorialPage page = pages[currentPage];

        if (tutorialImage != null)
        {
            if (slideDirection == 0 || imageRect == null)
                tutorialImage.sprite = page.image;
            else
                AnimateImage(page.image, slideDirection);
        }

        if (tutorialText != null)
            tutorialText.text = page.text;

        UpdateButtons();
    }

    void AnimateImage(Sprite newSprite, int direction)
    {
        isAnimating = true;

        Vector2 center = imageStartPosition;
        Vector2 exitPosition = center + Vector2.right * direction * slideDistance;
        Vector2 enterPosition = center - Vector2.right * direction * slideDistance;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(imageRect.DOAnchorPos(exitPosition, slideDuration).SetEase(Ease.InCubic));
        sequence.AppendCallback(() =>
        {
            tutorialImage.sprite = newSprite;
            imageRect.anchoredPosition = enterPosition;
        });
        sequence.Append(imageRect.DOAnchorPos(center, slideDuration).SetEase(Ease.OutCubic));
        sequence.OnComplete(() =>
        {
            isAnimating = false;
        });
    }

    void UpdateButtons()
    {
        bool isFirstPage = currentPage == 0;
        bool isLastPage = currentPage == pages.Length - 1;

        if (previousButton != null)
            previousButton.SetActive(!isFirstPage);

        if (nextButton != null)
            nextButton.SetActive(!isLastPage);

        if (continueButton != null)
            continueButton.SetActive(isLastPage);
    }

    public void ContinueToGame()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("TutorialManager: Game scene name is empty.");
            return;
        }

        StartCoroutine(GameLevel());
    }

    IEnumerator GameLevel()
    {
        yield return StartCoroutine(Transition.Instance.PlayTransition());

        SceneManager.LoadScene(gameSceneName);

        yield return StartCoroutine(Transition.Instance.EndTransition());
    }
}
