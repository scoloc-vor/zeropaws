using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 体力系统 - 管理玩家体力
/// 每局消耗1点，最大20点
/// 自动恢复：每5分钟+1点
/// </summary>
public class StaminaSystem : MonoBehaviour
{
    [SerializeField] private int maxStamina = 20;
    [SerializeField] private int recoveryTimeSeconds = 300; // 每点恢复时间(秒)
    [SerializeField] private bool useOfflineRecovery = true; // 是否支持离线恢复

    private int currentStamina;
    private float lastRecoveryTime;

    private const string STAMINA_KEY = "PlayerStamina";
    private const string STAMINA_TIME_KEY = "LastStaminaTime";

    public event Action<int> OnStaminaChanged;      // 体力改变事件
    public event Action<int> OnStaminaRecovered;    // 体力恢复事件
    public event Action OnStaminaEmpty;             // 体力用尽事件

    private Coroutine recoveryCoroutine;

    private void Start()
    {
        LoadStamina();
        StartStaminaRecovery();
    }

    private void OnDestroy()
    {
        SaveStamina();
    }

    /// <summary>
    /// 加载体力数据
    /// </summary>
    public void LoadStamina()
    {
        currentStamina = PlayerPrefs.GetInt(STAMINA_KEY, maxStamina);
        lastRecoveryTime = float.Parse(PlayerPrefs.GetString(STAMINA_TIME_KEY, Time.time.ToString()));

        // 计算离线期间恢复的体力
        if (useOfflineRecovery)
        {
            float timePassed = Time.time - lastRecoveryTime;
            int recoveredStamina = Mathf.FloorToInt(timePassed / recoveryTimeSeconds);
            
            if (recoveredStamina > 0)
            {
                currentStamina = Mathf.Min(currentStamina + recoveredStamina, maxStamina);
                Debug.Log($"离线恢复体力：{recoveredStamina}点");
            }
        }

        OnStaminaChanged?.Invoke(currentStamina);
    }

    /// <summary>
    /// 保存体力数据
    /// </summary>
    public void SaveStamina()
    {
        PlayerPrefs.SetInt(STAMINA_KEY, currentStamina);
        PlayerPrefs.SetString(STAMINA_TIME_KEY, Time.time.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 消耗体力
    /// </summary>
    public bool UseStamina(int amount = 1)
    {
        if (currentStamina < amount)
        {
            OnStaminaEmpty?.Invoke();
            Debug.LogWarning($"体力不足！");
            return false;
        }

        currentStamina -= amount;
        OnStaminaChanged?.Invoke(currentStamina);
        SaveStamina();

        Debug.Log($"消耗体力：{amount}，剩余：{currentStamina}/{maxStamina}");
        return true;
    }

    /// <summary>
    /// 恢复体力
    /// </summary>
    public void RecoverStamina(int amount = 1)
    {
        if (currentStamina >= maxStamina)
        {
            Debug.Log("体力已满");
            return;
        }

        int oldStamina = currentStamina;
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        int actualRecovered = currentStamina - oldStamina;

        OnStaminaRecovered?.Invoke(actualRecovered);
        OnStaminaChanged?.Invoke(currentStamina);
        SaveStamina();

        Debug.Log($"恢复体力：{actualRecovered}，当前：{currentStamina}/{maxStamina}");
    }

    /// <summary>
    /// 通过观看视频恢复体力（满值）
    /// </summary>
    public void WatchVideoForStaminaRecovery()
    {
        int recoveredAmount = maxStamina - currentStamina;
        currentStamina = maxStamina;
        OnStaminaRecovered?.Invoke(recoveredAmount);
        OnStaminaChanged?.Invoke(currentStamina);
        SaveStamina();
        Debug.Log($"观看视频，体力恢复满值（{recoveredAmount}点）");
    }

    /// <summary>
    /// 启动体力自动恢复
    /// </summary>
    private void StartStaminaRecovery()
    {
        if (recoveryCoroutine != null)
        {
            StopCoroutine(recoveryCoroutine);
        }
        recoveryCoroutine = StartCoroutine(AutoRecoveryCoroutine());
    }

    /// <summary>
    /// 体力自动恢复协程
    /// </summary>
    private IEnumerator AutoRecoveryCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(recoveryTimeSeconds);

            if (currentStamina < maxStamina)
            {
                RecoverStamina(1);
            }
        }
    }

    /// <summary>
    /// 获取当前体力
    /// </summary>
    public int GetCurrentStamina() => currentStamina;

    /// <summary>
    /// 获取最大体力
    /// </summary>
    public int GetMaxStamina() => maxStamina;

    /// <summary>
    /// 获取体力百分比
    /// </summary>
    public float GetStaminaPercentage() => (float)currentStamina / maxStamina;

    /// <summary>
    /// 检查体力是否充足
    /// </summary>
    public bool HasEnoughStamina(int requiredAmount = 1) => currentStamina >= requiredAmount;
}
