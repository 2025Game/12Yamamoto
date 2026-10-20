using UnityEngine;
using System.Collections.Generic;

public class RaindropConnector : MonoBehaviour
{
    [Header("線の設定")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    private GameObject firstRaindrop;

    // 接続した雨粒と線を記録するクラス
    private class Connection
    {
        public GameObject start;
        public GameObject end;
        public LineRenderer line;
    }

    // すべての接続を保存する
    private List<Connection> connections = new List<Connection>();

    private void Update()
    {
        // マウスの左クリックを検出する
        if (Input.GetMouseButtonDown(0))
        {
            CheckRaindropClick();
        }

        // 接続した線の位置を毎フレーム更新する
        UpdateConnections();
    }

    private void CheckRaindropClick()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            // 雨粒以外は無視する
            if (!clickedObject.name.StartsWith("Sphere"))
            {
                return;
            }

            // 1個目の雨粒を記録する
            if (firstRaindrop == null)
            {
                firstRaindrop = clickedObject;

                Debug.Log("接続元の雨粒：" + firstRaindrop.name);
            }
            else
            {
                // 同じ雨粒同士は接続しない
                if (firstRaindrop == clickedObject)
                {
                    return;
                }

                // 2個目の雨粒と接続する
                CreateConnection(firstRaindrop, clickedObject);

                // 次の接続に備えてリセットする
                firstRaindrop = null;
            }
        }
    }

    private void CreateConnection(GameObject start, GameObject end)
    {
        GameObject lineObject = new GameObject("RaindropLine");

        LineRenderer line = lineObject.AddComponent<LineRenderer>();

        line.positionCount = 2;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.startColor = Color.white;
        line.endColor = Color.white;

        line.useWorldSpace = true;

        if (lineMaterial != null)
        {
            line.material = lineMaterial;
        }
        else
        {
            line.material = new Material(
                Shader.Find("Sprites/Default")
            );
        }

        // 接続情報を保存する
        Connection connection = new Connection();

        connection.start = start;
        connection.end = end;
        connection.line = line;

        connections.Add(connection);

        // 作成直後に線の位置を更新する
        UpdateLine(connection);

        Debug.Log(
            "雨粒を接続：" + start.name + " → " + end.name
        );
    }

    private void UpdateConnections()
    {
        // すべての接続線を更新する
        foreach (Connection connection in connections)
        {
            UpdateLine(connection);
        }
    }

    private void UpdateLine(Connection connection)
    {
        if (connection.start == null ||
            connection.end == null ||
            connection.line == null)
        {
            return;
        }

        // 雨粒の現在位置に線の端を合わせる
        connection.line.SetPosition(
            0,
            connection.start.transform.position
        );

        connection.line.SetPosition(
            1,
            connection.end.transform.position
        );
    }
}