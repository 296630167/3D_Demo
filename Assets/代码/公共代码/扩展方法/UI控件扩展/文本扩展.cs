using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class 文本扩展
{
    public static Text 设置文本(this Text 文本组件, string 内容, float 显示时间 = 0f)
    {
        if (文本组件 != null) 文本组件.DOText(内容, 显示时间);
        return 文本组件;
    }
    
    public static Text 设置颜色(this Text 文本组件, Color 颜色, float 动画时间 = 0f)
    {
        if (文本组件 != null)
        {
            if (动画时间 > 0f)
                文本组件.DOColor(颜色, 动画时间);
            else
                文本组件.color = 颜色;
        }
        return 文本组件;
    }
    
    public static Text 设置透明度(this Text 文本组件, float 透明度, float 动画时间 = 0f)
    {
        if (文本组件 != null)
        {
            if (动画时间 > 0f)
                文本组件.DOFade(透明度, 动画时间);
            else
            {
                Color 颜色 = 文本组件.color;
                颜色.a = Mathf.Clamp01(透明度);
                文本组件.color = 颜色;
            }
        }
        return 文本组件;
    }
    
    public static Text 设置字体大小(this Text 文本组件, int 大小, float 动画时间 = 0f)
    {
        if (文本组件 != null)
        {
            if (动画时间 > 0f)
            {
                DOTween.To(() => 文本组件.fontSize, x => 文本组件.fontSize = x, 大小, 动画时间);
            }
            else
                文本组件.fontSize = 大小;
        }
        return 文本组件;
    }
}