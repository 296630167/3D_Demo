using UnityEngine;

public class 交互按钮_结束 : 交互按钮_基类
{
    public override void 初始化按钮(副本玩家单位 单位, 技能类 技能 = null)
    {
        base.初始化按钮(单位, 技能);
        按钮文本("结");
    }

    protected override void 点击交互按钮()
    {
        管理器.取消锁定(false);
        单位.回合结束();
    }
}