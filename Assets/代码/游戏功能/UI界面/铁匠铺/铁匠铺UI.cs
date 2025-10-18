using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class 铁匠铺UI : 面板基类
{
    protected override void 启动时()
    {
        base.启动时();
    }

    public void 进入()
    {
        设置默认内容();
        UI管理器.当前页面 = 当前页面.铁匠铺;
        print("进入铁匠铺UI");
    }
    private void 设置默认内容()
    {
        TMP文本("文本").text = "铁匠铺";
        TMP文本("对话内容").text = "欢迎来到铁匠铺！我可以为您锻造装备或进行交易。";
        TMP文本("名称").text = "老铁匠";
    }

    public void 锻造()
    {
        // UI管理器.显示UI("锻造UI", UI层级.弹窗);
    }

    public void 交易()
    {
        UI管理器.显示UI<交易UI>("交易UI", UI层级.弹窗, o => o.进入(cd.背包, cd.铁匠铺));
    }

    public void 离开()
    {
        UI管理器.关闭UI("铁匠铺UI");
        UI管理器.当前页面 = 当前页面.主城;
        // UI管理器.显示UI("主城UI", UI层级.主界面);
    }
}
