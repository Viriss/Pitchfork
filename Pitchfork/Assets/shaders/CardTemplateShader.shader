Shader "Pitchfork/CardTemplate" {
	Properties{
		_MainTex("Unit texture", 2D) = "white" {}
		_FacePlateTex("Faceplate texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Transparent" }

		Pass{
		Cull Back // now render the front faces
		ZWrite Off // don't write to depth buffer 
				   // in order not to occlude other objects
		Blend SrcAlpha OneMinusSrcAlpha
		// blend based on the fragment's alpha value

		CGPROGRAM

		#pragma vertex vert  
		#pragma fragment frag 

		uniform sampler2D _MainTex;
		uniform sampler2D _FacePlateTex;
		uniform sampler2D _Mask;

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
			float4 altColor = tex2D(_FacePlateTex, input.tex.xy);
			float4 maskColor = tex2D(_Mask, input.tex.xy);

			if (maskColor.r > 0.2)
			{
				textureColor.a = 0;
			}
			if (altColor.a > 0.1)
			{
				textureColor = altColor;
			}

			return textureColor;
		}

			ENDCG
		}
	}

	Fallback "Unlit/Transparent"
}