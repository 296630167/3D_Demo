using UnityEngine;
using TMPro;
using DG.Tweening;

public static class TMP文本扩展
{
    public static TextMeshProUGUI 设置文本(this TextMeshProUGUI TMP文本组件, string 内容, float 显示时间 = 0f)
    {
        if (TMP文本组件 != null)
        {
            if (显示时间 > 0f)
            {
                // 使用DOTween.To实现文本逐字显示效果
                string 原始文本 = TMP文本组件.text;
                TMP文本组件.text = "";
                DOTween.To(() => 0, x => 
                {
                    int 字符数量 = Mathf.RoundToInt(x);
                    TMP文本组件.text = 内容.Substring(0, Mathf.Min(字符数量, 内容.Length));
                }, 内容.Length, 显示时间).SetEase(Ease.Linear);
            }
            else
                TMP文本组件.text = 内容;
        }
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置颜色(this TextMeshProUGUI TMP文本组件, Color 颜色, float 动画时间 = 0f)
    {
        if (TMP文本组件 != null)
        {
            if (动画时间 > 0f)
                TMP文本组件.DOColor(颜色, 动画时间);
            else
                TMP文本组件.color = 颜色;
        }
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置透明度(this TextMeshProUGUI TMP文本组件, float 透明度, float 动画时间 = 0f)
    {
        if (TMP文本组件 != null)
        {
            if (动画时间 > 0f)
                TMP文本组件.DOFade(透明度, 动画时间);
            else
            {
                Color 颜色 = TMP文本组件.color;
                颜色.a = Mathf.Clamp01(透明度);
                TMP文本组件.color = 颜色;
            }
        }
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置字体大小(this TextMeshProUGUI TMP文本组件, float 大小, float 动画时间 = 0f)
    {
        if (TMP文本组件 != null)
        {
            if (动画时间 > 0f)
            {
                DOTween.To(() => TMP文本组件.fontSize, x => TMP文本组件.fontSize = x, 大小, 动画时间);
            }
            else
                TMP文本组件.fontSize = 大小;
        }
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置对齐方式(this TextMeshProUGUI TMP文本组件, TextAlignmentOptions 对齐方式)
    {
        if (TMP文本组件 != null) TMP文本组件.alignment = 对齐方式;
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置字体资源(this TextMeshProUGUI TMP文本组件, TMP_FontAsset 字体资源)
    {
        if (TMP文本组件 != null) TMP文本组件.font = 字体资源;
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置自动调整大小(this TextMeshProUGUI TMP文本组件, bool 启用自动调整 = true)
    {
        if (TMP文本组件 != null) TMP文本组件.enableAutoSizing = 启用自动调整;
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置自动调整范围(this TextMeshProUGUI TMP文本组件, float 最小大小, float 最大大小)
    {
        if (TMP文本组件 != null)
        {
            TMP文本组件.fontSizeMin = 最小大小;
            TMP文本组件.fontSizeMax = 最大大小;
        }
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置富文本(this TextMeshProUGUI TMP文本组件, bool 启用富文本 = true)
    {
        if (TMP文本组件 != null) TMP文本组件.richText = 启用富文本;
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置换行(this TextMeshProUGUI TMP文本组件, bool 启用换行 = true)
    {
        if (TMP文本组件 != null) TMP文本组件.enableWordWrapping = 启用换行;
        return TMP文本组件;
    }
    
    public static TextMeshProUGUI 设置溢出模式(this TextMeshProUGUI TMP文本组件, TextOverflowModes 溢出模式)
    {
        if (TMP文本组件 != null) TMP文本组件.overflowMode = 溢出模式;
        return TMP文本组件;
    }
}