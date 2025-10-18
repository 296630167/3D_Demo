using UnityEngine;
using DG.Tweening;

public static class 画布组扩展
{
    public static CanvasGroup 设置透明度(this CanvasGroup 画布组, float 透明度, float 动画时间 = 0f)
    {
        if (画布组 != null)
        {
            if (动画时间 > 0f)
                画布组.DOFade(透明度, 动画时间);
            else
                画布组.alpha = Mathf.Clamp01(透明度);
        }
        return 画布组;
    }
    
    public static CanvasGroup 启用交互(this CanvasGroup 画布组, bool 可交互 = true)
    {
        if (画布组 != null)
        {
            画布组.interactable = 可交互;
            画布组.blocksRaycasts = 可交互;
        }
        return 画布组;
    }
    
    public static CanvasGroup 禁用交互(this CanvasGroup 画布组)
    {
        return 画布组.启用交互(false);
    }
    
    public static CanvasGroup 闪烁效果(this CanvasGroup 画布组, float 持续时间 = 1f, int 闪烁次数 = 3)
    {
        if (画布组 != null)
        {
            Sequence 序列 = DOTween.Sequence();
            for (int i = 0; i < 闪烁次数; i++)
            {
                序列.Append(画布组.DOFade(0f, 持续时间 / (闪烁次数 * 2)));
                序列.Append(画布组.DOFade(1f, 持续时间 / (闪烁次数 * 2)));
            }
        }
        return 画布组;
    }
}