// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Indespectus/Transparency"
{
	Properties
	{
		_Transition("Transition", Range( 0 , 5)) = 0
		[HDR]_BaseColor("BaseColor", Color) = (1,0.5960785,0.4039216,0)
		_Distortion("Distortion", 2D) = "white" {}
		_DistortionScale("Distortion Scale", Range( 0 , 1)) = 0.01
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float4 _BaseColor;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _Distortion;
		uniform float4 _Distortion_ST;
		uniform float _DistortionScale;
		uniform float _Transition;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 uv_Distortion = i.uv_texcoord * _Distortion_ST.xy + _Distortion_ST.zw;
			float4 screenColor3_g7 = tex2D( _GrabTexture, ( ase_screenPosNorm + ( tex2D( _Distortion, uv_Distortion ) * _DistortionScale ) ).xy );
			float4 lerpResult38 = lerp( _BaseColor , screenColor3_g7 , _Transition);
			o.Albedo = lerpResult38.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-1680;110;798;906;127.1258;732.139;1;True;False
Node;AmplifyShaderEditor.ColorNode;5;255.8936,-479.3867;Float;False;Property;_BaseColor;BaseColor;1;1;[HDR];Create;True;0;0;False;0;1,0.5960785,0.4039216,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;207.9722,-148.7333;Float;False;Property;_Transition;Transition;0;0;Create;True;0;0;False;0;0;-1.647059;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;42;257.1104,-294.373;Float;False;Transparency;2;;7;fcfeaf05ec82be04eb741c931ac4dbad;0;0;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;38;529.634,-326.0389;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;762.1334,-325.2184;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Indespectus/Transparency;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;5;0
WireConnection;38;1;42;0
WireConnection;38;2;39;0
WireConnection;0;0;38;0
ASEEND*/
//CHKSM=5DDA51157A652116544CFB346E81B908F82FCF07