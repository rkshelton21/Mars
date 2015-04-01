// ColorTint shader v.1.0
// Copyright (C) 2014 Sergey Taraban <http://staraban.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

Shader "Particles/ColorTint" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	_TintColorRed ("Tint Color Red", Color) = (0.5,0.5,0.5,0.5)
	_TintColorGreen ("Tint Color Green", Color) = (0.5,0.5,0.5,0.5)
	_TintColorBlue ("Tint Color Blue", Color) = (0.5,0.5,0.5,0.5)	
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}

	// ---- Fragment program cards
	SubShader {
		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColorRed;
			fixed4 _TintColorGreen;
			fixed4 _TintColorBlue;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;

			fixed4 frag (v2f i) : COLOR
			{	
				float4 baseColor = tex2D(_MainTex, i.texcoord);
				float alpha = baseColor.a;
				baseColor = alpha * (baseColor.r * _TintColorRed + baseColor.g * _TintColorGreen + baseColor.b * _TintColorBlue);
				baseColor.r = 1.0f;// - step(alpha, 0.1);
				baseColor.g = 0.0f;// - step(alpha, 0.1);
				baseColor.b = 0.0f;// - step(alpha, 0.1);
				baseColor.a = 1.0f;// - step(alpha, 0.1);
				return baseColor;
			}
			ENDCG 
		}
	}
}
}