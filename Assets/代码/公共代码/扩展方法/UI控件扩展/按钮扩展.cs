using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public static class 按钮扩展
{
    public static Button 点击事件(this Button 按钮, UnityAction 回调)
    {
        if (按钮 != null)
        {
            按钮.onClick.RemoveAllListeners();
            按钮.onClick.AddListener(回调);
        }
        return 按钮;
    }

    public static Button 设置可交互(this Button 按钮, bool 可交互 = true)
    {
        if (按钮 != null) 按钮.interactable = 可交互;
        return 按钮;
    }

    public static Button 设置文本(this Button 按钮, string 文本)
    {
        if (按钮 == null) return 按钮;
        
        var TMP文本组件 = 按钮.GetComponentInChildren<TextMeshProUGUI>();
        if (TMP文本组件 != null)
        {
            TMP文本组件.text = 文本;
        }
        else
        {
            var 普通文本组件 = 按钮.GetComponentInChildren<Text>();
            if (普通文本组件 != null)
                普通文本组件.text = 文本;
        }
        return 按钮;
    }

    public static Button 设置TMP文本(this Button 按钮, string 文本)
    {
        if (按钮?.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI TMP文本组件)
            TMP文本组件.text = 文本;
        return 按钮;
    }

    public static Button 设置普通文本(this Button 按钮, string 文本)
    {
        if (按钮?.GetComponentInChildren<Text>() is Text 文本组件)
            文本组件.text = 文本;
        return 按钮;
    }

    public static Button 设置TMP文本颜色(this Button 按钮, Color 颜色)
    {
        if (按钮?.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI TMP文本组件)
            TMP文本组件.color = 颜色;
        return 按钮;
    }

    public static Button 设置TMP字体大小(this Button 按钮, float 大小)
    {
        if (按钮?.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI TMP文本组件)
            TMP文本组件.fontSize = 大小;
        return 按钮;
    }

    public static Button 设置TMP对齐方式(this Button 按钮, TextAlignmentOptions 对齐方式)
    {
        if (按钮?.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI TMP文本组件)
            TMP文本组件.alignment = 对齐方式;
        return 按钮;
    }

    public static Button 设置TMP字体资源(this Button 按钮, TMP_FontAsset 字体资源)
    {
        if (按钮?.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI TMP文本组件)
            TMP文本组件.font = 字体资源;
        return 按钮;
    }

    public static Button 设置图片(this Button 按钮, Sprite 精灵)
    {
        if (按钮?.GetComponent<Image>() is Image 图片组件)
            图片组件.sprite = 精灵;
        return 按钮;
    }

    public static Button 设置图片颜色(this Button 按钮, Color 颜色)
    {
        if (按钮?.GetComponent<Image>() is Image 图片组件)
            图片组件.color = 颜色;
        return 按钮;
    }

    public static TextMeshProUGUI 获取TMP文本(this Button 按钮)
    {
        return 按钮?.GetComponentInChildren<TextMeshProUGUI>();
    }

    public static Text 获取普通文本(this Button 按钮)
    {
        return 按钮?.GetComponentInChildren<Text>();
    }

    public static bool 有TMP文本组件(this Button 按钮)
    {
        return 按钮?.GetComponentInChildren<TextMeshProUGUI>() != null;
    }

    public static bool 有普通文本组件(this Button 按钮)
    {
        return 按钮?.GetComponentInChildren<Text>() != null;
    }
}