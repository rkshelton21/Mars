Shader "Custom/SolidColor" {
	Properties {
		_MainTex ("Image", 2D) = "white" {}		
		_Color ("Color", Color) = (1,1,1,1)
		_Alpha ("Alpha", Range(0,1)) = 1
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
			fixed4 _Color;
			float _Alpha;
			
            fixed4 frag(v2f_img i) : SV_Target {
                float4 result = tex2D(_MainTex, i.uv);
                result.r = _Color.r;
                result.g = _Color.g;
                result.b = _Color.b;                
                if(result.a > 0.0f)
                {
                	result.a = _Alpha;                
        		}
				return result;		
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
