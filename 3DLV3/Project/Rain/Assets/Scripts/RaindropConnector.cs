using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RaindropConnector : MonoBehaviour
{
    [Header("線の設定")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    [Header("田の形を判定する雨粒")]
    [Tooltip("0:左上 1:上中央 2:右上 3:右中央 4:右下 5:下中央 6:左下 7:左中央 8:中央")]
    [SerializeField] private GameObject[] shapeRaindrops = new GameObject[9];

    [Header("採点設定")]
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private float positionTolerance = 0.5f;

    // 接続した線を記録するクラス
    private class Connection
    {
        public GameObject start;
        public GameObject end;
        public LineRenderer line;
    }

    // 接続一覧
    private List<Connection> connections = new List<Connection>();

    // 田を作るために必要な接続
    private int[,] requiredConnections =
    {
        { 0, 1 },
        { 1, 2 },
        { 2, 3 },
        { 3, 4 },
        { 4, 5 },
        { 5, 6 },
        { 6, 7 },
        { 7, 0 },
        { 1, 8 },
        { 8, 5 },
        { 7, 8 },
        { 8, 3 }
    };


    // =====================================
    // 雨粒同士を接続する
    // =====================================

    public void ConnectRaindrops(GameObject start, GameObject end)
    {
        if (start == null || end == null)
            return;

        if (start == end)
            return;

        // すでに接続されているか確認
        foreach (Connection connection in connections)
        {
            if (
                (connection.start == start && connection.end == end) ||
                (connection.start == end && connection.end == start)
            )
            {
                return;
            }
        }

        // 線を作成
        GameObject lineObject = new GameObject("ConnectionLine");

        LineRenderer line = lineObject.AddComponent<LineRenderer>();

        line.positionCount = 2;
        line.SetPosition(0, start.transform.position);
        line.SetPosition(1, end.transform.position);

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        if (lineMaterial != null)
        {
            line.material = lineMaterial;
        }

        // 接続情報を保存
        Connection newConnection = new Connection();

        newConnection.start = start;
        newConnection.end = end;
        newConnection.line = line;

        connections.Add(newConnection);

        Debug.Log("雨粒を接続しました！");
    }


    // =====================================
    // 採点
    // =====================================

    public void CalculateScore()
    {
        // 雨粒が9個設定されているか確認
        if (shapeRaindrops == null || shapeRaindrops.Length < 9)
        {
            Debug.LogError("Shape Raindropsに9個の雨粒を設定してください。");
            return;
        }

        // 雨粒がすべて設定されているか確認
        for (int i = 0; i < 9; i++)
        {
            if (shapeRaindrops[i] == null)
            {
                Debug.LogError("Shape Raindropsの " + i + " 番目が未設定です。");
                return;
            }
        }

        // 各項目の点数を計算
        int shapeScore = CalculateShapeScore();
        int connectionScore = CalculateConnectionScore();
        int balanceScore = CalculateBalanceScore();

        // 合計点
        int totalScore = shapeScore + connectionScore + balanceScore;

        // ランク
        string rank = GetRank(totalScore);

        // 結果表示
        string result =
            "SCORE : " + totalScore + " / 100\n\n" +
            "形の一致度 : " + shapeScore + " / 50\n" +
            "接続の正確さ : " + connectionScore + " / 30\n" +
            "バランス : " + balanceScore + " / 20\n\n" +
            "RANK : " + rank;

        ShowResult(result);

        Debug.Log(result);

        // 採点後に次のステージボタンを表示
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.ShowNextStageButton();
        }
    }


    // =====================================
    // 形の一致度（50点）
    // =====================================

    private int CalculateShapeScore()
    {
        int correctLines = 0;

        // 横3列の判定
        if (IsAlignedHorizontally(0, 1, 2))
            correctLines++;

        if (IsAlignedHorizontally(7, 8, 3))
            correctLines++;

        if (IsAlignedHorizontally(6, 5, 4))
            correctLines++;

        // 縦3列の判定
        if (IsAlignedVertically(0, 7, 6))
            correctLines++;

        if (IsAlignedVertically(1, 8, 5))
            correctLines++;

        if (IsAlignedVertically(2, 3, 4))
            correctLines++;

        // 6本の線で50点
        float score = (correctLines / 6f) * 50f;

        return Mathf.RoundToInt(score);
    }


    // 横方向に並んでいるか
    private bool IsAlignedHorizontally(int a, int b, int c)
    {
        float y1 = shapeRaindrops[a].transform.position.y;
        float y2 = shapeRaindrops[b].transform.position.y;
        float y3 = shapeRaindrops[c].transform.position.y;

        float averageY = (y1 + y2 + y3) / 3f;

        return
            Mathf.Abs(y1 - averageY) <= positionTolerance &&
            Mathf.Abs(y2 - averageY) <= positionTolerance &&
            Mathf.Abs(y3 - averageY) <= positionTolerance;
    }


    // 縦方向に並んでいるか
    private bool IsAlignedVertically(int a, int b, int c)
    {
        float x1 = shapeRaindrops[a].transform.position.x;
        float x2 = shapeRaindrops[b].transform.position.x;
        float x3 = shapeRaindrops[c].transform.position.x;

        float averageX = (x1 + x2 + x3) / 3f;

        return
            Mathf.Abs(x1 - averageX) <= positionTolerance &&
            Mathf.Abs(x2 - averageX) <= positionTolerance &&
            Mathf.Abs(x3 - averageX) <= positionTolerance;
    }


    // =====================================
    // 接続の正確さ（30点）
    // =====================================

    private int CalculateConnectionScore()
    {
        int correctConnections = 0;

        int requiredCount = requiredConnections.GetLength(0);

        for (int i = 0; i < requiredCount; i++)
        {
            int startIndex = requiredConnections[i, 0];
            int endIndex = requiredConnections[i, 1];

            GameObject requiredStart = shapeRaindrops[startIndex];
            GameObject requiredEnd = shapeRaindrops[endIndex];

            foreach (Connection connection in connections)
            {
                bool matched =
                    (connection.start == requiredStart &&
                     connection.end == requiredEnd) ||
                    (connection.start == requiredEnd &&
                     connection.end == requiredStart);

                if (matched)
                {
                    correctConnections++;
                    break;
                }
            }
        }

        float score = (correctConnections / (float)requiredCount) * 30f;

        return Mathf.RoundToInt(score);
    }


    // =====================================
    // バランス（20点）
    // =====================================

    private int CalculateBalanceScore()
    {
        // 横方向の間隔
        float topSpacing = Mathf.Abs(
            shapeRaindrops[0].transform.position.x -
            shapeRaindrops[2].transform.position.x
        );

        float middleSpacing = Mathf.Abs(
            shapeRaindrops[7].transform.position.x -
            shapeRaindrops[3].transform.position.x
        );

        float bottomSpacing = Mathf.Abs(
            shapeRaindrops[6].transform.position.x -
            shapeRaindrops[4].transform.position.x
        );

        // 縦方向の間隔
        float leftSpacing = Mathf.Abs(
            shapeRaindrops[0].transform.position.y -
            shapeRaindrops[6].transform.position.y
        );

        float centerSpacing = Mathf.Abs(
            shapeRaindrops[1].transform.position.y -
            shapeRaindrops[5].transform.position.y
        );

        float rightSpacing = Mathf.Abs(
            shapeRaindrops[2].transform.position.y -
            shapeRaindrops[4].transform.position.y
        );

        // 平均間隔
        float averageHorizontal =
            (topSpacing + middleSpacing + bottomSpacing) / 3f;

        float averageVertical =
            (leftSpacing + centerSpacing + rightSpacing) / 3f;

        // 間隔のばらつき
        float horizontalDifference =
            Mathf.Abs(topSpacing - averageHorizontal) +
            Mathf.Abs(middleSpacing - averageHorizontal) +
            Mathf.Abs(bottomSpacing - averageHorizontal);

        float verticalDifference =
            Mathf.Abs(leftSpacing - averageVertical) +
            Mathf.Abs(centerSpacing - averageVertical) +
            Mathf.Abs(rightSpacing - averageVertical);

        float totalDifference =
            horizontalDifference + verticalDifference;

        // ばらつきが小さいほど高得点
        float score = Mathf.Clamp(
            20f - totalDifference * 2f,
            0f,
            20f
        );

        return Mathf.RoundToInt(score);
    }


    // =====================================
    // ランク判定
    // =====================================

    private string GetRank(int score)
    {
        if (score >= 90)
            return "S";

        if (score >= 80)
            return "A";

        if (score >= 60)
            return "B";

        if (score >= 40)
            return "C";

        return "D";
    }


    // =====================================
    // 結果表示
    // =====================================

    private void ShowResult(string result)
    {
        if (resultText != null)
        {
            resultText.text = result;
        }
        else
        {
            Debug.LogWarning("Result Textが設定されていません。");
        }
    }
}