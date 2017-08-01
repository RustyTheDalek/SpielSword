Shader "ScreenEffects/ScanLines"
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

			float _xScanLine0;
			float _xScanLine1;
			float _xScanLine2;
			float _xScanLine3;

			float _ScanJitter;
			float _noiseStrength;

			//This produces random values between 0 and 1
            float rand(float2 co)
            {
				return frac((sin( dot(co.xy , float2(12.345 * _Time.w, 67.890 * _Time.w) )) 
				* 12345.67890+_Time.w));
            }

			float scanLine(float texY, float xSL)
			{
				float dx = 1.005-abs(distance(texY, xSL));
				
 				if(dx > 0.99)
  					return xSL * fixed4(_ScanJitter,_ScanJitter,_ScanJitter,_ScanJitter);
  						  				
  				return texY;
			}

			float noiseLine(float2 tex, float xSL)
			{
				float dx = 1.005-abs(distance(tex.y, xSL));
				
 				if(dx > 0.99)
				{
					float noise = rand(tex) * _noiseStrength;

					return noise.x;
				}

				return 0;
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

				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine0);
				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine1);
				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine2);
				IN.texcoord.y = scanLine(IN.texcoord.y, _xScanLine3);

				IN.texcoord.y = IN.texcoord.y % 1;

				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

				float x = ((int)(IN.texcoord.x*320))/320.0;
				float y = ((int)(IN.texcoord.y*240))/240.0;
				
				c.rgb = lerp(c.rgb, fixed3(1,1,1), noiseLine(IN.texcoord, _xScanLine0));
				c.rgb = lerp(c.rgb, fixed3(1,1,1), noiseLine(IN.texcoord, _xScanLine1));
				c.rgb = lerp(c.rgb, fixed3(1,1,1), noiseLine(IN.texcoord, _xScanLine2));
				c.rgb = lerp(c.rgb, fixed3(1,1,1), noiseLine(IN.texcoord, _xScanLine3));

				c.rgb *= c.a;

				return c;
			}
			ENDCG
		}
	}
}
