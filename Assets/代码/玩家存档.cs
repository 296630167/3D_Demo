using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class 玩家存档
{
    // 经济系统数据
    public int 金币;

    // 背包系统数据
    public 杂货铺 杂货铺;
    public 铁匠铺 铁匠铺;
    public 炼金铺 炼金铺;
    public 背包 背包;
    public 仓库 仓库;

    // 角色系统数据
    public 角色类 主角属性;
    
    // 存档管理数据
    public DateTime 保存时间;
    public DateTime 读取时间;
    
    public void 初始化新存档()
    {
        金币 = 1000;
        主角属性 = 数据管理器.实例.角色数据.选择角色(7);
        杂货铺 = new 杂货铺(数据管理器.实例.道具数据.道具列表.Where(道具 => 道具.固定交易点 == 道具交易点.杂货铺).ToList());
        铁匠铺 = new 铁匠铺(数据管理器.实例.道具数据.道具列表.Where(道具 => 道具.固定交易点 == 道具交易点.铁匠铺).ToList());
        炼金铺 = new 炼金铺(数据管理器.实例.道具数据.道具列表.Where(道具 => 道具.固定交易点 == 道具交易点.炼金铺).ToList());
        背包 = new 背包();
        仓库 = new 仓库();
        更新保存时间();
        更新读取时间();
    }
    
    public void 更新保存时间()
    {
        保存时间 = DateTime.Now;
    }
    
    public void 更新读取时间()
    {
        读取时间 = DateTime.Now;
    }
}
