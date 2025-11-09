using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Net.Security;

public abstract class 基: MonoBehaviour
{
    public Transform t;
    public GameObject g;
    public RectTransform rt;
    // 更新sj的定义
    protected static 数据管理器 sj => 数据管理器.实例;
    protected static 玩家存档 cd => sj.当前存档;
    protected readonly Dictionary<string, GameObject> 子对象缓存 = new Dictionary<string, GameObject>();
    protected readonly Dictionary<(string, Type), Component> 组件缓存 = new Dictionary<(string, Type), Component>();
    #region 生命周期函数
    /// <summary>
    /// 对象实例化后立即执行，在所有Start()之前
    /// 执行次数：每个对象生命周期内只执行一次
    /// 用途：初始化组件引用、设置初始状态、单例模式实现
    /// </summary>
    protected virtual void Awake() => 启动时();
    
    /// <summary>
    /// 对象变为活跃状态时执行
    /// 执行次数：每次激活都会执行
    /// 用途：注册事件监听器、启动协程、重置临时状态、UI面板显示初始化
    /// </summary>
    protected virtual void OnEnable() => 激活时();
    
    /// <summary>
    /// 第一帧Update之前执行，确保所有对象的Awake都已执行完毕
    /// 执行次数：每个对象生命周期内只执行一次
    /// 用途：需要其他对象已初始化的逻辑、游戏开始设置、查找场景对象
    /// </summary>
    protected virtual void Start() => 开始时();
    /// <summary>
    /// 固定时间间隔执行（默认0.02秒/50Hz），不受帧率影响
    /// 执行次数：持续执行，独立于渲染帧率
    /// 用途：物理计算、刚体操作、需要稳定时间间隔的逻辑、网络同步计算
    /// 性能：时间复杂度应控制在O(1)或O(log n)
    /// </summary>
    protected virtual void FixedUpdate() => 固定更新();
    
    /// <summary>
    /// 每渲染帧执行一次，频率取决于帧率
    /// 执行次数：持续执行，与显示器刷新率和性能相关
    /// 用途：输入检测、UI更新、动画控制、游戏逻辑更新、相机控制
    /// 性能：避免复杂计算，时间复杂度应控制在O(1)或O(log n)
    /// </summary>
    protected virtual void Update() => 每帧更新();
    
    /// <summary>
    /// 所有Update()执行完毕后执行，确保所有对象的Update都已完成
    /// 执行次数：每帧执行一次
    /// 用途：相机跟随、UI元素位置调整、动画后处理、依赖其他对象Update结果的逻辑
    /// </summary>
    protected virtual void LateUpdate() => 延迟更新();
    
    /// <summary>
    /// 对象变为非活跃状态时执行
    /// 执行次数：每次禁用都会执行
    /// 用途：注销事件监听器、停止协程、清理临时资源、UI面板隐藏清理
    /// </summary>
    protected virtual void OnDisable() => 禁用时();
    
    /// <summary>
    /// 对象被销毁前执行
    /// 执行次数：对象生命周期内只执行一次
    /// 用途：释放资源、保存数据、清理静态引用、网络连接断开
    /// </summary>
    protected virtual void OnDestroy() => 销毁时();
    
    /// <summary>
    /// 应用失去焦点时执行（主要用于移动平台）
    /// 执行次数：每次暂停/恢复都会执行
    /// 用途：游戏暂停逻辑、保存游戏进度、音频静音处理、网络连接管理
    /// </summary>
    protected virtual void OnApplicationPause(bool 暂停状态) => 应用暂停时(暂停状态);
    
    /// <summary>
    /// 应用获得/失去焦点时执行（PC和移动平台都适用）
    /// 执行次数：每次焦点变化都会执行
    /// 用途：暂停/恢复游戏、调整音量、停止/启动某些更新、性能优化控制
    /// </summary>
    protected virtual void OnApplicationFocus(bool 焦点状态) => 应用焦点变化时(焦点状态);
    
    /// <summary>
    /// 应用程序退出前执行
    /// 执行次数：应用生命周期内只执行一次
    /// 用途：保存用户数据、清理临时文件、断开网络连接、释放系统资源
    /// </summary>
    protected virtual void OnApplicationQuit() => 应用退出时();
    #endregion 

    #region 抽象方法 - 子类必须实现
    //protected abstract void 启动时();
    //protected abstract void 激活时();
    //protected abstract void 开始时();
    #endregion

