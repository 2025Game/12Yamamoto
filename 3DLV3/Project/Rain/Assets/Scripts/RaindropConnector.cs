using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RaindropConnector : MonoBehaviour
{
    [Header("線の設定")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    [Header("採点用の雨粒")]
    [SerializeField] private GameObject[] shapeRaindrops = new GameObject[9];

    [Header("口のCollider判定")]
    [SerializeField] private Collider topArea;
    [SerializeField] private Collider rightArea;
    [SerializeField] private Collider bottomArea;
    [SerializeField] private Collider leftArea;

    [Header("判定精度")]
    [SerializeField] private int samplesPerLine = 30;

    [Header("採点結果")]
    [SerializeField] private TMP_Text resultText;

    private class Connection
    {
        public GameObject start;
        public GameObject end;
        public LineRenderer line;
    }

    private List<Connection> connections = new List<Connection>();

    // 田に必要な接続
    private int[,] requiredConnections =
    {
        { 0, 1 }, { 1, 2 },
        { 2, 3 }, { 3, 4 },
        { 4, 5 }, { 5, 6 },
        { 6, 7 }, { 7, 0 },
        { 1, 8 }, { 8, 5 },
        { 7, 8 }, { 8, 3 }
    };

    // =====================================
    // 雨粒同士を接続
    // =====================================

    public void ConnectRaindrops(GameObject start, GameObject end)
    {
        if (start == null || end == null || start == end)
            return;

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

        GameObject lineObject = new GameObject("ConnectionLine");

        LineRenderer line = lineObject.AddComponent<LineRenderer>();

        line.useWorldSpace = true;
        line.positionCount = 2;

        line.SetPosition(0, start.transform.position);
        line.SetPosition(1, end.transform.position);

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        if (lineMaterial != null)
            line.material = lineMaterial;

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
        GameManager gameManager = FindObjectOfType<GameManager>();

        int currentStage = 1;

        if (gameManager != null)
            currentStage = gameManager.GetCurrentStage();

        if (currentStage == 1)
        {
            CalculateMouthScore();
        }
        else
        {
            CalculateOldScore();
        }
    }

    // =====================================
    // 口のCollider採点
    // =====================================

    private void CalculateMouthScore()
    {
        if (topArea == null ||
            rightArea == null ||
            bottomArea == null ||
            leftArea == null)
        {
            Debug.LogError("口のColliderが設定されていません！");
            ShowResult("Colliderが設定されていません！");
            return;
        }

        int score = 0;

        bool topPassed = CheckArea(topArea);
        bool rightPassed = CheckArea(rightArea);
        bool bottomPassed = CheckArea(bottomArea);
        bool leftPassed = CheckArea(leftArea);

        if (topPassed) score += 25;
        if (rightPassed) score += 25;
        if (bottomPassed) score += 25;
        if (leftPassed) score += 25;

        string result =
            "SCORE : " + score + " / 100\n\n" +
            "上側 : " + (topPassed ? "OK" : "未通過") + "\n" +
            "右側 : " + (rightPassed ? "OK" : "未通過") + "\n" +
            "下側 : " + (bottomPassed ? "OK" : "未通過") + "\n" +
            "左側 : " + (leftPassed ? "OK" : "未通過");

        ShowResult(result);

        Debug.Log(result);
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.ShowNextStageButton();
    }

    // =====================================
    // Colliderを線が通過したか調べる
    // =====================================

    private bool CheckArea(Collider targetArea)
    {
        foreach (Connection connection in connections)
        {
            if (connection.line == null)
                continue;

            LineRenderer line = connection.line;

            for (int i = 0; i < line.positionCount - 1; i++)
            {
                Vector3 start = line.GetPosition(i);
                Vector3 end = line.GetPosition(i + 1);

                for (int j = 0; j <= samplesPerLine; j++)
                {
                    float t = j / (float)samplesPerLine;

                    Vector3 point = Vector3.Lerp(start, end, t);

                    if (targetArea.bounds.Contains(point))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // =====================================
    // 以前の採点（ステージ2以降）
    // =====================================

    private void CalculateOldScore()
    {
        for (int i = 0; i < 9; i++)
        {
            if (shapeRaindrops[i] == null)
            {
                Debug.LogError(
                    "Shape Raindropsの " + i + " 番目が未設定です。"
                );
                return;
            }
        }

        int shapeScore = CalculateOldShapeScore();
        int connectionScore = CalculateOldConnectionScore();
        int balanceScore = CalculateOldBalanceScore();

        int totalScore = shapeScore + connectionScore + balanceScore;

        string result =
            "SCORE : " + totalScore + " / 100\n\n" +
            "形の一致度 : " + shapeScore + " / 50\n" +
            "接続の正確さ : " + connectionScore + " / 30\n" +
            "バランス : " + balanceScore + " / 20\n\n" +
            "RANK : " + GetRank(totalScore);

        ShowResult(result);

        Debug.Log(result);

        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
            gameManager.ShowNextStageButton();
    }

    private int CalculateOldShapeScore()
    {
        int correctLines = 0;

        if (IsAlignedHorizontally(0, 1, 2)) correctLines++;
        if (IsAlignedHorizontally(7, 8, 3)) correctLines++;
        if (IsAlignedHorizontally(6, 5, 4)) correctLines++;

        if (IsAlignedVertically(0, 7, 6)) correctLines++;
        if (IsAlignedVertically(1, 8, 5)) correctLines++;
        if (IsAlignedVertically(2, 3, 4)) correctLines++;

        return Mathf.RoundToInt((correctLines / 6f) * 50f);
    }

    private int CalculateOldConnectionScore()
    {
        int correctConnections = 0;

        int requiredCount = requiredConnections.GetLength(0);

        for (int i = 0; i < requiredCount; i++)
        {
            GameObject requiredStart =
                shapeRaindrops[requiredConnections[i, 0]];

            GameObject requiredEnd =
                shapeRaindrops[requiredConnections[i, 1]];

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

        return Mathf.RoundToInt(
            (correctConnections / (float)requiredCount) * 30f
        );
    }

    private int CalculateOldBalanceScore()
    {
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

        float averageHorizontal =
            (topSpacing + middleSpacing + bottomSpacing) / 3f;

        float averageVertical =
            (leftSpacing + centerSpacing + rightSpacing) / 3f;

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

        float score = Mathf.Clamp(
            20f - totalDifference * 2f,
            0f,
            20f
        );

        return Mathf.RoundToInt(score);
    }

    // =====================================
    // 横・縦の位置判定
    // =====================================

    private bool IsAlignedHorizontally(int a, int b, int c)
    {
        float y1 = shapeRaindrops[a].transform.position.y;
        float y2 = shapeRaindrops[b].transform.position.y;
        float y3 = shapeRaindrops[c].transform.position.y;

        float maxY = Mathf.Max(y1, y2, y3);
        float minY = Mathf.Min(y1, y2, y3);

        return maxY - minY <= 1f;
    }

    private bool IsAlignedVertically(int a, int b, int c)
    {
        float x1 = shapeRaindrops[a].transform.position.x;
        float x2 = shapeRaindrops[b].transform.position.x;
        float x3 = shapeRaindrops[c].transform.position.x;

        float maxX = Mathf.Max(x1, x2, x3);
        float minX = Mathf.Min(x1, x2, x3);

        return maxX - minX <= 1f;
    }

    // =====================================
    // ランク判定
    // =====================================

    private string GetRank(int score)
    {
        if (score >= 90) return "S";
        if (score >= 80) return "A";
        if (score >= 60) return "B";
        if (score >= 40) return "C";

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