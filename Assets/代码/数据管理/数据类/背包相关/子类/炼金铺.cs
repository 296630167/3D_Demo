using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 炼金铺 : 背包基类
{
    public 炼金铺(List<道具类> 道具列表) : base(背包类型.商店, 道具列表)
    {
    }
}