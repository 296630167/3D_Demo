using UnityEngine;
using System.Collections.Generic;

public class 组合物体排序脚本 : MonoBehaviour
{
    [Header("排序设置")]
    [Tooltip("排序层级值，数值越大渲染越靠后（越在上层显示），数值越小渲染越靠前（越在下层显示）")]
    public float 层级 = 0;

    private static readonly int SortingLayerPropertyID = Shader.PropertyToID("_SortingLayer");
    private const string SortingShaderName = "排序测试/物体排序着色器";
    private List<Material> 材质列表 = new List<Material>();
    private Shader 排序着色器;
    private float 上次层级值;

    private void Awake()
    {
        初始化材质();
    }

    protected virtual void Start()
    {
        应用层级到所有材质();
        上次层级值 = 层级;
    }

    private void Update()
    {
        if (!Mathf.Approximately(层级, 上次层级值))
        {
            应用层级到所有材质();
            上次层级值 = 层级;
        }
    }

    private void 初始化材质()
    {
        排序着色器 = Shader.Find(SortingShaderName);
        
        if (排序着色器 == null)
        {
            Debug.LogError($"[组合物体排序脚本] 无法找到着色器: {SortingShaderName}，请确保着色器已正确导入！");
            return;
        }

        Renderer[] 所有渲染器 = GetComponentsInChildren<Renderer>(true);
        
        foreach (Renderer 渲染器 in 所有渲染器)
        {
            if (渲染器 == null) continue;
            
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
                
                Material 新材质 = new Material(排序着色器);
                新材质.name = $"{原始材质.name}_排序实例";
                
                复制材质属性(原始材质, 新材质);
                
                新材质数组[i] = 新材质;
                材质列表.Add(新材质);
            }
            
            渲染器.materials = 新材质数组;
        }
        
        if (材质列表.Count == 0)
        {
            //Debug.LogWarning($"[组合物体排序脚本] 在对象 '{gameObject.name}' 及其子对象中未找到任何Renderer组件！");
        }
        else
        {
            //Debug.Log($"[组合物体排序脚本] 成功初始化 {材质列表.Count} 个材质实例");
        }
    }

    private void 复制材质属性(Material 原始材质, Material 新材质)
    {
        if (原始材质.HasProperty("_MainTex"))
        {
            新材质.SetTexture("_BaseMap", 原始材质.GetTexture("_MainTex"));
        }
        else if (原始材质.HasProperty("_BaseMap"))
        {
            新材质.SetTexture("_BaseMap", 原始材质.GetTexture("_BaseMap"));
        }
        
        if (原始材质.HasProperty("_Color"))
        {
            新材质.SetColor("_BaseColor", 原始材质.GetColor("_Color"));
        }
        else if (原始材质.HasProperty("_BaseColor"))
        {
            新材质.SetColor("_BaseColor", 原始材质.GetColor("_BaseColor"));
        }
        
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
        
        if (原始材质.HasProperty("_Cutoff"))
        {
            新材质.SetFloat("_Cutoff", 原始材质.GetFloat("_Cutoff"));
        }
    }

    private void 应用层级到所有材质()
    {
        foreach (Material 材质 in 材质列表)
        {
            if (材质 != null)
            {
                材质.SetFloat(SortingLayerPropertyID, 层级);
            }
        }
        
        //Debug.Log($"[组合物体排序脚本] 对象 '{gameObject.name}' 的层级已更新为: {层级}");
    }

    public void 设置层级(float 新层级)
    {
        层级 = 新层级;
        应用层级到所有材质();
        上次层级值 = 层级;
    }

    public float 获取层级()
    {
        return 层级;
    }

    private void OnDestroy()
    {
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
}