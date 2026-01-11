using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 副本玩家单位 : 副本单位脚本
{
    public bool 交互按钮可点击;
    public override void 初始化(副本单位 单位)
    {
        base.初始化(单位);
        单位.所属阵营 = 所属阵营.玩家阵营;
    }
    public override void 回合开始()
    {
        base.回合开始();
        初始化交互按钮UI();
        // 玩家单位回合制开始时 要在 副本UI里 的 交互按钮列表 里 创建各种交互按钮
        // 分别是 移动 防御 跳过 还要 各种技能
        // 每个交互按钮 都要绑定单独的鼠标进入 离开 点击锁定 取消锁定 方法
        // 在这里测试下刚才写的 群攻技能范围 绘制方法

    }
    public override void 回合结束()
    {
        交互按钮可点击 = false;
        // sj.新副本UI.清理交互按钮();
        sj.副本交互按钮管理.清理按钮();
        base.回合结束();
    }

    private void 初始化交互按钮UI()
    {
        sj.副本交互按钮管理.初始化管理器();
        sj.副本交互按钮管理.创建按钮<交互按钮_移动>(this);
        sj.副本交互按钮管理.创建按钮<交互按钮_单体技能>(this, sj.技能数据.选择技能(1));
        sj.副本交互按钮管理.创建按钮<交互按钮_范围技能>(this, sj.技能数据.选择技能(2));
        sj.副本交互按钮管理.创建按钮<交互按钮_结束>(this);
    }
    protected override IEnumerator 使用单体技能攻击敌人后携程(副本单位脚本 敌人, 技能类 技能)
    {
        yield return 启动携程(base.使用单体技能攻击敌人后携程(敌人, 技能));
        交互按钮可点击 = true;
    }
    public override IEnumerator 先移动再使用单体技能攻击敌人携程(副本单位脚本 敌人, 技能类 技能)
    {
        单位.移动终点格子 = 检测敌人移动后是否在范围内(敌人.单位, 技能.射程 * 1.9f);
        计算移动路径(单位.移动终点格子);
        计算当前路径行动力消耗();
        yield return 启动携程(移动前携程());
        yield return 启动携程(移动中携程());
        yield return 启动携程(base.移动后携程());
        yield return 启动携程(使用单体技能攻击敌人携程(敌人, 技能));
        交互按钮可点击 = true;
    }
}
