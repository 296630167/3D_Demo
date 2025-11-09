using System;
using UnityEngine;
public enum 数据加载状态 { 未开始, 加载中, 成功, 失败 }
public class 数据管理器 : 游戏数据管理器基类<数据管理器>
{
    public 道具管理 道具数据;
    public 技能管理 技能数据;
    public 角色管理 角色数据;
    public 技能树管理 技能树数据;
    public 玩家存档 当前存档;
    private 数据加载状态 _当前加载状态 = 数据加载状态.未开始;
    private int _成功加载数量 = 0;
    private int _总数据数量 = 4;
    public 副本UI 副本UI;
    public 副本场景 副本场景;
    public 小地图UI 副本小地图;
    public 辅助线绘制 副本辅助线;

    public 数据加载状态 当前加载状态 => _当前加载状态;
    protected override void 初始化核心系统()
    {
        // 初始化对象池系统
        初始化对象池();
        技能数据.初始化字典();
        角色数据.初始化字典();
        技能树数据.初始化字典();
        道具数据.初始化字典();
        //_当前加载状态 = 数据加载状态.加载中;
        //_成功加载数量 = 0;
        //_当前加载状态 = _成功加载数量 == _总数据数量 ? 数据加载状态.成功 : 数据加载状态.失败;
    }

    private void 初始化对象池()
    {
        // 这里可以预创建一些常用的对象池
        // 例如：对象池.创建对象池("预制体/子弹", 50);
        // 例如：对象池.创建对象池("预制体/特效", 20);
        
        Debug.Log("对象池系统初始化完成");
    }
    private T 加载数据<T>(string 路径) where T : UnityEngine.Object
    {
        var 数据 = 取.资源<T>($"数据/{路径}");
        if (数据 != null) _成功加载数量++;
        return 数据;
    }
    public bool 是否所有数据加载完成() => _当前加载状态 == 数据加载状态.成功;
    public bool 是否加载失败() => _当前加载状态 == 数据加载状态.失败;
    public float 获取加载进度() => _总数据数量 == 0 ? 0f : (float)_成功加载数量 / _总数据数量;

    public void 创建存档()
    {
        当前存档 = new 玩家存档();
        当前存档.初始化新存档();
    }
}