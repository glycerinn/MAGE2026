using UnityEngine;

public class SatelliteMinigame : MonoBehaviour, IMinigame
{
    public string MinigameName => "SatelliteMinigame";

    [Header("Line")]
    public Transform linePivot;
    public Transform line;
    public float rotationSpeed = 90f;

    [Header("Target")]
    public Transform targetPivot;
    public Transform targetBar;

    [Header("Angles")]
    public float targetMinAngle = -90f;
    public float targetMaxAngle = 90f;

    [Header("Alignment")]
    public float acceptableAngle = 5f;
    public float requiredHoldTime = 2f;

    [Header("Input")]
    public KeyCode rotateLeft = KeyCode.A;
    public KeyCode rotateRight = KeyCode.D;

    [Header("Clue")]
    public CluePopup cluePopup;

    [Header("Win Condition")]
    public int winAmount = 3;

    private float lineAngle;
    private float targetAngle;
    private int completedRounds;
    private float alignmentTimer;
    private bool hasWon;

    void Start()
    {
        ResetMinigame();
    }

    void Update()
    {
        if (hasWon)
            return;

        RotateLine();
        CheckAlignment();
    }

    void RotateLine()
    {
        float direction = 0f;

        if (Input.GetKey(rotateLeft))
            direction += 1f;

        if (Input.GetKey(rotateRight))
            direction -= 1f;

        if (direction == 0f)
            return;

        lineAngle += direction * rotationSpeed * Time.deltaTime;

        lineAngle = Mathf.Clamp(
            lineAngle,
            targetMinAngle,
            targetMaxAngle
        );

        linePivot.localRotation =
            Quaternion.Euler(0f, 0f, lineAngle);
    }

    void CheckAlignment()
    {
        float difference = Mathf.Abs(
            Mathf.DeltaAngle(lineAngle, targetAngle)
        );

        if (difference <= acceptableAngle)
        {
            alignmentTimer += Time.deltaTime;

            if (alignmentTimer >= requiredHoldTime)
            {
                CompleteRound();
            }
        }
        else
        {
            alignmentTimer = 0f;
        }
    }

    void CompleteRound()
    {
        alignmentTimer = 0f;
        completedRounds++;

        Debug.Log(
            "Satellite round complete! " +
            completedRounds + "/" + winAmount
        );

        if (completedRounds >= winAmount)
        {
            hasWon = true;

            ReportDiagnosisClue();

            Debug.Log("SATELLITE MINIGAME WON!");

            StageManager.Instance.MinigameWon(MinigameName);
            return;
        }

        GenerateNewTarget();
    }

    void GenerateNewTarget()
    {
        targetAngle = Random.Range(
            targetMinAngle,
            targetMaxAngle
        );

        targetPivot.localRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        alignmentTimer = 0f;
    }

    void ReportDiagnosisClue()
    {
        if (DiagnosisManager.Instance == null)
            return;

        string satelliteClue =
            DiagnosisManager.Instance.GetRequiredObservation(
                "Satellite"
            );

        if (string.IsNullOrEmpty(satelliteClue))
        {
            Debug.LogError(
                "No Satellite diagnosis clue found."
            );
            return;
        }

        DiagnosisManager.Instance.SetObservation(
            "Satellite",
            satelliteClue
        );

        if (cluePopup != null)
        {
            cluePopup.ShowClue(
                "Satellite",
                satelliteClue
            );
        }
    }

    public void ResetMinigame()
    {
        completedRounds = 0;
        hasWon = false;
        alignmentTimer = 0f;
        lineAngle = 0f;

        if (cluePopup != null)
            cluePopup.HideClue();

        linePivot.localRotation =
            Quaternion.identity;

        targetPivot.localRotation =
            Quaternion.identity;

        GenerateNewTarget();
    }
}