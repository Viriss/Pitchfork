Shader "Pitchfork/ColorIdentity" {
	Properties{
		_MainTex("First Color texture", 2D) = "white" {}
		_SecondTex("Second Color texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Perc("Percentage of mana value", Float) = 0.4
	}

	SubShader{
		Tags{ "Queue" = "Transparent" }

		Pass{
			Cull Back						// now render the front faces
			ZWrite Off						// don't write to depth buffer 
											// in order not to occlude other objects
			Blend SrcAlpha OneMinusSrcAlpha // blend based on the fragment's alpha value

			CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag 

				uniform sampler2D _MainTex;
				uniform sampler2D _SecondTex;
				uniform sampler2D _Mask;
				float _Perc;

				struct vertexInput {
					float4 vertex : POSITION;
					float4 texcoord : TEXCOORD0;
				};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float4 tex : TEXCOORD0;
				};

				vertexOutput vert(vertexInput input)
				{
					vertexOutput output;

					output.tex = input.texcoord;
					output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
					return output;
				}

				float4 frag(vertexOutput input) : COLOR
				{
					float4 textureColor = tex2D(_MainTex, input.tex.xy);
					float4 altColor = tex2D(_SecondTex, input.tex.xy);
					float4 maskColor = tex2D(_Mask, input.tex.xy);
					float BlendPerc = 0.7;

					//Show full color
					textureColor = lerp(textureColor, altColor, 1 - maskColor.r);

					if (input.tex.y > _Perc)
					{
						//Dim the color
						textureColor.rgb = lerp(textureColor, dot(textureColor.rgb, float3(0.3, 0.59, 0.11)), BlendPerc);
					}
					else {
						float diff = input.tex.y - (_Perc * 0.85);
						if (_Perc < 1 && diff > 0)
						{
							textureColor.rgb = textureColor.rgb * (1 + (diff * 15));
						}
					}

					return textureColor;
				}

			ENDCG
			}
		}

	Fallback "Unlit/Transparent"
}