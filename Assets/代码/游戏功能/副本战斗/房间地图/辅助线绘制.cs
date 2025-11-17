using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class 辅助线绘制 : 基
{
    [Header("网格设置")]
    public int 格子行 = 5;
    public int 格子列 = 5;

    [Header("线条设置")]
    public Color 线条颜色 = Color.black;
    public float 线条宽度 = 0.05f;
    public Material 线条材质;

    [Header("显示控制")]
    public bool 默认显示 = true;

    private List<GameObject> 线条对象列表 = new List<GameObject>();
    private GameObject 线条容器;

    [Header("参数")]
    public float 网格间距 = 0.5f;   // 网格线间距（原先固定0.5）
    public float 平面偏移 = -0.25f;  // 全局平移（用于把网格整体平移半格）
    public Vector3 绘制偏移 = new Vector3(-0.005f, 0.01f, -0.005f);

    [Header("填充区域设置")]
    public Material 填充材质;
    private Dictionary<string, GameObject> 填充区域对象字典 = new Dictionary<string, GameObject>(); // 按名称管理多个区域
    private Dictionary<string, GameObject> 填充圆形对象字典 = new Dictionary<string, GameObject>(); // 按名称管理多个圆形区域
    #region 显示控制
    public bool 是否显示 => 线条容器 && 线条容器.activeSelf;
    public void 切换显示() => 设置显示(!是否显示);
    public void 显示辅助线() => 设置显示(true);
    public void 隐藏辅助线() => 设置显示(false);

    private void 设置显示(bool 显示状态)
    {
        if (线条容器) 线条容器.SetActive(显示状态);
    }
    #endregion

    #region 网格绘制
    public void 初始化辅助线网格(Vector3 起点, int 行 = 5, int 列 = 5)
    {
        清理所有线条();
        确保容器存在();
        线条材质 ??= new Material(Shader.Find("Sprites/Default")) { color = 线条颜色 };

        float 水平偏移 = -行 * 网格间距 * 0.5f;
        float 垂直偏移 = -列 * 网格间距 * 0.5f;
        float 半宽 = 线条宽度 * 0.5f;

        for (int i = 0; i <= 行; i++)
            创建线条(
                new Vector3(起点.x - 列 * 网格间距 * 0.5f - 半宽 + 平面偏移, 起点.y, 起点.z + 垂直偏移 + i * 网格间距 + 平面偏移),
                new Vector3(起点.x + 列 * 网格间距 * 0.5f + 半宽 + 平面偏移, 起点.y, 起点.z + 垂直偏移 + i * 网格间距 + 平面偏移),
                $"水平线_{i}"
            );

        for (int i = 0; i <= 列; i++)
            创建线条(
                new Vector3(起点.x + 水平偏移 + i * 网格间距 + 平面偏移, 起点.y, 起点.z - 行 * 网格间距 * 0.5f - 半宽 + 平面偏移),
                new Vector3(起点.x + 水平偏移 + i * 网格间距 + 平面偏移, 起点.y, 起点.z + 行 * 网格间距 * 0.5f + 半宽 + 平面偏移),
                $"垂直线_{i}"
            );

        线条容器.SetActive(默认显示);
    }

    public void 初始化辅助线网格_中心(Vector3 起点, int 行 = 5, int 列 = 5) => 初始化辅助线网格(起点, 行, 列);

    public void 初始化辅助线网格_原点(int 行 = 5, int 列 = 5)
    {
        清理所有线条();
        确保容器存在();
        线条材质 ??= new Material(Shader.Find("Sprites/Default")) { color = 线条颜色 };

        float 半宽 = 线条宽度 * 0.5f;
        float y = 0f;

        for (int i = 0; i <= 行; i++)
            创建线条(new Vector3(0 - 半宽, y, i * 网格间距),
                    new Vector3(列 * 网格间距 + 半宽, y, i * 网格间距), $"水平线_{i}");

        for (int i = 0; i <= 列; i++)
            创建线条(new Vector3(i * 网格间距, y, 0 - 半宽),
                    new Vector3(i * 网格间距, y, 行 * 网格间距 + 半宽), $"垂直线_{i}");

        线条容器.SetActive(默认显示);
    }

    public void 重新绘制原点网格() => 初始化辅助线网格_原点(格子行, 格子列);
    public void 重新绘制网格() => 初始化辅助线网格(Vector3.zero, 格子行, 格子列);
    #endregion

    #region 线条工具
    private void 创建线条(Vector3 起点, Vector3 终点, string 名称)
    {
        var 线条对象 = new GameObject(名称) { transform = { parent = 线条容器.transform } };
        var 渲染器 = 线条对象.AddComponent<LineRenderer>();

        渲染器.material = 线条材质;
        渲染器.startColor = 渲染器.endColor = 线条颜色;
        渲染器.startWidth = 渲染器.endWidth = 线条宽度;
        渲染器.useWorldSpace = true;
        渲染器.positionCount = 2;
        渲染器.SetPositions(new Vector3[] { 起点, 终点 });

        线条对象列表.Add(线条对象);
    }

    public void 更新线条颜色(Color 新颜色)
    {
        线条颜色 = 新颜色;
        线条对象列表.ForEach(obj =>
        {
            if (obj?.GetComponent<LineRenderer>() is var 渲染器 && 渲染器)
                渲染器.startColor = 渲染器.endColor = 新颜色;
        });
        if (线条材质) 线条材质.color = 新颜色;
    }
    #endregion

    #region 填充矩形
    public void 绘制矩形区域(string 区域名称, Vector3 中心, float 宽 = 1.5f, float 高 = 1.5f, Color 颜色 = default, float 透明度 = 0.5f, int 层级 = 0)
    {
        // 如果区域名称已存在，先清理旧的
        if (填充区域对象字典.ContainsKey(区域名称))
        {
            取消绘制矩形区域(区域名称);
        }

        确保容器存在();

        var go = new GameObject($"矩形填充区域_{区域名称}") { transform = { parent = 线条容器.transform } };
        填充区域对象字典[区域名称] = go;

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();

        float 半宽 = 宽 * 0.5f;
        float 半高 = 高 * 0.5f;
        float y = 中心.y;

        var 网格 = new Mesh();
        网格.vertices = new Vector3[] {
            new Vector3(-半宽, 0, -半高),
            new Vector3(半宽, 0, -半高),
            new Vector3(-半宽, 0, 半高),
            new Vector3(半宽, 0, 半高),
        };
        网格.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        网格.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        网格.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mf.sharedMesh = 网格;

        if (颜色 == default) 颜色 = Color.white;
        颜色.a = 透明度;

        if (!填充材质)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (!shader) shader = Shader.Find("Sprites/Default");
            if (!shader) shader = Shader.Find("Unlit/Color");
            填充材质 = new Material(shader);
        }

        var 材质实例 = mr.material;
        材质实例.shader = 填充材质.shader;

        应用透明材质并上色(材质实例, 颜色);
        mr.sortingOrder = 层级;

        go.layer = gameObject.layer;
        go.transform.position = 中心;
        线条容器.SetActive(默认显示);
    }

    public void 取消绘制矩形区域(string 区域名称 = null)
    {
        if (区域名称 == null)
        {
            // 清理所有矩形区域
            foreach (var kvp in 填充区域对象字典.ToList())
            {
                if (kvp.Value)
                {
                    DestroyImmediate(kvp.Value);
                }
            }
            填充区域对象字典.Clear();
        }
        else
        {
            // 清理指定名称的区域
            if (填充区域对象字典.ContainsKey(区域名称))
            {
                if (填充区域对象字典[区域名称])
                {
                    DestroyImmediate(填充区域对象字典[区域名称]);
                }
                填充区域对象字典.Remove(区域名称);
            }
        }
    }

    public void 更新填充颜色(Color 新颜色, string 区域名称 = null)
    {
        if (区域名称 == null)
        {
            // 更新所有矩形区域的颜色
            foreach (var kvp in 填充区域对象字典)
            {
                if (kvp.Value && kvp.Value.TryGetComponent<MeshRenderer>(out var 渲染器) && 渲染器)
                {
                    应用透明材质并上色(渲染器.material, 新颜色);
                }
            }
            // 更新所有圆形区域的颜色
            foreach (var kvp in 填充圆形对象字典)
            {
                if (kvp.Value && kvp.Value.TryGetComponent<MeshRenderer>(out var 渲染器) && 渲染器)
                {
                    应用透明材质并上色(渲染器.material, 新颜色);
                }
            }
        }
        else
        {
            // 更新指定名称的区域颜色
            if (填充区域对象字典.ContainsKey(区域名称) && 填充区域对象字典[区域名称] &&
                填充区域对象字典[区域名称].TryGetComponent<MeshRenderer>(out var 矩形渲染器) && 矩形渲染器)
            {
                应用透明材质并上色(矩形渲染器.material, 新颜色);
            }
            if (填充圆形对象字典.ContainsKey(区域名称) && 填充圆形对象字典[区域名称] &&
                填充圆形对象字典[区域名称].TryGetComponent<MeshRenderer>(out var 圆形渲染器) && 圆形渲染器)
            {
                应用透明材质并上色(圆形渲染器.material, 新颜色);
            }
        }

        if (填充材质)
        {
            if (填充材质.HasProperty("_BaseColor")) 填充材质.SetColor("_BaseColor", 新颜色);
            else 填充材质.color = 新颜色;
        }
    }

    private void 应用透明材质并上色(Material 材质, Color 颜色)
    {
        材质.SetOverrideTag("RenderType", "Transparent");
        材质.EnableKeyword("_ALPHABLEND_ON");
        材质.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        材质.DisableKeyword("_ALPHATEST_ON");
        材质.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        材质.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        材质.SetInt("_ZWrite", 0);

        if (材质.HasProperty("_Surface")) 材质.SetFloat("_Surface", 1f);
        if (材质.HasProperty("_Blend")) 材质.SetFloat("_Blend", 0f);
        if (材质.HasProperty("_BaseColor")) 材质.SetColor("_BaseColor", 颜色);
        if (材质.HasProperty("_Color")) 材质.SetColor("_Color", 颜色);
        材质.color = 颜色;
    }

    public void 绘制圆形区域(string 区域名称, Vector3 中心, float 半径, Color 颜色 = default, float 透明度 = 0.5f, int 层级 = 0)
    {
        // 如果区域名称已存在，先清理旧的
        if (填充圆形对象字典.ContainsKey(区域名称))
        {
            取消绘制圆形区域(区域名称);
        }

        确保容器存在();

        var go = new GameObject($"圆形填充区域_{区域名称}") { transform = { parent = 线条容器.transform } };
        填充圆形对象字典[区域名称] = go;

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();

        float y = 0f;
        int 分段数 = 32; // 圆形分段数，保证平滑度

        var 网格 = new Mesh();

        // 创建顶点数组：中心点 + 圆周上的点
        var 顶点列表 = new List<Vector3>();
        var 三角形列表 = new List<int>();
        var 法线列表 = new List<Vector3>();
        var UV列表 = new List<Vector2>();

        // 添加中心点（本地坐标）
        顶点列表.Add(new Vector3(0f, y, 0f));
        法线列表.Add(Vector3.up);
        UV列表.Add(new Vector2(0.5f, 0.5f));

        // 添加圆周上的点（本地坐标）
        for (int i = 0; i <= 分段数; i++)
        {
            float 角度 = (float)i / 分段数 * 2f * Mathf.PI;
            float x = Mathf.Cos(角度) * 半径;
            float z = Mathf.Sin(角度) * 半径;

            顶点列表.Add(new Vector3(x, y, z));
            法线列表.Add(Vector3.up);

            // UV坐标映射到圆形
            float u = 0.5f + Mathf.Cos(角度) * 0.5f;
            float v = 0.5f + Mathf.Sin(角度) * 0.5f;
            UV列表.Add(new Vector2(u, v));
        }

        // 创建三角形
        for (int i = 1; i <= 分段数; i++)
        {
            三角形列表.Add(0);      // 中心点
            三角形列表.Add(i);      // 当前圆周点
            三角形列表.Add(i + 1);  // 下一个圆周点
        }

        网格.vertices = 顶点列表.ToArray();
        网格.triangles = 三角形列表.ToArray();
        网格.normals = 法线列表.ToArray();
        网格.uv = UV列表.ToArray();
        mf.sharedMesh = 网格;

        if (颜色 == default) 颜色 = Color.white;
        颜色.a = 透明度;

        if (!填充材质)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (!shader) shader = Shader.Find("Sprites/Default");
            if (!shader) shader = Shader.Find("Unlit/Color");
            填充材质 = new Material(shader);
        }

        var 材质实例 = mr.material;
        材质实例.shader = 填充材质.shader;

        应用透明材质并上色(材质实例, 颜色);
        mr.sortingOrder = 层级;

        go.layer = gameObject.layer;
        go.transform.position = 中心;
        线条容器.SetActive(默认显示);
    }

    public void 取消绘制圆形区域(string 区域名称 = null)
    {
        if (区域名称 == null)
        {
            // 清理所有圆形区域
            foreach (var kvp in 填充圆形对象字典.ToList())
            {
                if (kvp.Value)
                {
                    DestroyImmediate(kvp.Value);
                }
            }
            填充圆形对象字典.Clear();
        }
        else
        {
            // 清理指定名称的圆形区域
            if (填充圆形对象字典.ContainsKey(区域名称))
            {
                if (填充圆形对象字典[区域名称])
                {
                    DestroyImmediate(填充圆形对象字典[区域名称]);
                }
                填充圆形对象字典.Remove(区域名称);
            }
        }
    }
    #endregion
    #region 清理与生命周期
    public void 清理所有线条()
    {
        线条对象列表.ForEach(obj => { if (obj) DestroyImmediate(obj); });
        线条对象列表.Clear();
        取消绘制矩形区域(); // 清理所有矩形区域
        取消绘制圆形区域(); // 清理所有圆形区域
        if (线条容器) DestroyImmediate(线条容器);
    }

    private void 确保容器存在()
    {
        if (!线条容器)
            线条容器 = new GameObject("辅助线网格容器") { transform = { parent = transform } };
    }
    protected override void 销毁时() => 清理所有线条();
    #endregion

    public void 绘制移动范围区域(副本单位脚本 单位)
    {
        const string 区域名称 = "移动范围_合批";
        if (单位 == null || 单位.可移动范围格子哈希集 == null || 单位.可移动范围格子哈希集.Count == 0)
        {
            取消绘制矩形区域(区域名称);
            return;
        }

        if (填充区域对象字典.ContainsKey(区域名称))
        {
            取消绘制矩形区域(区域名称);
        }

        确保容器存在();

        var go = new GameObject($"矩形填充区域_{区域名称}") { transform = { parent = 线条容器.transform } };
        填充区域对象字典[区域名称] = go;

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();

        var 顶点列表 = new List<Vector3>();
        var 三角形列表 = new List<int>();
        var 法线列表 = new List<Vector3>();
        var UV列表 = new List<Vector2>();

        float 宽 = 0.5f;
        float 高 = 0.5f;
        float 半宽 = 宽 * 0.5f;
        float 半高 = 高 * 0.5f;

        int 顶点起始索引 = 0;
        foreach (var 格子 in 单位.可移动范围格子哈希集)
        {
            var c = 格子.场景坐标 + new Vector3(-0.005f, 0.01f, -0.005f);
            float y = c.y;

            顶点列表.Add(new Vector3(c.x - 半宽, y, c.z - 半高));
            顶点列表.Add(new Vector3(c.x + 半宽, y, c.z - 半高));
            顶点列表.Add(new Vector3(c.x - 半宽, y, c.z + 半高));
            顶点列表.Add(new Vector3(c.x + 半宽, y, c.z + 半高));

            法线列表.Add(Vector3.up);
            法线列表.Add(Vector3.up);
            法线列表.Add(Vector3.up);
            法线列表.Add(Vector3.up);

            UV列表.Add(new Vector2(0, 0));
            UV列表.Add(new Vector2(1, 0));
            UV列表.Add(new Vector2(0, 1));
            UV列表.Add(new Vector2(1, 1));

            三角形列表.Add(顶点起始索引 + 0);
            三角形列表.Add(顶点起始索引 + 2);
            三角形列表.Add(顶点起始索引 + 1);
            三角形列表.Add(顶点起始索引 + 2);
            三角形列表.Add(顶点起始索引 + 3);
            三角形列表.Add(顶点起始索引 + 1);

            顶点起始索引 += 4;
        }

        var 网格 = new Mesh();
        网格.indexFormat = 顶点列表.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
        网格.SetVertices(顶点列表);
        网格.SetTriangles(三角形列表, 0);
        网格.SetNormals(法线列表);
        网格.SetUVs(0, UV列表);

        mf.sharedMesh = 网格;

        if (!填充材质)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (!shader) shader = Shader.Find("Sprites/Default");
            if (!shader) shader = Shader.Find("Unlit/Color");
            填充材质 = new Material(shader);
        }

        var 材质实例 = mr.material;
        材质实例.shader = 填充材质.shader;

        var 颜色 = Color.white;
        颜色.a = 0.5f;
        应用透明材质并上色(材质实例, 颜色);

        mr.sortingOrder = 0; // 移动区域层级=0

        go.layer = gameObject.layer;
        go.transform.position = Vector3.zero;
        线条容器.SetActive(默认显示);
    }
    public void 绘制移动目标区域(Vector3 v3, bool 显示 = true)
    {
        if (显示)
        {
            绘制矩形区域("移动目标", v3 + new Vector3(-0.005f, 0.01f, -0.005f), 1.5f, 1.5f, Color.green, 0.5f, 10);
        }
        else
        {
            取消绘制矩形区域("移动目标");
        }
    }

    public void 绘制移动路径(副本玩家单位 单位 = null)
    {
        if (单位 == null || 单位.移动路径列表 == null || 单位.移动路径列表.Count == 0)
        {
            取消绘制线("移动路径");
            return;
        }

        确保容器存在();
        取消绘制线("移动路径");

        if (!线条材质)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (!shader) shader = Shader.Find("Sprites/Default");
            if (!shader) shader = Shader.Find("Unlit/Color");
            线条材质 = new Material(shader) { color = 线条颜色 };
        }

        var 路径线条对象 = new GameObject("移动路径") { transform = { parent = 线条容器.transform } };
        var 渲染器 = 路径线条对象.AddComponent<LineRenderer>();
        渲染器.material = 线条材质;
        渲染器.startColor = 线条颜色;
        渲染器.endColor = 线条颜色;
        渲染器.startWidth = 线条宽度;
        渲染器.endWidth = 线条宽度;
        渲染器.useWorldSpace = true;
        渲染器.numCornerVertices = 2;
        渲染器.numCapVertices = 2;
        渲染器.sortingOrder = 5; // 路径在线条层级高于移动范围

        渲染器.positionCount = 单位.移动路径列表.Count;
        渲染器.SetPositions(单位.移动路径列表.Select(x => x.场景坐标 + 绘制偏移).ToArray());

        线条容器.SetActive(默认显示);
    }

    public void 取消绘制线(string 名称 = null)
    {
        if (!线条容器) return;
        if (string.IsNullOrEmpty(名称)) return;
        var 现有 = 线条容器.transform.Find(名称);
        if (现有) DestroyImmediate(现有.gameObject);
    }

    public void 绘制技能攻击范围(Vector3 坐标,技能类 技能)
    {
        // 使用 绘制圆形区域 方法 大小为 技能的射程属性
        绘制圆形区域("技能攻击范围", 坐标 + 绘制偏移, 技能.射程 * 1.5f + 0.5f, Color.gray, 0.5f, 10);
    }
    public void 取消绘制技能攻击范围() => 取消绘制圆形区域("技能攻击范围");

    public void 绘制技能选择单位区域(Vector3 场景坐标, Color 颜色)
    {
        绘制矩形区域("技能选择单位", 场景坐标 + 绘制偏移, 1.5f, 1.5f, 颜色, 0.5f, 10);
    }
    public void 取消绘制技能选择单位区域() => 取消绘制矩形区域("技能选择单位");
}