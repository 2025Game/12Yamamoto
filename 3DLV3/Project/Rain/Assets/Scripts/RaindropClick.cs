using UnityEngine;

public class RaindropClick : MonoBehaviour
{
    [Header("選択設定")]
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("移動設定")]
    [SerializeField] private float moveSensitivity = 0.2f;

    private Renderer objectRenderer;
    private Color originalColor;

    private Camera mainCamera;

    private static RaindropClick currentSelected;

    private bool isDragging = false;

    private Vector3 dragStartMousePosition;
    private Vector3 dragStartObjectPosition;

    private float fixedZ;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        fixedZ = transform.position.z;
    }

    private void OnMouseDown()
    {
        if (objectRenderer == null || mainCamera == null)
        {
            return;
        }

        // すでに選択中の雨粒をクリックした場合
        if (currentSelected == this)
        {
            // 選択状態は維持し、ドラッグを開始する
            BeginDrag();
            return;
        }

        // 前の雨粒を元の色に戻す
        if (currentSelected != null)
        {
            currentSelected.Deselect();
        }

        // 今クリックした雨粒を選択する
        currentSelected = this;
        objectRenderer.material.color = selectedColor;

        Debug.Log("雨粒を選択：" + gameObject.name);

        // ドラッグ開始
        BeginDrag();
    }

    private void BeginDrag()
    {
        isDragging = true;

        dragStartMousePosition = Input.mousePosition;
        dragStartObjectPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null)
        {
            return;
        }

        Vector3 mouseDelta =
            Input.mousePosition - dragStartMousePosition;

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(dragStartObjectPosition);

        screenPosition.x += mouseDelta.x * moveSensitivity;
        screenPosition.y += mouseDelta.y * moveSensitivity;

        Vector3 newPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        // 奥行き方向は固定する
        newPosition.z = fixedZ;

        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Deselect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }

        if (currentSelected == this)
        {
            currentSelected = null;
        }

        isDragging = false;

        Debug.Log("雨粒の選択を解除：" + gameObject.name);
    }
}