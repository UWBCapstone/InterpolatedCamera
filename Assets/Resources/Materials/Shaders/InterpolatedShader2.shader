Shader "Custom/InterpolatedShader2" {
	Properties
	{
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

	float4 _MainTex_ST;
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

	v2f vert(appdata v) {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv_MainTex = TRANSFORM_TEX(v.uv, _Tex0);
		UNITY_TRANSFER_FOG(o, o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 c = tex2D(_Tex0, i.uv_MainTex);

	return c;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}