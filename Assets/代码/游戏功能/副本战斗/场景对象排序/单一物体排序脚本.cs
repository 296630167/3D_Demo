using UnityEngine;

public class 单一物体排序脚本 : 组合物体排序脚本
{
    [Tooltip("场景中每个格子的单位大小")]
    public float 格子单位大小 = 1.0f;
    
    [Tooltip("贴图占用的格子数量")]
    public float 占用格子数 = 1.0f;
    
    // 记录当前的Renderer组件
    private Renderer 当前渲染器;

    protected override void Start()
    {
        base.Start();
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
        // 默认x旋转90度
        transform.设置旋转(new Vector3(90, 0, 0)); // 默认x旋转90度
        //设置Z层级为当前的z坐标
        //设置层级(transform.position.z);
        //transform.设置位置(位置);
        //自适应尺寸();
    }

    public void 更新贴图(string 贴图路径)
    {
        // print($"图片/{贴图路径}");
        Texture2D 贴图 = 取.资源<Texture2D>($"图片/{贴图路径}");
        
        if (贴图 != null)
        {
            // 获取并记录Renderer
            当前渲染器 = GetComponent<Renderer>();
            if (当前渲染器 != null && 当前渲染器.material != null)
            {
                当前渲染器.material.SetTexture("_BaseMap", 贴图);
                // 应用当前层级
                当前渲染器.material.SetFloat(Shader.PropertyToID("_SortingLayer"), 层级);
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
            Debug.LogError($"[单一物体排序脚本] 无法找到贴图: {贴图路径},请确保贴图位于Resources/图片/文件夹中");
        }
    }
    
    // 重写父类的设置层级方法
    public new void 设置层级(float 新层级)
    {
        层级 = 新层级;
        
        // 如果有记录的Renderer,直接更新其材质
        if (当前渲染器 != null && 当前渲染器.material != null)
        {
            当前渲染器.material.SetFloat(Shader.PropertyToID("_SortingLayer"), 层级);
            //Debug.Log($"[单一物体排序脚本] 对象 '{gameObject.name}' 的层级已更新为: {层级}");
        }
        else
        {
            // 如果没有记录的Renderer,回退到父类方法
            base.设置层级(新层级);
        }
    }
    
    public void 自适应尺寸()
    {
        if (当前渲染器 == null)
        {
            当前渲染器 = GetComponent<Renderer>();
        }
        
        if (当前渲染器 == null || 当前渲染器.material == null)
        {
            Debug.LogWarning("[单一物体排序脚本] 未找到Renderer组件或材质");
            return;
        }
        
        Texture 贴图 = 当前渲染器.material.GetTexture("_BaseMap");
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
        
        //Debug.Log($"[单一物体排序脚本] 贴图 {贴图.name} 尺寸: {贴图宽度}x{贴图.height}, 宽高比: {宽高比:F2}, 占用格子: {占用格子数}, 调整后大小: {目标宽度:F2}x{目标高度:F2}");
    }
}