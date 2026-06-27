using UnityEngine;
using System;

/// <summary>
/// 金币系统 - 管理游戏内货币
/// </summary>
public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private int initialCoins = 100;
    [SerializeField] private int maxCoins = 999999;

    private int coins;
    private const string COINS_KEY = "PlayerCoins";

    public event Action<int> OnCoinsChanged; // 金币改变事件
    public event Action<int> OnCoinAdded;    // 金币增加事件
    public event Action<int> OnCoinSpent;    // 金币消耗事件

    private void Start()
    {
        LoadCoins();
    }

    /// <summary>
    /// 加载金币数据
    /// </summary>
    public void LoadCoins()
    {
        coins = PlayerPrefs.GetInt(COINS_KEY, initialCoins);
        OnCoinsChanged?.Invoke(coins);
    }

    /// <summary>
    /// 保存金币数据
    /// </summary>
    public void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, coins);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    public bool AddCoins(int amount)
    {
        if (amount <= 0) return false;

        int newCoins = Mathf.Min(coins + amount, maxCoins);
        int actualAdd = newCoins - coins;
        coins = newCoins;

        OnCoinAdded?.Invoke(actualAdd);
        OnCoinsChanged?.Invoke(coins);
        SaveCoins();

        Debug.Log($"增加金币：{actualAdd}，总计：{coins}");
        return true;
    }

    /// <summary>
    /// 消耗金币
    /// </summary>
    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (coins < amount)
        {
            Debug.LogWarning($"金币不足！需要：{amount}，拥有：{coins}");
            return false;
        }

        coins -= amount;
        OnCoinSpent?.Invoke(amount);
        OnCoinsChanged?.Invoke(coins);
        SaveCoins();

        Debug.Log($"消耗金币：{amount}，剩余：{coins}");
        return true;
    }

    /// <summary>
    /// 检查金币是否足够
    /// </summary>
    public bool HasEnoughCoins(int amount)
    {
        return coins >= amount;
    }

    /// <summary>
    /// 获取当前金币
    /// </summary>
    public int GetCoins() => coins;

    /// <summary>
    /// 设置金币（调试用）
    /// </summary>
    public void SetCoins(int amount)
    {
        coins = Mathf.Clamp(amount, 0, maxCoins);
        OnCoinsChanged?.Invoke(coins);
        SaveCoins();
    }

    /// <summary>
    /// 通过观看视频获得金币
    /// </summary>
    public void WatchVideoForCoins(int videoReward = 500)
    {
        AddCoins(videoReward);
        Debug.Log($"观看视频获得 {videoReward} 金币");
    }
}
