using UnityEngine;
using UnityEditor;
using System.IO;

public class 角色编辑器 : EditorWindow
{
    [MenuItem("工具/编辑器/角色编辑器")]
    public static void 打开窗口()
    {
        var 窗口 = GetWindow<角色编辑器>("角色编辑器");
        窗口.Show();
    }

    private 角色管理 角色数据;
    private const string 角色数据路径 = "Assets/Resources/数据/角色数据.asset";
    private 角色类 当前选中角色;
    private Vector2 角色列表滚动位置;

    private void OnEnable()
    {
        角色数据 = AssetDatabase.LoadAssetAtPath<角色管理>(角色数据路径);

        if (角色数据 == null)
        {
            Debug.LogWarning($"无法在路径 {角色数据路径} 找到角色数据文件");
        }
        else
        {
            Debug.Log($"成功加载角色数据: {角色数据路径}");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("角色编辑器", EditorStyles.boldLabel);
        
        if (角色数据 != null)
        {
            GUILayout.Label($"成功加载角色数据，共 {角色数据.角色列表.Count} 个角色");
            
            // 水平布局：左侧导航 + 右侧详情
            EditorGUILayout.BeginHorizontal();
            {
                // 左侧角色导航
                绘制左侧角色导航();
                
                // 右侧角色详情
                绘制右侧角色详情();
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("未能加载角色数据");
        }
    }
    
    private void 绘制左侧角色导航()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        {
            // 标题和添加按钮
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("角色列表", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("添加新角色", GUILayout.Width(100), GUILayout.Height(25)))
                {
                    添加新角色();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            
            // 滚动视图包装角色列表
            角色列表滚动位置 = EditorGUILayout.BeginScrollView(角色列表滚动位置);
            {
                if (角色数据.角色列表 != null && 角色数据.角色列表.Count > 0)
                {
                    for (int i = 0; i < 角色数据.角色列表.Count; i++)
                    {
                        var 角色 = 角色数据.角色列表[i];
                        string 基础名称 = string.IsNullOrEmpty(角色.名称) ? $"角色_{i + 1}" : 角色.名称;
                        string 显示名称 = $"[{角色.id}] {基础名称}";
                        
                        // 角色按钮和删除按钮的水平布局
                        EditorGUILayout.BeginHorizontal();
                        {
                            // 角色选择按钮
                            if (GUILayout.Button(显示名称, GUILayout.Height(30)))
                            {
                                选择角色(i);
                            }
                            
                            // 删除按钮
                            if (GUILayout.Button("删", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                删除角色(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        GUILayout.Space(2);
                    }
                }
                else
                {
                    GUILayout.Label("暂无角色", EditorStyles.centeredGreyMiniLabel);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }
    
    private void 绘制右侧角色详情()
    {
        EditorGUILayout.BeginVertical();
        {
            if (当前选中角色 != null)
            {
                string 角色标题 = string.IsNullOrEmpty(当前选中角色.名称) ? "未命名角色" : 当前选中角色.名称;
                GUILayout.Label($"角色详情 - [{当前选中角色.id}] {角色标题}", EditorStyles.boldLabel);
                GUILayout.Space(10);
                
                // 基础属性
                GUILayout.Label("基础属性", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                
                当前选中角色.名称 = EditorGUILayout.TextField("名称", 当前选中角色.名称);
                当前选中角色.力量 = EditorGUILayout.IntField("力量", 当前选中角色.力量);
                当前选中角色.智力 = EditorGUILayout.IntField("智力", 当前选中角色.智力);
                当前选中角色.敏捷 = EditorGUILayout.IntField("敏捷", 当前选中角色.敏捷);
                当前选中角色.耐力 = EditorGUILayout.IntField("耐力", 当前选中角色.耐力);
                当前选中角色.行动力 = EditorGUILayout.IntField("行动力", 当前选中角色.行动力);
                
                GUILayout.Space(10);
                
                // 战斗属性
                GUILayout.Label("战斗属性", EditorStyles.boldLabel);
                当前选中角色.暴击概率 = EditorGUILayout.FloatField("暴击概率", 当前选中角色.暴击概率);
                当前选中角色.暴击伤害倍率 = EditorGUILayout.FloatField("暴击伤害倍率", 当前选中角色.暴击伤害倍率);
                
                GUILayout.Space(10);
                
                // 修正属性
                GUILayout.Label("修正属性", EditorStyles.boldLabel);
                当前选中角色.闪避修正 = EditorGUILayout.FloatField("闪避修正", 当前选中角色.闪避修正);
                当前选中角色.魔法伤害修正 = EditorGUILayout.FloatField("魔法伤害修正", 当前选中角色.魔法伤害修正);
                当前选中角色.血量上限修正 = EditorGUILayout.IntField("血量上限修正", 当前选中角色.血量上限修正);
                当前选中角色.移动格子修正 = EditorGUILayout.IntField("移动格子修正", 当前选中角色.移动格子修正);
                
                GUILayout.Space(10);
                
                // 抗性属性
                GUILayout.Label("抗性属性", EditorStyles.boldLabel);
                当前选中角色.物理抗性 = EditorGUILayout.IntField("物理抗性", 当前选中角色.物理抗性);
                当前选中角色.火焰抗性 = EditorGUILayout.IntField("火焰抗性", 当前选中角色.火焰抗性);
                当前选中角色.冰冻抗性 = EditorGUILayout.IntField("冰冻抗性", 当前选中角色.冰冻抗性);
                当前选中角色.腐败抗性 = EditorGUILayout.IntField("腐败抗性", 当前选中角色.腐败抗性);
                
                GUILayout.Space(10);
                
                // 计算属性（只读显示）
                GUILayout.Label("计算属性（只读）", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.FloatField("闪避", 当前选中角色.闪避);
                EditorGUILayout.FloatField("魔法伤害", 当前选中角色.魔法伤害);
                EditorGUILayout.IntField("血量上限", 当前选中角色.血量上限);
                EditorGUILayout.IntField("可移动格子距离", 当前选中角色.可移动格子距离);
                EditorGUI.EndDisabledGroup();
                
                // 检查是否有修改
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(角色数据);
                }
            }
            else
            {
                GUILayout.Label("请选择一个角色进行编辑", EditorStyles.centeredGreyMiniLabel);
            }
        }
        EditorGUILayout.EndVertical();
    }
    
    private void 选择角色(int 索引)
    {
        if (角色数据 != null && 索引 >= 0 && 索引 < 角色数据.角色列表.Count)
        {
            当前选中角色 = 角色数据.角色列表[索引];
        }
    }
    
    private void 添加新角色()
    {
        if (角色数据 != null)
        {
            var 新角色 = new 角色类()
            {
                名称 = $"新角色_{角色数据.角色列表.Count + 1}",
                // 力量 = 10,
                // 智力 = 10,
                // 敏捷 = 10,
                // 耐力 = 10,
                // 行动力 = 3
            };
            
            角色数据.角色列表.Add(新角色);
            
            // 重新排列所有角色的ID
            重新排列角色ID();
            
            当前选中角色 = 新角色;
            
            // 标记数据已修改
            EditorUtility.SetDirty(角色数据);
            
            Debug.Log($"添加了新角色: {新角色.名称}");
        }
    }
    
    private void 删除角色(int 索引)
    {
        if (角色数据 != null && 索引 >= 0 && 索引 < 角色数据.角色列表.Count)
        {
            var 要删除的角色 = 角色数据.角色列表[索引];
            string 角色名称 = string.IsNullOrEmpty(要删除的角色.名称) ? $"角色_{索引 + 1}" : 要删除的角色.名称;
            
            // 确认删除对话框
            if (EditorUtility.DisplayDialog("确认删除", $"确定要删除角色 '{角色名称}' 吗？\n此操作无法撤销。", "删除", "取消"))
            {
                // 如果删除的是当前选中的角色，清空选择
                if (当前选中角色 == 要删除的角色)
                {
                    当前选中角色 = null;
                }
                
                // 从列表中移除角色
                角色数据.角色列表.RemoveAt(索引);
                
                // 重新排列所有角色的ID
                重新排列角色ID();
                
                // 标记数据已修改
                EditorUtility.SetDirty(角色数据);
                
                Debug.Log($"删除了角色: {角色名称}");
            }
        }
    }
    
    private void 重新排列角色ID()
    {
        if (角色数据 != null && 角色数据.角色列表 != null)
        {
            for (int i = 0; i < 角色数据.角色列表.Count; i++)
            {
                角色数据.角色列表[i].id = i + 1;
            }
            
            // 标记数据已修改
            EditorUtility.SetDirty(角色数据);
            
            Debug.Log($"重新排列了 {角色数据.角色列表.Count} 个角色的ID");
        }
    }
}