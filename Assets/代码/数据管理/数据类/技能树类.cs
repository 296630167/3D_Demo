using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class 技能树连接
{
    public 技能树格子 上级格子;
    public 技能树格子 下级格子;
    
    public 技能树连接()
    {
    }
    
    public 技能树连接(技能树格子 上级, 技能树格子 下级)
    {
        上级格子 = 上级;
        下级格子 = 下级;
    }
}

[System.Serializable]
public class 技能树类
{
    public int 技能树ID;
    public string 技能树名称;
    public string 技能树描述;
    public List<技能树格子> 技能树格子列表 = new List<技能树格子>();
    public List<技能树连接> 技能树连接列表 = new List<技能树连接>();
    public int 技能总数 => 技能树格子列表.Count;
    public string 背景路径;
    public 技能树类()
    {
        技能树格子列表 = new List<技能树格子>(18);
        for (int i = 0; i < 18; i++)
        {
            var 格子 = new 技能树格子();
            格子.技能 = null; // 明确设置为null
            格子.行 = i / 3;
            格子.列 = i % 3;
            技能树格子列表.Add(格子);
        }
    }
}
[Serializable]
public class 技能树格子
{
    public 技能类 技能;
    public int 行;
    public int 列;
    
    public 技能树格子()
    {
        技能 = null; // 明确设置为null，防止Unity自动创建默认对象
    }
}