// draws only to depth buffer and before any other object so that they become obscured 
Shader "Custom/DepthMask" 
{ 
	SubShader { 
		Tags { "Queue"="Geometry-1" "ForceNoShadowCasting"="true" "IgnoreProjector"="true"}  // queue = 1999 
		Pass { 
		  Blend Zero One 
		} 
	}
}
