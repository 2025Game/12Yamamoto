
using UnityEngine;

public class RaindropClick : MonoBehaviour
{
    [Header("選択中の色")]
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("移動設定")]
    [SerializeField] private float moveSensitivity = 0.2f;

    private Renderer objectRenderer;
    private Color originalColor;
    private Camera mainCamera;

    // 現在選択している雨粒
    private static RaindropClick currentSelected;

    // ドラッグ中かどうか
    private bool isDragging;

    // ドラッグ開始時の情報
    private Vector3 dragStartMousePosition;
    private Vector3 dragStartObjectPosition;

    // 奥行きを固定する
    private float fixedZ;

    private void Start()
    {
        // 必要なコンポーネントを取得
        objectRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;

        // 元の色を保存
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // 最初のZ座標を保存
        fixedZ = transform.position.z;
    }

    private void OnMouseDown()
    {
        if (objectRenderer == null || mainCamera == null)
        {
            return;
        }

        // すでに別の雨粒が選択されている場合は解除
        if (currentSelected != null &&
            currentSelected != this)
        {
            currentSelected.Deselect();
        }

        // この雨粒を選択状態にする
        currentSelected = this;

        // 選択中だけ黄色にする
        objectRenderer.material.color = selectedColor;

        Debug.Log("雨粒を選択：" + gameObject.name);

        // ドラッグ開始
        BeginDrag();
    }

    private void BeginDrag()
    {
        isDragging = true;

        // マウスの開始位置を保存
        dragStartMousePosition = Input.mousePosition;

        // 雨粒の開始位置を保存
        dragStartObjectPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null)
        {
            return;
        }

        // マウスの移動量を計算
        Vector3 mouseDelta =
            Input.mousePosition - dragStartMousePosition;

        // 雨粒の位置をスクリーン座標に変換
        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(
                dragStartObjectPosition
            );

        // 左右に移動
        screenPosition.x +=
            mouseDelta.x * moveSensitivity;

        // 上下に移動
        screenPosition.y +=
            mouseDelta.y * moveSensitivity;

        // ワールド座標に戻す
        Vector3 newPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        // 奥行き方向には動かさない
        newPosition.z = fixedZ;

        // 新しい位置を反映
        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        // マウスを離したらドラッグだけ終了
        // 選択状態と色は維持する
        isDragging = false;
    }

    // 選択を解除して元の色に戻す
    public void Deselect()
    {
        // 元の色に戻す
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }

        // ドラッグを終了
        isDragging = false;

        // 選択状態を解除
        if (currentSelected == this)
        {
            currentSelected = null;
        }

        Debug.Log("雨粒の選択を解除：" + gameObject.name);
    }

    // 外部のスクリプトから現在の選択を解除する
    public static void DeselectCurrent()
    {
        if (currentSelected != null)
        {
            currentSelected.Deselect();
        }
    }

    // 現在選択されている雨粒を取得する
    public static RaindropClick GetCurrentSelected()
    {
        return currentSelected;
    }
}


