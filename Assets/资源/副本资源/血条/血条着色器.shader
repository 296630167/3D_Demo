Shader "Custom/血条着色器"
{
    Properties
    {
        _BackgroundColor("背景颜色", Color) = (1, 0, 0, 1)
        _HealthBarColor("血条颜色", Color) = (0, 1, 0, 1)
        _HealthRatio("血量比例", Range(0, 1)) = 1.0
        _MainTex("主纹理", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            fixed4 _BackgroundColor;
            fixed4 _HealthBarColor;
            float _HealthRatio;
            sampler2D _MainTex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // 根据纹理坐标的x值来确定血条填充程度
                // x < 血量比例 显示血条颜色，否则显示背景颜色
                if (uv.x < _HealthRatio)
                {
                    return _HealthBarColor;
                }
                else
                {
                    return _BackgroundColor;
                }
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}