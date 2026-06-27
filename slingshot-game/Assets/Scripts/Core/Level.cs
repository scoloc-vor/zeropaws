using UnityEngine;
using System;

/// <summary>
/// 关卡系统 - 管理关卡数据和逻辑
/// </summary>
public class Level : MonoBehaviour
{
    [System.Serializable]
    public class TargetData
    {
        public int targetId;
        public Vector3 position;
        public TargetType type;
        public int coins;
        public bool isDestroyed;
    }

    public enum TargetType
    {
        Normal,    // 普通目标
        Special,   // 特殊目标
        Bonus      // 奖励目标
    }

    private int levelIndex;
    private TargetData[] targets;
    private float difficulty;
    private int targetCount;

    public Level(int index)
    {
        levelIndex = index;
        difficulty = 1f + (index - 1) * 0.1f; // 难度递进
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    public void Initialize()
    {
        GenerateTargets();
        Debug.Log($"关卡 {levelIndex} 已初始化，难度倍数: {difficulty:F2}");
    }

    /// <summary>
    /// 生成目标
    /// </summary>
    private void GenerateTargets()
    {
        // 根据难度生成不同数量的目标
        int targetCount = Mathf.Min(3 + (levelIndex / 5), 10);
        targets = new TargetData[targetCount];

        for (int i = 0; i < targetCount; i++)
        {
            targets[i] = new TargetData
            {
                targetId = i,
                position = new Vector3(
                    UnityEngine.Random.Range(-5f, 5f),
                    UnityEngine.Random.Range(0f, 5f),
                    UnityEngine.Random.Range(5f, 15f)
                ),
                type = GetRandomTargetType(),
                coins = GetCoinsForTargetType(GetRandomTargetType()),
                isDestroyed = false
            };
        }
    }

    /// <summary>
    /// 获取随机目标类型
    /// </summary>
    private TargetType GetRandomTargetType()
    {
        float rand = UnityEngine.Random.value;
        if (rand < 0.7f) return TargetType.Normal;
        else if (rand < 0.95f) return TargetType.Special;
        else return TargetType.Bonus;
    }

    /// <summary>
    /// 获取目标类型对应的金币
    /// </summary>
    private int GetCoinsForTargetType(TargetType type)
    {
        return type switch
        {
            TargetType.Normal => 10,
            TargetType.Special => 50,
            TargetType.Bonus => 200,
            _ => 0
        };
    }

    /// <summary>
    /// 获取关卡奖励
    /// </summary>
    public int GetReward(bool isPerfect)
    {
        int baseReward = 100;
        int perfectBonus = isPerfect ? 200 : 0;
        return baseReward + perfectBonus;
    }

    /// <summary>
    /// 获取所有目标
    /// </summary>
    public TargetData[] GetTargets() => targets;

    /// <summary>
    /// 获取关卡索引
    /// </summary>
    public int GetLevelIndex() => levelIndex;

    /// <summary>
    /// 获取难度倍数
    /// </summary>
    public float GetDifficulty() => difficulty;
}
