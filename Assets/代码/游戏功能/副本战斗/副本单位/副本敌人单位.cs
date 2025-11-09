using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class 副本敌人单位 : 副本单位脚本
{
    技能类 技能;
    public override void 初始化(副本单位 单位)
    {
        base.初始化(单位);
        单位.所属阵营 = 所属阵营.敌人阵营;
    }
    public override void 回合开始()
    {
        base.回合开始();
        // AI行为判断
        技能 = sj.技能数据.选择技能(1);
        StartCoroutine(AI行为判定());
    }

    public override void 回合结束()
    {
        Debug.Log("回合结束");
        base.回合结束();
    }


    private IEnumerator AI行为判定()
    {
        // 锁定距离最近的敌人
        // 锁定成功
        //  默认使用技能
        //  判断敌人是否再技能范围内
        //  判断敌人移动后+技能范围 敌人是否在范围内
        //  判断移动范围内的所有格子  选择 距离敌人的距离比当前格子距离敌人位置更近的格子 移动
        //  不存在则回合结束
        //  锁定失败-回合结束

        if(选择距离最近的玩家阵营单位())
        {
            // 锁定敌人成功
            Debug.Log($"锁定距离最近的玩家单位{锁定敌人单位.单位.角色属性.名称}");
            // 检测敌人是否在技能范围内
            Debug.Log($"开始检测{锁定敌人单位.单位.角色属性.名称}是否在技能范围内");
            if (检测敌人是否在技能范围内(锁定敌人单位.单位,技能.射程 * 1.9f))
            {
                Debug.Log($"敌人{锁定敌人单位.单位.角色属性.名称}在技能范围内");
                yield return 启动携程(使用单体技能攻击敌人携程(锁定敌人单位, 技能));
                yield break;
            }
            Debug.Log($"敌人{锁定敌人单位.单位.角色属性.名称}不在技能范围内");
            // 检测敌人移动后+技能范围 敌人是否在范围内
            Debug.Log($"开始检测{锁定敌人单位.单位.角色属性.名称}移动后是否在技能范围内");
            if(检测敌人移动后是否在范围内(锁定敌人单位.单位, 技能.射程 * 1.9f) != null)
            {
                Debug.Log($"敌人{锁定敌人单位.单位.角色属性.名称}移动后在技能范围内");
                yield return 启动携程(使用单体技能攻击敌人携程(锁定敌人单位, 技能));
                yield break;
            }
            Debug.Log($"敌人{锁定敌人单位.单位.角色属性.名称}移动后不在技能范围内");
            // 选择移动范围内 距离比自身距离敌人更近的格子 移动
            if(选择最近的可移动格子(锁定敌人单位.单位.所在格子, out var 移动目标格子))
            {
                Vector3 敌人坐标 = 锁定敌人单位.单位.所在格子.场景坐标;
                Vector3 所在坐标 = 单位.所在格子.场景坐标;
                Vector3 移动目标坐标 = 移动目标格子.场景坐标;
                Debug.Log($"开始检测移动目标格子{移动目标格子.行},{移动目标格子.列}是否比自身距离敌人更近");
                Debug.Log($"移动目标格子距离敌人{Vector3.Distance(移动目标坐标,敌人坐标)}");
                Debug.Log($"自身距离敌人{Vector3.Distance(所在坐标,敌人坐标)}");
                if (Vector3.Distance(移动目标坐标,敌人坐标) >= Vector3.Distance(所在坐标,敌人坐标))
                {
                    Debug.Log("没有可移动的格子,回合结束");
                    回合结束();
                    yield break;
                }
                Debug.Log($"找到可以移动的格子{移动目标格子.行},{移动目标格子.列},开始移动");
                开始移动();
                yield break;
            }
        }
        else
        {
            // 没找到 回合结束
            回合结束();
            yield break;
        }
    }
    public override IEnumerator 移动后携程()
    {
        yield return StartCoroutine(base.移动后携程());
        Debug.Log("移动结束,回合结束");
        回合结束();
        // yield return StartCoroutine(AI行为判定());
    }
    private bool 选择距离最近的玩家阵营单位()
    {
        var 玩家单位列表 = sj.副本场景.玩家单位.Where(x => x.单位.活着).ToList();
        if (玩家单位列表.Count == 0)
        {
            print("没找到敌人");
            return false;
        }
        锁定敌人单位 = 玩家单位列表.OrderBy(x => Vector3.Distance(x.单位.所在格子.场景坐标, 单位.所在格子.场景坐标)).First();
        print($"锁定敌人 {锁定敌人单位.单位.角色属性.名称}");
        return true;
    }
    public override IEnumerator 被攻击(int 最终伤害)
    {
        yield return StartCoroutine(base.被攻击(单位.角色属性.血量上限));
    }
    protected override IEnumerator 使用单体技能攻击敌人携程(副本单位脚本 敌人, 技能类 技能)
    {
        yield return StartCoroutine(base.使用单体技能攻击敌人携程(敌人, 技能));
        回合结束();
    }
    public override IEnumerator 先移动再使用单体技能攻击敌人携程(副本单位脚本 敌人, 技能类 技能)
    {
        单位.移动终点格子 = 检测敌人移动后是否在范围内(敌人.单位, 技能.射程 * 1.9f);
        计算移动路径(单位.移动终点格子);
        yield return StartCoroutine(移动前携程());
        yield return StartCoroutine(移动中携程());
        yield return StartCoroutine(base.移动后携程());
        yield return StartCoroutine(使用单体技能攻击敌人携程(敌人, 技能));
        回合结束();
    }
}
