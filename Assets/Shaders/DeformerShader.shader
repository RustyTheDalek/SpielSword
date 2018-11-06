// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/DeformerShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_DeformerTex("Deformer", 2D) = "Grey" {}
		_MaskTex("Deformer Mask", 2D) = "White"{}

		_Intensity("Deformer Intensity", Range(-1, 1)) = 0
		_Speed("Deformer Speed", Range(-5, 5)) = 1
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 uvDeformer : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float2 uvDeformer : TEXCOORD1;
				};

				fixed4 _Color;

				sampler2D _DeformerTex;
				float4 _DeformerTex_ST;

				sampler2D _MaskTex;

				float _Intensity;
				float _Speed;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.uvDeformer = IN.uvDeformer;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv, float uvDeformer)
				{
					float2 deformer = tex2D(_DeformerTex, uvDeformer + _Time * _Speed);
					float2 uvOffset = deformer * _Intensity * 0.5;

					fixed4 mask = tex2D(_MaskTex, uv);
					fixed4 color = tex2D(_MainTex, uv + uvOffset * mask.r);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					float2 uvDeformer = IN.uvDeformer * _DeformerTex_ST.xy;

					fixed4 c = SampleSpriteTexture(IN.texcoord, uvDeformer) * IN.color;
					c.rgb *= c.a;
					return c;
				}
			ENDCG
			}
		}
}