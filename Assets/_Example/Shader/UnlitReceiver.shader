Shader "Custom/UnlitReceiver"
{	
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				LIGHTING_COORDS(1, 2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _MainTex_ST.xy;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				float attenuation = LIGHT_ATTENUATION(i);
				fixed4 color = tex2D(_MainTex, i.uv);
				return color * attenuation;
			}
			ENDCG
		}
	}

	Fallback "VertexLit"
}
