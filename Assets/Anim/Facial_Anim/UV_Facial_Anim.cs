//Shader "Custom/UV"
//{
//    properties
//        {
//        _MainTex("텍스처", 2D) = "white"{}
//        _U ("오프셋 U", float) = 1
//        _V ("오프셋 V", float) = 1
//    }
//    SubShader
//        {
//        Tags{ "RenderType"="Opaque"}
//        LOD 200

//        #pragma surface surf Standard fullforwardshadows
//        #pragma target 3.0

//        sampler2D _MainTex;
//        float1 _U;
//        float1 _V;

//        struct Input
//        {
//         float2 uv_MainTex;
//        };

//        UNITY_INSTANCING_BUFFER_START(Props)
//        UNITY_INSTANCING_BUFFER_END(Props)

//        void surf (Input IN, inout SurfaceOutput o)
//        {
//           float4 maintex = tex2D(_MainTex, IN.uv_MainTex + float2(_U, _V));
//    0.Emission = maintex.rgb;
//        }
//    }
//FallBack "Diffuse"
//}