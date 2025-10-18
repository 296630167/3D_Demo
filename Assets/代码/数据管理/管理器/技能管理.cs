using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}