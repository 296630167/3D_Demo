using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 交互按钮_移动 : 交互按钮_基类
{
    private 副本_房间_地图_格子 上一次目标格子;
    public override void 初始化按钮(副本玩家单位 单位, 技能类 技能 = null)
    {
        base.初始化按钮(单位, 技能);
        按钮文本("移");
    }
    protected override void 检测鼠标所在格子()
    {
        var 鼠标所在格子 = 场景.检测鼠标所在格子();
        if (单位.选择最近的可移动格子(鼠标所在格子, out 副本_房间_地图_格子 目标格子))
        {
            if (目标格子 != 上一次目标格子)
            {
                单位.朝向目标格子(目标格子);
                辅助线.绘制移动目标区域(目标格子.场景坐标, true);
                辅助线.绘制移动路径(单位);
                上一次目标格子 = 目标格子;
                sj.副本UI.显示消耗行动力提示文本_自定义($"花费行动力：{单位.本次移动消耗行动力}");
            }
        }
        else
        {
            if (上一次目标格子 != null)
            {
                辅助线.绘制移动目标区域(Vector3.zero, false);
                辅助线.绘制移动路径(null);
                上一次目标格子 = null;
                sj.副本UI.隐藏消耗行动力提示文本();
            }
        }
    }
    protected override void 鼠标点击格子()
    {
        交互携程 = 启动携程(监听单位移动携程());
    }

    private IEnumerator 监听单位移动携程()
    {
        取消锁定();
        yield return 启动携程(单位.开始移动携程());
        取消锁定(true);
        交互携程 = null;
    }

    protected override void 点击交互按钮()
    {
        if (单位.可移动范围字典.Count == 0)
        {
            管理器.取消锁定(true);
            return;
        }
        辅助线.绘制移动范围区域(单位);
    }

    protected override void 离开交互按钮()
    {
        辅助线.绘制移动范围区域(null);
        sj.副本UI.隐藏消耗行动力提示文本();
    }

    protected override void 进入交互按钮()
    {
        辅助线.绘制移动范围区域(单位);
    }

    protected override void 取消交互按钮()
    {
        辅助线.绘制移动范围区域(null);
        辅助线.绘制移动目标区域(Vector3.zero, false);
        辅助线.绘制移动路径(null);
        上一次目标格子 = null;
        sj.副本UI.隐藏消耗行动力提示文本();
    }
}
