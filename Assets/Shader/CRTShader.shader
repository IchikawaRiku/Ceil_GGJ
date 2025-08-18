Shader "Custom/ApertureGrille_NoTexture"
{
    Properties
    {
        _StripeWidth ("Stripe Width (pixels)", Range(1, 20)) = 3
        _Intensity ("Color Intensity", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" }

        Pass
        {
            Name "ApertureGrilleStatic"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // ? 必要なURP共通ユーティリティをインクルード
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"

            float _StripeWidth;
            float _Intensity;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz); // ? 修正済み
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // スクリーンX座標をピクセル単位に変換
                float screenX = uv.x * _ScreenParams.x;

                // 3色の縦縞
                float stripeIndex = fmod(floor(screenX / _StripeWidth), 3.0);
                float3 mask = stripeIndex == 0 ? float3(1, 0, 0) :
                              stripeIndex == 1 ? float3(0, 1, 0) :
                                                 float3(0, 0, 1);

                return half4(mask * _Intensity, 1.0);
            }

            ENDHLSL
        }
    }
}