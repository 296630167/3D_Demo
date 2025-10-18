using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "道具数据", menuName = "数据管理/道具管理")]
public class 道具管理 : ScriptableObject
{
    [FoldoutGroup("消耗品")]
    [TableList(AlwaysExpanded = true)]
    public List<药水> 药水列表 = new List<药水>();
    
    [FoldoutGroup("消耗品")]
    [TableList(AlwaysExpanded = true)]
    public List<卷轴> 卷轴列表 = new List<卷轴>();
    
    [FoldoutGroup("武器装备")]
    [TableList(AlwaysExpanded = true)]
    public List<武器> 单手武器列表 = new List<武器>();
    
    [FoldoutGroup("武器装备")]
    [TableList(AlwaysExpanded = true)]
    public List<武器> 双手武器列表 = new List<武器>();
    
    [FoldoutGroup("武器装备")]
    [TableList(AlwaysExpanded = true)]
    public List<武器> 远程武器列表 = new List<武器>();
    
    [FoldoutGroup("武器装备")]
    [TableList(AlwaysExpanded = true)]
    public List<武器> 法杖列表 = new List<武器>();
    
    [FoldoutGroup("武器装备")]
    [TableList(AlwaysExpanded = true)]
    public List<辅助武器> 辅助武器列表 = new List<辅助武器>();
    
    [FoldoutGroup("材料")]
    [TableList(AlwaysExpanded = true)]
    public List<草药> 草药列表 = new List<草药>();
    
    [FoldoutGroup("材料")]
    [TableList(AlwaysExpanded = true)]
    public List<锻造材料> 锻造材料列表 = new List<锻造材料>();
    
    [FoldoutGroup("其他")]
    [TableList(AlwaysExpanded = true)]
    public List<补给> 补给列表 = new List<补给>();
    
    [ShowInInspector, ReadOnly]
    public List<消耗品> 消耗品列表 => 药水列表.Cast<消耗品>().Concat(卷轴列表).ToList();
    
    [ShowInInspector, ReadOnly]
    public List<装备> 装备列表 => 单手武器列表.Cast<装备>().Concat(双手武器列表).Concat(远程武器列表).Concat(法杖列表).Concat(辅助武器列表).ToList();
    
    [ShowInInspector, ReadOnly]
    public List<材料> 材料列表 => 草药列表.Cast<材料>().Concat(锻造材料列表).ToList();
    [ShowInInspector, ReadOnly]
    public List<道具类> 道具列表 => 消耗品列表.Cast<道具类>().Concat(装备列表).Concat(材料列表).Concat(补给列表).ToList();
    
    private void OnValidate()
    {
        更新道具ID();
    }
    
    private void 更新道具ID()
    {
        for (int i = 0; i < 道具列表.Count; i++)
        {
            if (道具列表[i] != null)
            {
                道具列表[i].id = i + 1;
            }
        }
    }
}