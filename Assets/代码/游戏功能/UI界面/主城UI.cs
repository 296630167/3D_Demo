using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class 主城UI : 面板基类
{
    [Header("建筑配置")]
    public string[] 建筑名称数组 = { "铁匠铺", "教堂", "雕塑", "酒馆", "杂货铺", "炼金铺" };

    [Header("移动控制")]
    public float 移动速度 = 10f;
    public float 最小X坐标 = -920.5f;
    public float 最大X坐标 = 920.5f;
    public float 最小Y坐标 = -540f;
    public float 最大Y坐标 = 540f;

    [Header("位置初始化")]
    public float 默认X坐标 = 0f;
    public float 默认Y坐标 = 0f;
    public float 位置重置动画时长 = 0.5f;
    protected override void 每帧更新()
    {
        if (UI管理器.当前页面 != 当前页面.主城) return;
        检测UI悬停移动();
    }
    private void 初始化建筑()
    {
        try
        {
            foreach (var 建筑名称 in 建筑名称数组)
            {
                图片(建筑名称).alphaHitTestMinimumThreshold = 0.5f;
                按钮(建筑名称).interactable = true;
            }
        }
        catch { }
    }
    public void 进入()
    {
        显示();
        初始化建筑();
        初始化主城位置();
        UI管理器.当前页面 = 当前页面.主城;
    }
    public void 离开()
    {
        隐藏();
    }
    public override void 显示()
    {
        base.显示();
        g.显示();
    }
    public override void 隐藏()
    {
        base.隐藏();
        g.隐藏();
    }
    #region 滚动背景控制
    private void 初始化主城位置()
    {
        try
        {
            var 主城背景 = 矩形变换("主城背景");
            if (主城背景 == null) return;
            Vector2 原始目标位置 = new Vector2(默认X坐标, 默认Y坐标);
            Vector2 限制目标位置 = 检查边界限制(原始目标位置);
            Vector2 当前位置 = 主城背景.anchoredPosition;
            if (Vector2.Distance(当前位置, 限制目标位置) > 0.1f)
            {
                if (位置重置动画时长 > 0) StartCoroutine(平滑移动到位置(主城背景, 限制目标位置));
                else 主城背景.anchoredPosition = 限制目标位置;
            }
        }
        catch { }
    }
    private System.Collections.IEnumerator 平滑移动到位置(RectTransform 目标对象, Vector2 目标位置)
    {
        Vector2 起始位置 = 目标对象.anchoredPosition;
        Vector2 限制目标位置 = 检查边界限制(目标位置);
        float 已用时间 = 0f;
        while (已用时间 < 位置重置动画时长)
        {
            已用时间 += Time.deltaTime;
            float 进度 = Mathf.SmoothStep(0f, 1f, 已用时间 / 位置重置动画时长);
            Vector2 当前位置 = Vector2.Lerp(起始位置, 限制目标位置, 进度);
            目标对象.anchoredPosition = 检查边界限制(当前位置);
            yield return null;
        }
        目标对象.anchoredPosition = 限制目标位置;
    }
    private void 检测UI悬停移动()
    {
        var 主城背景 = 矩形变换("主城背景");
        if (主城背景 == null) return;
        if (检测UI悬停("向左移动")) 移动主城背景(主城背景, 主城背景.anchoredPosition.x + 移动速度);
        else if (检测UI悬停("向右移动")) 移动主城背景(主城背景, 主城背景.anchoredPosition.x - 移动速度);
    }
    private bool 检测UI悬停(string UI名称)
    {
        var UI矩形 = 矩形变换(UI名称);
        if (UI矩形 == null) return false;
        Vector2 本地坐标;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(UI矩形, Input.mousePosition, null, out 本地坐标) && UI矩形.rect.Contains(本地坐标);
    }
    private void 移动主城背景(RectTransform 背景, Vector2 新位置)
    {
        if (背景 != null) 背景.anchoredPosition = 检查边界限制(新位置);
    }
    private void 移动主城背景(RectTransform 背景, float 新X坐标)
    {
        if (背景 != null) 移动主城背景(背景, new Vector2(新X坐标, 背景.anchoredPosition.y));
    }
    private Vector2 检查边界限制(Vector2 位置) => new Vector2(Mathf.Clamp(位置.x, 最小X坐标, 最大X坐标), Mathf.Clamp(位置.y, 最小Y坐标, 最大Y坐标));
    private bool 是否超出边界(Vector2 位置) => 位置.x < 最小X坐标 || 位置.x > 最大X坐标 || 位置.y < 最小Y坐标 || 位置.y > 最大Y坐标;
    public string 获取限制范围() => $"X范围: [{最小X坐标}, {最大X坐标}], Y范围: [{最小Y坐标}, {最大Y坐标}]";
    #endregion
    public void 铁匠铺()
    {
        UI管理器.显示UI<铁匠铺UI>("铁匠铺UI", UI层级.弹窗, o => o.进入());
    }
    public void 教堂()
    {
        // UI管理器.显示UI<教堂UI>("教堂UI", UI层级.弹窗);
    }
    public void 雕塑()
    {
        // UI管理器.显示UI<雕塑UI>("雕塑UI", UI层级.弹窗);
    }
    public void 酒馆()
    {
        // 这里要隐藏主城UI
        UI管理器.关闭UI("主城UI");
        UI管理器.显示UI<酒馆UI>("酒馆UI", UI层级.弹窗, o => o.进入());
    }
    public void 杂货铺()
    {
        UI管理器.显示UI<杂货铺UI>("杂货铺UI", UI层级.弹窗, o => o.进入());
    }
    public void 炼金铺()
    {
        // UI管理器.显示UI<炼金铺UI>("炼金铺UI", UI层级.弹窗);
    }
    public void 进入副本()
    {
        // 这里要隐藏主城UI
        UI管理器.关闭UI("主城UI");
        UI管理器.显示UI<副本UI>("副本UI", UI层级.弹窗, o => o.进入副本场景());
    }
}