using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class 界面接口扩展脚本: MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler,
    IScrollHandler, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler,
    IMoveHandler, ISubmitHandler, ICancelHandler
{
    #region 事件变量定义

    [Header("基础鼠标事件")]
    public UnityAction<PointerEventData> 鼠标进入事件;
    public UnityAction<PointerEventData> 鼠标离开事件;
    public UnityAction<PointerEventData> 鼠标悬停期间事件;
    public UnityAction<PointerEventData> 鼠标按下事件;
    public UnityAction<PointerEventData> 鼠标抬起事件;
    public UnityAction<PointerEventData> 鼠标点击事件;

    [Header("长按事件")]
    public UnityAction<PointerEventData> 鼠标长按事件;
    public UnityAction<PointerEventData> 鼠标长按开始事件;
    public UnityAction<PointerEventData> 鼠标长按结束事件;

    [Header("鼠标按键事件")]
    public UnityAction<PointerEventData> 鼠标左键单击事件;
    public UnityAction<PointerEventData> 鼠标右键单击事件;
    public UnityAction<PointerEventData> 鼠标中键单击事件;
    public UnityAction<PointerEventData> 鼠标左键双击事件;
    public UnityAction<PointerEventData> 鼠标右键双击事件;
    public UnityAction<PointerEventData> 鼠标中键双击事件;
    public UnityAction<PointerEventData, int> 鼠标左键多次点击事件;
    public UnityAction<PointerEventData, int> 鼠标右键多次点击事件;
    public UnityAction<PointerEventData, int> 鼠标中键多次点击事件;

    [Header("拖拽事件")]
    public UnityAction<PointerEventData> 拖拽开始事件;
    public UnityAction<PointerEventData> 拖拽中事件;
    public UnityAction<PointerEventData> 拖拽结束事件;
    public UnityAction<PointerEventData> 拖拽放下事件;

    [Header("滚动事件")]
    public UnityAction<PointerEventData> 滚轮滚动事件;

    [Header("选择事件")]
    public UnityAction<BaseEventData> 选中事件;
    public UnityAction<BaseEventData> 取消选中事件;
    public UnityAction<BaseEventData> 更新选中事件;

    [Header("导航事件")]
    public UnityAction<AxisEventData> 移动事件;
    public UnityAction<BaseEventData> 提交事件;
    public UnityAction<BaseEventData> 取消事件;

    #endregion

    #region 点击检测配置

    [Header("点击检测配置")]
    public float 双击间隔时间 = 0.15f;
    public float 多次点击间隔时间 = 0.1f;
    public float 长按触发时间 = 1.0f;
    public float 长按重复间隔 = 0.1f;
    public float 悬停延迟时间 = 3.0f;
    public float 悬停检测间隔 = 0.1f;

    #endregion

    #region 私有变量

    private float 上次左键点击时间;
    private float 上次右键点击时间;
    private float 上次中键点击时间;
    private int 左键点击次数;
    private int 右键点击次数;
    private int 中键点击次数;
    private Coroutine 左键点击检测协程;
    private Coroutine 右键点击检测协程;
    private Coroutine 中键点击检测协程;
    private Coroutine 长按检测协程;
    private bool 正在长按;
    private PointerEventData 长按事件数据;
    private bool 正在拖拽;
    private Coroutine 悬停检测协程;
    private bool 正在悬停;
    private bool 悬停已触发;

    #endregion

    #region 基础鼠标事件接口实现

    public void OnPointerEnter(PointerEventData eventData)
    {
        鼠标进入事件?.Invoke(eventData);
        开始悬停检测(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        鼠标离开事件?.Invoke(eventData);
        结束悬停检测();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        鼠标按下事件?.Invoke(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            开始长按检测(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        鼠标抬起事件?.Invoke(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            结束长按检测(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果正在拖拽，不处理点击事件
        if (正在拖拽)
        {
            return;
        }

        鼠标点击事件?.Invoke(eventData);

        float 当前时间 = Time.time;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            处理左键点击(eventData, 当前时间);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            处理右键点击(eventData, 当前时间);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            处理中键点击(eventData, 当前时间);
        }
    }

    #endregion

    #region 拖拽事件接口实现

    public void OnBeginDrag(PointerEventData eventData)
    {
        正在拖拽 = true;

        // 停止长按检测，避免拖拽时触发长按
        if (长按检测协程 != null)
        {
            StopCoroutine(长按检测协程);
            长按检测协程 = null;
        }

        // 停止点击检测，避免拖拽时触发点击事件
        if (左键点击检测协程 != null)
        {
            StopCoroutine(左键点击检测协程);
            左键点击检测协程 = null;
            左键点击次数 = 0;
        }

        拖拽开始事件?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        拖拽中事件?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        正在拖拽 = false;
        拖拽结束事件?.Invoke(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        拖拽放下事件?.Invoke(eventData);
    }

    #endregion

    #region 滚动事件接口实现

    public void OnScroll(PointerEventData eventData)
    {
        滚轮滚动事件?.Invoke(eventData);
    }

    #endregion

    #region 选择事件接口实现

    public void OnSelect(BaseEventData eventData)
    {
        选中事件?.Invoke(eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        取消选中事件?.Invoke(eventData);
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        更新选中事件?.Invoke(eventData);
    }

    #endregion

    #region 导航事件接口实现

    public void OnMove(AxisEventData eventData)
    {
        移动事件?.Invoke(eventData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        提交事件?.Invoke(eventData);
    }

    public void OnCancel(BaseEventData eventData)
    {
        取消事件?.Invoke(eventData);
    }

    #endregion

    #region 点击处理私有方法

    private void 处理左键点击(PointerEventData eventData, float 当前时间)
    {
        if (当前时间 - 上次左键点击时间 <= 双击间隔时间)
        {
            鼠标左键双击事件?.Invoke(eventData);
            左键点击次数 = 0;
            if (左键点击检测协程 != null)
            {
                StopCoroutine(左键点击检测协程);
                左键点击检测协程 = null;
            }
        }
        else
        {
            左键点击次数 = 1;
            if (左键点击检测协程 != null)
            {
                StopCoroutine(左键点击检测协程);
            }
            左键点击检测协程 = StartCoroutine(检测左键多次点击(eventData));
        }
        上次左键点击时间 = 当前时间;
    }

    private void 处理右键点击(PointerEventData eventData, float 当前时间)
    {
        if (当前时间 - 上次右键点击时间 <= 双击间隔时间)
        {
            鼠标右键双击事件?.Invoke(eventData);
            右键点击次数 = 0;
            if (右键点击检测协程 != null)
            {
                StopCoroutine(右键点击检测协程);
                右键点击检测协程 = null;
            }
        }
        else
        {
            右键点击次数 = 1;
            if (右键点击检测协程 != null)
            {
                StopCoroutine(右键点击检测协程);
            }
            右键点击检测协程 = StartCoroutine(检测右键多次点击(eventData));
        }
        上次右键点击时间 = 当前时间;
    }

    private void 处理中键点击(PointerEventData eventData, float 当前时间)
    {
        if (当前时间 - 上次中键点击时间 <= 双击间隔时间)
        {
            鼠标中键双击事件?.Invoke(eventData);
            中键点击次数 = 0;
            if (中键点击检测协程 != null)
            {
                StopCoroutine(中键点击检测协程);
                中键点击检测协程 = null;
            }
        }
        else
        {
            中键点击次数 = 1;
            if (中键点击检测协程 != null)
            {
                StopCoroutine(中键点击检测协程);
            }
            中键点击检测协程 = StartCoroutine(检测中键多次点击(eventData));
        }
        上次中键点击时间 = 当前时间;
    }

    #endregion

    #region 点击检测协程

    private IEnumerator 检测左键多次点击(PointerEventData eventData)
    {
        float 开始时间 = Time.time;
        while (Time.time - 开始时间 < 多次点击间隔时间)
        {
            yield return null;
        }

        if (左键点击次数 == 1)
        {
            鼠标左键单击事件?.Invoke(eventData);
        }
        else if (左键点击次数 > 1)
        {
            鼠标左键多次点击事件?.Invoke(eventData, 左键点击次数);
        }

        左键点击次数 = 0;
        左键点击检测协程 = null;
    }

    private IEnumerator 检测右键多次点击(PointerEventData eventData)
    {
        float 开始时间 = Time.time;
        while (Time.time - 开始时间 < 多次点击间隔时间)
        {
            yield return null;
        }

        if (右键点击次数 == 1)
        {
            鼠标右键单击事件?.Invoke(eventData);
        }
        else if (右键点击次数 > 1)
        {
            鼠标右键多次点击事件?.Invoke(eventData, 右键点击次数);
        }

        右键点击次数 = 0;
        右键点击检测协程 = null;
    }

    private IEnumerator 检测中键多次点击(PointerEventData eventData)
    {
        float 开始时间 = Time.time;
        while (Time.time - 开始时间 < 多次点击间隔时间)
        {
            yield return null;
        }

        if (中键点击次数 == 1)
        {
            鼠标中键单击事件?.Invoke(eventData);
        }
        else if (中键点击次数 > 1)
        {
            鼠标中键多次点击事件?.Invoke(eventData, 中键点击次数);
        }

        中键点击次数 = 0;
        中键点击检测协程 = null;
    }

    #endregion

    #region 长按检测方法

    private void 开始长按检测(PointerEventData eventData)
    {
        // 如果正在拖拽，不启动长按检测
        if (正在拖拽)
        {
            return;
        }

        长按事件数据 = eventData;
        正在长按 = false;

        if (长按检测协程 != null)
        {
            StopCoroutine(长按检测协程);
        }

        长按检测协程 = StartCoroutine(检测长按(eventData));
    }

    private void 结束长按检测(PointerEventData eventData)
    {
        if (长按检测协程 != null)
        {
            StopCoroutine(长按检测协程);
            长按检测协程 = null;
        }

        if (正在长按)
        {
            正在长按 = false;
            鼠标长按结束事件?.Invoke(eventData);
        }
    }

    private IEnumerator 检测长按(PointerEventData eventData)
    {
        yield return new WaitForSeconds(长按触发时间);

        if (!正在长按)
        {
            正在长按 = true;
            鼠标长按开始事件?.Invoke(eventData);
            鼠标长按事件?.Invoke(eventData);
        }

        while (正在长按)
        {
            yield return new WaitForSeconds(长按重复间隔);
            if (正在长按)
            {
                鼠标长按事件?.Invoke(eventData);
            }
        }
    }

    #endregion

    #region 悬停检测方法

    private void 开始悬停检测(PointerEventData eventData)
    {
        正在悬停 = true;
        悬停已触发 = false;
        
        if (悬停检测协程 != null)
        {
            StopCoroutine(悬停检测协程);
        }
        
        悬停检测协程 = StartCoroutine(检测悬停期间(eventData));
    }
    
    private void 结束悬停检测()
    {
        正在悬停 = false;
        悬停已触发 = false;
        
        if (悬停检测协程 != null)
        {
            StopCoroutine(悬停检测协程);
            悬停检测协程 = null;
        }
    }
    
    private IEnumerator 检测悬停期间(PointerEventData eventData)
    {
        yield return new WaitForSeconds(悬停延迟时间);
        
        if (正在悬停)
        {
            悬停已触发 = true;
        }
        
        while (正在悬停 && 悬停已触发)
        {
            鼠标悬停期间事件?.Invoke(eventData);
            yield return null;
        }
    }
    
    #endregion
}