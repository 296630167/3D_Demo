using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DG.Tweening;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;
using UnityEditor.SceneManagement;
using UnityEngine;

public class 副本单位脚本 : 基
{
    public 副本单位 单位;
    public 角色朝向脚本 角色朝向;
    public 角色动画脚本 角色动画;
    public 角色移动脚本 角色移动;
    public Dictionary<副本_房间_地图_格子,List<副本_房间_地图_格子>> 可移动范围字典 = new Dictionary<副本_房间_地图_格子, List<副本_房间_地图_格子>>();
    // 这个格子 需要自动去重
    public HashSet<副本_房间_地图_格子> 可移动范围格子哈希集 = new HashSet<副本_房间_地图_格子>();
    public List<副本_房间_地图_格子> 移动路径列表 = new List<副本_房间_地图_格子>();
    public 副本单位脚本 锁定敌人单位;
    public 血条组件 血条;
    public virtual void 初始化(副本单位 单位)
    {
        this.单位 = 单位;
        单位.活着 = true;
        单位.行为 = 副本单位行为状态.待机;
        单位.移动终点格子 = null;
        角色朝向 = 对象("角色").AddComponent<角色朝向脚本>();
        角色动画 = 对象("角色").AddComponent<角色动画脚本>();
        角色移动 = 对象("角色").AddComponent<角色移动脚本>();
        t.设置位置(单位.所在格子.场景坐标);
        锁定敌人单位 = null;
        GameObject 血条对象 = 对象池.取出对象("预制体/副本/血条");
        血条对象.transform.SetParent(t);
        if(血条对象.GetComponent<血条组件>() == null)
        {
            血条对象.AddComponent<血条组件>();
        }
        血条 = 血条对象.GetComponent<血条组件>();
        血条.初始化(单位.角色属性.血量上限);
        血条.更新当前血量(单位.角色属性.当前血量);
    }

    public virtual void 回合开始()
    {
        sj.副本UI.清理交互按钮();
        单位.行为 = 副本单位行为状态.待机;
        单位.剩余行动力 = 单位.角色属性.行动力;
        计算可移动范围();
        相机锁定();
    }
    public virtual void 回合结束()
    {
        锁定敌人单位 = null;
        sj.副本场景.更新行动单位();
    }
    protected virtual void 相机锁定() => Camera.main.锁定单位(t, 5f, 5f, new Vector3(45f, 45f, 0f), 0.5f);

