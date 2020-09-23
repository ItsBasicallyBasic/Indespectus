// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Indespectus/Dissolve"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,0)
		_Albedo("Albedo", 2D) = "white" {}
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		_Metalic("Metalic", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_DissolveMap("DissolveMap", 2D) = "white" {}
		_DissolveValue("DissolveValue", Range( 0 , 1)) = 0
		_EdgeGlow("EdgeGlow", Float) = 0.02
		[HDR]_DissolveEdge("DissolveEdge", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _Tint;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;
		uniform sampler2D _DissolveMap;
		uniform float4 _DissolveMap_ST;
		uniform float _DissolveValue;
		uniform float _EdgeGlow;
		uniform float4 _DissolveEdge;
		uniform sampler2D _Metalic;
		uniform float4 _Metalic_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 Normal20 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = Normal20;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			float4 Albedo19 = ( tex2D( _Albedo, uv_Albedo ) * _Tint * tex2D( _AmbientOcclusion, uv_AmbientOcclusion ) );
			o.Albedo = Albedo19.rgb;
			float2 uv_DissolveMap = i.uv_texcoord * _DissolveMap_ST.xy + _DissolveMap_ST.zw;
			float4 tex2DNode1 = tex2D( _DissolveMap, uv_DissolveMap );
			float4 temp_cast_1 = (( _DissolveValue + _EdgeGlow )).xxxx;
			o.Emission = ( step( tex2DNode1 , temp_cast_1 ) * _DissolveEdge ).rgb;
			float2 uv_Metalic = i.uv_texcoord * _Metalic_ST.xy + _Metalic_ST.zw;
			float4 Metalic21 = tex2D( _Metalic, uv_Metalic );
			float4 temp_output_25_0 = Metalic21;
			o.Metallic = temp_output_25_0.r;
			o.Smoothness = temp_output_25_0.r;
			o.Alpha = 1;
			clip( ( (-0.6 + (( 1.0 - _DissolveValue ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2DNode1 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1927;14;1906;1005;1221.619;1431.976;1.691997;True;True
Node;AmplifyShaderEditor.CommentaryNode;6;-710,-333;Float;False;965;579;Di;5;2;4;5;3;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;38.61475,-1727.589;Float;False;907.4426;1228.596;Albedo & Normals;9;21;20;19;18;17;16;15;14;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;13;88.61475,-1677.589;Float;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;None;5b884e8faaadc08469620f618ad3407b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-659,-283;Float;False;Property;_DissolveValue;DissolveValue;7;0;Create;True;0;0;False;0;0;1.009999;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;187.7267,-1470.061;Float;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;1,1,1,0;0.5283019,0.5283019,0.5283019,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;124.8226,-1295.932;Float;True;Property;_AmbientOcclusion;Ambient Occlusion;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;272.7923,131.0016;Float;False;Property;_EdgeGlow;EdgeGlow;8;0;Create;True;0;0;False;0;0.02;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;512.0066,-1489.297;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;441.2408,38.8809;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;116.2236,-1083.308;Float;True;Property;_Normal;Normal;4;1;[Normal];Create;True;0;0;False;0;None;7396112ed4430a64eabfb97ea2d3e541;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;4;-361,-272;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-434.5001,-5.599942;Float;True;Property;_DissolveMap;DissolveMap;6;0;Create;True;0;0;False;0;5ce06d9cc7d193143ac691b6730689d4;5e56c0e728e5deb49a39b40f37bc3c7e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;97.50171,-835.4635;Float;True;Property;_Metalic;Metalic;3;0;Create;True;0;0;False;0;None;0674c07abee82e04bacfcfa969bb8fd9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;489.4796,-790.8254;Float;False;Metalic;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;7;610.1649,-59.50814;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;682.0457,-1495.88;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;10;582.108,103.7808;Float;False;Property;_DissolveEdge;DissolveEdge;9;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2,0.1630131,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;527.6506,-1246.544;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;5;-163,-211;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;943.5818,-460.2856;Float;False;19;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;867.9971,-218.8141;Float;False;21;Metalic;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;808.0302,-33.25449;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;851.5818,-373.2856;Float;False;20;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;101,-111;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1132.282,-380.3779;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Indespectus/Dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;13;0
WireConnection;16;1;14;0
WireConnection;16;2;15;0
WireConnection;24;0;2;0
WireConnection;24;1;9;0
WireConnection;4;0;2;0
WireConnection;21;0;17;0
WireConnection;7;0;1;0
WireConnection;7;1;24;0
WireConnection;19;0;16;0
WireConnection;20;0;18;0
WireConnection;5;0;4;0
WireConnection;11;0;7;0
WireConnection;11;1;10;0
WireConnection;3;0;5;0
WireConnection;3;1;1;0
WireConnection;0;0;22;0
WireConnection;0;1;23;0
WireConnection;0;2;11;0
WireConnection;0;3;25;0
WireConnection;0;4;25;0
WireConnection;0;10;3;0
ASEEND*/
//CHKSM=7F970D98FACF0489EBFDE3D8AC289D7B022798F1