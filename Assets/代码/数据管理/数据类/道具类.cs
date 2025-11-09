using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 道具类
{
    public int id;
    public string 名称;
    public 大类 大类;
    public 小类 小类;
    public string 介绍;
    public int 长度;
    public int 高度;
    public 品质 品质;
    public 道具交易点 固定交易点;
    public int 数量;
    public int 数量上限;
    public int 价格;
    public int 行;
    public int 列;
    public string 图标路径;

    public 道具类()
    {
        大类 = 大类.消耗品;
        小类 = 小类.药水;
        品质 = 品质.普通;
        固定交易点 = 道具交易点.无;
        长度 = 1;
        高度 = 1;
        数量 = 1;
    }
}
#region 消耗品
[Serializable]
public class 消耗品 : 道具类
{
    public 消耗品():base()
    {
        大类 = 大类.消耗品;
    }
}
[Serializable]
public class 药水 : 消耗品
{
    public 回复类型 回复类型;
    public int 回复数值;
    
    public 药水():base()
    {
        大类 = 大类.消耗品;
        小类 = 小类.药水;
    }
}
[Serializable]
public class 卷轴 : 消耗品
{
    public 技能类 卷轴技能;
    
    public 卷轴():base()
    {
        大类 = 大类.消耗品;
        小类 = 小类.卷轴;
    }
}
#endregion
#region 装备
[Serializable]
public class 装备 : 道具类
{
    public 装备():base()
    {
        大类 = 大类.装备;
    }
}
[Serializable]
public class 武器 : 装备
{
    public List<(属性, float)> 属性倍率 = new List<(属性, float)>();
    public int 耐力需求;
    public float 命中加成;
    public float 暴击率;
    public int 耐久;
    public List<技能类> 装备附带技能列表 = new List<技能类>();
    public Dictionary<材料, int> 打造所需材料字典 = new Dictionary<材料, int>();
    public List<(属性伤害类型, int)> 固定伤害列表 = new List<(属性伤害类型, int)>();
    public List<(属性伤害类型, float)> 比例伤害列表 = new List<(属性伤害类型, float)>();
    public float 穿甲;
    public float 魔法加成;
    
    public 武器():base()
    {
        大类 = 大类.装备;
    }
}
[Serializable]
public class 单手武器 : 武器
{
    public 单手武器():base()
    {
        大类 = 大类.装备;
        小类 = 小类.单手武器;
    }
}
[Serializable]
public class 双手武器 : 武器
{
    public 双手武器():base()
    {
        大类 = 大类.装备;
        小类 = 小类.双手武器;
    }
}
[Serializable]
public class 远程武器 : 武器
{
    public 远程武器():base()
    {
        大类 = 大类.装备;
        小类 = 小类.远程武器;
    }
}
[Serializable]
public class 法杖 : 武器
{
    public 法杖():base()
    {
        大类 = 大类.装备;
        小类 = 小类.法杖;
    }
}
[Serializable]
public class 辅助武器 : 装备
{
    public List<(属性, int)> 属性增幅 = new List<(属性, int)>();
    public int 耐力需求;
    public int 耐久;
    public List<int> 装备附带技能列表 = new List<int>();
    public List<(int, int)> 打造所需材料列表 = new List<(int, int)>();
    
    public 辅助武器():base()
    {
        大类 = 大类.装备;
        小类 = 小类.辅助武器;
    }
}
#endregion
#region 材料
[Serializable]
public class 材料 : 道具类
{
    public 材料():base()
    {
        大类 = 大类.材料;
    }
}
[Serializable]
public class 草药 : 材料
{
    public 草药():base()
    {
        大类 = 大类.材料;
        小类 = 小类.草药;
    }
}
[Serializable]
public class 锻造材料 : 材料
{
    public 锻造材料():base()
    {
        大类 = 大类.材料;
        小类 = 小类.锻造材料;
    }
}
#endregion
#region 补给
[Serializable]
public class 补给 : 道具类
{
    public 补给():base()
    {
        大类 = 大类.补给;
        小类 = 小类.补给;
    }
}
#endregion