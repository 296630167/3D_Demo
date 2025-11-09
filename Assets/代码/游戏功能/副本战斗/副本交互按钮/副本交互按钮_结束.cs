using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 副本交互按钮_结束 : 副本交互按钮基类
{
    public override void 初始化按钮(副本玩家单位 单位,技能类 技能 = null)
    {
        base.初始化按钮(单位);
        按钮文本("结");
        // 按钮图标();
    }
    protected override void 点击交互按钮()
    {
        print("回合结束了");
        单位.回合结束();
    }
}
