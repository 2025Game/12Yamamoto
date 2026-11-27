using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("生成するブロック")]
    [SerializeField] private GameObject[] blockPrefabs;

    [Header("生成位置")]
    [SerializeField] private float spawnHeight = 10f;

    void Start()
    {
        SpawnBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        if (blockPrefabs.Length == 0)
        {
            Debug.LogWarning("ブロックが登録されていません！");
            return;
        }

        int randomIndex = Random.Range(0, blockPrefabs.Length);

        GameObject selectedBlock = blockPrefabs[randomIndex];

        Vector3 spawnPosition = new Vector3(
            0f,
            spawnHeight,
            0f
        );

        Instantiate(
            selectedBlock,
            spawnPosition,
            Quaternion.identity
        );
    }
}