    public void 朝向目标格子(副本_房间_地图_格子 目标格子)
    {
        if(目标格子!=null)
        {
            角色朝向.设置朝向(目标格子.场景坐标);
        }
    }
    #region  移动相关代码
    #region  移动相关数据计算
    protected virtual void 计算可移动范围() => sj.副本UI.地图管理.计算格子上单位的可移动范围(this);
    public virtual bool 选择最近的可移动格子(副本_房间_地图_格子 格子, out 副本_房间_地图_格子 目标格子)
    {
        if(格子==null || 可移动范围字典.Count == 0)
        {
            目标格子 = null;
            return false;
        }
        if(可移动范围格子哈希集.Contains(格子) && 可移动范围字典.ContainsKey(格子))
        {
            目标格子 = 格子;
            计算移动路径(格子);
            return true;
        }
        // 在 可移动范围字典 里 选择距离这个格子最近的格子 返回
        float 最近距离 = Mathf.Infinity;
        副本_房间_地图_格子 最近格子 = null;
        foreach (var 可移动格子 in 可移动范围字典)
        {
            float 距离 = (可移动格子.Key.场景坐标 - 格子.场景坐标).magnitude;
            if (距离 < 最近距离)
            {
                最近距离 = 距离;
                最近格子 = 可移动格子.Key;
            }
        }
        目标格子 = 最近格子;
        if(最近格子!=null)
        {
            计算移动路径(最近格子);
            return true;
        }
        return false;
    }
    public virtual void 计算移动路径(副本_房间_地图_格子 目标格子)
    {
        单位.移动终点格子 = 目标格子;
        移动路径列表.Clear();
        if (目标格子 == null) return;

        var 起点格子 = 单位.所在格子;
        if (起点格子 == null) return;

        var 可达集合 = new HashSet<副本_房间_地图_格子>(可移动范围字典.Keys);
        可达集合.Add(起点格子);
        if (!可达集合.Contains(目标格子) && 目标格子 != 起点格子) return;

        var 队列 = new Queue<副本_房间_地图_格子>();
        var 已访问 = new HashSet<副本_房间_地图_格子>();
        var 前驱 = new Dictionary<副本_房间_地图_格子, 副本_房间_地图_格子>();

        队列.Enqueue(起点格子);
        已访问.Add(起点格子);

        while (队列.Count > 0)
        {
            var 当前格子 = 队列.Dequeue();
            if (当前格子 == 目标格子) break;

            var 邻居数组 = new[]{
                sj.副本UI.地图管理.目标格子上(当前格子),
                sj.副本UI.地图管理.目标格子下(当前格子),
                sj.副本UI.地图管理.目标格子左(当前格子),
                sj.副本UI.地图管理.目标格子右(当前格子),
                sj.副本UI.地图管理.目标格子左上(当前格子),
                sj.副本UI.地图管理.目标格子右上(当前格子),
                sj.副本UI.地图管理.目标格子左下(当前格子),
                sj.副本UI.地图管理.目标格子右下(当前格子)
            };

            for (int i = 0; i < 邻居数组.Length; i++)
            {
                var 邻居 = 邻居数组[i];
                if (邻居 == null) continue;
                if (!可达集合.Contains(邻居)) continue;
                if (已访问.Contains(邻居)) continue;

                前驱[邻居] = 当前格子;
                已访问.Add(邻居);
                队列.Enqueue(邻居);
            }
        }

        if (起点格子 == 目标格子) return;
        if (!前驱.ContainsKey(目标格子)) return;

        var 回溯 = 目标格子;
        var 反序路径 = new Stack<副本_房间_地图_格子>();
        while (回溯 != 起点格子)
        {
            反序路径.Push(回溯);
            回溯 = 前驱[回溯];
        }
        while (反序路径.Count > 0)
            移动路径列表.Add(反序路径.Pop());
    }
    // 移动方法 
    private 副本_房间_地图_格子 移动起始所在格子;
    private 副本_房间_地图_格子 移动最后到达格子;
    private int 当前行动力可移动格子;
    public virtual void 开始移动()
    {
        if (单位.剩余行动力 <= 0) return;
        if (移动路径列表 == null || 移动路径列表.Count == 0) return;
        StartCoroutine(开始移动携程());
    }
    private IEnumerator 开始移动携程()
    {
        if (移动路径列表 == null || 移动路径列表.Count == 0) yield break;
        if (单位.剩余行动力 <= 0) yield break;

        yield return StartCoroutine(移动前携程());
        yield return StartCoroutine(移动中携程());
        yield return StartCoroutine(移动后携程());
    }
    public virtual IEnumerator 移动前携程()
    {
        单位.剩余行动力--;
        角色动画.设置动画参数("大剑跑步", true);
        移动起始所在格子 = 单位.所在格子;
        移动最后到达格子 = 移动起始所在格子;
        当前行动力可移动格子 = 单位.角色属性.敏捷;
        yield return null;
    }
    public virtual IEnumerator 移动中携程()
    {
        const float 段时长 = 0.2f;
        float 相机位置高度 = 5f;
        float 相机跟随速度 = 8f;
        Vector3 相机偏移 = new Vector3(-相机位置高度 / 2f, 相机位置高度, -相机位置高度 / 2f);
        while (移动路径列表.Count > 0)
        {
            var 目标坐标 = 移动路径列表[0].场景坐标;
            角色朝向.设置朝向(目标坐标, 0.1f);
            var 起始坐标 = t.v3();
            float 进度 = 0f;
    
            while (进度 < 1f)
            {
                进度 += Time.deltaTime / 段时长;
                var 新位置 = Vector3.Lerp(起始坐标, 目标坐标, Mathf.Clamp01(进度));
                t.设置位置(新位置);
                Camera.main.跟随目标(t, 相机偏移, 相机跟随速度);
                yield return null;
            }
    
            移动最后到达格子 = 移动路径列表[0];
            移动路径列表.RemoveAt(0);
            当前行动力可移动格子--;
            if (当前行动力可移动格子 <= 0 && 移动路径列表.Count > 0)
            {
                if (单位.剩余行动力 > 0)
                {
                    单位.剩余行动力--;
                    当前行动力可移动格子 = 单位.角色属性.敏捷;
                }
                else
                {
                    break;
                }
            }
        }
    }
    public virtual IEnumerator 移动后携程()
    {
        角色动画.设置动画参数("大剑跑步", false);
        sj.副本UI.地图管理.清理格子单位(移动起始所在格子);
        sj.副本UI.地图管理.分配格子单位(单位, 移动最后到达格子);
        单位.所在格子 = 移动最后到达格子;
        单位.移动终点格子 = null;
        计算可移动范围();
        Camera.main.锁定单位(t, 5f, 5f, new Vector3(45f, 45f, 0f), 0.3f);
        yield return null;
    }
    #endregion
    #endregion
    #region 技能相关代码
    public virtual bool 检测敌人是否在技能范围内()
    {
        if (锁定敌人单位 == null) return false;
        // 如果 敌人所在的位置 与自身位置 格子距离 < 3 返回true
        float 距离 = Vector3.Distance(单位.所在格子.场景坐标, 锁定敌人单位.单位.所在格子.场景坐标);
        print(距离);
        return 距离 <= 1.5f;
    }
    public virtual bool 检测敌人是否在技能范围内(副本单位 敌人单位,float 技能范围)
    {
        float 距离 = Vector3.Distance(单位.所在格子.场景坐标, 敌人单位.所在格子.场景坐标);
        return 距离 <= 技能范围;
    }

