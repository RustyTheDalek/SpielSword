﻿Shader "ScreenEffects/CRTDistort"
{
	Properties
	{
        [PerRendererData] _MainTex ("Screen", 2D) = "white" {}
		_DisplacementTex ("DisplacementTex", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader
	{
		//Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
			uniform sampler2D _DisplacementTex;

			float4 _DisplacementTex_ST;

			fixed _DisplaceStrength;
 
			fixed4 frag(v2f_img IN) : COLOR
			{

				half3 n = tex2D(_DisplacementTex, IN.uv);
				half3 d = n * 2 - 1;
				IN.uv += d * _DisplaceStrength;
				IN.uv = saturate(IN.uv);
 
				float4 c = tex2D(_MainTex, IN.uv);

				return c;
			}
			ENDCG
		}
	}
}