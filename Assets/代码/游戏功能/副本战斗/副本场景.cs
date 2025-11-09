using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 副本场景 : 面板基类
{
    public List<副本单位脚本> 所有单位 => 玩家单位.Cast<副本单位脚本>().Concat(敌人单位.Cast<副本单位脚本>()).ToList();
    public List<副本玩家单位> 玩家单位 = new List<副本玩家单位>();
    public List<副本敌人单位> 敌人单位 = new List<副本敌人单位>();
    public Dictionary<副本单位, 副本玩家单位> 玩家单位字典 = new Dictionary<副本单位, 副本玩家单位>();
    public Dictionary<副本单位, 副本敌人单位> 敌人单位字典 = new Dictionary<副本单位, 副本敌人单位>();
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
        // 检测鼠标所在格子();
    }
    public 副本_房间_地图_格子 检测鼠标所在格子()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (房间地图.Raycast(ray, out float enter))
        {
            Vector3 世界坐标 = ray.GetPoint(enter);
            int 行 = Mathf.FloorToInt(世界坐标.z / 0.5f);
            int 列 = Mathf.FloorToInt(世界坐标.x / 0.5f);
            //var 格子 = sj.副本UI.地图管理.取格子或空(行, 列);
            return sj.副本UI.地图管理.取格子或空(行, 列);
            //if(格子!=null)
            //{
            //    sj.副本辅助线.绘制矩形区域("123", new Vector3(列 * 0.25f + 0.125f, 0.01f, 行 * 0.25f + 0.125f), 0.5f, 0.5f);
            //}
        }
        return null;
    }
    #region 进入房间逻辑
    public void 进入房间(副本_房间 房间)
    {
        创建房间地形(房间);
        创建玩家阵营(房间);
        switch(房间.房间类型)
        {
            case 副本房间类型.战斗:
                战斗房间逻辑(房间);
                break;
        }
        场景激活 = true;
    }
    private void 创建房间地形(副本_房间 房间)
    {

    }
    private void 创建玩家阵营(副本_房间 房间)
    {
        玩家单位.Clear();
        玩家单位字典.Clear();
        foreach(var r in cd.副本上阵单位数组)
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
            Camera.main.锁定单位(玩家单位[0].t, 5f, 5f, new Vector3(45f, 45f, 0f));
        }
    }
    private void 创建敌人阵营(副本_房间 房间)
    {
        敌人单位.Clear();
        敌人单位字典.Clear();
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
       public void 离开房间(副本_房间 房间)
    {
        清理房间地形();
        清理玩家阵营();
        清理敌人阵营();
    }

    private void 清理房间地形()
    {

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
        if(!战斗进行中)return;
        Debug.Log("单位行动分割线---------------------------------------------------------");
        Debug.Log("开始选择行动单位");
        for (int i = 0; i < 战斗单位列表.Count; i++)
        {
            var 单位 = 战斗单位列表[i];
            战斗单位列表.Remove(单位);
            战斗单位列表.Add(单位);
            if(单位==null) continue;
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
        启动携程(判断战斗是否结束());
    }

    private IEnumerator 判断战斗是否结束()
    {
        if(玩家单位.All(单位 => !单位.单位.活着))
        {
            战斗进行中 = false;
            StopAllCoroutines();
            // 战斗失败 游戏结束
            sj.副本UI.显示游戏结束弹窗();
        }
        else if(敌人单位.All(单位 => !单位.单位.活着))
        {
            战斗进行中 = false;
            StopAllCoroutines();
            // 战斗胜利 继续游戏
            print("战斗胜利 继续游戏");
            // 显示小地图按钮
            sj.副本UI.刷新副本房间状态();
        }
        yield return null;
    }
    #endregion
}
