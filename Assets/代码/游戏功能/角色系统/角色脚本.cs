using System;
using UnityEngine;

public class 角色脚本 : 基
{
    public 角色类 角色属性;
    [Header("角色组件")]
    public 角色动画脚本 角色动画;
    public 角色朝向脚本 角色朝向;
    public 角色移动脚本 角色移动;
    public 角色状态 当前状态;

    protected override void 开始时()
    {
        角色动画 = 对象("角色").AddComponent<角色动画脚本>();
        角色朝向 = 对象("角色").AddComponent<角色朝向脚本>();
        角色移动 = 对象("角色").AddComponent<角色移动脚本>();
        当前状态 = 角色状态.待机;
    }

    private void 设置动画参数(string v1, bool v2) => 角色动画.设置动画参数(v1, v2);
    private void 设置动画参数(string v1, float v2) => 角色动画.设置动画参数(v1, v2);
    private void 设置动画参数(string v1, int v2) => 角色动画.设置动画参数(v1, v2);

    protected override void 每帧更新()
    {
        switch (当前状态)
        {
            case 角色状态.待机:
                break;
            case 角色状态.移动:
                if(角色移动.是否到达目标点())
                {
                    当前状态 = 角色状态.待机;
                    设置动画参数("大剑跑步", false);
                }
                else
                {
                    角色朝向.平滑朝向(角色移动.获取目标位置());
                    角色移动.移动();
                }
                break;
            default:
                break;
        }
    }
    public void 移动到目标点(Vector3 移动位置)
    {
        角色移动.准备移动(移动位置);
        当前状态 = 角色状态.移动;
        设置动画参数("大剑跑步", true);
    }
}