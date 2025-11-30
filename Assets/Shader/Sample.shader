Shader "Unlit/Sample"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Amplitude ("Move Amplitude", Range(0.0,1.0)) = 0.2
        _Speed ("Move Speed", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Amplitude;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                float t = _Time.y * _Speed;
                v.vertex.x += sin(t) * _Amplitude;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;  
                return col;
            }
            ENDCG
        }
    }
}
