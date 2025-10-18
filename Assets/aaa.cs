using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class aaa : 面板基类
{
    public 副本房间_基类 入口;
    public 副本房间_基类 出口;
    public 副本房间_基类[,] 副本房间数组;
    Image[,] 小地图格子数组;
    public List<副本房间_基类> 主路径房间列表;
    路线绘制器 路线绘制器;
    protected override void 开始时()
    {
        副本房间数组 = new 副本房间_基类[5, 5];
        小地图格子数组 = new Image[5, 5];
        路线绘制器 = 组件<路线绘制器>("路线绘制");
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Transform 格子 = 变换("小地图容器").GetChild(x * 5 + y);
                小地图格子数组[x, y] = 格子.GetChild(0).图片();
            }
        }
        for (int 行 = 0; 行 < 5; 行++)
        {
            for (int 列 = 0; 列 < 5; 列++)
            {
                副本房间数组[行, 列] = new 副本房间_基类(行, 列);
            }
        }
        StartCoroutine(流程());
    }
    IEnumerator 流程()
    {
        bool 选择行边 = this.随机数(0, 1) == 0;
        bool 入口在第一边 = this.随机数(0, 1) == 0;
        var (入口位置, 出口位置) = 生成相对边位置(选择行边, 入口在第一边);
        入口 = 副本房间数组[入口位置.x, 入口位置.y];
        入口.房间类型 = 副本房间类型.入口;
        出口 = 副本房间数组[出口位置.x, 出口位置.y];
        出口.房间类型 = 副本房间类型.出口;
        颜色(入口.行, 入口.列, Color.green);
        yield return new WaitForSeconds(0.5f);
        颜色(出口.行, 出口.列, Color.red);
        yield return new WaitForSeconds(0.5f);
        主路径房间列表 = new List<副本房间_基类>();
        yield return StartCoroutine(生成最短路径(入口, 出口));
        yield return StartCoroutine(生成延长路径(15));
    }
    IEnumerator 生成最短路径(副本房间_基类 起点, 副本房间_基类 终点)
    {
        主路径房间列表.Clear();
        
        int 当前行 = 起点.行;
        int 当前列 = 起点.列;
        int 目标行 = 终点.行;
        int 目标列 = 终点.列;
        
        Debug.Log($"生成最短路径：从({当前行}, {当前列})到({目标行}, {目标列})");
        
        副本房间_基类 当前房间 = 副本房间数组[当前行, 当前列];
        当前房间.路径类型 = 副本房间路径类型.主路;
        主路径房间列表.Add(当前房间);
        
        while (当前行 != 目标行 || 当前列 != 目标列)
        {
            if (当前行 != 目标行)
            {
                当前行 += 当前行 < 目标行 ? 1 : -1;
            }
            else if (当前列 != 目标列)
            {
                当前列 += 当前列 < 目标列 ? 1 : -1;
            }
            
            当前房间 = 副本房间数组[当前行, 当前列];
            当前房间.路径类型 = 副本房间路径类型.主路;
            主路径房间列表.Add(当前房间);
            
            if (当前行 != 目标行 || 当前列 != 目标列)
            {
                颜色(当前行, 当前列, Color.yellow);
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
        Debug.Log($"最短路径生成完成，总步数：{主路径房间列表.Count}");
    }
    IEnumerator 生成延长路径(int 最小路径长度)
    {
        while(主路径房间列表.Count < 最小路径长度)
        {
            yield return StartCoroutine(尝试扩展路径(4));
            yield return StartCoroutine(尝试扩展路径(3));
            
            var 可扩展格子列表 = 寻找可扩展格子(2);
            if(可扩展格子列表.Count == 0) break;
            
            var 随机格子 = 可扩展格子列表.随机返回();
            应用路径扩展(随机格子);
            yield return StartCoroutine(显示扩展动画(随机格子));
        }
        yield return new WaitForSeconds(0.5f);
        副本房间_基类 房间 = null;
        print(入口.行 + " " + 入口.列);
        print(主路径房间列表[0].行 + " " + 主路径房间列表[0].列);
        print(主路径房间列表[1].行 + " " + 主路径房间列表[1].列);
        foreach(var r in 主路径房间列表)
        {
            if(房间!=null)
            {
                添加连线(格子坐标(房间.行, 房间.列), 格子坐标(r.行, r.列));
            }
            颜色(r.行, r.列, Color.blue);
            房间 = r;
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    private struct 可扩展格子
    {
        public List<副本房间_基类> 主路径格子;
        public List<副本房间_基类> 扩展格子;
        public List<副本房间_基类> 移除格子;
        public int 插入索引;
        
        public 可扩展格子(List<副本房间_基类> 主路径格子, List<副本房间_基类> 扩展格子, List<副本房间_基类> 移除格子, int 插入索引)
        {
            this.主路径格子 = 主路径格子;
            this.扩展格子 = 扩展格子;
            this.移除格子 = 移除格子;
            this.插入索引 = 插入索引;
        }
    }

    private IEnumerator 尝试扩展路径(int 连接数)
    {
        var 可扩展格子列表 = 寻找可扩展格子(连接数);
        if (可扩展格子列表.Count == 0) yield break;
        
        var 随机格子 = 可扩展格子列表.随机返回();
        应用路径扩展(随机格子);
        yield return StartCoroutine(显示扩展动画(随机格子));
    }

    private void 应用路径扩展(可扩展格子 格子)
    {
        foreach (var 扩展房间 in 格子.扩展格子)
        {
            扩展房间.路径类型 = 副本房间路径类型.主路;
        }
        
        for (int i = 格子.扩展格子.Count - 1; i >= 0; i--)
        {
            主路径房间列表.Insert(格子.插入索引, 格子.扩展格子[i]);
        }
        
        foreach (var 移除房间 in 格子.移除格子)
        {
            移除房间.路径类型 = 副本房间路径类型.阻挡;
            主路径房间列表.Remove(移除房间);
        }
    }

    private IEnumerator 显示扩展动画(可扩展格子 格子)
    {
        设置格子颜色组(格子.主路径格子, Color.blue);
        设置格子颜色组(格子.扩展格子, Color.green);
        yield return new WaitForSeconds(0.5f);
        
        设置格子颜色组(格子.主路径格子, Color.yellow);
        设置格子颜色组(格子.扩展格子, Color.yellow);
        yield return new WaitForSeconds(0.5f);
        
        if (格子.移除格子.Count > 0)
        {
            设置格子颜色组(格子.移除格子, Color.red);
            yield return new WaitForSeconds(0.5f);
            设置格子颜色组(格子.移除格子, Color.white);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void 设置格子颜色组(List<副本房间_基类> 格子列表, Color 颜色)
    {
        foreach (var 格子 in 格子列表)
        {
            this.颜色(格子.行, 格子.列, 颜色);
        }
    }

    private List<可扩展格子> 寻找可扩展格子(int 连接数)
    {
        List<可扩展格子> 可扩展格子列表 = new List<可扩展格子>();
        int 最大索引 = 主路径房间列表.Count - 连接数 + 1;

        for (int i = 0; i < 最大索引; i++)
        {
            var 主路径格子 = new List<副本房间_基类>();
            for (int j = 0; j < 连接数; j++)
            {
                主路径格子.Add(主路径房间列表[i + j]);
            }
            
            if (检查同行(主路径格子))
            {
                添加方向扩展(可扩展格子列表, 主路径格子, i, true);
            }
            else if (检查同列(主路径格子))
            {
                添加方向扩展(可扩展格子列表, 主路径格子, i, false);
            }
        }
        return 可扩展格子列表;
    }

    private void 添加方向扩展(List<可扩展格子> 列表, List<副本房间_基类> 主路径格子, int 插入索引, bool 是同行)
    {
        var 方向方法组 = 是同行 ? 
            new System.Func<副本房间_基类, 副本房间_基类>[] { 周边房间_上, 周边房间_下 } :
            new System.Func<副本房间_基类, 副本房间_基类>[] { 周边房间_左, 周边房间_右 };
            
        foreach (var 方向方法 in 方向方法组)
        {
            var 扩展格子 = new List<副本房间_基类>();
            bool 有效扩展 = true;
            
            foreach (var 主格子 in 主路径格子)
            {
                var 扩展房间 = 方向方法(主格子);
                if (扩展房间 == null || 扩展房间.路径类型 == 副本房间路径类型.主路)
                {
                    有效扩展 = false;
                    break;
                }
                扩展格子.Add(扩展房间);
            }
            
            if (有效扩展)
            {
                var 移除格子 = 主路径格子.Count > 2 ? 主路径格子.GetRange(1, 主路径格子.Count - 2) : new List<副本房间_基类>();
                列表.Add(new 可扩展格子(主路径格子, 扩展格子, 移除格子, 插入索引 + 1));
            }
        }
    }

    private bool 检查同行(List<副本房间_基类> 格子列表)
    {
        return 格子列表.All(格子 => 格子.行 == 格子列表[0].行);
    }

    private bool 检查同列(List<副本房间_基类> 格子列表)
    {
        return 格子列表.All(格子 => 格子.列 == 格子列表[0].列);
    }


    
    private (Vector2Int 入口位置, Vector2Int 出口位置) 生成相对边位置(bool 是行边, bool 入口在第一边)
    {
        if (是行边)
        {
            // 行边处理：上下边
            int 入口列 = this.随机数(0, 5 - 1);
            int 出口列 = this.随机数(0, 5 - 1);
            return 入口在第一边
                ? (new Vector2Int(0, 入口列), new Vector2Int(5 - 1, 出口列))
                : (new Vector2Int(5 - 1, 入口列), new Vector2Int(0, 出口列));
        }
        else
        {
            // 列边处理：左右边
            int 入口行 = this.随机数(0, 5 - 1);
            int 出口行 = this.随机数(0, 5 - 1);
            return 入口在第一边
                ? (new Vector2Int(入口行, 0), new Vector2Int(出口行, 5 - 1))
                : (new Vector2Int(入口行, 5 - 1), new Vector2Int(出口行, 0));
        }
    }





    private void 颜色(int x,int y, Color c)
    {
        小地图格子数组[x, y].color = c;
    }
    public 副本房间_基类 周边房间_上(副本房间_基类 目标房间 = null) => 获取指定方向房间(目标房间, 1, 0);
    public 副本房间_基类 周边房间_下(副本房间_基类 目标房间 = null) => 获取指定方向房间(目标房间, -1, 0);
    public 副本房间_基类 周边房间_左(副本房间_基类 目标房间 = null) => 获取指定方向房间(目标房间, 0, -1);
    public 副本房间_基类 周边房间_右(副本房间_基类 目标房间 = null) => 获取指定方向房间(目标房间, 0, 1);
    public 副本房间_基类 当前所在房间;
    private 副本房间_基类 获取指定方向房间(副本房间_基类 目标房间, int 行偏移, int 列偏移)
    {
        // 如果目标房间为null，使用当前所在房间
        var 基准房间 = 目标房间 ?? 当前所在房间;

        // 空值安全检查
        if (基准房间 == null) return null;

        int 目标行 = 基准房间.行 + 行偏移;
        int 目标列 = 基准房间.列 + 列偏移;

        return (目标行 >= 0 && 目标行 < 5 && 目标列 >= 0 && 目标列 < 5)
            ? 副本房间数组[目标行, 目标列]
            : null;
    }
    private Vector2 格子坐标(int 行, int 列)
    {
        return new Vector2(列 * 55 + 列 * 20 + 27.5f, 行 * 55 + 行 * 20 + 27.5f);
    }

    private void 添加连线(Vector2 起点, Vector2 终点, Color? 颜色 = null, float? 宽度 = null)
    {
        路线绘制器.添加连线(起点, 终点, 颜色, 宽度);
    }

}
