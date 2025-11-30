Shader "Fire" 
{
	Properties
	{
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_GradientTex("Gradient Texture", 2D) = "white" {}
 
		_BrighterCol("Brighter Color", Color) = (1, 0.8, 0, 1)
		_MiddleCol("Middle Color", Color) = (1, 0.3, 0, 1)  
		_DarkerCol("Darker Color", Color) = (0.8, 0, 0, 1)

		_FlameSpeed("Flame Speed", Range(0.1, 5)) = 2.0
        _FlameHeight("Flame Height", Range(0, 2)) = 1.0
        _FlameWobble("Flame Wobble", Range(0, 1)) = 0.3
        _FlameWidth("Flame Width", Range(0, 0.5)) = 0.15
	}
 
	SubShader
	{
		//The shader is transparent
		Tags
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
			
		Blend SrcAlpha OneMinusSrcAlpha
 
		Pass 
		{
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc" //to use _Time
 
 
			sampler2D _NoiseTex;
			sampler2D _GradientTex;
 
			float4 _BrighterCol;
			float4 _MiddleCol;
			float4 _DarkerCol;

			float _FlameSpeed;
            float _FlameHeight;
            float _FlameWobble;
            float _FlameWidth;
 
			//Input for the vertex
			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
 
			//Output for the fragment
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
 
			v2f vert(appdata v) {
				v2f o;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float height = v.texcoord.y;
                float time = _Time.y * _FlameSpeed;
                float wobbleX = sin(time + worldPos.y * 3.0) * _FlameWobble * height;
                wobbleX += sin(time * 1.7 + worldPos.y * 5.0) * _FlameWobble * 0.5 * height;
                float wobbleZ = cos(time * 1.3 + worldPos.y * 4.0) * _FlameWobble * 0.7 * height;
                float stretch = sin(time * 2.0 + worldPos.x * 2.0) * _FlameHeight * 0.1 * height;
                float taper = lerp(1.0, 0.3, height * height);
                v.vertex.x += wobbleX * _FlameWidth * taper;
                v.vertex.z += wobbleZ * _FlameWidth * taper;
                v.vertex.y += stretch;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;

                return o;
			}
 
			float4 frag(v2f IN) : SV_Target {
				
				float noiseValue = tex2D(_NoiseTex, IN.uv - float2(0, _Time.x)).x; //fire with scrolling
				float gradientValue = tex2D(_GradientTex, IN.uv).x;
				
				float step1 = step(noiseValue, gradientValue);
				float step2 = step(noiseValue, gradientValue-0.2);
				float step3 = step(noiseValue, gradientValue-0.4);
 
				//The entire fire color
				float4 c = float4
					(
						//Calculates where to place the darker color instead of the brighter one
						lerp
						(
							_BrighterCol.rgb,
							_DarkerCol.rgb,
							step1 - step2 //Corresponds to "L1" in my GIF
						),
 
					step1 //This is the alpha of our fire, which is the "outer" color, i.e. the step1
					);
 
				c.rgb = lerp //Calculates where to place the middle color
					(
						c.rgb,
						_MiddleCol.rgb,
						step2 - step3 //Corresponds to "L2" in my GIF
					);
 
				return c;
			}
			ENDCG
		}
	
	}
}

//source from https://blog.febucci.com/2019/05/fire-shader/