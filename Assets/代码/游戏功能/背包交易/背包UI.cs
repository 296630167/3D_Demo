using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.UI;

public class 背包UI : 面板基类
{
    public 背包类型 背包类型;
    public 背包操作接口 背包接口;
    public Dictionary<道具类,背包UI道具> 道具字典 = new Dictionary<道具类,背包UI道具>();
    public Transform 道具拖拽时容器;
    public Transform 道具默认容器;
    public 背包UI 鼠标所在背包 => UI管理器.获取UI脚本<交易UI>("交易UI").鼠标所在背包();
    private Dictionary<RectTransform,Vector2Int> 格子字典 = new Dictionary<RectTransform, Vector2Int>();
    private RectTransform[,] 格子数组 = new RectTransform[7, 7];
    private Vector2 上次鼠标位置;
    private Vector2Int? 缓存格子位置;
    // 测试代码 - 鼠标悬停高亮功能相关字段
    // private Vector2Int? 上次高亮格子;
    public virtual void 初始化背包(背包操作接口 背包接口数据,Transform 道具拖拽时容器)
    {
        this.背包类型 = 背包接口数据.获取背包类型();
        this.背包接口 = 背包接口数据;
        this.道具拖拽时容器 = 道具拖拽时容器;
        this.道具默认容器 = 变换("道具容器");
        初始化背包格子();
        初始化背包道具();
    }
    public void 初始化背包格子()
    {
        for (int 格子索引 = 0; 格子索引 < 49; 格子索引++)
        {
            RectTransform 格子RT = 变换("格子容器").GetChild(格子索引).GetComponent<RectTransform>();
            int 行 = 格子索引 / 7;
            int 列 = 格子索引 % 7;
            格子字典[格子RT] = new Vector2Int(行, 列);
            格子数组[行, 列] = 格子RT;
        }
    }
    public virtual void 初始化背包道具()
    {
        变换("道具容器").清理();
        道具字典 = new Dictionary<道具类,背包UI道具>();
        foreach (道具类 道具 in 背包接口.获取背包数据().道具列表)
        {
            GameObject 道具UI对象 = 取.对象("背包/道具UI", 变换("道具容器"));
            背包UI道具 道具脚本 = 道具UI对象.GetComponent<背包UI道具>();
            if (道具脚本 == null)
            {
                道具脚本 = 道具UI对象.AddComponent<背包UI道具>();
            }
            道具脚本.初始化(道具, this);
            道具脚本.拖拽时父级 = 道具拖拽时容器;
            道具字典[道具] = 道具脚本;
        }
    }
    public Vector2Int? 获取鼠标所在格子()
    {
        Vector2 当前鼠标位置 = Input.mousePosition;
        if (当前鼠标位置 == 上次鼠标位置)
        {
            return 缓存格子位置;
        }
        上次鼠标位置 = 当前鼠标位置;
        foreach (var 格子项 in 格子字典)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(格子项.Key, 当前鼠标位置))
            {
                缓存格子位置 = 格子项.Value;
                return 缓存格子位置;
            }
        }
        缓存格子位置 = null;
        return null;
    }
    public RectTransform 获取格子对象(int 行, int 列)
    {
        if (行 >= 0 && 行 < 7 && 列 >= 0 && 列 < 7)
        {
            return 格子数组[行, 列];
        }
        return null;
    }
    public RectTransform 获取格子对象(Vector2Int 位置)
    {
        return 获取格子对象(位置.x, 位置.y);
    }
    // 测试代码 - 设置格子颜色方法（用于鼠标悬停高亮效果）
    /*
    private void 设置格子颜色(Vector2Int 位置, Color 颜色)
    {
        RectTransform 格子对象 = 获取格子对象(位置);
        if (格子对象 != null)
        {
            Image 格子图像 = 格子对象.GetComponent<Image>();
            if (格子图像 != null)
            {
                格子图像.color = 颜色;
            }
        }
    }
    */
    protected override void Update()
    {
        base.Update();
        
        // 测试代码 - 鼠标悬停高亮功能和调试输出
        /*
        Vector2Int? 当前格子位置 = 获取鼠标所在格子();
        
        if (当前格子位置 != 上次高亮格子)
        {
            if (上次高亮格子.HasValue)
            {
                设置格子颜色(上次高亮格子.Value, Color.black);
            }
            
            if (当前格子位置.HasValue)
            {
                设置格子颜色(当前格子位置.Value, Color.green);
                print($"鼠标所在格子: ({当前格子位置.Value.x}, {当前格子位置.Value.y})");
                print(格子数组[当前格子位置.Value.x, 当前格子位置.Value.y]);
            }
            
            上次高亮格子 = 当前格子位置;
        }
        */
    }
}