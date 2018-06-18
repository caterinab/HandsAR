Shader "Custom/ButtonShader" {
	SubShader {
		Tags { "RenderQueue"="Geometry+1" }
		ZTest Always
		Pass{
			Blend One Zero
		}
	}
}
