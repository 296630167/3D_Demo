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
    public 数据加载状态 当前加载状态 => _当前加载状态;
    protected override void 初始化核心系统()
    {
        //_当前加载状态 = 数据加载状态.加载中;
        //_成功加载数量 = 0;
        //道具数据 = 加载数据<道具管理>("道具数据");
        //技能数据 = 加载数据<技能管理>("技能数据");
        //角色数据 = 加载数据<角色管理>("角色数据");
        //技能树数据 = 加载数据<技能树管理>("技能树数据");
        //_当前加载状态 = _成功加载数量 == _总数据数量 ? 数据加载状态.成功 : 数据加载状态.失败;
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