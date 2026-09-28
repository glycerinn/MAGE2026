using UnityEngine;

public class SatelliteMinigame : MonoBehaviour
{
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

    [Header("Input")]
    public KeyCode rotateLeft = KeyCode.A;
    public KeyCode rotateRight = KeyCode.D;
    public KeyCode checkAlignment = KeyCode.Space;

    [Header("Win Condition")]
    public int winAmount = 3;

    private float lineAngle;
    private float targetAngle;
    private int completedRounds;
    private bool hasWon;

    void Start()
    {
        completedRounds = 0;
        hasWon = false;

        lineAngle = 0f;
        linePivot.localRotation = Quaternion.identity;
        targetPivot.localRotation = Quaternion.identity;

        GenerateNewTarget();
    }

    void Update()
    {
        if (hasWon)
            return;

        RotateLine();

        if (Input.GetKeyDown(checkAlignment))
        {
            CheckAlignment();
        }
    }

    void RotateLine()
    {
        float direction = 0f;

        if (Input.GetKey(rotateLeft))
        {
            direction += 1f;
        }

        if (Input.GetKey(rotateRight))
        {
            direction -= 1f;
        }

        if (direction == 0f)
            return;

        lineAngle += direction * rotationSpeed * Time.deltaTime;
        lineAngle = Mathf.Clamp(lineAngle, targetMinAngle, targetMaxAngle);

        linePivot.localRotation = Quaternion.Euler(0f, 0f, lineAngle);
    }

    void CheckAlignment()
    {
        float difference = Mathf.Abs(
            Mathf.DeltaAngle(lineAngle, targetAngle)
        );

        if (difference <= acceptableAngle)
        {
            completedRounds++;

            Debug.Log(
                "Satellite round complete! " +
                completedRounds + "/" + winAmount
            );

            if (completedRounds >= winAmount)
            {
                hasWon = true;

                Debug.Log("SATELLITE MINIGAME WON!");

                StageManager.Instance.MinigameWon("Satellite");
                return;
            }

            GenerateNewTarget();
        }
    }

    void GenerateNewTarget()
    {
        targetAngle = Random.Range(
            targetMinAngle,
            targetMaxAngle
        );

        targetPivot.localRotation = Quaternion.Euler(
            0f,
            0f,
            targetAngle
        );
    }
}