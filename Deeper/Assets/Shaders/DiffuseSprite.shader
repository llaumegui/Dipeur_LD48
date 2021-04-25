// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Deeper/DiffuseSprite"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector]_MainTex("MainTex", 2D) = "white" {}
		[SingleLineTexture]_Emissive("Emissive", 2D) = "white" {}
		_AlphaMask("AlphaMask", Float) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HDR]_GlowColor("GlowColor", Color) = (1,0,0,1)
		[HDR]_OutlineColor("OutlineColor", Color) = (0,0.465055,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		SamplerState sampler_MainTex;
		uniform float4 _OutlineColor;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _GlowColor;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _AlphaMask;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode25 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = tex2DNode25.rgb;
			float textureOpacity66 = tex2DNode25.a;
			float temp_output_48_0 = ( 1.0 - textureOpacity66 );
			float smoothstepResult58 = smoothstep( 0.0 , 0.0 , ( 1.0 - temp_output_48_0 ));
			float lerpResult54 = lerp( smoothstepResult58 , temp_output_48_0 , smoothstepResult58);
			float4 Outline65 = ( lerpResult54 * _OutlineColor );
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			o.Emission = ( Outline65 + ( tex2D( _Emissive, uv_Emissive ) * _GlowColor ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			clip( ( tex2DNode25.a * ( ase_vertex3Pos.y + _AlphaMask ) ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
574.4;73.6;637;439;-648.4895;1179.913;2.337824;True;False
Node;AmplifyShaderEditor.SamplerNode;25;-802.7098,-69.27047;Inherit;True;Property;_MainTex;MainTex;1;1;[HideInInspector];Create;True;0;0;False;0;False;-1;f8acf47736d722947bc40b6ee5c70d6c;e4b72d6cc7703dc45903734a05a08945;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-437.6873,30.46895;Inherit;False;textureOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;92.15016,-721.2436;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;382.9116,-724.9001;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;632.5017,-975.6213;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;917.6451,-1092.074;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;1314.304,-699.3598;Inherit;False;Property;_OutlineColor;OutlineColor;7;1;[HDR];Create;True;0;0;False;0;False;0,0.465055,1,1;0,0.465055,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;1283.224,-965.3335;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;1604.169,-806.0917;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;8.7,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-869.3143,491.8387;Inherit;False;Property;_AlphaMask;AlphaMask;3;0;Create;True;0;0;False;0;False;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;35;-880.7578,309.4288;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-778.111,-295.6768;Inherit;False;Property;_GlowColor;GlowColor;6;1;[HDR];Create;True;0;0;False;0;False;1,0,0,1;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;42;-786.0485,-517.537;Inherit;True;Property;_Emissive;Emissive;2;1;[SingleLineTexture];Create;True;0;0;False;0;False;-1;37cf5ce453cc44249813f1baf3d986ed;37cf5ce453cc44249813f1baf3d986ed;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;1923.268,-689.8145;Inherit;False;Outline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-662.1428,373.2498;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.82;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-384.7967,-373.407;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;-377.4068,-479.0131;Inherit;False;65;Outline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-88.7074,-30.46791;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-270.7705,297.3974;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-103.803,-391.8563;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-97.45161,72.07597;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;24;282.8669,-144.0798;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Deeper/DiffuseSprite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;66;0;25;4
WireConnection;48;0;67;0
WireConnection;49;0;48;0
WireConnection;58;0;49;0
WireConnection;54;0;58;0
WireConnection;54;1;48;0
WireConnection;54;2;58;0
WireConnection;57;0;54;0
WireConnection;57;1;59;0
WireConnection;65;0;57;0
WireConnection;36;0;35;2
WireConnection;36;1;37;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;38;0;25;4
WireConnection;38;1;36;0
WireConnection;79;0;80;0
WireConnection;79;1;43;0
WireConnection;24;0;25;0
WireConnection;24;2;79;0
WireConnection;24;3;39;0
WireConnection;24;4;40;0
WireConnection;24;10;38;0
ASEEND*/
//CHKSM=FCF0F32A323CE38801C39B6E695899D432DFC756