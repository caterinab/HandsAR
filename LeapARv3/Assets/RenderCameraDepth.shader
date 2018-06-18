// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RenderCameraDepth"
{
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 screenuv : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.screenuv = ComputeScreenPos(o.pos);
				return o;
			}

			sampler2D_float _CameraDepthTexture;

			half3 frag(v2f i) : COLOR
			{
				float depthValue = 1 - Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenuv)).x);
				return half3(depthValue, depthValue, depthValue);
			}
			ENDCG
		}
	}
}