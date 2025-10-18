using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class 输入框扩展
{
    public static InputField 修改中(this InputField 输入框, UnityAction<string> 回调)
    {
        输入框.onValueChanged.RemoveAllListeners();
        输入框.onValueChanged.AddListener(回调);
        return 输入框;
    }
    
    public static InputField 修改结束(this InputField 输入框, UnityAction<string> 回调)
    {
        输入框.onEndEdit.RemoveAllListeners();
        输入框.onEndEdit.AddListener(回调);
        return 输入框;
    }
    
    public static InputField 设置占位符文本(this InputField 输入框, string 占位符)
    {
        if (输入框?.placeholder is Text 占位符文本)
            占位符文本.text = 占位符;
        return 输入框;
    }
    
    public static InputField 限制数字输入(this InputField 输入框)
    {
        if (输入框 != null)
            输入框.contentType = InputField.ContentType.DecimalNumber;
        return 输入框;
    }
    
    public static InputField 限制整数输入(this InputField 输入框)
    {
        if (输入框 != null)
            输入框.contentType = InputField.ContentType.IntegerNumber;
        return 输入框;
    }
    
    public static InputField 设置字符限制(this InputField 输入框, int 最大字符数)
    {
        if (输入框 != null) 输入框.characterLimit = 最大字符数;
        return 输入框;
    }
}