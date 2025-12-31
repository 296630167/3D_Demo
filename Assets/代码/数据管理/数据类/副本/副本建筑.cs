using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 副本建筑类 - 用于记录副本中的建筑物信息(如柱子、障碍物等)
/// </summary>
public class 副本建筑
{
    /// <summary>
    /// 建筑类型(柱子、墙壁等)
    /// </summary>
    public 副本建筑类型 建筑类型;
    
    /// <summary>
    /// 建筑所在的中心格子
    /// </summary>
    public 副本_房间_地图_格子 所在格子;
    
    /// <summary>
    /// 建筑占用的所有格子列表
    /// </summary>
    public List<副本_房间_地图_格子> 占用格子列表;
    
    /// <summary>
    /// 建筑占地行数
    /// </summary>
    public int 占地行数;
    
    /// <summary>
    /// 建筑占地列数
    /// </summary>
    public int 占地列数;
    
    public 副本建筑(副本建筑类型 类型, 副本_房间_地图_格子 中心格子, int 占地行数, int 占地列数)
    {
        this.建筑类型 = 类型;
        this.所在格子 = 中心格子;
        this.占地行数 = 占地行数;
        this.占地列数 = 占地列数;
        this.占用格子列表 = new List<副本_房间_地图_格子>();
    }
}

/// <summary>
/// 副本建筑类型枚举
/// </summary>
public enum 副本建筑类型
{
    柱子,
    墙壁,
    障碍物,
    装饰物
}
