using System;
using System.Collections.Generic;
using UnityEngine;

public interface 背包操作接口
{
    背包基类 获取背包数据();
    背包类型 获取背包类型();
    void 放入道具(道具类 道具,int 数量,Action<int,道具类> 回调);
    void 放入道具(道具类 道具,int 数量,int 行,int 列,Action<int,道具类> 回调);
    void 占用区域格子(int 起始行, int 起始列, int 长度, int 高度);
    void 取消占用区域格子(int 起始行, int 起始列, int 长度, int 高度);
    void 占用区域格子(道具类 道具);
    void 取消占用区域格子(道具类 道具);
    void 更新道具所在格子(道具类 道具,int 行,int 列);
    void 取出道具(道具类 道具, int 数量, Action<int, 道具类> 回调);
    bool 检测区域是否被占用(int 起始行, int 起始列, int 长度, int 高度);
    bool 检测区域是否被占用(道具类 道具);
}