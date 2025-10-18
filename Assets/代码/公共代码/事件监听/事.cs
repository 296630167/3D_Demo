using System;
using System.Collections.Generic;
using UnityEngine;

public static class 事
{
    #region 事件数据结构
    private static readonly Dictionary<string, List<Action>> 无参事件字典 = new Dictionary<string, List<Action>>();
    private static readonly Dictionary<string, List<Action<object>>> 有参事件字典 = new Dictionary<string, List<Action<object>>>();
    private static readonly Dictionary<string, List<Action<object[]>>> 多参事件字典 = new Dictionary<string, List<Action<object[]>>>();
    #endregion

    #region 事件注册方法
    public static void 注册事件(string 事件名, Action 回调)
    {
        if (!无参事件字典.ContainsKey(事件名))
        {
            无参事件字典[事件名] = new List<Action>();
        }
        无参事件字典[事件名].Add(回调);
    }

    public static void 注册事件<T>(string 事件名, Action<T> 回调)
    {
        if (!有参事件字典.ContainsKey(事件名))
        {
            有参事件字典[事件名] = new List<Action<object>>();
        }
        有参事件字典[事件名].Add(obj => 回调((T)obj));
    }

    public static void 注册事件(string 事件名, Action<object[]> 回调)
    {
        if (!多参事件字典.ContainsKey(事件名))
        {
            多参事件字典[事件名] = new List<Action<object[]>>();
        }
        多参事件字典[事件名].Add(回调);
    }
    #endregion

    #region 事件注销方法
    public static void 注销事件(string 事件名, Action 回调)
    {
        if (无参事件字典.ContainsKey(事件名))
        {
            无参事件字典[事件名].Remove(回调);
            if (无参事件字典[事件名].Count == 0)
            {
                无参事件字典.Remove(事件名);
            }
        }
    }

    public static void 注销事件<T>(string 事件名, Action<T> 回调)
    {
        if (有参事件字典.ContainsKey(事件名))
        {
            有参事件字典[事件名].RemoveAll(action => action.Method == 回调.Method && action.Target == 回调.Target);
            if (有参事件字典[事件名].Count == 0)
            {
                有参事件字典.Remove(事件名);
            }
        }
    }

    public static void 注销事件(string 事件名, Action<object[]> 回调)
    {
        if (多参事件字典.ContainsKey(事件名))
        {
            多参事件字典[事件名].Remove(回调);
            if (多参事件字典[事件名].Count == 0)
            {
                多参事件字典.Remove(事件名);
            }
        }
    }

    public static void 注销所有事件(string 事件名)
    {
        无参事件字典.Remove(事件名);
        有参事件字典.Remove(事件名);
        多参事件字典.Remove(事件名);
    }

    public static void 清空所有事件()
    {
        无参事件字典.Clear();
        有参事件字典.Clear();
        多参事件字典.Clear();
    }
    #endregion

