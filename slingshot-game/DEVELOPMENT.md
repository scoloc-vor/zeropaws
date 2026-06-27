# 🎮 弹弓大师 Pro - 开发指南

## 📋 项目文件结构

```
slingshot-game/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs          ✅ 游戏核心管理器
│   │   │   ├── SlingshotController.cs  ✅ 弹弓控制逻辑
│   │   │   └── Level.cs                ✅ 关卡系统
│   │   │
│   │   ├── Systems/
│   │   │   ├── CurrencySystem.cs       ✅ 金币管理
│   │   │   ├── StaminaSystem.cs        ✅ 体力管理
│   │   │   ├── EquipmentSystem.cs      ✅ 装备升级
│   │   │   └── SaveSystem.cs           ✅ 存档系统
│   │   │
│   │   ├── Ads/
│   │   │   └── AdManager.cs            ✅ 广告管理（AdMob）
│   │   │
│   │   ├── UI/
│   │   │   └── UIManager.cs            ✅ UI总管理
│   │   │
│   │   ├── Config/
│   │   │   └── GameConfig.cs           ✅ 项目配置
│   │   │
│   │   └── Utils/
│   │       └── [待添加]
│   │
│   ├── Prefabs/           [待创建]
│   ├── Scenes/            [待创建]
│   └── Resources/         [待创建]
│
└── README.md              ✅ 项目说明
```

---

## ✅ 已完成功能

### 1. **核心系统** ✅
- [x] GameManager - 游戏状态管理
- [x] SlingshotController - 第一视角弹弓射击
- [x] Level - 关卡生成系统

### 2. **游戏系统** ✅
- [x] CurrencySystem - 金币获取/消耗
- [x] StaminaSystem - 体力消耗/恢复
- [x] EquipmentSystem - 装备升级
- [x] SaveSystem - 数据存档

### 3. **广告系统** ✅
- [x] AdManager - Google AdMob 集成
- [x] 激励视频广告
- [x] 插屏广告
- [x] Banner 广告

### 4. **UI 系统** ✅
- [x] UIManager - UI 总管理
- [x] 实时 UI 更新
- [x] 按钮事件处理

---

## 🚀 快速开始

### 第一步：Unity 项目设置

```bash
1. 创建新 Unity 项目 (2021 LTS 或更高)
2. 导入项目文件到 Assets 文件夹
3. 导入 Google Mobile Ads SDK
   - 访问: https://github.com/googleads/googleads-mobile-unity
   - 导入 .unitypackage 文件
```

### 第二步：场景设置

```
1. 创建 Menu.unity 场景 (主菜单)
   - 添加 Canvas
   - 添加菜单按钮

2. 创建 Gameplay.unity 场景 (游戏场景)
   - 添加 Main Camera (第一视角)
   - 添加 GameManager (到场景)
   - 添加 UIManager (到场景)
   - 添加 AdManager (到场景)
   - 添加 SlingshotController
```

### 第三步：配置 AdMob

```csharp
// 在 AdManager.cs 中修改
private string androidAppId = "YOUR_ANDROID_APP_ID";
private string iOSAppId = "YOUR_IOS_APP_ID";

// 替换广告单元 ID
private string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
private string interstitialAdUnitId = "YOUR_INTERSTITIAL_AD_UNIT_ID";
private string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";
```

### 第四步：运行游戏

```
Unity Editor → Play 按钮 → 开始游戏！
```

---

## 💰 金币流向说明

### 获得金币
```
击中目标
├─ 普通目标:  10 枚金币
├─ 特殊目标:  50 枚金币
└─ 奖励目标: 200 枚金币

完成关卡
├─ 基础奖励: 100 枚金币
└─ 完美通关: 200 枚金币

观看广告
├─ 激励视频:  500 枚金币
└─ 双倍奖励:  奖励翻倍
```

### 消耗金币
```
装备升级
├─ 弹弓 Lv2:   100 枚金币
├─ 弹弓 Lv3:   200 枚金币
└─ ... (逐级升高)

子弹升级
├─ 子弹 Lv2:    50 枚金币
├─ 子弹 Lv3:   100 枚金币
└─ ... (逐级升高)
```

---

## ⚡ 体力系统说明

```
体力消耗
├─ 每局游戏消耗: 1 点
├─ 最大体力:   20 点
└─ 无体力时无法进行游戏

体力恢复
├─ 自动恢复: 每 5 分钟 +1 点
├─ 离线恢复: 支持（关闭应用后也恢复）
└─ 看视频恢复: 立即恢复满值
```

---

## 🎯 代码使用示例

### 获取金币
```csharp
CurrencySystem currency = GameManager.Instance.GetCurrencySystem();
currency.AddCoins(100);  // 增加 100 金币
currency.SpendCoins(50); // 消耗 50 金币
int coins = currency.GetCoins(); // 获取当前金币
```

### 消耗体力
```csharp
StaminaSystem stamina = GameManager.Instance.GetStaminaSystem();
if (stamina.HasEnoughStamina()) {
    stamina.UseStamina(1);  // 消耗 1 点体力
}
stamina.RecoverStamina(5); // 恢复 5 点
```

### 装备升级
```csharp
EquipmentSystem equipment = GameManager.Instance.GetEquipmentSystem();
equipment.UpgradeSlingshot();  // 升级弹弓
equipment.UpgradeProjectile(); // 升级子弹
int level = equipment.GetSlingshotLevel(); // 获取等级
```

### 显示广告
```csharp
AdManager adManager = AdManager.Instance;
adManager.ShowRewardedAd((success) => {
    if (success) {
        // 用户成功看完广告，发放奖励
        CurrencySystem.AddCoins(500);
    }
});
```

---

## 📊 游戏数据存储

### 本地存储位置
```
Android: /data/data/com.company.app/files/player_data.json
iOS:     /var/mobile/Containers/.../Documents/player_data.json
Windows: %APPDATA%/LocalLow/.../player_data.json
```

### 存储数据格式
```json
{
  "coins": 5000,
  "stamina": 20,
  "currentLevel": 15,
  "slingshotLevel": 5,
  "projectileLevel": 3,
  "lastPlayTime": "2026-06-27T10:00:00Z",
  "totalPlayTime": 3600
}
```

---

## 🔧 下一步需要做的事

- [ ] 创建游戏场景预制体
- [ ] 美术资源导入（模型、贴图、动画）
- [ ] 音效系统集成
- [ ] 多语言本地化
- [ ] 性能优化
- [ ] 真机测试
- [ ] 上线 App Store / Google Play

---

## 🐛 常见问题

### Q: 如何在编辑器测试广告？
A: AdManager 中已内置编辑器模拟模式，无需真实广告 ID 即可测试。

### Q: 怎样修改游戏难度？
A: 在 GameConfig.cs 中调整参数，或修改 Level.cs 的生成逻辑。

### Q: 如何添加新的关卡？
A: 目前关卡自动生成，可修改 Level.cs 中的生成算法。

### Q: 支持离线游戏吗？
A: 完全支持离线游戏，所有数据本地存储。

---

## 📞 技术支持

遇到问题？
1. 检查 Debug 日志
2. 查看代码注释
3. 提交 GitHub Issue

---

**祝您游戏开发顺利！🎮✨**
