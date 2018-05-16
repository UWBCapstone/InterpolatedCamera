Shader "Custom/InterpolatedCameraShader_BoxFilterWeighing" {
	Properties
	{
		//_MainTex("Texture", 2DArray) = "" {}
		_Tex0("Texture", 2D) = "white" {}
		_Tex1("Texture", 2D) = "white" {}
		_Tex2("Texture", 2D) = "white" {}
		_Tex3("Texture", 2D) = "white" {}
		_Tex4("Texture", 2D) = "white" {}
		_Tex5("Texture", 2D) = "white" {}
		_Tex6("Texture", 2D) = "white" {}
		_Tex7("Texture", 2D) = "white" {}
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
//#pragma target 3.5 // May be able to remove
#pragma multi_compile_fog

#include "UnityCG.cginc"

		// https://forum.unity.com/threads/how-to-declare-global-constant-in-cg.280920/
		static const float MAX_CAMERAS = float(8);

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uvTex0 : TEXCOORD0;
		float2 uvTex1 : TEXCOORD1;
		float2 uvTex2 : TEXCOORD2;
		float2 uvTex3 : TEXCOORD3;
		float2 uvTex4 : TEXCOORD4;
		float2 uvTex5 : TEXCOORD5;
		float2 uvTex6 : TEXCOORD6;
		float2 uvTex7 : TEXCOORD7;
		UNITY_FOG_COORDS(1)
		float4 vertex : SV_POSITION;
	};

	//sampler2D _MainTex;
	sampler2D _Tex0;
	sampler2D _Tex1;
	sampler2D _Tex2;
	sampler2D _Tex3;
	sampler2D _Tex4;
	sampler2D _Tex5;
	sampler2D _Tex6;
	sampler2D _Tex7;

	//float4 _MainTex_ST;
	float4 _Tex0_ST;
	float4 _Tex1_ST;
	float4 _Tex2_ST;
	float4 _Tex3_ST;
	float4 _Tex4_ST;
	float4 _Tex5_ST;
	float4 _Tex6_ST;
	float4 _Tex7_ST;

	// https://www.alanzucconi.com/2016/10/24/arrays-shaders-unity-5-4/
	float4 _UV0[4];
	float4 _UV1[4];
	float4 _UV2[4];
	float4 _UV3[4];
	float4 _UV4[4];
	float4 _UV5[4];
	float4 _UV6[4];
	float4 _UV7[4];

	float4 _BoxFilterWeights0[16];
	float4 _BoxFilterWeights1[16];
	float4 _BoxFilterWeights2[16];
	float4 _BoxFilterWeights3[16];
	float4 _BoxFilterWeights4[16];
	float4 _BoxFilterWeights5[16];
	float4 _BoxFilterWeights6[16];
	float4 _BoxFilterWeights7[16];

	/*float2 uvTex1;
	float2 uvTex2;
	float2 uvTex3;
	float2 uvTex4;
	float2 uvTex5;
	float2 uvTex6;
	float2 uvTex7;*/

	v2f vert(appdata v) {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uvTex0 = TRANSFORM_TEX(v.uv, _Tex0);
		UNITY_TRANSFER_FOG(o, o.vertex);

		o.uvTex1 = TRANSFORM_TEX(v.uv, _Tex1);
		o.uvTex2 = TRANSFORM_TEX(v.uv, _Tex2);
		o.uvTex3 = TRANSFORM_TEX(v.uv, _Tex3);
		o.uvTex4 = TRANSFORM_TEX(v.uv, _Tex4);
		o.uvTex5 = TRANSFORM_TEX(v.uv, _Tex5);
		o.uvTex6 = TRANSFORM_TEX(v.uv, _Tex6);
		o.uvTex7 = TRANSFORM_TEX(v.uv, _Tex7);

		return o;
	}

	//UNITY_DECLARE_TEX2DARRAY(_MainTex);

	fixed4 frag(v2f i) : SV_Target
	{
		// === UV 0
/*
		float2 deltaXLeft = lerp(_UV0[0].x, _UV0[1].x, i.uvTex0.x);
		float2 deltaXRight = lerp(_UV0[3].x, _UV0[2].x, i.uvTex0.x);
		float UVx = i.uvTex0.x * (step(0, deltaXLeft.x) * step(deltaXRight.x, 1));
		float2 deltaYBot = lerp(_UV0[0].y, _UV0[3].y, i.uvTex0.y);
		float2 deltaYTop = lerp(_UV0[1].y, _UV0[2].y, i.uvTex0.y);
		float UVy = i.uvTex0.y * (step(0, deltaYBot.y) * step(deltaYTop.y, 1));*/

		// lerp(_UV0[0].x, _UV0[1].x, 1) -> get average of left x value
		// lerp(_UV0[3].x, _UV0[2].x, 1) -> get average of right x value
		// interpolate between these to get UVx = lerp(left, right, i.uvTex0.x);
		// keep hit the way it is, that's the only place where step is applied

		float2 leftX = lerp(_UV0[0].x, _UV0[1].x, 1);
		float2 rightX = lerp(_UV0[3].x, _UV0[2].x, 1);
		float UVx = lerp(leftX, rightX, i.uvTex0.x);
		float2 bottomY = lerp(_UV0[0].y, _UV0[3].y, 1);
		float2 topY = lerp(_UV0[1].y, _UV0[2].y, 1);
		float UVy = lerp(bottomY, topY, i.uvTex0.y);

		//float hit = step(0, deltaXLeft.x) * step(deltaXRight.x, 1) * step(0, deltaYBot.y) * step(deltaYTop.y, 1);
		//float hit = 1;
		float hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		float numHits = hit;

		//float3 uv = float3(UVx, UVy, 0);
		//float4 texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, uv) * hit;
		float2 uv = float2(UVx, UVy);
		float4 texCont = tex2D(_Tex0, uv) * hit;
		float4 sumTex = texCont;

		//// === UV 1

		leftX = lerp(_UV1[0].x, _UV1[1].x, 1);
		rightX = lerp(_UV1[3].x, _UV1[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex1.x);
		bottomY = lerp(_UV1[0].y, _UV1[3].y, 1);
		topY = lerp(_UV1[1].y, _UV1[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex1.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex1, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 2

		leftX = lerp(_UV2[0].x, _UV2[1].x, 1);
		rightX = lerp(_UV2[3].x, _UV2[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex2.x);
		bottomY = lerp(_UV2[0].y, _UV2[3].y, 1);
		topY = lerp(_UV2[1].y, _UV2[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex2.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex2, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 3

		leftX = lerp(_UV3[0].x, _UV3[1].x, 1);
		rightX = lerp(_UV3[3].x, _UV3[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex3.x);
		bottomY = lerp(_UV3[0].y, _UV3[3].y, 1);
		topY = lerp(_UV3[1].y, _UV3[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex3.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex3, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 4

		leftX = lerp(_UV4[0].x, _UV4[1].x, 1);
		rightX = lerp(_UV4[3].x, _UV4[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex4.x);
		bottomY = lerp(_UV4[0].y, _UV4[3].y, 1);
		topY = lerp(_UV4[1].y, _UV4[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex4.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex4, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 5

		leftX = lerp(_UV5[0].x, _UV5[1].x, 1);
		rightX = lerp(_UV5[3].x, _UV5[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex5.x);
		bottomY = lerp(_UV5[0].y, _UV5[3].y, 1);
		topY = lerp(_UV5[1].y, _UV5[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex5.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex5, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 6

		leftX = lerp(_UV6[0].x, _UV6[1].x, 1);
		rightX = lerp(_UV6[3].x, _UV6[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex6.x);
		bottomY = lerp(_UV6[0].y, _UV6[3].y, 1);
		topY = lerp(_UV6[1].y, _UV6[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex6.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex6, uv) * hit;
		sumTex = sumTex + texCont;

		// === UV 7

		leftX = lerp(_UV7[0].x, _UV7[1].x, 1);
		rightX = lerp(_UV7[3].x, _UV7[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex7.x);
		bottomY = lerp(_UV7[0].y, _UV7[3].y, 1);
		topY = lerp(_UV7[1].y, _UV7[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex7.y);

		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		numHits = numHits + hit;

		uv = float2(UVx, UVy);
		texCont = tex2D(_Tex7, uv) * hit;
		sumTex = sumTex + texCont;

		// Divide total texture found by number of hits found
		fixed4 c = sumTex / numHits;

		//fixed4 c = tex2D(_Tex7, i.uvTex7);

		return c;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}