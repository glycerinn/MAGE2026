using UnityEngine;
using UnityEngine.UI; // Wajib ditambahkan untuk mengakses UI Image

public class ObjectSlider : MonoBehaviour
{
    [Header("Handle")]
    public Transform handle;

    [Header("Water Fill (Metode Health Bar)")]
    public Image waterFillImage; // Drag komponen Image objek "Fill" ke sini

    [Header("Slider Range")]
    public float bottomY = -2f;
    public float topY = 2f;
    
    [Header("Animation Settings")]
    public float smoothSpeed = 5f;

    public int Value { get; private set; }
    
    // Target untuk posisi panah dan isi air
    private float targetY;
    private float targetFillAmount; 

    private SliderMinigame minigame;

    void Awake()
    {
        minigame = GetComponentInParent<SliderMinigame>();
        
        // Inisialisasi awal agar tidak lompat
        targetY = handle.localPosition.y;
        if (waterFillImage != null)
        {
            targetFillAmount = waterFillImage.fillAmount;
        }
    }

    void Update()
    {
        // 1. Animasi untuk Handle (Panah)
        if (handle != null)
        {
            Vector3 currentPos = handle.localPosition;
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, Time.deltaTime * smoothSpeed);
            handle.localPosition = currentPos;
        }

        // 2. Animasi untuk Water Fill dengan Fill Amount (Gaya Health Bar)
        if (waterFillImage != null)
        {
            // Nilai fillAmount berkisar dari 0.0 (kosong) hingga 1.0 (penuh)
            waterFillImage.fillAmount = Mathf.Lerp(waterFillImage.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
        }
    }

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, 0, 100);

        float normalized = Value / 100f;
        
        targetY = Mathf.Lerp(bottomY, topY, normalized);

        // Menentukan target pengisian untuk Image
        if (waterFillImage != null)
        {
             targetFillAmount = normalized;
        }
    }

    public void Select()
    {
        minigame.SelectSlider(this);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 bottom = transform.position;
        bottom.y += bottomY;

        Vector3 top = transform.position;
        top.y += topY;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bottom, 0.08f);
        Gizmos.DrawSphere(top, 0.08f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottom, top);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(bottom + Vector3.left * 0.15f, bottom + Vector3.right * 0.15f);
        Gizmos.DrawLine(top + Vector3.left * 0.15f, top + Vector3.right * 0.15f);
    }
}