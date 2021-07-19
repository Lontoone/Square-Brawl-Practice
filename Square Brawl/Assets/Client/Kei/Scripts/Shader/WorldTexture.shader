// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WorldTexture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noisy Texture",2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent"   "RenderQueue" = "Transparent" "RenderType" = "TransparentCutout"}

		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;

			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 noisy = tex2D(_NoiseTex , i.worldPos * _NoiseTex_ST.xy + _NoiseTex_ST.zw);

				fixed4 res = col;
				res.a = length(noisy.rgb);

				return res;
			}
			ENDCG
		}
	}
}