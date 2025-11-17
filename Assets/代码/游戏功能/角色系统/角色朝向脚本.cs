using UnityEngine;

public class 角色朝向脚本 : 基
{
    private Transform 根元素;
    public float 旋转速度 = 10f;
    public float 角度偏移 = 90f;
    protected override void 开始时()
    {
        根元素 = t.parent;
    }
    public void 设置朝向(Vector3 目标位置, float 过度时间 = 0f)
    {
        // 使用根元素的位置计算方向，但控制角色模型的旋转
        Vector3 当前位置 = 根元素.position;
        Vector3 朝向目标的方向 = (目标位置 - 当前位置).normalized;
        Vector3 背对方向 = -朝向目标的方向;
        背对方向.y = 0;
        背对方向 = 背对方向.normalized;

        if (背对方向 != Vector3.zero)
        {
            Quaternion 基础旋转 = Quaternion.LookRotation(背对方向);
            Quaternion 偏移角度 = Quaternion.Euler(0, 角度偏移, 0);
            Quaternion 最终旋转 = 基础旋转 * 偏移角度;

            if (过度时间 > 0f)
                t.设置旋转(最终旋转.eulerAngles, 过度时间);
            else
                t.rotation = 最终旋转;
        }
    }

    public void 平滑朝向(Vector3 目标位置)
    {
        // 使用根元素的位置计算方向，但控制角色模型的旋转
        Vector3 当前位置 = 根元素.position;
        Vector3 朝向目标的方向 = (目标位置 - 当前位置).normalized;
        Vector3 背对方向 = -朝向目标的方向;
        背对方向.y = 0;
        背对方向 = 背对方向.normalized;

        if (背对方向 != Vector3.zero)
        {
            Quaternion 基础旋转 = Quaternion.LookRotation(背对方向);
            Quaternion 偏移角度 = Quaternion.Euler(0, 角度偏移, 0);
            Quaternion 目标旋转 = 基础旋转 * 偏移角度;

            // 平滑旋转角色模型
            t.rotation = Quaternion.Slerp(t.rotation, 目标旋转, 旋转速度 * Time.deltaTime);
        }
    }
}