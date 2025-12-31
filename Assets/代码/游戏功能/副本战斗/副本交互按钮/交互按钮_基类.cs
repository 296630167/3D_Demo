using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class 交互按钮_基类 : 基
{
    protected 副本玩家单位 单位;
    protected 技能类 技能;
    public 辅助线绘制 辅助线 => sj.新副本UI.辅助线;
    public 副本场景 场景 => sj.副本场景;
    protected 交互按钮_管理器 管理器 => sj.副本交互按钮管理;
    protected bool 已初始化;
    protected Coroutine 交互携程;
    public virtual void 初始化按钮(副本玩家单位 单位, 技能类 技能 = null)
    {
        this.单位 = 单位;
        this.技能 = 技能;
        g.鼠标进入(鼠标进入事件);
        g.鼠标离开(鼠标离开事件);
        g.鼠标悬停期间(鼠标悬停事件);
        g.鼠标点击(鼠标点击事件);
        已初始化 = true;
    }
    #region 每帧更新事件
    protected override void 每帧更新()
    {
        if (!已初始化) return;
        if (管理器 == null) return;
        if (管理器.当前锁定按钮 != this) return;
        if (!管理器.已锁定交互按钮) return;
        if (右键取消锁定当前按钮()) return;
        if (每帧检查鼠标所在格子()) return;
        if (左键点击鼠标所在格子()) return;
        交互按钮每帧更新事件();
    }

    private bool 左键点击鼠标所在格子()
    {
        if (!左键按下()) return false;
        if (指针在UI上()) return false;
        管理器.交互按钮可点击 = false;
        鼠标点击格子();
        return true;
    }

    private bool 每帧检查鼠标所在格子()
    {
        if (指针在UI上()) return true;
        检测鼠标所在格子();
        return false;
    }
    private bool 右键取消锁定当前按钮()
    {
        if (右键按下())
        {
            管理器.取消锁定(true);
            取消交互按钮();
            return true;
        }
        return false;
    }
    #endregion
    protected virtual void 检测鼠标所在格子() { }
    protected virtual void 鼠标点击格子() { }
    protected virtual void 交互按钮每帧更新事件() { }
    protected virtual void 进入交互按钮() { }
    protected virtual void 离开交互按钮() { }
    protected virtual void 点击交互按钮() { }
    protected virtual void 取消交互按钮() { }
    private void 鼠标点击事件(PointerEventData p)
    {
        if (!已初始化) return;
        if (p.button != PointerEventData.InputButton.Left) return;
        if (!管理器.交互按钮可点击) return;
        if (管理器.当前锁定按钮 == this) return;
        //if (管理器.当前锁定按钮 != null) 管理器.当前锁定按钮.取消锁定(true);
        管理器.锁定(this);
        点击交互按钮();
    }

    private void 鼠标悬停事件(PointerEventData p)
    {
        //if (!管理器.交互按钮可点击) return;
        //if (管理器.已锁定交互按钮) return;
        //进入交互按钮();
    }

    private void 鼠标进入事件(PointerEventData p)
    {
        if (!管理器.交互按钮可点击) return;
        if (管理器.已锁定交互按钮) return;
        进入交互按钮();
    }

    private void 鼠标离开事件(PointerEventData p)
    {
        if (!管理器.交互按钮可点击) return;
        if (管理器.已锁定交互按钮) return;
        离开交互按钮();
    }
    public virtual void 清理按钮()
    {
        if (交互携程 != null) { 关闭携程(交互携程); 交互携程 = null; }
        已初始化 = false;
        单位 = null;
        Destroy(this);
    }

    public virtual void 取消锁定(bool 交互按钮状态 = false)
    {
        管理器.取消锁定(交互按钮状态);
        取消交互按钮();
    }
    protected void 按钮文本(string v)
    {
        var 文本组件 = TMP文本("文本");
        if (文本组件 != null) 文本组件.text = v;
    }

    protected void 按钮图标(Sprite 图标)
    {
        var 图片组件 = 图片("图标");
        if (图片组件 != null) 图片组件.sprite = 图标;
    }

    protected bool 左键按下()
    {
        return Input.GetMouseButtonDown(0);
    }

    protected bool 右键按下()
    {
        return Input.GetMouseButtonDown(1);
    }

    protected bool 指针在UI上()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}