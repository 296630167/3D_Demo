using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "技能数据", menuName = "数据管理/技能管理")]
public class 技能管理 : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    public List<技能类> 技能列表 = new List<技能类>();
    
    private void OnValidate()
    {
        更新技能ID();
    }
    
    private void 更新技能ID()
    {
        for (int i = 0; i < 技能列表.Count; i++)
        {
            if (技能列表[i] != null)
            {
                技能列表[i].id = i + 1;
            }
        }
    }
    
    private Dictionary<int, 技能类> 技能字典 = new Dictionary<int, 技能类>();

    public void 初始化字典()
    {
        技能字典.Clear();
        if (技能列表 != null)
        {
            for (int i = 0; i < 技能列表.Count; i++)
            {
                var 技能 = 技能列表[i];
                Debug.Log($"技能ID: {技能.id}, 技能名称: {技能.名称}");
                if (技能 != null)
                {
                    技能字典[技能.id] = 技能;
                }
            }
        }
        Debug.Log(选择技能(1));
    }
    
    public 技能类 选择技能(int id)
    {
        if (技能字典.TryGetValue(id, out var 技能))
        {
            return 技能.Json深拷贝();
        }
        return null;
    }
}