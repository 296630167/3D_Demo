using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "技能树数据", menuName = "数据管理/技能树管理")]
public class 技能树管理 : ScriptableObject
{
    [FoldoutGroup("技能树配置")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "技能树名称")]
    public List<技能树类> 技能树列表 = new List<技能树类>();
    private Dictionary<int, 技能树类> 技能树字典 = new Dictionary<int, 技能树类>();

    public void 初始化字典()
    {
        技能树字典.Clear();
        if (技能树列表 != null)
        {
            for (int i = 0; i < 技能树列表.Count; i++)
            {
                var 技能树 = 技能树列表[i];
                if (技能树 != null)
                {
                    技能树字典[技能树.技能树ID] = 技能树;
                }
            }
        }
    }

    public 技能树类 选择技能树(int id)
    {
        if (技能树字典.TryGetValue(id, out var 技能树))
        {
            return 技能树.Json深拷贝();
        }
        return null;
    }
}