using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class 副本_房间
{
    public void 设置入口房间()
    {
        this.房间类型 = 副本房间类型.入口;
        this.房间状态 = 副本房间状态.已探索;
    }
}
