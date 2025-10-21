using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class 小地图UI : 面板基类
{
    副本类 副本;
    路线绘制器 路线绘制器;
    RectTransform[,] 小地图格子数组;
    protected override void 开始时()
    {
        副本 = new 副本类
        {
            副本房间管理 = new 副本房间管理器()
        };
        路线绘制器 = 组件<路线绘制器>("路线绘制");
        // 等待几秒 等副本初始化完成 再继续调用下面代码
        this.延迟执行(5f, () =>
        {
            初始化小地图UI();
            初始化小地图UI连线();
        });
    }
    private void 初始化小地图UI()
    {
        小地图格子数组 = new RectTransform[副本.副本房间管理.副本房间行数, 副本.副本房间管理.副本房间列数];
        for (int x = 0; x < 副本.副本房间管理.副本房间行数; x++)
        {
            for (int y = 0; y < 副本.副本房间管理.副本房间列数; y++)
            {
                副本房间 房间 = 副本.副本房间管理.副本房间数组[x, y];
                Transform 格子 = 变换("小地图容器").GetChild(x * 副本.副本房间管理.副本房间列数 + y + 1);
                小地图格子数组[x,y] = 格子 as RectTransform;
                switch(房间.房间类型)
                {
                    case 副本房间类型.入口:
                        格子.GetChild(0).图片().color = Color.green;
                        break;
                    case 副本房间类型.出口:
                        格子.GetChild(0).图片().color = Color.red;
                        break;
                    default:
                        if(房间.路径类型== 副本房间路径类型.主路)
                        {
                            格子.GetChild(0).图片().color = Color.yellow;
                        }
                        else
                        {
                            格子.GetChild(0).图片().color = Color.gray;
                        }
                        break;
                }
                // 格子.GetChild(0).GetChild(0).TMP_文本(房间.房间类型.ToString());
            }
        }
    }
    private void 初始化小地图UI连线()
    {
        for (int 行 = 0; 行 < 副本.副本房间管理.副本房间行数; 行++)
        {
            for (int 列 = 0; 列 < 副本.副本房间管理.副本房间列数; 列++)
            {
                var 房间 = 副本.副本房间管理.副本房间数组[行, 列];
                
                // 调试信息：显示房间门状态
                //Debug.Log($"房间({行},{列}) 门状态: 上={房间.上}, 下={房间.下}, 左={房间.左}, 右={房间.右}");
                
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
}
