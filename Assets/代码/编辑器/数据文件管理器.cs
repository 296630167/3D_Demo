using UnityEngine;
using UnityEditor;
using System.IO;

public class 数据文件管理器 : EditorWindow
{
    private Vector2 滚动位置;
    private bool 显示详细信息 = false;
    
    [MenuItem("工具/数据文件管理器")]
    public static void 显示窗口()
    {
        GetWindow<数据文件管理器>("数据文件管理器");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Unity数据文件管理器", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        滚动位置 = EditorGUILayout.BeginScrollView(滚动位置);
        
        显示详细信息 = EditorGUILayout.Toggle("显示详细信息", 显示详细信息);
        GUILayout.Space(10);
        
        绘制数据文件状态();
        GUILayout.Space(20);
        
        绘制管理按钮();
        
        EditorGUILayout.EndScrollView();
    }
    
    private void 绘制数据文件状态()
    {
        GUILayout.Label("数据文件状态检查", EditorStyles.boldLabel);
        
        检查并显示文件状态("角色数据", "Assets/Resources/数据/角色数据.asset", typeof(角色管理));
        检查并显示文件状态("技能数据", "Assets/Resources/数据/技能数据.asset", typeof(技能管理));
        检查并显示文件状态("技能树数据", "Assets/Resources/数据/技能树数据.asset", typeof(技能树管理));
        检查并显示文件状态("道具数据", "Assets/Resources/数据/道具数据.asset", typeof(道具管理));
    }
    
