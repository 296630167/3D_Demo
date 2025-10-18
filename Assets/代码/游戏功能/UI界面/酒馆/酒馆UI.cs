using System;
using UnityEngine;

public class 酒馆UI : 面板基类
{
    private GameObject 酒馆场景对象, 主角模型;
    private Camera 主相机;
    private 角色脚本 主角角色;
    private bool 允许与酒馆单位交互;
    private bool 正在与酒馆单位对话;
    public void 进入()
    {
        酒馆场景对象 = 取.对象("场景/酒馆场景", Vector3.zero, Quaternion.identity);
        
        // 初始化角色系统
        初始化角色系统();
        正在与酒馆单位对话 = false;
        Camera.main.设置正交大小(25f);
        Camera.main.锁定单位(主角模型.transform, new Vector3(-25f, 50f, -25f), new Vector3(45f, 45f, 0f));
        UI管理器.当前页面 = 当前页面.酒馆;
    }
    
    private void 初始化角色系统()
    {
        主角模型 = 取.对象("模型/爱丽丝", Vector3.zero, Quaternion.identity);
        主角角色 = 主角模型.GetComponent<角色脚本>();
        if (主角角色 == null)
        {
            主角角色 = 主角模型.AddComponent<角色脚本>();
        }
        主角模型.AddComponent<酒馆主角单位>();
    }
    protected override void 每帧更新()
    {
        检测鼠标点击();
        // 检测主角如果是移动状态 设置相机跟随
        if (主角角色.当前状态 == 角色状态.移动)
        {
            Camera.main.锁定单位(主角模型.transform, new Vector3(-25f, 50f, -25f), new Vector3(45f, 45f, 0f));
        }
        else
        {
            // 如果 允许与酒馆单位交互 =true  && 正在与酒馆单位对话 = false 玩家按了一次F后 触发对话交互方法
            if (允许与酒馆单位交互 && !正在与酒馆单位对话)
            {
                触发对话交互方法();
            }
        }
    }

    private void 触发对话交互方法()
    {
        if (!Input.GetKeyDown(KeyCode.F))
        {
            return;
        }
        正在与酒馆单位对话 = true;
        UI管理器.显示UI<对话UI>("对话UI", UI层级.弹窗, o => o.进入酒馆对话());
    }

    public void 离开()
    {
        // 清理角色系统引用
        主角角色 = null;
        
        if (酒馆场景对象 != null)
        {
            DestroyImmediate(酒馆场景对象);
            酒馆场景对象 = null;
        }
        if (主角模型 != null)
        {
            DestroyImmediate(主角模型);
            主角模型 = null;
        }
        主相机 = null;
        UI管理器.关闭UI("酒馆UI");
        UI管理器.显示UI<主城UI>("主城UI", UI层级.主界面, o => o.进入());
    }

    private void 检测鼠标点击()
    {
        if (Input.GetMouseButtonDown(0))
        {
            获取点击位置();
        }
    }

    private void 获取点击位置()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray 射线 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit 碰撞信息;
        
        if (Physics.Raycast(射线, out 碰撞信息))
        {
            if (碰撞信息.collider.gameObject.name.Contains("地板") || 
                碰撞信息.collider.gameObject.tag == "地板")
            {
                Vector3 点击位置 = 碰撞信息.point;
                主角角色.移动到目标点(点击位置);
            }
        }
    }

    public void 更新交互提示(bool v)
    {
        允许与酒馆单位交互 = v;
        对象("交互提示文本").切换显示(v);
    }
}
