using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 副本UI : 面板基类
{
    public 副本 当前副本;
    public 副本_房间管理 当前副本所有房间;
    public 副本_房间 上一个房间;
    public 副本_房间 当前所在房间;
    public 副本_房间_地图管理 地图管理;
    public 副本场景 副本场景;
    public 小地图UI 小地图;
    public 辅助线绘制 辅助线;
    List<GameObject> 交互按钮列表;
    public 副本交互按钮基类 当前锁定交互按钮;
    public bool 交互事件锁定;
    public void 进入副本场景() => 启动携程(初始化副本携程());
    private IEnumerator 初始化副本携程()
    {
        sj.副本UI = this;
        cd.初始化新存档();
        yield return null;
        yield return StartCoroutine(初始化副本相关资源());
        foreach(var r in cd.副本上阵单位数组)
        {
            r.存在单位 = false;
        }
        cd.副本上阵单位数组[0] = new 副本单位() { 角色属性 = cd.主角,存在单位 = true };
        yield return null;
        进入副本房间(当前副本所有房间.入口);
    }
    private IEnumerator 初始化副本相关资源()
    {
        // 副本数据
        当前副本 = new 副本("新副本", 副本难度.普通, 副本状态.未开始);
        当前副本所有房间 = 当前副本.副本房间管理;
        上一个房间 = null;
        // 小地图数据
        小地图 = 组件<小地图UI>("小地图UI");
        小地图.初始化(当前副本所有房间);
        sj.副本小地图 = 小地图;
        // 场景数据
        GameObject 场景对象 = 取.对象("副本/副本场景");
        副本场景 = 场景对象.GetComponent<副本场景>();
        辅助线 = 场景对象.GetComponent<辅助线绘制>();
        sj.副本辅助线 = 辅助线;
        // 交互按钮列表
        交互按钮列表 = new List<GameObject>();
        // 对象池数据
        对象池.创建对象池("预制体/副本/交互按钮", 20);
        对象池.创建对象池("预制体/模型/爱丽丝", 20);
        对象池.创建对象池("预制体/模型/矮人", 20);
        对象池.创建对象池("预制体/副本/血条", 20);
        对象池.创建对象池("预制体/副本/伤害字体对象", 20);
        对象池.创建对象池("预制体/副本/死亡动画", 20);
        yield break;
    }
    public void 进入副本房间(副本_房间 房间)
    {
        更新副本UI(房间);
        更新副本数据(房间);
        更新副本场景(房间);
        小地图.进入房间(房间);
    }

    private void 更新副本UI(副本_房间 房间)
    {
        // 清理交互按钮列表 挨个放回对象池
        foreach(var b in 交互按钮列表)
        {
            对象池.归还对象(b);
        }
        交互按钮列表.Clear();
    }

    private void 更新副本场景(副本_房间 房间)
    {
        // 场景地图数据
        地图管理 = 当前所在房间.房间地图;
        地图管理.初始化地图格子();
        地图管理.分配玩家单位坐标(cd.副本上阵单位数组, 房间, sj.副本UI.上一个房间);
        if (房间.首次进入房间)
        {
            房间.首次进入房间 = false;
            地图管理.分配房间单位坐标(房间, sj.副本UI.上一个房间);
        }
        辅助线.初始化辅助线网格_原点(30, 30);
        // 场景对象
        副本场景.进入房间(房间);
    }

    private void 更新副本数据(副本_房间 房间)
    {
        if (上一个房间 != null)
        {
            离开副本房间(上一个房间);
        }
        上一个房间 = 当前所在房间;
        当前所在房间 = 房间;
        if (房间.房间状态 != 副本房间状态.已探索)
        {
            房间.房间状态 = 副本房间状态.探索中;
        }
    }

    private void 离开副本房间(副本_房间 房间)
    {
        上一个房间 = 房间;
        辅助线.清理所有线条();
        小地图.离开房间(房间);
        副本场景.离开房间(房间);
    }

    public void 前往副本房间(int 房间方向)
    {
        副本_房间 目标房间 = null;
        switch(房间方向)
        {
            case 0:目标房间 = 当前副本所有房间.周边房间_上(当前所在房间);break;
            case 1:目标房间 = 当前副本所有房间.周边房间_下(当前所在房间);break;
            case 2:目标房间 = 当前副本所有房间.周边房间_左(当前所在房间);break;
            case 3:目标房间 = 当前副本所有房间.周边房间_右(当前所在房间);break;
        }
        if (目标房间 != null)
        {
            离开副本房间(当前所在房间);
            进入副本房间(目标房间);
        }
        else
        {
            Debug.LogWarning($"无法前往{房间方向}方向，该方向没有房间");
        }
    }

    public void 创建交互按钮<T>(副本玩家单位 玩家单位, 技能类 技能 = null) where  T : 副本交互按钮基类
    {
        GameObject 按钮 = 对象池.取出对象("预制体/副本/交互按钮");
        按钮.transform.SetParent(变换("交互按钮区域"), false);
        按钮.AddComponent<T>().初始化按钮(玩家单位, 技能);
        交互按钮列表.Add(按钮);
    }
    public void 清理交互按钮()
    {
        当前锁定交互按钮 = null;
        foreach(var b in 交互按钮列表)
        {
            Destroy(b.GetComponent<副本交互按钮基类>());
            对象池.归还对象(b);
        }
        交互按钮列表.Clear();
    }

    public void 刷新副本房间状态()
    {
        小地图.更新交互按钮区域状态(当前所在房间);
    }

    public void 显示游戏结束弹窗()
    {
        // 清理副本相关场景
        清理交互按钮();
        Destroy(小地图.g);
        Destroy(副本场景.g);
        // 关闭副本UI
        UI管理器.关闭UI("副本UI");
        // 显示主城UI
        UI管理器.显示UI<主城UI>("主城UI", UI层级.弹窗,o => o.进入());
    }
}