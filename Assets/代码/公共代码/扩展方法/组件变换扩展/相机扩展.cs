using UnityEngine;
using DG.Tweening;

public static class 相机扩展
{
    public static Camera 设置视野角度(this Camera 相机, float 视野角度, float 动画时间 = 0f)
    {
        if (相机 != null)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 相机.fieldOfView, x => 相机.fieldOfView = x, 视野角度, 动画时间);
            else
                相机.fieldOfView = 视野角度;
        }
        return 相机;
    }

    public static Camera 设置正交大小(this Camera 相机, float 正交大小, float 动画时间 = 0f)
    {
        if (相机 != null && 相机.orthographic)
        {
            if (动画时间 > 0f)
                DOTween.To(() => 相机.orthographicSize, x => 相机.orthographicSize = x, 正交大小, 动画时间);
            else
                相机.orthographicSize = 正交大小;
        }
        return 相机;
    }

    public static Camera 跟随目标(this Camera 相机, Transform 目标, Vector3 偏移量 = default, float 跟随速度 = 5f)
    {
        if (相机 != null && 目标 != null)
        {
            Vector3 目标位置 = 目标.position + 偏移量;
            if (跟随速度 > 0f)
            {
                相机.transform.position = Vector3.Lerp(相机.transform.position, 目标位置, Time.deltaTime * 跟随速度);
            }
            else
            {
                相机.transform.position = 目标位置;
            }
        }
        return 相机;
    }

    public static Camera 震动效果(this Camera 相机, float 震动强度 = 0.5f, float 持续时间 = 0.3f)
    {
        if (相机 != null)
        {
            Vector3 原始位置 = 相机.transform.position;
            相机.transform.DOShakePosition(持续时间, 震动强度)
                  .OnComplete(() => 相机.transform.position = 原始位置);
        }
        return 相机;
    }

    public static Camera 平滑看向目标(this Camera 相机, Transform 目标, float 旋转速度 = 2f, float 动画时间 = 0f)
    {
        if (相机 != null && 目标 != null)
        {
            Vector3 方向 = (目标.position - 相机.transform.position).normalized;
            Quaternion 目标旋转 = Quaternion.LookRotation(方向);
            
            if (动画时间 > 0f)
            {
                相机.transform.DORotateQuaternion(目标旋转, 动画时间);
            }
            else if (旋转速度 > 0f)
            {
                相机.transform.rotation = Quaternion.Slerp(相机.transform.rotation, 目标旋转, Time.deltaTime * 旋转速度);
            }
            else
            {
                相机.transform.rotation = 目标旋转;
            }
        }
        return 相机;
    }

    public static Camera 设置渲染层级(this Camera 相机, int 层级)
    {
        if (相机 != null)
        {
            相机.depth = 层级;
        }
        return 相机;
    }

    public static Camera 设置背景颜色(this Camera 相机, Color 颜色)
    {
        if (相机 != null)
        {
            相机.backgroundColor = 颜色;
        }
        return 相机;
    }

    public static Camera 设置渲染模式(this Camera 相机, CameraClearFlags 清除标志)
    {
        if (相机 != null)
        {
            相机.clearFlags = 清除标志;
        }
        return 相机;
    }

    public static Camera 设置裁剪距离(this Camera 相机, float 近裁剪面, float 远裁剪面)
    {
        if (相机 != null)
        {
            相机.nearClipPlane = 近裁剪面;
            相机.farClipPlane = 远裁剪面;
        }
        return 相机;
    }

    public static Camera 切换投影模式(this Camera 相机, bool 是否正交 = true)
    {
        if (相机 != null)
        {
            相机.orthographic = 是否正交;
        }
        return 相机;
    }

    public static Camera 锁定位置(this Camera 相机, Vector3 目标位置)
    {
        if (相机 != null)
        {
            相机.transform.LookAt(目标位置);
        }
        return 相机;
    }

    public static Camera 锁定单位(this Camera 相机, Transform 目标单位)
    {
        if (相机 != null && 目标单位 != null)
        {
            Vector3 看向目标点 = 目标单位.position + Vector3.up * 1.5f;
            相机.transform.LookAt(看向目标点);
        }
        return 相机;
    }

    public static Camera 锁定位置(this Camera 相机, Vector3 目标位置, Vector3 位置偏移, Vector3 旋转角度)
    {
        if (相机 != null)
        {
            相机.transform.position = 目标位置 + 位置偏移;
            相机.transform.eulerAngles = 旋转角度;
        }
        return 相机;
    }

    public static Camera 锁定单位(this Camera 相机, Transform 目标单位, Vector3 位置偏移, Vector3 旋转角度)
    {
        if (相机 != null && 目标单位 != null)
        {
            相机.transform.position = 目标单位.position + 位置偏移;
            相机.transform.eulerAngles = 旋转角度;
        }
        return 相机;
    }
}