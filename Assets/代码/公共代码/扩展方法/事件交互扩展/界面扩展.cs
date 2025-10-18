using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class 界面扩展
{
    #region 基础鼠标事件扩展

    public static GameObject 鼠标进入(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标进入事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标离开(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标离开事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标悬停期间(this GameObject 对象, UnityAction<PointerEventData> 方法, float 延迟时间 = 3.0f)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标悬停期间事件 = 方法;
        组件.悬停延迟时间 = 延迟时间;
        return 对象;
    }
    
    public static GameObject 鼠标悬停(this GameObject 对象, UnityAction<PointerEventData> 进入方法, UnityAction<PointerEventData> 离开方法 = null)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标进入事件 = 进入方法;
        if (离开方法 != null)
        {
            组件.鼠标离开事件 = 离开方法;
        }
        return 对象;
    }
    
    public static GameObject 鼠标按下(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标按下事件 = 方法;
        return 对象;
    }
    
    public static GameObject 鼠标抬起(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标抬起事件 = 方法;
        return 对象;
    }
    
    public static GameObject 鼠标点击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标点击事件 = 方法;
        return 对象;
    }
    
    #endregion

    #region 长按事件扩展

    public static GameObject 鼠标长按(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标长按事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标长按开始(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标长按开始事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标长按结束(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标长按结束事件 = 方法;
        return 对象;
    }

    #endregion

    #region 鼠标按键事件扩展

    public static GameObject 鼠标左键单击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标左键单击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标右键单击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标右键单击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标中键单击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标中键单击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标左键双击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标左键双击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标右键双击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标右键双击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标中键双击(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标中键双击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标左键多次点击(this GameObject 对象, UnityAction<PointerEventData, int> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标左键多次点击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标右键多次点击(this GameObject 对象, UnityAction<PointerEventData, int> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标右键多次点击事件 = 方法;
        return 对象;
    }

    public static GameObject 鼠标中键多次点击(this GameObject 对象, UnityAction<PointerEventData, int> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.鼠标中键多次点击事件 = 方法;
        return 对象;
    }

    #endregion

    #region 拖拽事件扩展

    public static GameObject 拖拽开始(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.拖拽开始事件 = 方法;
        return 对象;
    }

    public static GameObject 拖拽(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.拖拽中事件 = 方法;
        return 对象;
    }

    public static GameObject 拖拽结束(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.拖拽结束事件 = 方法;
        return 对象;
    }

    public static GameObject 拖拽放下(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.拖拽放下事件 = 方法;
        return 对象;
    }

    #endregion

    #region 滚动事件扩展

    public static GameObject 滚轮滚动(this GameObject 对象, UnityAction<PointerEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.滚轮滚动事件 = 方法;
        return 对象;
    }

    #endregion

    #region 选择事件扩展

    public static GameObject 选中(this GameObject 对象, UnityAction<BaseEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.选中事件 = 方法;
        return 对象;
    }

    public static GameObject 取消选中(this GameObject 对象, UnityAction<BaseEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.取消选中事件 = 方法;
        return 对象;
    }

    public static GameObject 更新选中(this GameObject 对象, UnityAction<BaseEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.更新选中事件 = 方法;
        return 对象;
    }

    #endregion

    #region 导航事件扩展

    public static GameObject 移动(this GameObject 对象, UnityAction<AxisEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.移动事件 = 方法;
        return 对象;
    }

    public static GameObject 提交(this GameObject 对象, UnityAction<BaseEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.提交事件 = 方法;
        return 对象;
    }

    public static GameObject 取消(this GameObject 对象, UnityAction<BaseEventData> 方法)
    {
        var 组件 = 获取或添加接口组件(对象);
        组件.取消事件 = 方法;
        return 对象;
    }

    #endregion

    #region 私有辅助方法

    private static 界面接口扩展脚本 获取或添加接口组件(GameObject 对象)
    {
        var 组件 = 对象.GetComponent<界面接口扩展脚本>();
        if (组件 == null)
        {
            组件 = 对象.AddComponent<界面接口扩展脚本>();
        }
        return 组件;
    }

    #endregion
}