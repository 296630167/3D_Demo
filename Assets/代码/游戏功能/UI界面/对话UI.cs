using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 对话UI : 面板基类
{
    public void 进入()
    {
        对象("酒馆选项").隐藏();
    }
    #region  酒馆对话
    public void 进入酒馆对话()
    {
        对象("酒馆选项").隐藏();
    }
    private void 邀请进队()
    {
        print("邀请进队");
    }
    private void 查看装备()
    {
        print("查看装备");
    }
    private void 结束对话()
    {
        UI管理器.关闭UI("对话UI");
    }
    #endregion
}
