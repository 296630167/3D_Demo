using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 角色类
{
    public int id;
    public string 名称;
    public int 力量;
    public int 智力;
    public int 敏捷;
    public int 耐力;
    public float 暴击概率;
    public float 暴击伤害倍率;
    public float 闪避修正;
    public float 魔法伤害修正;
    public int 血量上限修正;
    public int 移动格子修正;
    public int 物理抗性;
    public int 火焰抗性;
    public int 冰冻抗性;
    public int 腐败抗性;
    public int 行动力;
    public float 闪避 => 敏捷 * 闪避修正;
    public float 魔法伤害 => 智力 * 魔法伤害修正;
    public int 血量上限 => 耐力 * 血量上限修正;
    public int 可移动格子距离 => 敏捷 + 移动格子修正;
}