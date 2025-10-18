using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 路线绘制器 : Graphic
{
    public enum 坐标原点类型 { 画布中心, 左下角, 左上角 }
    
    [Header("线条配置")]
    [SerializeField] private float 线条宽度 = 2f;
    [SerializeField] private Color 线条颜色 = Color.yellow;
    [SerializeField] private bool 使用渐变色 = false;
    [SerializeField] private Color 起点颜色 = Color.green;
    [SerializeField] private Color 终点颜色 = Color.red;
    [Header("坐标配置")]
    [SerializeField] private 坐标原点类型 坐标原点 = 坐标原点类型.左下角;
    [Header("路径数据")]
    private List<Vector2> 路径点列表 = new List<Vector2>();
    private List<连线数据> 连线列表 = new List<连线数据>();

    [System.Serializable]
    public struct 连线数据
    {
        public Vector2 起点;
        public Vector2 终点;
        public Color 颜色;
        public float 宽度;

        public 连线数据(Vector2 起点, Vector2 终点, Color 颜色, float 宽度)
        {
            this.起点 = 起点;
            this.终点 = 终点;
            this.颜色 = 颜色;
            this.宽度 = 宽度;
        }
    }

    public void 设置路径点(List<Vector2> 路径点)
    {
        路径点列表.Clear();
        路径点列表.AddRange(路径点);
        生成连线数据();
        SetVerticesDirty();
    }

    public void 添加连线(Vector2 起点, Vector2 终点, Color? 颜色 = null, float? 宽度 = null)
    {
        连线数据 新连线 = new 连线数据(起点, 终点, 颜色 ?? 线条颜色, 宽度 ?? 线条宽度);
        连线列表.Add(新连线);
        SetVerticesDirty();
    }

    public void 清除连线()
    {
        路径点列表.Clear();
        连线列表.Clear();
        SetVerticesDirty();
    }

    private void 生成连线数据()
    {
        连线列表.Clear();
        if (路径点列表.Count < 2) return;
        for (int i = 0; i < 路径点列表.Count - 1; i++)
        {
            Color 当前颜色 = 线条颜色;
            if (使用渐变色)
            {
                float 进度 = (float)i / (路径点列表.Count - 2);
                当前颜色 = Color.Lerp(起点颜色, 终点颜色, 进度);
            }
            连线数据 新连线 = new 连线数据(路径点列表[i], 路径点列表[i + 1], 当前颜色, 线条宽度);
            连线列表.Add(新连线);
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (连线列表.Count == 0) return;
        Rect 画布区域 = rectTransform.rect;
        foreach (var 连线 in 连线列表)
        {
            绘制单条连线(vh, 连线, 画布区域);
        }
    }

    private void 绘制单条连线(VertexHelper vh, 连线数据 连线, Rect 画布区域)
    {
        Vector2 起点UI = 转换坐标(连线.起点, 画布区域);
        Vector2 终点UI = 转换坐标(连线.终点, 画布区域);
        Vector2 方向 = (终点UI - 起点UI).normalized;
        Vector2 垂直方向 = new Vector2(-方向.y, 方向.x);
        float 半宽 = 连线.宽度 * 0.5f;
        Vector2 顶点1 = 起点UI + 垂直方向 * 半宽;
        Vector2 顶点2 = 起点UI - 垂直方向 * 半宽;
        Vector2 顶点3 = 终点UI - 垂直方向 * 半宽;
        Vector2 顶点4 = 终点UI + 垂直方向 * 半宽;
        UIVertex 顶点 = UIVertex.simpleVert;
        顶点.color = 连线.颜色;
        int 起始索引 = vh.currentVertCount;
        顶点.position = 顶点1;
        vh.AddVert(顶点);
        顶点.position = 顶点2;
        vh.AddVert(顶点);
        顶点.position = 顶点3;
        vh.AddVert(顶点);
        顶点.position = 顶点4;
        vh.AddVert(顶点);
        vh.AddTriangle(起始索引, 起始索引 + 1, 起始索引 + 2);
        vh.AddTriangle(起始索引 + 2, 起始索引 + 3, 起始索引);
    }

    public void 设置线条样式(float 宽度, Color 颜色)
    {
        线条宽度 = 宽度;
        线条颜色 = 颜色;
        SetVerticesDirty();
    }

    public void 设置渐变色(Color 起点颜色, Color 终点颜色)
    {
        使用渐变色 = true;
        this.起点颜色 = 起点颜色;
        this.终点颜色 = 终点颜色;
        生成连线数据();
        SetVerticesDirty();
    }

    public void 设置坐标原点(坐标原点类型 原点类型)
    {
        坐标原点 = 原点类型;
        SetVerticesDirty();
    }

    private Vector2 转换坐标(Vector2 原坐标, Rect 画布区域)
    {
        switch (坐标原点)
        {
            case 坐标原点类型.左下角:
                return 原坐标 + new Vector2(-画布区域.width / 2, -画布区域.height / 2);
            case 坐标原点类型.左上角:
                return new Vector2(原坐标.x - 画布区域.width / 2, -原坐标.y + 画布区域.height / 2);
            case 坐标原点类型.画布中心:
            default:
                return 原坐标;
        }
    }
}
