// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Indespectus/Distortion"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,0)
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_DistortionNormal("DistortionNormal", 2D) = "bump" {}
		[HDR]_GlowColor("Glow Color", Color) = (0,0,0,0)
		_EmissiveTex("EmissiveTex", 2D) = "white" {}
		_Metalic("Metalic", 2D) = "white" {}
		_DistortionScaling("DistortionScaling", Range( 0 , 0.2)) = 0.2
		_Transition("Transition", Range( 0 , 1.5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _Tint;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;
		uniform float _Transition;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _DistortionNormal;
		uniform float4 _DistortionNormal_ST;
		uniform float _DistortionScaling;
		uniform sampler2D _EmissiveTex;
		uniform float4 _EmissiveTex_ST;
		uniform float4 _GlowColor;
		uniform sampler2D _Metalic;
		uniform float4 _Metalic_ST;


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
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 Normal39 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = Normal39;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			float4 Albedo37 = ( tex2D( _Albedo, uv_Albedo ) * _Tint * tex2D( _AmbientOcclusion, uv_AmbientOcclusion ) );
			float clampResult49 = clamp( ( -0.5 + _Transition ) , 0.0 , 1.0 );
			float4 lerpResult44 = lerp( Albedo37 , float4( 0,0,0,0 ) , ( 1.0 - clampResult49 ));
			o.Albedo = lerpResult44.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 uv_DistortionNormal = i.uv_texcoord * _DistortionNormal_ST.xy + _DistortionNormal_ST.zw;
			float4 screenColor23 = tex2D( _GrabTexture, ( (ase_grabScreenPosNorm).xy + (( UnpackNormal( tex2D( _DistortionNormal, uv_DistortionNormal ) ) * _DistortionScaling )).xy ) );
			float4 sRefraction27 = screenColor23;
			float2 uv_EmissiveTex = i.uv_texcoord * _EmissiveTex_ST.xy + _EmissiveTex_ST.zw;
			float4 lerpResult25 = lerp( sRefraction27 , ( tex2D( _EmissiveTex, uv_EmissiveTex ) * _GlowColor ) , clampResult49);
			o.Emission = lerpResult25.rgb;
			float2 uv_Metalic = i.uv_texcoord * _Metalic_ST.xy + _Metalic_ST.zw;
			float4 Metalic52 = tex2D( _Metalic, uv_Metalic );
			float4 temp_output_53_0 = Metalic52;
			o.Metallic = temp_output_53_0.r;
			o.Smoothness = temp_output_53_0.r;
			float clampResult47 = clamp( _Transition , 0.0 , 1.0 );
			o.Alpha = clampResult47;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1920;0;1920;1019;1335.671;427.1039;1.394941;True;True
Node;AmplifyShaderEditor.CommentaryNode;28;-3069.325,-30.27052;Float;False;1595.244;549.2399;Simple Refraction;9;17;23;22;21;20;19;16;18;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;17;-3019.325,192.0363;Float;True;Property;_DistortionNormal;DistortionNormal;5;0;Create;True;0;0;False;0;None;302951faffe230848aa0d3df7bb70faa;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2994.782,403.9694;Float;False;Property;_DistortionScaling;DistortionScaling;10;0;Create;True;0;0;False;0;0.2;0.2;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;18;-2611.306,19.72948;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2605.519,257.736;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;21;-2412.426,255.5359;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;41;-2124.109,-1301.094;Float;False;907.4426;1228.596;Albedo & Normals;9;39;32;37;35;30;34;36;51;52;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-2355.626,35.63626;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;36;-2037.901,-869.4376;Float;True;Property;_AmbientOcclusion;Ambient Occlusion;3;0;Create;True;0;0;False;0;None;5b884e8faaadc08469620f618ad3407b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-1186.264,439.2702;Float;False;Property;_Transition;Transition;11;0;Create;True;0;0;False;0;0;1.5;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-1974.997,-1043.566;Float;False;Property;_Tint;Tint;2;0;Create;True;0;0;False;0;1,1,1,0;0.2924528,0.2924528,0.2924528,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-2127.22,107.7369;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;30;-2074.109,-1251.094;Float;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;None;5b884e8faaadc08469620f618ad3407b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1650.717,-1062.803;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;23;-1939.845,56.33699;Float;False;Global;_ScreenGrab0;Screen Grab 0;-1;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-928.7499,249.3719;Float;False;2;2;0;FLOAT;-0.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;49;-672.0743,210.7867;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1480.678,-1069.385;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-1722.748,54.6376;Float;False;sRefraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;32;-2046.5,-656.8136;Float;True;Property;_Normal;Normal;4;1;[Normal];Create;True;0;0;False;0;None;7396112ed4430a64eabfb97ea2d3e541;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;-2065.222,-408.969;Float;True;Property;_Metalic;Metalic;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;38;-608.1285,-97.28622;Float;False;37;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-1635.073,-820.0497;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-728.6986,117.3254;Float;False;27;sRefraction;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-1673.244,-364.3309;Float;False;Metalic;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;48;-597.0123,358.448;Float;False;GlowEmission;6;;7;16accc8a32775e945b5e6c1cd3b1ce86;0;0;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;45;-489.4429,35.7436;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-269.6303,72.84223;Float;False;39;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;25;-322.8862,162.8488;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;4.869783,192.2501;Float;False;52;Metalic;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;44;-334.9381,-75.11922;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;47;-389.8918,391.8867;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;579.6577,-6.191797;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Indespectus/Distortion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;17;0
WireConnection;19;1;16;0
WireConnection;21;0;19;0
WireConnection;20;0;18;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;35;0;30;0
WireConnection;35;1;34;0
WireConnection;35;2;36;0
WireConnection;23;0;22;0
WireConnection;46;1;43;0
WireConnection;49;0;46;0
WireConnection;37;0;35;0
WireConnection;27;0;23;0
WireConnection;39;0;32;0
WireConnection;52;0;51;0
WireConnection;45;0;49;0
WireConnection;25;0;26;0
WireConnection;25;1;48;0
WireConnection;25;2;49;0
WireConnection;44;0;38;0
WireConnection;44;2;45;0
WireConnection;47;0;43;0
WireConnection;0;0;44;0
WireConnection;0;1;40;0
WireConnection;0;2;25;0
WireConnection;0;3;53;0
WireConnection;0;4;53;0
WireConnection;0;9;47;0
ASEEND*/
//CHKSM=F7289AB0A836CBBC85983F1CB5FC594EBB7D7472