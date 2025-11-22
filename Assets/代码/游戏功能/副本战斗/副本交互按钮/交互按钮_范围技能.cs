using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 交互按钮_范围技能 : 交互按钮_基类
{
    private Vector3 上一次圆心;
    private const string 指示圈名称 = "范围技能_指示圈";
    private HashSet<string> 高亮名称集 = new HashSet<string>();

    public override void 初始化按钮(副本玩家单位 单位, 技能类 技能)
    {
        base.初始化按钮(单位, 技能);
        按钮文本("群");
        if (技能?.图标 != null) 按钮图标(技能.图标);
        上一次圆心 = 单位.单位.所在格子.场景坐标;
    }

    protected override void 点击交互按钮()
    {
        if (单位.单位.剩余行动力 <= 0)
        {
            管理器.取消锁定(true);
            return;
        }
        辅助线.绘制技能攻击范围(单位.单位.所在格子.场景坐标, 技能);
        sj.副本UI.显示消耗行动力提示文本(技能.消耗行动力);
    }

    protected override void 离开交互按钮()
    {
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        清理范围高亮();
        sj.副本UI.隐藏消耗行动力提示文本();
    }

    protected override void 进入交互按钮()
    {
        上一次圆心 = 单位.单位.所在格子.场景坐标;
        辅助线.绘制技能攻击范围(单位.单位.所在格子.场景坐标, 技能);
    }

    protected override void 取消交互按钮()
    {
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        上一次圆心 = Vector3.positiveInfinity;
        清理范围高亮();
        sj.副本UI.隐藏消耗行动力提示文本();
    }

    protected override void 检测鼠标所在格子()
    {
        if (单位.单位.剩余行动力 <= 0) return;
        var 圆心 = 获取鼠标世界坐标();
        if (圆心 == Vector3.positiveInfinity) return;
        if (float.IsNaN(圆心.x) || float.IsNaN(圆心.y) || float.IsNaN(圆心.z)) return;
        if (float.IsInfinity(圆心.x) || float.IsInfinity(圆心.y) || float.IsInfinity(圆心.z)) return;

        var 原点 = 单位.单位.所在格子.场景坐标;
        float 攻击半径 = 技能.射程 * 1.5f + 0.5f;
        var 偏移 = 圆心 - 原点;
        // if (偏移.magnitude > 攻击半径)
        // {
        //     圆心 = 原点 + 偏移.normalized * 攻击半径;
        // }

        if ((圆心 - 上一次圆心).sqrMagnitude < 0.0001f) return;
        上一次圆心 = 圆心;

        float 指示半径 = 技能.作用范围 * 1.5f + 0.5f;
        bool 在射程内 = 偏移.magnitude <= 攻击半径;
        var 颜色 = 在射程内 ? Color.gray : Color.red;
        辅助线.绘制圆形区域(指示圈名称, 圆心 + new Vector3(-0.005f, 0.01f, -0.005f), 指示半径, 颜色, 0.5f, 10);

        清理范围高亮();
        标记范围内单位(圆心, 指示半径);
    }

    protected override void 鼠标点击格子()
    {
        if (单位.单位.剩余行动力 <= 0)
        {
            管理器.取消锁定(true);
            清理范围高亮();
            return;
        }
        float 半径 = 技能.作用范围 * 1.5f + 0.5f;
        var 敌人列表 = new List<副本单位脚本>();
        for (int i = 0; i < sj.副本场景.敌人单位.Count; i++)
        {
            var 敌人 = sj.副本场景.敌人单位[i];
            var 格子 = 敌人.单位.所在格子;
            if (格子 == null) continue;
            if (Vector3.Distance(格子.场景坐标, 上一次圆心) > 半径) continue;
            if (!目标合法(敌人.单位)) continue;
            敌人列表.Add(敌人);
        }
        管理器.取消锁定(true);
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        清理范围高亮();
        交互携程 = 启动携程(监听技能结束携程(敌人列表));
    }

    private IEnumerator 监听技能结束携程(List<副本单位脚本> 敌人列表)
    {
        取消锁定();
        yield return 启动携程(单位.使用范围技能攻击敌人携程(敌人列表, 技能));
        取消锁定(true);
        交互携程 = null;
    }

    private Vector3 获取鼠标世界坐标()
    {
        if (Camera.main == null) return Vector3.positiveInfinity;
        var 射线 = Camera.main.ScreenPointToRay(Input.mousePosition);
        var 平面 = new Plane(Vector3.up, Vector3.zero);
        if (平面.Raycast(射线, out float 距离))
        {
            var 点 = 射线.GetPoint(距离);
            if (float.IsNaN(点.x) || float.IsNaN(点.y) || float.IsNaN(点.z)) return Vector3.positiveInfinity;
            if (float.IsInfinity(点.x) || float.IsInfinity(点.y) || float.IsInfinity(点.z)) return Vector3.positiveInfinity;
            return 点;
        }
        return Vector3.positiveInfinity;
    }

    private void 标记范围内单位(Vector3 圆心, float 半径)
    {
        标记列表内单位(sj.副本场景.玩家单位, 圆心, 半径);
        标记列表内单位(sj.副本场景.敌人单位, 圆心, 半径);
    }

    private void 标记列表内单位<T>(System.Collections.Generic.List<T> 列表, Vector3 圆心, float 半径) where T : 副本单位脚本
    {
        for (int i = 0; i < 列表.Count; i++)
        {
            var 脚本 = 列表[i];
            var 数据单位 = 脚本?.单位;
            var 格子 = 数据单位?.所在格子;
            if (格子 == null) continue;
            if (!目标合法(数据单位)) continue;
            if (Vector3.Distance(格子.场景坐标, 圆心) > 半径) continue;
            string 名称 = $"范围技能_单位_{格子.行}_{格子.列}";
            高亮名称集.Add(名称);
            辅助线.绘制矩形区域(名称, 格子.场景坐标 + new Vector3(-0.005f, 0.01f, -0.005f), 1.5f, 1.5f, Color.green, 0.5f, 10);
        }
    }

    private void 清理范围高亮()
    {
        foreach (var 名称 in 高亮名称集)
        {
            辅助线.取消绘制矩形区域(名称);
        }
        高亮名称集.Clear();
    }

    private bool 目标合法(副本单位 目标)
    {
        switch (技能.施法对象)
        {
            case 技能施法对象.自己:
                return 目标 == 单位.单位;
            case 技能施法对象.敌人:
                return 目标.所属阵营 != 单位.单位.所属阵营;
            case 技能施法对象.队友:
                return 目标.所属阵营 == 单位.单位.所属阵营 && 目标 != 单位.单位;
            case 技能施法对象.所有人:
                return true;
        }
        return false;
    }
}