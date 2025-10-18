using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public static class Mono方法管理器
{
    #region 内部MonoBehaviour实例
    private static Mono方法内核 _内核;
    private static Mono方法内核 内核
    {
        get
        {
            if (_内核 == null)
            {
                GameObject 管理器对象 = new GameObject("Mono方法管理器");
                _内核 = 管理器对象.AddComponent<Mono方法内核>();
                UnityEngine.Object.DontDestroyOnLoad(管理器对象);
            }
            return _内核;
        }
    }
    #endregion

    #region 帧事件管理
    public static void 添加普通帧事件(Action 事件)
    {
        内核.添加普通帧事件(事件);
    }

    public static void 添加固定帧事件(Action 事件)
    {
        内核.添加固定帧事件(事件);
    }

    public static void 添加延迟帧事件(Action 事件)
    {
        内核.添加延迟帧事件(事件);
    }

    public static void 移除普通帧事件(Action 事件)
    {
        内核.移除普通帧事件(事件);
    }

    public static void 移除固定帧事件(Action 事件)
    {
        内核.移除固定帧事件(事件);
    }

    public static void 移除延迟帧事件(Action 事件)
    {
        内核.移除延迟帧事件(事件);
    }

    public static void 清空普通帧事件()
    {
        内核.清空普通帧事件();
    }

    public static void 清空固定帧事件()
    {
        内核.清空固定帧事件();
    }

    public static void 清空延迟帧事件()
    {
        内核.清空延迟帧事件();
    }

    public static void 清空所有事件()
    {
        内核.清空所有事件();
    }
    #endregion

    #region 协程管理
    // public static Coroutine 开启协程(string 协程)
    // {
    //     return 内核.开启协程(协程);
    // }

    // public static Coroutine 开启协程(IEnumerator 协程)
    // {
    //     return 内核.开启协程(协程);
    // }

    // public static Coroutine 开启协程(string 协程, object 参数)
    // {
    //     return 内核.开启协程(协程, 参数);
    // }

    // public static void 结束协程(IEnumerator 协程)
    // {
    //     内核.结束协程(协程);
    // }

    // public static void 结束协程(Coroutine 协程)
    // {
    //     内核.结束协程(协程);
    // }

    // public static void 结束协程(string 协程)
    // {
    //     内核.结束协程(协程);
    // }

    public static void 结束所有协程()
    {
        内核.结束所有协程();
    }
    #endregion

    #region 延迟和重复执行
    public static void 延迟执行(float 秒数, Action 事件)
    {
        内核.延迟执行(秒数, 事件);
    }

    public static void 重复执行(int 次数, float 秒数, Action 事件)
    {
        内核.重复执行(次数, 秒数, 事件);
    }
    #endregion

    #region 扩展方法（融合Mono方法扩展功能）
    public static void 添加普通帧事件(this object 对象, Action 事件)
    {
        添加普通帧事件(事件);
    }

    public static void 添加固定帧事件(this object 对象, Action 事件)
    {
        添加固定帧事件(事件);
    }

    public static void 添加延迟帧事件(this object 对象, Action 事件)
    {
        添加延迟帧事件(事件);
    }

    public static void 移除普通帧事件(this object 对象, Action 事件)
    {
        移除普通帧事件(事件);
    }

    public static void 移除固定帧事件(this object 对象, Action 事件)
    {
        移除固定帧事件(事件);
    }

    public static void 移除延迟帧事件(this object 对象, Action 事件)
    {
        移除延迟帧事件(事件);
    }

    public static void 清空普通帧事件(this object 对象)
    {
        清空普通帧事件();
    }

    public static void 清空固定帧事件(this object 对象)
    {
        清空固定帧事件();
    }

    public static void 清空延迟帧事件(this object 对象)
    {
        清空延迟帧事件();
    }

    public static void 清空所有事件(this object 对象)
    {
        清空所有事件();
    }

    public static Coroutine 开启协程(this object 对象, string 协程)
    {
        return 开启协程(协程);
    }

    public static Coroutine 开启协程(this string 协程)
    {
        return 开启协程(协程);
    }

    public static Coroutine 开启协程(this object 对象, IEnumerator 协程)
    {
        return 开启协程(协程);
    }

    public static Coroutine 开启协程(this IEnumerator 协程)
    {
        return 开启协程(协程);
    }

    public static Coroutine 开启协程(this object 对象, string 协程, object 参数)
    {
        return 开启协程(协程, 参数);
    }

    public static Coroutine 开启协程(this string 协程, object 参数)
    {
        return 开启协程(协程, 参数);
    }

    public static void 结束协程(this IEnumerator 协程)
    {
        结束协程(协程);
    }

    public static void 结束协程(this Coroutine 协程)
    {
        结束协程(协程);
    }

    public static void 结束协程(this string 协程)
    {
        结束协程(协程);
    }

    public static void 结束所有协程(this object 对象)
    {
        结束所有协程();
    }

    public static void 延迟执行(this object 对象, float 秒数, Action 事件)
    {
        延迟执行(秒数, 事件);
    }

    public static void 延迟执行(this Action 事件, float 秒数)
    {
        延迟执行(秒数, 事件);
    }

    public static void 重复执行(this object 对象, int 次数, float 秒数, Action 事件)
    {
        重复执行(次数, 秒数, 事件);
    }

    public static void 重复执行(this Action 事件, int 次数, float 秒数)
    {
        重复执行(次数, 秒数, 事件);
    }
    #endregion
}

