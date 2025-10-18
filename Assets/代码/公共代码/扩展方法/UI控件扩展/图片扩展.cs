using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class 图片扩展
{
    public static Image 设置图片(this Image 图片组件, Sprite 精灵)
    {
        if (图片组件 != null) 图片组件.sprite = 精灵;
        return 图片组件;
    }
    
    public static Image 设置颜色(this Image 图片组件, Color 颜色, float 动画时间 = 0f)
    {
        if (图片组件 != null)
        {
            if (动画时间 > 0f)
                图片组件.DOColor(颜色, 动画时间);
            else
                图片组件.color = 颜色;
        }
        return 图片组件;
    }
    
    public static Image 设置透明度(this Image 图片组件, float 透明度, float 动画时间 = 0f)
    {
        if (图片组件 != null)
        {
            if (动画时间 > 0f)
                图片组件.DOFade(透明度, 动画时间);
            else
            {
                Color 颜色 = 图片组件.color;
                颜色.a = Mathf.Clamp01(透明度);
                图片组件.color = 颜色;
            }
        }
        return 图片组件;
    }
}