    #region 虚方法 - 子类可选择性重写    
    protected virtual void 启动时()
    {
        t = transform;
        g = gameObject;
        rt = t as RectTransform;
        缓存所有子对象();
    }
    protected virtual void 激活时()
    {
        注册事件监听();
    }
    protected virtual void 开始时()
    {
        StartCoroutine(开始时携程());
    }
    protected virtual IEnumerator 开始时携程()
    {
        yield return null;
    }
    protected virtual void 固定更新() { }
    protected virtual void 每帧更新() { }
    protected virtual void 延迟更新() { }
    protected virtual void 禁用时() { }
    protected virtual void 销毁时()
    {
        注销事件监听();
    }
    protected virtual void 应用暂停时(bool 暂停状态) { }
    protected virtual void 应用焦点变化时(bool 焦点状态) { }
    protected virtual void 应用退出时() { }
    #endregion
    #region 事件系统集成
    protected virtual void 注册事件监听()
    {
        
    }
    
    protected virtual void 注销事件监听()
    {

    }
    // 无参
    protected void 监听事件(string 分组名, string 事件名, Action 回调) => 事.注册事件(分组名, 事件名, 回调);
    protected void 监听事件(string 事件名, Action 回调) => 事.注册事件(事件名, 回调);
    protected void 监听唯一事件(string 事件名, Action 回调) => 事.注册唯一事件(事件名, 回调);
    protected void 触发事件(string 事件名) => 事.触发事件(事件名);
    protected void 触发唯一事件(string 事件名) => 事.触发唯一事件(事件名);
    protected void 移除事件(string 分组名) => 事.移除事件(分组名);
    protected void 移除事件(string 事件名,Action 回调) => 事.移除事件(事件名,回调);
    protected void 移除唯一事件(string 事件名) => 事.移除唯一事件(事件名);

    // 有参
    protected void 监听事件<T>(string 分组名, string 事件名, Action<T> 回调) => 事.注册事件(分组名, 事件名, 回调);
    protected void 监听事件<T>(string 事件名, Action<T> 回调) => 事.注册事件(事件名, 回调);
    protected void 监听唯一事件<T>(string 事件名, Action<T> 回调) => 事.注册唯一事件<T>(事件名, 回调);
    protected void 触发事件<T>(string 事件名, T 参数) => 事.触发事件(事件名, 参数);
    protected void 触发唯一事件<T>(string 事件名, T 参数) => 事.触发唯一事件(事件名, 参数);
    protected void 移除事件<T>(string 分组名, string 事件名, Action<T> 回调) => 事.移除事件(分组名, 事件名, 回调);
    protected void 移除事件<T>(string 事件名, Action<T> 回调) => 事.移除事件(事件名, 回调);
    #endregion
    #region 组件获取方法
    private void 缓存所有子对象()
    {
        子对象缓存.Clear();
        组件缓存.Clear();

        Transform[] 所有子对象 = GetComponentsInChildren<Transform>(true);

        foreach (Transform 子对象 in 所有子对象)
        {
            子对象缓存[子对象.name] = 子对象.gameObject;
            
            // 自动绑定按钮事件
            Button 按钮组件 = 子对象.GetComponent<Button>();
            if (按钮组件 != null)
            {
                自动绑定按钮事件(按钮组件, 子对象.name);
            }
        }
    }


