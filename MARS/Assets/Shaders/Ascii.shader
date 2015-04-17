Shader "Custom/Ascii" {
	Properties {
		_MainTex ("Image", 2D) = "white" {}		
		_Map ("Ascii Map", 2D) = "white" {}
		_XR ("Test X", Range(0,1000)) = 1
		_YR ("Test Y", Range(0,1000)) = 1
		_Mask ("Mask Color", Color) = (1,0,0,1)
		_Match ("Match Color", Color) = (1,1,1,1)
		_TextColor ("Text Color", Color) = (1,1,1,1)
	}
	SubShader {
        Tags {"Queue" = "Transparent" }
    	Blend SrcAlpha OneMinusSrcAlpha     
		Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _Map;
			float _XR;
			float _YR;
			fixed4 _Mask;
			fixed4 _Match;
			fixed4 _TextColor;
			
            fixed4 frag(v2f_img i) : SV_Target {
                bool p = fmod(i.uv.x*8.0,2.0) < 1.0;
                bool q = fmod(i.uv.y*8.0,2.0) > 1.0;
                
                float c = (p && q) || !(p || q);
                
                //return fixed4(fixed3(c, c, c),1.0f);
                float numTexelsX = _XR;
                float numTexelsY = _YR;
                float pixel_width = 1.0f/numTexelsX;
				float pixel_height = 1.0f/numTexelsY;
				half2 new_uv = half2((int)(i.uv.x/pixel_width)*pixel_width, (int)(i.uv.y/pixel_height)*pixel_height);
				float rand = frac(sin( dot(float3(new_uv.x, new_uv.y, (int)(_Time.g*2)) ,float3(12.9898f,78.233f,45.5432f) )) * 43758.5453f);
				float4 pix = tex2D(_MainTex, new_uv);
				int GRAYLEVELS = 16; 
  				int gray = floor((pix.r + pix.g + pix.b)*GRAYLEVELS/3.0f);
  				float whiteBlinkPercent = 0.997f;
				if(rand > whiteBlinkPercent)
				{
					float rand2 = frac(sin( dot(float3(i.uv.x, i.uv.y, (int)(_Time.g*10)) ,float3(12.9898f,78.233f,45.5432f) )) * 43758.5453f);
					gray = floor(rand2*(GRAYLEVELS - 1));
				}
				//float percentOfPixelX = (new_uv.x / numTexelsX) - floor(new_uv.x / numTexelsX);
				//float percentOfPixelY = (new_uv.y / numTexelsY) - floor(new_uv.y / numTexelsY);
				float percentOfPixelX = (i.uv.x - (int)(i.uv.x / pixel_width)*pixel_width) / pixel_width;
				float percentOfPixelY = (i.uv.y - (int)(i.uv.y / pixel_height)*pixel_height) / pixel_height;
				float charIndexX = gray % 4;
				float charIndexY = (int)(gray / 4);
				float sizeCharX =  1 / (32.0f / 8.0f);
				float sizeCharY = 1 / (32.0f / 8.0f);
				float char_uvX = (charIndexX * sizeCharX) + sizeCharX*percentOfPixelX;
				float char_uvY = (charIndexY * sizeCharY) + sizeCharY*percentOfPixelY;
				float2 char_uv = float2(char_uvX, char_uvY);
				//char_uv = float2(_XR, _YR);
				
				//return tex2D(_MainTex, new_uv);
				//return tex2D(_Map, float2(12.0f, 20.0f));
				float4 result = tex2D(_Map, char_uv);
				result.r = _TextColor.r;
				result.g = _TextColor.g;
				result.b = _TextColor.b;
				result.a = min(pix.a, result.a);
				
				if(pix.r == _Mask.r && pix.g == _Mask.g && pix.b == _Mask.b && pix.a == _Mask.a)
				{
					result = _Match;
				}
					
				if(rand > 0.9f)
				{
					//if text background (Mask)
					if(pix.r == _Mask.r && pix.g == _Mask.g && pix.b == _Mask.b && pix.a == _Mask.a)
					{
						//flicker for text background
						result = float4(0.2f, 0.6f, 0.2f, 0.5f);				
					}
					else
					{
						if(rand > whiteBlinkPercent)
						{
							result = float4(0.9f, 1.0f, 0.9f, 1.0f);
							result.a = tex2D(_Map, char_uv).a;							
						}
						else
						{
							result *= float4(0.2f, 1.0f, 0.2f, 1.0f);
							result.a = max(0.2f, result.a);
						}
					}
				}
				
				//return tex2D(_MainTex, i.uv);
				return result;		
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
