﻿Shader "Custom/SkinOnly" {
	Properties{
		//_Color("Color", Color) = (1,1,1)
		_TransparentColor("Transparent Color", Color) = (0,0,0)
		_Threshold("Threshold", Float) = 0.1
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" }
		//Tags{ "Queue" = "Geometry-1" "ForceNoShadowCasting" = "true" "IgnoreProjector" = "true" }  // queue = 1999 
		LOD 200
		ZTest Always
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed4 _TransparentColor;
		half _Threshold;
		void surf(Input IN, inout SurfaceOutput o) {
			// Read color from the texture
			half4 c = tex2D(_MainTex, IN.uv_MainTex);

			// Output colour will be the texture color * the vertex colour
			//half4 output_col = c * _Color;

			//calculate the difference between the texture color and the transparent color
			//note: we use 'dot' instead of length(transparent_diff) as its faster, and
			//although it'll really give the length squared, its good enough for our purposes!
			half3 transparent_diff = c.rgb - _TransparentColor.rgb;
			half transparent_diff_squared = dot(transparent_diff,transparent_diff);

			//if colour is too close to the transparent one, discard it.
			//note: you could do cleverer things like fade out the alpha
			if (transparent_diff_squared > _Threshold)
			{
				//o.Emission = output_col.rgb;
				o.Emission = c.rgb;
				o.Alpha = 1;
			}
			else {
				o.Alpha = 0;
			}
			//clip(transparent_diff_squared - _Threshold);

			//output albedo and alpha just like a normal shader
			//o.Emission = output_col.rgb;
			//o.Albedo = output_col.rgb;
		}
		
		ENDCG
	}
		FallBack "Diffuse"
}