Shader "Custom/Phong"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss ("Gloss", Float ) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct v2f //v2f = Interpolators
            {
                float4 vertex    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Gloss;

            v2f vert (appdata v) //appdata=meshData
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float frag (v2f i) : SV_Target
            {
                //diffuse lighting
                float3 N = normalize(i.normal);
                float3 L = normalize(_WorldSpaceLightPos0.xyz); // actually a direction
                float3 diffuseLight = dot(N,L) *_LightColor0.rgb;

                //specular lighting
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 R = reflect(-L,N);
                float3 RdotV = saturate(dot(V,R));
                float spec = pow(RdotV, _Gloss);// specular exponent
                float3 specularLight = spec * _LightColor0.rgb;

                float3 albedo = tex2D(_MainTex, i.uv).rgb;

                float3 finalColor = albedo * diffuseLight + specularLight;
                return finalColor;
                //return float4(specularLight.xxx,1);

                //return float4(diffuseLight, 1);
            }
            ENDCG
        }
    }
}

//source from https://www.youtube.com/watch?v=mL8U8tIiRRg&t=9729s
//????????