Shader "Custom/InterpolatedCameraShader_SimpleWeighing" {
	Properties
	{
		_ViewCamPos("Viewing Camera Position", Vector) = (0,0,0,0)
		_ViewCamDir("Viewing Camera Direction", Vector) = (0,0,0,0) // Assumes this is normalized
		//_MainTex("Texture", 2DArray) = "" {}
		_Tex0("Texture", 2D) = "white" {}
	_CamPos0("Camera Position 0", Vector) = (0,0,0,0)
		_CamDir0("Camera Direction 0", Vector) = (0,0,0,0)
	_Tex1("Texture", 2D) = "white" {}
	_CamPos1("Camera Position 1", Vector) = (0,0,0,0)
		_CamDir1("Camera Direction 1", Vector) = (0,0,0,0)
	_Tex2("Texture", 2D) = "white" {}
	_CamPos2("Camera Position 2", Vector) = (0,0,0,0)
		_CamDir2("Camera Direction 2", Vector) = (0,0,0,0)
	_Tex3("Texture", 2D) = "white" {}
	_CamPos3("Camera Position 3", Vector) = (0,0,0,0)
		_CamDir3("Camera Direction 3", Vector) = (0,0,0,0)
	_Tex4("Texture", 2D) = "white" {}
	_CamPos4("Camera Position 4", Vector) = (0,0,0,0)
		_CamDir4("Camera Direction 4", Vector) = (0,0,0,0)
	_Tex5("Texture", 2D) = "white" {}
	_CamPos5("Camera Position 5", Vector) = (0,0,0,0)
		_CamDir5("Camera Direction 5", Vector) = (0,0,0,0)
	_Tex6("Texture", 2D) = "white" {}
	_CamPos6("Camera Position 6", Vector) = (0,0,0,0)
		_CamDir6("Camera Direction 6", Vector) = (0,0,0,0)
	_Tex7("Texture", 2D) = "white" {}
	_CamPos7("Camera Position 7", Vector) = (0,0,0,0)
		_CamDir7("Camera Direction 7", Vector) = (0,0,0,0)
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

	// Weighing functionality

	float4 _ViewCamPos;
	float4 _ViewCamDir;

	float4 _CamPos0;
	float4 _CamPos1;
	float4 _CamPos2;
	float4 _CamPos3;
	float4 _CamPos4;
	float4 _CamPos5;
	float4 _CamPos6;
	float4 _CamPos7;

	float4 _CamDir0;
	float4 _CamDir1;
	float4 _CamDir2;
	float4 _CamDir3;
	float4 _CamDir4;
	float4 _CamDir5;
	float4 _CamDir6;
	float4 _CamDir7;

	float _cam0Angle;
	float _cam1Angle;
	float _cam2Angle;
	float _cam3Angle;
	float _cam4Angle;
	float _cam5Angle;
	float _cam6Angle;
	float _cam7Angle;

	float4 _cam0Up;
	float4 _cam1Up;
	float4 _cam2Up;
	float4 _cam3Up;
	float4 _cam4Up;
	float4 _cam5Up;
	float4 _cam6Up;
	float4 _cam7Up;

	float4 _cam0Right;
	float4 _cam1Right;
	float4 _cam2Right;
	float4 _cam3Right;
	float4 _cam4Right;
	float4 _cam5Right;
	float4 _cam6Right;
	float4 _cam7Right;

	float _cam0Near;
	float _cam1Near;
	float _cam2Near;
	float _cam3Near;
	float _cam4Near;
	float _cam5Near;
	float _cam6Near;
	float _cam7Near;

	float _cam0Far;
	float _cam1Far;
	float _cam2Far;
	float _cam3Far;
	float _cam4Far;
	float _cam5Far;
	float _cam6Far;
	float _cam7Far;

	float _cam0Aspect;
	float _cam1Aspect;
	float _cam2Aspect;
	float _cam3Aspect;
	float _cam4Aspect;
	float _cam5Aspect;
	float _cam6Aspect;
	float _cam7Aspect;

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

		////float hit = step(0, deltaXLeft.x) * step(deltaXRight.x, 1) * step(0, deltaYBot.y) * step(deltaYTop.y, 1);
		////float hit = 1;
		//float hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		//float numHits = hit;

		////float3 uv = float3(UVx, UVy, 0);
		////float4 texCont = UNITY_SAMPLE_TEX2DARRAY(_MainTex, uv) * hit;
		float2 uv = float2(UVx, UVy);
		//float4 texCont = tex2D(_Tex0, uv) * hit;
		//float4 sumTex = texCont;

		// 1) Calculate the opposite lengths (i.e. the width and height of the plane (divided by 2))
		float oppFarHeight = _cam0Far * tan(radians(_cam0Angle));
		float oppFarWidth = _cam0Aspect * oppFarHeight;
		float4 centFar = _CamPos0 + _CamDir0 * _cam0Far;
		float oppNearHeight = _cam0Near * tan(radians(_cam0Angle));
		float oppNearWidth = _cam0Near * oppNearHeight;
		float4 centNear = _CamPos0 + _CamDir0 * _cam0Near;

		// 2) Calculate the frustum corners
		float4 frustumFarLL = centFar - _cam0Right * oppFarWidth - _cam0Up * oppFarHeight;
		float4 frustumFarUL = centFar - _cam0Right * oppFarWidth + _cam0Up * oppFarHeight;
		float4 frustumFarUR = centFar + _cam0Right * oppFarWidth + _cam0Up * oppFarHeight;
		float4 frustumFarLR = centFar + _cam0Right * oppFarWidth - _cam0Up * oppFarHeight;
		float4 frustumNearLL = centNear - _cam0Right * oppNearWidth - _cam0Up * oppNearHeight;
		float4 frustumNearUL = centNear - _cam0Right * oppNearWidth + _cam0Up * oppNearHeight;
		float4 frustumNearUR = centNear + _cam0Right * oppNearWidth + _cam0Up * oppNearHeight;
		float4 frustumNearLR = centNear + _cam0Right * oppNearWidth - _cam0Up * oppNearHeight;

		// 3) Calculate frustum plane normals
		float3 flTemp = cross(frustumNearLL, frustumNearUL);
		float4 fLNorm = float4(flTemp.x, flTemp.y, flTemp.z, 0);
		//float4 fLNorm = cross(float3(frustumNearLL.x, frustumNearLL.y, frustumNearLL.z), float3(frustumNearUL.x, frustumNearUL.y, frustumNearUL.z));
		flTemp = cross(frustumNearUL, frustumNearUR);
		float4 fTNorm = float4(flTemp.x, flTemp.y, flTemp.z, 0);
		flTemp = cross(frustumNearUR, frustumNearLR);
		float4 fRNorm = float4(flTemp.x, flTemp.y, flTemp.z, 0);
		flTemp = cross(frustumNearLR, frustumNearLL);
		float4 fBNorm = float4(flTemp.x, flTemp.y, flTemp.z, 0);

		// 4) Calculate intersection position of viewing camera dir and all 4 frustum planes
		// ******************************************************************************************************************
		float4 planePoint = frustumFarLL;
		float4 planeNormal = fLNorm;
		float4 nearPlanePoint = frustumNearLL;

		// 4aL) calculate the distance between the linePoint and the line-plane intersection point
		float dotNumerator = dot((planePoint - _ViewCamPos), planeNormal);
		float dotDenominator = dot(_ViewCamDir, planeNormal); // Assumes lineVec is normalized
		float dis = dotNumerator / dotDenominator; // distance of the float4to the intersection point
		// 4bL) calculate the intersection point from the given info
		float4 intersection = _ViewCamPos + dis * _ViewCamDir;
		// 4cL) calculate if the frustum was actually intersected
		float fLIntersected = step(0.001, dotDenominator);
		// 4dL) set the intersection to the tracked frustum intersection point vector
		float4 fLIntersection = intersection;
		// 4eL) set the intersected bool to be false if the distance intersection lies beyond the far clip plane or before the near clip plane
		float4 compA = intersection - nearPlanePoint;
		float4 compB = planePoint - nearPlanePoint;
		// 4fL) see if they are pointed in the same direction
		fLIntersected = step(0.001, dot(compA, compB)) * fLIntersected;
		// 4gL) see if the intersection actually lies on a plane created by the frustum (is nearer than the far plane point)
		fLIntersected = step((length(compB) - length(compA)), 0.001) * fLIntersected;

		// ******************************************************************************************************************
		planePoint = frustumFarUL;
		planeNormal = fTNorm;
		nearPlanePoint = frustumNearUL;

		// 4aT) calculate the distance between the linePoint and the line-plane intersection point
		dotNumerator = dot((planePoint - _ViewCamPos), planeNormal);
		dotDenominator = dot(_ViewCamDir, planeNormal); // Assumes lineVec is normalized
		dis = dotNumerator / dotDenominator; // distance of the float4to the intersection point
		// 4bT) calculate the intersection point from the given info
		intersection = _ViewCamPos + dis * _ViewCamDir;
		// 4cT) calculate if the frustum was actually intersected
		float4 fTIntersected = step(0.001, dotDenominator);
		// 4dT) set the intersection to the tracked frustum intersection point vector
		float4 fTIntersection = intersection;
		// 4eT) set the intersected bool to be false if the distance intersection lies beyond the far clip plane or before the near clip plane
		compA = intersection - nearPlanePoint;
		compB = planePoint - nearPlanePoint;
		// 4fT) see if they are pointed in the same direction
		fTIntersected = step(0.001, dot(compA, compB)) * fTIntersected;
		// 4gT) see if the intersection actually lies on a plane created by the frustum (is nearer than the far plane point)
		fTIntersected = step((length(compB) - length(compA)), 0.001) * fTIntersected;

		// ******************************************************************************************************************
		planePoint = frustumFarUR;
		planeNormal = fRNorm;
		nearPlanePoint = frustumNearUR;

		// 4aR) calculate the distance between the linePoint and the line-plane intersection point
		dotNumerator = dot((planePoint - _ViewCamPos), planeNormal);
		dotDenominator = dot(_ViewCamDir, planeNormal); // Assumes lineVec is normalized
		dis = dotNumerator / dotDenominator; // distance of the float4to the intersection point
		// 4bR) calculate the intersection point from the given info
		intersection = _ViewCamPos + dis * _ViewCamDir;
		// 4cR) calculate if the frustum was actually intersected
		float4 fRIntersected = step(0.001, dotDenominator);
		// 4dR) set the intersection to the tracked frustum intersection point vector
		float4 fRIntersection = intersection;
		// 4eR) set the intersected bool to be false if the distance intersection lies beyond the far clip plane or before the near clip plane
		compA = intersection - nearPlanePoint;
		compB = planePoint - nearPlanePoint;
		// 4fR) see if they are pointed in the same direction
		fRIntersected = step(0.001, dot(compA, compB)) * fLIntersected;
		// 4gR) see if the intersection actually lies on a plane created by the frustum (is nearer than the far plane point)
		fRIntersected = step((length(compB) - length(compA)), 0.001) * fRIntersected;

		// ******************************************************************************************************************
		planePoint = frustumFarLR;
		planeNormal = fBNorm;
		nearPlanePoint = frustumNearLR;

		// 4aB) calculate the distance between the linePoint and the line-plane intersection point
		dotNumerator = dot((planePoint - _ViewCamPos), planeNormal);
		dotDenominator = dot(_ViewCamDir, planeNormal); // Assumes lineVec is normalized
		dis = dotNumerator / dotDenominator; // distance of the float4to the intersection point
		// 4bB) calculate the intersection point from the given info
		intersection = _ViewCamPos + dis * _ViewCamDir;
		// 4cB) calculate if the frustum was actually intersected
		float4 fBIntersected = step(0.001, dotDenominator);
		// 4dB) set the intersection to the tracked frustum intersection point vector
		float4 fBIntersection = intersection;
		// 4eB) set the intersected bool to be false if the distance intersection lies beyond the far clip plane or before the near clip plane
		compA = intersection - nearPlanePoint;
		compB = planePoint - nearPlanePoint;
		// 4fB) see if they are pointed in the same direction
		fBIntersected = step(0.001, dot(compA, compB)) * fBIntersected;
		// 4gB) see if the intersection actually lies on a plane created by the frustum (is nearer than the far plane point)
		fBIntersected = step((length(compB) - length(compA)), 0.001) * fBIntersected;

		// 5) Distill the intersection positions into one
		// Assumes an intersection position has a positive value
		intersection = step(0.001, fLIntersected) * fLIntersection + step(0.001, fTIntersected) * fTIntersection + step(0.001, fRIntersected) * fRIntersection + step(0.001, fBIntersected) * fBIntersection;

		// 6) Calculate relative opposite distance & adjacent distance
		// this opposite and adjacent represent the plane if the intersection point was on the clip plane
		float newOpp = (intersection - _CamPos0) * sin(radians(_cam0Angle));
		float newAdj = (intersection - _CamPos0) * cos(radians(_cam0Angle));
		float newCent = _CamPos0 + _CamDir0 * newAdj;

		// 7) Calculate which oppFar to use
		// When using the opposite far value, we have to consider whether the new opp value
		// is representative of the width or the height (since aspect ratio is a thing)
		float oppFar = step(0.001, (step(0.001, fLIntersected) + step(0.001, fRIntersected))) * oppFarWidth + step(0.001, (step(0.001, fTIntersected) + step(0.001, fBIntersected))) * oppFarHeight;
		int validIntersected = step(0.001, oppFar);

		// 8) Calculate the opposites' ratio (linearly scales from 0 at the far ends of the frustum to 1 at the center of the frustum)
		float oppRatio = (oppFar - newOpp) / oppFar;
		oppRatio = validIntersected * oppRatio; // Handle cases where the frustum isn't intersected at all by setting its value to 0

		// 9) Sum contribution of oppRatio
		float4 texCont = tex2D(_Tex0, uv) * oppRatio;
		float4 sumTex = texCont;
		float hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		float numHits = hit;
		
		//// === UV 1

		leftX = lerp(_UV1[0].x, _UV1[1].x, 1);
		rightX = lerp(_UV1[3].x, _UV1[2].x, 1);
		UVx = lerp(leftX, rightX, i.uvTex1.x);
		bottomY = lerp(_UV1[0].y, _UV1[3].y, 1);
		topY = lerp(_UV1[1].y, _UV1[2].y, 1);
		UVy = lerp(bottomY, topY, i.uvTex1.y);

		//hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1);
		//numHits = numHits + hit;

		uv = float2(UVx, UVy);
		//texCont = tex2D(_Tex1, uv) * hit;
		//sumTex = sumTex + texCont;

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

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

		// -- REPLACE LINE HERE

		texCont = tex2D(_Tex1, uv) * oppRatio;
		sumTex = sumTex + texCont;
		hit = step(0, UVx) * step(UVx, 1) * step(0, UVy) * step(UVy, 1) * step(0, oppRatio);
		numHits = numHits + hit;

		// === Divide total texture found by number of hits found
		fixed4 c = sumTex / numHits;

		//fixed4 c = tex2D(_Tex7, i.uvTex7);

		return c;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}