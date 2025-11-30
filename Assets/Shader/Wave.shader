Shader "Unlit/Wave"
{
    Properties // input data
    {
        _ColorA ("ColorA", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 1
        _ColorEnd ("Color End", Range(0,1)) = 0 
    }
    SubShader
    {
        // subshader tags
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            //pass tags

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            #include "UnityCG.cginc"
            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            //automatically filled out by Unity
            struct appdata // appdata = MeshData //per-vertext mesh data
            {
                float4 vertex : POSITION; // vertex position
                float3 normals : NORMAL; // local space normal direction
                //float4 tangent : TANGENT; // tangent direction (xyz) tangent sign (w) surface orientation information
                //float4 color : COLOR; // vertex colors
                float2 uv0 : TEXCOORD0; // uv0 diffuse/normal map textures
                //float4 uv1 : TEXCOORD1; // uv1 coordinates lightmap coordinates
                //float4 uv2 : TEXCODRD2; // uv2 coordinates lightmap coordinates
                //float4 uv3 : TEXCODRD3; // uv3 coordinates lightmap coordinates 
            };

            //data passed from the vertex shader to the fragment shader
            // this will interpolate/blend across the triangle!
            struct v2f //v2f = Interpolators
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal: TEXCOORD0;
                float2 uv : TEXCOORD1;
                float4 lengthProperties : TEXCOORD2;
                //float2 justSomeValues : TEXCOORD3;
                
                
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


            float InverseLerp(float a, float b, float v){
                return (v-a)/(b-a);
            }

            float4 frag (v2f i) : SV_Target // color calculation per pixel
            {
               
                float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
                float t = cos((i.uv.y + xOffset + _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
                t *= 1-i.uv.y;
                return t;
                //return float4(t, 0, 1);

                //blend between two colors based on the X UV coordinate
                //float t = cos(i.uv.x * TAU * 2) * 0.5 + 0.5;
                //return t;
                
            }
            ENDCG
        }
    }
}
