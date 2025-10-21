using UnityEngine;

public class 角色数据测试 : MonoBehaviour
{
    void Start()
    {
        // 测试加载角色数据
        try
        {
            角色管理 角色数据 = Resources.Load<角色管理>("数据/角色数据");
            
            if (角色数据 != null)
            {
                Debug.Log("✅ 角色数据.asset 文件加载成功！");
                Debug.Log($"当前角色列表数量: {角色数据.角色列表.Count}");
                
                // 如果有角色数据，显示第一个角色信息
                if (角色数据.角色列表.Count > 0)
                {
                    var 第一个角色 = 角色数据.角色列表[0];
                    Debug.Log($"第一个角色: {第一个角色.名称}, ID: {第一个角色.id}");
                }
            }
            else
            {
                Debug.LogError("❌ 角色数据.asset 文件加载失败！文件为null");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ 加载角色数据时发生错误: {e.Message}");
        }
    }
}