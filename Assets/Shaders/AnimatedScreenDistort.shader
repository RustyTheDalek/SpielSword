// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ScreenEffects/AnimatedScreenDistort"
{
	Properties
	{
		_DisplacementTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		_Cols ("Cols Count", Int) = 20
		_Rows ("Rows Count", Int) = 1
		_Frame ("Per Frame Length", Float) = 0.1
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			sampler2D _DisplacementTex;
			float4 _DisplacementTex_ST;

			fixed4 _Color;

			uint _Cols;
			uint _Rows;

			float _Frame;
			
			fixed4 shot (sampler2D tex, float2 uv, float dx, float dy, int frame) 
			{
				return tex2D(tex, float2((uv.x * dx) + fmod(frame, _Cols) * dx, 1.0 - ((uv.y * dy) + (frame / _Cols) * dy)));
			}

			fixed4 frag (v2f_img i) : SV_Target {
				int frames = _Rows * _Cols;
				float frame = fmod(_Time.y / _Frame, frames);
				int current = floor(frame);
				float dx = 1.0 / _Cols;
				float dy = 1.0 / _Rows;

				// not lerping to next frame
				// return shot(_MainTex, i.uv, dx, dy, current) * _Color;

				int next = floor(fmod(frame + 1, frames));
				return shot(_DisplacementTex, i.uv, dx, dy, next) * _Color;
			}

			ENDCG
		}
	}
}
