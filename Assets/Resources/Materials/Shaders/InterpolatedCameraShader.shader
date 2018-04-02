Shader "Custom/InterpolatedCameraShader" {
	Properties
	{
		_MainTex("Texture", 2DArray) = "" {}
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
#pragma target 3.5 // May be able to remove
#pragma multi_compile_fog

#include "UnityCG.cginc"

		// https://forum.unity.com/threads/how-to-declare-global-constant-in-cg.280920/
		static const float MAX_CAMERAS = float(4);

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
		//float4 t_vertex : TEXCOORD1;
	};

	//sampler2D _MainTex;
	float4 _MainTex_ST;

	// https://www.alanzucconi.com/2016/10/24/arrays-shaders-unity-5-4/
	float4 _UV0[4];
	float4 _UV1[4];
	float4 _UV2[4];
	float4 _UV3[4];
	float4 _UV4[4];
	float4 _UV5[4];
	float4 _UV6[4];
	float4 _UV7[4];

	v2f vert(appdata v) {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		//o.t_vertex = v.vertex;
		o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
		UNITY_TRANSFER_FOG(o, o.vertex);
		return o;
	}

	UNITY_DECLARE_TEX2DARRAY(_MainTex);

	fixed4 frag(v2f i) : SV_Target
	{
		// === UV 0

		float2 deltaXLeft = lerp(_UV0[0].x, _UV0[1].x, i.uv_MainTex.x);
		float2 deltaXRight = lerp(_UV0[3].x, _UV0[2].x, i.uv_MainTex.x);
		float UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		float2 deltaYBot = lerp(_UV0[0].y, _UV0[3].y, i.uv_MainTex.y);
		float2 deltaYTop = lerp(_UV0[1].y, _UV0[2].y, i.uv_MainTex.y);
		float UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		float hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		float numHits = hit;

		float3 uv = float3(UVx, UVy, 0);
		float4 texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, uv) * hit;
		float4 sumTex = texCont;

		// === UV 1

		deltaXLeft = lerp(_UV1[0].x, _UV1[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV1[3].x, _UV1[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV1[0].y, _UV1[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV1[1].y, _UV1[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 1)) * hit;
		sumTex = texCont;

		// === UV 2

		deltaXLeft = lerp(_UV2[0].x, _UV2[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV2[3].x, _UV2[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV2[0].y, _UV2[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV2[1].y, _UV2[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 2)) * hit;
		sumTex = texCont;

		// === UV 3

		deltaXLeft = lerp(_UV3[0].x, _UV3[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV3[3].x, _UV3[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV3[0].y, _UV3[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV3[1].y, _UV3[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 3)) * hit;
		sumTex = texCont;

		// === UV 4

		deltaXLeft = lerp(_UV4[0].x, _UV4[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV4[3].x, _UV4[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV4[0].y, _UV4[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV4[1].y, _UV4[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 4)) * hit;
		sumTex = texCont;

		// === UV 5

		deltaXLeft = lerp(_UV5[0].x, _UV5[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV5[3].x, _UV5[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV5[0].y, _UV5[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV5[1].y, _UV5[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 5)) * hit;
		sumTex = texCont;

		// === UV 6

		deltaXLeft = lerp(_UV6[0].x, _UV6[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV6[3].x, _UV6[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV6[0].y, _UV6[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV6[1].y, _UV6[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 6)) * hit;
		sumTex = texCont;

		// === UV 7

		deltaXLeft = lerp(_UV7[0].x, _UV7[1].x, i.uv_MainTex.x);
		deltaXRight = lerp(_UV7[3].x, _UV7[2].x, i.uv_MainTex.x);
		UVx = i.uv_MainTex.x * (step(0, deltaXLeft) * step(deltaXRight, 1));
		deltaYBot = lerp(_UV7[0].y, _UV7[3].y, i.uv_MainTex.y);
		deltaYTop = lerp(_UV7[1].y, _UV7[2].y, i.uv_MainTex.y);
		UVy = i.uv_MainTex.y * (step(0, deltaYBot) * step(deltaYTop, 1));

		hit = step(0, deltaXLeft) * step(deltaXRight, 1) * step(0, deltaYBot) * step(deltaYTop, 1);
		numHits = numHits + hit;
		texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(UVx, UVy, 7)) * hit;
		sumTex = texCont;

		// Divide total texture found by number of hits found
		fixed4 c = sumTex / numHits;

		return c;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}