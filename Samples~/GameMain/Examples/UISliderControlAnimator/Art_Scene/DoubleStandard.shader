Shader "ArtTool/DoubleStandard"
{
    Properties
    {
        _Cutoff( "Mask Clip Value", Float ) = 0.5
        _Color ("颜色", Color) = (1,1,1,1)
        _MainTex ("Albedo贴图", 2D) = "white" {}
        [NoScaleOffset]_MS ("金属粗糙度贴图", 2D) = "white" {}
        [NoScaleOffset][Normal]_Normal ("法线贴图", 2D) = "bump" {}
        [NoScaleOffset]_AO ("AO贴图", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MS;
        sampler2D _Normal;
        sampler2D _AO;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Cutoff = 0.5;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            half3 normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex)).xyz;
            half4 ms = tex2D(_MS, IN.uv_MainTex);
            half4 ao = tex2D(_AO, IN.uv_MainTex);

            o.Albedo = c.rgb;
            o.Normal = normal;
            o.Occlusion = ao.r;
            o.Metallic = _Metallic * ms.r;
            o.Smoothness = _Glossiness * ms.a;
            o.Alpha = 1;
            clip(c.a - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
