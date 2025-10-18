using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public static class 下拉框扩展
{
    public static Dropdown 选项改变(this Dropdown 下拉框, UnityAction<int> 回调)
    {
        下拉框.onValueChanged.RemoveAllListeners();
        下拉框.onValueChanged.AddListener(回调);
        return 下拉框;
    }
    
    public static Dropdown 添加选项(this Dropdown 下拉框, string 选项文本)
    {
        if (下拉框 != null)
            下拉框.options.Add(new Dropdown.OptionData(选项文本));
        return 下拉框;
    }
    
    public static Dropdown 清空选项(this Dropdown 下拉框)
    {
        if (下拉框 != null) 下拉框.options.Clear();
        return 下拉框;
    }
    
    public static Dropdown 设置选中项(this Dropdown 下拉框, int 索引)
    {
        if (下拉框 != null) 下拉框.value = 索引;
        return 下拉框;
    }
    
    public static TMP_Dropdown 选项改变(this TMP_Dropdown TMP下拉框, UnityAction<int> 回调)
    {
        TMP下拉框.onValueChanged.RemoveAllListeners();
        TMP下拉框.onValueChanged.AddListener(回调);
        return TMP下拉框;
    }
    
    public static TMP_Dropdown 添加选项(this TMP_Dropdown TMP下拉框, string 选项文本)
    {
        if (TMP下拉框 != null)
            TMP下拉框.options.Add(new TMP_Dropdown.OptionData(选项文本));
        return TMP下拉框;
    }
    
    public static TMP_Dropdown 清空选项(this TMP_Dropdown TMP下拉框)
    {
        if (TMP下拉框 != null) TMP下拉框.options.Clear();
        return TMP下拉框;
    }
    
    public static TMP_Dropdown 设置选中项(this TMP_Dropdown TMP下拉框, int 索引)
    {
        if (TMP下拉框 != null) TMP下拉框.value = 索引;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置标签文本(this TMP_Dropdown TMP下拉框, string 文本)
    {
        if (TMP下拉框?.captionText != null)
            TMP下拉框.captionText.text = 文本;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置标签颜色(this TMP_Dropdown TMP下拉框, Color 颜色)
    {
        if (TMP下拉框?.captionText != null)
            TMP下拉框.captionText.color = 颜色;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置标签字体大小(this TMP_Dropdown TMP下拉框, float 大小)
    {
        if (TMP下拉框?.captionText != null)
            TMP下拉框.captionText.fontSize = 大小;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置标签对齐方式(this TMP_Dropdown TMP下拉框, TextAlignmentOptions 对齐方式)
    {
        if (TMP下拉框?.captionText != null)
            TMP下拉框.captionText.alignment = 对齐方式;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置标签字体资源(this TMP_Dropdown TMP下拉框, TMP_FontAsset 字体资源)
    {
        if (TMP下拉框?.captionText != null)
            TMP下拉框.captionText.font = 字体资源;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置项目文本颜色(this TMP_Dropdown TMP下拉框, Color 颜色)
    {
        if (TMP下拉框?.itemText != null)
            TMP下拉框.itemText.color = 颜色;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置项目文本字体大小(this TMP_Dropdown TMP下拉框, float 大小)
    {
        if (TMP下拉框?.itemText != null)
            TMP下拉框.itemText.fontSize = 大小;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置项目文本对齐方式(this TMP_Dropdown TMP下拉框, TextAlignmentOptions 对齐方式)
    {
        if (TMP下拉框?.itemText != null)
            TMP下拉框.itemText.alignment = 对齐方式;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置项目文本字体资源(this TMP_Dropdown TMP下拉框, TMP_FontAsset 字体资源)
    {
        if (TMP下拉框?.itemText != null)
            TMP下拉框.itemText.font = 字体资源;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP设置可交互(this TMP_Dropdown TMP下拉框, bool 可交互 = true)
    {
        if (TMP下拉框 != null) TMP下拉框.interactable = 可交互;
        return TMP下拉框;
    }
    
    public static TMP_Dropdown TMP刷新显示(this TMP_Dropdown TMP下拉框)
    {
        if (TMP下拉框 != null) TMP下拉框.RefreshShownValue();
        return TMP下拉框;
    }
}