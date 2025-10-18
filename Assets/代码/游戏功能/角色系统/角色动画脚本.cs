using System;
using System.Collections.Generic;
using UnityEngine;

public class 角色动画脚本 : 基
{
    [Header("动画组件")]
    private Animator 动画控制器;
    private Dictionary<string,float> 动画字典 = new Dictionary<string, float>();
    
    protected override void 开始时()
    {
        初始化动画组件();
    }
    
    private void 初始化动画组件()
    {
        动画控制器 = GetComponent<Animator>();
        if (动画控制器 == null)
        {
            Debug.LogWarning($"角色动画脚本: 在 {gameObject.name} 上未找到Animator组件");
        }
        else
        {
            Debug.Log($"角色动画脚本初始化完成: {gameObject.name}");
        }
        foreach (var 动画 in 动画控制器.runtimeAnimatorController.animationClips)
        {
            动画字典.Add(动画.name, 动画.length);
        }
    }
    
    public void 播放指定动画(string 动画名称)
    {
        if (动画控制器 != null)
        {
            动画控制器.Play(动画名称, 0, 0f);
        }
    }
    
    public void 设置动画参数(string 参数名, bool 值)
    {
        if (动画控制器 != null)
        {
            动画控制器.SetBool(参数名, 值);
        }
    }
    
    public void 设置动画参数(string 参数名, float 值)
    {
        if (动画控制器 != null)
        {
            动画控制器.SetFloat(参数名, 值);
        }
    }
    
    public void 设置动画参数(string 参数名, int 值)
    {
        if (动画控制器 != null)
        {
            动画控制器.SetInteger(参数名, 值);
        }
    }
    
    public bool 获取动画状态(string 状态名)
    {
        if (动画控制器 != null)
        {
            return 动画控制器.GetCurrentAnimatorStateInfo(0).IsName(状态名);
        }
        return false;
    }
    
    public float 获取动画进度()
    {
        if (动画控制器 != null)
        {
            return 动画控制器.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        return 0f;
    }
}