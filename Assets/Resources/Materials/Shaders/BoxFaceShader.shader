Shader "Custom/BoxFaceShader" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_uvLL("LowerLeftUV", Vector) = (0,0,0,0)
		_uvUL("UpperLeftUV", Vector) = (0,0,0,0)
		_uvLR("LowerRightUV", Vector) = (0,0,0,0)
		_uvUR("UpperRightUV", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			//#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv_MainTex : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 t_vertex : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _uvLL;
			float4 _uvLR;
			float4 _uvUL;
			float4 _uvUR;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.t_vertex = v.vertex;
				o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// Bilinear interpolation idea taken from https://forum.unity.com/threads/bi-lerp.175105/

				float2 deltaXBot = lerp(_uvLL, _uvLR, i.uv_MainTex.x);
				float2 deltaXTop = lerp(_uvUL, _uvUR, i.uv_MainTex.x);
				float2 true_uv = lerp(deltaXBot, deltaXTop, i.uv_MainTex.y);

				/*float2 true_uv = i.uv_MainTex;
				float uvx = (i.uv_MainTex.x * (_uvLR.x - _uvLL.x)) + _uvLL.x;
				float uvy = (i.uv_MainTex.y * (_uvUL.y - _uvLL.y)) + _uvLL.y;

				true_uv.x = uvx;
				true_uv.y = uvy;*/

				fixed4 c = tex2D(_MainTex, true_uv);
				//fixed4 c = tex2D(_MainTex, i.uv_MainTex);

				return c;
			}
			ENDCG
		}
	}
		FallBack "Diffuse"
}