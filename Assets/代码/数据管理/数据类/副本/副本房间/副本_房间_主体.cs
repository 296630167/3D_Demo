using System;
using System.Collections.Generic;
using System.Linq;

public partial class 副本_房间
{
    public int 行;
    public int 列;
    public 副本房间类型 房间类型;
    public 副本房间状态 房间状态;
    public 副本_房间路径类型 路径类型;
    public bool 首次进入房间;
    public bool 上;
    public bool 下;
    public bool 左;
    public bool 右;
    private 副本_房间_地图管理 _房间地图;
    public 副本_房间_地图管理 房间地图
    {
        get
        {
            if (_房间地图 == null)
            {
                _房间地图 = new 副本_房间_地图管理();
            }
            return _房间地图;
        }
    }
    public bool 可以离开当前房间;
    public List<副本建筑> 建筑列表 = new List<副本建筑>();
    public 副本_房间(int 行, int 列)
    {
        this.行 = 行;
        this.列 = 列;
        this.房间类型 = 副本房间类型.空地;
        this.房间状态 = 副本房间状态.未探索;
        this.路径类型 = 副本_房间路径类型.阻;
        this.上 = false;
        this.下 = false;
        this.左 = false;
        this.右 = false;
        this.首次进入房间 = true;
        this.可以离开当前房间 = true;
    }
}