using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;

public class TMP字体修复工具 : EditorWindow
{
    [MenuItem("工具/修复TMP字体引用")]
    public static void 显示窗口()
    {
        GetWindow<TMP字体修复工具>("TMP字体修复工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("TMP字体修复工具", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("问题诊断：");
        GUILayout.Label("• 检测到项目中的UI预制体引用了丢失的字体文件");
        GUILayout.Label("• 字体文件通过Git LFS存储但未正确下载");
        GUILayout.Label("• 这导致TMP组件无法正常显示文本");
        
        GUILayout.Space(10);
        GUILayout.Label("修复方案：");
        GUILayout.Label("• 将所有UI预制体的字体引用更新为TMP默认字体");
        GUILayout.Label("• 使用LiberationSans SDF作为替代字体");
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("开始修复TMP字体引用", GUILayout.Height(30)))
        {
            修复TMP字体引用();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("更新TMP Settings默认字体", GUILayout.Height(30)))
        {
            更新TMP设置();
        }
    }

    private void 修复TMP字体引用()
    {
        string[] 预制体路径数组 = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/UI", "Assets/Resources/预制体" });
        TMP_FontAsset 默认字体 = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Plugins/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        
        if (默认字体 == null)
        {
            Debug.LogError("无法找到LiberationSans SDF字体资源！");
            return;
        }
        
        int 修复数量 = 0;
        
        foreach (string guid in 预制体路径数组)
        {
            string 路径 = AssetDatabase.GUIDToAssetPath(guid);
            GameObject 预制体 = AssetDatabase.LoadAssetAtPath<GameObject>(路径);
            
            if (预制体 != null)
            {
                bool 需要保存 = false;
                TextMeshProUGUI[] TMP组件数组 = 预制体.GetComponentsInChildren<TextMeshProUGUI>(true);
                
                foreach (var TMP组件 in TMP组件数组)
                {
                    if (TMP组件.font == null || TMP组件.font.name.Contains("默认字体"))
                    {
                        TMP组件.font = 默认字体;
                        需要保存 = true;
                        修复数量++;
                    }
                }
                
                if (需要保存)
                {
                    EditorUtility.SetDirty(预制体);
                    Debug.Log($"修复预制体: {路径}");
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"TMP字体修复完成！共修复了 {修复数量} 个TMP组件");
        EditorUtility.DisplayDialog("修复完成", $"成功修复了 {修复数量} 个TMP组件的字体引用", "确定");
    }
    
    private void 更新TMP设置()
    {
        TMP_Settings TMP设置 = AssetDatabase.LoadAssetAtPath<TMP_Settings>("Assets/Plugins/TextMesh Pro/Resources/TMP Settings.asset");
        TMP_FontAsset 默认字体 = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Plugins/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        
        if (TMP设置 != null && 默认字体 != null)
        {
            TMP设置.m_defaultFontAsset = 默认字体;
            EditorUtility.SetDirty(TMP设置);
            AssetDatabase.SaveAssets();
            
            Debug.Log("TMP Settings默认字体已更新为LiberationSans SDF");
            EditorUtility.DisplayDialog("更新完成", "TMP Settings默认字体已更新", "确定");
        }
        else
        {
            Debug.LogError("无法找到TMP Settings或默认字体资源！");
        }
    }
}