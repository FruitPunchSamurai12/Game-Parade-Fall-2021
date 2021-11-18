Shader "Custom/SilhouetteShader" 
{
	 Properties 
     {
         _Color ("Main Color", Color) = (1,1,1,1)
     }
     
     Category 
     {
         SubShader 
         { 
             Tags { "Queue"="Overlay+1" }
     
             Pass
             {
                 ZWrite Off
                 ZTest Greater
                 Lighting Off
                 Color [_Color]
             }
         }
     }
     
     FallBack "Specular", 1
}