using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 副本单位
{
    public 角色类 角色属性;
    public 副本_房间_地图_格子 所在格子;
    public 副本_房间_地图_格子 移动终点格子;
    // public 副本单位状态 状态;
    public bool 活着;
    public 副本单位行为状态 行为;
    public int 剩余行动力;
    public 所属阵营 所属阵营;
    public bool 存在单位;
    public 副本单位()
    {
        存在单位 = false;
    }
}
