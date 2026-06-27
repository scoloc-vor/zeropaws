using UnityEngine;
using System;

/// <summary>
/// 弹弓控制器 - 管理弹弓的拉动、瞄准、发射
/// </summary>
public class SlingshotController : MonoBehaviour
{
    [Header("弹弓参数")]
    [SerializeField] private float maxPullDistance = 2f;      // 最大拉动距离
    [SerializeField] private float shootForceMultiplier = 50f; // 发射力度倍数
    [SerializeField] private Transform shootPoint;             // 发射点
    [SerializeField] private Animator slingshotAnimator;       // 弹弓动画

    [Header("子弹参数")]
    [SerializeField] private GameObject projectilePrefab;      // 子弹预制体
    [SerializeField] private float projectileLifetime = 10f;   // 子弹存活时间

    [Header("输入设置")]
    [SerializeField] private bool useMobileInput = false;      // 是否使用移动输入

    private float currentPullDistance = 0f;  // 当前拉动距离
    private bool isPulling = false;          // 是否正在拉动
    private Vector3 pullDirection = Vector3.forward;
    private float currentPower = 0f;         // 当前威力倍数

    public event Action<float> OnPullDistanceChanged; // 拉动距离改变事件
    public event Action OnShoot;                      // 发射事件

    private EquipmentSystem equipmentSystem;

    private void Start()
    {
        equipmentSystem = GameManager.Instance.GetEquipmentSystem();
    }

    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        if (isPulling)
        {
            UpdatePullVisualization();
        }
    }

    /// <summary>
    /// 处理输入
    /// </summary>
    private void HandleInput()
    {
        if (useMobileInput)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    /// <summary>
    /// 鼠标输入处理
    /// </summary>
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPull();
        }

        if (Input.GetMouseButton(0) && isPulling)
        {
            UpdatePull();
        }

        if (Input.GetMouseButtonUp(0) && isPulling)
        {
            Shoot();
        }
    }

    /// <summary>
    /// 触摸输入处理（移动设备）
    /// </summary>
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartPull();
            }
            else if (touch.phase == TouchPhase.Moved && isPulling)
            {
                UpdatePull();
            }
            else if (touch.phase == TouchPhase.Ended && isPulling)
            {
                Shoot();
            }
        }
    }

    /// <summary>
    /// 开始拉动弹弓
    /// </summary>
    private void StartPull()
    {
        isPulling = true;
        currentPullDistance = 0f;
        slingshotAnimator?.SetBool("IsPulling", true);
        Debug.Log("开始拉动弹弓");
    }

    /// <summary>
    /// 更新拉动状态
    /// </summary>
    private void UpdatePull()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // 简单的距离计算（实际应使用平面投影）
        float distFromCamera = 10f;
        Vector3 targetPos = Camera.main.transform.position + ray.direction * distFromCamera;
        
        // 限制拉动距离
        Vector3 pullDelta = targetPos - shootPoint.position;
        currentPullDistance = Mathf.Min(pullDelta.magnitude, maxPullDistance);
        
        OnPullDistanceChanged?.Invoke(currentPullDistance / maxPullDistance); // 发送比例 0-1
    }

    /// <summary>
    /// 发射弹弓
    /// </summary>
    private void Shoot()
    {
        if (!isPulling) return;

        isPulling = false;
        slingshotAnimator?.SetBool("IsPulling", false);

        // 计算发射力度
        currentPower = (currentPullDistance / maxPullDistance);
        
        // 应用装备加成
        float equipmentBonus = equipmentSystem.GetSlingshotPowerBonus();
        float finalForce = shootForceMultiplier * currentPower * equipmentBonus;

        // 创建子弹
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            // 获取摄像头前方作为发射方向
            Vector3 shootDirection = Camera.main.transform.forward;
            rb.velocity = shootDirection * finalForce;
        }

        // 销毁子弹
        Destroy(projectile, projectileLifetime);

        OnShoot?.Invoke();
        slingshotAnimator?.SetTrigger("Shoot");
        
        Debug.Log($"发射！威力：{currentPower:P0}，最终力度：{finalForce:F2}");
    }

    /// <summary>
    /// 更新拉动的视觉效果
    /// </summary>
    private void UpdatePullVisualization()
    {
        // 这里可以添加拉动的视觉效果
        // 例如：改变弹弓的形状、显示预测轨迹等
        
        if (slingshotAnimator != null)
        {
            slingshotAnimator.SetFloat("PullAmount", currentPullDistance / maxPullDistance);
        }
    }

    /// <summary>
    /// 获取当前拉动比例
    /// </summary>
    public float GetCurrentPullRatio() => currentPullDistance / maxPullDistance;

    /// <summary>
    /// 获取当前威力
    /// </summary>
    public float GetCurrentPower() => currentPower;
}
