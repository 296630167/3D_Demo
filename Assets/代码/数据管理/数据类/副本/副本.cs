public class 副本
{
    public string 副本名称;
    public 副本难度 副本难度;
    public 副本状态 副本状态;
    public 副本_房间管理 副本房间管理;
    public 副本(string 副本名称 = "新副本",副本难度 副本难度 = 副本难度.普通, 副本状态 副本状态 = 副本状态.未开始)
    {
        this.副本名称 = 副本名称;
        this.副本难度 = 副本难度;
        this.副本状态 = 副本状态;
        this.副本房间管理 = new 副本_房间管理();
    }
}