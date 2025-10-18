using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public static class 组件扩展
{
    public static Transform 设置父级(this Transform 变换, Transform 新父级, bool 保持本地位置 = true)
    {
        if (变换 != null)
        {
            变换.SetParent(新父级, 保持本地位置);
        }
        return 变换;
    }

    public static Transform 设置位置(this Transform 变换, Vector3 位置, float 动画时间 = 0f)
    {
        if (变换 != null)
        {
            if (动画时间 > 0f)
                变换.DOMove(位置, 动画时间);
            else
                变换.position = 位置;
        }
        return 变换;
    }
    
    public static Transform 设置本地位置(this Transform 变换, Vector3 本地位置, float 动画时间 = 0f)
    {
        if (变换 != null)
        {
            if (动画时间 > 0f)
                变换.DOLocalMove(本地位置, 动画时间);
            else
                变换.localPosition = 本地位置;
        }
        return 变换;
    }
    
    public static Transform 设置缩放(this Transform 变换, Vector3 缩放, float 动画时间 = 0f)
    {
        if (变换 != null)
        {
            if (动画时间 > 0f)
                变换.DOScale(缩放, 动画时间);
            else
                变换.localScale = 缩放;
        }
        return 变换;
    }
    
    public static Transform 设置旋转(this Transform 变换, Vector3 欧拉角, float 动画时间 = 0f)
    {
        if (变换 != null)
        {
            if (动画时间 > 0f)
                变换.DORotate(欧拉角, 动画时间);
            else
                变换.eulerAngles = 欧拉角;
        }
        return 变换;
    }
    
    public static Transform 重置变换(this Transform 变换)
    {
        if (变换 != null)
        {
            变换.localPosition = Vector3.zero;
            变换.localRotation = Quaternion.identity;
            变换.localScale = Vector3.one;
        }
        return 变换;
    }
    
    public static Transform 缩放脉冲(this Transform 变换, float 缩放倍数 = 1.2f, float 持续时间 = 0.5f)
    {
        if (变换 != null)
        {
            Vector3 原始缩放 = 变换.localScale;
            变换.DOScale(原始缩放 * 缩放倍数, 持续时间 / 2)
                  .SetLoops(2, LoopType.Yoyo);
        }
        return 变换;
    }
    
    public static Vector3 隐藏位置 = new Vector3(10000, 10000, 0);
    public static Vector3 显示位置 = Vector3.zero;
    
    public static void 显示(this Transform 对象)
    {
        对象?.gameObject.显示();
    }
    
    public static void 隐藏(this Transform 对象)
    {
        对象?.gameObject.隐藏();
    }
    
    public static void 打开(this Transform 对象)
    {
        对象?.gameObject.打开();
    }
    
    public static void 关闭(this Transform 对象)
    {
        对象?.gameObject.关闭();
    }
    public static void 清理(this Transform 父对象)
    {
        if (父对象 == null) return;
        for (int i = 父对象.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(父对象.GetChild(i).gameObject);
            else
                UnityEngine.Object.DestroyImmediate(父对象.GetChild(i).gameObject);
        }
    }
    public static Vector3 v3(this Transform 变换)
    {
        return 变换.position;
    }
    public static Vector2 v2(this Transform 变换)
    {
        return new Vector2(变换.position.x, 变换.position.y);
    }
    public static Vector3 v3(this RectTransform 变换)
    {
        return 变换.anchoredPosition3D;
    }
    public static Vector2 v2(this RectTransform 变换)
    {
        return 变换.anchoredPosition;
    }
    public static Button 按钮(this Transform 变换)
    {
        return 变换.GetComponent<Button>();
    }
    public static Image 图片(this Transform 变换)
    {
        return 变换.GetComponent<Image>();
    }
    public static Text 文本(this Transform 变换,string v)
    {
        Text 文本组件 = 变换.GetComponent<Text>();
        if(文本组件!=null)
        {
            文本组件.text = v;
        }
        return 文本组件;
    }
    public static TMP_Text TMP_文本(this Transform 变换, string v)
    {
        TMP_Text 文本组件 = 变换.GetComponent<TMP_Text>();
        if (文本组件 != null)
        {
            文本组件.text = v;
        }
        return 文本组件;
    }
}