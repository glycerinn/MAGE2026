using UnityEngine;

public class CameraParallaxMenu : MonoBehaviour
{
    [Header("Mouse Parallax Settings")]
    public float moveAmount = 0.3f;     // Jarak geser posisi kamera akibat mouse
    public float rotateAmount = 2.0f;   // Derajat putaran kamera akibat mouse
    public float smoothSpeed = 3.0f;    // Seberapa mulus pergerakannya

    [Header("Idle Auto-Move Settings")]
    public float idleSwayAmount = 0.1f; // Jarak gerak mandiri saat mouse diam
    public float idleSwaySpeed = 1.0f;  // Kecepatan ayunan mandiri

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Menyimpan posisi dan rotasi awal kamera sebagai titik pusat
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // 1. Membaca posisi mouse dan mengubahnya ke rentang -1 (kiri/bawah) hingga 1 (kanan/atas)
        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;
        
        // Membatasi nilai agar kamera tidak terpental jika mouse keluar dari layar game
        mouseX = Mathf.Clamp(mouseX, -1f, 1f);
        mouseY = Mathf.Clamp(mouseY, -1f, 1f);

        // 2. Menghitung efek gerak sendiri (Sway) menggunakan gelombang melingkar
        float swayX = Mathf.Sin(Time.time * idleSwaySpeed) * idleSwayAmount;
        float swayY = Mathf.Cos(Time.time * idleSwaySpeed * 0.9f) * idleSwayAmount;

        // 3. Menghitung target posisi dan rotasi akhir (Gabungan Mouse + Gerak Sendiri)
        Vector3 targetPos = startPosition + new Vector3(mouseX * moveAmount + swayX, mouseY * moveAmount + swayY, 0f);
        Quaternion targetRot = startRotation * Quaternion.Euler(-mouseY * rotateAmount, mouseX * rotateAmount, 0f);

        // 4. Menerapkan posisi dan rotasi tersebut ke kamera dengan interpolasi (Lerp) agar sangat mulus
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}