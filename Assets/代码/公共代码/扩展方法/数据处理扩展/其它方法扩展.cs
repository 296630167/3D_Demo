using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class 其它方法扩展
{
    #region 截图和文件操作

    public static void 截屏(string 文件名 = "截图", int 宽 = 1920, int 高 = 1080, string 保存路径 = null)
    {
        if (string.IsNullOrEmpty(保存路径))
        {
            保存路径 = Path.Combine(Application.persistentDataPath, "截图");
        }
        if (!Directory.Exists(保存路径))
        {
            Directory.CreateDirectory(保存路径);
        }
        string 完整路径 = Path.Combine(保存路径, $"{文件名}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
        ScreenCapture.CaptureScreenshot(完整路径);
        Debug.Log($"截图已保存至: {完整路径}");
    }

    public static void 打开游戏根目录(this object 对象)
    {
        Application.OpenURL(Application.persistentDataPath);
    }

    #endregion

    #region 字符串和随机数生成

    public static string 获取随机字符串(this object 对象, int 长度)
    {
        const string 字符集 = "0123456789abwjefghijklmnopqrstuvwxyzABwjEFGHIJKLMNOPQRSTUVWXYZ";
        var 随机数 = new System.Random();
        var 结果 = new char[长度];

        for (int i = 0; i < 长度; i++)
        {
            结果[i] = 字符集[随机数.Next(字符集.Length)];
        }

        return new string(结果);
    }

    public static float 在字符串区间生成随机数(this string 范围)
    {
        string[] 范围数组 = 范围.Split('-');
        float 最小值 = float.Parse(范围数组[0]);
        float 最大值 = float.Parse(范围数组[1]);
        return 随机数(最小值, 最大值);
    }

    #endregion

    #region 深拷贝

    public static T Json深拷贝<T>(this T 原数据) where T : class
    {
        if (原数据 == null) return null;
        string json字符串 = JsonConvert.SerializeObject(原数据);
        return JsonConvert.DeserializeObject<T>(json字符串);
    }

    #endregion

    #region 鼠标和输入控制

    public static bool 鼠标悬停在UI元素上(this object 对象)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 0)
        {
            RaycastResult topResult = results[0];
            if (topResult.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }

    public static Vector2 鼠标位置(this object 对象)
    {
        return Input.mousePosition;
    }

    public static Vector3 鼠标世界位置(this object 对象, Camera 相机 = null, float 距离 = 100f)
    {
        if (相机 == null)
            相机 = Camera.main;
        Vector3 鼠标位置 = Input.mousePosition;
        Ray 射线 = 相机.ScreenPointToRay(鼠标位置);
        if (Physics.Raycast(射线, out RaycastHit 碰撞信息, 距离))
        {
            return 碰撞信息.point;
        }
        return 射线.GetPoint(距离);
    }

    public static Vector2 鼠标世界位置2D(this object 对象, Camera 相机 = null)
    {
        if (相机 == null)
            相机 = Camera.main;
        Vector3 鼠标位置 = Input.mousePosition;
        return 相机.ScreenToWorldPoint(new Vector3(鼠标位置.x, 鼠标位置.y, 相机.nearClipPlane));
    }

    public static void 设置鼠标可见(this object 对象, bool 可见 = true)
    {
        Cursor.visible = 可见;
    }

    public static void 设置鼠标锁定(this object 对象, bool 锁定 = true)
    {
        Cursor.lockState = 锁定 ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public static void 设置多点触控(this object 对象, bool 启用 = false)
    {
        Input.multiTouchEnabled = 启用;
    }

    #endregion

    #region 颜色处理

    public static Color 颜色(this string 颜色, float 透明度 = 1f)
    {
        if (ColorUtility.TryParseHtmlString($"#{颜色}", out Color 新颜色))
        {
            新颜色.a = 透明度;
            return 新颜色;
        }
        else
        {
            return new Color(0, 0, 0, 透明度);
        }
    }

    public static Color 随机颜色(this object 对象)
    {
        return new Color(随机数(0f, 1f), 随机数(0f, 1f), 随机数(0f, 1f), 1f);
    }

    #endregion

    #region 随机数和概率

    public static float 随机数(float 最小值, float 最大值)
    {
        return UnityEngine.Random.Range(最小值, 最大值);
    }

    public static float 随机数(this object 对象, float 最小值, float 最大值)
    {
        return UnityEngine.Random.Range(最小值, 最大值);
    }

    public static int 随机数(int 最小值, int 最大值)
    {
        return UnityEngine.Random.Range(最小值, 最大值 + 1);
    }

    public static int 随机数(this object 对象, int 最小值, int 最大值)
    {
        return UnityEngine.Random.Range(最小值, 最大值 + 1);
    }

    public static bool 触发结果(this int 触发概率)
    {
        return 随机数(1, 100) <= 触发概率;
    }

    public static bool 触发结果(this float 触发概率)
    {
        return 随机数(0f, 1f) <= 触发概率;
    }

    #endregion

    #region 数值范围和限制

    public static bool 在范围内(this int 值, int 最小值, int 最大值)
    {
        return 值 >= 最小值 && 值 <= 最大值;
    }

    public static bool 在范围内(this float 值, float 最小值, float 最大值)
    {
        return 值 >= 最小值 && 值 <= 最大值;
    }

    public static float 限制范围(this float 值, float 最小值, float 最大值)
    {
        return 值 < 最小值 ? 最小值 : (值 > 最大值 ? 最大值 : 值);
    }

    public static double 限制范围(this double 值, double 最小值, double 最大值)
    {
        return 值 < 最小值 ? 最小值 : (值 > 最大值 ? 最大值 : 值);
    }

    #endregion

    #region 数学运算

    public static int 绝对值(this int 值)
    {
        return Mathf.Abs(值);
    }

    public static float 绝对值(this float 值)
    {
        return Mathf.Abs(值);
    }

    public static int 最大值(this int 值, params int[] 其他值)
    {
        int 最大值 = 值;
        foreach (int 当前值 in 其他值)
        {
            if (当前值 > 最大值)
            {
                最大值 = 当前值;
            }
        }
        return 最大值;
    }

    public static float 最大值(this float 值, params float[] 其他值)
    {
        float 最大值 = 值;
        foreach (float 当前值 in 其他值)
        {
            if (当前值 > 最大值)
            {
                最大值 = 当前值;
            }
        }
        return 最大值;
    }

    public static int 最小值(this int 值, params int[] 其他值)
    {
        int 最小值 = 值;
        foreach (int 当前值 in 其他值)
        {
            if (当前值 < 最小值)
            {
                最小值 = 当前值;
            }
        }
        return 最小值;
    }

    public static float 最小值(this float 值, params float[] 其他值)
    {
        float 最小值 = 值;
        foreach (float 当前值 in 其他值)
        {
            if (当前值 < 最小值)
            {
                最小值 = 当前值;
            }
        }
        return 最小值;
    }

    public static int 幂(this int 数字, int 幂)
    {
        return (int)Mathf.Pow(数字, 幂);
    }

    public static float 幂(this float 数字, float 幂)
    {
        return Mathf.Pow(数字, 幂);
    }

    public static int 平方根(this int 数字)
    {
        return (int)Mathf.Sqrt(数字);
    }

    public static float 平方根(this float 数字)
    {
        return Mathf.Sqrt(数字);
    }

    public static int 四舍五入(this int 数字)
    {
        return Mathf.RoundToInt(数字);
    }

    public static float 四舍五入(this float 数字)
    {
        return Mathf.Round(数字);
    }

    public static int 向上取整(this int 数字)
    {
        return Mathf.CeilToInt(数字);
    }

    public static float 向上取整(this float 数字)
    {
        return Mathf.Ceil(数字);
    }

    public static int 向下取整(this int 数字)
    {
        return Mathf.FloorToInt(数字);
    }

    public static float 向下取整(this float 数字)
    {
        return Mathf.Floor(数字);
    }

    #endregion

    #region 向量计算

    public static float 向量距离(this Vector2 起点, Vector2 终点)
    {
        return Vector2.Distance(起点, 终点);
    }

    public static float 向量距离(this Vector3 起点, Vector3 终点)
    {
        return Vector3.Distance(起点, 终点);
    }

    public static float 计算角度(this Vector3 起点, Vector3 终点)
    {
        return Mathf.Atan2(终点.y - 起点.y, 终点.x - 起点.x) * Mathf.Rad2Deg;
    }

    public static float 计算角度(this Vector2 起点, Vector2 终点)
    {
        return Mathf.Atan2(终点.y - 起点.y, 终点.x - 起点.x) * Mathf.Rad2Deg;
    }

    public static Vector3 计算方向(this Vector3 起点, Vector3 终点)
    {
        return (终点 - 起点).normalized;
    }

    public static Vector2 计算方向(this Vector2 起点, Vector2 终点)
    {
        return (终点 - 起点).normalized;
    }

    public static Vector3 计算旋转角度(this Vector3 起点, Vector3 终点, float 旋转速度)
    {
        Vector3 方向 = 起点.计算方向(终点);
        float 角度 = 起点.计算角度(终点);
        角度 += 旋转速度 * Time.deltaTime;
        方向 = new Vector3(Mathf.Cos(角度 * Mathf.Deg2Rad), Mathf.Sin(角度 * Mathf.Deg2Rad), 0);
        return 方向;
    }

    public static Vector2 计算旋转角度(this Vector2 起点, Vector2 终点, float 旋转速度)
    {
        Vector2 方向 = 起点.计算方向(终点);
        float 角度 = 起点.计算角度(终点);
        角度 += 旋转速度 * Time.deltaTime;
        方向 = new Vector2(Mathf.Cos(角度 * Mathf.Deg2Rad), Mathf.Sin(角度 * Mathf.Deg2Rad));
        return 方向;
    }

    #endregion

    #region 时间和游戏控制

    public static void 设置时间缩放(this object 对象, float 缩放 = 1.0f)
    {
        Time.timeScale = 缩放;
    }

    public static float 游戏帧率(this object 对象)
    {
        return 1.0f / Time.unscaledDeltaTime;
    }

    public static void 设置游戏帧率(this object 对象, int 帧率 = -1)
    {
        Application.targetFrameRate = 帧率;
    }

    public static void 退出应用程序(this object 对象)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static DateTime 当前时间(this object 对象)
    {
        return DateTime.Now;
    }

    #endregion

    #region 日期时间操作

    public static DateTime 添加天数(this DateTime 日期, int 天数)
    {
        return 日期.AddDays(天数);
    }

    public static DateTime 添加小时(this DateTime 日期, double 小时)
    {
        return 日期.AddHours(小时);
    }

    public static DateTime 添加分钟(this DateTime 日期, double 分钟)
    {
        return 日期.AddMinutes(分钟);
    }

    public static DateTime 添加秒数(this DateTime 日期, double 秒数)
    {
        return 日期.AddSeconds(秒数);
    }

    public static DateTime 添加毫秒(this DateTime 日期, double 毫秒)
    {
        return 日期.AddMilliseconds(毫秒);
    }

    public static DateTime 添加月份(this DateTime 日期, int 月份)
    {
        return 日期.AddMonths(月份);
    }

    public static DateTime 添加年份(this DateTime 日期, int 年份)
    {
        return 日期.AddYears(年份);
    }

    #endregion

    #region 集合操作

    public static T 随机返回<T>(this List<T> 列表)
    {
        if (列表 == null || 列表.Count == 0)
        {
            return default(T);
        }
        
        int 随机索引 = 随机数(0, 列表.Count - 1);
        return 列表[随机索引];
    }
    public static KeyValuePair<TKey, TValue> 随机返回<TKey, TValue>(this Dictionary<TKey, TValue> 字典)
    {
        if (字典 == null || 字典.Count == 0)
        {
            return default(KeyValuePair<TKey, TValue>);
        }
        
        int 随机索引 = 随机数(0, 字典.Count - 1);
        return 字典.ElementAt(随机索引);
    }
    #endregion
}
