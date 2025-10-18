using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public static class 滑动条扩展
{
    public static Slider 数值改变(this Slider 滑动条, UnityAction<float> 回调)
    {
        滑动条.onValueChanged.RemoveAllListeners();
        滑动条.onValueChanged.AddListener(回调);
        return 滑动条;
    }
    
    public static Slider 设置数值(this Slider 滑动条, float 数值, float 动画时间 = 0f)
    {
        if (滑动条 != null)
        {
            if (动画时间 > 0f)
                滑动条.DOValue(数值, 动画时间);
            else
                滑动条.value = 数值;
        }
        return 滑动条;
    }
    
    public static Slider 设置范围(this Slider 滑动条, float 最小值, float 最大值)
    {
        if (滑动条 != null)
        {
            滑动条.minValue = 最小值;
            滑动条.maxValue = 最大值;
        }
        return 滑动条;
    }
    
    public static Slider 设置整数模式(this Slider 滑动条, bool 整数模式 = true)
    {
        if (滑动条 != null) 滑动条.wholeNumbers = 整数模式;
        return 滑动条;
    }
}