using UnityEngine;

/// <summary>
/// 项目配置文件
/// 存储游戏常用配置常数
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Slingshot/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("游戏参数")]
    public int maxLevels = 100;
    public int initialCoins = 100;
    public int maxCoins = 999999;

    [Header("体力参数")]
    public int maxStamina = 20;
    public int staminaRecoveryTimeSeconds = 300; // 5分钟

    [Header("装备参数")]
    public int maxEquipmentLevel = 50;
    public float equipmentUpgradeCostMultiplier = 1.1f;

    [Header("弹弓参数")]
    public float maxPullDistance = 2f;
    public float shootForceMultiplier = 50f;
    public float projectileLifetime = 10f;

    [Header("广告参数")]
    public int interstitialShowInterval = 3; // 每3局显示一次
    public int videoRewardCoins = 500;
    public bool enableTestAds = true;

    [Header("UI 参数")]
    public float uiUpdateInterval = 0.1f;
}
