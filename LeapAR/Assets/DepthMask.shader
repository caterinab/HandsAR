Shader "DepthMask"
{
  SubShader {
    // queue = 2001
    Tags { "Queue"="Geometry-1" }
    Pass {
      Blend Zero One
    }
  } 
}
