using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class 背包基类 : 背包操作接口
{
    public 背包类型 类型;
    public int 行数 = 7;
    public int 列数 = 7;
    public bool[,] 道具占用格子数组;
    public List<道具类> 道具列表;
    public 背包基类(背包类型 类型)
    {
        this.类型 = 类型;
        初始化背包();
    }
    public 背包基类(背包类型 类型, List<道具类> 道具列表)
    {
        this.类型 = 类型;
        道具占用格子数组 = new bool[行数, 列数];
        this.道具列表 = new List<道具类>();
        清空所有格子();
        foreach (var 道具 in 道具列表)
        {
            放入道具(道具, 道具.数量, null);
            // 在控制台把 7*7的格子打印出来 然后把当前道具所占用的区域填黑
            // 打印背包格子(道具);
        }
    }

    // private void 打印背包格子(道具类 道具)
    // {
    //     var 输出 = new System.Text.StringBuilder();
        
    //     输出.AppendLine($"=== 背包格子状态 ===");
    //     if (道具 != null)
    //     {
    //         输出.AppendLine($"道具: {道具.名称} | 位置: ({道具.行},{道具.列}) | 尺寸: {道具.长度}×{道具.高度}");
    //     }
    //     输出.AppendLine("格子状态: □=空闲 ■=占用");
    //     输出.AppendLine("─────────────────");
        
    //     for (int 行 = 0; 行 < 行数; 行++)
    //     {
    //         for (int 列 = 0; 列 < 列数; 列++)
    //         {
    //             输出.Append(道具占用格子数组[行, 列] ? "■ " : "□ ");
    //         }
    //         输出.AppendLine();
    //     }
        
    //     输出.AppendLine("─────────────────");
    //     Debug.Log(输出.ToString());
    // }
    
    // public void 调试打印背包状态(道具类 指定道具 = null)
    // {
    //     打印背包格子(指定道具);
    // }

    public void 初始化背包()
    {
        道具占用格子数组 = new bool[行数, 列数];
        道具列表 = new List<道具类>();
        清空所有格子();
    }
    private void 清空所有格子()
    {
        for (int i = 0; i < 行数; i++)
        {
            for (int j = 0; j < 列数; j++)
            {
                道具占用格子数组[i, j] = false;
            }
        }
    }
    #region 背包操作接口实现
    public 背包基类 获取背包数据()
    {
        return this;
    }
    public 背包类型 获取背包类型()
    {
        return 类型;
    }
    #region 放入道具1 无操作或者快捷键快速放入道具 能合并就合并 不能合并就单独放个格子
    // 回调 
    // 参数1 结果类型 0 放入失败 1 合并道具成功 2 单独放入道具成功
    // 参数2 道具类 已经放入了背包的道具
    public void 放入道具(道具类 道具,int 数量,Action<int,道具类> 回调)
    {
        if (合并道具(道具, 数量, out 道具类 背包内道具))
        {
            回调?.Invoke(1, 背包内道具);
            return;
        }
        if (单独放入道具(道具,数量,out 背包内道具))
        {
            回调?.Invoke(2, 背包内道具);
            return;
        }
        回调?.Invoke(0, null);
    }
    public bool 合并道具(道具类 道具,int 数量, out 道具类 背包内道具)
    {
        var 目标道具 = 道具列表.Find(x => x.id == 道具.id);
        if (目标道具 != null && 目标道具.数量 + 数量 <= 目标道具.数量上限) 
        { 
            目标道具.数量 += 数量; 
            背包内道具 = 目标道具;
            return true; 
        }
        背包内道具 = null;
        return false;
    }

    public bool 单独放入道具(道具类 道具,int 数量, out 道具类 背包内道具)
    {
        if (查找空闲位置(道具, out int 行位置, out int 列位置))
        {
            占用区域格子(行位置, 列位置, 道具.长度, 道具.高度);
            var 新道具 = 道具.Json深拷贝();
            新道具.数量 = 数量;
            新道具.行 = 行位置;
            新道具.列 = 列位置;
            背包内道具 = 新道具;
            道具列表.Add(新道具);
            return true;
        }
        背包内道具 = null;
        return false;
    }
    public bool 查找空闲位置(道具类 道具, out int 行位置, out int 列位置)
    {
        for (int i = 0; i < 行数; i++)
        {
            for (int j = 0; j < 列数; j++)
            {
                if (!道具占用格子数组[i, j])
                {
                    if (i + 道具.高度 <= 行数 && j + 道具.长度 <= 列数)
                    {
                        if (!检测区域是否被占用(i, j, 道具.长度, 道具.高度))
                        {
                            行位置 = i;
                            列位置 = j;
                            return true;
                        }
                    }
                }
            }
        }
        行位置 = -1;
        列位置 = -1;
        return false;
    }
    public bool 检测区域是否被占用(int 起始行, int 起始列, int 长度, int 高度)
    {
        if (起始行 + 高度 > 行数 || 起始列 + 长度 > 列数 || 起始行 < 0 || 起始列 < 0)
        {
            return true;
        }
        for (int x = 0; x < 长度; x++)
        {
            for (int y = 0; y < 高度; y++)
            {
                if (道具占用格子数组[起始行 + y, 起始列 + x])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool 检测区域是否被占用(道具类 道具)
    {
        return 检测区域是否被占用(道具.行, 道具.列, 道具.长度, 道具.高度);
    }
    public void 占用区域格子(int 起始行, int 起始列, int 长度, int 高度)
    {
        for (int x = 0; x < 长度; x++)
        {
            for (int y = 0; y < 高度; y++)
            {
                道具占用格子数组[起始行 + y, 起始列 + x] = true;
            }
        }
    }
    public void 取消占用区域格子(int 起始行, int 起始列, int 长度, int 高度)
    {
        for (int x = 0; x < 长度; x++)
        {
            for (int y = 0; y < 高度; y++)
            {
                道具占用格子数组[起始行 + y, 起始列 + x] = false;
            }
        }
    }
    public void 占用区域格子(道具类 道具)
    {
        占用区域格子(道具.行, 道具.列, 道具.长度, 道具.高度);
    }
    public void 取消占用区域格子(道具类 道具)
    {
        取消占用区域格子(道具.行, 道具.列, 道具.长度, 道具.高度);
    }
    #endregion
    #region 放入道具2 通过拖拽到具体格子位置
    public void 放入道具(道具类 道具,int 数量,int 行,int 列,Action<int,道具类> 回调)
    {
        if (行 + 道具.高度 <= 行数 && 列 + 道具.长度 <= 列数)
        {
            if (!检测区域是否被占用(行, 列, 道具.长度, 道具.高度))
            {
                占用区域格子(行, 列, 道具.长度, 道具.高度);
                var 新道具 = 道具.Json深拷贝();
                新道具.数量 = 数量;
                新道具.行 = 行;
                新道具.列 = 列;
                道具列表.Add(新道具);
                回调?.Invoke(1, 新道具);
                return;
            }
        }
        回调?.Invoke(0, null);
    }

    #endregion
    public void 更新道具所在格子(道具类 道具,int 行,int 列)
    {
        取消占用区域格子(道具.行, 道具.列, 道具.长度, 道具.高度);
        占用区域格子(行, 列, 道具.长度, 道具.高度);
        道具.行 = 行;
        道具.列 = 列;
    }
    public void 取出道具(道具类 道具, int 数量, Action<int, 道具类> 回调)
    {
        // 还需要判断 取出道具的数量 如果归零了 则从道具列表中移除
        if (道具.数量 == 数量 || 数量 == -1)
        {
            道具列表.Remove(道具);
            取消占用区域格子(道具.行, 道具.列, 道具.长度, 道具.高度);
            回调?.Invoke(2, 道具);
            return;
        }
        if (道具.数量 > 数量)
        {
            道具.数量 -= 数量;
            回调?.Invoke(1, 道具);
            return;
        }
        回调?.Invoke(0, null);
    }
    #endregion
}