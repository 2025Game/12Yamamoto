using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BlockController : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("�ړ��͈�")]
    [SerializeField] private float limitX = 3.5f;
    [SerializeField] private float limitZ = 3.5f;

    [Header("��]���x")]
    [SerializeField] private float rotateSpeed = 90f;

    [Header("�������x")]
    [SerializeField] private float dropSpeed = 20f;

    private Rigidbody rb;
    private bool isLanded = false;
    private bool isDropping = false;

    private float moveX;
    private float moveZ;
    private bool rotatePressed;

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

        if (Input.GetKeyDown(KeyCode.W))
            rotatePressed = true;

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

        nextPosition.x = Mathf.Clamp(
            nextPosition.x, -limitX, limitX
        );

        nextPosition.z = Mathf.Clamp(
            nextPosition.z, -limitZ, limitZ
        );

        rb.MovePosition(nextPosition);

        if (rotatePressed)
        {
            rb.MoveRotation(
                rb.rotation *
                Quaternion.Euler(0f, rotateSpeed, 0f)
            );

            rotatePressed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isLanded = true;
            isDropping = false;
        }
    }
}