    public 副本_房间_地图_格子 检测敌人移动后是否在范围内(副本单位 目标单位, float 射程)
    {
        if (目标单位 == null || 目标单位.所在格子 == null) return null;
        if (可移动范围字典 == null || 可移动范围字典.Count == 0) return null;

        var 敌人格子 = 目标单位.所在格子;
        var 自身格子 = 单位.所在格子;
        float 最近到自身距离 = Mathf.Infinity;
        副本_房间_地图_格子 最近格子 = null;

        foreach (var 可移动项 in 可移动范围字典)
        {
            var 可移动格子 = 可移动项.Key;
            float 到敌人距离 = Vector3.Distance(可移动格子.场景坐标, 敌人格子.场景坐标);
            if (到敌人距离 <= 射程)
            {
                float 到自身距离 = Vector3.Distance(可移动格子.场景坐标, 自身格子.场景坐标);
                if (到自身距离 < 最近到自身距离)
                {
                    最近到自身距离 = 到自身距离;
                    最近格子 = 可移动格子;
                }
            }
        }

        return 最近格子;
    }
    #endregion

    #region  攻击
    public virtual void 使用单体技能攻击敌人(副本单位脚本 敌人, 技能类 技能) => StartCoroutine(使用单体技能攻击敌人携程(敌人, 技能));
    protected virtual IEnumerator 使用单体技能攻击敌人携程(副本单位脚本 敌人, 技能类 技能)
    {
        yield return StartCoroutine(使用单体技能攻击敌人前携程(敌人, 技能));
        yield return StartCoroutine(使用单体技能攻击敌人中携程(敌人, 技能));
        yield return StartCoroutine(使用单体技能攻击敌人后携程(敌人, 技能));
    }
    // 使用单体技能携程 也像移动一样 分 前中后
    protected virtual IEnumerator 使用单体技能攻击敌人前携程(副本单位脚本 敌人, 技能类 技能)
    {
        角色动画.设置动画参数("大剑攻击", true);
        yield return null;
    }
    protected virtual IEnumerator 使用单体技能攻击敌人中携程(副本单位脚本 敌人, 技能类 技能)
    {
        yield return new WaitForSeconds(角色动画.获取动画时长("大剑攻击"));
    }
    protected virtual IEnumerator 使用单体技能攻击敌人后携程(副本单位脚本 敌人, 技能类 技能)
    {
        角色动画.设置动画参数("大剑攻击", false);
        bool 命中 = 计算命中结果(敌人,技能);
        int 最终伤害 = 1;
        if(命中)
        {
            bool 格挡 = 计算格挡结果(敌人, 技能);
            if (格挡)
            {
                // 格挡成功 
                // 最终伤害 = 技能伤害*0.2;
                // 且不再计算后续伤害加成
            }
            else
            {
                // 格挡失败 判断伤害是否暴击
                bool 暴击 = 计算暴击结果(敌人, 技能);
                if (暴击)
                {
                    // 暴击成功 最终伤害 = 技能伤害*敌人.暴击伤害倍率;
                    // 出现 暴击 字样
                }
                else
                {
                    // 暴击失败 最终伤害 = 技能伤害;
                    // 出现 -XXX 字样
                    yield return 启动携程(敌人.被攻击(最终伤害));
                    // 敌人.被攻击(最终伤害);
                }
            }
        }
        else
        {
            // 技能未命中
            // 敌人播放 闪避动画
            // 显示 闪避 文字
        }
        print("计算攻击结果");
        yield return null;
    }

