using UnityEngine;

public static class 游戏对象扩展
{
    public static Vector3 隐藏位置 = new Vector3(10000, 10000, 0);
    public static Vector3 显示位置 = Vector3.zero;
    
    // public static void 显示(this GameObject 对象)
    // {
    //     if (对象 == null) return;
    //     对象.transform.position = 显示位置;
    // }
    
    // public static void 隐藏(this GameObject 对象)
    // {
    //     if (对象 == null) return;
    //     对象.transform.position = 隐藏位置;
    // }
    
    public static void 显示(this GameObject 对象)
    {
        if (对象 == null) return;
        对象.SetActive(true);
    }
    
    public static void 隐藏(this GameObject 对象)
    {
        if (对象 == null) return;
        对象.SetActive(false);
    }
    public static void 切换显示(this GameObject 对象,bool 显示 = true)
    {
        if (对象 == null) return;
        对象.SetActive(显示);
    }
}