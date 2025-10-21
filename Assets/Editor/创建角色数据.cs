using UnityEngine;
using UnityEditor;
using System.IO;

public class 创建角色数据
{
    [MenuItem("工具/重新创建角色数据")]
    public static void 重新创建角色数据文件()
    {
        // 创建新的角色管理实例
        角色管理 新角色数据 = ScriptableObject.CreateInstance<角色管理>();
        
        // 设置保存路径
        string 保存路径 = "Assets/Resources/数据/角色数据.asset";
        
        // 确保目录存在
        string 目录路径 = Path.GetDirectoryName(保存路径);
        if (!Directory.Exists(目录路径))
        {
            Directory.CreateDirectory(目录路径);
        }
        
        // 创建asset文件
        AssetDatabase.CreateAsset(新角色数据, 保存路径);
        
        // 刷新资源数据库
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // 选中新创建的文件
        Selection.activeObject = 新角色数据;
        
        Debug.Log("角色数据.asset 文件已重新创建完