    #region 事件触发方法
    public static void 触发事件(string 事件名)
    {
        if (无参事件字典.ContainsKey(事件名))
        {
            var 事件列表 = 无参事件字典[事件名];
            for (int i = 事件列表.Count - 1; i >= 0; i--)
            {
                try
                {
                    事件列表[i]?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"事件触发异常: {事件名} - {e.Message}");
                }
            }
        }
    }

    public static void 触发事件<T>(string 事件名, T 参数)
    {
        if (有参事件字典.ContainsKey(事件名))
        {
            var 事件列表 = 有参事件字典[事件名];
            for (int i = 事件列表.Count - 1; i >= 0; i--)
            {
                try
                {
                    事件列表[i]?.Invoke(参数);
                }
                catch (Exception e)
                {
                    Debug.LogError($"事件触发异常: {事件名} - {e.Message}");
                }
            }
        }
    }

    public static void 触发事件(string 事件名, params object[] 参数)
    {
        if (多参事件字典.ContainsKey(事件名))
        {
            var 事件列表 = 多参事件字典[事件名];
            for (int i = 事件列表.Count - 1; i >= 0; i--)
            {
                try
                {
                    事件列表[i]?.Invoke(参数);
                }
                catch (Exception e)
                {
                    Debug.LogError($"事件触发异常: {事件名} - {e.Message}");
                }
            }
        }
    }

    public static void 延迟触发事件(string 事件名, float 延迟秒数)
    {
        Mono方法管理器.延迟执行(延迟秒数, () => 触发事件(事件名));
    }

    public static void 延迟触发事件<T>(string 事件名, T 参数, float 延迟秒数)
    {
        Mono方法管理器.延迟执行(延迟秒数, () => 触发事件(事件名, 参数));
    }

    public static void 延迟触发事件(string 事件名, float 延迟秒数, params object[] 参数)
    {
        Mono方法管理器.延迟执行(延迟秒数, () => 触发事件(事件名, 参数));
    }
    #endregion

    #region 事件查询方法
    public static bool 事件是否存在(string 事件名)
    {
        return 无参事件字典.ContainsKey(事件名) ||
               有参事件字典.ContainsKey(事件名) ||
               多参事件字典.ContainsKey(事件名);
    }

    public static int 获取事件监听数量(string 事件名)
    {
        int 数量 = 0;
        if (无参事件字典.ContainsKey(事件名)) 数量 += 无参事件字典[事件名].Count;
        if (有参事件字典.ContainsKey(事件名)) 数量 += 有参事件字典[事件名].Count;
        if (多参事件字典.ContainsKey(事件名)) 数量 += 多参事件字典[事件名].Count;
        return 数量;
    }

    public static List<string> 获取所有事件名称()
    {
        var 所有事件 = new HashSet<string>();
        foreach (var 键 in 无参事件字典.Keys) 所有事件.Add(键);
        foreach (var 键 in 有参事件字典.Keys) 所有事件.Add(键);
        foreach (var 键 in 多参事件字典.Keys) 所有事件.Add(键);
        return new List<string>(所有事件);
    }
    #endregion

    #region MonoBehaviour扩展方法（融合事件系统扩展）
    public static void 监听事件(this MonoBehaviour 对象, string 事件名, Action 回调)
    {
        注册事件(事件名, 回调);
    }

    public static void 监听事件<T>(this MonoBehaviour 对象, string 事件名, Action<T> 回调)
    {
        注册事件(事件名, 回调);
    }

    public static void 监听事件(this MonoBehaviour 对象, string 事件名, Action<object[]> 回调)
    {
        注册事件(事件名, 回调);
    }

    public static void 停止监听事件(this MonoBehaviour 对象, string 事件名, Action 回调)
    {
        注销事件(事件名, 回调);
    }

    public static void 停止监听事件<T>(this MonoBehaviour 对象, string 事件名, Action<T> 回调)
    {
        注销事件(事件名, 回调);
    }

    public static void 停止监听事件(this MonoBehaviour 对象, string 事件名, Action<object[]> 回调)
    {
        注销事件(事件名, 回调);
    }

    public static void 停止监听所有事件(this MonoBehaviour 对象, string 事件名)
    {
        注销所有事件(事件名);
    }

    public static void 触发事件(this MonoBehaviour 对象, string 事件名)
    {
        触发事件(事件名);
    }

    public static void 触发事件<T>(this MonoBehaviour 对象, string 事件名, T 参数)
    {
        触发事件(事件名, 参数);
    }

    public static void 触发事件(this MonoBehaviour 对象, string 事件名, params object[] 参数)
    {
        触发事件(事件名, 参数);
    }

    public static void 延迟触发事件(this MonoBehaviour 对象, string 事件名, float 延迟秒数)
    {
        延迟触发事件(事件名, 延迟秒数);
    }

    public static void 延迟触发事件<T>(this MonoBehaviour 对象, string 事件名, T 参数, float 延迟秒数)
    {
        延迟触发事件(事件名, 参数, 延迟秒数);
    }

    public static void 监听(string 事件名, Action 回调)
    {
        注册事件(事件名, 回调);
    }

    public static void 监听<T>(string 事件名, Action<T> 回调)
    {
        注册事件(事件名, 回调);
    }

    public static void 停止监听(string 事件名, Action 回调)
    {
        注销事件(事件名, 回调);
    }

    public static void 停止监听<T>(string 事件名, Action<T> 回调)
    {
        注销事件(事件名, 回调);
    }

    public static void 触发(string 事件名)
    {
        触发事件(事件名);
    }

    public static void 触发<T>(string 事件名, T 参数)
    {
        触发事件(事件名, 参数);
    }

    public static void 触发(string 事件名, params object[] 参数)
    {
        触发事件(事件名, 参数);
    }

    public static void 延迟触发(string 事件名, float 延迟秒数)
    {
        延迟触发事件(事件名, 延迟秒数);
    }

    public static void 延迟触发<T>(string 事件名, T 参数, float 延迟秒数)
    {
        延迟触发事件(事件名, 参数, 延迟秒数);
    }
    #endregion
}