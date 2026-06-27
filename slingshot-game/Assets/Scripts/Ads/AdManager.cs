using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 广告管理器 - 集成 Google AdMob
/// 支持 Banner、插屏、激励视频广告
/// </summary>
public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [Header("AdMob ID 配置")]
    [SerializeField] private string androidAppId = "ca-app-pub-xxxxxxxxxxxxxxxx~yyyyyyyyyy";
    [SerializeField] private string iOSAppId = "ca-app-pub-xxxxxxxxxxxxxxxx~yyyyyyyyyy";

    [SerializeField] private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";      // 测试 ID
    [SerializeField] private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712"; // 测试 ID
    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";    // 测试 ID

    [Header("广告设置")]
    [SerializeField] private bool enableTestMode = true; // 是否使用测试 ID
    [SerializeField] private int interstitialShowInterval = 3; // 每X局显示一次插屏广告

    private int gameSessionCount = 0;

    private bool isBannerLoaded = false;
    private bool isInterstitialLoaded = false;
    private bool isRewardedLoaded = false;

    public event Action<bool> OnRewardedAdClosed; // 激励视频关闭事件
    public event Action OnBannerLoaded;
    public event Action OnInterstitialLoaded;

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
        InitializeAdMob();
        LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();
    }

    /// <summary>
    /// 初始化 Google Mobile Ads SDK
    /// </summary>
    private void InitializeAdMob()
    {
#if UNITY_ANDROID
        string appId = enableTestMode ? "ca-app-pub-3940256099942544~3347511713" : androidAppId;
#elif UNITY_IOS
        string appId = enableTestMode ? "ca-app-pub-3940256099942544~5575463675" : iOSAppId;
#else
        string appId = "unused";
#endif

        // 这里应该初始化 Google Mobile Ads SDK
        // 实际项目中需要导入 Google Mobile Ads 插件
        Debug.Log($"AdMob 已初始化，App ID: {appId}");
    }

    /// <summary>
    /// 加载 Banner 广告
    /// </summary>
    public void LoadBannerAd()
    {
#if UNITY_EDITOR
        Debug.Log("[Editor] Banner 广告加载 (模拟)");
        isBannerLoaded = true;
        OnBannerLoaded?.Invoke();
        return;
#endif

        // 实际实现：使用 Google Mobile Ads SDK
        // RequestBannerAd();
        
        isBannerLoaded = true;
        OnBannerLoaded?.Invoke();
        Debug.Log("Banner 广告已加载");
    }

    /// <summary>
    /// 加载插屏广告
    /// </summary>
    public void LoadInterstitialAd()
    {
#if UNITY_EDITOR
        Debug.Log("[Editor] 插屏广告加载 (模拟)");
        isInterstitialLoaded = true;
        OnInterstitialLoaded?.Invoke();
        return;
#endif

        isInterstitialLoaded = true;
        OnInterstitialLoaded?.Invoke();
        Debug.Log("插屏广告已加载");
    }

    /// <summary>
    /// 加载激励视频广告
    /// </summary>
    public void LoadRewardedAd()
    {
#if UNITY_EDITOR
        Debug.Log("[Editor] 激励视频加载 (模拟)");
        isRewardedLoaded = true;
        return;
#endif

        isRewardedLoaded = true;
        Debug.Log("激励视频已加载");
    }

    /// <summary>
    /// 显示 Banner 广告
    /// </summary>
    public void ShowBannerAd()
    {
        if (!isBannerLoaded)
        {
            Debug.LogWarning("Banner 广告未加载");
            LoadBannerAd();
            return;
        }

        // 显示 Banner 广告逻辑
        Debug.Log("显示 Banner 广告");
    }

    /// <summary>
    /// 隐藏 Banner 广告
    /// </summary>
    public void HideBannerAd()
    {
        Debug.Log("隐藏 Banner 广告");
    }

    /// <summary>
    /// 显示插屏广告（在游戏失败或关卡完成时）
    /// </summary>
    public void ShowInterstitialAd()
    {
        gameSessionCount++;

        // 每 N 局显示一次
        if (gameSessionCount % interstitialShowInterval != 0)
        {
            Debug.Log($"跳过插屏广告 (下次在第 {interstitialShowInterval - (gameSessionCount % interstitialShowInterval)} 局显示)");
            return;
        }

        if (!isInterstitialLoaded)
        {
            Debug.LogWarning("插屏广告未加载");
            LoadInterstitialAd();
            return;
        }

        Debug.Log("显示插屏广告");
        // 显示后需要重新加载
        LoadInterstitialAd();
    }

    /// <summary>
    /// 显示激励视频广告（获得奖励）
    /// </summary>
    public void ShowRewardedAd(Action<bool> onAdClosed = null)
    {
        if (!isRewardedLoaded)
        {
            Debug.LogWarning("激励视频未加载");
            LoadRewardedAd();
            onAdClosed?.Invoke(false); // 广告未加载，奖励失败
            return;
        }

        Debug.Log("显示激励视频");
        
        // 模拟看完广告
        StartCoroutine(SimulateRewardedAdWatch(onAdClosed));
    }

    /// <summary>
    /// 模拟看完激励视频（仅在编辑器测试用）
    /// </summary>
    private System.Collections.IEnumerator SimulateRewardedAdWatch(Action<bool> onAdClosed)
    {
        yield return new UnityEngine.WaitForSeconds(1f);
        
        bool userCompletedAd = true; // 模拟用户看完广告
        onAdClosed?.Invoke(userCompletedAd);
        OnRewardedAdClosed?.Invoke(userCompletedAd);
        
        // 重新加载激励视频
        LoadRewardedAd();
        
        Debug.Log($"激励视频完成，奖励发放：{(userCompletedAd ? "成功" : "失败")}");
    }

    /// <summary>
    /// 检查是否可以显示激励视频
    /// </summary>
    public bool IsRewardedAdReady() => isRewardedLoaded;

    /// <summary>
    /// 获取广告配置信息
    /// </summary>
    public string GetAdMobInfo()
    {
        return $"AdMob 配置\n" +
               $"Banner ID: {bannerAdUnitId}\n" +
               $"插屏 ID: {interstitialAdUnitId}\n" +
               $"激励视频 ID: {rewardedAdUnitId}\n" +
               $"测试模式: {enableTestMode}";
    }
}
