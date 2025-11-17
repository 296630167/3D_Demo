using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "角色数据", menuName = "数据管理/角色管理")]
public class 角色管理 : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    public List<角色类> 角色列表 = new List<角色类>();

    private Dictionary<int, 角色类> 角色字典 = new Dictionary<int, 角色类>();

    public void 初始化字典()
    {
        角色字典.Clear();
        if (角色列表 != null)
        {
            for (int i = 0; i < 角色列表.Count; i++)
            {
                var 角色 = 角色列表[i];
                角色.初始化();
                if (角色 != null)
                {
                    角色字典[角色.id] = 角色;
                }
            }
        }
    }
    private void OnValidate()
    {
        for (int i = 0; i < 角色列表.Count; i++)
        {
            角色列表[i].id = i + 1;
        }
    }
    // 过滤 角色列表 通过id 返回对应的角色 方法脚本里有深拷贝
    public 角色类 选择角色(int id)
    {
        if (角色字典.TryGetValue(id, out var 角色))
        {
            return 角色.Json深拷贝();
        }
        return null;
    }
}