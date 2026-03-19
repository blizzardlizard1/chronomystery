Shader "Custom/DitherFade"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Scale", Float) = 1.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _OcclusionMap ("Occlusion", 2D) = "white" {}
        _Fade ("Fade", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;

        half _BumpScale;
        half _Metallic;
        half _Smoothness;
        half _Fade;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_OcclusionMap;
            float4 screenPos;
        };

        // 4x4 Bayer dither matrix
        static const float bayerMatrix[16] = {
             0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0,
            12.0/16.0,  4.0/16.0, 14.0/16.0,  6.0/16.0,
             3.0/16.0, 11.0/16.0,  1.0/16.0,  9.0/16.0,
            15.0/16.0,  7.0/16.0, 13.0/16.0,  5.0/16.0
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Dither clipping
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            float2 screenPixel = screenUV * _ScreenParams.xy;

            int x = (int)fmod(screenPixel.x, 4.0);
            int y = (int)fmod(screenPixel.y, 4.0);
            float threshold = bayerMatrix[y * 4 + x];

            clip(_Fade - threshold - 0.001);

            // Standard PBR surface
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpScale);
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap).g;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}