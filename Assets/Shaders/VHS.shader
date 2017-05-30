// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/VHS"
{
		Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

			float _xScanLine0;
			float _xScanLine1;

			float _yScanLine;
			float _noiseStrength;
			float _ScanJitter;

			//This produces random values between 0 and 1
            float rand(float2 co)
             {
                     return frac((sin( dot(co.xy , float2(12.345 * _Time.w, 67.890 * 
					 _Time.w) )) * 12345.67890+_Time.w));
             }

			float scanLine(float texY, float xSL)
			{
				float dx = 1.001-abs(distance(texY, xSL));
				
				if(dx > 0.99)
					return  xSL * fixed4(_ScanJitter,_ScanJitter,_ScanJitter,_ScanJitter);
				
				return texY;
			}

			v2f vert(appdata_t IN)
			{		
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				_yScanLine = _yScanLine % 1;

				float dy = 1-abs(distance(OUT.vertex.y, _yScanLine));

				dy = ((int)(dy*15))/15.0;
				dy = dy;
				OUT.vertex.x += dy * 0.025 + rand(float2(dy,dy)).r/500;//0.025;

				return OUT;
			}


			fixed4 frag(v2f IN) : SV_Target
			{
				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine0);
				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine1);

				IN.texcoord.y = IN.texcoord.y % 1;

				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				float x = ((int)(IN.texcoord.x*320))/320.0;
				float y = ((int)(IN.texcoord.y*240))/240.0;

				float noise = rand(IN.texcoord) * _noiseStrength;

				float3 stat = lerp(c, fixed3(1, 1, 1), noise.x);

				return fixed4(stat.xyz, IN.color.a);
			}
		ENDCG
		}
	}
}
