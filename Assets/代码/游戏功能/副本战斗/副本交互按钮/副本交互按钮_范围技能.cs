using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class 副本交互按钮_范围技能 : 副本交互按钮基类
{
    public override void 初始化按钮(副本玩家单位 单位,技能类 技能)
    {
        base.初始化按钮(单位, 技能);
        按钮文本("群");
    }

    protected override void 交互按钮每帧更新事件()
    {
    }
    protected override void 取消交互按钮()
    {

    }
    protected override void 点击交互按钮()
    {
    }

    protected override void 悬停交互按钮()
    {
    }

    protected override void 离开交互按钮()
    {
    }

    protected override void 进入交互按钮()
    {
    }
}