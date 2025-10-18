using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class 背包UI道具 : 基
{
    public 道具类 道具;
    public 背包UI 所属背包;
    private Vector2 原始位置;
    private bool 正在拖拽;
    public Transform 拖拽时父级;
    private Vector2 拖拽坐标差;

    public void 初始化(道具类 道具, 背包UI 背包引用)
    {
        this.道具 = 道具;
        所属背包 = 背包引用;
        rt.设置界面锚点(界面锚点.左上角);
        float 宽度调整 = 道具.长度 > 1 ? (道具.长度 - 1) * 5 : 0;
        float 高度调整 = 道具.高度 > 1 ? (道具.高度 - 1) * 5 : 0;
        rt.设置尺寸(new Vector2(道具.长度 * 100 + 宽度调整, 道具.高度 * 100 + 高度调整));
        float x偏移 = 道具.列 * 100f + 道具.列 * 5f;
        float y偏移 = -道具.行 * 100f - 道具.行 * 5f;
        rt.设置位置(new Vector2(x偏移, y偏移));
        TMP文本("道具名字").text = 道具.名称;
        原始位置 = rt.anchoredPosition;
        绑定拖拽事件();
    }
    private void 绑定拖拽事件()
    {
        g.拖拽开始(当拖拽开始);
        g.拖拽(当拖拽中);
        g.拖拽结束(当拖拽结束);
        g.鼠标点击(当鼠标单击);
    }
    public virtual void 当拖拽开始(PointerEventData 事件数据)
    {
        正在拖拽 = true;
        原始位置 = rt.anchoredPosition;
        t.设置父级(拖拽时父级);
        t.SetAsLastSibling();
        Vector2 v1 = (事件数据.position / new Vector2(Screen.width, Screen.height) * new Vector2(1920f, 1080f));
        拖拽坐标差 = v1 - rt.anchoredPosition;
        组件<CanvasGroup>(name).blocksRaycasts = false;
        所属背包.背包接口.取消占用区域格子(道具);
        print("开始拖拽");
    }
    public virtual void 当拖拽中(PointerEventData 事件数据)
    {
        if (正在拖拽)
        {
            Vector2 v1 = (事件数据.position / new Vector2(Screen.width, Screen.height) * new Vector2(1920f, 1080f));
            rt.anchoredPosition = v1 - 拖拽坐标差;
        }
    }
    public virtual void 当拖拽结束(PointerEventData 事件数据)
    {
        var 目标背包 = 所属背包.鼠标所在背包;
        if(目标背包 == null)
        {
            道具归位();
            return;
        }
        Vector2Int? 鼠标所在格子坐标 = 目标背包.获取鼠标所在格子();
        if(鼠标所在格子坐标 == null)
        {
            道具归位();
            return;
        }
        if(目标背包.背包接口.检测区域是否被占用(鼠标所在格子坐标.Value.x,鼠标所在格子坐标.Value.y,道具.长度,道具.高度))
        {
            道具归位();
            return;
        }
        if(目标背包 == 所属背包)
        {
            道具.行 = 鼠标所在格子坐标.Value.x;
            道具.列 = 鼠标所在格子坐标.Value.y;
            原始位置 = new Vector2(道具.列 * 100f + 道具.列 * 5f, -道具.行 * 100f - 道具.行 * 5f);
            道具归位();
            return;
        }
        处理跨背包拖拽交互(目标背包, 鼠标所在格子坐标.Value);
    }

    public virtual void 处理跨背包拖拽交互(背包UI 目标背包, Vector2Int 目标格子坐标)
    {
        背包类型 源背包类型 = 所属背包.背包类型;
        背包类型 目标背包类型 = 目标背包.背包类型;
        
        if ((源背包类型 == 背包类型.背包 && 目标背包类型 == 背包类型.仓库) ||
            (源背包类型 == 背包类型.仓库 && 目标背包类型 == 背包类型.背包))
        {
            切换背包(目标背包, 目标格子坐标);
            print($"背包仓库互相拖拽：{道具.名称} 从 {所属背包.背包类型} 到 {目标背包.背包类型}");
            return;
        }
        
        if (源背包类型 == 背包类型.背包 && 目标背包类型 == 背包类型.商店)
        {
            出售道具(目标背包, 目标格子坐标);
            print($"出售道具：{道具.名称} 从背包");
            return;
        }
        
        if (源背包类型 == 背包类型.商店 && 目标背包类型 == 背包类型.背包)
        {
            购买道具(目标背包, 目标格子坐标);
            print($"购买道具：{道具.名称} 从商店");
            return;
        }
        
        print($"不支持的拖拽操作：{源背包类型} → {目标背包类型}");
        道具归位();
    }

    protected virtual void 切换背包(背包UI 目标背包, Vector2Int 目标格子坐标)
    {
        int 数量 = 道具.数量;
        所属背包.背包接口.取出道具(道具, 数量, null);
        目标背包.背包接口.放入道具(道具, 数量, 目标格子坐标.x, 目标格子坐标.y, (int 结果, 道具类 新道具) =>
        {
            if (结果 == 1)
            {
                所属背包 = 目标背包;
                道具 = 新道具;
                原始位置 = new Vector2(道具.列 * 100f + 道具.列 * 5f, -道具.行 * 100f - 道具.行 * 5f);
            }
        });
        道具归位();
    }

    protected virtual void 出售道具(背包UI 目标背包, Vector2Int 目标格子坐标)
    {
        int 数量 = 道具.数量;
        cd.金币 += 数量 * 10;
        切换背包(目标背包, 目标格子坐标);
        道具归位();
    }

    protected virtual void 购买道具(背包UI 目标背包, Vector2Int 目标格子坐标)
    {
        if(cd.金币 >= 道具.价格)
        {
            cd.金币 -= 道具.价格;
            切换背包(目标背包, 目标格子坐标);
        }
        道具归位();
    }

    public virtual void 道具归位()
    {
        正在拖拽 = false;
        t.设置父级(所属背包.道具默认容器);
        rt.anchoredPosition = 原始位置;
        组件<CanvasGroup>(name).blocksRaycasts = true;
        所属背包.背包接口.占用区域格子(道具);
    }

    public virtual void 当鼠标单击(PointerEventData 事件数据)
    {
        if (正在拖拽) return;
        
        switch (事件数据.button)
        {
            case PointerEventData.InputButton.Left:
                print($"左键单击道具: {道具.名称}");
                break;
            case PointerEventData.InputButton.Right:
                print($"右键单击道具: {道具.名称}");
                break;
        }
    }
}