using System.Collections;
using TMPro;
using UnityEngine;

public class HeadlineClue : MonoBehaviour
{
    public static HeadlineClue Instance;

    [Header("Text")]
    public TMP_Text clueText;

    [Header("2D Button")]
    public GameObject showClueButton;
    public Camera buttonCamera;

    [Header("2D Loading Slider")]
    public GameObject loadingSlider;
    public Transform sliderFill;
    public float loadingDuration = 3f;

    private string headlineValue;
    private Coroutine loadingCoroutine;
    private Vector3 sliderFillStartScale;
    private bool isLoading;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        Instance = this;

        if (sliderFill != null)
            sliderFillStartScale = sliderFill.localScale;

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy || isLoading || showClueButton == null)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        Camera cam = buttonCamera != null ? buttonCamera : Camera.main;

        if (cam == null)
            return;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = cam.ScreenToWorldPoint(mousePosition);
        Vector2 clickPosition = new Vector2(worldPosition.x, worldPosition.y);

        Collider2D hit = Physics2D.OverlapPoint(clickPosition);

        if (hit != null &&
            (hit.gameObject == showClueButton ||
             hit.transform.IsChildOf(showClueButton.transform)))
        {
            BeginLoading();
        }
    }

    public void ShowForStage(int stage)
    {
        if (stage != 5)
        {
            Hide();
            return;
        }

        if (clueText == null)
        {
            Debug.LogError("HeadlineClue: Clue Text is not assigned.");
            return;
        }

        if (DiagnosisManager.Instance == null)
        {
            Debug.LogError("HeadlineClue: DiagnosisManager.Instance is null.");
            return;
        }

        headlineValue = DiagnosisManager.Instance.GetRequiredObservation("Headline");

        if (string.IsNullOrEmpty(headlineValue))
        {
            Debug.LogError("HeadlineClue: No Headline observation was found in the selected diagnosis rule.");
            return;
        }

        gameObject.SetActive(true);

        clueText.text = "";
        clueText.gameObject.SetActive(false);

        if (showClueButton != null)
            showClueButton.SetActive(true);

        if (loadingSlider != null)
            loadingSlider.SetActive(false);

        if (sliderFill != null)
        {
            sliderFill.localScale = new Vector3(
                0f,
                sliderFillStartScale.y,
                sliderFillStartScale.z
            );
        }

        isLoading = false;
    }

    public void BeginLoading()
    {
        if (isLoading)
            return;

        if (loadingSlider == null || sliderFill == null)
        {
            Debug.LogError("HeadlineClue: Loading Slider or Slider Fill is not assigned.");
            return;
        }

        isLoading = true;

        if (showClueButton != null)
            showClueButton.SetActive(false);

        loadingSlider.SetActive(true);

        sliderFill.localScale = new Vector3(
            0f,
            sliderFillStartScale.y,
            sliderFillStartScale.z
        );

        loadingCoroutine = StartCoroutine(LoadClue());
    }

    private IEnumerator LoadClue()
    {
        audioManager.playFix2SFX();
        float elapsed = 0f;
        float duration = Mathf.Max(loadingDuration, 0.01f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            sliderFill.localScale = new Vector3(
                sliderFillStartScale.x * progress,
                sliderFillStartScale.y,
                sliderFillStartScale.z
            );

            yield return null;
        }

        sliderFill.localScale = sliderFillStartScale;
        loadingSlider.SetActive(false);

        clueText.text = headlineValue;
        clueText.gameObject.SetActive(true);

        if (DiagnosisManager.Instance != null)
            DiagnosisManager.Instance.SetObservation("Headline", headlineValue);

        loadingCoroutine = null;
        Debug.Log("Stage 5 headline shown: " + headlineValue);
    }

    public void Hide()
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        isLoading = false;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}