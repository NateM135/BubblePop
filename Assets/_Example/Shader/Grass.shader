Shader "Custom/Grass"
{
	Properties
	{
		_MainColor("Color", COLOR) = (1, 1, 1)

		_WindTexture("Wind Texture", 2D) = "white" {}

		_WorldSize("World Size", vector) = (1, 1, 1, 1)
		_WindSpeed("Wind Speed", vector) = (1, 1, 1, 1)

		_WaveSpeed("Wave Speed", float) = 1.0
		_WaveAmp("Wave Amplitude", float) = 1.0

		_HeightCutoff("Height Cutoff", float) = 1.0
		_HeightFactor("Height Factor", float) = 1.0
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			Cull Off

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
				};

				float4 _MainColor;

				float4 _WorldSize;
				float4 _WorldOrigin;

				sampler2D _WindTexture;
				float4 _WindSpeed;

				float _WaveSpeed;
				float _WaveAmp;

				float _HeightCutoff;
				float _HeightFactor;

				v2f vert(appdata input)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(input.vertex);

					float4 worldPos = mul(input.vertex, unity_ObjectToWorld);
					float2 samplePos = worldPos.xz / _WorldSize.xz;

					samplePos += _Time.x * _WindSpeed.xz;

					// Height factor is 0 or 1, then multiply by height factor
					float heightFactor = input.vertex.y > _HeightCutoff;
					heightFactor = heightFactor * (input.vertex.y * _HeightFactor);

					float windSample = tex2Dlod(_WindTexture, float4(samplePos, 0, 0));
					o.pos.z += sin(_WaveSpeed * windSample) * _WaveAmp * heightFactor;
					o.pos.x += cos(_WaveSpeed * windSample) * _WaveAmp * heightFactor;

					return o;
				}

				fixed4 frag(v2f input) : COLOR
				{
					return _MainColor;
				}
				ENDCG
			}
		}
}
