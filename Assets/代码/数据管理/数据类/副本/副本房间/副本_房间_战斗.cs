using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class 副本_房间
{
    public int 敌人数量;
    public int 小怪数量;
    public int 精英数量;
    public int 首领数量;
    public List<副本单位> 小怪列表 = new List<副本单位>();
    public List<副本单位> 精英列表 = new List<副本单位>();
    public List<副本单位> 首领列表 = new List<副本单位>();

    public void 设置战斗房间()
    {
        this.房间类型 = 副本房间类型.战斗;
        this.房间状态 = 副本房间状态.未探索;
        初始化战斗房间数据();
        初始化战斗房间单位数据();
    }
    private void 初始化战斗房间数据()
    {
        int 房间怪物生成类型 = this.随机数(0, 3);
        敌人数量 = 10;
        小怪数量 = 0;
        精英数量 = 0;
        首领数量 = 0;
        switch (房间怪物生成类型)
        {
            case 0:
                敌人数量 = this.随机数(7, 8);
                小怪数量 = 敌人数量;
                break;
            case 1:
                敌人数量 = 4;
                小怪数量 = 3;
                精英数量 = 1;
                break;
            case 2:
                敌人数量 = 2;
                精英数量 = 2;
                break;
            default:
                敌人数量 = 10;
                小怪数量 = 10;
                break;
        }
    }
    public void 初始化战斗房间单位数据()
    {
        for (int i = 0; i < 小怪数量;i++)
        {
            小怪列表.Add(new 副本单位() { 角色属性 = 数据管理器.实例.角色数据.选择角色(3) });
        }
        for (int i = 0; i < 精英数量;i++)
        {
            精英列表.Add(new 副本单位() { 角色属性 = 数据管理器.实例.角色数据.选择角色(3) });
        }
        for (int i = 0; i < 首领数量;i++)
        {
            首领列表.Add(new 副本单位() { 角色属性 = 数据管理器.实例.角色数据.选择角色(3) });
        }
    }

}
