# 🤝 贡献指南

感谢您对本项目的兴趣！以下是贡献代码的指南。

## 📋 行为准则

本项目采用贡献者行为准则。参与本项目意味着您同意遵守其条款。

## 🐛 报告 Bug

在报告 bug 之前，请检查问题列表，确保问题未被报告过。

在报告 bug 时，请包括：
- 清晰的 bug 描述
- 复现步骤
- 实际行为
- 预期行为
- 屏幕截图（如适用）
- 您的环境信息（Unity 版本、平台等）

## 💡 建议功能

功能建议使用 GitHub Issues 提交，请包括：
- 清晰的描述
- 用例
- 可能的实现方式

## 🔄 拉取请求流程

1. **Fork 项目**
   ```bash
   git clone https://github.com/YOUR_USERNAME/zeropaws.git
   cd slingshot-game
   ```

2. **创建分支**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **提交更改**
   ```bash
   git add .
   git commit -m "feat: 添加新功能的描述"
   ```

4. **推送到分支**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **提交 Pull Request**
   - 描述您的更改
   - 关联相关 Issue
   - 确保代码通过测试

## 📐 代码风格

### C# 代码规范

```csharp
// ✅ 好的例子
public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxLevel = 100;
    
    /// <summary>
    /// 这是一个方法的文档注释
    /// </summary>
    public void StartGame()
    {
        // 实现...
    }
}

// ❌ 避免
public class gamemanager : MonoBehaviour
{
    public int ml = 100; // 变量名不清晰
    public void sg() { } // 方法名缩写
}
```

### 命名规范
- 类名：PascalCase (GameManager)
- 方法名：PascalCase (StartGame)
- 变量名：camelCase (maxLevel)
- 常量：UPPER_SNAKE_CASE (MAX_LEVEL)
- 私有字段：_camelCase 或 m_camelCase
- SerializeField：保持简洁且有意义

### 注释规范
```csharp
// 单行注释

/// <summary>
/// 方法的摘要说明
/// </summary>
/// <param name="parameter">参数说明</param>
/// <returns>返回值说明</returns>
public void MethodName(int parameter) { }

// 复杂逻辑需要解释
// 为什么要这样做？
```

## 🧪 测试

在提交 PR 之前，请确保：

1. 代码在 Unity Editor 中运行正常
2. 没有 Console 错误
3. 新功能有测试覆盖
4. 不会破坏现有功能

## 📚 文档

如果您的更改涉及 API 或用户界面，请相应更新文档：

- README.md - 项目概述
- DEVELOPMENT.md - 开发指南
- 代码注释 - 复杂逻辑

## 📄 许可证

通过提交拉取请求，您同意在 MIT 许可证下许可您的贡献。

## ❓ 问题

有问题？请通过以下方式联系：
- 提交 Issue
- 查看讨论
- 查看现有文档

---

感谢您的贡献！🙏
