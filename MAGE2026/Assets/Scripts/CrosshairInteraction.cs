
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrosshairInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public float maxDistance = 10f;
    public LayerMask monitorUILayer = ~0;

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            ClickButton();
    }

    void ClickButton()
    {
        if (mainCamera == null || EventSystem.current == null)
            return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponentInParent<Button>();

            if (button == null || !button.interactable)
                continue;

            if (Vector3.Distance(mainCamera.transform.position, result.worldPosition) > maxDistance)
                continue;

            button.onClick.Invoke();
            return;
        }
    }
}
