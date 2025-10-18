using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 铁匠铺 : 背包基类
{
    public 铁匠铺(List<道具类> 道具列表) : base(背包类型.商店, 道具列表)
    {
    }
}