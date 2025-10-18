using System;
using System.Collections.Generic;
using UnityEngine;

public enum UI层级
{
    背景,
    主界面,
    弹窗,
    提示,
    顶层,
    引导,
    调试
}
public enum 当前页面
{
    首页,
    主城,
    铁匠铺,
    教堂,
    雕塑,
    酒馆,
    杂货铺,
    炼金铺,
    交易
}
public class UI管理器 : MonoBehaviour
{
    private static UI管理器 实例;
    public static UI管理器 获取实例
    {
        get
        {
            if (实例 == null)
            {
                GameObject 管理器对象 = new GameObject("UI管理器");
                实例 = 管理器对象.AddComponent<UI管理器>();
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(管理器对象);
                }
            }
            return 实例;
        }
    }

    private Canvas 背景画布;
    private Canvas 主界面画布;
    private Canvas 弹窗画布;
    private Canvas 提示画布;
    private Canvas 顶层画布;
    private Canvas 引导画布;
    private Canvas 调试画布;

    private Dictionary<UI层级, Canvas> UI层级字典;
    private Dictionary<string, GameObject> UI对象缓存 = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> 预制体缓存 = new Dictionary<string, GameObject>();

    public static 当前页面 当前页面;

    private void Awake()
    {
        if (实例 == null)
        {
            实例 = this;
            DontDestroyOnLoad(gameObject);
            初始化UI层级();
        }
        else if (实例 != this)
        {
            Destroy(gameObject);
        }
    }

    private void 初始化UI层级()
    {
        try
        {
            背景画布 = 创建画布(UI层级.背景, 0);
            主界面画布 = 创建画布(UI层级.主界面, 100);
            弹窗画布 = 创建画布(UI层级.弹窗, 200);
            提示画布 = 创建画布(UI层级.提示, 300);
            顶层画布 = 创建画布(UI层级.顶层, 400);
            引导画布 = 创建画布(UI层级.引导, 500);
            调试画布 = 创建画布(UI层级.调试, 600);
            UI层级字典 = new Dictionary<UI层级, Canvas> { { UI层级.背景, 背景画布 }, { UI层级.主界面, 主界面画布 }, { UI层级.弹窗, 弹窗画布 }, { UI层级.提示, 提示画布 }, { UI层级.顶层, 顶层画布 }, { UI层级.引导, 引导画布 }, { UI层级.调试, 调试画布 } };
            创建EventSystem();
            显示UI<开始UI>("开始UI", UI层级.主界面);
        }
        catch { }
    }

    private Canvas 创建画布(UI层级 层级, int 排序顺序)
    {
        try
        {
            var 画布对象 = new GameObject($"UI画布_{层级}");
            画布对象.transform.SetParent(transform, false);
            var 新画布 = 画布对象.AddComponent<Canvas>();
            画布对象.AddComponent<UnityEngine.UI.CanvasScaler>();
            画布对象.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            配置画布属性(新画布, 排序顺序);
            return 新画布;
        }
        catch { return null; }
    }

    private void 配置画布属性(Canvas 画布, int 排序顺序)
    {
        画布.renderMode = RenderMode.ScreenSpaceOverlay;
        画布.sortingOrder = 排序顺序;
        画布.pixelPerfect = false;
        var 缩放器 = 画布.GetComponent<UnityEngine.UI.CanvasScaler>();
        if (缩放器 != null)
        {
            缩放器.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            缩放器.referenceResolution = new Vector2(1920, 1080);
            缩放器.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            缩放器.matchWidthOrHeight = 0.5f;
        }
    }

    public static GameObject 显示UI(string UI名称, UI层级 层级 = UI层级.主界面) => 获取实例.内部创建并显示UI(UI名称, 层级);
    public static GameObject 显示UI<T>(string UI名称, UI层级 层级, System.Action<T> 回调方法 = null) where T : MonoBehaviour => 获取实例.内部创建并显示UI<T>(UI名称, 层级, 回调方法);
    public static GameObject 显示UI(string 预制体路径, string UI名称, UI层级 层级 = UI层级.主界面) => 获取实例.内部创建并显示UI(预制体路径, UI名称, 层级);
    public static GameObject 显示UI<T>(string 预制体路径, string UI名称, UI层级 层级, System.Action<T> 回调方法 = null) where T : MonoBehaviour => 获取实例.内部创建并显示UI<T>(预制体路径, UI名称, 层级, 回调方法);

    public GameObject 内部创建并显示UI(string UI名称, UI层级 层级)
    {
        if (string.IsNullOrEmpty(UI名称)) return null;
        if (UI对象缓存.TryGetValue(UI名称, out GameObject 缓存UI))
        {
            if (缓存UI != null)
            {
                缓存UI.SetActive(true);
                缓存UI.transform.SetAsLastSibling();
                return 缓存UI;
            }
            else UI对象缓存.Remove(UI名称);
        }
        var 目标画布 = 获取指定层级画布(层级);
        if (目标画布 == null) return null;
        var 预制体 = 加载UI预制体(UI名称);
        if (预制体 == null) return null;
        var UI对象 = Instantiate(预制体, 目标画布.transform);
        UI对象.name = UI名称;
        UI对象.SetActive(true);
        UI对象.transform.SetAsLastSibling();
        UI对象缓存[UI名称] = UI对象;
        return UI对象;
    }

    public GameObject 内部创建并显示UI<T>(string UI名称, UI层级 层级, System.Action<T> 回调方法) where T : MonoBehaviour
    {
        var UI对象 = 内部创建并显示UI(UI名称, 层级);
        if (UI对象 == null) return null;
        try
        {
            var 脚本组件 = UI对象.GetComponent<T>() ?? 自动绑定脚本组件<T>(UI对象, UI名称);
            if (脚本组件 != null && 回调方法 != null) 回调方法.Invoke(脚本组件);
        }
        catch { }
        return UI对象;
    }

    public GameObject 内部创建并显示UI(string 预制体路径, string UI名称, UI层级 层级)
    {
        if (string.IsNullOrEmpty(预制体路径) || string.IsNullOrEmpty(UI名称)) return null;
        if (UI对象缓存.TryGetValue(UI名称, out GameObject 缓存UI))
        {
            if (缓存UI != null)
            {
                缓存UI.SetActive(true);
                缓存UI.transform.SetAsLastSibling();
                return 缓存UI;
            }
            else UI对象缓存.Remove(UI名称);
        }
        var 目标画布 = 获取指定层级画布(层级);
        if (目标画布 == null) return null;
        var 预制体 = 加载UI预制体(预制体路径, UI名称);
        if (预制体 == null) return null;
        var UI对象 = Instantiate(预制体, 目标画布.transform);
        UI对象.name = UI名称;
        UI对象.SetActive(true);
        UI对象.transform.SetAsLastSibling();
        UI对象缓存[UI名称] = UI对象;
        return UI对象;
    }

    public GameObject 内部创建并显示UI<T>(string 预制体路径, string UI名称, UI层级 层级, System.Action<T> 回调方法) where T : MonoBehaviour
    {
        var UI对象 = 内部创建并显示UI(预制体路径, UI名称, 层级);
        if (UI对象 == null) return null;
        try
        {
            var 脚本组件 = UI对象.GetComponent<T>() ?? 自动绑定脚本组件<T>(UI对象, UI名称);
            if (脚本组件 != null && 回调方法 != null) 回调方法.Invoke(脚本组件);
        }
        catch { }
        return UI对象;
    }

    private GameObject 加载UI预制体(string UI名称)
    {
        if (预制体缓存.TryGetValue(UI名称, out GameObject 缓存预制体))
        {
            if (缓存预制体 != null) return 缓存预制体;
            else 预制体缓存.Remove(UI名称);
        }
        var 预制体 = Resources.Load<GameObject>($"UI/{UI名称}");
        if (预制体 == null) return null;
        预制体缓存[UI名称] = 预制体;
        return 预制体;
    }

    private GameObject 加载UI预制体(string 预制体路径, string UI名称)
    {
        var 缓存键 = $"{预制体路径}_{UI名称}";
        if (预制体缓存.TryGetValue(缓存键, out GameObject 缓存预制体))
        {
            if (缓存预制体 != null) return 缓存预制体;
            else 预制体缓存.Remove(缓存键);
        }
        var 预制体 = Resources.Load<GameObject>(预制体路径);
        if (预制体 == null) return null;
        预制体缓存[缓存键] = 预制体;
        return 预制体;
    }

    public Canvas 获取指定层级画布(UI层级 层级) => UI层级字典.TryGetValue(层级, out Canvas 画布) ? 画布 : null;
    public static void 关闭UI(string UI名称) => 获取实例.内部关闭UI(UI名称);

    public void 内部关闭UI(string UI名称)
    {
        if (string.IsNullOrEmpty(UI名称)) return;
        if (UI对象缓存.TryGetValue(UI名称, out GameObject UI对象))
        {
            if (UI对象 != null) UI对象.SetActive(false);
            else UI对象缓存.Remove(UI名称);
        }
    }

    public static Canvas 获取画布(UI层级 层级) => 获取实例.获取指定层级画布(层级);
    public static GameObject 获取UI(string UI名称) => 获取实例.内部获取UI(UI名称);

    public GameObject 内部获取UI(string UI名称)
    {
        if (string.IsNullOrEmpty(UI名称)) return null;
        if (UI对象缓存.TryGetValue(UI名称, out GameObject UI对象))
        {
            if (UI对象 != null) return UI对象;
            else UI对象缓存.Remove(UI名称);
        }
        return null;
    }

    public static T 获取UI脚本<T>(string UI名称) where T : MonoBehaviour => 获取实例.内部获取UI脚本<T>(UI名称);

    public T 内部获取UI脚本<T>(string UI名称) where T : MonoBehaviour
    {
        var UI对象 = 内部获取UI(UI名称);
        if (UI对象 == null) return null;
        var 脚本组件 = UI对象.GetComponent<T>() ?? 自动绑定脚本组件<T>(UI对象, UI名称);
        return 脚本组件;
    }

    public static Component 获取UI脚本(string UI名称, Type 脚本类型) => 获取实例.内部获取UI脚本(UI名称, 脚本类型);

    public Component 内部获取UI脚本(string UI名称, Type 脚本类型)
    {
        if (脚本类型 == null) return null;
        var UI对象 = 内部获取UI(UI名称);
        if (UI对象 == null) return null;
        var 脚本组件 = UI对象.GetComponent(脚本类型) ?? 自动绑定脚本组件(UI对象, 脚本类型, UI名称);
        return 脚本组件;
    }

    public static bool UI是否存在(string UI名称) => 获取实例.内部UI是否存在(UI名称);
    public bool 内部UI是否存在(string UI名称) => !string.IsNullOrEmpty(UI名称) && UI对象缓存.ContainsKey(UI名称) && UI对象缓存[UI名称] != null;

    private T 自动绑定脚本组件<T>(GameObject UI对象, string UI名称) where T : MonoBehaviour
    {
        if (UI对象 == null) return null;
        try
        {
            var 现有组件 = UI对象.GetComponent<T>();
            if (现有组件 != null) return 现有组件;
            var 新组件 = UI对象.AddComponent<T>();
            return 新组件;
        }
        catch { return null; }
    }

    private Component 自动绑定脚本组件(GameObject UI对象, Type 脚本类型, string UI名称)
    {
        if (UI对象 == null || 脚本类型 == null) return null;
        try
        {
            var 现有组件 = UI对象.GetComponent(脚本类型);
            if (现有组件 != null) return 现有组件;
            var 新组件 = UI对象.AddComponent(脚本类型);
            return 新组件;
        }
        catch { return null; }
    }

    private void 创建EventSystem()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            DontDestroyOnLoad(eventSystemObject);
        }
    }
}