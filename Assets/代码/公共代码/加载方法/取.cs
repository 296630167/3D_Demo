using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Xml;
using ExcelDataReader;
using System.Data;

public static class 取
{
    #region 缓存管理
    private static Dictionary<string, UnityEngine.Object> _缓存 = new Dictionary<string, UnityEngine.Object>();
    
    public static void 清缓存()
    {
        _缓存.Clear();
    }
    #endregion

    #region 基础资源加载
    public static T 资源<T>(string 路径) where T : UnityEngine.Object
    {
        if (_缓存.TryGetValue(路径, out UnityEngine.Object 缓存资源))
        {
            return 缓存资源 as T;
        }
        T obj = Resources.Load<T>(路径);
        if (obj != null)
        {
            _缓存[路径] = obj;
        }
        return obj;
    }

    public static async Task<T> 资源Async<T>(string 路径) where T : UnityEngine.Object
    {
        if (_缓存.TryGetValue(路径, out UnityEngine.Object 缓存资源))
        {
            return 缓存资源 as T;
        }
        ResourceRequest request = Resources.LoadAsync<T>(路径);
        while (!request.isDone)
        {
            await Task.Yield();
        }
        if (request.asset != null)
        {
            _缓存[路径] = request.asset;
        }
        return request.asset as T;
    }