    private bool 计算命中结果(副本单位脚本 敌人, 技能类 技能)
    {
        float 攻击单位命中率 = 10f;
        float 被攻击者闪避率 = 敌人.单位.角色属性.闪避;
        float 命中率 = 攻击单位命中率 - 被攻击者闪避率;
        return true;
    }
    private bool 计算格挡结果(副本单位脚本 敌人, 技能类 技能)
    {
        // float 格挡概率 = 敌人.单位.角色属性.格挡概率;
        return false;
    }
    private bool 计算暴击结果(副本单位脚本 敌人, 技能类 技能)
    {
        // float 暴击率 = 敌人.单位.角色属性.暴击率;
        return false;
    }

    public virtual IEnumerator 被攻击(int 最终伤害)
    {
        单位.角色属性.当前血量 -= 最终伤害;
        血条.更新当前血量(单位.角色属性.当前血量);
        显示伤害字体(最终伤害);
        if(单位.角色属性.当前血量 <= 0)
        {
            // 死亡
            // 角色动画.设置动画参数("死亡", true);
            单位.活着 = false;
            对象池.归还对象(血条.gameObject);
            // 创建死亡动画
            var 死亡动画 = 对象池.取出对象("预制体/副本/死亡动画");
            死亡动画.transform.SetParent(t, false);
            // 死亡动画.transform.position = Vector3.zero;
            死亡动画.transform.localPosition = new Vector3(-1.5f, 3f, -1.5f);
            死亡动画.transform.rotation = Quaternion.Euler(45, 45, 0);
            死亡动画.transform.DOScale(Vector3.one * 0.2f, 0.3f);
            sj.副本UI.地图管理.清理格子单位(单位.所在格子);
            sj.副本场景.当前行动单位.计算可移动范围();
            yield return new WaitForSeconds(1.1f);
            yield return 启动携程(sj.副本场景.单位阵亡(this));
        }
    }

    public virtual void 显示伤害字体(int 最终伤害)
    {
        var 字体对象 = 对象池.取出对象("预制体/副本/伤害字体对象");
        字体对象.transform.SetParent(t, false);
        字体对象.transform.localPosition = new Vector3(0f, 2f, 0f);
        字体对象.transform.GetChild(0).TMP_文本($"-{最终伤害}");
        字体对象.transform.DOMoveY(3f, 1f).OnComplete(() => 对象池.归还对象(字体对象));
    }
    public void 先移动再使用单体技能攻击敌人(副本单位脚本 敌人, 技能类 技能)
    {
        StartCoroutine(先移动再使用单体技能攻击敌人携程(敌人, 技能));
    }

    public virtual IEnumerator 先移动再使用单体技能攻击敌人携程(副本单位脚本 敌人, 技能类 技能)
    {
        单位.移动终点格子 = 检测敌人移动后是否在范围内(敌人.单位, 技能.射程 * 1.9f);
        计算移动路径(单位.移动终点格子);
        yield return StartCoroutine(移动前携程());
        yield return StartCoroutine(移动中携程());
        yield return StartCoroutine(移动后携程());
        yield return StartCoroutine(使用单体技能攻击敌人携程(敌人, 技能));
    }

    public void 归还血条()
    {
        if (血条 == null) return;
        对象池.归还对象(血条.gameObject);
    }
    #endregion
}
