using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 杂货铺 : 背包基类
{
    public 杂货铺(List<道具类> 道具列表) : base(背包类型.商店, 道具列表)
    {
    }
}