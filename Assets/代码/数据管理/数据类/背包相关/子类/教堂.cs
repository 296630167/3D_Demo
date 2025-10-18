using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 教堂 : 背包基类
{
    public 教堂(List<道具类> 道具列表) : base(背包类型.商店, 道具列表)
    {
    }
}