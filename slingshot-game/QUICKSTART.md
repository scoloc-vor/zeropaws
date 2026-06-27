# 🚀 快速启动指南

## 📋 前置要求

- ✅ **Unity 2021 LTS** 或更高版本
- ✅ **Visual Studio** 或任何 C# IDE
- ✅ **Git** 版本控制
- ✅ **Google AdMob 账户** (用于广告变现)

---

## 🎮 5 分钟快速开始

### 步骤 1：克隆项目
```bash
git clone https://github.com/scoloc-vor/zeropaws.git
cd slingshot-game
```

### 步骤 2：在 Unity 中打开项目
```
Unity Hub → Open → 选择项目文件夹
等待 Unity 加载完成
```

### 步骤 3：运行游戏
```
Project 窗口 → Scenes → 双击 Gameplay.unity
Play 按钮 ▶️
```

### 步骤 4：测试功能
```
鼠标左键 = 拉动弹弓
松开 = 发射
Space = 测试通关
R = 测试失败
V = 看视频获得金币
```

---

## 💻 项目结构速览

```
🎮 核心游戏逻辑
├─ GameManager.cs         主管理器
├─ SlingshotController.cs 弹弓控制
└─ Level.cs               关卡系统

💰 游戏系统
├─ CurrencySystem.cs      金币管理
├─ StaminaSystem.cs       体力管理
├─ EquipmentSystem.cs     装备升级
└─ SaveSystem.cs          存档系统

📺 变现系统
└─ AdManager.cs           广告管理

🎨 用户界面
└─ UIManager.cs           UI 总控

⚙️ 配置
└─ GameConfig.cs          游戏配置
```

---

## 🔧 常用操作

### 修改游戏参数
```csharp
// 在 GameConfig.cs 中修改
public int maxLevels = 100;           // 最大关卡数
public int initialCoins = 100;        // 初始金币
public int maxStamina = 20;           // 最大体力
public int videoRewardCoins = 500;    // 视频奖励金币
```

### 调整难度
```csharp
// 在 Level.cs 中修改
difficulty = 1f + (index - 1) * 0.1f;  // 每关难度增加 10%
```

### 修改金币成本
```csharp
// 在 EquipmentSystem.cs 中修改升级成本
upgradeCost = 100 * i;  // 弹弓升级花费
upgradeCost = 50 * i;   // 子弹升级花费
```

---

## 📱 广告配置

