using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 小地图UI : 面板基类
{
    路线绘制器 路线绘制器 => 组件<路线绘制器>("路线绘制");
    RectTransform[,] 小地图格子数组;
    Image[,] 小地图格子图标数组;
    TMP_Text[,] 小地图格子文本数组;
    public void 初始化(副本_房间管理 所有房间)
    {
        小地图格子数组 = new RectTransform[所有房间.副本_房间行数, 所有房间.副本_房间列数];
        小地图格子图标数组 = new Image[所有房间.副本_房间行数, 所有房间.副本_房间列数];
        小地图格子文本数组 = new TMP_Text[所有房间.副本_房间行数, 所有房间.副本_房间列数];
        for (int x = 0; x < 所有房间.副本_房间行数; x++)
        {
            for (int y = 0; y < 所有房间.副本_房间列数; y++)
            {
                副本_房间 房间 = 所有房间.副本_房间数组[x, y];
                Transform 格子 = 变换("小地图容器").GetChild(x * 所有房间.副本_房间列数 + y + 1);
                小地图格子数组[x,y] = 格子 as RectTransform;
                小地图格子图标数组[x,y] = 格子.GetChild(0).图片();
                小地图格子文本数组[x,y] = 格子.GetChild(0).GetChild(0).TMP_文本("?");
                小地图格子图标数组[x,y].color = Color.black;
                // 更新房间格子UI(房间);
            }
        }
    }
    public void 进入房间(副本_房间 房间)
    {
        小地图格子文本数组[房间.行, 房间.列].text = (房间.房间类型 == 副本房间类型.入口 || 房间.房间类型 == 副本房间类型.出口) ? 房间.房间类型.ToString()[0].ToString() : "我";
        小地图格子文本数组[房间.行, 房间.列].color = Color.black;
        小地图格子图标数组[房间.行, 房间.列].color = Color.yellow;
        更新UI连线(房间);
        更新交互按钮区域状态(房间);
    }
    public void 离开房间(副本_房间 房间)
    {
        if (房间 != null && 房间.房间状态 == 副本房间状态.已探索)
        {
            小地图格子文本数组[房间.行, 房间.列].text = 房间.房间类型.ToString()[0].ToString(); 
            小地图格子文本数组[房间.行, 房间.列].color = Color.black;
            小地图格子图标数组[房间.行, 房间.列].color = Color.green;
        }
    }
    private void 更新UI连线(副本_房间 房间)
    {
        if(房间.上)
        {
            添加连线(
                格子坐标(房间.行, 房间.列),
                格子坐标(房间.行 + 1, 房间.列)
            );
        }
        if(房间.下)
        {
            添加连线(
                格子坐标(房间.行, 房间.列),
                格子坐标(房间.行 - 1, 房间.列)
            );
        }
        if(房间.左)
        {
            添加连线(
                格子坐标(房间.行, 房间.列),
                格子坐标(房间.行, 房间.列 - 1)
            );
        }
        if(房间.右)
        {
            添加连线(
                格子坐标(房间.行, 房间.列),
                格子坐标(房间.行, 房间.列 + 1)
            );
        }
    }

    private void 初始化小地图UI连线(副本_房间管理 所有房间)
    {
        for (int 行 = 0; 行 < 所有房间.副本_房间行数; 行++)
        {
            for (int 列 = 0; 列 < 所有房间.副本_房间列数; 列++)
            {
                var 房间 = 所有房间.副本_房间数组[行, 列];
                if (房间.上)
                {
                    添加连线(
                        格子坐标(行, 列),
                        格子坐标(行 + 1, 列)
                    );
                }
                if (房间.右)
                {
                    添加连线(
                        格子坐标(行, 列),
                        格子坐标(行, 列 + 1)
                    );
                    //Debug.Log($"添加右连线: ({行},{列}) -> ({行},{列+1})");
                }
            }
        }
    }

    private Vector2 格子坐标(int 行, int 列)
    {
        return new Vector2(列 * 55 + 列 * 20 + 27.5f, 行 * 55 + 行 * 20 + 27.5f);
    }

    private void 添加连线(Vector2 起点, Vector2 终点, Color? 颜色 = null, float? 宽度 = null)
    {
        路线绘制器.添加连线(起点, 终点, 颜色, 宽度);
    }
    private void 设置路径点(List<Vector2> 路径点)
    {
        路线绘制器.设置路径点(路径点);
    }

    public void 更新交互按钮区域状态(副本_房间 房间)
    {
        bool 显示 = 房间.可离开当前房间;
        变换("切换房间按钮").切换显示(显示);
        if(显示)
        {
            对象("上").切换显示(房间.上);
            对象("下").切换显示(房间.下);
            对象("左").切换显示(房间.左);
            对象("右").切换显示(房间.右);
        }
    }
    private void 上() => sj.副本UI.前往副本房间(0);
    private void 下() => sj.副本UI.前往副本房间(1);
    private void 左() => sj.副本UI.前往副本房间(2);
    private void 右() => sj.副本UI.前往副本房间(3);
}
