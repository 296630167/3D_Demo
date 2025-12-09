using UnityEngine;

[ExecuteAlways]
public class 渲染排序层级 : MonoBehaviour
{
    public float 层级 = 0.5f;
    [SerializeField] float 归一化层级预览;
    public bool 应用到子对象 = true;
    public bool 自动替换着色器 = true;
    public string 手动排序着色器 = "测试/ZSortShader_Manual";
    private Renderer[] 渲染器数组;
    private bool 已初始化;
    
    void 初始化()
    {
        渲染器数组 = 应用到子对象 ? GetComponentsInChildren<Renderer>(true) : new Renderer[] { GetComponent<Renderer>() };
        已初始化 = true;
    }
    
    private void Update()
    {
        // 注释掉这行，避免层级值被强制设置为position.z
        // 层级 = transform.position.z;
        归一化层级预览 = 计算归一化(层级);
        应用();
    }
    
    public void 设置层级(float 值)
    {
        层级 = 值;
        归一化层级预览 = 计算归一化(层级);
        应用();
    }
    
    void 应用()
    {
        if (!已初始化) 初始化();
        if (渲染器数组 == null) return;
        var 着色器 = Shader.Find(手动排序着色器);
        // 不再进行归一化，直接使用原始值以保持精度
        float 显示值 = 层级;
        归一化层级预览 = 显示值;
        for (int i = 0; i < 渲染器数组.Length; i++)
        {
            var r = 渲染器数组[i];
            if (r == null) continue;
            var 材质集 = r.materials;
            if (材质集 == null) continue;
            for (int m = 0; m < 材质集.Length; m++)
            {
                var mat = 材质集[m];
                if (mat == null) continue;
                if (!mat.HasProperty("_ManualOrder") && 自动替换着色器 && 着色器 != null)
                {
                    var 新 = new Material(mat);
                    新.shader = 着色器;
                    if (mat.HasProperty("_BaseMap")) 新.SetTexture("_BaseMap", mat.GetTexture("_BaseMap"));
                    else if (mat.HasProperty("_MainTex")) 新.SetTexture("_BaseMap", mat.GetTexture("_MainTex"));
                    if (mat.HasProperty("_BaseColor")) 新.SetColor("_BaseColor", mat.GetColor("_BaseColor"));
                    else if (mat.HasProperty("_Color")) 新.SetColor("_BaseColor", mat.GetColor("_Color"));
                    材质集[m] = 新;
                    mat = 新;
                }
                if (mat.HasProperty("_ManualOrder")) mat.SetFloat("_ManualOrder", 显示值);
                // 添加_ZDepthScale属性设置，确保使用合适的精度因子
                if (mat.HasProperty("_ZDepthScale")) mat.SetFloat("_ZDepthScale", 0.0001f);
                if (mat.HasProperty("_GuiYiHuaEnable")) mat.SetFloat("_GuiYiHuaEnable", 0f);
            }
            r.materials = 材质集;
        }
    }
    
    // 移除归一化，直接返回原始值
    float 计算归一化(float 值) => 值/10000f;
    void OnEnable() { 归一化层级预览 = 计算归一化(层级); 应用(); }
    void OnValidate() { 归一化层级预览 = 计算归一化(层级); 应用(); }
}