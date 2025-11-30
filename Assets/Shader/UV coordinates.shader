Shader "Unlit/UV coordinates"
{
     Properties // input data
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("UV Scale", Float) = 1
        _Offset ("UV Offset", Float) = 0
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            #include "UnityCG.cginc"

            float4 _ColorA;
            float _Scale;
            float _Offset;

            //automatically filled out by Unity
            struct appdata // appdata = MeshData //per-vertext mesh data
            {
                float4 vertex : POSITION; // vertex position
                float3 normals : NORMAL;
                //float4 tangent : TANGENT;
                //float4 color : COLOR;
                float2 uv0 : TEXCOORD0; // uv0 diffuse/normal map textures
                //float2 uv1 : TEXCOORD1; // uv1 coordinates lightmap coordinates
            };

            //data passed from the vertex shader to the fragment shader
            // this will interpolate/blend across the triangle!
            struct v2f //v2f = Interpolators
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal: TEXCOORD0;
                float2 uv : TEXCOORD1;
                //float2 tangent : TEXCOORD1;
                //float2 justSomeValues : TEXCOORD2;
                //float2 uv : TEXCOORD0; // 
                
            };

            

            v2f vert (appdata v) //appdata = meshData
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //local space to clip space
                o.normal = UnityObjectToWorldNormal(v.normals); // just pass through
                o.uv = v.uv0; //(v.uv0 + _Offset )* _Scale; //passthrough
                return o;
            }

            // bool 0 1
            // int
            // float4 = Vector4 (32 bit float)
            // half (16 bit float)
            // fixed (lower precision) -1 to 1
            // float4 -> half 4 -> fixed4
            // float4x4 -> half 4x4 (C#: Matrix4x4)
            // 

            float4 frag (v2f i) : SV_Target // color calculation per pixel
            {
                
                //return float4(i.uv.yyy, 1);
                //return float4(i.uv.xxx, 1);
                return float4(i.uv,0,1);
                //return float4(i.normal,1); // _Color color value
            }
            ENDCG
        }
    }
}
