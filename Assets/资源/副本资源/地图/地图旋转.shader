Shader "Custom/RotateTextureZ"
{
    Properties
    {
        _MainTex("主纹理", 2D) = "white" {}
        _Rotate("旋转角度", Range(0,360)) = 45
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Rotate;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // 中心旋转UV
                    float rad = radians(_Rotate);
                    float2x2 rot = float2x2(cos(rad), -sin(rad), sin(rad), cos(rad));
                    float2 centeredUV = v.uv - 0.5;
                    centeredUV = mul(rot, centeredUV);
                    o.uv = centeredUV + 0.5;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    return col;
                }
                ENDCG
            }
        }
}
