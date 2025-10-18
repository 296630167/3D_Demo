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
}