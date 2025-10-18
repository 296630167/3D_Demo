using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 酒馆主角单位 : 基
{
    private 酒馆单位 当前锁定单位;
    private List<酒馆单位> 触发区域内单位列表 = new List<酒馆单位>();
    private 酒馆单位 距离最近的酒馆单位;
    protected override void 开始时()
    {
        var 触发器 = g.AddComponent<BoxCollider>();
        触发器.isTrigger = true;
        触发器.center = new Vector3(0, 0.5f, 0);
        触发器.size = Vector3.one;
        var 刚体 = g.AddComponent<Rigidbody>();
        刚体.useGravity = false;
    }
    // 酒馆的其它角色单位 身上会有触发区域
    // 在主角单位这里 检测 当主角进入触发区域 的方法
    private void OnTriggerEnter(Collider other) 
    {
        // 占位规则 暂时不按一下规则做
        // 这里要判断 触发的单位 的tag是不是酒馆单位
        // 如果是 就记录这个单位
        // 获取这个单位的脚本 调用这个单位的锁定反馈方法
        // 如果连续或者同时进入多个 挨个取消之前单位的锁定反馈方法 只锁定最后一个
        
        // 创建一个列表 把每个进入触发器区域的酒馆单位 都放进去 然后判断 这个列表的单位 > 0 则 调用 酒馆UI 的准备交互方法
        if (other.CompareTag("酒馆单位"))
        {
            主角靠近酒馆单位(other.GetComponent<酒馆单位>());
        }
    }
    private void 主角靠近酒馆单位(酒馆单位 酒馆单位)
    {
        if (触发区域内单位列表.Contains(酒馆单位))
        {
            return;
        }
        触发区域内单位列表.Add(酒馆单位);
        计算距离最近的单位();
        UI管理器.获取UI脚本<酒馆UI>("酒馆UI").更新交互提示(true);
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("酒馆单位"))
        {
            主角离开酒馆单位(other.GetComponent<酒馆单位>());
        }
    }
    private void 主角离开酒馆单位(酒馆单位 酒馆单位)
    {
        if (!触发区域内单位列表.Contains(酒馆单位))
        {
            return;
        }
        触发区域内单位列表.Remove(酒馆单位);
        计算距离最近的单位();
        UI管理器.获取UI脚本<酒馆UI>("酒馆UI").更新交互提示(距离最近的酒馆单位 != null);
    }
    /// <summary>
    /// 计算距离最近的单位 - 性能优化版本
    /// 时间复杂度: O(n)，但避免了开方运算和重复的transform访问
    /// </summary>
    private void 计算距离最近的单位()
    {
        距离最近的酒馆单位 = null;
        
        // 早期返回：列表为空时直接返回
        if (触发区域内单位列表 == null || 触发区域内单位列表.Count == 0)
        {
            return;
        }
        
        // 缓存主角位置，避免重复访问transform.position
        Vector3 主角位置 = t.v3();
        float 最近平方距离 = float.MaxValue;
        // 使用for循环替代foreach，性能更优
        for (int i = 触发区域内单位列表.Count - 1; i >= 0; i--)
        {
            var 单位 = 触发区域内单位列表[i];
            // 空值检查：移除已销毁的对象
            if (单位 == null || 单位.transform == null)
            {
                触发区域内单位列表.RemoveAt(i);
                continue;
            }
            // 使用平方距离比较，避免开方运算（性能提升约30-50%）
            Vector3 单位位置 = 单位.t.v3();
            float 平方距离 = (主角位置 - 单位位置).sqrMagnitude;
            if (平方距离 < 最近平方距离)
            {
                最近平方距离 = 平方距离;
                距离最近的酒馆单位 = 单位;
            }
        }
    }
}