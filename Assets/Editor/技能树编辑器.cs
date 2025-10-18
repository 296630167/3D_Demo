using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public class 技能树编辑器 : EditorWindow
{
    
    [MenuItem("工具/编辑器/技能树编辑器")]
    public static void ShowWindow()
    {
        var window = GetWindow<技能树编辑器>("技能树编辑器");
        window.Show();
    }
    
    // 数据路径
    private const string 技能树数据路径 = "Assets/Resources/数据/技能树数据.asset";
    private const string 技能数据路径 = "Assets/Resources/数据/技能数据.asset";
    
    // 核心数据
    private 技能树管理 技能树数据;
    private 技能管理 技能数据;
    private 技能树类 当前选中技能树;
    
    // 数据状态
    private bool 数据已修改 = false;
    private bool 数据加载成功 = false;
    
    // 连接状态字典 - 用元组(左格子,右格子)作为key，bool作为value记录激活状态
    private Dictionary<(技能树格子, 技能树格子), bool> 连接状态字典 = new Dictionary<(技能树格子, 技能树格子), bool>();

    /// <summary>
    /// 从技能树连接列表加载连接状态到字典中
    /// </summary>
    private void 加载连接状态(技能树类 技能树)
    {
        连接状态字典.Clear();
        if (技能树?.技能树连接列表 != null)
        {
            foreach (var 连接 in 技能树.技能树连接列表)
            {
                if (连接.上级格子 != null && 连接.下级格子 != null)
                {
                    var 连接元组 = (连接.上级格子, 连接.下级格子);
                    连接状态字典[连接元组] = true; // 已存在的连接设为激活状态
                }
            }
        }
    }

    private void OnEnable()
    {
        // 确保编辑器启动时没有选中技能树
        当前选中技能树 = null;
        加载数据();
    }

    private void 加载数据()
    {
        // 加载技能树数据
        技能树数据 = AssetDatabase.LoadAssetAtPath<技能树管理>(技能树数据路径);
        if (技能树数据 == null)
        {
            //技能树数据 = 创建默认技能树数据();
        }

        // 加载技能数据
        技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>(技能数据路径);
        if (技能数据 == null)
        {
            Debug.LogWarning($"无法加载技能数据: {技能数据路径}");
            数据加载成功 = false;
            return;
        }

        数据加载成功 = 技能树数据 != null && 技能数据 != null;

        if (数据加载成功)
        {
            Debug.Log("技能树编辑器数据加载成功");
        }
        else
        {
            Debug.LogError("技能树编辑器数据加载失败");
        }
    }
    private void OnGUI()
    {
        if (!数据加载成功)
        {
            GUILayout.Label("数据加载失败，请检查数据文件路径", EditorStyles.boldLabel);
            if (GUILayout.Button("重新加载数据"))
            {
                加载数据();
            }
            return;
        }

        GUILayout.Label("技能树编辑器", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        {
            // 左侧技能导航
            绘制左侧导航();
            
            // 右侧技能树详情
            绘制右侧详情();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void 绘制左侧导航()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        {
            // 标题和添加按钮
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("技能树列表", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("清理数据", GUILayout.Width(70), GUILayout.Height(25)))
                {
                    清理所有空技能对象();
                }
                
                if (GUILayout.Button("添加新技能树", GUILayout.Width(100), GUILayout.Height(25)))
                {
                    // 添加新的技能树导航按钮
                    var r = new 技能树类();
                    技能树数据.技能树列表.Add(r);
                    技能树重新分配id();
                    当前选中技能树 = r; // 设置当前选中技能树，右侧会自动显示
                    自动保存数据();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            // 滚动视图包装技能列表
            EditorGUILayout.BeginScrollView(Vector2.zero);
            {
                if (技能树数据.技能树列表 != null && 技能树数据.技能树列表.Count > 0)
                {
                    for (int i = 0; i < 技能树数据.技能树列表.Count; i++)
                    {
                        var r = 技能树数据.技能树列表[i];
                        string 基础名称 = string.IsNullOrEmpty(r.技能树名称) ? $"技能树_{i + 1}" : r.技能树名称;
                        string 显示名称 = $"[{r.技能树ID} {基础名称}]";
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(显示名称, GUILayout.Width(100), GUILayout.Height(25)))
                            {
                                // 点击技能树导航按钮
                                Debug.Log($"切换到技能树: {显示名称}, 格子数量: {r.技能树格子列表?.Count ?? 0}");
                                // 选中技能树(r);
                                当前选中技能树 = r;
                            }
                            // 删除按钮
                            if (GUILayout.Button("删", GUILayout.Width(50), GUILayout.Height(25)))
                            {
                                if (EditorUtility.DisplayDialog("删除确认", $"确定删除技能树 {显示名称} 吗？", "删除", "取消"))
                                {
                                    // 数据发生改变 调用方法 重拍技能树数据的id
                                    技能树数据.技能树列表.RemoveAt(i);
                                    技能树重新分配id();
                                    if(技能树数据.技能树列表.Count == 0)
                                    {
                                        当前选中技能树 = null;
                                    }
                                    自动保存数据();
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(2);
                    }
                }
                else
                {
                    GUILayout.Label("暂无技能树", EditorStyles.centeredGreyMiniLabel);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void 绘制右侧详情()
    {
        EditorGUILayout.BeginVertical();
        {
            if (当前选中技能树 != null)
            {
                选中技能树(当前选中技能树);
            }
            else
            {
                GUILayout.Label("请选择一个技能树进行编辑", EditorStyles.centeredGreyMiniLabel);
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void 技能树重新分配id()
    {
        技能树数据.技能树列表.ForEach(x => x.技能树ID = 技能树数据.技能树列表.IndexOf(x));
    }

    /// <summary>
    /// 从技能树连接列表初始化连接状态字典
    /// </summary>
    private void 初始化连接状态字典(技能树类 技能树)
    {
        // 清空当前连接状态字典
        连接状态字典.Clear();
        
        // 从技能树连接列表加载已保存的连接状态
        if (技能树.技能树连接列表 != null)
        {
            foreach (var 连接 in 技能树.技能树连接列表)
            {
                if (连接.上级格子 != null && 连接.下级格子 != null)
                {
                    var 连接元组 = (连接.上级格子, 连接.下级格子);
                    连接状态字典[连接元组] = true; // 列表中的连接都是激活状态
                }
            }
        }
        
        Debug.Log($"已从技能树连接列表加载 {连接状态字典.Count} 个连接状态");
    }

    private void 选中技能树(技能树类 r)
    {
            // 初始化连接状态字典
            初始化连接状态字典(r);
            
            // 技能树基本信息编辑
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("技能树基本信息", EditorStyles.boldLabel);
                
                EditorGUI.BeginChangeCheck();
                r.技能树名称 = EditorGUILayout.TextField("技能树名称", r.技能树名称);
                //当前选中技能树.技能树描述 = EditorGUILayout.TextArea(当前选中技能树.技能树描述, GUILayout.Height(60));
                
                if (EditorGUI.EndChangeCheck())
                {
                    自动保存数据();
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            // 计算预览区域尺寸: 3列6行，每个格子75x75，间距增大到80
            float 格子宽度 = 75f;
            float 格子高度 = 75f;
            float 水平间距 = 80f;
            float 垂直间距 = 80f;
            float 左右边距 = 30f;
            float 上下边距 = 30f;
            
            float 预览宽度 = 3 * 格子宽度 + 2 * 水平间距 + 左右边距; // 3个格子 + 2个间距 + 左右边距
            float 预览高度 = 6 * 格子高度 + 5 * 垂直间距 + 上下边距; // 6行格子 + 5个间距 + 上下边距
            
            var previewRect = GUILayoutUtility.GetRect(0, 0, GUILayout.Width(预览宽度), GUILayout.Height(预览高度));
            // 使用深蓝色作为预览背景，与空技能格子的灰色区分开
            EditorGUI.DrawRect(previewRect, new Color(0.1f, 0.1f, 0.2f, 1f));
            var innerRect = new Rect(previewRect.x + 1, previewRect.y + 1, previewRect.width - 2, previewRect.height - 2);
            
            // 记录所有格子的位置信息，用于绘制连接线
            var 格子位置字典 = new Dictionary<技能树格子, Rect>();
            
            foreach(var r1 in r.技能树格子列表)
            {
                // 使用统一的布局变量计算每个格子的正确位置
                float x = previewRect.x + 左右边距/2 + r1.列 * (格子宽度 + 水平间距);
                float y = previewRect.y + 上下边距/2 + r1.行 * (格子高度 + 垂直间距);
                var 格子区域 = new Rect(x, y, 格子宽度, 格子高度);
                
                // 记录格子位置
                格子位置字典[r1] = 格子区域;
                
                // 处理鼠标事件
                Event 当前事件 = Event.current;
                if (格子区域.Contains(当前事件.mousePosition))
                {
                    if (当前事件.type == EventType.MouseDown)
                    {
                        if (当前事件.button == 0) // 左键点击
                        {
                            选择技能窗口(r1);
                            当前事件.Use();
                        }
                        else if (当前事件.button == 1) // 右键清空
                        {
                            Debug.Log($"右键清空技能格子 - 行:{r1.行}, 列:{r1.列}");
                            r1.技能 = null;
                            自动保存数据();
                            Repaint(); // 强制重绘界面
                            当前事件.Use();
                        }
                    }
                }
                // 判断是否为有效技能（不为null且有名称或id大于0）
                bool 有有效技能 = r1.技能 != null && (r1.技能.id > 0 || !string.IsNullOrEmpty(r1.技能.名称));
                // 绘制格子背景
                Color 背景颜色 = 有有效技能 ? 
                    new Color(0.5f, 0.8f, 1f, 0.8f) :      // 有技能：浅蓝色
                    new Color(0.6f, 0.6f, 0.6f, 0.9f);     // 空技能：浅灰色，与深蓝背景形成对比
                EditorGUI.DrawRect(格子区域, 背景颜色);
                // 显示技能内容或占位符
                var 样式 = new GUIStyle(EditorStyles.centeredGreyMiniLabel) { normal = { textColor = Color.white }, fontSize = 有有效技能 ? 10 : 12 };
                if (有有效技能) {
                    if (r1.技能.图标 != null) GUI.DrawTexture(new Rect(格子区域.x + 5, 格子区域.y + 5, 格子区域.width - 10, 格子区域.height - 25), r1.技能.图标.texture, ScaleMode.ScaleToFit);
                    GUI.Label(new Rect(格子区域.x, 格子区域.y + 格子区域.height - 20, 格子区域.width, 20), string.IsNullOrEmpty(r1.技能.名称) ? $"技能_{r1.技能.id}" : r1.技能.名称, 样式);
                } else GUI.Label(new Rect(格子区域.x, 格子区域.y + 格子区域.height/2 - 10, 格子区域.width, 20), "点击选择", 样式);
            }
            
            // 绘制每个格子周围的连接小按钮
            绘制格子连接按钮(r, 格子位置字典);
    }
    private void 选择技能窗口(技能树格子 格子)
    {
        Debug.Log($"打开技能选择窗口 - 格子位置: 行{格子.行}, 列{格子.列}, 当前技能: {(格子.技能?.名称 ?? "无")}");
        
        // 加载技能数据
        if (技能数据 == null || 技能数据.技能列表 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载技能数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        // 添加清空选项
        menu.AddItem(new GUIContent("清空"), false, () =>
        {
            Debug.Log($"清空技能格子 - 行:{格子.行}, 列:{格子.列}");
            格子.技能 = null;
            自动保存数据();
            Repaint(); // 强制重绘界面
            Debug.Log("技能已清空并保存");
        });
        
        menu.AddSeparator("");
        
        // 添加所有可用技能
        foreach (var 技能 in 技能数据.技能列表)
        {
            if (技能 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(技能.名称) ? $"技能_{技能.id}" : 技能.名称;
                bool 已选中 = 格子.技能 == 技能;
                
                menu.AddItem(new GUIContent(显示名称), 已选中, () =>
                {
                    Debug.Log($"选择技能: {显示名称} - 格子位置 行:{格子.行}, 列:{格子.列}");
                    格子.技能 = 技能;
                    自动保存数据();
                    Repaint(); // 强制重绘界面
                    Debug.Log($"技能已设置并保存: {技能.名称}");
                });
            }
        }
        
        menu.ShowAsContext();
    }
    
    private void 自动保存数据()
    {
        if (技能树数据 != null)
        {
            数据已修改 = true;
            EditorUtility.SetDirty(技能树数据);
            AssetDatabase.SaveAssets();
            数据已修改 = false;
            Debug.Log("技能树数据已自动保存");
            
            // 强制重绘界面以更新状态显示
            Repaint();
        }
    }
    
    private void 保存数据()
    {
        自动保存数据();
    }
    
    private void 清理所有空技能对象()
    {
        if (技能树数据 == null || 技能树数据.技能树列表 == null)
        {
            Debug.LogWarning("技能树数据为空，无法清理");
            return;
        }
        
        int 清理计数 = 0;
        
        foreach (var 技能树 in 技能树数据.技能树列表)
        {
            if (技能树.技能树格子列表 == null)
                continue;
                
            foreach (var 格子 in 技能树.技能树格子列表)
            {
                // 如果技能对象存在但是id为0且名称为空，则认为是空技能对象
                if (格子.技能 != null && 
                    格子.技能.id == 0 && 
                    string.IsNullOrEmpty(格子.技能.名称))
                {
                    格子.技能 = null;
                    清理计数++;
                }
            }
        }
        
        if (清理计数 > 0)
        {
            自动保存数据();
            Debug.Log($"已清理 {清理计数} 个空技能对象");
        }
        else
        {
            Debug.Log("没有发现需要清理的空技能对象");
        }
        
        Repaint(); // 强制重绘界面
    }
    
    /// <summary>
    /// 绘制每个格子周围的连接小按钮
    /// </summary>
    private void 绘制格子连接按钮(技能树类 技能树, System.Collections.Generic.Dictionary<技能树格子, Rect> 格子位置字典)
    {
        float 小按钮大小 = 12f;
        
        foreach (var 格子 in 技能树.技能树格子列表)
        {
            if (!格子位置字典.ContainsKey(格子)) continue;
            
            var 当前格子区域 = 格子位置字典[格子];
            var 当前中心 = new Vector2(当前格子区域.center.x, 当前格子区域.center.y);
            
            // 1. 水平连接按钮（左右）
            绘制水平连接按钮(技能树, 格子, 当前格子区域, 格子位置字典, 小按钮大小);
            
            // 2. 垂直连接按钮（上下行的左中右三个位置）
            绘制垂直连接按钮(技能树, 格子, 当前格子区域, 格子位置字典, 小按钮大小);
        }
        
        // 3. 绘制所有已建立的连接线
        绘制所有连接线(技能树, 格子位置字典);
    }
    
    /// <summary>
    /// 绘制水平方向的连接按钮
    /// </summary>
    private void 绘制水平连接按钮(技能树类 技能树, 技能树格子 当前格子, Rect 当前格子区域, Dictionary<技能树格子, Rect> 格子位置字典, float 小按钮大小)
    {
        // 右侧连接按钮（当前技能是第1列或第2列时显示）
        if (当前格子.列 < 2) // 0,1列可以向右连接
        {
            var 右侧格子 = 技能树.技能树格子列表.FirstOrDefault(g => g.行 == 当前格子.行 && g.列 == 当前格子.列 + 1);
            if (右侧格子 != null)
            {
                float 按钮X = 当前格子区域.xMax + 10f;
                float 按钮Y = 当前格子区域.center.y - 小按钮大小 / 2;
                var 按钮区域 = new Rect(按钮X, 按钮Y, 小按钮大小, 小按钮大小);
                
                var 连接元组 = (当前格子, 右侧格子);
                var 反向连接元组 = (右侧格子, 当前格子);
                
                // 从连接状态字典读取状态
                bool 已连接 = 连接状态字典.ContainsKey(连接元组) && 连接状态字典[连接元组] ||
                            连接状态字典.ContainsKey(反向连接元组) && 连接状态字典[反向连接元组];
                
                绘制小连接按钮(按钮区域, 当前格子, 右侧格子, 技能树, 已连接, "→");
            }
        }
        
        // 左侧连接按钮（当前技能是第2列或第3列时显示）
        if (当前格子.列 > 0) // 1,2列可以向左连接
        {
            var 左侧格子 = 技能树.技能树格子列表.FirstOrDefault(g => g.行 == 当前格子.行 && g.列 == 当前格子.列 - 1);
            if (左侧格子 != null)
            {
                float 按钮X = 当前格子区域.xMin - 小按钮大小 - 10f;
                float 按钮Y = 当前格子区域.center.y - 小按钮大小 / 2;
                var 按钮区域 = new Rect(按钮X, 按钮Y, 小按钮大小, 小按钮大小);
                
                var 连接元组 = (当前格子, 左侧格子);
                var 反向连接元组 = (左侧格子, 当前格子);
                
                // 从连接状态字典读取状态
                bool 已连接 = 连接状态字典.ContainsKey(连接元组) && 连接状态字典[连接元组] ||
                            连接状态字典.ContainsKey(反向连接元组) && 连接状态字典[反向连接元组];
                
                绘制小连接按钮(按钮区域, 当前格子, 左侧格子, 技能树, 已连接, "←");
            }
        }
    }
    
    /// <summary>
    /// 绘制垂直方向的连接按钮
    /// </summary>
    private void 绘制垂直连接按钮(技能树类 技能树, 技能树格子 当前格子, Rect 当前格子区域, Dictionary<技能树格子, Rect> 格子位置字典, float 小按钮大小)
    {
        // 上方连接按钮（如果上一行有技能）
        var 上行格子列表 = 技能树.技能树格子列表.Where(g => g.行 == 当前格子.行 - 1).ToList();
        if (上行格子列表.Any())
        {
            绘制行连接按钮(技能树, 当前格子, 当前格子区域, 上行格子列表, 小按钮大小, true);
        }
        
        // 下方连接按钮（如果下一行有技能）
        var 下行格子列表 = 技能树.技能树格子列表.Where(g => g.行 == 当前格子.行 + 1).ToList();
        if (下行格子列表.Any())
        {
            绘制行连接按钮(技能树, 当前格子, 当前格子区域, 下行格子列表, 小按钮大小, false);
        }
    }
    
    /// <summary>
    /// 绘制一行的连接按钮（左中右三个位置）
    /// </summary>
    private void 绘制行连接按钮(技能树类 技能树, 技能树格子 当前格子, Rect 当前格子区域, List<技能树格子> 目标行格子列表, float 小按钮大小, bool 是上方)
    {
        float 按钮间距 = 18f;
        float 起始X = 当前格子区域.center.x - 按钮间距;
        float 按钮Y = 是上方 ? 
            当前格子区域.yMin - 小按钮大小 - 10f : 
            当前格子区域.yMax + 10f;
        
        // 绘制三个位置的按钮：左(0列)、中(1列)、右(2列)
        for (int 列索引 = 0; 列索引 < 3; 列索引++)
        {
            var 目标格子 = 目标行格子列表.FirstOrDefault(g => g.列 == 列索引);
            if (目标格子 != null)
            {
                float 按钮X = 起始X + 列索引 * 按钮间距;
                var 按钮区域 = new Rect(按钮X, 按钮Y, 小按钮大小, 小按钮大小);
                
                var 连接元组 = (当前格子, 目标格子);
                var 反向连接元组 = (目标格子, 当前格子);
                
                // 从连接状态字典读取状态
                bool 已连接 = 连接状态字典.ContainsKey(连接元组) && 连接状态字典[连接元组] ||
                            连接状态字典.ContainsKey(反向连接元组) && 连接状态字典[反向连接元组];
                
                string 按钮文本 = 是上方 ? "↑" : "↓";
                绘制小连接按钮(按钮区域, 当前格子, 目标格子, 技能树, 已连接, 按钮文本);
            }
        }
    }
    
    /// <summary>
    /// 绘制小连接按钮
    /// </summary>
    private void 绘制小连接按钮(Rect 按钮区域, 技能树格子 格子1, 技能树格子 格子2, 技能树类 技能树, bool 已连接, string 方向符号)
    {
        var 按钮样式 = new GUIStyle(GUI.skin.button);
        按钮样式.fontSize = 8;
        按钮样式.normal.textColor = Color.white;
        按钮样式.padding = new RectOffset(0, 0, 0, 0);
        
        // 设置按钮背景颜色
        var 原背景色 = GUI.backgroundColor;
        GUI.backgroundColor = 已连接 ? new Color(1f, 0.3f, 0.3f, 0.8f) : new Color(0.3f, 1f, 0.3f, 0.8f);
        
        string 按钮文本 = 已连接 ? "×" : 方向符号;
        if (GUI.Button(按钮区域, 按钮文本, 按钮样式))
        {
            // 创建连接元组用于状态字典
            var 连接元组 = (格子1, 格子2);
            
            if (已连接)
            {
                // 移除连接
                技能树.技能树连接列表.RemoveAll(c => 
                    (c.上级格子 == 格子1 && c.下级格子 == 格子2) || 
                    (c.上级格子 == 格子2 && c.下级格子 == 格子1));
                
                // 同步到连接状态字典
                连接状态字典[连接元组] = false;
                var 反向连接元组 = (格子2, 格子1);
                连接状态字典[反向连接元组] = false;
                
                Debug.Log($"移除连接: ({格子1.行},{格子1.列}) <-> ({格子2.行},{格子2.列})");
            }
            else
            {
                // 添加连接
                技能树.技能树连接列表.Add(new 技能树连接(格子1, 格子2));
                
                // 同步到连接状态字典
                连接状态字典[连接元组] = true;
                var 反向连接元组 = (格子2, 格子1);
                连接状态字典[反向连接元组] = true;
                
                Debug.Log($"添加连接: ({格子1.行},{格子1.列}) <-> ({格子2.行},{格子2.列})");
            }
            自动保存数据();
            Repaint();
        }
        
        GUI.backgroundColor = 原背景色;
    }
    
    /// <summary>
    /// 绘制所有已建立的连接线
    /// </summary>
    private void 绘制所有连接线(技能树类 技能树, System.Collections.Generic.Dictionary<技能树格子, Rect> 格子位置字典)
    {
        if (技能树.技能树连接列表 == null || 技能树.技能树连接列表.Count == 0)
            return;
            
        foreach (var 连接 in 技能树.技能树连接列表)
        {
            if (连接.上级格子 == null || 连接.下级格子 == null)
                continue;
                
            if (!格子位置字典.ContainsKey(连接.上级格子) || !格子位置字典.ContainsKey(连接.下级格子))
                continue;
            
            var 连接元组 = (连接.上级格子, 连接.下级格子);
            var 反向连接元组 = (连接.下级格子, 连接.上级格子);
            
            // 检查连接状态字典中的激活状态
            bool 连接激活 = (连接状态字典.ContainsKey(连接元组) && 连接状态字典[连接元组]) ||
                         (连接状态字典.ContainsKey(反向连接元组) && 连接状态字典[反向连接元组]);
            
            if (!连接激活) continue; // 只绘制激活的连接
                
            var 起始格子区域 = 格子位置字典[连接.上级格子];
            var 目标格子区域 = 格子位置字典[连接.下级格子];
            
            // 计算连接线的起点和终点
            Vector2 起点, 终点;
            计算连接线端点(起始格子区域, 目标格子区域, out 起点, out 终点);
            
            // 绘制连接线
            绘制连接线(起点, 终点, Color.yellow, 2f);
        }
    }
    
    /// <summary>
    /// 计算两个格子之间连接线的起点和终点
    /// </summary>
    private void 计算连接线端点(Rect 起始格子区域, Rect 目标格子区域, out Vector2 起点, out Vector2 终点)
    {
        var 起始中心 = 起始格子区域.center;
        var 目标中心 = 目标格子区域.center;
        
        // 优先判断是否为跨行连接（不同行之间的连接）
        if (Mathf.Abs(起始中心.y - 目标中心.y) > 5f) // 如果Y轴距离大于5像素，认为是跨行连接
        {
            // 跨行连接 - 始终从上面一行格子的最下端中心点连接到下面一行格子的最上端中心点
            if (起始中心.y < 目标中心.y)
            {
                // 起始格子在上方，目标格子在下方
                起点 = new Vector2(起始中心.x, 起始格子区域.yMax);  // 上面格子的最下端中心点
                终点 = new Vector2(目标中心.x, 目标格子区域.yMin);  // 下面格子的最上端中心点
            }
            else
            {
                // 起始格子在下方，目标格子在上方 - 调整为从上面的格子连到下面的格子
                起点 = new Vector2(目标中心.x, 目标格子区域.yMax);  // 上面格子的最下端中心点
                终点 = new Vector2(起始中心.x, 起始格子区域.yMin);  // 下面格子的最上端中心点
            }
        }
        else
        {
            // 同行连接（水平连接）
            if (起始中心.x < 目标中心.x)
            {
                // 从左到右
                起点 = new Vector2(起始格子区域.xMax, 起始中心.y);
                终点 = new Vector2(目标格子区域.xMin, 目标中心.y);
            }
            else
            {
                // 从右到左
                起点 = new Vector2(起始格子区域.xMin, 起始中心.y);
                终点 = new Vector2(目标格子区域.xMax, 目标中心.y);
            }
        }
    }
    
    /// <summary>
    /// 绘制两点之间的连接线
    /// </summary>
    private void 绘制连接线(Vector2 起点, Vector2 终点, Color 颜色, float 宽度)
    {
        var 原色 = Handles.color;
        Handles.color = 颜色;
        Handles.DrawAAPolyLine(宽度, 起点, 终点);
        Handles.color = 原色;
    }
}