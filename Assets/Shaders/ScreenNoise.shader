Shader "ScreenEffects/ScreenNoise"
{
	Properties
	{
        [PerRendererData] _MainTex ("Screen", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;

			float _noiseStrength;

			//This produces random values between 0 and 1
            float rand(float2 co)
            {
                     return frac((sin( dot(co.xy , float2(12.345 * _Time.w, 67.890 * 
					 _Time.w) )) * 12345.67890+_Time.w));
            }

			struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

			struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{		
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}
 
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

				float x = ((int)(IN.texcoord.x*320))/320.0;
				float y = ((int)(IN.texcoord.y*240))/240.0;

				float noise = rand(IN.texcoord) * _noiseStrength;
				
				c.rgb = lerp(c.rgb, fixed3(1,1,1), noise.x);

				c.rgb *= c.a;

				return c;
			}
			ENDCG
		}
	}
}
