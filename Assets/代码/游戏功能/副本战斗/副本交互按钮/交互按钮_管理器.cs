using System;
using System.Collections.Generic;
using UnityEngine;
public class 交互按钮_管理器 : 基
{
    public List<交互按钮_基类> 交互按钮列表 = new List<交互按钮_基类>();
    public 交互按钮_基类 当前锁定按钮;
    public bool 已锁定交互按钮;
    public bool 未锁定交互按钮 => !已锁定交互按钮;
    public bool 交互按钮可点击;
    // public 辅助线
    public 辅助线绘制 辅助线 => sj.副本辅助线;
    public 副本场景 场景 => sj.副本场景;

    public void 初始化管理器()
    {
        已锁定交互按钮 = false;
        交互按钮可点击 = true;
    }
    protected override void 开始时()
    {
        sj.副本交互按钮管理 = this;
    }
    public void 创建按钮<T>(副本玩家单位 玩家单位, 技能类 技能 = null) where T : 交互按钮_基类
    {
        GameObject 按钮 = 对象池.取出对象("预制体/副本/交互按钮");
        按钮.transform.SetParent(t, false);
        var 组件 = 按钮.AddComponent<T>();
        组件.初始化按钮(玩家单位, 技能);
        交互按钮列表.Add(组件);
        交互按钮可点击 = true;
    }

    public void 清理按钮()
    {
        当前锁定按钮 = null;
        已锁定交互按钮 = false;
        交互按钮可点击 = false;
        for (int i = 0; i < 交互按钮列表.Count; i++)
        {
            var 组件 = 交互按钮列表[i];
            GameObject 对象 = 组件.g;
            if (组件 != null) 组件.清理按钮();
            对象池.归还对象(对象);
        }
        交互按钮列表.Clear();
    }

    public void 锁定(交互按钮_基类 按钮)
    {
        if (当前锁定按钮 != null) 当前锁定按钮.取消锁定();
        当前锁定按钮 = 按钮;
        已锁定交互按钮 = true;
        交互按钮可点击 = false;
    }
    public void 取消锁定(bool 交互按钮状态 = false)
    {
        当前锁定按钮 = null;
        已锁定交互按钮 = false;
        交互按钮可点击 = 交互按钮状态;
    }
}
