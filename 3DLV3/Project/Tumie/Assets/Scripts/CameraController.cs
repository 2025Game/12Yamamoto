using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("カメラ切り替え速度")]
    [SerializeField] private float changeSpeed = 5f;

    [Header("カメラ1：テトリス風")]
    [SerializeField] private Vector3 normalPosition = new Vector3(0f, 5f, -14f);
    [SerializeField] private Vector3 normalRotation = new Vector3(0f, 0f, 0f);

    [Header("カメラ2：真上")]
    [SerializeField] private Vector3 topPosition = new Vector3(0f, 16f, 0f);
    [SerializeField] private Vector3 topRotation = new Vector3(90f, 0f, 0f);

    [Header("カメラ3：斜め")]
    [SerializeField] private Vector3 diagonalPosition = new Vector3(0f, 12f, -12f);
    [SerializeField] private Vector3 diagonalRotation = new Vector3(45f, 0f, 0f);

    private int currentMode = 0;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        targetPosition = normalPosition;
        targetRotation = Quaternion.Euler(normalRotation);

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeCamera();
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * changeSpeed
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * changeSpeed
        );
    }

    void ChangeCamera()
    {
        currentMode++;

        if (currentMode > 2)
            currentMode = 0;

        switch (currentMode)
        {
            case 0:
                targetPosition = normalPosition;
                targetRotation = Quaternion.Euler(normalRotation);
                break;

            case 1:
                targetPosition = topPosition;
                targetRotation = Quaternion.Euler(topRotation);
                break;

            case 2:
                targetPosition = diagonalPosition;
                targetRotation = Quaternion.Euler(diagonalRotation);
                break;
        }
    }
}