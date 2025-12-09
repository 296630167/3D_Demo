using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 物体排序脚本 - 用于控制物体的渲染层级排序
/// 将此脚本挂载到需要控制渲染顺序的物体上
/// 脚本会自动遍历当前对象及所有子对象，为所有包含Renderer的对象应用排序着色器
/// </summary>
public class 物体排序脚本 : MonoBehaviour
{
    [Header("排序设置")]
    [Tooltip("排序层级值，数值越大渲染越靠后（越在上层显示），数值越小渲染越靠前（越在下层显示）")]
    public float 层级 = 0;

    // 着色器属性ID，用于高效设置材质参数
    private static readonly int SortingLayerPropertyID = Shader.PropertyToID("_SortingLayer");
    
    // 排序着色器的名称
    private const string SortingShaderName = "排序测试/物体排序着色器";
    
    // 存储所有需要控制的材质实例列表
    private List<Material> 材质列表 = new List<Material>();
    
    // 缓存着色器引用
    private Shader 排序着色器;
    
    // 记录上一次的层级值，用于检测变化
    private float 上次层级值;

    private void Awake()
    {
        初始化材质();
    }

    private void Start()
    {
        // 初始化时应用层级值
        应用层级到所有材质();
        上次层级值 = 层级;
    }

    private void Update()
    {
        // 检测层级值是否发生变化，如果变化则更新所有材质
        if (!Mathf.Approximately(层级, 上次层级值))
        {
            应用层级到所有材质();
            上次层级值 = 层级;
        }
    }

    /// <summary>
    /// 初始化材质 - 遍历所有子对象，为每个Renderer创建独立的材质实例并应用排序着色器
    /// </summary>
    private void 初始化材质()
    {
        // 加载排序着色器
        排序着色器 = Shader.Find(SortingShaderName);
        
        if (排序着色器 == null)
        {
            Debug.LogError($"[物体排序脚本] 无法找到着色器: {SortingShaderName}，请确保着色器已正确导入！");
            return;
        }

        // 获取当前对象及所有子对象上的Renderer组件
        Renderer[] 所有渲染器 = GetComponentsInChildren<Renderer>(true);
        
        foreach (Renderer 渲染器 in 所有渲染器)
        {
            if (渲染器 == null) continue;
            
            // 获取渲染器上的所有材质
            Material[] 原始材质数组 = 渲染器.sharedMaterials;
            Material[] 新材质数组 = new Material[原始材质数组.Length];
            
            for (int i = 0; i < 原始材质数组.Length; i++)
            {
                Material 原始材质 = 原始材质数组[i];
                
                if (原始材质 == null)
                {
                    新材质数组[i] = null;
                    continue;
                }
                
                // 创建新的材质实例，确保每个材质独立控制
                Material 新材质 = new Material(排序着色器);
                新材质.name = $"{原始材质.name}_排序实例";
                
                // 复制原始材质的贴图和颜色属性
                复制材质属性(原始材质, 新材质);
                
                新材质数组[i] = 新材质;
                材质列表.Add(新材质);
            }
            
            // 应用新的材质数组到渲染器
            渲染器.materials = 新材质数组;
        }
        
        if (材质列表.Count == 0)
        {
            Debug.LogWarning($"[物体排序脚本] 在对象 '{gameObject.name}' 及其子对象中未找到任何Renderer组件！");
        }
        else
        {
            Debug.Log($"[物体排序脚本] 成功初始化 {材质列表.Count} 个材质实例");
        }
    }

    /// <summary>
    /// 复制原始材质的关键属性到新材质
    /// </summary>
    private void 复制材质属性(Material 原始材质, Material 新材质)
    {
        // 复制主贴图
        if (原始材质.HasProperty("_MainTex"))
        {
            新材质.SetTexture("_BaseMap", 原始材质.GetTexture("_MainTex"));
        }
        else if (原始材质.HasProperty("_BaseMap"))
        {
            新材质.SetTexture("_BaseMap", 原始材质.GetTexture("_BaseMap"));
        }
        
        // 复制主颜色
        if (原始材质.HasProperty("_Color"))
        {
            新材质.SetColor("_BaseColor", 原始材质.GetColor("_Color"));
        }
        else if (原始材质.HasProperty("_BaseColor"))
        {
            新材质.SetColor("_BaseColor", 原始材质.GetColor("_BaseColor"));
        }
        
        // 复制贴图缩放和偏移
        if (原始材质.HasProperty("_MainTex"))
        {
            新材质.SetTextureScale("_BaseMap", 原始材质.GetTextureScale("_MainTex"));
            新材质.SetTextureOffset("_BaseMap", 原始材质.GetTextureOffset("_MainTex"));
        }
        else if (原始材质.HasProperty("_BaseMap"))
        {
            新材质.SetTextureScale("_BaseMap", 原始材质.GetTextureScale("_BaseMap"));
            新材质.SetTextureOffset("_BaseMap", 原始材质.GetTextureOffset("_BaseMap"));
        }
        
        // 复制Alpha裁切值
        if (原始材质.HasProperty("_Cutoff"))
        {
            新材质.SetFloat("_Cutoff", 原始材质.GetFloat("_Cutoff"));
        }
    }

    /// <summary>
    /// 将当前层级值应用到所有材质实例
    /// </summary>
    private void 应用层级到所有材质()
    {
        foreach (Material 材质 in 材质列表)
        {
            if (材质 != null)
            {
                材质.SetFloat(SortingLayerPropertyID, 层级);
            }
        }
        
        // 添加调试日志
        Debug.Log($"[物体排序脚本] 对象 '{gameObject.name}' 的层级已更新为: {层级}");
    }

    /// <summary>
    /// 公开方法：设置排序层级
    /// </summary>
    /// <param name="新层级">新的层级值</param>
    public void 设置层级(float 新层级)
    {
        层级 = 新层级;
        应用层级到所有材质();
        上次层级值 = 层级;
    }

    /// <summary>
    /// 公开方法：获取当前排序层级
    /// </summary>
    /// <returns>当前层级值</returns>
    public float 获取层级()
    {
        return 层级;
    }

    private void OnDestroy()
    {
        // 清理创建的材质实例，避免内存泄漏
        foreach (Material 材质 in 材质列表)
        {
            if (材质 != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(材质);
                }
                else
                {
                    DestroyImmediate(材质);
                }
            }
        }
        材质列表.Clear();
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器模式下，当Inspector中的值改变时更新材质
    /// </summary>
    private void OnValidate()
    {
        // 仅在运行时且材质列表已初始化时更新
        if (Application.isPlaying && 材质列表 != null && 材质列表.Count > 0)
        {
            应用层级到所有材质();
            上次层级值 = 层级;
        }
    }
#endif
}