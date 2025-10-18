using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class JSON扩展
{
    private static readonly JsonSerializerSettings 序列化设置 = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };

    public static void 保存到JSON<T>(this T 对象, string 文件路径)
    {
        try
        {
            string json内容 = JsonConvert.SerializeObject(对象, 序列化设置);
            File.WriteAllText(文件路径, json内容);
            Debug.Log($"数据已保存到: {文件路径}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"保存JSON文件失败: {ex.Message}");
        }
    }

    public static T 从JSON读取<T>(string 文件路径) where T : class, new()
    {
        try
        {
            if (!File.Exists(文件路径))
            {
                Debug.LogWarning($"文件不存在: {文件路径}，返回默认对象");
                return new T();
            }

            string json内容 = File.ReadAllText(文件路径);
            if (string.IsNullOrEmpty(json内容))
            {
                Debug.LogWarning($"文件内容为空: {文件路径}，返回默认对象");
                return new T();
            }

            T 结果 = JsonConvert.DeserializeObject<T>(json内容, 序列化设置);
            Debug.Log($"成功从文件读取数据: {文件路径}");
            return 结果 ?? new T();
        }
        catch (Exception ex)
        {
            Debug.LogError($"读取JSON文件失败: {ex.Message}，返回默认对象");
            return new T();
        }
    }

    public static T 从Resources读取JSON<T>(string 资源路径) where T : class, new()
    {
        try
        {
            var 文本资源 = Resources.Load<TextAsset>(资源路径);
            if (文本资源 == null)
            {
                Debug.LogWarning($"Resources中未找到文件: {资源路径}，返回默认对象");
                return new T();
            }

            T 结果 = JsonConvert.DeserializeObject<T>(文本资源.text, 序列化设置);
            Debug.Log($"成功从Resources读取数据: {资源路径}");
            return 结果 ?? new T();
        }
        catch (Exception ex)
        {
            Debug.LogError($"从Resources读取JSON失败: {ex.Message}，返回默认对象");
            return new T();
        }
    }

    public static string 转换为JSON<T>(this T 对象)
    {
        try
        {
            return JsonConvert.SerializeObject(对象, 序列化设置);
        }
        catch (Exception ex)
        {
            Debug.LogError($"对象转换为JSON失败: {ex.Message}");
            return string.Empty;
        }
    }

    public static T 从JSON转换<T>(this string json字符串) where T : class, new()
    {
        try
        {
            if (string.IsNullOrEmpty(json字符串))
            {
                return new T();
            }

            T 结果 = JsonConvert.DeserializeObject<T>(json字符串, 序列化设置);
            return 结果 ?? new T();
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON字符串转换失败: {ex.Message}");
            return new T();
        }
    }
}