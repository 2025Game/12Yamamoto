using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BlockController : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("移動範囲")]
    [SerializeField] private float limitX = 3.5f;
    [SerializeField] private float limitZ = 3.5f;

    [Header("回転角度")]
    [SerializeField] private float rotateAngle = 90f;

    [Header("落下速度")]
    [SerializeField] private float dropSpeed = 8f;

    private Rigidbody rb;

    private bool isLanded = false;
    private bool isDropping = false;

    private float moveX;
    private float moveZ;

    private int pendingRotationY = 0;
    private int pendingRotationX = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isLanded || isDropping) return;

        moveX = 0f;
        moveZ = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;

        if (Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Input.GetKey(KeyCode.UpArrow))
            moveZ = 1f;

        if (Input.GetKey(KeyCode.DownArrow))
            moveZ = -1f;

        // Y軸回転
        if (Input.GetKeyDown(KeyCode.A))
            pendingRotationY = 1;

        if (Input.GetKeyDown(KeyCode.D))
            pendingRotationY = -1;

        // X軸回転
        if (Input.GetKeyDown(KeyCode.Q))
            pendingRotationX = 1;

        if (Input.GetKeyDown(KeyCode.E))
            pendingRotationX = -1;

        // 即座に落下
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDropping = true;
            rb.linearVelocity = Vector3.down * dropSpeed;
        }
    }

    void FixedUpdate()
    {
        if (isLanded || isDropping) return;

        Vector3 movement = new Vector3(
            moveX,
            0f,
            moveZ
        ).normalized * moveSpeed * Time.fixedDeltaTime;

        Vector3 nextPosition = rb.position + movement;

        nextPosition.x = Mathf.Clamp(nextPosition.x, -limitX, limitX);
        nextPosition.z = Mathf.Clamp(nextPosition.z, -limitZ, limitZ);

        rb.MovePosition(nextPosition);

        // 2方向の回転
        if (pendingRotationX != 0 || pendingRotationY != 0)
        {
            Quaternion rotation = Quaternion.Euler(
                rotateAngle * pendingRotationX,
                rotateAngle * pendingRotationY,
                0f
            );

            rb.MoveRotation(rb.rotation * rotation);

            pendingRotationX = 0;
            pendingRotationY = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Block"))
        {
            isLanded = true;
            isDropping = false;

            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }
}