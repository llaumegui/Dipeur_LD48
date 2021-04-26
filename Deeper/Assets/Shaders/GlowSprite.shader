// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Deeper/GlowSprite"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[SingleLineTexture]_Emissive("Emissive", 2D) = "black" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HDR]_GlowColor("GlowColor", Color) = (1,1,1,1)
		[HDR]_OutlineColor("OutlineColor", Color) = (1,1,1,1)
		_OutlineWidth("OutlineWidth", Range( 0 , 1)) = 1
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

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float4 _Tint;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _OutlineWidth;
		SamplerState sampler_MainTex;
		uniform float4 _OutlineColor;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _GlowColor;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode25 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = ( _Tint * tex2DNode25 ).rgb;
			float textureOpacity66 = tex2DNode25.a;
			float temp_output_48_0 = ( 1.0 - textureOpacity66 );
			float smoothstepResult58 = smoothstep( 0.0 , ( 1.0 - _OutlineWidth ) , ( 1.0 - temp_output_48_0 ));
			float lerpResult54 = lerp( smoothstepResult58 , temp_output_48_0 , smoothstepResult58);
			float4 Outline65 = ( lerpResult54 * _OutlineColor );
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float4 EmissiveMap90 = ( Outline65 + ( tex2D( _Emissive, uv_Emissive ) * _GlowColor ) );
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
512;73.6;746;439;1872.169;557.5917;2.431812;True;False
Node;AmplifyShaderEditor.SamplerNode;25;-1014.653,97.4369;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;75c4272bf5aac534688dcb2fff33adc7;f8acf47736d722947bc40b6ee5c70d6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-665.8831,310.9465;Inherit;False;textureOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-1485.573,1395.192;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1173.406,999.9099;Inherit;False;Property;_OutlineWidth;OutlineWidth;9;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;-1194.812,1391.536;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-911.2599,1164.327;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;-854.082,1041.643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-644.4036,1147.146;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;54;-279.5944,1382.13;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;-248.5145,1648.103;Inherit;False;Property;_OutlineColor;OutlineColor;8;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;0,0.465055,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;41.35054,1541.371;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;8.7,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;41;-889.5984,-967.0759;Inherit;False;Property;_GlowColor;GlowColor;7;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;360.4495,1657.648;Inherit;False;Outline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-946.1146,-1193.493;Inherit;True;Property;_Emissive;Emissive;5;1;[SingleLineTexture];Create;True;0;0;False;0;False;-1;d61018f6ffc7b124ab1f8351eff91c27;37cf5ce453cc44249813f1baf3d986ed;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-496.2839,-1044.806;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;-456.6242,-1187.146;Inherit;False;65;Outline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-171.5329,-1081.15;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;89;-993.8024,-106.6394;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;177.1588,-987.9462;Inherit;False;EmissiveMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-608.97,-13.0411;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;-10.80374,176.2591;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;85;-92.88816,-326.1482;Inherit;True;Property;_NormalMap;NormalMap;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;91;-34.93204,-114.2569;Inherit;False;90;EmissiveMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-70.5498,-11.02942;Inherit;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-82.90885,78.86261;Inherit;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;24;282.8669,-144.0798;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Deeper/GlowSprite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;6;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;90;0;79;0
WireConnection;88;0;89;0
WireConnection;88;1;25;0
WireConnection;24;0;88;0
WireConnection;24;1;85;0
WireConnection;24;2;91;0
WireConnection;24;3;39;0
WireConnection;24;4;40;0
WireConnection;24;10;92;0
ASEEND*/
//CHKSM=DD635429EFACB17445280A0ECAAA8CFEA9B94B16