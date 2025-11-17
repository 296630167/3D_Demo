using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class 技能类
{
    public int id;
    public string 名称;
    public string 介绍;
    public 技能施法类型 施法类型;
    public int 施法次数;
    public 技能施法对象 施法对象;
    public int 消耗魔法;
    public float 射程;
    public float 命中;
    public int 消耗行动力;
    public 技能施加效果 施加效果;
    public int 基础伤害;
    public float 技能伤害倍率;
    public 技能伤害类型 伤害类型;
    public 技能伤害计算方式 伤害计算方式;
    public int 冷却时间;
    public float 作用范围;
    public string 图标路径;
    public Sprite 图标
    {
        get
        {
            if (string.IsNullOrEmpty(图标路径)) return null;
            return 取.图片(图标路径);
        }
    }
    public 技能类()
    {
        施加效果 = 技能施加效果.无;
        施法次数 = 1;
        射程 = 1f;
        命中 = 1f;
        消耗行动力 = 1;
        技能伤害倍率 = 1f;
        作用范围 = 0f;
    }
}