using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class 副本交互按钮_移动 : 副本交互按钮基类
{
    public override void 初始化按钮(副本玩家单位 单位,技能类 技能 = null)
    {
        base.初始化按钮(单位);
        按钮文本("移");
        // 按钮图标();
    }

    protected override void 鼠标点击格子()
    {
        取消点击交互按钮();
        单位.开始移动();
    }

    protected override void 检测鼠标所在格子()
    {
        var 鼠标所在格子 = sj.副本场景.检测鼠标所在格子();
        if(单位.选择最近的可移动格子(鼠标所在格子,out 副本_房间_地图_格子 目标格子))
        {
            单位.朝向目标格子(目标格子);
            sj.副本辅助线.绘制移动目标区域(目标格子.场景坐标, true);
            sj.副本辅助线.绘制移动路径(单位);
        }
        else
        {
            sj.副本辅助线.绘制移动目标区域(Vector3.zero, false);
            sj.副本辅助线.绘制移动路径(null);
        }
    }

    protected override void 取消交互按钮()
    {
        sj.副本辅助线.绘制移动范围区域(null);
        sj.副本辅助线.绘制移动目标区域(Vector3.zero, false);
        sj.副本辅助线.绘制移动路径(null);
    }
    protected override void 点击交互按钮()
    {
        if(单位.可移动范围字典.Count ==0)
        {
            取消点击交互按钮();
            return;
        }
        sj.副本辅助线.绘制移动范围区域(单位);
    }
    protected override void 悬停交互按钮()
    {
        print("鼠标悬停移动按钮");
    }
    protected override void 离开交互按钮()
    {
        sj.副本辅助线.绘制移动范围区域(null);
    }
    protected override void 进入交互按钮()
    {
        sj.副本辅助线.绘制移动范围区域(单位);
    }
}