using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public static class 事
{
    private const string 默认分组 = "默认";
    private static readonly ConcurrentDictionary<string, Action> 唯一事件字典 = new ConcurrentDictionary<string, Action>();
    private static readonly ConcurrentDictionary<string, Delegate> 唯一事件泛型字典 = new ConcurrentDictionary<string, Delegate>();
    private static readonly ConcurrentDictionary<string, Action<object[]>> 唯一事件多参字典 = new ConcurrentDictionary<string, Action<object[]>>();
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Action>>> 分组事件字典 = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Action>>>();
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Delegate>>> 分组事件泛型字典 = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Delegate>>>();
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Action<object[]>>>> 分组事件多参字典 = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentBag<Action<object[]>>>>();
    private static readonly object 锁对象 = new object();

    public static void 注册唯一事件(string 事件名称, Action 事件)
    {
        if (string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        唯一事件字典.AddOrUpdate(事件名称, 事件, (key, oldValue) => 事件);
    }

    public static void 注册唯一事件<T>(string 事件名称, Action<T> 事件)
    {
        if (string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        唯一事件泛型字典.AddOrUpdate(事件名称, 事件, (key, oldValue) => 事件);
    }

    public static void 注册唯一事件(string 事件名称, Action<object[]> 事件)
    {
        if (string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        唯一事件多参字典.AddOrUpdate(事件名称, 事件, (key, oldValue) => 事件);
    }

    public static void 触发唯一事件(string 事件名称)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        if (唯一事件字典.TryGetValue(事件名称, out Action 事件))
        {
            try { 事件?.Invoke(); }
            catch (Exception e) { Debug.LogError($"唯一事件触发异常: {事件名称} - {e.Message}"); }
        }
    }

    public static void 触发唯一事件<T>(string 事件名称, T 参数)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        if (唯一事件泛型字典.TryGetValue(事件名称, out Delegate 事件))
        {
            try { (事件 as Action<T>)?.Invoke(参数); }
            catch (Exception e) { Debug.LogError($"唯一事件触发异常: {事件名称} - {e.Message}"); }
        }
    }

    public static void 触发唯一事件(string 事件名称, params object[] 参数)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        if (唯一事件多参字典.TryGetValue(事件名称, out Action<object[]> 事件))
        {
            try { 事件?.Invoke(参数); }
            catch (Exception e) { Debug.LogError($"唯一事件触发异常: {事件名称} - {e.Message}"); }
        }
    }

    public static void 移除唯一事件(string 事件名称)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        唯一事件字典.TryRemove(事件名称, out _);
        唯一事件泛型字典.TryRemove(事件名称, out _);
        唯一事件多参字典.TryRemove(事件名称, out _);
    }

    public static void 注册事件(string 事件名称, Action 事件) => 注册事件(默认分组, 事件名称, 事件);

    public static void 注册事件(string 分组名称, string 事件名称, Action 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        分组事件字典.GetOrAdd(分组名称, _ => new ConcurrentDictionary<string, ConcurrentBag<Action>>()).GetOrAdd(事件名称, _ => new ConcurrentBag<Action>()).Add(事件);
    }

    public static void 注册事件<T>(string 事件名称, Action<T> 事件) => 注册事件(默认分组, 事件名称, 事件);

    public static void 注册事件<T>(string 分组名称, string 事件名称, Action<T> 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        分组事件泛型字典.GetOrAdd(分组名称, _ => new ConcurrentDictionary<string, ConcurrentBag<Delegate>>()).GetOrAdd(事件名称, _ => new ConcurrentBag<Delegate>()).Add(事件);
    }

    public static void 注册事件(string 事件名称, Action<object[]> 事件) => 注册事件(默认分组, 事件名称, 事件);

    public static void 注册事件(string 分组名称, string 事件名称, Action<object[]> 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        分组事件多参字典.GetOrAdd(分组名称, _ => new ConcurrentDictionary<string, ConcurrentBag<Action<object[]>>>()).GetOrAdd(事件名称, _ => new ConcurrentBag<Action<object[]>>()).Add(事件);
    }

    public static void 触发事件(string 事件名称)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        foreach (var 分组 in 分组事件字典.Values)
            if (分组.TryGetValue(事件名称, out var 事件列表))
                foreach (var 事件 in 事件列表)
                    try { 事件?.Invoke(); } catch (Exception e) { Debug.LogError($"分组事件触发异常: {事件名称} - {e.Message}"); }
    }

    public static void 触发事件<T>(string 事件名称, T 参数)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        foreach (var 分组 in 分组事件泛型字典.Values)
            if (分组.TryGetValue(事件名称, out var 事件列表))
                foreach (var 事件 in 事件列表)
                    try { (事件 as Action<T>)?.Invoke(参数); } catch (Exception e) { Debug.LogError($"分组事件触发异常: {事件名称} - {e.Message}"); }
    }

    public static void 触发事件(string 事件名称, params object[] 参数)
    {
        if (string.IsNullOrEmpty(事件名称)) return;
        foreach (var 分组 in 分组事件多参字典.Values)
            if (分组.TryGetValue(事件名称, out var 事件列表))
                foreach (var 事件 in 事件列表)
                    try { 事件?.Invoke(参数); } catch (Exception e) { Debug.LogError($"分组事件触发异常: {事件名称} - {e.Message}"); }
    }

    public static void 移除事件(string 事件名称, Action 事件) => 移除事件(默认分组, 事件名称, 事件);

    public static void 移除事件(string 分组名称, string 事件名称, Action 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        lock (锁对象)
        {
            if (分组事件字典.TryGetValue(分组名称, out var 分组字典) && 分组字典.TryGetValue(事件名称, out var 事件列表))
            {
                var 新列表 = new ConcurrentBag<Action>();
                foreach (var 现有事件 in 事件列表) if (!ReferenceEquals(现有事件, 事件)) 新列表.Add(现有事件);
                分组字典.TryUpdate(事件名称, 新列表, 事件列表);
            }
        }
    }

    public static void 移除事件<T>(string 事件名称, Action<T> 事件) => 移除事件(默认分组, 事件名称, 事件);

    public static void 移除事件<T>(string 分组名称, string 事件名称, Action<T> 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        lock (锁对象)
        {
            if (分组事件泛型字典.TryGetValue(分组名称, out var 分组字典) && 分组字典.TryGetValue(事件名称, out var 事件列表))
            {
                var 新列表 = new ConcurrentBag<Delegate>();
                foreach (var 现有事件 in 事件列表) if (!ReferenceEquals(现有事件, 事件)) 新列表.Add(现有事件);
                分组字典.TryUpdate(事件名称, 新列表, 事件列表);
            }
        }
    }

    public static void 移除事件(string 事件名称, Action<object[]> 事件) => 移除事件(默认分组, 事件名称, 事件);

    public static void 移除事件(string 分组名称, string 事件名称, Action<object[]> 事件)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称) || 事件 == null) return;
        lock (锁对象)
        {
            if (分组事件多参字典.TryGetValue(分组名称, out var 分组字典) && 分组字典.TryGetValue(事件名称, out var 事件列表))
            {
                var 新列表 = new ConcurrentBag<Action<object[]>>();
                foreach (var 现有事件 in 事件列表) if (!ReferenceEquals(现有事件, 事件)) 新列表.Add(现有事件);
                分组字典.TryUpdate(事件名称, 新列表, 事件列表);
            }
        }
    }

    public static void 移除事件(string 分组名称)
    {
        if (string.IsNullOrEmpty(分组名称)) return;
        分组事件字典.TryRemove(分组名称, out _);
        分组事件泛型字典.TryRemove(分组名称, out _);
        分组事件多参字典.TryRemove(分组名称, out _);
    }

    public static bool 唯一事件是否存在(string 事件名称) => !string.IsNullOrEmpty(事件名称) && (唯一事件字典.ContainsKey(事件名称) || 唯一事件泛型字典.ContainsKey(事件名称) || 唯一事件多参字典.ContainsKey(事件名称));

    public static bool 分组事件是否存在(string 事件名称)
    {
        if (string.IsNullOrEmpty(事件名称)) return false;
        foreach (var 分组 in 分组事件字典.Values) if (分组.ContainsKey(事件名称)) return true;
        foreach (var 分组 in 分组事件泛型字典.Values) if (分组.ContainsKey(事件名称)) return true;
        foreach (var 分组 in 分组事件多参字典.Values) if (分组.ContainsKey(事件名称)) return true;
        return false;
    }

    public static int 获取分组事件监听数量(string 分组名称, string 事件名称)
    {
        if (string.IsNullOrEmpty(分组名称) || string.IsNullOrEmpty(事件名称)) return 0;
        int 数量 = 0;
        if (分组事件字典.TryGetValue(分组名称, out var 无参分组) && 无参分组.TryGetValue(事件名称, out var 无参事件列表)) 数量 += 无参事件列表.Count;
        if (分组事件泛型字典.TryGetValue(分组名称, out var 有参分组) && 有参分组.TryGetValue(事件名称, out var 有参事件列表)) 数量 += 有参事件列表.Count;
        if (分组事件多参字典.TryGetValue(分组名称, out var 多参分组) && 多参分组.TryGetValue(事件名称, out var 多参事件列表)) 数量 += 多参事件列表.Count;
        return 数量;
    }

    public static List<string> 获取所有分组名称()
    {
        var 分组集合 = new HashSet<string>();
        foreach (var 分组名 in 分组事件字典.Keys) 分组集合.Add(分组名);
        foreach (var 分组名 in 分组事件泛型字典.Keys) 分组集合.Add(分组名);
        foreach (var 分组名 in 分组事件多参字典.Keys) 分组集合.Add(分组名);
        return new List<string>(分组集合);
    }

    public static void 清空所有事件()
    {
        唯一事件字典.Clear();
        唯一事件泛型字典.Clear();
        唯一事件多参字典.Clear();
        分组事件字典.Clear();
        分组事件泛型字典.Clear();
        分组事件多参字典.Clear();
    }

    public static string 获取系统状态()
    {
        var 状态信息 = new System.Text.StringBuilder();
        状态信息.AppendLine("=== 事件系统状态 ===");
        状态信息.AppendLine($"唯一事件数量: {唯一事件字典.Count + 唯一事件泛型字典.Count + 唯一事件多参字典.Count}");
        状态信息.AppendLine($"分组数量: {获取所有分组名称().Count}");
        foreach (var 分组名 in 获取所有分组名称())
        {
            int 总事件数 = 0;
            if (分组事件字典.TryGetValue(分组名, out var 无参分组)) foreach (var 事件列表 in 无参分组.Values) 总事件数 += 事件列表.Count;
            if (分组事件泛型字典.TryGetValue(分组名, out var 有参分组)) foreach (var 事件列表 in 有参分组.Values) 总事件数 += 事件列表.Count;
            if (分组事件多参字典.TryGetValue(分组名, out var 多参分组)) foreach (var 事件列表 in 多参分组.Values) 总事件数 += 事件列表.Count;
            状态信息.AppendLine($"分组 '{分组名}' 事件数量: {总事件数}");
        }
        return 状态信息.ToString();
    }
}