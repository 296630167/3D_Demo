using UnityEngine;

public class 单一物体排序脚本 : 组合物体排序脚本
{
    [Tooltip("场景中每个格子的单位大小")]
    public float 格子单位大小 = 1.0f;
    
    [Tooltip("贴图占用的格子数量")]
    public float 占用格子数 = 1.0f;

    protected override void Start()
    {
        base.Start();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.SetFloat(Shader.PropertyToID("_SortingLayer"), 层级);
        }
    }
    public void 初始化(string 贴图路径, float 占用格子数, int 贴图类型 = 0)
    {
        this.占用格子数 = 占用格子数;
        更新贴图(贴图路径);
        switch (贴图类型)
        {
            case 0: // 地板
                transform.设置位置(new Vector3(0, 0, transform.localScale.y / 2f));
                break;
        }
        //transform.设置位置(位置);
        //自适应尺寸();
    }

    public void 更新贴图(string 贴图路径)
    {
        Texture2D 贴图 = 取.资源<Texture2D>($"图片/{贴图路径}");
        
        if (贴图 != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.SetTexture("_BaseMap", 贴图);
                renderer.material.SetFloat(Shader.PropertyToID("_SortingLayer"), 层级);
                自适应尺寸();
                
                //Debug.Log($"[单一物体排序脚本] 对象 '{gameObject.name}' 的贴图已更新为: {贴图路径}，尺寸: {贴图.width}x{贴图.height}");
            }
            else
            {
                //Debug.LogError($"[单一物体排序脚本] 无法找到Renderer组件或材质");
            }
        }
        else
        {
            Debug.LogError($"[单一物体排序脚本] 无法找到贴图: {贴图路径}，请确保贴图位于Resources/图片/文件夹中");
        }
    }
    
    public void 自适应尺寸()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null || renderer.material == null)
        {
            Debug.LogWarning("[单一物体排序脚本] 未找到Renderer组件或材质");
            return;
        }
        
        Texture 贴图 = renderer.material.GetTexture("_BaseMap");
        if (贴图 == null)
        {
            Debug.LogWarning("[单一物体排序脚本] 材质中未找到贴图");
            return;
        }
        
        int 贴图宽度 = 贴图.width;
        int 贴图高度 = 贴图.height;
        
        if (贴图宽度 <= 0 || 贴图高度 <= 0)
        {
            Debug.LogWarning($"[单一物体排序脚本] 贴图尺寸无效: {贴图宽度}x{贴图高度}");
            return;
        }
        
        float 宽高比 = (float)贴图宽度 / 贴图高度;
        float 目标宽度 = 占用格子数 * 格子单位大小;
        float 目标高度 = 目标宽度 / 宽高比;
        
        transform.localScale = new Vector3(目标宽度, 目标高度, 1);
        renderer.material.SetFloat(Shader.PropertyToID("_SortingLayer"), 层级);
        
        //Debug.Log($"[单一物体排序脚本] 贴图 {贴图.name} 尺寸: {贴图宽度}x{贴图.height}, 宽高比: {宽高比:F2}, 占用格子: {占用格子数}, 调整后大小: {目标宽度:F2}x{目标高度:F2}");
    }
}