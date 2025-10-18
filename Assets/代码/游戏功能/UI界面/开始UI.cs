using System.Collections;
using UnityEngine;

public class 开始UI : 面板基类
{
    protected override void 开始时()
    {
        // var 副本 = new 副本类();
        // 副本.副本房间管理 = new 副本房间管理器(5, 5);
    }
    public void 开始游戏按钮() => StartCoroutine(开始游戏流程());

    private IEnumerator 开始游戏流程()
    {
        // 显示过场
        yield return null;
        // 初始化数据
        sj.创建存档();
        yield return null;
        // 打开主城
        GameObject 主城UI对象 = null;
        bool 主城UI加载完成 = false;
        try
        {
            主城UI对象 = UI管理器.显示UI<主城UI>("主城UI", UI层级.主界面, (主城UI脚本) =>
            {
                主城UI加载完成 = 主城UI脚本 != null;
                if (主城UI加载完成) 主城UI脚本.进入();
            });
            if (主城UI对象 == null) yield break;
        }
        catch { yield break; }
        float 等待时间 = 0f;
        while (!主城UI加载完成 && 等待时间 < 5f)
        {
            yield return new WaitForSeconds(0.1f);
            等待时间 += 0.1f;
        }
        yield return null;
        UI管理器.关闭UI("开始UI");
    }

    public void 继续游戏按钮() { }

    public void 退出游戏按钮() => this.退出应用程序();
}
