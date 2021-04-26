// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Deeper/GlowSprite"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[SingleLineTexture]_Emissive("Emissive", 2D) = "black" {}
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
			o.Albedo = tex2DNode25.rgb;
			float textureOpacity66 = tex2DNode25.a;
			float temp_output_48_0 = ( 1.0 - textureOpacity66 );
			float smoothstepResult58 = smoothstep( 0.0 , ( 1.0 - _OutlineWidth ) , ( 1.0 - temp_output_48_0 ));
			float lerpResult54 = lerp( smoothstepResult58 , temp_output_48_0 , smoothstepResult58);
			float4 Outline65 = ( lerpResult54 * _OutlineColor );
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			o.Emission = ( Outline65 + ( tex2D( _Emissive, uv_Emissive ) * _GlowColor ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( tex2DNode25.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
700;73.6;558;439;691.2679;527.1892;2.136425;True;False
Node;AmplifyShaderEditor.SamplerNode;25;-802.7098,-69.27047;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;False;-1;f8acf47736d722947bc40b6ee5c70d6c;f8acf47736d722947bc40b6ee5c70d6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-453.9401,144.2392;Inherit;False;textureOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;92.15016,-721.2436;Inherit;False;66;textureOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;382.9116,-724.9001;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;372.969,-1155.712;Inherit;False;Property;_OutlineWidth;OutlineWidth;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;632.5017,-975.6213;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;739.3156,-1111.367;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;917.6451,-1092.074;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;1314.304,-699.3598;Inherit;False;Property;_OutlineColor;OutlineColor;7;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;0,0.465055,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;1283.224,-965.3335;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;1604.169,-806.0917;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;8.7,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-897.6529,-684.6345;Inherit;True;Property;_Emissive;Emissive;5;1;[SingleLineTexture];Create;True;0;0;False;0;False;-1;37cf5ce453cc44249813f1baf3d986ed;37cf5ce453cc44249813f1baf3d986ed;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;1923.268,-689.8145;Inherit;False;Outline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;41;-841.1369,-458.2175;Inherit;False;Property;_GlowColor;GlowColor;6;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;80;-408.1633,-678.2873;Inherit;False;65;Outline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-447.8228,-535.9478;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;85;-228.4916,-289.1622;Inherit;True;Property;_NormalMap;NormalMap;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-97.45161,72.07597;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;83;1645.963,-1069.035;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;32.81;False;2;FLOAT;-0.27;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-88.7074,-30.46791;Inherit;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-123.0717,-572.2919;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RoundOpNode;84;1946.591,-963.2902;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;24;282.8669,-144.0798;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Deeper/GlowSprite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;83;0;54;0
WireConnection;79;0;80;0
WireConnection;79;1;43;0
WireConnection;84;0;83;0
WireConnection;24;0;25;0
WireConnection;24;1;85;0
WireConnection;24;2;79;0
WireConnection;24;3;39;0
WireConnection;24;4;40;0
WireConnection;24;10;25;4
ASEEND*/
//CHKSM=5E2123A8CE0EFCB26FFA8680A1E822BA623CA7E7