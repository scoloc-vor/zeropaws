using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// UI 管理器 - 管理所有游戏UI显示
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("游戏内 UI")]
    [SerializeField] private Text staminaText;        // 体力显示
    [SerializeField] private Text coinsText;          // 金币显示
    [SerializeField] private Text levelText;          // 关卡显示
    [SerializeField] private Slider pullSlider;       // 拉动条
    [SerializeField] private Image pullFillImage;     // 拉动条填充

    [Header("关卡完成 UI")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Text rewardText;        // 奖励金币
    [SerializeField] private Button watchVideoButton; // 看视频按钮
    [SerializeField] private Button nextLevelButton;  // 下一关按钮

    [Header("失败 UI")]
    [SerializeField] private GameObject failPanel;
    [SerializeField] private Button retryButton;      // 重试按钮
    [SerializeField] private Button exitButton;       // 退出按钮

    [Header("体力不足 UI")]
    [SerializeField] private GameObject staminaEmptyPanel;
    [SerializeField] private Button watchVideoForStaminaButton; // 看视频恢复体力
    [SerializeField] private Button waitButton;       // 等待恢复

    [Header("升级 UI")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Text slingshotLevelText;
    [SerializeField] private Text projectileLevelText;
    [SerializeField] private Button upgradeSlingshotButton;
    [SerializeField] private Button upgradeProjectileButton;

    [Header("颜色设置")]
    [SerializeField] private Color enoughCoinsColor = Color.white;    // 金币足够
    [SerializeField] private Color insufficientCoinsColor = Color.red; // 金币不足

    private GameManager gameManager;
    private CurrencySystem currencySystem;
    private StaminaSystem staminaSystem;
    private EquipmentSystem equipmentSystem;
    private AdManager adManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        currencySystem = gameManager.GetCurrencySystem();
        staminaSystem = gameManager.GetStaminaSystem();
        equipmentSystem = gameManager.GetEquipmentSystem();
        adManager = AdManager.Instance;

        SubscribeToEvents();
        RegisterButtonListeners();
        UpdateAllUI();
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    private void SubscribeToEvents()
    {
        currencySystem.OnCoinsChanged += UpdateCoinsUI;
        staminaSystem.OnStaminaChanged += UpdateStaminaUI;
        gameManager.OnGameStateChanged += UpdateUIForGameState;
        gameManager.OnLevelChanged += UpdateLevelUI;

        // 弹弓控制器事件订阅
        SlingshotController slingshotController = FindObjectOfType<SlingshotController>();
        if (slingshotController != null)
        {
            slingshotController.OnPullDistanceChanged += UpdatePullSlider;
        }
    }

    /// <summary>
    /// 注册按钮监听
    /// </summary>
    private void RegisterButtonListeners()
    {
        if (watchVideoButton != null)
            watchVideoButton.onClick.AddListener(OnWatchVideoForReward);

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevel);

        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetry);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExit);

        if (watchVideoForStaminaButton != null)
            watchVideoForStaminaButton.onClick.AddListener(OnWatchVideoForStamina);

        if (upgradeSlingshotButton != null)
            upgradeSlingshotButton.onClick.AddListener(OnUpgradeSlingshot);

        if (upgradeProjectileButton != null)
            upgradeProjectileButton.onClick.AddListener(OnUpgradeProjectile);
    }

    /// <summary>
    /// 更新所有 UI
    /// </summary>
    public void UpdateAllUI()
    {
        UpdateCoinsUI(currencySystem.GetCoins());
        UpdateStaminaUI(staminaSystem.GetCurrentStamina());
        UpdateLevelUI(gameManager.GetCurrentLevelNumber());
    }

    /// <summary>
    /// 更新金币 UI
    /// </summary>
    private void UpdateCoinsUI(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"💰 {coins}";
        }
    }

    /// <summary>
    /// 更新体力 UI
    /// </summary>
    private void UpdateStaminaUI(int stamina)
    {
        if (staminaText != null)
        {
            int maxStamina = staminaSystem.GetMaxStamina();
            staminaText.text = $"⚡ {stamina}/{maxStamina}";
        }
    }

    /// <summary>
    /// 更新关卡 UI
    /// </summary>
    private void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }

    /// <summary>
    /// 更新拉动条
    /// </summary>
    private void UpdatePullSlider(float pullRatio)
    {
        if (pullSlider != null)
        {
            pullSlider.value = pullRatio;
        }
    }

    /// <summary>
    /// 根据游戏状态更新 UI
    /// </summary>
    private void UpdateUIForGameState(GameManager.GameState state)
    {
        HideAllPanels();

        switch (state)
        {
            case GameManager.GameState.Playing:
                Debug.Log("游戏进行中");
                break;

            case GameManager.GameState.LevelComplete:
                ShowLevelCompleteUI();
                break;

            case GameManager.GameState.GameOver:
                ShowFailUI();
                break;

            case GameManager.GameState.Upgrade:
                ShowUpgradeUI();
                break;
        }
    }

    /// <summary>
    /// 显示关卡完成 UI
    /// </summary>
    private void ShowLevelCompleteUI()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            // 这里显示奖励金币等信息
        }
    }

    /// <summary>
    /// 显示失败 UI
    /// </summary>
    private void ShowFailUI()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(true);
            adManager.ShowInterstitialAd(); // 显示插屏广告
        }
    }

    /// <summary>
    /// 显示升级 UI
    /// </summary>
    private void ShowUpgradeUI()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            UpdateEquipmentUI();
        }
    }

    /// <summary>
    /// 更新装备 UI
    /// </summary>
    private void UpdateEquipmentUI()
    {
        if (slingshotLevelText != null)
            slingshotLevelText.text = $"弹弓等级: Lv.{equipmentSystem.GetSlingshotLevel()}";

        if (projectileLevelText != null)
            projectileLevelText.text = $"子弹等级: Lv.{equipmentSystem.GetProjectileLevel()}";

        // 更新升级按钮颜色
        UpdateUpgradeButtonColor();
    }

    /// <summary>
    /// 更新升级按钮颜色
    /// </summary>
    private void UpdateUpgradeButtonColor()
    {
        int slingshotCost = equipmentSystem.GetNextSlingshotUpgradeCost();
        int projectileCost = equipmentSystem.GetNextProjectileUpgradeCost();
        int coins = currencySystem.GetCoins();

        if (upgradeSlingshotButton != null)
        {
            Image buttonImage = upgradeSlingshotButton.GetComponent<Image>();
            buttonImage.color = coins >= slingshotCost ? enoughCoinsColor : insufficientCoinsColor;
        }

        if (upgradeProjectileButton != null)
        {
            Image buttonImage = upgradeProjectileButton.GetComponent<Image>();
            buttonImage.color = coins >= projectileCost ? enoughCoinsColor : insufficientCoinsColor;
        }
    }

    /// <summary>
    /// 隐藏所有面板
    /// </summary>
    private void HideAllPanels()
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
        if (staminaEmptyPanel != null) staminaEmptyPanel.SetActive(false);
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    // ============ 按钮回调函数 ============

    private void OnWatchVideoForReward()
    {
        adManager.ShowRewardedAd((success) =>
        {
            if (success)
            {
                currencySystem.WatchVideoForCoins(500);
                Debug.Log("奖励已发放");
            }
        });
    }

    private void OnNextLevel()
    {
        gameManager.StartGame();
    }

    private void OnRetry()
    {
        gameManager.StartGame();
    }

    private void OnExit()
    {
        gameManager.SetGameState(GameManager.GameState.Menu);
    }

    private void OnWatchVideoForStamina()
    {
        adManager.ShowRewardedAd((success) =>
        {
            if (success)
            {
                staminaSystem.WatchVideoForStaminaRecovery();
                HideAllPanels();
                gameManager.StartGame();
            }
        });
    }

    private void OnUpgradeSlingshot()
    {
        if (equipmentSystem.UpgradeSlingshot())
        {
            UpdateEquipmentUI();
            UpdateCoinsUI(currencySystem.GetCoins());
        }
        else
        {
            // 金币不足，显示看视频选项
            ShowWatchVideoPrompt("升级弹弓");
        }
    }

    private void OnUpgradeProjectile()
    {
        if (equipmentSystem.UpgradeProjectile())
        {
            UpdateEquipmentUI();
            UpdateCoinsUI(currencySystem.GetCoins());
        }
        else
        {
            ShowWatchVideoPrompt("升级子弹");
        }
    }

    /// <summary>
    /// 显示看视频提示
    /// </summary>
    private void ShowWatchVideoPrompt(string action)
    {
        Debug.Log($"金币不足，无法进行 {action}");
        // 这里可以弹出一个提示对话框
    }

    /// <summary>
    /// 显示体力不足面板
    /// </summary>
    public void ShowStaminaEmptyPanel()
    {
        if (staminaEmptyPanel != null)
        {
            staminaEmptyPanel.SetActive(true);
        }
    }
}
