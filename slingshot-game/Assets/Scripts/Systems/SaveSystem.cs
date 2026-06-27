using UnityEngine;
using System;
using System.IO;

/// <summary>
/// 存档系统 - 管理游戏数据的保存和加载
/// </summary>
public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public int coins = 100;
        public int stamina = 20;
        public int currentLevel = 1;
        public int slingshotLevel = 1;
        public int projectileLevel = 1;
        public long lastPlayTime;
        public long totalPlayTime;
    }

    private PlayerData playerData;
    private string savePath;
    private const string SAVE_FILE = "player_data.json";

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE);
    }

    /// <summary>
    /// 加载玩家数据
    /// </summary>
    public void LoadPlayerData()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                playerData = JsonUtility.FromJson<PlayerData>(json);
                Debug.Log($"成功加载存档：{savePath}");
            }
            else
            {
                // 如果文件不存在，创建新数据
                playerData = new PlayerData();
                SavePlayerData();
                Debug.Log("创建新存档");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"加载存档失败：{e.Message}");
            playerData = new PlayerData();
        }
    }

    /// <summary>
    /// 保存玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        try
        {
            playerData.lastPlayTime = DateTime.Now.Ticks;
            string json = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"存档已保存：{savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存存档失败：{e.Message}");
        }
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    public void DeletePlayerData()
    {
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                playerData = new PlayerData();
                Debug.Log("存档已删除");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"删除存档失败：{e.Message}");
        }
    }

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    public PlayerData GetPlayerData() => playerData;

    /// <summary>
    /// 获取存档路径
    /// </summary>
    public string GetSavePath() => savePath;
}