    public static async void 资源<T>(string 路径, System.Action<T> 回调) where T : UnityEngine.Object
    {
        T 结果 = await 资源Async<T>(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region Json数据加载
    public static T Json<T>(string 路径)
    {
        TextAsset 文本资源 = 资源<TextAsset>("JSON/" + 路径);
        if (文本资源 != null)
        {
            return JsonConvert.DeserializeObject<T>(文本资源.text);
        }
        return default(T);
    }

    public static async Task<T> JsonAsync<T>(string 路径)
    {
        TextAsset 文本资源 = await 资源Async<TextAsset>("JSON/" + 路径);
        if (文本资源 != null)
        {
            return JsonConvert.DeserializeObject<T>(文本资源.text);
        }
        return default(T);
    }

    public static async void Json<T>(string 路径, System.Action<T> 回调)
    {
        T 结果 = await JsonAsync<T>(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region 游戏对象加载
    public static GameObject 对象(string 路径)
    {
        GameObject 预制体 = 资源<GameObject>("预制体/" + 路径);
        if (预制体 != null)
        {
            return UnityEngine.Object.Instantiate(预制体);
        }
        return null;
    }

    public static GameObject 对象(string 路径, Transform 父对象)
    {
        GameObject 预制体 = 资源<GameObject>("预制体/" + 路径);
        if (预制体 != null)
        {
            return UnityEngine.Object.Instantiate(预制体, 父对象);
        }
        return null;
    }

    public static GameObject 对象(string 路径, Vector3 位置, Quaternion 旋转)
    {
        GameObject 预制体 = 资源<GameObject>("预制体/" + 路径);
        if (预制体 != null)
        {
            return UnityEngine.Object.Instantiate(预制体, 位置, 旋转);
        }
        return null;
    }

    public static async Task<GameObject> 对象Async(string 路径)
    {
        GameObject 预制体 = await 资源Async<GameObject>("预制体/" + 路径);
        if (预制体 != null)
        {
            return UnityEngine.Object.Instantiate(预制体);
        }
        return null;
    }

    public static async Task<GameObject> 对象Async(string 路径, Transform 父对象)
    {
        GameObject 预制体 = await 资源Async<GameObject>("预制体/" + 路径);
        if (预制体 != null)
        {
            return UnityEngine.Object.Instantiate(预制体, 父对象);
        }
        return null;
    }

    public static async void 对象(string 路径, System.Action<GameObject> 回调)
    {
        GameObject 结果 = await 对象Async(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region 图片资源加载
    public static Sprite 图片(string 路径)
    {
        return 资源<Sprite>("图片/" + 路径);
    }

    public static async Task<Sprite> 图片Async(string 路径)
    {
        return await 资源Async<Sprite>("图片/" + 路径);
    }

    public static async void 图片(string 路径, System.Action<Sprite> 回调)
    {
        Sprite 结果 = await 图片Async(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region 音频资源加载
    public static AudioClip 音频(string 路径)
    {
        return 资源<AudioClip>("音频/" + 路径);
    }

    public static async Task<AudioClip> 音频Async(string 路径)
    {
        return await 资源Async<AudioClip>("音频/" + 路径);
    }

    public static async void 音频(string 路径, System.Action<AudioClip> 回调)
    {
        AudioClip 结果 = await 音频Async(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region 材质资源加载
    public static Material 材质(string 路径)
    {
        return 资源<Material>("材质/" + 路径);
    }

    public static async Task<Material> 材质Async(string 路径)
    {
        return await 资源Async<Material>("材质/" + 路径);
    }

    public static async void 材质(string 路径, System.Action<Material> 回调)
    {
        Material 结果 = await 材质Async(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region 字体资源加载
    public static Font 字体(string 路径)
    {
        return 资源<Font>("字体/" + 路径);
    }

    public static async Task<Font> 字体Async(string 路径)
    {
        return await 资源Async<Font>("字体/" + 路径);
    }

    public static async void 字体(string 路径, System.Action<Font> 回调)
    {
        Font 结果 = await 字体Async(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region XML数据加载
    public static XmlDocument Xml(string 路径)
    {
        TextAsset 文本资源 = 资源<TextAsset>(路径);
        if (文本资源 != null)
        {
            XmlDocument xml文档 = new XmlDocument();
            xml文档.LoadXml(文本资源.text);
            return xml文档;
        }
        return null;
    }

    public static async Task<XmlDocument> XmlAsync(string 路径)
    {
        TextAsset 文本资源 = await 资源Async<TextAsset>(路径);
        if (文本资源 != null)
        {
            XmlDocument xml文档 = new XmlDocument();
            xml文档.LoadXml(文本资源.text);
            return xml文档;
        }
        return null;
    }

    public static async void Xml(string 路径, System.Action<XmlDocument> 回调)
    {
        XmlDocument 结果 = await XmlAsync(路径);
        回调?.Invoke(结果);
    }
    #endregion

    #region StreamingAssets数据加载
    public static async Task<byte[]> Streaming字节Async(string 路径)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, 路径);
        using (UnityWebRequest request = UnityWebRequest.Get(fullPath))
        {
            var op = request.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.data;
            }
            else
            {
                Debug.LogError($"加载StreamingAssets字节失败: {fullPath} - {request.error}");
                return null;
            }
        }
    }

    public static async void Streaming字节(string 路径, System.Action<byte[]> 回调)
    {
        byte[] 结果 = await Streaming字节Async(路径);
        回调?.Invoke(结果);
    }

    public static async Task<string> 文本Async(string 路径)
    {
        byte[] 字节数据 = await Streaming字节Async(路径);
        if (字节数据 != null)
        {
            return System.Text.Encoding.UTF8.GetString(字节数据);
        }
        return null;
    }

    public static async void 文本(string 路径, System.Action<string> 回调)
    {
        string 结果 = await 文本Async(路径);
        回调?.Invoke(结果);
    }

    public static async Task<Texture2D> 贴图Async(string 路径)
    {
        byte[] 字节数据 = await Streaming字节Async(路径);
        if (字节数据 != null)
        {
            Texture2D 贴图 = new Texture2D(2, 2);
            if (贴图.LoadImage(字节数据))
            {
                return 贴图;
            }
            else
            {
                Debug.LogError($"从字节数据创建贴图失败: {路径}");
                return null;
            }
        }
        return null;
    }

    public static async void 贴图(string 路径, System.Action<Texture2D> 回调)
    {
        Texture2D 结果 = await 贴图Async(路径);
        回调?.Invoke(结果);
    }

    public static async Task<DataSet> ExcelAsync(string 路径)
    {
        byte[] 字节数据 = await Streaming字节Async(路径);
        if (字节数据 != null)
        {
            try
            {
                using (var 内存流 = new MemoryStream(字节数据))
                {
                    using (var 读取器 = ExcelReaderFactory.CreateReader(内存流))
                    {
                        var 数据集 = 读取器.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = false
                            }
                        });
                        return 数据集;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"解析Excel文件失败: {路径} - {ex.Message}");
                return null;
            }
        }
        return null;
    }

    public static async void Excel(string 路径, System.Action<DataSet> 回调)
    {
        DataSet 结果 = await ExcelAsync(路径);
        回调?.Invoke(结果);
    }

    public static async Task<DataTable> Excel表Async(string 路径, int 表索引 = 0)
    {
        DataSet 数据集 = await ExcelAsync(路径);
        if (数据集 != null && 数据集.Tables.Count > 表索引)
        {
            return 数据集.Tables[表索引];
        }
        return null;
    }

    public static async void Excel表(string 路径, System.Action<DataTable> 回调, int 表索引 = 0)
    {
        DataTable 结果 = await Excel表Async(路径, 表索引);
        回调?.Invoke(结果);
    }
    #endregion
}