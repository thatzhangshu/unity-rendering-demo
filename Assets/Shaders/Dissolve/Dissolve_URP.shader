Shader "Custom/Dissolve/URP"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)

        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _EdgeWidth ("Edge Width", Range(0.001, 0.2)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1,0.5,0,1)
        _EdgeEmissionIntensity ("Edge Emission Intensity", Float) = 3.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="AlphaTest"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            float4 _BaseMap_ST;
            float4 _NoiseTex_ST;

            float4 _BaseColor;
            float _DissolveAmount;
            float _EdgeWidth;
            float4 _EdgeColor;
            float _EdgeEmissionIntensity;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float2 noiseUV     : TEXCOORD1;
                float3 positionWS : TEXCOORD2;

            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionHCS = positionInputs.positionCS;

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.noiseUV = TRANSFORM_TEX(input.uv, _NoiseTex);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, input.noiseUV).r;
                
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, input.noiseUV).r;

                // 加入高度影响
                float heightFactor = input.positionWS.y * 0.5;

                // 混合
                noise += heightFactor;
                
                // 小于阈值的片元直接裁掉
                clip(noise - _DissolveAmount);

                // 边缘区域判断：越接近阈值，越靠近溶解边缘
                float edge = smoothstep(_DissolveAmount, _DissolveAmount + _EdgeWidth, noise);

                // 反转，让接近阈值的位置更亮
                float edgeMask = 1.0 - edge;

                // float3 finalColor = baseCol.rgb;
                // finalColor = lerp(_EdgeColor.rgb * _EdgeEmissionIntensity, finalColor, edge);

                float3 edgeEmission = _EdgeColor.rgb * _EdgeEmissionIntensity * edgeMask;

                float3 finalColor = baseCol.rgb + edgeEmission;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}