using System.Collections.Generic;
using UnityEngine;

public static class 对象池
{
    private static Dictionary<string, Queue<GameObject>> _对象池字典 = new Dictionary<string, Queue<GameObject>>();
    private static Dictionary<string, GameObject> _预制体缓存 = new Dictionary<string, GameObject>();
    private static Dictionary<GameObject, string> _对象来源映射 = new Dictionary<GameObject, string>();
    private static Transform _对象池容器;

    private static Transform 对象池容器
    {
        get
        {
            if (_对象池容器 == null)
            {
                var 容器对象 = new GameObject("对象池容器");
                Object.DontDestroyOnLoad(容器对象);
                _对象池容器 = 容器对象.transform;
            }
            return _对象池容器;
        }
    }

    public static void 创建对象池(string 预制体路径, int 初始数量 = 10)
    {
        if (_对象池字典.ContainsKey(预制体路径))
        {
            Debug.LogWarning($"对象池已存在: {预制体路径}");
            return;
        }

        var 预制体 = 取.资源<GameObject>(预制体路径);
        if (预制体 == null)
        {
            Debug.LogError($"无法加载预制体: {预制体路径}");
            return;
        }

        _预制体缓存[预制体路径] = 预制体;
        _对象池字典[预制体路径] = new Queue<GameObject>();

        for (int i = 0; i < 初始数量; i++)
        {
            var 对象 = Object.Instantiate(预制体, 对象池容器);
            对象.SetActive(false);
            _对象池字典[预制体路径].Enqueue(对象);
            _对象来源映射[对象] = 预制体路径;
        }

        Debug.Log($"创建对象池成功: {预制体路径}, 初始数量: {初始数量}");
    }

    public static GameObject 取出对象(string 预制体路径, Vector3 位置 = default, Quaternion 旋转 = default)
    {
        if (!_对象池字典.ContainsKey(预制体路径))
        {
            Debug.LogWarning($"对象池不存在，自动创建: {预制体路径}");
            创建对象池(预制体路径);
        }

        GameObject 对象;
        var 对象队列 = _对象池字典[预制体路径];

        if (对象队列.Count > 0)
        {
            对象 = 对象队列.Dequeue();
        }
        else
        {
            if (!_预制体缓存.ContainsKey(预制体路径))
            {
                Debug.LogError($"预制体缓存不存在: {预制体路径}");
                return null;
            }

            对象 = Object.Instantiate(_预制体缓存[预制体路径]);
            _对象来源映射[对象] = 预制体路径;
        }

        对象.transform.position = 位置;
        对象.transform.rotation = 旋转 == default ? Quaternion.identity : 旋转;
        对象.SetActive(true);

        return 对象;
    }

    public static void 归还对象(GameObject 对象)
    {
        if (对象 == null)
        {
            Debug.LogWarning("尝试归还空对象");
            return;
        }

        if (!_对象来源映射.ContainsKey(对象))
        {
            Debug.LogWarning($"对象不属于对象池，直接销毁: {对象.name}");
            Object.Destroy(对象);
            return;
        }

        string 预制体路径 = _对象来源映射[对象];
        对象.SetActive(false);
        对象.transform.SetParent(对象池容器);
        _对象池字典[预制体路径].Enqueue(对象);
    }

    public static void 清理对象池(string 预制体路径 = null)
    {
        if (预制体路径 == null)
        {
            foreach (var 队列 in _对象池字典.Values)
            {
                while (队列.Count > 0)
                {
                    var 对象 = 队列.Dequeue();
                    if (对象 != null)
                    {
                        Object.Destroy(对象);
                    }
                }
            }
            _对象池字典.Clear();
            _预制体缓存.Clear();
            _对象来源映射.Clear();
            
            if (_对象池容器 != null)
            {
                Object.Destroy(_对象池容器.gameObject);
                _对象池容器 = null;
            }
            
            Debug.Log("清理所有对象池完成");
        }
        else
        {
            if (_对象池字典.ContainsKey(预制体路径))
            {
                var 队列 = _对象池字典[预制体路径];
                while (队列.Count > 0)
                {
                    var 对象 = 队列.Dequeue();
                    if (对象 != null)
                    {
                        _对象来源映射.Remove(对象);
                        Object.Destroy(对象);
                    }
                }
                _对象池字典.Remove(预制体路径);
                _预制体缓存.Remove(预制体路径);
                
                Debug.Log($"清理对象池完成: {预制体路径}");
            }
        }
    }

    public static int 获取对象池数量(string 预制体路径)
    {
        return _对象池字典.ContainsKey(预制体路径) ? _对象池字典[预制体路径].Count : 0;
    }

    public static bool 对象池是否存在(string 预制体路径)
    {
        return _对象池字典.ContainsKey(预制体路径);
    }
}