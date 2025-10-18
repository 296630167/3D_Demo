using UnityEngine;
using UnityEditor;
using System.IO;

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

    private void OnEnable()
    {
        技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>(技能数据路径);

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
            
            // 水平布局：左侧导航 + 右侧详情
            EditorGUILayout.BeginHorizontal();
            {
                // 左侧技能导航
                绘制左侧技能导航();
                
                // 右侧技能详情
                绘制右侧技能详情();
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("技能数据未加载", EditorStyles.helpBox);
            
            if (GUILayout.Button("尝试重新加载"))
            {
                OnEnable();
            }
        }
    }

    private void 绘制左侧技能导航()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        {
            GUILayout.Label("技能列表", EditorStyles.boldLabel);
            
            // 添加新技能按钮
            if (GUILayout.Button("添加新技能", GUILayout.Height(30)))
            {
                添加新技能();
            }
            
            GUILayout.Space(10);
            
            // 技能列表滚动区域
            技能列表滚动位置 = EditorGUILayout.BeginScrollView(技能列表滚动位置, GUILayout.Height(800));
            {
                if (技能数据.技能列表 != null)
                {
                    for (int i = 0; i < 技能数据.技能列表.Count; i++)
                    {
                        var 技能 = 技能数据.技能列表[i];
                        if (技能 == null) continue;

                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            // 技能信息显示
                            EditorGUILayout.BeginVertical();
                            {
                                string 显示名称 = string.IsNullOrEmpty(技能.名称) ? $"技能_{技能.id}" : 技能.名称;
                                
                                // 技能选择按钮 - 增大可点击区域
                                bool 是否选中 = 当前选中技能 == 技能;
                                GUI.backgroundColor = 是否选中 ? Color.cyan : Color.white;
                                
                                // 创建更大的按钮样式
                                GUIStyle 技能按钮样式 = new GUIStyle(GUI.skin.button);
                                技能按钮样式.alignment = TextAnchor.MiddleLeft;
                                技能按钮样式.fontSize = 14;
                                技能按钮样式.fontStyle = FontStyle.Bold;
                                
                                if (GUILayout.Button(显示名称, 技能按钮样式, GUILayout.Height(35), GUILayout.ExpandWidth(true)))
                                {
                                    当前选中技能 = 技能;
                                }
                                
                                GUI.backgroundColor = Color.white;
                                
                                // 显示技能基本信息
                                EditorGUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label($"ID: {技能.id}", EditorStyles.miniLabel, GUILayout.Width(50));
                                    GUILayout.Label($"类型: {技能.施法类型}", EditorStyles.miniLabel);
                                }
                                EditorGUILayout.EndHorizontal();
                                
                                if (!string.IsNullOrEmpty(技能.介绍))
                                {
                                    GUILayout.Label($"介绍: {技能.介绍}", EditorStyles.miniLabel);
                                }
                            }
                            EditorGUILayout.EndVertical();
                            
                            // 删除按钮 - 也增大一些
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
                    // 基本信息
                    GUILayout.Label("基本信息", EditorStyles.boldLabel);
                    当前选中技能.名称 = EditorGUILayout.TextField("技能名称", 当前选中技能.名称);
                    当前选中技能.介绍 = EditorGUILayout.TextArea(当前选中技能.介绍, GUILayout.Height(60));
                    
                    GUILayout.Space(10);
                    
                    // 施法属性
                    GUILayout.Label("施法属性", EditorStyles.boldLabel);
                    当前选中技能.施法类型 = (技能施法类型)EditorGUILayout.EnumPopup("施法类型", 当前选中技能.施法类型);
                    当前选中技能.施法次数 = EditorGUILayout.IntField("施法次数", 当前选中技能.施法次数);
                    当前选中技能.施法对象 = (技能施法对象)EditorGUILayout.EnumPopup("施法对象", 当前选中技能.施法对象);
                    当前选中技能.消耗魔法 = EditorGUILayout.IntField("消耗魔法", 当前选中技能.消耗魔法);
                    当前选中技能.射程 = EditorGUILayout.FloatField("射程", 当前选中技能.射程);
                    当前选中技能.命中 = EditorGUILayout.FloatField("命中", 当前选中技能.命中);
                    当前选中技能.消耗行动力 = EditorGUILayout.IntField("消耗行动力", 当前选中技能.消耗行动力);
                    
                    GUILayout.Space(10);
                    
                    // 效果属性
                    GUILayout.Label("效果属性", EditorStyles.boldLabel);
                    当前选中技能.施加效果 = (技能施加效果)EditorGUILayout.EnumPopup("施加效果", 当前选中技能.施加效果);
                    当前选中技能.基础伤害 = EditorGUILayout.IntField("基础伤害", 当前选中技能.基础伤害);
                    当前选中技能.技能伤害倍率 = EditorGUILayout.FloatField("技能伤害倍率", 当前选中技能.技能伤害倍率);
                    当前选中技能.伤害类型 = (技能伤害类型)EditorGUILayout.EnumPopup("伤害类型", 当前选中技能.伤害类型);
                    当前选中技能.伤害计算方式 = (技能伤害计算方式)EditorGUILayout.EnumPopup("伤害计算方式", 当前选中技能.伤害计算方式);
                    当前选中技能.冷却时间 = EditorGUILayout.IntField("冷却时间", 当前选中技能.冷却时间);
                    当前选中技能.作用范围 = EditorGUILayout.FloatField("作用范围", 当前选中技能.作用范围);
                    
                    GUILayout.Space(10);
                    
                    // 图标 - 优化的图标选择界面
                    GUILayout.Label("视觉效果", EditorStyles.boldLabel);
                    
                    // 图标选择区域 - 靠左布局
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(120));
                        {
                            GUILayout.Label("技能图标", EditorStyles.boldLabel);
                            
                            // 图标预览区域
                            var previewRect = GUILayoutUtility.GetRect(80, 80, GUILayout.Width(80), GUILayout.Height(80));
                            
                            // 绘制边框
                            EditorGUI.DrawRect(previewRect, new Color(0.3f, 0.3f, 0.3f, 1f));
                            var innerRect = new Rect(previewRect.x + 1, previewRect.y + 1, previewRect.width - 2, previewRect.height - 2);
                            
                            if (当前选中技能.图标 != null)
                            {
                                // 显示图标
                                GUI.DrawTexture(innerRect, 当前选中技能.图标.texture, ScaleMode.ScaleToFit);
                            }
                            else
                            {
                                // 显示占位符
                                EditorGUI.DrawRect(innerRect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                                var labelRect = new Rect(innerRect.x, innerRect.y + innerRect.height/2 - 10, innerRect.width, 20);
                                GUI.Label(labelRect, "无图标", EditorStyles.centeredGreyMiniLabel);
                            }
                            
                            GUILayout.Space(5);
                            
                            // 图标选择字段
                            当前选中技能.图标 = (Sprite)EditorGUILayout.ObjectField(当前选中技能.图标, typeof(Sprite), false, GUILayout.Width(80));
                        }
                        EditorGUILayout.EndVertical();
                        
                        GUILayout.FlexibleSpace(); // 推送剩余空间到右侧
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    GUILayout.Space(20);
                    
                    // 保存按钮
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
            
            // 确认删除对话框
            if (EditorUtility.DisplayDialog("确认删除", $"确定要删除技能 '{技能名称}' 吗？\n此操作无法撤销。", "删除", "取消"))
            {
                // 如果删除的是当前选中的技能，清空选择
                if (当前选中技能 == 要删除的技能)
                {
                    当前选中技能 = null;
                }
                
                // 从列表中移除技能
                技能数据.技能列表.RemoveAt(索引);
                
                // 重新排列所有技能的ID
                重新排列技能ID();
                
                // 标记数据已修改
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