// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//================================================================================================================================
//
//  Copyright (c) 2015-2019 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

Shader "Sample/Coloring3D" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _MainTex_TexelSize;

            float4x4 _UvPints;
            float4x4 _RenderingViewMatrix;
            float4x4 _RenderingProjectMatrix;

            struct v2f {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
                float4  fixedPos : TEXCOORD2;
            } ;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

                float4 top = lerp(_UvPints[0], _UvPints[2], o.uv.x);
                float4 bottom = lerp(_UvPints[1], _UvPints[3], o.uv.x);
                float4 fixedPos = lerp(bottom, top, o.uv.y);
                float4x4 Rendering_Matrix_VP = mul(_RenderingProjectMatrix, _RenderingViewMatrix);
                o.fixedPos = ComputeGrabScreenPos(mul(Rendering_Matrix_VP, fixedPos));
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                float2 coord = i.fixedPos.xy / i.fixedPos.w;
#if SHADER_API_METAL
#if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0.0)
                    coord.y = 1.0 - coord.y;
#endif
                coord.x = 1.0 - coord.x;
#else
#if UNITY_UV_STARTS_AT_TOP
                coord.y = 1.0 - coord.y;
#endif
#endif

                return tex2D(_MainTex, coord);
            }
            ENDCG
        }
    }
}
