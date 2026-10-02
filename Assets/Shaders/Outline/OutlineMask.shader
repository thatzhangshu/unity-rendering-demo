Shader "Hidden/URPShowcase/OutlineMask"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "OutlineMask"

            Cull Back
            ZWrite Off
            ZTest Always
            ColorMask R

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            float _ScreenSpaceOutlineVisibleOnly;
            float _ScreenSpaceOutlineDepthBias;

            struct Attributes
            {
                float4 positionOS : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionHCS =
                    TransformObjectToHClip(input.positionOS.xyz);

                return output;
            }


            float GetOutlineEyeDepth(float rawDepth)
            {
                if (unity_OrthoParams.w > 0.5)
                {
                    return LinearDepthToEyeDepth(rawDepth);
                }

                return LinearEyeDepth(rawDepth, _ZBufferParams);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                if (_ScreenSpaceOutlineVisibleOnly > 0.5)
                {
                    float2 screenUV =
                        GetNormalizedScreenSpaceUV(input.positionHCS);

                    screenUV =
                        UnityStereoTransformScreenSpaceTex(screenUV);

                    float sceneRawDepth =
                        SampleSceneDepth(screenUV);

                    float targetRawDepth =
                        input.positionHCS.z;

                    float sceneEyeDepth =
                        GetOutlineEyeDepth(sceneRawDepth);

                    float targetEyeDepth =
                        GetOutlineEyeDepth(targetRawDepth);

                    // targetDepth 更大表示目标位于当前场景表面后方。
                    // clip 小于 0 时丢弃该目标片元。
                    clip(
                        sceneEyeDepth +
                        _ScreenSpaceOutlineDepthBias -
                        targetEyeDepth);
                }

                return half4(1.0, 0.0, 0.0, 1.0);
            }

            ENDHLSL
        }
    }

    FallBack Off
}