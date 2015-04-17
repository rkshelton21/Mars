Shader "Custom/LightMask" {
Properties {    
    _MainTex ("Texture", 2D) = "white" { }    
    _RangeCount ("RangeCount", Range(1,10)) = 4.0
    _Tint ("Tint", color) = (0.26,0.19,0.16,0.0)
    _AmbientCutOff ("CuttOff", Range(0,1)) = 1
}
/// <summary>
/// Multiple metaball shader.
/// 
/// Separates each particle by color and overrides it with the one specified.
/// Notice the texture that passes through this shader only looks at particles, and has a black background.
/// The core element for the color merging is the floor function, try tweaking it to achive the desired result.
///
/// Visit: www.codeartist.mx for more stuff. Thanks for checking out this example.
/// Credit: Rodrigo Fernandez Diaz
/// Contact: q_layer@hotmail.com
/// </summary>
SubShader {
	Tags {"Queue" = "Transparent" }
    Pass {
    Blend SrcAlpha OneMinusSrcAlpha     
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag	
	#include "UnityCG.cginc"	
	float4 _Color;
	sampler2D _MainTex;
	float _RangeCount;
	float _AmbientCutOff;
	float4 _Tint;
		
	struct v2f {
	    float4  pos : SV_POSITION;
	    float2  uv : TEXCOORD0;
	};	
	float4 _MainTex_ST;		
	v2f vert (appdata_base v){
	    v2f o;
	    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	    return o;
	}	
	
	// Here goes the metaball magic
	float COLOR_TRESHHOLD=0.1; //To separate and process each color.		
	half4 frag (v2f i) : COLOR{		
		half4 texcol= tex2D (_MainTex, i.uv); 
		half4 finalColor= texcol;					
		//This is for the water effect
		//finalColor=half4(0.0,texcol.g/2.0,texcol.b,0.5);
		//finalColor.b=floor((finalColor.b/0.2)*0.5);
		//finalColor.a=min(0.1, texcol.a);
		
		texcol.a = 1;
		float a = 0.5;
		if(texcol.r > 0.01)
		{
			//texcol.r = 1;
			texcol.a = a;
		}
		if(texcol.g > 0.01)
		{
			//texcol.g = 1;
			texcol.a = a;
		}
		if(texcol.b > 0.01)
		{
			//texcol.b = 1;
			texcol.a = a;
		}
		
		float n = _RangeCount;
		float a2 = floor((finalColor.a/0.2)*0.5);
		finalColor.r = floor(finalColor.r / (1/n)) / n;
		finalColor.g = floor(finalColor.g / (1/n)) / n;
		finalColor.b = floor(finalColor.b / (1/n)) / n;
		finalColor *= finalColor * _Tint;		
		finalColor.a= min(texcol.a, _AmbientCutOff);
		return finalColor;
	}
	ENDCG

    }
}
Fallback "VertexLit"
} 






