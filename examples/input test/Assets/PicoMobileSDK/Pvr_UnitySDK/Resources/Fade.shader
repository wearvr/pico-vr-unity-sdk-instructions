Shader "Pvr_UnitySDK/Fade" {
   Properties{  
        _Color("Color", Color) = (0,0,0,1)  
    }  
        SubShader{  
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }  
        LOD 100  
  
        ZWrite Off  
        Blend SrcAlpha OneMinusSrcAlpha  
        Color[_Color]  
  
        Pass{}  
    }  
}