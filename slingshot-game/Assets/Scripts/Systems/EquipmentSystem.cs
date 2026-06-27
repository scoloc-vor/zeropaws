using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 装备系统 - 管理弹弓和子弹的升级
/// </summary>
public class EquipmentSystem : MonoBehaviour
{
    [System.Serializable]
    public class EquipmentLevel
    {
        public int level = 1;
        public float powerMultiplier = 1f;     // 威力倍数
        public float accuracyMultiplier = 1f;  // 准度倍数
        public int upgradeCost = 100;          // 升级花费
    }

    [SerializeField] private List<EquipmentLevel> slingshotLevels = new List<EquipmentLevel>();
    [SerializeField] private List<EquipmentLevel> projectileLevels = new List<EquipmentLevel>();

    private int currentSlingshotLevel = 1;
    private int currentProjectileLevel = 1;

    private const string SLINGSHOT_LEVEL_KEY = "SlingshotLevel";
    private const string PROJECTILE_LEVEL_KEY = "ProjectileLevel";

    public event Action<int> OnSlingshotUpgraded;
    public event Action<int> OnProjectileUpgraded;

    private CurrencySystem currencySystem;

    private void Start()
    {
        currencySystem = GameManager.Instance.GetCurrencySystem();
        LoadEquipment();
        InitializeLevels();
    }

    /// <summary>
    /// 初始化装备等级数据
    /// </summary>
    private void InitializeLevels()
    {
        if (slingshotLevels.Count == 0)
        {
            // 创建默认的弹弓等级
            for (int i = 1; i <= 50; i++)
            {
                slingshotLevels.Add(new EquipmentLevel
                {
                    level = i,
                    powerMultiplier = 1f + (i - 1) * 0.01f,
                    accuracyMultiplier = 1f + (i - 1) * 0.005f,
                    upgradeCost = 100 * i
                });
            }
        }

        if (projectileLevels.Count == 0)
        {
            // 创建默认的子弹等级
            for (int i = 1; i <= 50; i++)
            {
                projectileLevels.Add(new EquipmentLevel
                {
                    level = i,
                    powerMultiplier = 1f + (i - 1) * 0.02f,
                    accuracyMultiplier = 1f + (i - 1) * 0.01f,
                    upgradeCost = 50 * i
                });
            }
        }
    }

    /// <summary>
    /// 加载装备数据
    /// </summary>
    private void LoadEquipment()
    {
        currentSlingshotLevel = PlayerPrefs.GetInt(SLINGSHOT_LEVEL_KEY, 1);
        currentProjectileLevel = PlayerPrefs.GetInt(PROJECTILE_LEVEL_KEY, 1);
    }

    /// <summary>
    /// 保存装备数据
    /// </summary>
    private void SaveEquipment()
    {
        PlayerPrefs.SetInt(SLINGSHOT_LEVEL_KEY, currentSlingshotLevel);
        PlayerPrefs.SetInt(PROJECTILE_LEVEL_KEY, currentProjectileLevel);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 升级弹弓
    /// </summary>
    public bool UpgradeSlingshot()
    {
        if (currentSlingshotLevel >= slingshotLevels.Count)
        {
            Debug.LogWarning("弹弓已达最高级");
            return false;
        }

        int nextLevel = currentSlingshotLevel + 1;
        EquipmentLevel levelData = slingshotLevels[nextLevel - 1];
        int cost = levelData.upgradeCost;

        if (!currencySystem.SpendCoins(cost))
        {
            Debug.Log($"金币不足！升级需要 {cost} 金币");
            return false;
        }

        currentSlingshotLevel = nextLevel;
        OnSlingshotUpgraded?.Invoke(currentSlingshotLevel);
        SaveEquipment();

        Debug.Log($"弹弓升级到 Lv.{currentSlingshotLevel}");
        return true;
    }

    /// <summary>
    /// 升级子弹
    /// </summary>
    public bool UpgradeProjectile()
    {
        if (currentProjectileLevel >= projectileLevels.Count)
        {
            Debug.LogWarning("子弹已达最高级");
            return false;
        }

        int nextLevel = currentProjectileLevel + 1;
        EquipmentLevel levelData = projectileLevels[nextLevel - 1];
        int cost = levelData.upgradeCost;

        if (!currencySystem.SpendCoins(cost))
        {
            Debug.Log($"金币不足！升级需要 {cost} 金币");
            return false;
        }

        currentProjectileLevel = nextLevel;
        OnProjectileUpgraded?.Invoke(currentProjectileLevel);
        SaveEquipment();

        Debug.Log($"子弹升级到 Lv.{currentProjectileLevel}");
        return true;
    }

    /// <summary>
    /// 获取弹弓威力加成
    /// </summary>
    public float GetSlingshotPowerBonus()
    {
        if (currentSlingshotLevel > slingshotLevels.Count) return 1f;
        return slingshotLevels[currentSlingshotLevel - 1].powerMultiplier;
    }

    /// <summary>
    /// 获取弹弓准度加成
    /// </summary>
    public float GetSlingshotAccuracyBonus()
    {
        if (currentSlingshotLevel > slingshotLevels.Count) return 1f;
        return slingshotLevels[currentSlingshotLevel - 1].accuracyMultiplier;
    }

    /// <summary>
    /// 获取子弹威力加成
    /// </summary>
    public float GetProjectilePowerBonus()
    {
        if (currentProjectileLevel > projectileLevels.Count) return 1f;
        return projectileLevels[currentProjectileLevel - 1].powerMultiplier;
    }

    /// <summary>
    /// 获取子弹准度加成
    /// </summary>
    public float GetProjectileAccuracyBonus()
    {
        if (currentProjectileLevel > projectileLevels.Count) return 1f;
        return projectileLevels[currentProjectileLevel - 1].accuracyMultiplier;
    }

    /// <summary>
    /// 获取下一级升级花费
    /// </summary>
    public int GetNextSlingshotUpgradeCost()
    {
        if (currentSlingshotLevel >= slingshotLevels.Count) return -1;
        return slingshotLevels[currentSlingshotLevel].upgradeCost;
    }

    public int GetNextProjectileUpgradeCost()
    {
        if (currentProjectileLevel >= projectileLevels.Count) return -1;
        return projectileLevels[currentProjectileLevel].upgradeCost;
    }

    /// <summary>
    /// 获取当前等级
    /// </summary>
    public int GetSlingshotLevel() => currentSlingshotLevel;
    public int GetProjectileLevel() => currentProjectileLevel;
}