    private void 自动绑定按钮事件(Button 按钮, string 按钮名称)
    {
        string 方法名 = 按钮名称;

        var 方法信息 = this.GetType().GetMethod(方法名, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (方法信息 != null && 方法信息.GetParameters().Length == 0)
        {
            按钮.onClick.RemoveAllListeners();
            按钮.onClick.AddListener(() => 方法信息.Invoke(this, null));
            //Debug.Log($"自动绑定按钮事件: {按钮名称} -> {方法名}()");
        }
    }

    protected void 刷新缓存()
    {
        缓存所有子对象();
    }
    protected T 组件<T>(string 对象名称) where T : Component
    {
        var 缓存键 = (对象名称, typeof(T));

        if (组件缓存.TryGetValue(缓存键, out var 缓存组件))
        {
            return (T)缓存组件;
        }

        if (子对象缓存.TryGetValue(对象名称, out var 目标对象))
        {
            T 组件 = 目标对象.GetComponent<T>();
            if (组件 != null)
            {
                组件缓存[缓存键] = 组件;
                return 组件;
            }
        }

        return null;
    }
    protected GameObject 对象(string 对象名称)
    {
        return 子对象缓存.TryGetValue(对象名称, out var 对象) ? 对象 : null;
    }
    protected Transform 变换(string 对象名称) => 组件<Transform>(对象名称);
    protected RectTransform 矩形变换(string 对象名称) => 组件<RectTransform>(对象名称);
    protected Button 按钮(string 对象名称) => 组件<Button>(对象名称);
    protected Text 文本(string 对象名称) => 组件<Text>(对象名称);
    protected TextMeshProUGUI TMP文本(string 对象名称) => 组件<TextMeshProUGUI>(对象名称);
    protected TextMeshPro TMP3D文本(string 对象名称) => 组件<TextMeshPro>(对象名称);
    protected TMP_InputField TMP输入框(string 对象名称) => 组件<TMP_InputField>(对象名称);
    protected TMP_Dropdown TMP下拉框(string 对象名称) => 组件<TMP_Dropdown>(对象名称);
    protected Image 图片(string 对象名称) => 组件<Image>(对象名称);
    protected Toggle 开关(string 对象名称) => 组件<Toggle>(对象名称);
    protected Slider 滑动条(string 对象名称) => 组件<Slider>(对象名称);
    protected InputField 输入框(string 对象名称) => 组件<InputField>(对象名称);
    protected Dropdown 下拉框(string 对象名称) => 组件<Dropdown>(对象名称);
    protected ScrollRect 滚动视图(string 对象名称) => 组件<ScrollRect>(对象名称);
    protected Canvas 画布(string 对象名称) => 组件<Canvas>(对象名称);
    protected CanvasGroup 画布组(string 对象名称) => 组件<CanvasGroup>(对象名称);
    protected Animator 动画器(string 对象名称) => 组件<Animator>(对象名称);
    protected Animation 动画(string 对象名称) => 组件<Animation>(对象名称);
    protected AudioSource 音频源(string 对象名称) => 组件<AudioSource>(对象名称);
    protected Rigidbody 刚体(string 对象名称) => 组件<Rigidbody>(对象名称);
    protected Rigidbody2D 刚体2D(string 对象名称) => 组件<Rigidbody2D>(对象名称);
    protected Collider 碰撞器(string 对象名称) => 组件<Collider>(对象名称);
    protected Collider2D 碰撞器2D(string 对象名称) => 组件<Collider2D>(对象名称);
    protected BoxCollider 盒子碰撞器(string 对象名称) => 组件<BoxCollider>(对象名称);
    protected SphereCollider 球体碰撞器(string 对象名称) => 组件<SphereCollider>(对象名称);
    protected CapsuleCollider 胶囊碰撞器(string 对象名称) => 组件<CapsuleCollider>(对象名称);
    protected MeshCollider 网格碰撞器(string 对象名称) => 组件<MeshCollider>(对象名称);
    protected Renderer 渲染器(string 对象名称) => 组件<Renderer>(对象名称);
    protected MeshRenderer 网格渲染器(string 对象名称) => 组件<MeshRenderer>(对象名称);
    protected SkinnedMeshRenderer 蒙皮网格渲染器(string 对象名称) => 组件<SkinnedMeshRenderer>(对象名称);
    protected SpriteRenderer 精灵渲染器(string 对象名称) => 组件<SpriteRenderer>(对象名称);
    protected LineRenderer 线条渲染器(string 对象名称) => 组件<LineRenderer>(对象名称);
    protected TrailRenderer 拖尾渲染器(string 对象名称) => 组件<TrailRenderer>(对象名称);
    protected ParticleSystem 粒子系统(string 对象名称) => 组件<ParticleSystem>(对象名称);
    #endregion
    // public virtual void 显示() => t.position = Vector2.zero;
    // public virtual void 隐藏() => t.position = new Vector2(3000, 3000);
    public virtual void 显示() => g.SetActive(true);
    public virtual void 隐藏() => g.SetActive(false);
    public virtual void 切换显示(bool 显示) => g.SetActive(显示);
    public virtual void 销毁() => Destroy(g);
    // 二次封装下 启动 和 关闭携程的方法 
    public virtual Coroutine 启动携程(IEnumerator 携程) => StartCoroutine(携程);
    public virtual void 关闭携程(Coroutine 携程) => StopCoroutine(携程);
    public virtual Coroutine 启动携程(string 携程名称) => StartCoroutine(携程名称);
    public virtual void 关闭携程(string 携程名称) => StopCoroutine(携程名称);

}