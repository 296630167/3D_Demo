using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class 副本交互按钮基类 : 基
{
    protected 副本玩家单位 单位;
    protected 技能类 技能;
    // protected bool 交互事件锁定;
    protected bool 已初始化 = false;

    public virtual void 初始化按钮(副本玩家单位 单位, 技能类 技能 = null)
    {
        this.单位 = 单位;
        this.技能 = 技能;
        //按钮文本("按钮");
        // 按钮图标();
        // 鼠标 进入 离开 悬停 点击锁定 事件
        g.鼠标进入(鼠标进入事件);
        g.鼠标离开(鼠标离开事件);
        g.鼠标悬停期间(鼠标悬停事件);
        g.鼠标点击(鼠标点击事件);
        // 交互事件锁定 = false;
        已初始化 = true;
    }
    protected virtual void 每帧检测事件()
    {
        if(!sj.副本UI.交互事件锁定) return;
        if(Input.GetMouseButtonDown(1))
        {
            单位.交互按钮可点击 = true;
            取消点击交互按钮();
            return;
        }
        检测鼠标所在格子();
        if(Input.GetMouseButtonDown(0))
        {
            // 如果鼠标点击在UI上 打印123
            if(EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("点击在UI上");
                return;
            }
            单位.交互按钮可点击 = false;
            鼠标点击格子();
        }
        交互按钮每帧更新事件();
    }

    protected virtual void 检测鼠标所在格子()
    {

    }
    protected virtual void 鼠标点击格子()
    {

    }

    protected virtual void 交互按钮每帧更新事件(){}
    private void 鼠标点击事件(PointerEventData p)
    {
        if(!已初始化) return;
        if(p.button != PointerEventData.InputButton.Left) return;
        if (!单位.交互按钮可点击) return;
        if(sj.副本UI.当前锁定交互按钮 == this) return;
        if(sj.副本UI.当前锁定交互按钮 != null)
        {
            sj.副本UI.当前锁定交互按钮.取消点击交互按钮();
        }
        sj.副本UI.当前锁定交互按钮 = this;
        sj.副本UI.交互事件锁定 = true;
        // 单位.交互按钮可点击 = false;
        Mono方法管理器.添加普通帧事件(每帧检测事件);
        点击交互按钮();
    }
    private void 鼠标悬停事件(PointerEventData p)
    {
        if(!单位.交互按钮可点击) return;
        // if(sj.副本UI.当前锁定交互按钮 != null && sj.副本UI.当前锁定交互按钮 != this) return;
        悬停交互按钮();
    }
    private void 鼠标离开事件(PointerEventData p)
    {
        // if(sj.副本UI.当前锁定交互按钮 != null && sj.副本UI.当前锁定交互按钮 != this) return;
        if(sj.副本UI.交互事件锁定) return;
        离开交互按钮();
    }
    private void 鼠标进入事件(PointerEventData p)
    {
        if(!单位.交互按钮可点击) return;
        if(sj.副本UI.交互事件锁定)return;
        // if(sj.副本UI.当前锁定交互按钮 != null && sj.副本UI.当前锁定交互按钮 != this) return;
        进入交互按钮();
    }
    public virtual void 取消点击交互按钮()
    {
        Mono方法管理器.移除普通帧事件(每帧检测事件);
        sj.副本UI.当前锁定交互按钮 = null;
        sj.副本UI.交互事件锁定 = false;
        // 单位.交互按钮可点击 = true;
        取消交互按钮();
    }
    protected virtual void 进入交互按钮() { }
    protected virtual void 离开交互按钮() { }
    protected virtual void 悬停交互按钮() { }
    protected virtual void 点击交互按钮() { }
    protected virtual void 取消交互按钮() { }
    protected void 按钮图标(Sprite 图标)
    {
        图片("图标").sprite = 图标;
    }
    protected void 按钮文本(string v)
    {
        TMP文本("文本").text = v;
    }

    public void 清理按钮()
    {
        已初始化 = false;
        单位 = null;
        Destroy(this);
    }
}
