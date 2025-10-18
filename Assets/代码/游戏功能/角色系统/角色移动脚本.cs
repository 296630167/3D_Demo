using UnityEngine;

public class 角色移动脚本 : 基
{
    private Transform 根元素;
    private Vector3 目标位置;
    protected override void 开始时()
    {
        根元素 = t.parent;
    }
    public void 准备移动(Vector3 目标位置)
    {
        this.目标位置 = 目标位置;
    }
    public void 移动(float 移动速度 = 10f)
    {
        if (根元素 == null) return;
        根元素.position = Vector3.MoveTowards(根元素.position, 目标位置, 移动速度 * Time.deltaTime);
    }
    public Vector3 获取位置() => 根元素 != null ? 根元素.position : Vector3.zero;
    public Vector3 获取目标位置() => 目标位置;
    public void 设置位置(Vector3 新位置) => 根元素.position = 新位置;
    public float 计算到目标距离(Vector3 目标位置)
    {
        if (根元素 == null) return float.MaxValue;
        return Vector3.Distance(根元素.position, 目标位置);
    }
    public bool 是否到达目标点(float 容差 = 0.1f) => 计算到目标距离(目标位置) <= 容差;
}