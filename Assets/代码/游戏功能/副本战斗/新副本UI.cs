using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class 新副本UI : 面板基类
{
    public Camera 地图相机;
    public Camera 人物相机;

    public 副本 当前副本;
    public 副本_房间管理 当前副本所有房间 => 当前副本?.副本房间管理;
    public 副本_房间 上一个房间;
    public 副本_房间 当前所在房间;
    public 副本_房间_地图管理 地图管理;

    private 小地图UI 小地图;

    public 副本场景 副本场景;
    private 辅助线绘制 辅助线;

    private GameObject 消耗行动力提示对象;
    private TMP_Text 消耗行动力提示文本;
    private Vector2 消耗行动力提示偏移 = new Vector2(0f, -50f);
    #region 初始化副本场景
    protected override IEnumerator 开始时携程()
    {
        print("正在进入副本场景,初始化副本相关资源");
        sj.新副本UI = this;
        地图相机 = GameObject.Find("副本地图相机").GetComponent<Camera>();
        人物相机 = GameObject.Find("副本人物相机").GetComponent<Camera>();
        // 后面把这行注释掉 用正常存档数据
        cd.初始化新存档();
        yield return 启动携程(初始化副本数据());
        yield return 启动携程(初始化副本小地图());
        yield return 启动携程(初始化副本场景());
        yield return 启动携程(初始化副本资源());
        // 初始化玩家队伍 上阵单位 这个后面要迁移到 选择副本和选择阵容页面
        yield return 启动携程(初始化副本阵容());
        yield return 启动携程(进入副本房间(当前副本所有房间.入口));
    }
    private IEnumerator 初始化副本数据()
    {
        // 副本数据
        当前副本 = new 副本("新副本", 副本难度.普通, 副本状态.未开始);
        //当前副本所有房间 = 当前副本.副本房间管理;
        上一个房间 = null;
        yield return null;
    }
    private IEnumerator 初始化副本小地图()
    {
        小地图 = 组件<小地图UI>("小地图UI");
        yield return 启动携程(小地图.初始化(当前副本所有房间));
    }
    private IEnumerator 初始化副本场景()
    {
        GameObject 场景对象 = GameObject.Find("副本场景");
        副本场景 = 场景对象.GetComponent<副本场景>();
        辅助线 = 场景对象.GetComponent<辅助线绘制>();
        yield return null;
    }
    private IEnumerator 初始化副本资源()
    {
        对象池.创建对象池("预制体/副本/交互按钮", 20);
        对象池.创建对象池("预制体/模型/爱丽丝", 20);
        对象池.创建对象池("预制体/模型/矮人", 20);
        对象池.创建对象池("预制体/副本/血条", 20);
        对象池.创建对象池("预制体/副本/伤害字体对象", 20);
        对象池.创建对象池("预制体/副本/死亡动画", 20);
        消耗行动力提示对象 = 取.对象("副本/消耗行动力提示文本", t);
        if (消耗行动力提示对象 != null)
        {
            消耗行动力提示对象.SetActive(false);
            消耗行动力提示文本 = 消耗行动力提示对象.GetComponentInChildren<TMP_Text>();
        }
        yield return null;
    }
    private IEnumerator 初始化副本阵容()
    {
        foreach (var r in cd.副本上阵单位数组) r.存在单位 = false;
        cd.副本上阵单位数组[0] = new 副本单位() { 角色属性 = cd.主角, 存在单位 = true };
        yield return null;
    }
    #endregion
    #region 进入副本房间

    private IEnumerator 进入副本房间(副本_房间 房间)
    {
        yield return 启动携程(更新副本房间数据(房间));
        yield return 启动携程(更新副本房间场景(房间));
        yield return 启动携程(小地图.进入房间(房间));
    }

    private IEnumerator 更新副本房间数据(副本_房间 房间)
    {
        yield return 启动携程(离开副本房间());
        上一个房间 = 当前所在房间;
        当前所在房间 = 房间;
        if (房间.房间状态 != 副本房间状态.已探索) 房间.房间状态 = 副本房间状态.探索中;
    }

    private IEnumerator 更新副本房间场景(副本_房间 房间)
    {
        地图管理 = 当前所在房间.房间地图;
        // 暂时写一个新的初始化方法
        // 
        yield return 启动携程(地图管理.初始化新房间数据(房间));

        // yield return 启动携程(地图管理.初始化地图格子());
        // yield return 启动携程(地图管理.分配玩家单位坐标(cd.副本上阵单位数组, 房间, 上一个房间));
        // if(房间.首次进入房间)
        // {
        //     房间.首次进入房间 = false;
        //     yield return 启动携程(地图管理.分配房间单位坐标(房间, 上一个房间));
        // }
        yield return 启动携程(辅助线.设置菱形格尺寸(1f, 0.75f));
        yield return 启动携程(辅助线.初始化辅助线网格_菱形(Vector3.zero, 30, 30));
        yield return 启动携程(副本场景.进入房间(房间));
    }
    #endregion
    #region 离开副本房间
    private IEnumerator 离开副本房间()
    {
        if (上一个房间 == null) yield break;
        yield return 启动携程(辅助线.清理所有线条());
        yield return 启动携程(小地图.离开房间(上一个房间));
        yield return 启动携程(副本场景.离开房间(上一个房间));
    }
    #endregion
    #region 小地图交互
    public void 前往副本房间(int 房间方向)
    {
        副本_房间 目标房间 = null;
        switch (房间方向)
        {
            case 0: 目标房间 = 当前副本所有房间.周边房间_上(当前所在房间); break;
            case 1: 目标房间 = 当前副本所有房间.周边房间_下(当前所在房间); break;
            case 2: 目标房间 = 当前副本所有房间.周边房间_左(当前所在房间); break;
            case 3: 目标房间 = 当前副本所有房间.周边房间_右(当前所在房间); break;
        }
        if (目标房间 != null)
            启动携程(进入副本房间(目标房间));
    }

    public void 刷新小地图状态()
    {
        小地图.更新交互按钮区域状态(当前所在房间);
    }
    #endregion
    #region 相机锁定位置
    public void 相机锁定(Vector3 锁定坐标, float 过渡时间 = 0f)
    {
        // 锁定地图相机到指定位置（基于默认坐标 0,50,0）
        锁定单个相机(地图相机, 锁定坐标, new Vector3(0f, 50f, 0f), new Vector3(90f, 0f, 0f), 过渡时间);
        
        // 锁定人物相机到指定位置（基于默认坐标 0,50,-50）
        锁定单个相机(人物相机, 锁定坐标, new Vector3(0f, 50f, -50f), new Vector3(45f, 0f, 0f), 过渡时间);
    }
    public void 相机锁定(Transform 目标单位, float 过渡时间 = 0f)
    {
        if (目标单位 != null)
        {
            相机锁定(目标单位.position, 过渡时间);
        }
    }
    private void 锁定单个相机(Camera 相机, Vector3 锁定坐标, Vector3 默认坐标, Vector3 旋转角度, float 过渡时间)
    {
        if (相机 == null) return;
        
        Vector3 目标位置 = 锁定坐标 + 默认坐标;
        
        if (过渡时间 > 0f)
        {
            // 使用DOTween进行平滑过渡
            相机.transform.DOMove(目标位置, 过渡时间);
            相机.transform.DORotate(旋转角度, 过渡时间);
        }
        else
        {
            相机.transform.position = 目标位置;
            相机.transform.eulerAngles = 旋转角度;
        }
    }
    public void 相机跟随(Transform 目标, float 跟随速度 = 8f)
    {
        if (目标 == null) return;
        
        // 地图相机跟随目标
        if (地图相机 != null)
        {
            Vector3 默认偏移 = new Vector3(0f, 50f, 0f);
            地图相机.跟随目标(目标, 默认偏移, 跟随速度);
        }
        
        // 人物相机跟随目标
        if (人物相机 != null)
        {
            Vector3 默认偏移 = new Vector3(0f, 50f, -50f);
            人物相机.跟随目标(目标, 默认偏移, 跟随速度);
        }
    }
    public void 相机跟随(Vector3 目标坐标, float 跟随速度 = 8f)
    {
        // 地图相机跟随坐标
        跟随单个相机(地图相机, 目标坐标, new Vector3(0f, 50f, 0f), 跟随速度);
        
        // 人物相机跟随坐标
        跟随单个相机(人物相机, 目标坐标, new Vector3(0f, 50f, -50f), 跟随速度);
    }
    private void 跟随单个相机(Camera 相机, Vector3 目标坐标, Vector3 默认坐标, float 跟随速度)
    {
        if (相机 == null) return;
        
        Vector3 目标位置 = 目标坐标 + 默认坐标;
        
        if (跟随速度 > 0f)
        {
            相机.transform.position = Vector3.Lerp(相机.transform.position, 目标位置, Time.deltaTime * 跟随速度);
        }
        else
        {
            相机.transform.position = 目标位置;
        }
    }
    #endregion
    #region 游戏结束


    public void 显示游戏结束弹窗()
    {
        // 清理副本相关场景
        sj.副本交互按钮管理.清理按钮();
        Destroy(小地图.g);
        Destroy(副本场景.g);
        // 关闭副本UI
        UI管理器.关闭UI("副本UI");
        // 显示主城UI
        UI管理器.显示UI<主城UI>("主城UI", UI层级.弹窗, o => o.进入());
    }

    #endregion
    #region 技能消耗文字UI效果
    protected override void 每帧更新()
    {
        if (消耗行动力提示对象 != null && 消耗行动力提示对象.activeSelf)
            消耗行动力提示对象.transform.position = (Vector2)Input.mousePosition + 消耗行动力提示偏移;
    }

    public void 显示消耗行动力提示文本(int 值)
    {
        if (消耗行动力提示对象 == null) return;
        if (消耗行动力提示文本 != null) 消耗行动力提示文本.text = $"-{值}行动力";
        消耗行动力提示对象.SetActive(true);
    }

    public void 隐藏消耗行动力提示文本()
    {
        if (消耗行动力提示对象 == null) return;
        消耗行动力提示对象.SetActive(false);
    }

    public void 显示消耗行动力提示文本_自定义(string 文本)
    {
        if (消耗行动力提示对象 == null) return;
        if (消耗行动力提示文本 != null) 消耗行动力提示文本.text = 文本 ?? string.Empty;
        消耗行动力提示对象.SetActive(true);
    }
    #endregion
}
