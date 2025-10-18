using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class 开关扩展
{
    public static Toggle 切换开关(this Toggle 开关, UnityAction<bool> 回调)
    {
        开关.onValueChanged.RemoveAllListeners();
        开关.onValueChanged.AddListener(回调);
        return 开关;
    }
    
    public static Toggle 设置状态(this Toggle 开关, bool 状态)
    {
        if (开关 != null) 开关.isOn = 状态;
        return 开关;
    }
    
    public static Toggle 设置可交互(this Toggle 开关, bool 可交互 = true)
    {
        if (开关 != null) 开关.interactable = 可交互;
        return 开关;
    }
}