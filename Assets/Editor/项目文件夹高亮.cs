using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class 文件夹高亮项
{
    public string 路径;
    public Color 背景颜色 = new Color(1f, 1f, 0f, 1f);
    public Color 标题颜色 = Color.black;
    public bool 加粗;
    public bool 应用到子项 = true;
    public Color 子项背景颜色 = new Color(1f, 1f, 0f, 0.25f);
}

[System.Serializable]
public class 文件夹高亮配置
{
    public List<文件夹高亮项> 列表 = new List<文件夹高亮项>();
}

[InitializeOnLoad]
public static class 项目文件夹高亮器
{
    static 文件夹高亮配置 配置 = new 文件夹高亮配置();
    static string 键 = "项目文件夹高亮配置";
    static 项目文件夹高亮器()
    {
        读取配置();
        EditorApplication.projectWindowItemOnGUI += 绘制;
    }
    public static 文件夹高亮配置 获取配置()
    {
        return 配置;
    }
    public static void 读取配置()
    {
        string 文本 = EditorPrefs.GetString(键, "");
        配置 = string.IsNullOrEmpty(文本) ? new 文件夹高亮配置() : JsonUtility.FromJson<文件夹高亮配置>(文本) ?? new 文件夹高亮配置();
    }
    public static void 写入配置()
    {
        string 文本 = JsonUtility.ToJson(配置);
        EditorPrefs.SetString(键, 文本);
        EditorApplication.RepaintProjectWindow();
    }
    static void 绘制(string guid, Rect 区域)
    {
        string 资源路径 = AssetDatabase.GUIDToAssetPath(guid);
        文件夹高亮项 直接项 = 查找项(资源路径);
        if (直接项 != null && AssetDatabase.IsValidFolder(资源路径))
        {
            EditorGUI.DrawRect(区域, 直接项.背景颜色);
            GUIStyle 样式 = new GUIStyle(EditorStyles.label);
            样式.normal.textColor = 直接项.标题颜色;
            样式.fontStyle = 直接项.加粗 ? FontStyle.Bold : FontStyle.Normal;
            Rect 文本区域 = 区域;
            文本区域.xMin += 20f;
            string 名称 = Path.GetFileName(资源路径);
            GUI.Label(文本区域, 名称, 样式);
            return;
        }
        文件夹高亮项 父项 = 查找父项(资源路径);
        if (父项 != null && 父项.应用到子项)
        {
            EditorGUI.DrawRect(区域, 父项.子项背景颜色);
        }
    }
    static 文件夹高亮项 查找项(string 路径)
    {
        if (配置 == null || 配置.列表 == null) return null;
        for (int i = 0; i < 配置.列表.Count; i++)
        {
            文件夹高亮项 项 = 配置.列表[i];
            if (项 != null && 项.路径 == 路径) return 项;
        }
        return null;
    }
    static 文件夹高亮项 查找父项(string 路径)
    {
        if (配置 == null || 配置.列表 == null) return null;
        for (int i = 0; i < 配置.列表.Count; i++)
        {
            文件夹高亮项 项 = 配置.列表[i];
            if (项 != null && !string.IsNullOrEmpty(项.路径))
            {
                string 前缀 = 项.路径.EndsWith("/") ? 项.路径 : 项.路径 + "/";
                if (路径.Length > 项.路径.Length && 路径.StartsWith(前缀)) return 项;
            }
        }
        return null;
    }
}

public class 项目文件夹高亮配置窗口 : EditorWindow
{
    Vector2 滚动位置;
    [MenuItem("工具/项目文件夹高亮配置")]
    public static void 打开()
    {
        var 窗口 = GetWindow<项目文件夹高亮配置窗口>(true, "文件夹高亮配置", true);
        窗口.minSize = new Vector2(520f, 320f);
        窗口.Show();
    }
    void OnGUI()
    {
        文件夹高亮配置 配置 = 项目文件夹高亮器.获取配置();
        if (配置.列表 == null) 配置.列表 = new List<文件夹高亮项>();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加")) 配置.列表.Add(new 文件夹高亮项());
        if (GUILayout.Button("保存全部")) 项目文件夹高亮器.写入配置();
        EditorGUILayout.EndHorizontal();
        滚动位置 = EditorGUILayout.BeginScrollView(滚动位置);
        for (int i = 0; i < 配置.列表.Count; i++)
        {
            文件夹高亮项 项 = 配置.列表[i];
            EditorGUILayout.BeginVertical("box");
            DefaultAsset 目标 = null;
            if (!string.IsNullOrEmpty(项.路径)) 目标 = AssetDatabase.LoadAssetAtPath<DefaultAsset>(项.路径);
            Object 新目标 = EditorGUILayout.ObjectField("文件夹", 目标, typeof(DefaultAsset), false);
        if (新目标 != null)
        {
            string p = AssetDatabase.GetAssetPath(新目标);
            if (AssetDatabase.IsValidFolder(p)) 项.路径 = p;
        }
        项.背景颜色 = EditorGUILayout.ColorField("背景颜色", 项.背景颜色);
        项.标题颜色 = EditorGUILayout.ColorField("标题颜色", 项.标题颜色);
        项.加粗 = EditorGUILayout.Toggle("加粗", 项.加粗);
        项.应用到子项 = EditorGUILayout.Toggle("应用到子项", 项.应用到子项);
        项.子项背景颜色 = EditorGUILayout.ColorField("子项背景颜色", 项.子项背景颜色);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("应用")) 项目文件夹高亮器.写入配置();
        if (GUILayout.Button("删除")) { 配置.列表.RemoveAt(i); i--; 项目文件夹高亮器.写入配置(); EditorGUILayout.EndHorizontal(); EditorGUILayout.EndVertical(); continue; }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
    }
}

