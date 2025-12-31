using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 副本场景 : 面板基类
{
    副本_房间 当前房间;
    public List<副本单位脚本> 所有单位 => 玩家单位.Cast<副本单位脚本>().Concat(敌人单位.Cast<副本单位脚本>()).ToList();
    public List<副本玩家单位> 玩家单位 = new List<副本玩家单位>();
    public List<副本敌人单位> 敌人单位 = new List<副本敌人单位>();
    public Dictionary<副本单位, 副本玩家单位> 玩家单位字典 = new Dictionary<副本单位, 副本玩家单位>();
    public Dictionary<副本单位, 副本敌人单位> 敌人单位字典 = new Dictionary<副本单位, 副本敌人单位>();
    public List<GameObject> 建筑物列表 = new List<GameObject>();
    Plane 房间地图;
    Camera 相机;
    bool 场景激活;
    protected override void 开始时()
    {
        sj.副本场景 = this;
        t.position = Vector3.zero;
        房间地图 = new Plane(Vector3.up, Vector3.zero);
        相机 = Camera.main;
        场景激活 = false;
    }
    protected override void 每帧更新()
    {
        if (!场景激活) return;
    }
    public 副本_房间_地图_格子 检测鼠标所在格子()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (房间地图.Raycast(ray, out float enter))
        {
            Vector3 世界坐标 = ray.GetPoint(enter);
            float ax = 0.5f; // 列步长的x分量
            float az = 0.375f; // 行/列步长的z分量
            float 列实数 = (世界坐标.x / ax + 世界坐标.z / az) * 0.5f;
            float 行实数 = (-世界坐标.x / ax + 世界坐标.z / az) * 0.5f;
            int 行 = 行实数.向下取整();
            int 列 = 列实数.向下取整();
            return sj.新副本UI.地图管理.取格子或空(行, 列);
        }
        return null;
    }
    #region 进入房间逻辑
    public IEnumerator 进入房间(副本_房间 房间)
    {
        当前房间 = 房间;
        更新地板贴图();
        创建房间地形(房间);
        创建玩家阵营(房间);
        switch (房间.房间类型)
        {
            case 副本房间类型.战斗:
                if (房间.可以离开当前房间) yield break;
                战斗房间逻辑(房间);
                break;
        }
        场景激活 = true;
        yield return null;
    }
    private void 更新地板贴图()
    {
        组件<单一物体排序脚本>("副本地板").初始化("副本/地板/地板2", 20f, 0);
        组件<单一物体排序脚本>("副本地板").设置层级((float)渲染层级.场景地板);
    }
    private void 创建房间地形(副本_房间 房间)
    {
        // 先清理之前的建筑物（归还对象池）
        清理房间地形();
        
        // 遍历房间的建筑列表，为每个建筑创建游戏对象
        foreach (var 建筑 in 房间.建筑列表)
        {
            if (建筑 == null || 建筑.所在格子 == null) continue;
            
            // 从对象池取出建筑预制体
            GameObject 建筑对象 = 对象池.取出对象("预制体/副本/场景建筑/副本场景对象");
            建筑对象.transform.SetParent(t);
            
            // 计算位置偏移：让建筑的左下角（绿点）对齐到所在格子的左下角顶点
            // 因为贴图锤点在中心（红点），所以需要向左下偏移半个占地尺寸
            // 菱形格子：X轴向右，Z轴向上
            float 偏移X = (建筑.占地列数 * 0.5f); // 向左偏移半个宽度
            float 偏离Z = (建筑.占地行数 * 0.5f); // 向下偏移半个高度
            Vector3 基础位置 = 建筑.所在格子.场景坐标 + new Vector3(0, 0f, 偏离Z);
            // 设置建筑位置
            建筑对象.transform.position = 基础位置;
            
            // 设置建筑旋转 - X轴旋转90度
            建筑对象.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            
            // 根据建筑占地大小设置缩放（柱子占2x2格子）
            float 缩放比例X = 建筑.占地列数; // 列数对应X轴缩放
            float 缩放比例Z = 建筑.占地行数; // 行数对应Z轴缩放
            建筑对象.transform.localScale = new Vector3(缩放比例X, 1f, 缩放比例Z);
            
            // 获取建筑上的单一物体排序脚本组件
            var 排序脚本 = 建筑对象.GetComponent<单一物体排序脚本>();
            if (排序脚本 != null)
            {
                // 根据建筑类型设置不同的贴图
                string 贴图路径 = "副本/场景/篝火立柱"; // 默认使用篇火立柱贴图
                switch (建筑.建筑类型)
                {
                    case 副本建筑类型.柱子:
                        贴图路径 = "副本/场景/篝火立柱";
                        break;
                    case 副本建筑类型.墙壁:
                        贴图路径 = "副本/场景/篝火立柱"; // 暂时使用相同贴图
                        break;
                    case 副本建筑类型.障碍物:
                        贴图路径 = "副本/场景/篝火立柱";
                        break;
                    case 副本建筑类型.装饰物:
                        贴图路径 = "副本/场景/篝火立柱";
                        break;
                }
                
                // 初始化建筑贴图
                排序脚本.初始化(贴图路径, 2f, 建筑.所在格子.行);
                // 设置层级：使用Z坐标作为层级，确保大于地板层级0
                排序脚本.设置层级(建筑.所在格子.场景坐标.z);
            }
            
            // 添加到建筑物列表
            建筑物列表.Add(建筑对象);
        }
    }
    private void 创建玩家阵营(副本_房间 房间)
    {
        // 先清理之前的玩家单位
        清理玩家阵营();
        
        foreach (var r in cd.副本上阵单位数组)
        {
            if (r == null || r.角色属性 == null) continue;
            GameObject 单位对象 = 对象池.取出对象("预制体/模型/爱丽丝");
            单位对象.transform.SetParent(t);
            副本玩家单位 单位脚本 = 单位对象.AddComponent<副本玩家单位>();
            单位脚本.初始化(r);
            玩家单位.Add(单位脚本);
            玩家单位字典[r] = 单位脚本;
        }
        // 相机 像酒馆的一样 默认看向 第一个玩家单位
        if (玩家单位.Count > 0)
        {
            sj.新副本UI.相机锁定(玩家单位[0].t);
            //Camera.main.锁定单位(玩家单位[0].t, 50f, 5f, new Vector3(90f, 0f, 0f));
        }
    }
    private void 创建敌人阵营(副本_房间 房间)
    {
        // 先清理之前的敌人单位
        清理敌人阵营();
        
        foreach (var r in 房间.首领列表)
        {
            GameObject 单位对象 = 对象池.取出对象("预制体/模型/矮人");
            单位对象.transform.SetParent(t);
            副本敌人单位 单位脚本 = 单位对象.AddComponent<副本敌人单位>();
            单位脚本.初始化(r);
            敌人单位.Add(单位脚本);
            敌人单位字典[r] = 单位脚本;
        }
        foreach (var r in 房间.精英列表)
        {
            GameObject 单位对象 = 对象池.取出对象("预制体/模型/矮人");
            单位对象.transform.SetParent(t);
            副本敌人单位 单位脚本 = 单位对象.AddComponent<副本敌人单位>();
            单位脚本.初始化(r);
            敌人单位.Add(单位脚本);
            敌人单位字典[r] = 单位脚本;
        }
        foreach (var r in 房间.小怪列表)
        {
            GameObject 单位对象 = 对象池.取出对象("预制体/模型/矮人");
            单位对象.transform.SetParent(t);
            副本敌人单位 单位脚本 = 单位对象.AddComponent<副本敌人单位>();
            单位脚本.初始化(r);
            敌人单位.Add(单位脚本);
            敌人单位字典[r] = 单位脚本;
        }
    }
    #endregion
    #region 离开房间逻辑
    public IEnumerator 离开房间(副本_房间 房间)
    {
        清理房间地形();
        清理玩家阵营();
        清理敌人阵营();
        yield return null;
    }

    private void 清理房间地形()
    {
        // 清理所有建筑物
        foreach (var 建筑对象 in 建筑物列表)
        {
            if (建筑对象 != null)
            {
                对象池.归还对象(建筑对象);
            }
        }
        建筑物列表.Clear();
    }

    private void 清理玩家阵营()
    {
        清理单位阵营(玩家单位);
        玩家单位字典.Clear();
    }

    private void 清理敌人阵营()
    {
        清理单位阵营(敌人单位);
        敌人单位字典.Clear();
    }

    private void 清理单位阵营<T>(List<T> 单位列表) where T : 副本单位脚本
    {
        for (int i = 0; i < 单位列表.Count; i++)
        {
            var 单位 = 单位列表[i];
            if (单位 == null) continue;
            var 游戏对象 = 单位.gameObject;
            if (游戏对象 == null) continue;
            if (单位.TryGetComponent<T>(out var 脚本组件)) Destroy(脚本组件);
            对象池.归还对象(游戏对象);
        }
        单位列表.Clear();
    }
    #endregion
    #region 战斗房间逻辑
    List<副本单位脚本> 战斗单位列表;
    public 副本单位脚本 当前行动单位;
    public bool 战斗进行中;
    private void 战斗房间逻辑(副本_房间 房间)
    {
        战斗单位列表 = new List<副本单位脚本>();
        创建敌人阵营(房间);
        战斗单位列表.Clear();
        战斗开始();
    }

    private void 战斗开始()
    {
        战斗进行中 = true;
        计算战斗单位行动顺序();
        更新行动单位();
    }
    private void 计算战斗单位行动顺序()
    {
        战斗单位列表 = 所有单位.Where(副本单位 => 副本单位.单位.活着).ToList();
        战斗单位列表.Sort((a, b) => b.单位.角色属性.敏捷.CompareTo(a.单位.角色属性.敏捷));
    }

    public void 更新行动单位()
    {
        // 从0开始 遍历 战斗单位列表 
        // 找到 活着 的单位
        // 找到后 把这个单位 放到列表的最后
        if (!战斗进行中) return;


        for (int i = 0; i < 战斗单位列表.Count; i++)
        {
            var 单位 = 战斗单位列表[i];
            战斗单位列表.Remove(单位);
            战斗单位列表.Add(单位);
            if (单位 == null) continue;
            if (单位.单位.活着)
            {
                当前行动单位 = 单位;
                单位.回合开始();
                break;
            }
        }
    }

    public IEnumerator 单位阵亡(副本单位脚本 单位)
    {


        if (单位 == null) yield break;
        GameObject 单位对象 = 单位.gameObject;

        if (单位对象 == null) yield break;
        var 本体 = 单位.单位;
        if (本体 != null) 本体.活着 = false;
        Destroy(单位);
        对象池.归还对象(单位对象);


        yield return 启动携程(判断战斗是否结束());

    }

    private IEnumerator 判断战斗是否结束()
    {

        if (玩家单位.All(单位 => !单位.单位.活着))
        {
            战斗进行中 = false;
            StopAllCoroutines();

            // 战斗失败 游戏结束
            sj.新副本UI.显示游戏结束弹窗();
        }
        else if (敌人单位.All(单位 => !单位.单位.活着))
        {
            战斗进行中 = false;
            StopAllCoroutines();
            // 战斗胜利 继续游戏

            当前房间.可以离开当前房间 = true;
            // 显示小地图按钮
            sj.新副本UI.刷新小地图状态();
        }


        yield return null;
    }
    #endregion
}
