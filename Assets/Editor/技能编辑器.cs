using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class 技能编辑器 : EditorWindow
{
    [MenuItem("工具/编辑器/技能编辑器")]
    public static void 打开窗口()
    {
        var 窗口 = GetWindow<技能编辑器>("技能编辑器");
        窗口.Show();
    }
    private 技能管理 技能数据;
    private const string 技能数据路径 = "Assets/Resources/数据/技能数据.asset";
    private 技能类 当前选中技能;
    private Vector2 技能列表滚动位置;
    private Sprite 临时预览图标;
    private Dictionary<int, Sprite> 技能图标预览缓存 = new Dictionary<int, Sprite>();

    private void OnEnable()
    {
        技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>(技能数据路径);
        技能图标预览缓存.Clear();

        if (技能数据 == null)
        {
            Debug.LogWarning($"无法在路径 {技能数据路径} 找到技能数据文件");
        }
        else
        {
            Debug.Log($"成功加载技能数据: {技能数据路径}");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("技能编辑器", EditorStyles.boldLabel);
        
        if (技能数据 != null)
        {
            GUILayout.Label($"成功加载技能数据，共 {技能数据.技能列表.Count} 个技能");
            
            EditorGUILayout.BeginHorizontal();
            {
                绘制左侧技能导航();
                绘制右侧技能详情();
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("技能数据未加载", EditorStyles.helpBox);
            
            if (GUILayout.Button("尝试重新加载"))
            {
                技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>(技能数据路径);
            }
        }
    }

    private void 绘制左侧技能导航()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(260));
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("添加技能", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    添加新技能();
                }
                
                if (GUILayout.Button("保存", GUILayout.Width(80), GUILayout.Height(30)))
                {
                    保存技能数据();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            
            技能列表滚动位置 = EditorGUILayout.BeginScrollView(技能列表滚动位置);
            {
                if (技能数据 != null && 技能数据.技能列表 != null)
                {
                    for (int i = 0; i < 技能数据.技能列表.Count; i++)
                    {
                        var 技能 = 技能数据.技能列表[i];
                        if (技能 == null) continue;
                        
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            if (GUILayout.Button($"[{技能.id}] {技能.名称}", GUILayout.Height(40)))
                            {
                                当前选中技能 = 技能;
                                临时预览图标 = null;
                                Repaint();
                            }
                            
                            if (GUILayout.Button("删除", GUILayout.Width(60), GUILayout.Height(40)))
                            {
                                删除技能(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(2);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void 绘制右侧技能详情()
    {
        EditorGUILayout.BeginVertical();
        {
            if (当前选中技能 != null)
            {
                GUILayout.Label($"编辑技能: {当前选中技能.名称}", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label("基本信息", EditorStyles.boldLabel);
                    当前选中技能.名称 = EditorGUILayout.TextField("技能名称", 当前选中技能.名称);
                    当前选中技能.介绍 = EditorGUILayout.TextArea(当前选中技能.介绍, GUILayout.Height(60));
                    
                    GUILayout.Space(10);
                    
                    GUILayout.Label("施法属性", EditorStyles.boldLabel);
                    当前选中技能.施法类型 = (技能施法类型)EditorGUILayout.EnumPopup("施法类型", 当前选中技能.施法类型);
                    当前选中技能.施法次数 = EditorGUILayout.IntField("施法次数", 当前选中技能.施法次数);
                    当前选中技能.施法对象 = (技能施法对象)EditorGUILayout.EnumPopup("施法对象", 当前选中技能.施法对象);
                    当前选中技能.消耗魔法 = EditorGUILayout.IntField("消耗魔法", 当前选中技能.消耗魔法);
                    当前选中技能.射程 = EditorGUILayout.FloatField("射程", 当前选中技能.射程);
                    当前选中技能.命中 = EditorGUILayout.FloatField("命中", 当前选中技能.命中);
                    当前选中技能.消耗行动力 = EditorGUILayout.IntField("消耗行动力", 当前选中技能.消耗行动力);
                    
                    GUILayout.Space(10);
                    
                    GUILayout.Label("效果属性", EditorStyles.boldLabel);
                    当前选中技能.施加效果 = (技能施加效果)EditorGUILayout.EnumPopup("施加效果", 当前选中技能.施加效果);
                    当前选中技能.基础伤害 = EditorGUILayout.IntField("基础伤害", 当前选中技能.基础伤害);
                    当前选中技能.技能伤害倍率 = EditorGUILayout.FloatField("技能伤害倍率", 当前选中技能.技能伤害倍率);
                    当前选中技能.伤害类型 = (技能伤害类型)EditorGUILayout.EnumPopup("伤害类型", 当前选中技能.伤害类型);
                    当前选中技能.伤害计算方式 = (技能伤害计算方式)EditorGUILayout.EnumPopup("伤害计算方式", 当前选中技能.伤害计算方式);
                    当前选中技能.冷却时间 = EditorGUILayout.IntField("冷却时间", 当前选中技能.冷却时间);
                    当前选中技能.作用范围 = EditorGUILayout.FloatField("作用范围", 当前选中技能.作用范围);
                    
                    GUILayout.Space(10);
                    
                    GUILayout.Label("视觉效果", EditorStyles.boldLabel);
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(120));
                        {
                            GUILayout.Label("技能图标", EditorStyles.boldLabel);
                            var 路径文本 = string.IsNullOrEmpty(当前选中技能.图标路径) ? "未设置" : 当前选中技能.图标路径;
                            GUILayout.Label($"图标路径: {路径文本}", EditorStyles.miniLabel);
                            var 预览区域 = GUILayoutUtility.GetRect(80, 80, GUILayout.Width(80), GUILayout.Height(80));
                            EditorGUI.DrawRect(预览区域, new Color(0.3f, 0.3f, 0.3f, 1f));
                            var 内部区域 = new Rect(预览区域.x + 1, 预览区域.y + 1, 预览区域.width - 2, 预览区域.height - 2);
                            
                            Sprite 显示图标 = null;
                            if (!string.IsNullOrEmpty(当前选中技能.图标路径)) 显示图标 = 取.图片(当前选中技能.图标路径);
                            if (显示图标 == null && 临时预览图标 != null) 显示图标 = 临时预览图标;
                            if (显示图标 == null) 技能图标预览缓存.TryGetValue(当前选中技能.id, out 显示图标);
                            
                            Texture 预览纹理 = null;
                            if (显示图标 != null)
                            {
                                if (显示图标.texture != null) 预览纹理 = 显示图标.texture;
                                else 预览纹理 = AssetPreview.GetAssetPreview(显示图标) ?? AssetPreview.GetMiniThumbnail(显示图标);
                            }
                            
                            if (预览纹理 != null)
                            {
                                GUI.DrawTexture(内部区域, 预览纹理, ScaleMode.ScaleToFit);
                            }
                            else
                            {
                                EditorGUI.DrawRect(内部区域, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                                var 文本区域 = new Rect(内部区域.x, 内部区域.y + 内部区域.height/2 - 10, 内部区域.width, 20);
                                GUI.Label(文本区域, "无图标", EditorStyles.centeredGreyMiniLabel);
                            }
                            
                            GUILayout.Space(5);
                            var 选择的图标 = (Sprite)EditorGUILayout.ObjectField(显示图标, typeof(Sprite), false, GUILayout.Width(80));
                            if (选择的图标 != null)
                            {
                                var 资源路径 = AssetDatabase.GetAssetPath(选择的图标);
                                if (!string.IsNullOrEmpty(资源路径) && 资源路径.StartsWith("Assets/Resources/"))
                                {
                                    var 相对路径 = 资源路径.Substring("Assets/Resources/".Length);
                                    var 无扩展 = Path.ChangeExtension(相对路径, null);
                                    if (无扩展.StartsWith("图片/"))
                                    {
                                        当前选中技能.图标路径 = 无扩展.Substring("图片/".Length);
                                    }
                                    else
                                    {
                                        当前选中技能.图标路径 = 无扩展;
                                    }
                                }
                                else
                                {
                                    当前选中技能.图标路径 = $"技能图标/{选择的图标.name}";
                                }
                                临时预览图标 = 选择的图标;
                                技能图标预览缓存[当前选中技能.id] = 选择的图标;
                                EditorUtility.SetDirty(技能数据);
                                Repaint();
                            }
                        }
                        EditorGUILayout.EndVertical();
                        
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    GUILayout.Space(20);
                    
                    if (GUILayout.Button("保存技能数据", GUILayout.Height(30)))
                    {
                        保存技能数据();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                GUILayout.Label("请从左侧选择一个技能进行编辑", EditorStyles.helpBox);
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void 添加新技能()
    {
        if (技能数据 != null)
        {
            var 新技能 = new 技能类();
            新技能.名称 = $"新技能_{技能数据.技能列表.Count + 1}";
            新技能.id = 技能数据.技能列表.Count + 1;
            
            技能数据.技能列表.Add(新技能);
            当前选中技能 = 新技能;
            临时预览图标 = null;
            
            EditorUtility.SetDirty(技能数据);
            
            Debug.Log($"添加了新技能: {新技能.名称}");
        }
    }

    private void 删除技能(int 索引)
    {
        if (技能数据 != null && 索引 >= 0 && 索引 < 技能数据.技能列表.Count)
        {
            var 要删除的技能 = 技能数据.技能列表[索引];
            string 技能名称 = string.IsNullOrEmpty(要删除的技能.名称) ? $"技能_{索引 + 1}" : 要删除的技能.名称;
            
            if (EditorUtility.DisplayDialog("确认删除", $"确定要删除技能 '{技能名称}' 吗？\n此操作无法撤销。", "删除", "取消"))
            {
                if (当前选中技能 == 要删除的技能)
                {
                    当前选中技能 = null;
                    临时预览图标 = null;
                }
                
                技能数据.技能列表.RemoveAt(索引);
                技能图标预览缓存.Remove(要删除的技能.id);
                重新排列技能ID();
                EditorUtility.SetDirty(技能数据);
                
                Debug.Log($"删除了技能: {技能名称}");
            }
        }
    }
    
    private void 重新排列技能ID()
    {
        if (技能数据 != null && 技能数据.技能列表 != null)
        {
            for (int i = 0; i < 技能数据.技能列表.Count; i++)
            {
                if (技能数据.技能列表[i] != null)
                {
                    技能数据.技能列表[i].id = i + 1;
                }
            }
        }
    }

    private void 保存技能数据()
    {
        if (技能数据 != null)
        {
            EditorUtility.SetDirty(技能数据);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("技能数据已保存");
        }
    }
}