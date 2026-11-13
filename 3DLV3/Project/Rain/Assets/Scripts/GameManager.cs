
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

    [Header("雨粒設定")]
    [SerializeField] private GameObject raindropPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField]
    private int[] stageRaindropLimits =
    {
        10, 10, 10, 10, 10
    };

    [SerializeField] private TMP_Text remainingRaindropsText;

    [Header("Wで追加する雨粒の色")]
    [SerializeField] private Color addedRaindropColor = Color.yellow;

    [Header("制限時間")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private TMP_Text timerText;

    [Header("画面表示")]
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text messageText;

    [Header("ボタン")]
    [SerializeField] private GameObject nextStageButton;

    private int remainingRaindrops;
    private float remainingTime;

    private bool gameFinished = false;
    private bool timeUp = false;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (gameFinished || timeUp)
            return;

        // 制限時間
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            TimeUp();
        }

        UpdateTimerDisplay();

        // Wキーで雨粒を追加
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddRaindrop();
        }
    }

    public void StartGame()
    {
        currentStage = 1;
        gameFinished = false;
        timeUp = false;

        if (nextStageButton != null)
        {
            nextStageButton.SetActive(false);
        }

        ResetRaindrops();
        ResetTimer();
        UpdateStageDisplay();

        Debug.Log("ゲーム開始！");
    }

    // 雨粒の残り数をリセット
    private void ResetRaindrops()
    {
        int index = currentStage - 1;

        if (index >= 0 && index < stageRaindropLimits.Length)
        {
            remainingRaindrops = stageRaindropLimits[index];
        }
        else
        {
            remainingRaindrops = 0;
        }

        UpdateRaindropDisplay();
    }

    // 制限時間をリセット
    private void ResetTimer()
    {
        remainingTime = timeLimit;
        timeUp = false;

        UpdateTimerDisplay();
    }

    // タイマー表示
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);

            timerText.text =
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
    }

    // Wキーで雨粒を追加
    public void AddRaindrop()
    {
        if (gameFinished || timeUp)
            return;

        if (remainingRaindrops <= 0)
            return;

        if (raindropPrefab == null || spawnPoint == null)
        {
            Debug.LogError("雨粒PrefabまたはSpawnPointが設定されていません！");
            return;
        }

        GameObject newRaindrop = Instantiate(
            raindropPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Wで追加した雨粒だけ色を変更
        RaindropClick click =
            newRaindrop.GetComponent<RaindropClick>();

        if (click != null)
        {
            click.SetNormalColor(addedRaindropColor);
        }

        remainingRaindrops--;

        UpdateRaindropDisplay();

        Debug.Log("雨粒を追加！残り：" + remainingRaindrops);
    }

    // 残り雨粒の表示
    private void UpdateRaindropDisplay()
    {
        if (remainingRaindropsText != null)
        {
            remainingRaindropsText.text =
                "残り雨粒：" + remainingRaindrops;
        }
    }

    // ステージ表示
    private void UpdateStageDisplay()
    {
        if (stageText != null)
        {
            stageText.text = "STAGE " + currentStage;
        }

        if (questionText != null &&
            currentStage <= stageQuestions.Length)
        {
            questionText.text =
                stageQuestions[currentStage - 1];
        }

        if (messageText != null)
        {
            messageText.text = "";
        }

        Debug.Log(
            "ステージ" + currentStage +
            " お題：" + stageQuestions[currentStage - 1]
        );
    }

    // 次のステージボタンを表示
    public void ShowNextStageButton()
    {
        if (nextStageButton != null)
        {
            nextStageButton.SetActive(true);
        }
    }

    // 次のステージへ
    public void NextStage()
    {
        if (gameFinished || timeUp)
            return;

        if (nextStageButton != null)
        {
            nextStageButton.SetActive(false);
        }

        if (currentStage < maxStage)
        {
            currentStage++;

            ResetRaindrops();
            ResetTimer();
            UpdateStageDisplay();
        }
        else
        {
            GameClear();
        }
    }

    // 時間切れ
    private void TimeUp()
    {
        timeUp = true;

        if (messageText != null)
        {
            messageText.text = "TIME UP!";
        }

        Debug.Log("時間切れ！");
    }

    // 全ステージクリア
    private void GameClear()
    {
        gameFinished = true;

        if (messageText != null)
        {
            messageText.text = "ALL CLEAR!";
        }

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
