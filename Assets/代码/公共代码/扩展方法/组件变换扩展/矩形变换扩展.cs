using UnityEngine;
using DG.Tweening;

public enum 界面锚点
{
    左上角,
    上中,
    右上角,
    左中,
    中心,
    右中,
    左下角,
    下中,
    右下角,
    拉伸
}

public static class 矩形变换扩展
{
    public static RectTransform 设置尺寸(this RectTransform 矩形变换, Vector2 尺寸, float 动画时间 = 0f)
    {
        if (矩形变换 != null)
        {
            if (动画时间 > 0f)
                矩形变换.DOSizeDelta(尺寸, 动画时间);
            else
                矩形变换.sizeDelta = 尺寸;
        }
        return 矩形变换;
    }
    
    public static RectTransform 设置宽度(this RectTransform 矩形变换, float 宽度, float 动画时间 = 0f)
    {
        if (矩形变换 != null)
        {
            if (动画时间 > 0f)
            {
                Vector2 目标尺寸 = 矩形变换.sizeDelta;
                目标尺寸.x = 宽度;
                矩形变换.DOSizeDelta(目标尺寸, 动画时间);
            }
            else
            {
                Vector2 尺寸 = 矩形变换.sizeDelta;
                尺寸.x = 宽度;
                矩形变换.sizeDelta = 尺寸;
            }
        }
        return 矩形变换;
    }
    
    public static RectTransform 设置高度(this RectTransform 矩形变换, float 高度, float 动画时间 = 0f)
    {
        if (矩形变换 != null)
        {
            if (动画时间 > 0f)
            {
                Vector2 目标尺寸 = 矩形变换.sizeDelta;
                目标尺寸.y = 高度;
                矩形变换.DOSizeDelta(目标尺寸, 动画时间);
            }
            else
            {
                Vector2 尺寸 = 矩形变换.sizeDelta;
                尺寸.y = 高度;
                矩形变换.sizeDelta = 尺寸;
            }
        }
        return 矩形变换;
    }
    
    public static RectTransform 设置位置(this RectTransform 矩形变换, Vector2 位置, float 动画时间 = 0f)
    {
        if (矩形变换 != null)
        {
            if (动画时间 > 0f)
                矩形变换.DOAnchorPos(位置, 动画时间);
            else
                矩形变换.anchoredPosition = 位置;
        }
        return 矩形变换;
    }
    
    public static void 设置界面锚点(this RectTransform 矩形变换, 界面锚点 锚点位置, bool 保持位置 = true)
    {
        if (矩形变换 == null) return;
        
        Vector2 原始位置 = 矩形变换.anchoredPosition;
        Vector2 原始大小 = 矩形变换.sizeDelta;
        
        switch (锚点位置)
        {
            case 界面锚点.左上角:
                矩形变换.anchorMin = new Vector2(0, 1);
                矩形变换.anchorMax = new Vector2(0, 1);
                矩形变换.pivot = new Vector2(0, 1);
                break;
            case 界面锚点.上中:
                矩形变换.anchorMin = new Vector2(0.5f, 1);
                矩形变换.anchorMax = new Vector2(0.5f, 1);
                矩形变换.pivot = new Vector2(0.5f, 1);
                break;
            case 界面锚点.右上角:
                矩形变换.anchorMin = new Vector2(1, 1);
                矩形变换.anchorMax = new Vector2(1, 1);
                矩形变换.pivot = new Vector2(1, 1);
                break;
            case 界面锚点.左中:
                矩形变换.anchorMin = new Vector2(0, 0.5f);
                矩形变换.anchorMax = new Vector2(0, 0.5f);
                矩形变换.pivot = new Vector2(0, 0.5f);
                break;
            case 界面锚点.中心:
                矩形变换.anchorMin = new Vector2(0.5f, 0.5f);
                矩形变换.anchorMax = new Vector2(0.5f, 0.5f);
                矩形变换.pivot = new Vector2(0.5f, 0.5f);
                break;
            case 界面锚点.右中:
                矩形变换.anchorMin = new Vector2(1, 0.5f);
                矩形变换.anchorMax = new Vector2(1, 0.5f);
                矩形变换.pivot = new Vector2(1, 0.5f);
                break;
            case 界面锚点.左下角:
                矩形变换.anchorMin = new Vector2(0, 0);
                矩形变换.anchorMax = new Vector2(0, 0);
                矩形变换.pivot = new Vector2(0, 0);
                break;
            case 界面锚点.下中:
                矩形变换.anchorMin = new Vector2(0.5f, 0);
                矩形变换.anchorMax = new Vector2(0.5f, 0);
                矩形变换.pivot = new Vector2(0.5f, 0);
                break;
            case 界面锚点.右下角:
                矩形变换.anchorMin = new Vector2(1, 0);
                矩形变换.anchorMax = new Vector2(1, 0);
                矩形变换.pivot = new Vector2(1, 0);
                break;
            case 界面锚点.拉伸:
                矩形变换.anchorMin = new Vector2(0, 0);
                矩形变换.anchorMax = new Vector2(1, 1);
                矩形变换.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
        
        if (保持位置)
        {
            矩形变换.anchoredPosition = 原始位置;
            矩形变换.sizeDelta = 原始大小;
        }
    }
    
    public static RectTransform 获取屏幕位置(this RectTransform 矩形变换)
    {
        if (矩形变换 != null)
        {
            Vector3 屏幕位置 = RectTransformUtility.WorldToScreenPoint(Camera.main, 矩形变换.position);
            Debug.Log($"屏幕位置: {屏幕位置}");
        }
        return 矩形变换;
    }
    
    public static bool 是否在屏幕内(this RectTransform 矩形变换)
    {
        if (矩形变换 == null) return false;
        Vector3 屏幕位置 = RectTransformUtility.WorldToScreenPoint(Camera.main, 矩形变换.position);
        return 屏幕位置.x >= 0 && 屏幕位置.x <= Screen.width && 屏幕位置.y >= 0 && 屏幕位置.y <= Screen.height;
    }
    
    public static RectTransform 弹性出现(this RectTransform 矩形变换, float 持续时间 = 0.5f)
    {
        if (矩形变换 != null)
        {
            矩形变换.localScale = Vector3.zero;
            矩形变换.DOScale(Vector3.one, 持续时间).SetEase(Ease.OutBack);
        }
        return 矩形变换;
    }
    
    public static RectTransform 摇摆效果(this RectTransform 矩形变换, float 强度 = 10f, float 持续时间 = 0.5f)
    {
        if (矩形变换 != null)
        {
            矩形变换.DOShakePosition(持续时间, 强度, 10, 90, false, true);
        }
        return 矩形变换;
    }
}