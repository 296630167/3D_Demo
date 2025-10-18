using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class 指针事件数据扩展
{
    public static GameObject 获取当前悬停对象(this BaseEventData 数据)
    {
        PointerEventData 指针数据 = 数据 as PointerEventData;
        return 指针数据?.pointerCurrentRaycast.gameObject;
    }
    
    public static List<RaycastResult> 获取所有射线检测结果(this BaseEventData 数据)
    {
        PointerEventData 指针数据 = 数据 as PointerEventData;
        if (指针数据 != null)
        {
            List<RaycastResult> 射线结果列表 = new List<RaycastResult>();
            EventSystem.current.RaycastAll(指针数据, 射线结果列表);
            return 射线结果列表;
        }
        return new List<RaycastResult>();
    }
    
    public static List<RaycastResult> 过滤射线检测结果(this BaseEventData 数据, System.Func<GameObject, bool> 过滤条件)
    {
        List<RaycastResult> 所有结果 = 数据.获取所有射线检测结果();
        List<RaycastResult> 过滤结果 = new List<RaycastResult>();
        for (int i = 0; i < 所有结果.Count; i++)
        {
            if (所有结果[i].gameObject != null && 过滤条件(所有结果[i].gameObject))
            {
                过滤结果.Add(所有结果[i]);
            }
        }
        return 过滤结果;
    }
    
    public static List<GameObject> 获取过滤对象列表(this BaseEventData 数据, System.Func<GameObject, bool> 过滤条件)
    {
        List<RaycastResult> 过滤结果 = 数据.过滤射线检测结果(过滤条件);
        List<GameObject> 对象列表 = new List<GameObject>(过滤结果.Count);
        for (int i = 0; i < 过滤结果.Count; i++)
        {
            对象列表.Add(过滤结果[i].gameObject);
        }
        return 对象列表;
    }
    
    public static List<T> 获取过滤组件列表<T>(this BaseEventData 数据, System.Func<GameObject, bool> 过滤条件 = null) where T : Component
    {
        List<RaycastResult> 所有结果 = 数据.获取所有射线检测结果();
        List<T> 组件列表 = new List<T>();
        for (int i = 0; i < 所有结果.Count; i++)
        {
            GameObject 对象 = 所有结果[i].gameObject;
            if (对象 != null)
            {
                T 组件 = 对象.GetComponent<T>();
                if (组件 != null && (过滤条件 == null || 过滤条件(对象)))
                {
                    组件列表.Add(组件);
                }
            }
        }
        return 组件列表;
    }
    
    public static List<RaycastResult> 获取指定标签的射线检测结果(this BaseEventData 数据, string 标签)
    {
        List<RaycastResult> 所有结果 = 数据.获取所有射线检测结果();
        List<RaycastResult> 过滤结果 = new List<RaycastResult>();
        for (int i = 0; i < 所有结果.Count; i++)
        {
            if (所有结果[i].gameObject != null && 所有结果[i].gameObject.CompareTag(标签))
            {
                过滤结果.Add(所有结果[i]);
            }
        }
        return 过滤结果;
    }
    
    public static List<GameObject> 获取指定标签的对象列表(this BaseEventData 数据, string 标签)
    {
        List<RaycastResult> 过滤结果 = 数据.获取指定标签的射线检测结果(标签);
        List<GameObject> 对象列表 = new List<GameObject>(过滤结果.Count);
        for (int i = 0; i < 过滤结果.Count; i++)
        {
            对象列表.Add(过滤结果[i].gameObject);
        }
        return 对象列表;
    }
    
    public static List<T> 获取指定标签的组件列表<T>(this BaseEventData 数据, string 标签) where T : Component
    {
        List<RaycastResult> 所有结果 = 数据.获取所有射线检测结果();
        List<T> 组件列表 = new List<T>();
        for (int i = 0; i < 所有结果.Count; i++)
        {
            GameObject 对象 = 所有结果[i].gameObject;
            if (对象 != null && 对象.CompareTag(标签))
            {
                T 组件 = 对象.GetComponent<T>();
                if (组件 != null)
                {
                    组件列表.Add(组件);
                }
            }
        }
        return 组件列表;
    }
}