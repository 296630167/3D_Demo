using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region 人物属性
public enum 属性
{
    力量,
    智力,
    敏捷,
    耐力,
    暴击概率,
    暴击伤害倍率,
    行动力,
    物理,
    火焰,
    冰冻,
    腐败,
}
public enum 技能伤害类型
{
    物理伤害=1,
    冰冻伤害=2,
    火焰伤害=3,
    腐败伤害=4,
    武器伤害 = 5,
    真实伤害 = 6,
}
public enum 属性伤害类型
{
    物理伤害=1,
    冰冻伤害=2,
    火焰伤害=3,
    腐败伤害=4
}
#endregion
#region  道具
public enum 大类
{
    消耗品 = 1,
    装备 = 2,
    材料 = 3,
    补给 = 4
}
public enum 小类
{
    药水=1,
    卷轴,
    单手武器,
    双手武器,
    远程武器,
    法杖,
    辅助武器,
    草药,
    锻造材料,
    补给
}
public enum 回复类型
{
    回复生命值 = 1,
    回复法力值 = 2,
    回复护盾值 = 3,
}
public enum 品质
{
    普通 = 1,
    优秀 = 2,
    稀有 = 3,
    史诗 = 4,
    传说 = 5,
    神话 = 6
}
public enum 道具交易点
{
    无 = 0,
    炼金铺 = 1,
    杂货铺 = 2,
    铁匠铺 = 3,
    教堂 = 4,
}
public enum 背包类型
{
    仓库 = 1,
    背包 = 2,
    商店 = 3
}
#endregion
#region 技能
public enum 技能施法类型
{
    点选=1,
    范围=2,
}
public enum 技能施法对象
{
    自己=1,
    敌人=2,
    队友=3,
    所有人=4,
}
public enum 技能伤害计算方式
{
    战技=1,
    法术=2
}
public enum 技能施加效果
{
    无=1,
}
public enum 效果类型
{
    减益效果=1,
    增益效果=2,
    免疫效果=3,
    特殊效果=4,
}
// "1=不可叠加
// 2=可叠加回合数
// 3=可叠加属性影响数值和回合数
// 4=可叠加属性影响数值 不可叠加回合数"

public enum 叠加方式
{
    不可叠加=1,
    只叠加回合=2,
    叠加属性和回合=3,
    叠加属性不叠加回合=4,
}
public enum 效果触发条件
{
    立即生效=1,
    回合开始=2,
    回合结束=3,
    受到伤害=4,
    造成伤害=5,
}
#endregion