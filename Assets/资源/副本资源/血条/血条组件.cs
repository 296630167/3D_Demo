using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 血条组件 : MonoBehaviour
{
    SpriteRenderer 渲染器;
    Material 材质;
    float 当前血量;
    float 最大血量值;

    public void 初始化(float 最大血量)
    {
        渲染器 = GetComponent<SpriteRenderer>();
        材质 = 渲染器 != null ? 渲染器.material : null;
        最大血量值 = 最大血量;
        当前血量 = 最大血量值;
        transform.localPosition = new Vector3(0f, 2f, 0f);
        transform.设置旋转(new Vector3(45f, 0, 0));
        transform.设置缩放(new Vector3(1f, 0.1f, 1f));
        
        // 设置渲染层级，确保血条显示在最前面
        if (渲染器 != null)
        {
            渲染器.sortingOrder = 9999;
        }
        
        更新比例();
    }
    void 确保材质()
    {
        if (材质 == null)
        {
            渲染器 = 渲染器 ?? GetComponent<SpriteRenderer>();
            材质 = 渲染器 != null ? 渲染器.material : null;
        }
    }
    void 更新比例()
    {
        确保材质();
        if (材质 == null) return;
        float 比例 = 最大血量值 <= 0f ? 0f : Mathf.Clamp01(当前血量 / 最大血量值);
        材质.SetFloat("_HealthRatio", 比例);
    }

    public void 更新当前血量(float 数值)
    {
        当前血量 = 数值;
        if (当前血量 < 0f) 当前血量 = 0f;
        if (当前血量 > 最大血量值) 当前血量 = 最大血量值;
        更新比例();
    }

    public void 更新最大血量(float 数值)
    {
        最大血量值 = 数值;
        if (最大血量值 < 0f) 最大血量值 = 0f;
        if (当前血量 > 最大血量值) 当前血量 = 最大血量值;
        更新比例();
    }

    public void 更新背景颜色(Color 颜色)
    {
        确保材质();
        if (材质 == null) return;
        材质.SetColor("_BackgroundColor", 颜色);
    }

    public void 更新血条颜色(Color 颜色)
    {
        确保材质();
        if (材质 == null) return;
        材质.SetColor("_HealthBarColor", 颜色);
    }
    
}
