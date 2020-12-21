// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PolyToots/Distort"
{
	Properties
	{
		_BumpMap("Normals", 2D) = "bump" {}
		_Distort("Distort", Range( 0 , 1)) = 0.08510161
		_MaskModifier("MaskModifier", Float) = 0.5
		_ColourCorrector("ColourCorrector", Float) = 1.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Distort;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_BumpMap);
		uniform float4 _BumpMap_ST;
		SamplerState sampler_BumpMap;
		uniform float _ColourCorrector;
		uniform float _MaskModifier;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float DistortValue33 = _Distort;
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 Norrmals29 = UnpackNormal( SAMPLE_TEXTURE2D( _BumpMap, sampler_BumpMap, uv_BumpMap ) );
			float4 screenColor14 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( (ase_grabScreenPosNorm).xy ,  0.0 ) + ( DistortValue33 * Norrmals29 ) ).xy);
			o.Albedo = ( saturate( screenColor14 ) * _ColourCorrector ).rgb;
			float2 CenteredUV15_g3 = ( i.uv_texcoord - float2( 0.5,0.5 ) );
			float2 break17_g3 = CenteredUV15_g3;
			float2 appendResult23_g3 = (float2(( length( CenteredUV15_g3 ) * 1.0 * 2.0 ) , ( atan2( break17_g3.x , break17_g3.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
			float4 clampResult74 = clamp( ( ( 1.0 - (appendResult23_g3).x ) * i.vertexColor * _MaskModifier ) , float4( 0,0,0,1 ) , float4( 1,1,1,0 ) );
			o.Alpha = clampResult74.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
206.4;525.6;1128;561;196.2385;569.7797;1.3;True;True
Node;AmplifyShaderEditor.CommentaryNode;35;-1305.307,-262.1557;Inherit;False;1186.294;406.4424;Comment;6;9;8;29;33;7;6;Sai_G's Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1227.586,23.71049;Inherit;False;Property;_Distort;Distort;1;0;Create;True;0;0;False;0;False;0.08510161;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1253.771,-206.0098;Inherit;True;Property;_BumpMap;Normals;0;0;Create;False;0;0;False;0;False;-1;None;a16d6b2842dabdd41b5c4217ab038bdb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;31;-1351.489,-1079.769;Inherit;False;1175.564;481.9544;Comment;8;13;14;12;30;15;10;11;34;Common;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-933.7353,-144.3245;Inherit;False;Norrmals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-928.7577,3.513248;Inherit;False;DistortValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;-1190.57,-806.2852;Inherit;False;33;DistortValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;32;-1050.459,431.5195;Inherit;False;969.6572;419.5354;Comment;6;26;27;28;21;18;73;Masking;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1176.866,-713.8147;Inherit;False;29;Norrmals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;10;-1301.489,-1029.769;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;11;-1069.002,-1025.988;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-955.1184,-739.2311;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;21;-980.2862,494.4849;Inherit;False;Polar Coordinates;-1;;3;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;26;-707.811,463.4753;Inherit;False;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-764.2423,-852.5835;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;27;-409.4657,507.8784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;18;-739.6143,624.6629;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;73;-479.9197,765.3499;Inherit;False;Property;_MaskModifier;MaskModifier;2;0;Create;True;0;0;False;0;False;0.5;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;14;-577.6471,-858.4592;Inherit;False;Global;_GrabScreen1;Grab Screen 1;2;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;15;-340.9247,-838.8723;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-282.005,-545.1215;Inherit;False;Property;_ColourCorrector;ColourCorrector;3;0;Create;True;0;0;False;0;False;1.5;1.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-242.8021,575.7983;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;9;-691.9709,-62.1301;Inherit;False;Sai_DepthMaskedRefraction;-1;;4;a63c900487a193f41a51cf8b7e7f977f;2,40,0,103,0;2;35;FLOAT3;0,0,0;False;37;FLOAT;0.02;False;1;FLOAT2;38
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;221.2285,-669.7022;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;74;116.5348,483.5752;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,1;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;8;-315.0135,-67.71333;Inherit;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;85;495.714,-705.3635;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;PolyToots/Distort;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;6;0
WireConnection;33;0;7;0
WireConnection;11;0;10;0
WireConnection;13;0;34;0
WireConnection;13;1;30;0
WireConnection;26;0;21;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;27;0;26;0
WireConnection;14;0;12;0
WireConnection;15;0;14;0
WireConnection;28;0;27;0
WireConnection;28;1;18;0
WireConnection;28;2;73;0
WireConnection;9;35;29;0
WireConnection;9;37;33;0
WireConnection;76;0;15;0
WireConnection;76;1;77;0
WireConnection;74;0;28;0
WireConnection;8;0;9;38
WireConnection;85;0;76;0
WireConnection;85;9;74;0
ASEEND*/
//CHKSM=95D1260B589CE285D331A5C69482D47CD8AE32D6