using UnityEngine;
using TMPro;
using UnityEngine.Events;

public static class TMP输入框扩展
{
    public static TMP_InputField 修改中(this TMP_InputField TMP输入框, UnityAction<string> 回调)
    {
        TMP输入框.onValueChanged.RemoveAllListeners();
        TMP输入框.onValueChanged.AddListener(回调);
        return TMP输入框;
    }
    
    public static TMP_InputField 修改结束(this TMP_InputField TMP输入框, UnityAction<string> 回调)
    {
        TMP输入框.onEndEdit.RemoveAllListeners();
        TMP输入框.onEndEdit.AddListener(回调);
        return TMP输入框;
    }
    
    public static TMP_InputField 提交时(this TMP_InputField TMP输入框, UnityAction<string> 回调)
    {
        TMP输入框.onSubmit.RemoveAllListeners();
        TMP输入框.onSubmit.AddListener(回调);
        return TMP输入框;
    }
    
    public static TMP_InputField 选中时(this TMP_InputField TMP输入框, UnityAction<string> 回调)
    {
        TMP输入框.onSelect.RemoveAllListeners();
        TMP输入框.onSelect.AddListener(回调);
        return TMP输入框;
    }
    
    public static TMP_InputField 取消选中时(this TMP_InputField TMP输入框, UnityAction<string> 回调)
    {
        TMP输入框.onDeselect.RemoveAllListeners();
        TMP输入框.onDeselect.AddListener(回调);
        return TMP输入框;
    }
    
    public static TMP_InputField 设置占位符文本(this TMP_InputField TMP输入框, string 占位符)
    {
        if (TMP输入框?.placeholder is TextMeshProUGUI 占位符文本)
            占位符文本.text = 占位符;
        return TMP输入框;
    }

    public static TMP_InputField 设置文本(this TMP_InputField TMP输入框, string 文本)
    {
        if (TMP输入框 != null) TMP输入框.text = 文本;
        return TMP输入框;
    }
    public static TMP_InputField 设置文本(this TMP_InputField TMP输入框, int 文本)
    {
        if (TMP输入框 != null) TMP输入框.text = 文本.ToString();
        return TMP输入框;
    }

    public static TMP_InputField 限制数字输入(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null)
            TMP输入框.contentType = TMP_InputField.ContentType.DecimalNumber;
        return TMP输入框;
    }
    
    public static TMP_InputField 限制整数输入(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null)
            TMP输入框.contentType = TMP_InputField.ContentType.IntegerNumber;
        return TMP输入框;
    }
    
    public static TMP_InputField 限制字母数字输入(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null)
            TMP输入框.contentType = TMP_InputField.ContentType.Alphanumeric;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置密码模式(this TMP_InputField TMP输入框, bool 启用密码模式 = true)
    {
        if (TMP输入框 != null)
            TMP输入框.contentType = 启用密码模式 ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置字符限制(this TMP_InputField TMP输入框, int 最大字符数)
    {
        if (TMP输入框 != null) TMP输入框.characterLimit = 最大字符数;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置可交互(this TMP_InputField TMP输入框, bool 可交互 = true)
    {
        if (TMP输入框 != null) TMP输入框.interactable = 可交互;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置只读(this TMP_InputField TMP输入框, bool 只读 = true)
    {
        if (TMP输入框 != null) TMP输入框.readOnly = 只读;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置多行模式(this TMP_InputField TMP输入框, TMP_InputField.LineType 行类型 = TMP_InputField.LineType.MultiLineNewline)
    {
        if (TMP输入框 != null) TMP输入框.lineType = 行类型;
        return TMP输入框;
    }
    
    public static TMP_InputField 设置字符验证(this TMP_InputField TMP输入框, TMP_InputField.CharacterValidation 验证类型)
    {
        if (TMP输入框 != null) TMP输入框.characterValidation = 验证类型;
        return TMP输入框;
    }
    
    public static TMP_InputField 清空文本(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null) TMP输入框.text = string.Empty;
        return TMP输入框;
    }
    
    public static TMP_InputField 获得焦点(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null) TMP输入框.ActivateInputField();
        return TMP输入框;
    }
    
    public static TMP_InputField 失去焦点(this TMP_InputField TMP输入框)
    {
        if (TMP输入框 != null) TMP输入框.DeactivateInputField();
        return TMP输入框;
    }
}