    private void 检查并显示文件状态(string 文件名, string 文件路径, System.Type 脚本类型)
    {
        EditorGUILayout.BeginHorizontal();
        
        bool 文件存在 = File.Exists(文件路径);
        bool 是LFS指针 = false;
        bool 脚本引用正确 = false;
        
        if (文件存在)
        {
            string 文件内容 = File.ReadAllText(文件路径);
            是LFS指针 = 文件内容.Contains("version https://git-lfs.github.com/spec/v1");
            
            if (!是LFS指针)
            {
                var 资源对象 = AssetDatabase.LoadAssetAtPath<ScriptableObject>(文件路径);
                脚本引用正确 = 资源对象 != null && 资源对象.GetType() == 脚本类型;
            }
        }
        
        Color 原始颜色 = GUI.color;
        if (!文件存在)
            GUI.color = Color.red;
        else if (是LFS指针)
            GUI.color = Color.yellow;
        else if (!脚本引用正确)
            GUI.color = Color.orange;
        else
            GUI.color = Color.green;
            
        GUILayout.Label($"{文件名}:", GUILayout.Width(80));
        
        string 状态文本;
        if (!文件存在)
            状态文本 = "文件不存在";
        else if (是LFS指针)
            状态文本 = "Git LFS指针文件";
        else if (!脚本引用正确)
            状态文本 = "脚本引用错误";
        else
            状态文本 = "正常";
            
        GUILayout.Label(状态文本, GUILayout.Width(120));
        
        GUI.color = 原始颜色;
        
        if (显示详细信息)
        {
            GUILayout.Label($"路径: {文件路径}", GUILayout.Width(300));
        }
        
        if (GUILayout.Button("修复", GUILayout.Width(60)))
        {
            修复单个文件(文件名, 文件路径, 脚本类型);
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void 绘制管理按钮()
    {
        GUILayout.Label("批量操作", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("重新创建所有数据文件", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("确认操作", "这将重新创建所有数据文件，现有数据将丢失。是否继续？", "确认", "取消"))
            {
                重新创建所有数据文件();
            }
        }
        
        if (GUILayout.Button("备份所有数据文件", GUILayout.Height(30)))
        {
            备份所有数据文件();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("检查Git LFS状态", GUILayout.Height(30)))
        {
            检查GitLFS状态();
        }
        
        if (GUILayout.Button("刷新状态", GUILayout.Height(30)))
        {
            Repaint();
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void 修复单个文件(string 文件名, string 文件路径, System.Type 脚本类型)
    {
        try
        {
            string 备份路径 = 文件路径.Replace(".asset", "_备份.asset");
            
            if (File.Exists(文件路径))
            {
                File.Copy(文件路径, 备份路径, true);
                Debug.Log($"已备份 {文件名} 到 {备份路径}");
            }
            
            if (File.Exists(文件路径))
            {
                File.Delete(文件路径);
            }
            
            var 新资源 = ScriptableObject.CreateInstance(脚本类型);
            新资源.name = 文件名;
            
            string 目录路径 = Path.GetDirectoryName(文件路径);
            if (!Directory.Exists(目录路径))
            {
                Directory.CreateDirectory(目录路径);
            }
            
            AssetDatabase.CreateAsset(新资源, 文件路径);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"成功修复 {文件名}");
            EditorUtility.DisplayDialog("修复完成", $"{文件名} 已成功修复", "确定");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"修复 {文件名} 时出错: {ex.Message}");
            EditorUtility.DisplayDialog("修复失败", $"修复 {文件名} 时出错: {ex.Message}", "确定");
        }
    }
    
    private void 重新创建所有数据文件()
    {
        try
        {
            备份所有数据文件();
            
            创建数据文件("角色数据", "Assets/Resources/数据/角色数据.asset", typeof(角色管理));
            创建数据文件("技能数据", "Assets/Resources/数据/技能数据.asset", typeof(技能管理));
            创建数据文件("技能树数据", "Assets/Resources/数据/技能树数据.asset", typeof(技能树管理));
            创建数据文件("道具数据", "Assets/Resources/数据/道具数据.asset", typeof(道具管理));
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("所有数据文件重新创建完成");
            EditorUtility.DisplayDialog("操作完成", "所有数据文件已重新创建", "确定");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"重新创建数据文件时出错: {ex.Message}");
            EditorUtility.DisplayDialog("操作失败", $"重新创建数据文件时出错: {ex.Message}", "确定");
        }
    }
    
    private void 创建数据文件(string 文件名, string 文件路径, System.Type 脚本类型)
    {
        if (File.Exists(文件路径))
        {
            File.Delete(文件路径);
        }
        
        var 新资源 = ScriptableObject.CreateInstance(脚本类型);
        新资源.name = 文件名;
        
        string 目录路径 = Path.GetDirectoryName(文件路径);
        if (!Directory.Exists(目录路径))
        {
            Directory.CreateDirectory(目录路径);
        }
        
        AssetDatabase.CreateAsset(新资源, 文件路径);
        Debug.Log($"创建 {文件名}: {文件路径}");
    }
    
    private void 备份所有数据文件()
    {
        string[] 数据文件路径 = {
            "Assets/Resources/数据/角色数据.asset",
            "Assets/Resources/数据/技能数据.asset", 
            "Assets/Resources/数据/技能树数据.asset",
            "Assets/Resources/数据/道具数据.asset"
        };
        
        foreach (string 文件路径 in 数据文件路径)
        {
            if (File.Exists(文件路径))
            {
                string 备份路径 = 文件路径.Replace(".asset", "_备份.asset");
                File.Copy(文件路径, 备份路径, true);
                Debug.Log($"备份文件: {文件路径} -> {备份路径}");
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("所有数据文件备份完成");
    }
    
    private void 检查GitLFS状态()
    {
        string[] 数据文件路径 = {
            "Assets/Resources/数据/角色数据.asset",
            "Assets/Resources/数据/技能数据.asset",
            "Assets/Resources/数据/技能树数据.asset", 
            "Assets/Resources/数据/道具数据.asset"
        };
        
        Debug.Log("=== Git LFS 状态检查 ===");
        
        foreach (string 文件路径 in 数据文件路径)
        {
            if (File.Exists(文件路径))
            {
                string 文件内容 = File.ReadAllText(文件路径);
                bool 是LFS指针 = 文件内容.Contains("version https://git-lfs.github.com/spec/v1");
                
                FileInfo 文件信息 = new FileInfo(文件路径);
                
                Debug.Log($"{文件路径}:");
                Debug.Log($"  大小: {文件信息.Length} 字节");
                Debug.Log($"  是否为LFS指针: {(是LFS指针 ? "是" : "否")}");
                Debug.Log($"  状态: {(是LFS指针 ? "需要修复" : "正常")}");
            }
            else
            {
                Debug.Log($"{文件路径}: 文件不存在");
            }
        }
        
        Debug.Log("=== 检查完成 ===");
    }
}