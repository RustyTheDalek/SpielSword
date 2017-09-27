Shader "ScreenEffects/ScreenDistort"
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
		Cull Off ZWrite Off ZTest Always

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
			fixed _NoiseStrength;

			fixed _Offset;
 
			fixed4 shot (sampler2D tex, float2 uv) 
			{
				uv.y += _Offset;
				return tex2D(tex, float2(uv.x, clamp(uv.y,0,1)));
			}

			float rand(float2 co)
            {
				return frac((sin( dot(co.xy , float2(12.345 * _Time.w, 67.890 * _Time.w) )) 
				* 12345.67890+_Time.w));
            }
	
			fixed4 frag(v2f_img IN) : COLOR
			{
				half3 n = shot(_DisplacementTex, IN.uv);
				//half3 n = tex2D(_DisplacementTex, IN.uv);
				half3 d = n * 2 - 1;
				IN.uv += ((1 - d.b) + (1-d.r))  * _DisplaceStrength;
				IN.uv = saturate(IN.uv);
 
				float4 c = tex2D(_MainTex, IN.uv);

				fixed3 scan = lerp(
					c.rgb, fixed3(
						_NoiseStrength,_NoiseStrength,_NoiseStrength), 
						rand(IN.uv) * clamp(_NoiseStrength - (d.g + d.b), 0, 1));
				c.rgb = saturate(scan);

				return c;
			}
			ENDCG
		}
	}
}
