Shader "Custom/Skybox" 
{		
	Properties
	{
		_SkyColor("Sky Color", COLOR) = (1, 1, 1)
		_HorizonColor ("Horizon Color", COLOR) = (1, 1, 1)

	}

	SubShader
	{ 
		Pass
		{ 
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		struct appdata
		{
			float4 position : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 position : SV_POSITION;
			fixed4 color : COLOR;
		};

		float4 _SkyColor;
		float4 _HorizonColor;

		v2f vert(appdata v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.position);
			o.color = lerp(_HorizonColor, _SkyColor, v.uv.y);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			return i.color;
		}

		ENDCG				
		}		
	} 
}