using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class 酒馆单位 : 基
{
    protected override void 开始时()
    {
        var 触发器 = g.AddComponent<BoxCollider>();
        触发器.isTrigger = true;
        触发器.center = new Vector3(0, 0.5f, 0);
        触发器.size = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void 去掉锁定()
    {

    }

    public void 锁定()
    {

    }
}
