Shader "排序测试/物体排序着色器"
{
    Properties
    {
        [MainTexture] _BaseMap ("贴图", 2D) = "white" {}
        [MainColor] _BaseColor ("颜色", Color) = (1,1,1,1)
        _SortingLayer ("排序层级", Float) = 0
        _Cutoff ("Alpha裁切", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "TransparentCutout"
            "RenderPipeline" = "UniversalPipeline"
            // 使用Transparent队列来确保正确的透明渲染顺序
            "Queue" = "Transparent"
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            ZWrite On
            ZTest LEqual
            Cull Back
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                float _SortingLayer;
                float _Cutoff;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float4 positionCS = TransformWorldToHClip(positionWS);
                
                // 将层级值除以100进行预处理，确保在有效范围内但又有足够的敏感度
                float normalizedLayer = _SortingLayer / 100.0;
                
                // 层级越大，渲染越靠后（离相机越远）
                // 层级越小，渲染越靠前（离相机越近）
                #if UNITY_REVERSED_Z
                    // DirectX等反向Z平台：深度值越小越远
                    // 层级大 -> 深度值小 -> 靠后渲染
                    positionCS.z -= normalizedLayer * positionCS.w;
                    // 限制深度值在有效范围内，确保正负层级都能正常显示
                    positionCS.z = clamp(positionCS.z, 0.0001 * positionCS.w, 0.9999 * positionCS.w);
                #else
                    // OpenGL等正向Z平台：深度值越大越远
                    // 层级大 -> 深度值大 -> 靠后渲染
                    positionCS.z += normalizedLayer * positionCS.w;
                    // 限制深度值在有效范围内，确保正负层级都能正常显示
                    positionCS.z = clamp(positionCS.z, -0.9999 * positionCS.w, 0.9999 * positionCS.w);
                #endif
                
                output.positionCS = positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }
            
            half4 frag(Varyings i) : SV_Target
            {
                half4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _BaseColor;
                clip(baseTex.a - _Cutoff);
                return baseTex;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            
            ZWrite On
            ColorMask 0
            Cull Back
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex DepthVert
            #pragma fragment DepthFrag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                float _SortingLayer;
                float _Cutoff;
            CBUFFER_END
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            Varyings DepthVert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float4 positionCS = TransformWorldToHClip(positionWS);
                
                // 将层级值除以100进行预处理，确保在有效范围内但又有足够的敏感度
                float normalizedLayer = _SortingLayer / 100.0;
                
                // 层级越大，渲染越靠后（离相机越远）
                // 层级越小，渲染越靠前（离相机越近）
                #if UNITY_REVERSED_Z
                    // DirectX等反向Z平台：深度值越小越远
                    // 层级大 -> 深度值小 -> 靠后渲染
                    positionCS.z -= normalizedLayer * positionCS.w;
                    positionCS.z = clamp(positionCS.z, 0.0001 * positionCS.w, 0.9999 * positionCS.w);
                #else
                    // OpenGL等正向Z平台：深度值越大越远
                    // 层级大 -> 深度值大 -> 靠后渲染
                    positionCS.z += normalizedLayer * positionCS.w;
                    positionCS.z = clamp(positionCS.z, -0.9999 * positionCS.w, 0.9999 * positionCS.w);
                #endif
                
                output.positionCS = positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }
            
            half4 DepthFrag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                clip(texColor.a - _Cutoff);
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Unlit"
}