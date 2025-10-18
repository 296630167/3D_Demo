using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public static class 滚动视图扩展
{
    public static ScrollRect 滚动改变(this ScrollRect 滚动视图, UnityAction<Vector2> 回调)
    {
        滚动视图.onValueChanged.RemoveAllListeners();
        滚动视图.onValueChanged.AddListener(回调);
        return 滚动视图;
    }
    
    public static ScrollRect 滚动到顶部(this ScrollRect 滚动视图, float 动画时间 = 0f)
    {
        if (滚动视图 != null)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 滚动视图.verticalNormalizedPosition, x => 滚动视图.verticalNormalizedPosition = x, 1f, 动画时间);
            else
                滚动视图.verticalNormalizedPosition = 1f;
        }
        return 滚动视图;
    }
    
    public static ScrollRect 滚动到底部(this ScrollRect 滚动视图, float 动画时间 = 0f)
    {
        if (滚动视图 != null)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 滚动视图.verticalNormalizedPosition, x => 滚动视图.verticalNormalizedPosition = x, 0f, 动画时间);
            else
                滚动视图.verticalNormalizedPosition = 0f;
        }
        return 滚动视图;
    }
    
    public static ScrollRect 滚动到左侧(this ScrollRect 滚动视图, float 动画时间 = 0f)
    {
        if (滚动视图 != null)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 滚动视图.horizontalNormalizedPosition, x => 滚动视图.horizontalNormalizedPosition = x, 0f, 动画时间);
            else
                滚动视图.horizontalNormalizedPosition = 0f;
        }
        return 滚动视图;
    }
    
    public static ScrollRect 滚动到右侧(this ScrollRect 滚动视图, float 动画时间 = 0f)
    {
        if (滚动视图 != null)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 滚动视图.horizontalNormalizedPosition, x => 滚动视图.horizontalNormalizedPosition = x, 1f, 动画时间);
            else
                滚动视图.horizontalNormalizedPosition = 1f;
        }
        return 滚动视图;
    }
}