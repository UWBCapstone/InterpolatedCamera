Shader "Custom/TableShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LLUV ("LowerLeftUV", Vector) = (0,0,0,0)
		_ULUV ("UpperLeftUV", Vector) = (0,0,0,0)
		_URUV("UpperRightUV", Vector) = (0,0,0,0)
		_LRUV("LowerRightUV", Vector) = (0,0,0,0)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _LLUV;
			float4 _ULUV;
			float4 _URUV;
			float4 _LRUV;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float uBot = lerp(_LLUV.x, _LRUV.x, i.uv.x);
				//float uTop = lerp(_ULUV.x, _URUV.x, i.uv.x);
				//float u = lerp(uBot, uTop, i.uv.y);

				//float vLeft = lerp(_LLUV.y, _ULUV.y, i.uv.y);
				//float vRight = lerp(_LRUV.y, _URUV.y, i.uv.y);
				//float v = lerp(vLeft, vRight, i.uv.x);
				//float2 trueUV = float2(u, v);

				//// sample the texture
				//fixed4 col = tex2D(_MainTex, trueUV);



			//	float uDenominator = (1 - i.uv.x) * (1 / i.vertex.w) + i.uv.x * (1 / i.vertex.w);
			//	float vDenominator = (1 - i.uv.y) * (1 / i.vertex.w) + i.uv.y * (1 / i.vertex.w);

			//	float uBotNumerator = (1 - i.uv.x) * (_LLUV.x / _LLUV.z) + i.uv.x * (_LRUV.x / _LRUV.z);
			//	float uBotDenominator = (1 - i.uv.x) * (1 / _LLUV.z) + i.uv.x * (1 / _LRUV.z);
			//float uTopNumerator = (1 - i.uv.x) * (_ULUV.x / _ULUV.z) + i.uv.x * (_URUV.x / _URUV.z);
			//float uTopDenominator = (1 - i.uv.x) * (1 / _ULUV.z) + i.uv.x * (1 / _URUV.z);
			//float uBot = uBotNumerator / uBotDenominator;
			//float uTop = uTopNumerator / uTopDenominator;
			//float u = lerp(uBot, uTop, i.uv.y);
			////float u = (1 - i.uv.y) * (uBot / i.vertex.w) + i.uv.y * (uTop / i.vertex.w);
			////u = u / vDenominator;


			//float vLeftNumerator = (1 - i.uv.y) * (_LLUV.y / _LLUV.z) + i.uv.y * (_ULUV.y / _ULUV.z);
			//float vLeftDenominator = (1 - i.uv.y) * (1 / _LLUV.z) + i.uv.y * (1 / _ULUV.z);
			//float vRightNumerator = (1 - i.uv.y) * (_LRUV.y / _LRUV.z) + i.uv.y * (_URUV.y / _URUV.z);
			//float vRightDenominator = (1 - i.uv.y) * (1 / _LRUV.z) + i.uv.y * (1 / _URUV.z);
			//float vLeft = vLeftNumerator / vLeftDenominator;
			//float vRight = vRightNumerator / vRightDenominator;
			//float v = lerp(vLeft, vRight, i.uv.x);
			///*float v = (1 - i.uv.x) * (vLeft / i.vertex.w) + i.uv.x * (vRight / i.vertex.w);
			//v = v / uDenominator;*/

			//float2 trueUV = float2(u, v);
			//fixed4 col = tex2D(_MainTex, trueUV);








				/*float uDenominator = (1 - i.uv.x) * (1 / i.vertex.w) + i.uv.x * (1 / i.vertex.w);
			float vDenominator = (1 - i.uv.y) * (1 / i.vertex.w) + i.uv.y * (1 / i.vertex.w);

			float uBotNumerator = (1 - i.uv.x) * (_LLUV.x / _ULUV.z) + i.uv.x * (_LRUV.x / _URUV.z);
			float uBotDenominator = (1 - i.uv.x) * (1 / _ULUV.z) + i.uv.x * (1 / _URUV.z);
			float uTopNumerator = (1 - i.uv.x) * (_ULUV.x / _LLUV.z) + i.uv.x * (_URUV.x / _LRUV.z);
			float uTopDenominator = (1 - i.uv.x) * (1 / _LLUV.z) + i.uv.x * (1 / _LRUV.z);
			float uBot = uBotNumerator / uBotDenominator;
			float uTop = uTopNumerator / uTopDenominator;
			float u = lerp(uBot, uTop, i.uv.y);*/
			/*float u = (1 - i.uv.y) * (uBot / i.vertex.w) + i.uv.y * (uTop / i.vertex.w);
			u = u / vDenominator;*/
			/*float uBot = lerp(_LLUV.x, _LRUV.x, i.uv.x);
			float uTop = lerp(_ULUV.x, _URUV.x, i.uv.x);
			float u = lerp(uBot, uTop, i.uv.y);*/

			float uBot = lerp(_LLUV.x, _LRUV.x, i.uv.x);
			float uTop = lerp(_ULUV.x, _URUV.x, i.uv.x);
			float topZ = _ULUV.z;
			float botZ = _LLUV.z;
			float uNumerator = (1 - i.uv.y) * (uBot / topZ) + i.uv.y * (uTop / botZ);
			float uDenominator = (1 - i.uv.y) * (1 / topZ) + i.uv.y * (1 / botZ);
			float u = uNumerator / uDenominator;

			float vLeftNumerator = (1 - i.uv.y) * (_LLUV.y / _ULUV.z) + i.uv.y * (_ULUV.y / _LLUV.z);
			float vLeftDenominator = (1 - i.uv.y) * (1 / _ULUV.z) + i.uv.y * (1 / _LLUV.z);
			float vRightNumerator = (1 - i.uv.y) * (_LRUV.y / _URUV.z) + i.uv.y * (_URUV.y / _LRUV.z);
			float vRightDenominator = (1 - i.uv.y) * (1 / _URUV.z) + i.uv.y * (1 / _LRUV.z);
			float vLeft = vLeftNumerator / vLeftDenominator;
			float vRight = vRightNumerator / vRightDenominator;
			float v = lerp(vLeft, vRight, i.uv.x);
			/*float v = (1 - i.uv.x) * (vLeft / i.vertex.w) + i.uv.x * (vRight / i.vertex.w);
			v = v / uDenominator;*/

			float2 trueUV = float2(u, v);
			fixed4 col = tex2D(_MainTex, trueUV);



				//fixed4 col = tex2D(_MainTex, i.uv);


				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