### 1. 创建 Google AdMob 账户
- 访问 [AdMob 官网](https://admob.google.com)
- 创建应用
- 生成广告单元 ID

### 2. 在 AdManager.cs 中配置
```csharp
// 测试 ID（当前使用）
private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

// 改为真实 ID（发布时）
private string bannerAdUnitId = "YOUR_REAL_BANNER_ID";
```

### 3. 导入 Google Mobile Ads SDK
```
1. 下载: https://github.com/googleads/googleads-mobile-unity
2. Unity: Assets → Import Package → Custom Package
3. 选择下载的 .unitypackage 文件
```

---

## 🎯 游戏流程图

```
┌─────────────────┐
│   主菜单        │
│ [开始] [升级]   │
└────────┬────────┘
         │
         ▼
┌─────────────────────────────┐
│    检查体力                 │
├─────────────────────────────┤
│ 是否有体力？               │
│ ├─ 有: 进入游戏            │
│ └─ 无: 看视频恢复或等待     │
└────────┬────────────────────┘
         │
         ▼
┌──────────────────────────────┐
│   游戏画面 (第一视角)       │
├──────────────────────────────┤
│ 拉动弹弓 → 发射 → 击中目标   │
│                              │
│ 获得金币 → 完成关卡          │
└────────┬─────────────────────┘
         │
    ┌────┴────┐
    ▼         ▼
 ┌────────┐ ┌──────────┐
 │关卡完成│ │失败/重玩 │
 │+200💰  │ │插屏广告  │
 │看视频  │ └──────────┘
 │双倍?   │
 └────┬───┘
      │
      ▼
  [下一关]
```

---

## 💰 变现计算

### 日活用户 (DAU) 收入估计

| DAU | 激励视频/日 | CPM | 月收入 |
|-----|----------|-----|--------|
| 100 | 50 | $3 | $150 |
| 1000 | 500 | $5 | $2500 |
| 10000 | 5000 | $8 | $40000 |
| 100000 | 50000 | $10 | $500000 |

**说明：**
- 激励视频 CPM 最高 (5-10 美元)
- 插屏广告 CPM 中等 (2-5 美元)
- Banner 广告 CPM 最低 (0.5-2 美元)

---

## 🧪 测试快捷键

| 按键 | 功能 |
|------|------|
| Space | 完成关卡 |
| R | 失败重玩 |
| V | 看视频获得 500 金币 |
| S | 看视频恢复体力 |
| U | 打开升级界面 |
| D | 显示调试信息 |
| L | 进入下一关 |

---

## 📊 性能优化建议

### 优化 1：对象池
```csharp
// 预先生成子弹，避免运行时创建
public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> projectiles;
    
    public GameObject GetProjectile()
    {
        return projectiles.Count > 0 
            ? projectiles.Dequeue() 
            : Instantiate(projectilePrefab);
    }
}
```

### 优化 2：广告预加载
```csharp
// 提前加载下一个广告
AdManager.Instance.LoadRewardedAd();
AdManager.Instance.LoadInterstitialAd();
```

### 优化 3：内存管理
```csharp
// 销毁未使用的资源
Resources.UnloadUnusedAssets();
System.GC.Collect();
```

---

## 🐛 常见问题排查

### ❌ 广告不显示
```
1. 检查 AdMob ID 是否正确
2. 确认网络连接正常
3. 检查设备时间是否正确
4. 查看 Unity Console 的错误信息
```

### ❌ 体力无法恢复
```
1. 检查 StaminaSystem 是否正确初始化
2. 查看 RecoveryCoroutine 是否启动
3. 确认 Time.deltaTime 是否正确
```

### ❌ 金币数据丢失
```
1. 检查 SaveSystem 权限
2. 查看存档路径是否正确
3. 检查 PlayerPrefs 是否被清空
```

---

## 📦 发布到应用商店

### Android (Google Play)
```
1. 生成签名 APK
   Build Settings → Player Settings → Keystore

2. 配置 AdMob
   AdManager.cs 中填入真实 ID

3. 提交到 Google Play
   Play Console → 新建应用 → 上传 APK
```

### iOS (App Store)
```
1. 配置开发者证书
   Build Settings → Signing

2. 配置 AdMob
   AdManager.cs 中填入真实 iOS ID

3. 提交到 App Store
   Xcode → Archive → Upload to App Store
```

---

## 📚 进阶开发

### 添加新关卡类型
```csharp
// 在 Level.cs 中添加新的目标类型
public enum TargetType
{
    Normal,
    Special,
    Bonus,
    Moving,      // 新：移动目标
    Explosive    // 新：爆炸目标
}
```

### 添加新的装备
```csharp
// 在 EquipmentSystem.cs 中添加
public bool UpgradeSpecial() { ... }
```

### 实现本地多人对战
```csharp
// 创建 MultiplayerManager.cs
public class MultiplayerManager : MonoBehaviour { ... }
```

---

## 🎓 学习资源

- 📖 [Unity 官方文档](https://docs.unity.com)
- 📖 [Google Mobile Ads 文档](https://developers.google.com/admob/unity/start)
- 📖 [C# 编程指南](https://docs.microsoft.com/zh-cn/dotnet/csharp/)
- 🎥 [Unity 教程频道](https://www.youtube.com/c/Unity3D)

---

## 📞 获取帮助

- 📧 提交 Issue: [GitHub Issues](https://github.com/scoloc-vor/zeropaws/issues)
- 💬 讨论: [GitHub Discussions](https://github.com/scoloc-vor/zeropaws/discussions)
- 📝 查看源代码中的注释获得帮助

---

**祝您开发顺利！🚀✨**

开始构建下一个爆款游戏吧！
