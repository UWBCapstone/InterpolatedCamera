Shader "Unlit/LinePlaneIntersect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				// Line plane intersect logic
				// public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec (dir), Vector3 planeNormal, Vector3 planePoint){
			
				// To get the plane normal of a frustum, we need to say that 
				float4 linePoint;
			float4 planeNormal;
			float4 lineVec;
				float4 planePoint;
				// calculate the distance between the linePoint and the line-plane intersection point
			float dotNumerator = dot((planePoint - linePoint), planeNormal);
			float dotDenominator = dot(lineVec, planeNormal); // Assumes lineVec is normalized

			// line and plane are not parallel
			float dis = dotNumerator / dotDenominator; // distance of the vector to the intersection point
			// calculate the distance between the ray origin and the intersection point
			float4 intersection = linePoint + dis * lineVec;

			// step(edge, number);
			intersection = step(0.001, dotDenominator) * intersection;
			// Determine if the intersection actually happened

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
