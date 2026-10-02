Shader "Hidden/URPShowcase/OutlineComposite"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "OutlineComposite"

            Cull Off
            ZWrite Off
            ZTest Always

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D_X(_ScreenSpaceOutlineMask);
            SAMPLER(sampler_ScreenSpaceOutlineMask);

            float4 _OutlineColor;
            float _OutlineWidth;

            struct Attributes
            {
                uint vertexID : SV_VertexID;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionHCS =
                    GetFullScreenTriangleVertexPosition(input.vertexID);

                output.uv =
                    GetFullScreenTriangleTexCoord(input.vertexID);

                return output;
            }

            half SampleMask(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X(
                    _ScreenSpaceOutlineMask,
                    sampler_ScreenSpaceOutlineMask,
                    uv).r;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv =
                    UnityStereoTransformScreenSpaceTex(input.uv);

                float2 offset =
                    rcp(_ScreenParams.xy) * _OutlineWidth;

                half center = SampleMask(uv);
                half dilated = center;

                dilated = max(dilated, SampleMask(uv + float2( offset.x, 0.0)));
                dilated = max(dilated, SampleMask(uv + float2(-offset.x, 0.0)));
                dilated = max(dilated, SampleMask(uv + float2(0.0,  offset.y)));
                dilated = max(dilated, SampleMask(uv + float2(0.0, -offset.y)));

                dilated = max(dilated, SampleMask(uv + float2( offset.x,  offset.y)));
                dilated = max(dilated, SampleMask(uv + float2(-offset.x,  offset.y)));
                dilated = max(dilated, SampleMask(uv + float2( offset.x, -offset.y)));
                dilated = max(dilated, SampleMask(uv + float2(-offset.x, -offset.y)));

                // 膨胀后的区域减去原 Mask，只留下目标外侧边缘。
                half edge = saturate(dilated - center);

                return half4(
                    _OutlineColor.rgb,
                    _OutlineColor.a * edge);
            }

            ENDHLSL
        }
    }

    FallBack Off
}