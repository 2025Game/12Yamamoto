using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("ステージ設定")]
    [SerializeField] private int currentStage = 1;
    [SerializeField] private int maxStage = 5;

    [Header("お題設定")]
    [SerializeField]
    private string[] stageQuestions =
    {
        "口", "日", "木", "田", "雨"
    };

    [Header("画面表示")]
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text messageText;

    [Header("ボタン")]
    [SerializeField] private GameObject nextStageButton;

    private bool gameFinished = false;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentStage = 1;
        gameFinished = false;

        if (nextStageButton != null)
            nextStageButton.SetActive(false);

        UpdateStageDisplay();

        Debug.Log("ゲーム開始！");
    }

    private void UpdateStageDisplay()
    {
        if (stageText != null)
            stageText.text = "STAGE " + currentStage;

        if (questionText != null &&
            currentStage <= stageQuestions.Length)
        {
            questionText.text =
                stageQuestions[currentStage - 1];
        }

        if (messageText != null)
            messageText.text = "";

        Debug.Log(
            "ステージ" + currentStage +
            " お題：" + stageQuestions[currentStage - 1]
        );
    }

    // 採点後に次のステージボタンを表示
    public void ShowNextStageButton()
    {
        if (nextStageButton != null)
            nextStageButton.SetActive(true);
    }

    public void NextStage()
    {
        if (gameFinished)
            return;

        if (nextStageButton != null)
            nextStageButton.SetActive(false);

        if (currentStage < maxStage)
        {
            currentStage++;
            UpdateStageDisplay();
        }
        else
        {
            GameClear();
        }
    }

    private void GameClear()
    {
        gameFinished = true;

        if (messageText != null)
            messageText.text = "ALL CLEAR!";

        Debug.Log("全ステージクリア！");
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public bool IsGameFinished()
    {
        return gameFinished;
    }
}