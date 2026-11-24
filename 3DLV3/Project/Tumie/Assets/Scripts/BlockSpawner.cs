using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("生成するブロック")]
    [SerializeField] private GameObject blockPrefab;

    [Header("生成位置")]
    [SerializeField] private float spawnHeight = 6f;

    void Start()
    {
        SpawnBlock();
    }

    void SpawnBlock()
    {
        Vector3 spawnPosition = new Vector3(
            0f,
            spawnHeight,
            0f
        );

        Instantiate(
            blockPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}