#region 内部MonoBehaviour实现
internal class Mono方法内核 : MonoBehaviour
{
    #region 事件列表
    private readonly List<Action> 普通帧事件列表 = new List<Action>();
    private readonly List<Action> 固定帧事件列表 = new List<Action>();
    private readonly List<Action> 延迟帧事件列表 = new List<Action>();
    #endregion

    #region Unity生命周期
    private void Update()
    {
        for (int i = 0; i < 普通帧事件列表.Count; i++)
        {
            普通帧事件列表[i]?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 固定帧事件列表.Count; i++)
        {
            固定帧事件列表[i]?.Invoke();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < 延迟帧事件列表.Count; i++)
        {
            延迟帧事件列表[i]?.Invoke();
        }
    }
    #endregion

    #region 事件管理方法
    public void 添加普通帧事件(Action 事件)
    {
        普通帧事件列表.Add(事件);
    }

    public void 添加固定帧事件(Action 事件)
    {
        固定帧事件列表.Add(事件);
    }

    public void 添加延迟帧事件(Action 事件)
    {
        延迟帧事件列表.Add(事件);
    }

    public void 移除普通帧事件(Action 事件)
    {
        普通帧事件列表.Remove(事件);
    }

    public void 移除固定帧事件(Action 事件)
    {
        固定帧事件列表.Remove(事件);
    }

    public void 移除延迟帧事件(Action 事件)
    {
        延迟帧事件列表.Remove(事件);
    }

    public void 清空普通帧事件()
    {
        普通帧事件列表.Clear();
    }

    public void 清空固定帧事件()
    {
        固定帧事件列表.Clear();
    }

    public void 清空延迟帧事件()
    {
        延迟帧事件列表.Clear();
    }

    public void 清空所有事件()
    {
        普通帧事件列表.Clear();
        固定帧事件列表.Clear();
        延迟帧事件列表.Clear();
    }
    #endregion

    #region 协程方法
    public Coroutine 开启协程(string 协程)
    {
        return StartCoroutine(协程);
    }

    public Coroutine 开启协程(IEnumerator 协程)
    {
        return StartCoroutine(协程);
    }

    public Coroutine 开启协程(string 协程, object 参数)
    {
        return StartCoroutine(协程, 参数);
    }

    public void 结束协程(IEnumerator 协程)
    {
        StopCoroutine(协程);
    }

    public void 结束协程(Coroutine 协程)
    {
        StopCoroutine(协程);
    }

    public void 结束协程(string 协程)
    {
        StopCoroutine(协程);
    }

    public void 结束所有协程()
    {
        StopAllCoroutines();
    }

    public void 延迟执行(float 秒数, Action 事件)
    {
        StartCoroutine(延迟执行协程(秒数, 事件));
    }

    private IEnumerator 延迟执行协程(float 秒数, Action 事件)
    {
        yield return new WaitForSeconds(秒数);
        事件?.Invoke();
    }

    public void 重复执行(int 次数, float 秒数, Action 事件)
    {
        StartCoroutine(重复执行协程(次数, 秒数, 事件));
    }

    private IEnumerator 重复执行协程(int 次数, float 秒数, Action 事件)
    {
        while (次数 != 0)
        {
            yield return new WaitForSeconds(秒数);
            事件?.Invoke();
            if (次数 != -1)
            {
                次数--;
            }
        }
    }
    #endregion
}
#endregion