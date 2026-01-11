using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 交互按钮_范围技能 : 交互按钮_基类
{
    private Vector3 上一次圆心;
    private Vector3 上一次作用圆心 = Vector3.positiveInfinity;
    private const string 指示圈名称 = "范围技能_指示圈";
    private HashSet<string> 高亮名称集 = new HashSet<string>();
    private HashSet<string> 作用范围名称集 = new HashSet<string>();
    private HashSet<副本_房间_地图_格子> 当前作用范围格子集 = new HashSet<副本_房间_地图_格子>();
    private 副本_房间_地图_格子 当前作用范围中心格子;
    private 副本_房间_地图_格子 上一次有效作用范围中心格子;
    private bool 当前鼠标在射程内;
    private bool 上一次鼠标在射程内;

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
        // 辅助线.绘制技能攻击范围(单位.单位.所在格子.场景坐标, 技能);
        //辅助线.绘制技能射程范围区域(单位, 技能);
        辅助线.显示辅助线();
        辅助线.设置网格可见(true);
        清理范围高亮();
        绘制群体技能范围();
        sj.新副本UI.显示消耗行动力提示文本(技能.消耗行动力);
    }

    protected override void 离开交互按钮()
    {
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        辅助线.设置网格可见(false);
        上一次作用圆心 = Vector3.positiveInfinity;
        当前鼠标在射程内 = false;
        上一次鼠标在射程内 = false;
        上一次有效作用范围中心格子 = null;
        清理作用范围高亮();
        清理范围高亮();
        sj.新副本UI.隐藏消耗行动力提示文本();
    }

    protected override void 进入交互按钮()
    {
        上一次圆心 = 单位.单位.所在格子.场景坐标;
        // 辅助线.绘制技能攻击范围(单位.单位.所在格子.场景坐标, 技能);
        //辅助线.绘制技能射程范围区域(单位, 技能);
        辅助线.显示辅助线();
        辅助线.设置网格可见(true);
        清理范围高亮();
        绘制群体技能范围();
    }

    protected override void 取消交互按钮()
    {
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        辅助线.设置网格可见(false);
        上一次圆心 = Vector3.positiveInfinity;
        上一次作用圆心 = Vector3.positiveInfinity;
        当前鼠标在射程内 = false;
        上一次鼠标在射程内 = false;
        上一次有效作用范围中心格子 = null;
        清理作用范围高亮();
        清理范围高亮();
        sj.新副本UI.隐藏消耗行动力提示文本();
    }

    protected override void 检测鼠标所在格子()
    {
        if (单位.单位.剩余行动力 <= 0) return;
        // var 圆心 = 获取鼠标世界坐标();
        // if (圆心 == Vector3.positiveInfinity) return;
        // if (float.IsNaN(圆心.x) || float.IsNaN(圆心.y) || float.IsNaN(圆心.z)) return;
        // if (float.IsInfinity(圆心.x) || float.IsInfinity(圆心.y) || float.IsInfinity(圆心.z)) return;

        // var 原点 = 单位.单位.所在格子.场景坐标;
        // float 攻击半径 = 技能.射程 * 1.5f + 0.5f;
        // var 偏移 = 圆心 - 原点;

        // if ((圆心 - 上一次圆心).sqrMagnitude < 0.0001f) return;
        // 上一次圆心 = 圆心;

        // float 指示半径 = 技能.作用范围 * 1.5f + 0.5f;
        // // 只考虑XZ平面距离，并考虑椭圆变形（垂直比例0.75）
        // float 垂直比例 = 0.75f; // 网格垂直间距 / 网格水平间距
        // float 椭圆距离 = Mathf.Sqrt(偏移.x * 偏移.x + (偏移.z / 垂直比例) * (偏移.z / 垂直比例));
        // bool 在射程内 = 椭圆距离 <= 攻击半径;
        // // 在范围内用青色，超出范围用红色，与攻击范围的灰色区分
        // var 颜色 = 在射程内 ? new Color(0.3f, 0.7f, 0.9f, 0.6f) : new Color(0.9f, 0.3f, 0.3f, 0.6f);
        // 辅助线.绘制圆形区域(指示圈名称, 圆心 + new Vector3(-0.005f, 0.01f, -0.005f), 指示半径, 颜色, 0.6f, (int)渲染层级.鼠标区域);
        //绘制群体技能范围();
        //标记范围内单位(圆心, 指示半径);

        if (技能 == null) return;
        if (sj?.新副本UI?.地图管理 == null) return;

        var 鼠标格子 = 场景.检测鼠标所在格子();
        if (鼠标格子 == null) return;

        bool 在射程内 = 单位?.技能攻击范围格子哈希集 != null && 单位.技能攻击范围格子哈希集.Contains(鼠标格子);
        当前鼠标在射程内 = 在射程内;
        if (在射程内)
        {
            上一次有效作用范围中心格子 = 鼠标格子;
        }

        var 目标中心格子 = 在射程内
            ? 鼠标格子
            : (上一次有效作用范围中心格子 ?? 单位?.单位?.所在格子 ?? 鼠标格子);
        var 作用圆心 = 目标中心格子.场景坐标;
        if ((作用圆心 - 上一次作用圆心).sqrMagnitude < 0.0001f && 在射程内 == 上一次鼠标在射程内) return;
        上一次作用圆心 = 作用圆心;
        上一次鼠标在射程内 = 在射程内;
        上一次圆心 = 作用圆心;

        绘制群体技能作用范围(目标中心格子, 在射程内);
    }

    private void 绘制群体技能范围()
    {
        // 根据技能的射程 计算格子的绘制范围 比如 射程=3 生成1=3个格子 3=9个格子距离
        // 那么以 单位中心格子为起点 找到所有与其距离<=9的格子 放到一个列表里 作为范围集合
        // 然后用绘制辅助线的脚本里的绘制方法 绘制每个格子出来
        if (技能 == null) return;
        if (单位 == null || 单位.单位 == null) return;
        if (单位.单位.所在格子 == null) return;
        if (sj?.新副本UI?.地图管理 == null) return;

        var 地图 = sj.新副本UI.地图管理;
        地图.计算技能的攻击范围(单位, 技能);
        if (单位.技能攻击范围格子哈希集 == null || 单位.技能攻击范围格子哈希集.Count == 0) return;

        var 颜色 = new Color(0.6f, 0.6f, 0.6f, 1f);
        var 偏移 = new Vector3(-0.005f, 0.01f, -0.005f);
        foreach (var 格子 in 单位.技能攻击范围格子哈希集)
        {
            if (格子 == null) continue;
            string 名称 = $"范围技能_射程_{格子.行}_{格子.列}";
            高亮名称集.Add(名称);
            辅助线.绘制矩形区域(名称, 格子.场景坐标 + 偏移, 辅助线.网格水平间距, 辅助线.网格垂直间距, 颜色, 0.8f, (int)渲染层级.移动范围);
        }
    }

    protected override void 鼠标点击格子()
    {
        if (单位.单位.剩余行动力 <= 0)
        {
            管理器.取消锁定(true);
            清理作用范围高亮();
            清理范围高亮();
            return;
        }

        if (技能 == null)
        {
            管理器.取消锁定(true);
            清理作用范围高亮();
            清理范围高亮();
            return;
        }

        if (当前作用范围中心格子 == null || 当前作用范围格子集 == null || 当前作用范围格子集.Count == 0)
        {
            管理器.取消锁定(true);
            清理作用范围高亮();
            清理范围高亮();
            return;
        }

        if (!当前鼠标在射程内)
        {
            管理器.取消锁定(true);
            清理作用范围高亮();
            清理范围高亮();
            return;
        }

        var 敌人列表 = new List<副本单位脚本>();
        for (int i = 0; i < sj.副本场景.敌人单位.Count; i++)
        {
            var 敌人 = sj.副本场景.敌人单位[i];
            var 格子 = 敌人.单位.所在格子;
            if (格子 == null) continue;
            if (!当前作用范围格子集.Contains(格子)) continue;
            if (!目标合法(敌人.单位)) continue;
            敌人列表.Add(敌人);
        }
        管理器.取消锁定(true);
        辅助线.取消绘制技能攻击范围();
        辅助线.取消绘制圆形区域(指示圈名称);
        清理作用范围高亮();
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
            // 3x3格子区域，显示在攻击范围上面
            辅助线.绘制矩形区域(名称, 格子.场景坐标 + new Vector3(-0.005f, 0.01f, -0.005f), 3.0f, 2.25f, Color.green, 0.5f, (int)渲染层级.鼠标区域);
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

    private void 清理作用范围高亮()
    {
        foreach (var 名称 in 作用范围名称集)
        {
            辅助线.取消绘制矩形区域(名称);
        }
        作用范围名称集.Clear();
        当前作用范围格子集.Clear();
        当前作用范围中心格子 = null;
    }

    private void 绘制群体技能作用范围(副本_房间_地图_格子 中心格子, bool 在射程内)
    {
        清理作用范围高亮();

        if (中心格子 == null) return;
        if (技能 == null) return;
        if (sj?.新副本UI?.地图管理 == null) return;

        int 最大步数 = Mathf.RoundToInt(技能.作用范围);
        if (最大步数 <= 0) return;

        当前作用范围中心格子 = 中心格子;

        var 地图 = sj.新副本UI.地图管理;
        var 待处理队列 = new Queue<副本_房间_地图_格子>();
        var 步数字典 = new Dictionary<副本_房间_地图_格子, int>();
        var 已访问集合 = new HashSet<副本_房间_地图_格子>();

        待处理队列.Enqueue(中心格子);
        步数字典[中心格子] = 0;

        while (待处理队列.Count > 0)
        {
            var 当前格子 = 待处理队列.Dequeue();
            int 当前步数 = 步数字典[当前格子];
            if (已访问集合.Contains(当前格子) || 当前步数 > 最大步数) continue;
            已访问集合.Add(当前格子);
            当前作用范围格子集.Add(当前格子);

            if (当前步数 >= 最大步数) continue;

            var 上 = 地图.目标格子上(当前格子);
            var 下 = 地图.目标格子下(当前格子);
            var 左 = 地图.目标格子左(当前格子);
            var 右 = 地图.目标格子右(当前格子);
            var 左上 = 地图.目标格子左上(当前格子);
            var 右上 = 地图.目标格子右上(当前格子);
            var 左下 = 地图.目标格子左下(当前格子);
            var 右下 = 地图.目标格子右下(当前格子);

            副本_房间_地图_格子[] 邻居数组 = { 上, 下, 左, 右, 左上, 右上, 左下, 右下 };
            for (int i = 0; i < 邻居数组.Length; i++)
            {
                var 邻居 = 邻居数组[i];
                if (邻居 == null || 已访问集合.Contains(邻居) || 步数字典.ContainsKey(邻居)) continue;
                int 新步数 = 当前步数 + 1;
                if (新步数 > 最大步数) continue;
                步数字典[邻居] = 新步数;
                待处理队列.Enqueue(邻居);
            }
        }

        var 颜色 = 在射程内 ? new Color(0.3f, 0.7f, 0.9f, 1f) : new Color(0.9f, 0.3f, 0.3f, 1f);
        var 偏移 = new Vector3(-0.005f, 0.01f, -0.005f);
        foreach (var 格子 in 当前作用范围格子集)
        {
            if (格子 == null) continue;
            string 名称 = $"范围技能_作用_{格子.行}_{格子.列}";
            作用范围名称集.Add(名称);
            辅助线.绘制矩形区域(名称, 格子.场景坐标 + 偏移, 辅助线.网格水平间距, 辅助线.网格垂直间距, 颜色, 0.8f, (int)渲染层级.鼠标区域);
        }
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
