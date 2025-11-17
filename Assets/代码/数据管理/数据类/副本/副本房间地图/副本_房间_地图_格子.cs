using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 副本_房间_地图_格子
{
    public int 行;
    public int 列;
    public bool 存在单位;
    public 副本单位 单位;
    public Vector3 场景坐标 => new Vector3(列 * 0.5f + 0.25f, 0, 行 * 0.5f + 0.25f);
    public Vector2Int 网格坐标 => new Vector2Int(行, 列);

    public 副本_房间_地图_格子(int 行, int 列)
    {
        this.行 = 行;
        this.列 = 列;
        存在单位 = false;
        单位 = null;
    }
    public void 添加单位(副本单位 单位)
    {
        存在单位 = true;
        this.单位 = 单位;
    }
    public void 移除单位()
    {
        存在单位 = false;
        单位 = null;
    }
}
