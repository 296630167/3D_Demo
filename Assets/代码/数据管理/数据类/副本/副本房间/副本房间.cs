public class 副本房间
{
    public int 行;
    public int 列;
    public 副本房间类型 房间类型;
    public 副本房间状态 房间状态;
    public 副本房间路径类型 路径类型;
    public bool 上;
    public bool 下;
    public bool 左;
    public bool 右;
    // 副本地图相关数据
    public 副本房间地图 房间地图;
    public 副本房间(int 行, int 列)
    {
        this.行 = 行;
        this.列 = 列;
        this.房间类型 = 副本房间类型.不存在;
        this.房间状态 = 副本房间状态.未探索;
        this.路径类型 = 副本房间路径类型.阻挡;
        this.上 = false;
        this.下 = false;
        this.左 = false;
        this.右 = false;
        房间地图 = new 副本房间地图();
    }
}