
using UnityEngine;

public class RaindropClick : MonoBehaviour
{
    [Header("選択中の色")]
    [SerializeField] private Color selectedColor = Color.red;

    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("落下設定")]
    [SerializeField] private float rapidFallSpeed = 15f;

    private Renderer objectRenderer;
    private Color originalColor;
    private Rigidbody rb;

    // Wで追加された雨粒専用の色
    private Color customColor;
    private bool hasCustomColor = false;

    // 現在選択している雨粒
    private static RaindropClick currentSelected;

    // 現在操作できる雨粒
    private static RaindropClick currentFalling;

    private bool isStopped = false;
    private float moveDirection;

    private void OnEnable()
    {
        currentFalling = this;
    }

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;

            if (hasCustomColor)
            {
                originalColor = customColor;
                objectRenderer.material.color = customColor;
            }
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbodyがありません！");
            return;
        }

        rb.useGravity = true;
        rb.isKinematic = false;

        Debug.Log("雨粒の操作開始：" + gameObject.name);
    }

    private void Update()
    {
        if (isStopped || currentFalling != this)
            return;

        moveDirection = 0f;

        // Aキーで左移動
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f;
        }

        // Dキーで右移動
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f;
        }

        // Sキーで急速落下
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = -rapidFallSpeed;
            rb.linearVelocity = velocity;
        }

        // Spaceキーで停止
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopRaindrop();
        }
    }

    private void FixedUpdate()
    {
        if (isStopped || currentFalling != this)
            return;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 movement =
            Vector3.right * moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    // Wで追加された雨粒の色を設定
    public void SetNormalColor(Color color)
    {
        customColor = color;
        hasCustomColor = true;
        originalColor = color;

        if (objectRenderer != null)
        {
            objectRenderer.material.color = color;
        }
    }

    // 雨粒を停止する
    private void StopRaindrop()
    {
        isStopped = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        if (currentFalling == this)
        {
            currentFalling = null;
        }

        Debug.Log("雨粒を停止：" + gameObject.name);
    }

    // 雨粒を選択
    private void OnMouseDown()
    {
        if (objectRenderer == null)
            return;

        // 前の選択を解除
        if (currentSelected != null &&
            currentSelected != this)
        {
            currentSelected.Deselect();
        }

        currentSelected = this;

        // Wで追加した雨粒以外は赤色にする
        if (!hasCustomColor)
        {
            objectRenderer.material.color = selectedColor;
        }

        Debug.Log("雨粒を選択：" + gameObject.name);
    }

    // 選択を解除
    public void Deselect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }

        if (currentSelected == this)
        {
            currentSelected = null;
        }

        Debug.Log("雨粒の選択を解除：" + gameObject.name);
    }

    // 外部から選択解除
    public static void DeselectCurrent()
    {
        if (currentSelected != null)
        {
            currentSelected.Deselect();
        }
    }

    // 現在選択されている雨粒を取得
    public static RaindropClick GetCurrentSelected()
    {
        return currentSelected;
    }
}
