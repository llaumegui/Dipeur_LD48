// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Deeper/GlowSprite"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[SingleLineTexture]_Emissive("Emissive", 2D) = "black" {}
		[HDR]_GlowColor("GlowColor", Color) = (1,1,1,1)
		[HDR]_OutlineColor("OutlineColor", Color) = (1,1,1,1)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_OutlineWidth("OutlineWidth", Range( 0 , 1)) = 1
		[HideInInspector]_Emission("Emission", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Tint;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _OutlineWidth;
		SamplerState sampler_MainTex;
		uniform float4 _OutlineColor;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _GlowColor;
		uniform float _Emission;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode25 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = ( _Tint * tex2DNode25 ).rgb;
			float textureOpacity66 = tex2DNode25.a;
			float temp_output_48_0 = ( 1.0 - textureOpacity66 );
			float smoothstepResult58 = smoothstep( 0.0 , ( 1.0 - _OutlineWidth ) , ( 1.0 - temp_output_48_0 ));
			float lerpResult54 = lerp( smoothstepResult58 , temp_output_48_0 , smoothstepResult58);
			float4 Outline65 = ( lerpResult54 * _OutlineColor );
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float4 color94 = IsGammaSpace() ? float4(1,0,0,1) : float4(1,0,0,1);
			float4 lerpResult96 = lerp( ( Outline65 + ( tex2D( _Emissive, uv_Emissive ) * _GlowColor ) ) , color94 , _Emission);
			float4 EmissiveMap90 = lerpResult96;
			o.Emission = EmissiveMap90.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( textureOpacity66 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
583.2;73.6;675;439;2411.601;116.8094;3.137888;True;False
Node;AmplifyShaderEditor.SamplerNode;25;-1014.653,97.4369;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;75c4272bf5aac534688dcb2fff33adc7;f8acf47736d722947bc40b6ee5c70d6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-528.8103,260.7108;Inherit;False;textureOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-1485.573,1395.192;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1173.406,999.9099;Inherit;False;Property;_OutlineWidth;OutlineWidth;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;-1194.812,1391.536;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-911.2599,1164.327;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;-854.082,1041.643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-644.4036,1147.146;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;-248.5145,1648.103;Inherit;False;Property;_OutlineColor;OutlineColor;6;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;0,0.465055,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;-279.5944,1382.13;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;41.35054,1541.371;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;8.7,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;360.4495,1657.648;Inherit;False;Outline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-946.1146,-1193.493;Inherit;True;Property;_Emissive;Emissive;4;1;[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;37cf5ce453cc44249813f1baf3d986ed;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-889.5984,-967.0759;Inherit;False;Property;_GlowColor;GlowColor;5;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-496.2839,-1044.806;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;-456.6242,-1187.146;Inherit;False;65;Outline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;94;-166.7949,-834.0916;Inherit;False;Constant;_EmissionColor;EmissionColor;10;0;Create;True;0;0;False;0;False;1,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;98;-195.8781,-635.5592;Float;False;Property;_Emission;Emission;9;1;[HideInInspector];Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-171.5329,-1081.15;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;96;132.8215,-868.4673;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;474.5665,-895.5083;Inherit;False;EmissiveMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;89;-993.8024,-106.6394;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;102;-1063.707,462.8846;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;99;-656.1204,417.4126;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;-34.93204,-114.2569;Inherit;False;90;EmissiveMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;103;-1795.114,432.5329;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-70.5498,-11.02942;Inherit;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;-10.80374,176.2591;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-608.97,-13.0411;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;104;-1424.501,499.8321;Inherit;True;3;0;FLOAT2;1.33,0;False;1;FLOAT;1.02;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-82.90885,78.86261;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;101;-1427.425,247.777;Inherit;True;Property;_Texture0;Texture 0;10;0;Create;True;0;0;False;0;False;75c4272bf5aac534688dcb2fff33adc7;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;24;282.8669,-144.0798;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Deeper/GlowSprite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;7;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;66;0;25;4
WireConnection;48;0;67;0
WireConnection;49;0;48;0
WireConnection;82;0;81;0
WireConnection;58;0;49;0
WireConnection;58;2;82;0
WireConnection;54;0;58;0
WireConnection;54;1;48;0
WireConnection;54;2;58;0
WireConnection;57;0;54;0
WireConnection;57;1;59;0
WireConnection;65;0;57;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;79;0;80;0
WireConnection;79;1;43;0
WireConnection;96;0;79;0
WireConnection;96;1;94;0
WireConnection;96;2;98;0
WireConnection;90;0;96;0
WireConnection;102;0;101;0
WireConnection;102;1;104;0
WireConnection;99;0;25;4
WireConnection;99;1;102;4
WireConnection;88;0;89;0
WireConnection;88;1;25;0
WireConnection;104;0;103;0
WireConnection;24;0;88;0
WireConnection;24;2;91;0
WireConnection;24;3;39;0
WireConnection;24;4;40;0
WireConnection;24;10;92;0
ASEEND*/
//CHKSM=78282A82639A3C4A5F4339083FD45E5F7CFED5A7