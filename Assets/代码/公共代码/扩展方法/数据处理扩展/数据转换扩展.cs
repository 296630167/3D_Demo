using System;
using UnityEngine;

/// <summary>
/// 数据转换扩展类，提供各种数据类型之间的转换方法
/// 支持object类型到基础数据类型的安全转换
/// 包含空值检查、类型匹配、异常处理等功能
/// </summary>
public static class 数据转换扩展
{
    #region 转为整数
    /// <summary>
    /// 将任意对象转换为整数类型
    /// 支持多种数据类型的智能转换，包括数值类型、布尔值、字符串等
    /// 对于浮点数会进行四舍五入处理
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的整数值，转换失败时返回0</returns>
    public static int 转为整数(this object 值)
    {
        // 检查输入值是否为null或数据库空值，如果是则返回默认值0
        if (值 == null || 值 == DBNull.Value) return 0;
        
        // 使用switch表达式进行类型匹配，提供最优的转换路径
        switch (值)
        {
            // 如果已经是int类型，直接返回，无需转换
            case int i: return i;
            // float类型使用Unity的RoundToInt方法进行四舍五入转换
            case float f: return Mathf.RoundToInt(f);
            // double类型使用Math.Round进行四舍五入后强制转换为int
            case double d: return (int)Math.Round(d);
            // decimal类型使用Math.Round进行四舍五入后强制转换为int
            case decimal dec: return (int)Math.Round(dec);
            // 布尔值转换：true转为1，false转为0
            case bool b: return b ? 1 : 0;
            // 字符串类型需要尝试解析
            case string s:
                // 首先尝试直接解析为整数
                if (int.TryParse(s, out int 结果))
                    return 结果;
                // 如果直接解析失败，尝试先解析为浮点数再转换为整数
                if (float.TryParse(s, out float 浮点结果))
                    return Mathf.RoundToInt(浮点结果);
                // 字符串解析完全失败时返回0
                return 0;
            // 对于其他未知类型，尝试调用ToString()后再解析
            default:
                // 将对象转换为字符串后尝试解析为整数
                if (int.TryParse(值.ToString(), out int 默认结果))
                    return 默认结果;
                // 所有转换方式都失败时返回0
                return 0;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为整数类型，转换失败时返回指定的默认值
    /// 这是转为整数方法的重载版本，提供了异常安全保障
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时返回的默认值</param>
    /// <returns>转换后的整数值或默认值</returns>
    public static int 转为整数(this object 值, int 默认值)
    {
        try
        {
            // 调用基础的转为整数方法进行转换
            return 值.转为整数();
        }
        catch
        {
            // 捕获任何可能的异常，返回用户指定的默认值
            return 默认值;
        }
    }
    #endregion
    
    #region 转为浮点数
    /// <summary>
    /// 将任意对象转换为单精度浮点数类型
    /// 支持各种数值类型、布尔值、字符串的转换
    /// 保持浮点数的精度特性
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的浮点数值，转换失败时返回0f</returns>
    public static float 转为浮点数(this object 值)
    {
        // 检查输入值是否为null或数据库空值，返回浮点数默认值0f
        if (值 == null || 值 == DBNull.Value) return 0f;
        
        // 使用switch进行类型匹配转换
        switch (值)
        {
            // 如果已经是float类型，直接返回
            case float f: return f;
            // int类型强制转换为float
            case int i: return (float)i;
            // double类型强制转换为float（可能会有精度损失）
            case double d: return (float)d;
            // decimal类型强制转换为float
            case decimal dec: return (float)dec;
            // 布尔值转换：true转为1f，false转为0f
            case bool b: return b ? 1f : 0f;
            // 字符串类型尝试解析为浮点数
            case string s:
                // 使用TryParse方法安全解析字符串为浮点数
                if (float.TryParse(s, out float 结果))
                    return 结果;
                // 解析失败返回0f
                return 0f;
            // 其他类型先转换为字符串再尝试解析
            default:
                // 调用ToString()方法后尝试解析为浮点数
                if (float.TryParse(值.ToString(), out float 默认结果))
                    return 默认结果;
                // 所有方式都失败时返回0f
                return 0f;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为浮点数类型，转换失败时返回指定的默认值
    /// 提供异常安全的浮点数转换功能
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时返回的默认值</param>
    /// <returns>转换后的浮点数值或默认值</returns>
    public static float 转为浮点数(this object 值, float 默认值)
    {
        try
        {
            // 调用基础的转为浮点数方法
            return 值.转为浮点数();
        }
        catch
        {
            // 异常时返回用户指定的默认值
            return 默认值;
        }
    }
    #endregion
    
    #region 转为双精度浮点数
    /// <summary>
    /// 将任意对象转换为双精度浮点数类型
    /// 提供比float更高的精度，适用于需要高精度计算的场景
    /// 支持各种数值类型的无损或最小损失转换
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的双精度浮点数值，转换失败时返回0.0</returns>
    public static double 转为双精度浮点数(this object 值)
    {
        // 检查空值，返回双精度浮点数的默认值0.0
        if (值 == null || 值 == DBNull.Value) return 0.0;
        
        // 类型匹配转换
        switch (值)
        {
            // 如果已经是double类型，直接返回
            case double d: return d;
            // float转double，精度会提升
            case float f: return (double)f;
            // int转double，无精度损失
            case int i: return (double)i;
            // decimal转double，可能有精度变化
            case decimal dec: return (double)dec;
            // 布尔值转换：true转为1.0，false转为0.0
            case bool b: return b ? 1.0 : 0.0;
            // 字符串解析为双精度浮点数
            case string s:
                // 使用TryParse安全解析
                if (double.TryParse(s, out double 结果))
                    return 结果;
                // 解析失败返回0.0
                return 0.0;
            // 其他类型通过ToString转换后解析
            default:
                if (double.TryParse(值.ToString(), out double 默认结果))
                    return 默认结果;
                return 0.0;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为双精度浮点数，转换失败时返回指定默认值
    /// 异常安全的双精度浮点数转换方法
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时的默认值</param>
    /// <returns>转换后的双精度浮点数或默认值</returns>
    public static double 转为双精度浮点数(this object 值, double 默认值)
    {
        try
        {
            // 调用基础转换方法
            return 值.转为双精度浮点数();
        }
        catch
        {
            // 异常保护，返回默认值
            return 默认值;
        }
    }
    #endregion
    
    #region 转为字符串
    /// <summary>
    /// 将任意对象转换为字符串类型
    /// 这是最通用的转换方法，几乎所有对象都可以转换为字符串
    /// 对null值和数据库空值进行特殊处理
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>对象的字符串表示，null值返回空字符串</returns>
    public static string 转为字符串(this object 值)
    {
        // null值或数据库空值返回空字符串而不是null
        if (值 == null || 值 == DBNull.Value) return string.Empty;
        // 调用对象的ToString()方法获取字符串表示
        return 值.ToString();
    }
    
    /// <summary>
    /// 将任意对象转换为字符串，转换结果为空时返回指定默认值
    /// 提供了对空字符串的额外处理逻辑
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换结果为空时的默认值</param>
    /// <returns>对象的字符串表示或默认值</returns>
    public static string 转为字符串(this object 值, string 默认值)
    {
        try
        {
            // 调用基础转换方法
            var 结果 = 值.转为字符串();
            // 如果结果为null或空字符串，返回默认值
            return string.IsNullOrEmpty(结果) ? 默认值 : 结果;
        }
        catch
        {
            // 异常时返回默认值
            return 默认值;
        }
    }
    #endregion
    
    #region 转为布尔值
    /// <summary>
    /// 将任意对象转换为布尔值类型
    /// 实现了多种类型到布尔值的智能转换规则
    /// 数值类型：非零为true，零为false
    /// 字符串：可解析的布尔值或非空字符串为true
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的布尔值，转换失败时返回false</returns>
    public static bool 转为布尔值(this object 值)
    {
        // null值或数据库空值返回false
        if (值 == null || 值 == DBNull.Value) return false;
        
        // 根据不同类型实现转换逻辑
        switch (值)
        {
            // 如果已经是bool类型，直接返回
            case bool b: return b;
            // 整数：非零为true，零为false
            case int i: return i != 0;
            // 浮点数：使用Unity的Approximately方法检查是否接近0
            case float f: return !Mathf.Approximately(f, 0f);
            // 双精度：使用Epsilon进行精度比较
            case double d: return Math.Abs(d) > double.Epsilon;
            // 字符串类型需要多重解析策略
            case string s:
                // 首先尝试解析为标准布尔值（"true"/"false"）
                if (bool.TryParse(s, out bool 布尔结果))
                    return 布尔结果;
                // 尝试解析为整数，非零为true
                if (int.TryParse(s, out int 整数结果))
                    return 整数结果 != 0;
                // 非空字符串视为true，空字符串视为false
                return !string.IsNullOrEmpty(s);
            // 其他类型先转字符串再解析
            default:
                if (bool.TryParse(值.ToString(), out bool 默认结果))
                    return 默认结果;
                return false;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为布尔值，转换失败时返回指定默认值
    /// 异常安全的布尔值转换方法
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时的默认值</param>
    /// <returns>转换后的布尔值或默认值</returns>
    public static bool 转为布尔值(this object 值, bool 默认值)
    {
        try
        {
            // 调用基础转换方法
            return 值.转为布尔值();
        }
        catch
        {
            // 异常保护
            return 默认值;
        }
    }
    #endregion
    
    #region 转为长整数
    /// <summary>
    /// 将任意对象转换为64位长整数类型
    /// 适用于需要大数值范围的场景，比int类型能表示更大的数值
    /// 对浮点数进行四舍五入处理以避免精度问题
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的长整数值，转换失败时返回0L</returns>
    public static long 转为长整数(this object 值)
    {
        // 空值检查，返回长整数默认值0L
        if (值 == null || 值 == DBNull.Value) return 0L;
        
        // 类型匹配转换
        switch (值)
        {
            // 如果已经是long类型，直接返回
            case long l: return l;
            // int转long，无精度损失
            case int i: return (long)i;
            // float转long，使用Math.Round四舍五入
            case float f: return (long)Math.Round(f);
            // double转long，使用Math.Round四舍五入
            case double d: return (long)Math.Round(d);
            // decimal转long，使用Math.Round四舍五入
            case decimal dec: return (long)Math.Round(dec);
            // 布尔值：true转为1L，false转为0L
            case bool b: return b ? 1L : 0L;
            // 字符串解析
            case string s:
                // 直接尝试解析为长整数
                if (long.TryParse(s, out long 结果))
                    return 结果;
                // 如果失败，尝试解析为浮点数再转换
                if (double.TryParse(s, out double 浮点结果))
                    return (long)Math.Round(浮点结果);
                return 0L;
            // 其他类型通过ToString转换
            default:
                if (long.TryParse(值.ToString(), out long 默认结果))
                    return 默认结果;
                return 0L;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为长整数，转换失败时返回指定默认值
    /// 异常安全的长整数转换方法
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时的默认值</param>
    /// <returns>转换后的长整数或默认值</returns>
    public static long 转为长整数(this object 值, long 默认值)
    {
        try
        {
            // 调用基础转换方法
            return 值.转为长整数();
        }
        catch
        {
            // 异常保护
            return 默认值;
        }
    }
    #endregion
    
    #region 转为小数
    /// <summary>
    /// 将任意对象转换为decimal类型（高精度小数）
    /// decimal类型适用于金融计算等需要精确小数运算的场景
    /// 提供比float和double更高的精度和更好的舍入控制
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <returns>转换后的decimal值，转换失败时返回0m</returns>
    public static decimal 转为小数(this object 值)
    {
        // 空值检查，返回decimal默认值0m
        if (值 == null || 值 == DBNull.Value) return 0m;
        
        // 类型匹配转换
        switch (值)
        {
            // 如果已经是decimal类型，直接返回
            case decimal dec: return dec;
            // float转decimal
            case float f: return (decimal)f;
            // double转decimal
            case double d: return (decimal)d;
            // int转decimal，无精度损失
            case int i: return (decimal)i;
            // long转decimal，无精度损失
            case long l: return (decimal)l;
            // 布尔值：true转为1m，false转为0m
            case bool b: return b ? 1m : 0m;
            // 字符串解析为decimal
            case string s:
                // 使用TryParse安全解析
                if (decimal.TryParse(s, out decimal 结果))
                    return 结果;
                return 0m;
            // 其他类型通过ToString转换
            default:
                if (decimal.TryParse(值.ToString(), out decimal 默认结果))
                    return 默认结果;
                return 0m;
        }
    }
    
    /// <summary>
    /// 将任意对象转换为decimal类型，转换失败时返回指定默认值
    /// 异常安全的decimal转换方法
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="默认值">转换失败时的默认值</param>
    /// <returns>转换后的decimal值或默认值</returns>
    public static decimal 转为小数(this object 值, decimal 默认值)
    {
        try
        {
            // 调用基础转换方法
            return 值.转为小数();
        }
        catch
        {
            // 异常保护
            return 默认值;
        }
    }
    #endregion
    
    #region 安全转换方法
    /// <summary>
    /// 尝试将对象转换为整数，返回转换是否成功
    /// 这是一个安全的转换方法，不会抛出异常
    /// 使用out参数返回转换结果，类似于int.TryParse的模式
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="结果">转换成功时的结果值，失败时为0</param>
    /// <returns>转换是否成功</returns>
    public static bool 尝试转为整数(this object 值, out int 结果)
    {
        // 初始化输出参数为默认值
        结果 = 0;
        try
        {
            // 尝试调用转为整数方法
            结果 = 值.转为整数();
            // 转换成功返回true
            return true;
        }
        catch
        {
            // 任何异常都视为转换失败
            return false;
        }
    }
    
    /// <summary>
    /// 尝试将对象转换为浮点数，返回转换是否成功
    /// 安全的浮点数转换方法，不抛出异常
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="结果">转换成功时的结果值，失败时为0f</param>
    /// <returns>转换是否成功</returns>
    public static bool 尝试转为浮点数(this object 值, out float 结果)
    {
        // 初始化输出参数
        结果 = 0f;
        try
        {
            // 尝试转换
            结果 = 值.转为浮点数();
            return true;
        }
        catch
        {
            // 转换失败
            return false;
        }
    }
    
    /// <summary>
    /// 尝试将对象转换为布尔值，返回转换是否成功
    /// 安全的布尔值转换方法，不抛出异常
    /// </summary>
    /// <param name="值">要转换的对象</param>
    /// <param name="结果">转换成功时的结果值，失败时为false</param>
    /// <returns>转换是否成功</returns>
    public static bool 尝试转为布尔值(this object 值, out bool 结果)
    {
        // 初始化输出参数
        结果 = false;
        try
        {
            // 尝试转换
            结果 = 值.转为布尔值();
            return true;
        }
        catch
        {
            // 转换失败
            return false;
        }
    }
    #endregion

    #region 浮点数转整数扩展方法
    /// <summary>
    /// 自定义四舍五入阈值转换为整数
    /// 当小数部分大于等于指定阈值时进位，否则舍去
    /// </summary>
    /// <param name="值">要转换的浮点数</param>
    /// <param name="阈值">进位阈值，范围0-9，默认为5</param>
    /// <returns>转换后的整数</returns>
    public static int 四舍五入转整数(this float 值, int 阈值 = 5)
    {
        if (阈值 < 0 || 阈值 > 9) 阈值 = 5;
        
        float 阈值比例 = 阈值 / 10.0f;
        bool 是负数 = 值 < 0;
        float 绝对值 = Math.Abs(值);
        
        float 整数部分 = Mathf.Floor(绝对值);
        float 小数部分 = 绝对值 - 整数部分;
        
        if (小数部分 >= 阈值比例)
        {
            整数部分 += 1;
        }
        
        return 是负数 ? -(int)整数部分 : (int)整数部分;
    }
    
    /// <summary>
    /// 自定义四舍五入阈值转换为整数（double版本）
    /// </summary>
    /// <param name="值">要转换的双精度浮点数</param>
    /// <param name="阈值">进位阈值，范围0-9，默认为5</param>
    /// <returns>转换后的整数</returns>
    public static int 四舍五入转整数(this double 值, int 阈值 = 5)
    {
        if (阈值 < 0 || 阈值 > 9) 阈值 = 5;
        
        double 阈值比例 = 阈值 / 10.0;
        bool 是负数 = 值 < 0;
        double 绝对值 = Math.Abs(值);
        
        double 整数部分 = Math.Floor(绝对值);
        double 小数部分 = 绝对值 - 整数部分;
        
        if (小数部分 >= 阈值比例)
        {
            整数部分 += 1;
        }
        
        return 是负数 ? -(int)整数部分 : (int)整数部分;
    }
    
    /// <summary>
    /// 向上取整转换为整数
    /// 任何有小数部分的数都会向上进位
    /// </summary>
    /// <param name="值">要转换的浮点数</param>
    /// <returns>向上取整后的整数</returns>
    public static int 向上取整(this float 值)
    {
        return Mathf.CeilToInt(值);
    }
    
    /// <summary>
    /// 向上取整转换为整数（double版本）
    /// </summary>
    /// <param name="值">要转换的双精度浮点数</param>
    /// <returns>向上取整后的整数</returns>
    public static int 向上取整(this double 值)
    {
        return (int)Math.Ceiling(值);
    }
    
    /// <summary>
    /// 向下取整转换为整数
    /// 直接舍去小数部分
    /// </summary>
    /// <param name="值">要转换的浮点数</param>
    /// <returns>向下取整后的整数</returns>
    public static int 向下取整(this float 值)
    {
        return Mathf.FloorToInt(值);
    }
    
    /// <summary>
    /// 向下取整转换为整数（double版本）
    /// </summary>
    /// <param name="值">要转换的双精度浮点数</param>
    /// <returns>向下取整后的整数</returns>
    public static int 向下取整(this double 值)
    {
        return (int)Math.Floor(值);
    }
    
    /// <summary>
    /// 截断转换为整数
    /// 直接去掉小数部分，等同于强制类型转换
    /// </summary>
    /// <param name="值">要转换的浮点数</param>
    /// <returns>截断后的整数</returns>
    public static int 截断转整数(this float 值)
    {
        return (int)值;
    }
    
    /// <summary>
    /// 截断转换为整数（double版本）
    /// </summary>
    /// <param name="值">要转换的双精度浮点数</param>
    /// <returns>截断后的整数</returns>
    public static int 截断转整数(this double 值)
    {
        return (int)值;
    }
    #endregion
}