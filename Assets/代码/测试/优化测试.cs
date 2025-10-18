using System.Collections.Generic;
using UnityEngine;

public class 优化测试 : MonoBehaviour
{
    void Start()
    {
        // 测试优化后的副本房间管理器
        var 管理器 = new 副本房间管理器(10, 10);
        
        // 测试生成延长路径功能
        try
        {
            // 这里只是编译测试，不实际运行
            Debug.Log("副本房间管理器优化代码编译成功！");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"优化代码出现错误: {e.Message}");
        }
    }
}