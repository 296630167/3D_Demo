using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class 道具编辑器 : EditorWindow
{
    [MenuItem("工具/编辑器/道具编辑器")]
    public static void 打开窗口()
    {
        var 窗口 = GetWindow<道具编辑器>("道具编辑器");
        窗口.Show();
    }

    private 道具管理 道具数据;
    private const string 道具数据路径 = "Assets/Resources/数据/道具数据.asset";
    
    private 技能管理 技能数据;
    private const string 技能数据路径 = "Assets/Resources/数据/技能数据.asset";
    
    // 导航状态
    private 大类? 当前选中大类 = null;
    private 小类? 当前选中小类 = null;
    private 道具类 当前选中道具;
    
    // 滚动位置
    private Vector2 大类导航滚动位置;
    private Vector2 小类导航滚动位置;
    private Vector2 道具列表滚动位置;
    private Vector2 道具详情滚动位置;

    private void OnEnable()
    {
        // 确保编辑器启动时没有选中道具
        当前选中道具 = null;
        加载数据();
    }

    /// <summary>
    /// 加载道具数据和技能数据
    /// </summary>
    private void 加载数据()
    {
        // 加载道具数据
        道具数据 = AssetDatabase.LoadAssetAtPath<道具管理>(道具数据路径);

        if (道具数据 == null)
        {
            Debug.LogWarning($"无法在路径 {道具数据路径} 找到道具数据文件");
        }
        else
        {
            Debug.Log($"成功加载道具数据: {道具数据路径}");
        }
        
        // 加载技能数据
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
        // 计算窗口高度的80%作为各区域的高度
        float 窗口高度 = position.height;
        float 区域高度 = 窗口高度 * 0.8f;
        // 确保最小高度
        区域高度 = Mathf.Max(区域高度, 300f);
        
        GUILayout.Label("道具编辑器", EditorStyles.boldLabel);
        
        if (道具数据 != null)
        {
            var 道具总数 = 道具数据.道具列表?.Count ?? 0;
            GUILayout.Label($"成功加载道具数据，共 {道具总数} 个道具");
            
            // 四列布局：大类 + 小类 + 道具 + 详情
            EditorGUILayout.BeginHorizontal();
            {
                // 第一列：大类导航（始终显示）
                绘制大类导航列();
                
                // 第二列：小类导航（选择大类后显示）
                if (当前选中大类.HasValue)
                {
                    绘制小类导航列();
                }
                
                // 第三列：道具导航（选择小类后显示）
                if (当前选中小类.HasValue)
                {
                    绘制道具导航列();
                }
                
                // 第四列：详情区域（选择道具后显示）
                if (当前选中道具 != null)
                {
                    绘制详情列();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("道具数据未加载", EditorStyles.centeredGreyMiniLabel);
            if (GUILayout.Button("重新加载数据"))
            {
                加载数据();
            }
        }
    }

    private void 绘制大类导航列()
    {
        // 计算区域高度
        float 窗口高度 = position.height;
        float 区域高度 = Mathf.Max(窗口高度 * 0.8f, 300f);
        
        EditorGUILayout.BeginVertical("box", GUILayout.Width(150));
        {
            GUILayout.Label("大类", EditorStyles.boldLabel);
            
            大类导航滚动位置 = EditorGUILayout.BeginScrollView(大类导航滚动位置, GUILayout.Height(区域高度));
            {
                var 大类列表 = System.Enum.GetValues(typeof(大类)).Cast<大类>().ToArray();
                
                foreach (var 大类 in 大类列表)
                {
                    // 根据选中状态改变按钮样式
                    var 按钮样式 = 当前选中大类 == 大类 ? EditorStyles.miniButtonMid : EditorStyles.miniButton;
                    var 背景色 = 当前选中大类 == 大类 ? Color.cyan : GUI.backgroundColor;
                    
                    GUI.backgroundColor = 背景色;
                    
                    if (GUILayout.Button($"{大类}", 按钮样式, GUILayout.Height(30)))
                    {
                        if (当前选中大类 == 大类)
                        {
                            // 点击已选中的大类，取消选择
                            当前选中大类 = null;
                            当前选中小类 = null;
                            当前选中道具 = null;
                        }
                        else
                        {
                            // 选择新的大类
                            当前选中大类 = 大类;
                            当前选中小类 = null; // 重置小类选择
                            当前选中道具 = null; // 重置道具选择
                        }
                    }
                    
                    GUI.backgroundColor = Color.white;
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void 绘制小类导航列()
    {
        // 计算区域高度
        float 窗口高度 = position.height;
        float 区域高度 = Mathf.Max(窗口高度 * 0.8f, 300f);
        
        EditorGUILayout.BeginVertical("box", GUILayout.Width(150));
        {
            GUILayout.Label($"小类", EditorStyles.boldLabel);
            GUILayout.Label($"({当前选中大类})", EditorStyles.miniLabel);
            
            小类导航滚动位置 = EditorGUILayout.BeginScrollView(小类导航滚动位置, GUILayout.Height(区域高度));
            {
                var 小类列表 = 获取大类对应小类列表(当前选中大类.Value);
                
                foreach (var 小类 in 小类列表)
                {
                    // 根据选中状态改变按钮样式
                    var 按钮样式 = 当前选中小类 == 小类 ? EditorStyles.miniButtonMid : EditorStyles.miniButton;
                    var 背景色 = 当前选中小类 == 小类 ? Color.green : GUI.backgroundColor;
                    
                    GUI.backgroundColor = 背景色;
                    
                    if (GUILayout.Button($"{小类}", 按钮样式, GUILayout.Height(30)))
                    {
                        if (当前选中小类 == 小类)
                        {
                            // 点击已选中的小类，取消选择
                            当前选中小类 = null;
                            当前选中道具 = null;
                        }
                        else
                        {
                            // 选择新的小类
                            当前选中小类 = 小类;
                            当前选中道具 = null; // 重置道具选择
                        }
                    }
                    
                    GUI.backgroundColor = Color.white;
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void 绘制道具导航列()
    {
        // 计算区域高度
        float 窗口高度 = position.height;
        float 区域高度 = Mathf.Max(窗口高度 * 0.8f, 300f);
        // 为添加按钮预留空间
        float 列表高度 = 区域高度 - 60f;
        
        EditorGUILayout.BeginVertical("box", GUILayout.Width(200));
        {
            GUILayout.Label($"道具列表", EditorStyles.boldLabel);
            GUILayout.Label($"({当前选中小类})", EditorStyles.miniLabel);
            
            道具列表滚动位置 = EditorGUILayout.BeginScrollView(道具列表滚动位置, GUILayout.Height(列表高度));
            {
                var 道具列表 = 获取当前小类道具列表();
                
                if (道具列表 != null && 道具列表.Count > 0)
                {
                    for (int i = 0; i < 道具列表.Count; i++)
                    {
                        var 道具 = 道具列表[i];
                        if (道具 == null) continue;
                        
                        string 显示名称 = string.IsNullOrEmpty(道具.名称) ? $"道具_{道具.id}" : 道具.名称;
                        
                        EditorGUILayout.BeginHorizontal();
                        {
                            // 根据选中状态改变按钮样式
                            var 按钮样式 = 当前选中道具 == 道具 ? EditorStyles.miniButtonMid : EditorStyles.miniButton;
                            var 背景色 = 当前选中道具 == 道具 ? Color.yellow : GUI.backgroundColor;
                            
                            GUI.backgroundColor = 背景色;
                            
                            if (GUILayout.Button($"[{道具.id}] {显示名称}", 按钮样式, GUILayout.Height(25)))
                            {
                                if (当前选中道具 == 道具)
                                {
                                    // 点击已选中的道具，取消选择
                                    当前选中道具 = null;
                                    GUI.FocusControl(null);
                                    Repaint();
                                }
                                else
                                {
                                    // 选择新的道具
                                    当前选中道具 = 道具;
                                    GUI.FocusControl(null);
                                    Repaint();
                                }
                            }
                            
                            GUI.backgroundColor = Color.white;
                            
                            // 删除按钮
                            if (GUILayout.Button("删", GUILayout.Width(30), GUILayout.Height(25)))
                            {
                                if (EditorUtility.DisplayDialog("删除确认", $"确定删除道具 {显示名称} 吗？", "删除", "取消"))
                                {
                                    // 如果删除的是当前选中的道具，清空选择
                                    if (当前选中道具 == 道具)
                                    {
                                        当前选中道具 = null;
                                        GUI.FocusControl(null);
                                        Repaint();
                                    }
                                    
                                    // 从对应列表中删除道具
                                    删除道具(道具);
                                    自动保存数据();
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    GUILayout.Label("该类别暂无道具", EditorStyles.centeredGreyMiniLabel);
                }
            }
            EditorGUILayout.EndScrollView();
            
            // 添加新道具按钮
            if (GUILayout.Button($"添加新{当前选中小类}", GUILayout.Height(30)))
            {
                添加新道具();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void 绘制详情列()
    {
        float 窗口高度 = position.height;
        float 区域高度 = Mathf.Max(窗口高度 * 0.8f, 300f);
        
        EditorGUILayout.BeginVertical("box");
        {
            GUILayout.Label("道具详情", EditorStyles.boldLabel);
            
            道具详情滚动位置 = EditorGUILayout.BeginScrollView(道具详情滚动位置, GUILayout.Height(区域高度));
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Label($"编辑道具: {当前选中道具.名称}", EditorStyles.boldLabel);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    GUI.SetNextControlName($"道具名称_{当前选中道具.id}");
                    string 新名称 = EditorGUILayout.TextField("名称", 当前选中道具.名称);
                    GUI.SetNextControlName($"道具介绍_{当前选中道具.id}");
                    string 新介绍 = EditorGUILayout.TextArea(当前选中道具.介绍, GUILayout.Height(60));
                    
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.EnumPopup("大类", 当前选中道具.大类);
                    EditorGUILayout.EnumPopup("小类", 当前选中道具.小类);
                    EditorGUI.EndDisabledGroup();
                    
                    GUI.SetNextControlName($"道具长度_{当前选中道具.id}");
                    int 新长度 = EditorGUILayout.IntField("长度", 当前选中道具.长度);
                    GUI.SetNextControlName($"道具高度_{当前选中道具.id}");
                    int 新高度 = EditorGUILayout.IntField("高度", 当前选中道具.高度);
                    GUI.SetNextControlName($"道具品质_{当前选中道具.id}");
                    品质 新品质 = (品质)EditorGUILayout.EnumPopup("品质", 当前选中道具.品质);
                    GUI.SetNextControlName($"道具交易点_{当前选中道具.id}");
                    道具交易点 新交易点 = (道具交易点)EditorGUILayout.EnumPopup("固定交易点", 当前选中道具.固定交易点);
                    GUI.SetNextControlName($"道具数量上限_{当前选中道具.id}");
                    int 新数量上限 = Mathf.Max(1, EditorGUILayout.IntField("数量上限", 当前选中道具.数量上限));
                    GUI.SetNextControlName($"道具价格_{当前选中道具.id}");
                    int 新价格 = Mathf.Max(0, EditorGUILayout.IntField("价格", 当前选中道具.价格));
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        当前选中道具.名称 = 新名称;
                        当前选中道具.介绍 = 新介绍;
                        当前选中道具.长度 = 新长度;
                        当前选中道具.高度 = 新高度;
                        当前选中道具.品质 = 新品质;
                        当前选中道具.固定交易点 = 新交易点;
                        当前选中道具.数量上限 = 新数量上限;
                        当前选中道具.价格 = 新价格;
                        
                        EditorUtility.SetDirty(道具数据);
                    }
                    
                    绘制特定道具属性();
                    
                    GUILayout.Space(10);
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("保存道具数据", GUILayout.Height(30)))
                        {
                            自动保存数据();
                            Debug.Log($"已保存道具: {当前选中道具.名称}");
                        }
                        
                        if (GUILayout.Button("取消选择", GUILayout.Height(30), GUILayout.Width(80)))
                        {
                            当前选中道具 = null;
                            GUI.FocusControl(null);
                            Repaint();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }



    private void 绘制特定道具属性()
    {
        if (!当前选中小类.HasValue) return;
        
        GUILayout.Space(10);
        GUILayout.Label("特定属性编辑", EditorStyles.boldLabel);
        
        switch (当前选中小类.Value)
        {
            case 小类.药水:
                绘制药水属性编辑();
                break;
            case 小类.卷轴:
                绘制卷轴属性编辑();
                break;
            case 小类.单手武器:
            case 小类.双手武器:
            case 小类.远程武器:
            case 小类.法杖:
                绘制武器属性编辑();
                break;
            case 小类.辅助武器:
                绘制辅助武器属性编辑();
                break;
            case 小类.草药:
                绘制草药属性编辑();
                break;
            case 小类.锻造材料:
                绘制锻造材料属性编辑();
                break;
            case 小类.补给:
                绘制补给属性编辑();
                break;
            default:
                GUILayout.Label("(该类型暂无特定属性编辑)", EditorStyles.miniLabel);
                break;
        }
    }
    
    private void 绘制药水属性编辑()
    {
        var 药水 = 当前选中道具 as 药水;
        if (药水 != null)
        {
            EditorGUI.BeginChangeCheck();
            
            GUI.SetNextControlName($"药水回复类型_{药水.id}");
            回复类型 新回复类型 = (回复类型)EditorGUILayout.EnumPopup("恢复类型", 药水.回复类型);
            GUI.SetNextControlName($"药水回复数值_{药水.id}");
            int 新回复数值 = EditorGUILayout.IntField("恢复数值", 药水.回复数值);
            
            if (EditorGUI.EndChangeCheck())
            {
                药水.回复类型 = 新回复类型;
                药水.回复数值 = 新回复数值;
                EditorUtility.SetDirty(道具数据);
            }
        }
    }
    
    private void 绘制卷轴属性编辑()
    {
        var 卷轴 = 当前选中道具 as 卷轴;
        if (卷轴 != null)
        {
            EditorGUILayout.LabelField("卷轴技能", EditorStyles.boldLabel);
            
            // 绘制技能图标选择区域
            绘制技能图标选择(卷轴);
        }
    }
    
    /// <summary>
    /// 绘制技能图标选择区域
    /// </summary>
    private void 绘制技能图标选择(卷轴 卷轴)
    {
        if (技能数据 == null || 技能数据.技能列表 == null)
        {
            EditorGUILayout.HelpBox("技能数据未加载，无法选择技能", MessageType.Warning);
            return;
        }
        
        // 技能图标区域大小 - 固定50x50像素
        float 图标宽度 = 50f;
        float 图标高度 = 50f;
        
        // 创建技能图标区域，限制宽度不扩展
        var 图标区域 = GUILayoutUtility.GetRect(图标宽度, 图标高度, GUILayout.Width(图标宽度), GUILayout.Height(图标高度), GUILayout.ExpandWidth(false));
        
        // 处理鼠标事件
        Event 当前事件 = Event.current;
        if (图标区域.Contains(当前事件.mousePosition))
        {
            if (当前事件.type == EventType.MouseDown)
            {
                if (当前事件.button == 0) // 左键点击 - 选择技能
                {
                    显示技能选择菜单(卷轴);
                    当前事件.Use();
                }
                else if (当前事件.button == 1) // 右键点击 - 清空技能
                {
                    卷轴.卷轴技能 = null;
                    自动保存数据();
                    Repaint();
                    当前事件.Use();
                }
            }
        }
        
        // 判断技能是否有效（不仅检查null，还要检查id和名称）
        bool 有有效技能 = 技能是否有效(卷轴.卷轴技能);
        
        // 绘制技能图标背景
        Color 背景颜色 = 有有效技能 ? 
            new Color(0.5f, 0.8f, 1f, 0.8f) :      // 有技能：浅蓝色
            new Color(0.6f, 0.6f, 0.6f, 0.9f);     // 无技能：浅灰色
        
        EditorGUI.DrawRect(图标区域, 背景颜色);
        
        // 绘制技能内容
        if (有有效技能)
        {
            // 绘制技能图标
            if (卷轴.卷轴技能.图标 != null)
            {
                var 图标内容区域 = new Rect(图标区域.x + 3, 图标区域.y + 3, 图标区域.width - 6, 图标区域.height - 15);
                GUI.DrawTexture(图标内容区域, 卷轴.卷轴技能.图标.texture, ScaleMode.ScaleToFit);
            }
            
            // 绘制技能名称
            var 名称区域 = new Rect(图标区域.x, 图标区域.y + 图标区域.height - 12, 图标区域.width, 12);
            var 名称样式 = new GUIStyle(EditorStyles.centeredGreyMiniLabel) 
            { 
                normal = { textColor = Color.white }, 
                fontSize = 8 
            };
            string 显示名称 = string.IsNullOrEmpty(卷轴.卷轴技能.名称) ? $"技能_{卷轴.卷轴技能.id}" : 卷轴.卷轴技能.名称;
            GUI.Label(名称区域, 显示名称, 名称样式);
        }
        else
        {
            // 绘制占位符文本
            var 占位符样式 = new GUIStyle(EditorStyles.centeredGreyMiniLabel) 
            { 
                normal = { textColor = Color.white }, 
                fontSize = 10 
            };
            var 占位符区域 = new Rect(图标区域.x, 图标区域.y + 图标区域.height/2 - 5, 图标区域.width, 10);
            GUI.Label(占位符区域, "选择", 占位符样式);
        }
    }
    
    /// <summary>
    /// 判断技能是否有效（不仅检查null，还要检查关键属性）
    /// </summary>
    private bool 技能是否有效(技能类 技能)
    {
        return 技能 != null && 
               技能.id > 0 && 
               !string.IsNullOrEmpty(技能.名称) && 
               技能.名称 != "新技能" && 
               !技能.名称.StartsWith("新技能_");
    }
    
    /// <summary>
    /// 显示技能选择菜单
    /// </summary>
    private void 显示技能选择菜单(卷轴 卷轴)
    {
        GenericMenu menu = new GenericMenu();
        
        // 添加清空选项
        menu.AddItem(new GUIContent("清空"), false, () =>
        {
            卷轴.卷轴技能 = null;
            自动保存数据();
            Repaint();
        });
        
        menu.AddSeparator("");
        
        // 添加所有有效技能
        foreach (var 技能 in 技能数据.技能列表)
        {
            if (技能是否有效(技能))
            {
                string 显示名称 = 技能.名称;
                bool 已选中 = 卷轴.卷轴技能 == 技能;
                
                menu.AddItem(new GUIContent(显示名称), 已选中, () =>
                {
                    卷轴.卷轴技能 = 技能;
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }

    #region 辅助方法

    private 小类[] 获取大类对应小类列表(大类 大类)
    {
        switch (大类)
        {
            case 大类.消耗品:
                return new 小类[] { 小类.药水, 小类.卷轴 };
            case 大类.装备:
                return new 小类[] { 小类.单手武器, 小类.双手武器, 小类.远程武器, 小类.法杖, 小类.辅助武器 };
            case 大类.材料:
                return new 小类[] { 小类.草药, 小类.锻造材料 };
            case 大类.补给:
                return new 小类[] { 小类.补给 };
            default:
                return new 小类[] { 小类.药水 };
        }
    }

    private 小类 获取大类默认小类(大类 大类)
    {
        var 小类列表 = 获取大类对应小类列表(大类);
        return 小类列表.Length > 0 ? 小类列表[0] : 小类.药水;
    }

    private List<道具类> 获取当前小类道具列表()
    {
        if (道具数据 == null || !当前选中小类.HasValue) return new List<道具类>();

        switch (当前选中小类.Value)
        {
            case 小类.药水:
                return 道具数据.药水列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.卷轴:
                return 道具数据.卷轴列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.单手武器:
                return 道具数据.单手武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.双手武器:
                return 道具数据.双手武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.远程武器:
                return 道具数据.远程武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.法杖:
                return 道具数据.法杖列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.辅助武器:
                return 道具数据.辅助武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.草药:
                return 道具数据.草药列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.锻造材料:
                return 道具数据.锻造材料列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.补给:
                return 道具数据.补给列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            default:
                return new List<道具类>();
        }
    }

    private void 添加新道具()
    {
        if (!当前选中小类.HasValue) return;
        
        道具类 新道具 = null;

        switch (当前选中小类.Value)
        {
            case 小类.药水:
                var 新药水 = new 药水();
                新药水.回复类型 = 回复类型.回复生命值;
                新药水.回复数值 = 50;
                新道具 = 新药水;
                道具数据.药水列表.Add(新药水);
                break;
            case 小类.卷轴:
                var 新卷轴 = new 卷轴();
                新卷轴.卷轴技能 = null;
                新道具 = 新卷轴;
                道具数据.卷轴列表.Add(新卷轴);
                break;
            case 小类.单手武器:
                新道具 = new 单手武器();
                道具数据.单手武器列表.Add((单手武器)新道具);
                break;
            case 小类.双手武器:
                新道具 = new 双手武器();
                道具数据.双手武器列表.Add((双手武器)新道具);
                break;
            case 小类.远程武器:
                新道具 = new 远程武器();
                道具数据.远程武器列表.Add((远程武器)新道具);
                break;
            case 小类.法杖:
                新道具 = new 法杖();
                道具数据.法杖列表.Add((法杖)新道具);
                break;
            case 小类.辅助武器:
                新道具 = new 辅助武器();
                道具数据.辅助武器列表.Add((辅助武器)新道具);
                break;
            case 小类.草药:
                新道具 = new 草药();
                道具数据.草药列表.Add((草药)新道具);
                break;
            case 小类.锻造材料:
                新道具 = new 锻造材料();
                道具数据.锻造材料列表.Add((锻造材料)新道具);
                break;
            case 小类.补给:
                新道具 = new 补给();
                道具数据.补给列表.Add((补给)新道具);
                break;
        }

        if (新道具 != null)
        {
            新道具.名称 = $"新{当前选中小类}";
            新道具.大类 = 当前选中大类.Value;
            新道具.小类 = 当前选中小类.Value;
            新道具.品质 = 品质.普通;
            新道具.固定交易点 = 道具交易点.无;
            新道具.长度 = 1;
            新道具.高度 = 1;
            新道具.数量上限 = 1;
            新道具.价格 = 0;
            
            // 自动分配ID
            新道具.id = 获取下一个可用ID(当前选中小类.Value);
            
            当前选中道具 = 新道具;
            GUI.FocusControl(null);
            Repaint();
            自动保存数据();
        }
    }

    private void 删除道具(道具类 道具)
    {
        if (!当前选中小类.HasValue) return;
        
        switch (当前选中小类.Value)
        {
            case 小类.药水:
                道具数据.药水列表.Remove((药水)道具);
                break;
            case 小类.卷轴:
                道具数据.卷轴列表.Remove((卷轴)道具);
                break;
            case 小类.单手武器:
                道具数据.单手武器列表.Remove((武器)道具);
                break;
            case 小类.双手武器:
                道具数据.双手武器列表.Remove((武器)道具);
                break;
            case 小类.远程武器:
                道具数据.远程武器列表.Remove((武器)道具);
                break;
            case 小类.法杖:
                道具数据.法杖列表.Remove((武器)道具);
                break;
            case 小类.辅助武器:
                道具数据.辅助武器列表.Remove((辅助武器)道具);
                break;
            case 小类.草药:
                道具数据.草药列表.Remove((草药)道具);
                break;
            case 小类.锻造材料:
                道具数据.锻造材料列表.Remove((锻造材料)道具);
                break;
            case 小类.补给:
                道具数据.补给列表.Remove((补给)道具);
                break;
        }
        
        // 删除后重新整理该类型道具的ID序列
        重新整理道具ID(当前选中小类.Value);
    }

    private void 自动保存数据()
    {
        if (道具数据 != null)
        {
            EditorUtility.SetDirty(道具数据);
            AssetDatabase.SaveAssets();
        }
    }
    
    /// <summary>
    /// 获取指定道具类型的ID范围
    /// </summary>
    private (int 起始ID, int 结束ID) 获取道具ID范围(小类 道具类型)
    {
        return (1, int.MaxValue);
    }
    
    /// <summary>
    /// 获取指定类型的道具列表
    /// </summary>
    private List<道具类> 获取道具列表(小类 道具类型)
    {
        if (道具数据 == null) return new List<道具类>();

        switch (道具类型)
        {
            case 小类.药水:
                return 道具数据.药水列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.卷轴:
                return 道具数据.卷轴列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.单手武器:
                return 道具数据.单手武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.双手武器:
                return 道具数据.双手武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.远程武器:
                return 道具数据.远程武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.法杖:
                return 道具数据.法杖列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.辅助武器:
                return 道具数据.辅助武器列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.草药:
                return 道具数据.草药列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.锻造材料:
                return 道具数据.锻造材料列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            case 小类.补给:
                return 道具数据.补给列表?.Cast<道具类>().ToList() ?? new List<道具类>();
            default:
                return new List<道具类>();
        }
    }
    
    /// <summary>
    /// 获取指定类型道具的下一个可用ID
    /// </summary>
    private int 获取下一个可用ID(小类 道具类型)
    {
        var 当前列表 = 获取道具列表(道具类型);
        
        if (当前列表.Count == 0)
        {
            return 1;
        }
        
        // 找到该类型中最大的ID
        int 最大ID = 当前列表.Max(x => x.id);
        return 最大ID + 1;
    }
    
    /// <summary>
    /// 重新整理指定类型道具的ID序列
    /// </summary>
    private void 重新整理道具ID(小类 道具类型)
    {
        var 当前列表 = 获取道具列表(道具类型);
        
        // 按现有ID排序，然后重新分配从1开始的连续ID
        var 排序列表 = 当前列表.OrderBy(x => x.id).ToList();
        for (int i = 0; i < 排序列表.Count; i++)
        {
            排序列表[i].id = i + 1;
        }
    }
    
    private void 保存数据()
    {
        自动保存数据();
    }
    
    /// <summary>
    /// 绘制武器属性编辑
    /// </summary>
    private void 绘制武器属性编辑()
    {
        var 武器 = 当前选中道具 as 武器;
        if (武器 == null) return;
        
        // 基础武器属性
        GUILayout.Label("基础属性", EditorStyles.boldLabel);
        武器.耐力需求 = EditorGUILayout.IntField("耐力需求", 武器.耐力需求);
        武器.命中加成 = EditorGUILayout.FloatField("命中加成", 武器.命中加成);
        武器.暴击率 = EditorGUILayout.FloatField("暴击率", 武器.暴击率);
        武器.耐久 = EditorGUILayout.IntField("耐久", 武器.耐久);
        武器.穿甲 = EditorGUILayout.FloatField("穿甲", 武器.穿甲);
        武器.魔法加成 = EditorGUILayout.FloatField("魔法加成", 武器.魔法加成);
        
        GUILayout.Space(10);
        
        // 属性倍率编辑
        绘制属性倍率编辑(武器.属性倍率);
        
        GUILayout.Space(10);
        
        // 固定伤害编辑
        绘制固定伤害编辑(武器.固定伤害列表);
        
        GUILayout.Space(10);
        
        // 比例伤害编辑
        绘制比例伤害编辑(武器.比例伤害列表);
        
        GUILayout.Space(10);
        
        // 装备附带技能编辑
        绘制装备技能编辑(武器.装备附带技能列表, "装备附带技能");
        
        GUILayout.Space(10);
        
        // 打造材料编辑
        绘制打造材料编辑(武器.打造所需材料字典);
    }
    
    /// <summary>
    /// 绘制辅助武器属性编辑
    /// </summary>
    private void 绘制辅助武器属性编辑()
    {
        var 辅助武器 = 当前选中道具 as 辅助武器;
        if (辅助武器 == null) return;
        
        // 基础属性
        GUILayout.Label("基础属性", EditorStyles.boldLabel);
        辅助武器.耐力需求 = EditorGUILayout.IntField("耐力需求", 辅助武器.耐力需求);
        辅助武器.耐久 = EditorGUILayout.IntField("耐久", 辅助武器.耐久);
        
        GUILayout.Space(10);
        
        // 属性增幅编辑
        绘制属性增幅编辑(辅助武器);
        
        GUILayout.Space(10);
        
        // 装备附带技能编辑
        绘制装备技能ID编辑(辅助武器.装备附带技能列表, "装备附带技能");
        
        GUILayout.Space(10);
        
        // 打造材料编辑
        绘制辅助武器打造材料编辑(辅助武器.打造所需材料列表);
    }
    
    /// <summary>
    /// 绘制属性倍率编辑（武器专用）
    /// </summary>
    private void 绘制属性倍率编辑(List<(属性, float)> 属性倍率列表)
    {
        GUILayout.Label("属性倍率", EditorStyles.boldLabel);
        
        if (属性倍率列表 == null) return;
        
        // 显示现有属性倍率
        for (int i = 属性倍率列表.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                var 当前项 = 属性倍率列表[i];
                var 新属性 = (属性)EditorGUILayout.EnumPopup(当前项.Item1, GUILayout.Width(100));
                var 新倍率 = EditorGUILayout.FloatField(当前项.Item2, GUILayout.Width(80));
                属性倍率列表[i] = (新属性, 新倍率);
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    属性倍率列表.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 添加新属性倍率
        if (GUILayout.Button("添加属性倍率", GUILayout.Height(25)))
        {
            属性倍率列表.Add((属性.力量, 1.0f));
        }
    }
    
    /// <summary>
    /// 绘制固定伤害编辑（武器专用）
    /// </summary>
    private void 绘制固定伤害编辑(List<(属性伤害类型, int)> 固定伤害列表)
    {
        GUILayout.Label("固定伤害", EditorStyles.boldLabel);
        
        if (固定伤害列表 == null) return;
        
        // 显示现有固定伤害
        for (int i = 固定伤害列表.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                var 当前项 = 固定伤害列表[i];
                var 新伤害类型 = (属性伤害类型)EditorGUILayout.EnumPopup(当前项.Item1, GUILayout.Width(120));
                var 新伤害值 = EditorGUILayout.IntField(当前项.Item2, GUILayout.Width(80));
                固定伤害列表[i] = (新伤害类型, 新伤害值);
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    固定伤害列表.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 添加新固定伤害
        if (GUILayout.Button("添加固定伤害", GUILayout.Height(25)))
        {
            固定伤害列表.Add((属性伤害类型.物理伤害, 1));
        }
    }
    
    /// <summary>
    /// 绘制比例伤害编辑（武器专用）
    /// </summary>
    private void 绘制比例伤害编辑(List<(属性伤害类型, float)> 比例伤害列表)
    {
        GUILayout.Label("比例伤害", EditorStyles.boldLabel);
        
        if (比例伤害列表 == null) return;
        
        // 显示现有比例伤害
        for (int i = 比例伤害列表.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                var 当前项 = 比例伤害列表[i];
                var 新伤害类型 = (属性伤害类型)EditorGUILayout.EnumPopup(当前项.Item1, GUILayout.Width(120));
                var 新比例值 = EditorGUILayout.FloatField(当前项.Item2, GUILayout.Width(80));
                比例伤害列表[i] = (新伤害类型, 新比例值);
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    比例伤害列表.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 添加新比例伤害
        if (GUILayout.Button("添加比例伤害", GUILayout.Height(25)))
        {
            比例伤害列表.Add((属性伤害类型.物理伤害, 0.1f));
        }
    }
    
    /// <summary>
    /// 绘制属性增幅编辑（辅助武器专用）
    /// </summary>
    private void 绘制属性增幅编辑(辅助武器 辅助武器)
    {
        GUILayout.Label("属性增幅", EditorStyles.boldLabel);
        
        if (辅助武器.属性增幅 == null)
            辅助武器.属性增幅 = new List<(属性, int)>();
        
        // 显示现有属性增幅
        for (int i = 辅助武器.属性增幅.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                var 当前项 = 辅助武器.属性增幅[i];
                var 新属性 = (属性)EditorGUILayout.EnumPopup(当前项.Item1, GUILayout.Width(100));
                var 新增幅 = EditorGUILayout.IntField(当前项.Item2, GUILayout.Width(80));
                辅助武器.属性增幅[i] = (新属性, 新增幅);
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    辅助武器.属性增幅.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 添加新属性增幅
        if (GUILayout.Button("添加属性增幅", GUILayout.Height(25)))
        {
            辅助武器.属性增幅.Add((属性.力量, 1));
        }
    }
    
    /// <summary>
    /// 绘制装备技能编辑（通用）
    /// </summary>
    private void 绘制装备技能编辑(List<技能类> 技能列表, string 标题)
    {
        GUILayout.Label(标题, EditorStyles.boldLabel);
        
        if (技能列表 == null) return;
        
        // 显示现有技能
        for (int i = 技能列表.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                // 技能选择区域
                EditorGUILayout.BeginVertical(GUILayout.Width(200));
                {
                    string 技能显示名 = 技能列表[i] != null ? 
                        (string.IsNullOrEmpty(技能列表[i].名称) ? $"技能_{技能列表[i].id}" : 技能列表[i].名称) : 
                        "未选择技能";
                    
                    if (GUILayout.Button(技能显示名, GUILayout.Height(25)))
                    {
                        显示装备技能选择菜单(技能列表, i);
                    }
                    
                    // 显示技能简要信息
                    if (技能列表[i] != null)
                    {
                        GUILayout.Label($"ID: {技能列表[i].id} | 类型: {技能列表[i].施法类型}", EditorStyles.miniLabel);
                    }
                }
                EditorGUILayout.EndVertical();
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    技能列表.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
        
        // 添加新技能
        if (GUILayout.Button($"添加{标题}", GUILayout.Height(25)))
        {
            技能列表.Add(null);
        }
    }
    
    /// <summary>
    /// 绘制打造材料编辑（武器专用）
    /// </summary>
    private void 绘制打造材料编辑(Dictionary<材料, int> 打造所需材料字典)
    {
        GUILayout.Label("打造材料", EditorStyles.boldLabel);
        
        if (打造所需材料字典 == null)
        {
            GUILayout.Label("材料字典未初始化", EditorStyles.helpBox);
            return;
        }
        
        // 显示现有材料
        var 材料键列表 = new List<材料>(打造所需材料字典.Keys);
        for (int i = 0; i < 材料键列表.Count; i++)
        {
            var 材料 = 材料键列表[i];
            EditorGUILayout.BeginHorizontal();
            
            // 材料名称显示
            string 材料名称 = 材料 != null ? (string.IsNullOrEmpty(材料.名称) ? $"材料_{材料.id}" : 材料.名称) : "未知材料";
            EditorGUILayout.LabelField(材料名称, GUILayout.Width(150));
            
            // 数量输入
            int 当前数量 = 打造所需材料字典[材料];
            int 新数量 = EditorGUILayout.IntField(当前数量, GUILayout.Width(60));
            if (新数量 != 当前数量 && 新数量 > 0)
            {
                打造所需材料字典[材料] = 新数量;
                自动保存数据();
            }
            
            // 删除按钮
            if (GUILayout.Button("删除", GUILayout.Width(50)))
            {
                打造所需材料字典.Remove(材料);
                自动保存数据();
                break;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        GUILayout.Space(10);
        
        // 添加材料按钮
        if (GUILayout.Button("添加打造材料", GUILayout.Height(25)))
        {
            显示武器材料选择菜单(打造所需材料字典);
        }
    }
    
    /// <summary>
    /// 显示武器材料选择菜单
    /// </summary>
    private void 显示武器材料选择菜单(Dictionary<材料, int> 材料字典)
    {
        if (道具数据 == null || 道具数据.材料列表 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载道具数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        foreach (var 材料 in 道具数据.材料列表)
        {
            if (材料 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(材料.名称) ? $"材料_{材料.id}" : 材料.名称;
                string 菜单路径 = $"{材料.小类}/{显示名称} (ID: {材料.id})";
                
                menu.AddItem(new GUIContent(菜单路径), false, () =>
                {
                    if (材料字典.ContainsKey(材料))
                    {
                        材料字典[材料]++;
                    }
                    else
                    {
                        材料字典[材料] = 1;
                    }
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }

    

    
    /// <summary>
    /// 绘制草药属性编辑
    /// </summary>
    private void 绘制草药属性编辑()
    {
        var 草药 = 当前选中道具 as 草药;
        if (草药 == null) return;
        
        GUILayout.Label("草药属性", EditorStyles.boldLabel);
        GUILayout.Label("草药类型暂无特殊属性需要编辑", EditorStyles.miniLabel);
        GUILayout.Label("可在基础属性中设置名称、介绍、品质等信息", EditorStyles.miniLabel);
    }
    
    /// <summary>
    /// 绘制锻造材料属性编辑
    /// </summary>
    private void 绘制锻造材料属性编辑()
    {
        var 锻造材料 = 当前选中道具 as 锻造材料;
        if (锻造材料 == null) return;
        
        GUILayout.Label("锻造材料属性", EditorStyles.boldLabel);
        GUILayout.Label("锻造材料类型暂无特殊属性需要编辑", EditorStyles.miniLabel);
        GUILayout.Label("可在基础属性中设置名称、介绍、品质等信息", EditorStyles.miniLabel);
    }
    
    /// <summary>
    /// 绘制补给属性编辑
    /// </summary>
    private void 绘制补给属性编辑()
    {
        var 补给 = 当前选中道具 as 补给;
        if (补给 == null) return;
        
        GUILayout.Label("补给属性", EditorStyles.boldLabel);
        GUILayout.Label("补给类型暂无特殊属性需要编辑", EditorStyles.miniLabel);
        GUILayout.Label("可在基础属性中设置名称、介绍、品质等信息", EditorStyles.miniLabel);
    }
    
    /// <summary>
    /// 显示装备技能选择菜单
    /// </summary>
    private void 显示装备技能选择菜单(List<技能类> 技能列表, int 索引)
    {
        // 加载技能数据
        var 技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>("Assets/代码/数据管理/数据编辑/技能数据.asset");
        if (技能数据 == null || 技能数据.技能列表 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载技能数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        // 添加清空选项
        menu.AddItem(new GUIContent("清空"), false, () =>
        {
            技能列表[索引] = null;
            自动保存数据();
            Repaint();
        });
        
        menu.AddSeparator("");
        
        // 添加所有技能选项
        foreach (var 技能 in 技能数据.技能列表)
        {
            if (技能 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(技能.名称) ? $"技能_{技能.id}" : 技能.名称;
                string 菜单路径 = $"{技能.施法类型}/{显示名称} (ID: {技能.id})";
                
                menu.AddItem(new GUIContent(菜单路径), false, () =>
                {
                    技能列表[索引] = 技能;
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }
    
    /// <summary>
    /// 显示材料选择菜单
    /// </summary>
    private void 显示材料选择菜单(Dictionary<材料, int> 材料字典, 材料 当前材料)
    {
        if (道具数据 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载道具数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        // 添加清空选项（移除当前材料）
        menu.AddItem(new GUIContent("移除此材料"), false, () =>
        {
            if (当前材料 != null && 材料字典.ContainsKey(当前材料))
            {
                材料字典.Remove(当前材料);
                自动保存数据();
                Repaint();
            }
        });
        
        menu.AddSeparator("");
        
        // 添加所有材料选项
        foreach (var 材料 in 道具数据.材料列表)
        {
            if (材料 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(材料.名称) ? $"材料_{材料.id}" : 材料.名称;
                string 菜单路径 = $"{材料.小类}/{显示名称} (ID: {材料.id})";
                
                // 检查是否已经存在
                bool 已存在 = 材料字典.ContainsKey(材料);
                
                menu.AddItem(new GUIContent(菜单路径), 已存在, () =>
                {
                    if (当前材料 != null && 材料字典.ContainsKey(当前材料))
                    {
                        // 替换现有材料
                        var 数量 = 材料字典[当前材料];
                        材料字典.Remove(当前材料);
                        材料字典[材料] = 数量;
                    }
                    else if (!材料字典.ContainsKey(材料))
                    {
                        // 添加新材料
                        材料字典[材料] = 1;
                    }
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }
    
    /// <summary>
    /// 绘制装备技能ID编辑（用于辅助武器等使用ID列表的装备）
    /// </summary>
    private void 绘制装备技能ID编辑(List<int> 技能ID列表, string 标题)
    {
        GUILayout.Label(标题, EditorStyles.boldLabel);
        
        if (技能ID列表 == null) return;
        
        // 加载技能数据
        var 技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>("Assets/代码/数据管理/数据编辑/技能数据.asset");
        
        // 显示现有技能
        for (int i = 技能ID列表.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            {
                // 技能选择区域
                EditorGUILayout.BeginVertical(GUILayout.Width(200));
                {
                    string 技能显示名 = "未选择技能";
                    if (技能数据 != null && 技能数据.技能列表 != null)
                    {
                        var 技能 = 技能数据.技能列表.Find(s => s != null && s.id == 技能ID列表[i]);
                        if (技能 != null)
                        {
                            技能显示名 = string.IsNullOrEmpty(技能.名称) ? $"技能_{技能.id}" : 技能.名称;
                        }
                        else
                        {
                            技能显示名 = $"技能ID_{技能ID列表[i]} (未找到)";
                        }
                    }
                    
                    if (GUILayout.Button(技能显示名, GUILayout.Height(25)))
                    {
                        显示装备技能ID选择菜单(技能ID列表, i);
                    }
                    
                    // 显示技能ID
                    GUILayout.Label($"ID: {技能ID列表[i]}", EditorStyles.miniLabel);
                }
                EditorGUILayout.EndVertical();
                
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    技能ID列表.RemoveAt(i);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
        
        // 添加新技能
        if (GUILayout.Button($"添加{标题}", GUILayout.Height(25)))
        {
            技能ID列表.Add(1);
        }
    }
    
    /// <summary>
    /// 显示装备技能ID选择菜单
    /// </summary>
    private void 显示装备技能ID选择菜单(List<int> 技能ID列表, int 索引)
    {
        // 加载技能数据
        var 技能数据 = AssetDatabase.LoadAssetAtPath<技能管理>("Assets/代码/数据管理/数据编辑/技能数据.asset");
        if (技能数据 == null || 技能数据.技能列表 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载技能数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        // 添加所有技能选项
        foreach (var 技能 in 技能数据.技能列表)
        {
            if (技能 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(技能.名称) ? $"技能_{技能.id}" : 技能.名称;
                string 菜单路径 = $"{技能.施法类型}/{显示名称} (ID: {技能.id})";
                
                menu.AddItem(new GUIContent(菜单路径), false, () =>
                {
                    技能ID列表[索引] = 技能.id;
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }
    

    


    /// <summary>
    /// 绘制辅助武器打造材料编辑
    /// </summary>
    private void 绘制辅助武器打造材料编辑(List<(int, int)> 材料列表)
    {
        GUILayout.Label("打造材料", EditorStyles.boldLabel);
        
        if (材料列表 == null)
        {
            GUILayout.Label("材料列表未初始化", EditorStyles.helpBox);
            return;
        }
        
        // 显示现有材料
        for (int i = 0; i < 材料列表.Count; i++)
        {
            var 材料项 = 材料列表[i];
            EditorGUILayout.BeginHorizontal();
            
            // 材料名称显示
            string 材料名称 = "未知材料";
            if (道具数据 != null && 道具数据.材料列表 != null)
            {
                var 材料 = 道具数据.材料列表.Find(m => m != null && m.id == 材料项.Item1);
                if (材料 != null)
                {
                    材料名称 = string.IsNullOrEmpty(材料.名称) ? $"材料_{材料.id}" : 材料.名称;
                }
                else
                {
                    材料名称 = $"材料ID_{材料项.Item1} (未找到)";
                }
            }
            EditorGUILayout.LabelField(材料名称, GUILayout.Width(150));
            
            // 数量输入
            int 新数量 = EditorGUILayout.IntField(材料项.Item2, GUILayout.Width(60));
            if (新数量 != 材料项.Item2 && 新数量 > 0)
            {
                材料列表[i] = (材料项.Item1, 新数量);
                自动保存数据();
            }
            
            // 删除按钮
            if (GUILayout.Button("删除", GUILayout.Width(50)))
            {
                材料列表.RemoveAt(i);
                自动保存数据();
                break;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        GUILayout.Space(10);
        
        // 添加材料按钮
        if (GUILayout.Button("添加打造材料", GUILayout.Height(25)))
        {
            显示辅助武器材料选择菜单(材料列表);
        }
    }
    
    /// <summary>
    /// 显示辅助武器材料选择菜单
    /// </summary>
    private void 显示辅助武器材料选择菜单(List<(int, int)> 材料列表)
    {
        if (道具数据 == null || 道具数据.材料列表 == null)
        {
            EditorUtility.DisplayDialog("错误", "无法加载道具数据", "确定");
            return;
        }
        
        GenericMenu menu = new GenericMenu();
        
        foreach (var 材料 in 道具数据.材料列表)
        {
            if (材料 != null)
            {
                string 显示名称 = string.IsNullOrEmpty(材料.名称) ? $"材料_{材料.id}" : 材料.名称;
                string 菜单路径 = $"{材料.小类}/{显示名称} (ID: {材料.id})";
                
                menu.AddItem(new GUIContent(菜单路径), false, () =>
                {
                    // 检查是否已存在相同材料
                    int 现有索引 = 材料列表.FindIndex(item => item.Item1 == 材料.id);
                    if (现有索引 >= 0)
                    {
                        // 如果已存在，增加数量
                        var 现有项 = 材料列表[现有索引];
                        材料列表[现有索引] = (现有项.Item1, 现有项.Item2 + 1);
                    }
                    else
                    {
                        // 如果不存在，添加新材料
                        材料列表.Add((材料.id, 1));
                    }
                    自动保存数据();
                    Repaint();
                });
            }
        }
        
        menu.ShowAsContext();
    }
    
    #endregion
}