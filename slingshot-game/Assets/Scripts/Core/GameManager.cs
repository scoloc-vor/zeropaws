using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 游戏核心管理器
/// 管理游戏状态、关卡流程、玩家数据
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevels = 100;
    
    private GameState gameState = GameState.Menu;
    private CurrencySystem currencySystem;
    private StaminaSystem staminaSystem;
    private EquipmentSystem equipmentSystem;
    private SaveSystem saveSystem;
    private Level currentLevelData;

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnLevelChanged;

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        LevelComplete,
        GameOver,
        Upgrade
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeSystems();
        LoadGame();
    }

    /// <summary>
    /// 初始化所有游戏系统
    /// </summary>
    private void InitializeSystems()
    {
        currencySystem = GetComponent<CurrencySystem>();
        staminaSystem = GetComponent<StaminaSystem>();
        equipmentSystem = GetComponent<EquipmentSystem>();
        saveSystem = GetComponent<SaveSystem>();

        if (currencySystem == null) currencySystem = gameObject.AddComponent<CurrencySystem>();
        if (staminaSystem == null) staminaSystem = gameObject.AddComponent<StaminaSystem>();
        if (equipmentSystem == null) equipmentSystem = gameObject.AddComponent<EquipmentSystem>();
        if (saveSystem == null) saveSystem = gameObject.AddComponent<SaveSystem>();
    }

    /// <summary>
    /// 加载存档数据
    /// </summary>
    private void LoadGame()
    {
        saveSystem.LoadPlayerData();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        OnLevelChanged?.Invoke(currentLevel);
    }

    /// <summary>
    /// 保存游戏进度
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        saveSystem.SavePlayerData();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        // 检查体力
        if (staminaSystem.GetCurrentStamina() <= 0)
        {
            Debug.Log("体力不足！需要看视频恢复");
            return;
        }

        staminaSystem.UseStamina();
        SetGameState(GameState.Playing);
        LoadLevel(currentLevel);
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        currentLevelData = new Level(levelIndex);
        currentLevelData.Initialize();
        Debug.Log($"已加载关卡 {levelIndex}");
    }

    /// <summary>
    /// 完成关卡
    /// </summary>
    public void CompleteLevel(int score, bool isPerfect = false)
    {
        int reward = currentLevelData.GetReward(isPerfect);
        currencySystem.AddCoins(reward);
        
        SetGameState(GameState.LevelComplete);
        
        // 解锁下一关
        if (currentLevel < maxLevels)
        {
            currentLevel++;
            OnLevelChanged?.Invoke(currentLevel);
        }
        
        SaveGame();
        Debug.Log($"关卡完成！奖励：{reward} 金币");
    }

    /// <summary>
    /// 关卡失败
    /// </summary>
    public void FailLevel()
    {
        SetGameState(GameState.GameOver);
        Debug.Log("关卡失败");
    }

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    public void SetGameState(GameState state)
    {
        if (gameState == state) return;
        gameState = state;
        OnGameStateChanged?.Invoke(gameState);
    }

    /// <summary>
    /// 获取各系统实例
    /// </summary>
    public CurrencySystem GetCurrencySystem() => currencySystem;
    public StaminaSystem GetStaminaSystem() => staminaSystem;
    public EquipmentSystem GetEquipmentSystem() => equipmentSystem;
    public Level GetCurrentLevel() => currentLevelData;
    public int GetCurrentLevelNumber() => currentLevel;
    public GameState GetGameState() => gameState;
}
