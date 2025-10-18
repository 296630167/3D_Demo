using System;
using UnityEngine;

public abstract class 游戏数据管理器基类<T> : MonoBehaviour where T : 游戏数据管理器基类<T>
{
    private static T _实例;
    private static readonly object _锁对象 = new object();
    
    public static T 实例
    {
        get
        {
            if (_实例 == null)
            {
                lock (_锁对象)
                {
                    if (_实例 == null)
                    {
                        GameObject 管理器对象 = new GameObject(typeof(T).Name);
                        _实例 = 管理器对象.AddComponent<T>();
                        DontDestroyOnLoad(管理器对象);
                    }
                }
            }
            return _实例;
        }
    }
    
    private bool _已初始化 = false;
    public bool 已初始化 => _已初始化;
    private void Awake()
    {
        if (_实例 == null)
        {
            _实例 = (T)this;
            DontDestroyOnLoad(gameObject);
            执行初始化();
        }
        else if (_实例 != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void 执行初始化()
    {
        if (_已初始化) return;
        _已初始化 = true;
        try
        {
            初始化核心系统();
            初始化扩展系统();
            Debug.Log($"{typeof(T).Name}初始化完成");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{typeof(T).Name}初始化失败: {ex.Message}");
        }
    }
    
    protected virtual void 初始化核心系统()
    {
        
    }
    
    protected virtual void 初始化扩展系统()
    {
    }
    
    private void OnDestroy()
    {
        if (_实例 == this)
        {
            _实例 = null;
        }
    }
}