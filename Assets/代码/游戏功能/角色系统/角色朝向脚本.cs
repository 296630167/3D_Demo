using UnityEngine;

public class 角色朝向脚本 : 基
{
    private Transform 根元素;
    public float 旋转速度 = 10f;
    public float 角度偏移 = 90f;
    public float 默认朝向角度 = 0f; // 默认待机时的Y轴旋转角度
    
    protected override void 开始时()
    {
        根元素 = t.parent;
    }
    
    // 新的朝向方法：只旋转Y轴，适配父级X轴45度旋转的结构
    public void 设置朝向(Vector3 目标位置, float 过度时间 = 0f)
    {
        Vector3 当前位置 = 根元素.position;
        Vector3 方向 = 目标位置 - 当前位置;
        方向.y = 0; // 忽略Y轴高度差
        
        if (方向.sqrMagnitude < 0.001f) return;
        
        // 计算Y轴旋转角度
        float Y轴角度 = Mathf.Atan2(方向.x, 方向.z) * Mathf.Rad2Deg;
        // 背对目标需要加180度，再加上偏移角度
        Y轴角度 = Y轴角度 + 180f + 角度偏移;
        
        设置Y轴旋转(Y轴角度, 过度时间);
    }
    
    // 重置为默认朝向（待机状态）
    public void 重置朝向(float 过度时间 = 0f)
    {
        设置Y轴旋转(默认朝向角度, 过度时间);
    }
    
    // 设置指定的Y轴旋转角度
    public void 设置Y轴旋转(float Y轴角度, float 过度时间 = 0f)
    {
        Vector3 新旋转 = new Vector3(0f, Y轴角度, 0f);
        
        if (过度时间 > 0f)
            t.设置旋转(新旋转, 过度时间);
        else
            t.localEulerAngles = 新旋转;
    }

    public void 平滑朝向(Vector3 目标位置)
    {
        Vector3 当前位置 = 根元素.position;
        Vector3 方向 = 目标位置 - 当前位置;
        方向.y = 0;
        
        if (方向.sqrMagnitude < 0.001f) return;
        
        float Y轴角度 = Mathf.Atan2(方向.x, 方向.z) * Mathf.Rad2Deg;
        Y轴角度 = Y轴角度 + 180f + 角度偏移;
        
        // 平滑旋转，只旋转Y轴
        float 当前Y = t.localEulerAngles.y;
        float 新Y = Mathf.LerpAngle(当前Y, Y轴角度, Time.deltaTime * 旋转速度);
        t.localEulerAngles = new Vector3(0f, 新Y, 0f);
    }
}