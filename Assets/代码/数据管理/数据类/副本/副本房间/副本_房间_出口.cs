using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class 副本_房间
{
    public void 设置出口房间()
    {
        this.房间类型 = 副本房间类型.出口;
        this.房间状态 = 副本房间状态.未探索;
    }
}
