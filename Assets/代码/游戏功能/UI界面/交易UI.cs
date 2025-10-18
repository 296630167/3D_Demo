using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 交易UI : 面板基类
{
    [SerializeField]private 背包UI 背包1脚本;
    [SerializeField]private 背包UI 背包2脚本;
    public void 进入(背包操作接口 背包1接口数据,背包操作接口 背包2接口数据 = null)
    {
        if (背包1接口数据 != null) 背包1脚本.初始化背包(背包1接口数据, 变换("道具拖拽时容器"));
        if (背包2接口数据 != null) 背包2脚本.初始化背包(背包2接口数据, 变换("道具拖拽时容器"));
    }
    public 背包UI 鼠标所在背包()
    {
        Vector2 鼠标屏幕位置 = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(背包1脚本.rt, 鼠标屏幕位置))
        {
            return 背包1脚本;
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(背包2脚本.rt, 鼠标屏幕位置))
        {
            return 背包2脚本;
        }
        return null;
    }
    protected override void 每帧更新()
    {
        检测右键关闭();
    }

    private void 检测右键关闭()
    {
        if (Input.GetMouseButtonDown(1))
        {
            关闭交易UI();
        }
    }

    public void 关闭交易UI()
    {
        UI管理器.关闭UI("交易UI");
    }
}
