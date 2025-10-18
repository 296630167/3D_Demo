using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class 杂货铺UI : 面板基类
{
    public void 进入()
    {
        设置默认内容();
        UI管理器.当前页面 = 当前页面.杂货铺;
        print("进入杂货铺UI");
    }

    private void 设置默认内容()
    {
        TMP文本("文本").text = "杂货铺";
        TMP文本("对话内容").text = "欢迎来到杂货铺！这里有各种生活用品和材料。";
        TMP文本("名称").text = "杂货商";
    }

    public void 交易()
    {
        UI管理器.显示UI<交易UI>("交易UI", UI层级.弹窗, o => o.进入(cd.背包, cd.杂货铺));
    }

    public void 离开()
    {
        UI管理器.关闭UI("杂货铺UI");
        UI管理器.当前页面 = 当前页面.主城;
        // UI管理器.显示UI("主城UI", UI层级.主界面);
    }
}