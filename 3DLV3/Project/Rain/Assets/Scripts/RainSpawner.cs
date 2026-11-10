using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    [Header("長い雨粒")]
    [SerializeField] private GameObject rainPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("落下設定")]
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float moveSpeed = 3f;

    [Header("移動範囲")]
    [SerializeField] private float minX = -4f;
    [SerializeField] private float maxX = 4f;

    [Header("画面下の位置")]
    [SerializeField] private float bottomY = -5f;

    private GameObject currentRain;

    private void Update()
    {
        // Qキーで長い雨粒を追加
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnRain();
        }

        if (currentRain == null)
        {
            return;
        }

        MoveRain();
        FallRain();
    }

    // 長い雨粒を生成
    private void SpawnRain()
    {
        // すでに雨粒が落下中なら追加しない
        if (currentRain != null)
        {
            Debug.Log("現在、雨粒が落下中です。");
            return;
        }

        if (rainPrefab == null || spawnPoint == null)
        {
            Debug.LogError("雨粒PrefabまたはSpawnPointが未設定です。");
            return;
        }

        currentRain = Instantiate(
            rainPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Debug.Log("長い雨粒を追加しました。");
    }

    // 左右に操作
    private void MoveRain()
    {
        float input = Input.GetAxisRaw("Horizontal");

        Vector3 position = currentRain.transform.position;

        position.x += input * moveSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, minX, maxX);

        currentRain.transform.position = position;
    }

    // 下に落下
    private void FallRain()
    {
        currentRain.transform.position +=
            Vector3.down * fallSpeed * Time.deltaTime;

        if (currentRain.transform.position.y <= bottomY)
        {
            Destroy(currentRain);

            currentRain = null;

            Debug.Log("長い雨粒が消えました。Qキーで次を追加できます。");
        }
    }
}