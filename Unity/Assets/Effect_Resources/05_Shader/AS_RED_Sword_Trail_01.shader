// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AS_RED_Sword_Trail_01"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_NoiseMap("_NoiseMap", 2D) = "white" {}
		_MaskTex("_MaskTex", 2D) = "white" {}
		_Val_01("_Val_01", Range( 0 , 1)) = 0.5
		_Val_02("_Val_02", Range( 0 , 1)) = 0.5128382
		_Val_03("_Val_03", Range( 0 , 2)) = 1
		_Intensity("_Intensity", Range( 0 , 5)) = 1
		_SPD("_SPD", Range( 0 , 3)) = 1
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv2_tex4coord2;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Intensity;
		uniform sampler2D _MainTex;
		uniform float _Val_01;
		uniform sampler2D _NoiseMap;
		uniform float _SPD;
		uniform float _Val_03;
		uniform float _Val_02;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 appendResult141 = (float4(i.uv2_tex4coord2.y , 0.0 , 0.0 , 0.0));
			float2 panner133 = ( 1.0 * _Time.y * float2( 0,0 ) + appendResult141.xy);
			float2 uv_TexCoord36 = i.uv_texcoord + panner133;
			float4 tex2DNode1 = tex2D( _MainTex, uv_TexCoord36 );
			float temp_output_50_0 = (-_Val_01 + (i.uv2_tex4coord2.x - 0.0) * (1.0 - -_Val_01) / (1.0 - 0.0));
			float2 panner37 = ( ( _Time.y * _SPD ) * float2( -1,0 ) + float2( 0,0 ));
			float2 uv_TexCoord93 = i.uv_texcoord + panner37;
			float4 tex2DNode41 = tex2D( _NoiseMap, uv_TexCoord93 );
			float smoothstepResult47 = smoothstep( temp_output_50_0 , ( temp_output_50_0 + _Val_01 ) , tex2DNode41.r);
			float temp_output_59_0 = (-_Val_02 + (( i.uv2_tex4coord2.x / _Val_03 ) - 0.0) * (1.0 - -_Val_02) / (1.0 - 0.0));
			float smoothstepResult61 = smoothstep( temp_output_59_0 , ( temp_output_59_0 + _Val_02 ) , tex2DNode41.r);
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float4 tex2DNode142 = tex2D( _MaskTex, uv_MaskTex );
			o.Emission = ( ( _Intensity * ( tex2DNode1 * i.vertexColor ) * ( smoothstepResult47 + ( smoothstepResult61 - smoothstepResult47 ) ) ) * tex2DNode142 ).rgb;
			o.Alpha = ( ( tex2DNode1.a * i.vertexColor.a ) * tex2DNode142.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

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
				float4 customPack1 : TEXCOORD1;
				float2 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xyzw = customInputData.uv2_tex4coord2;
				o.customPack1.xyzw = v.texcoord1;
				o.customPack2.xy = customInputData.uv_texcoord;
				o.customPack2.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv2_tex4coord2 = IN.customPack1.xyzw;
				surfIN.uv_texcoord = IN.customPack2.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=18900
2227;1;1371;986;3334.295;211.8914;1.479851;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;39;-2612.483,381.3029;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2715.196,457.801;Inherit;False;Property;_SPD;_SPD;8;0;Create;True;0;0;0;False;0;False;1;2.2;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-2402.26,381.6182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-2420.707,539.4503;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;62;-2349.816,1021.562;Float;False;Property;_Val_03;_Val_03;6;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2143.407,1136.005;Float;False;Property;_Val_02;_Val_02;5;0;Create;True;0;0;0;False;0;False;0.5128382;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2132.88,702.8779;Float;False;Property;_Val_01;_Val_01;4;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-1826.65,146.8336;Inherit;False;Constant;_Float4;Float 4;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-2235.47,332.7603;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;55;-2036.403,1003.073;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;58;-1837.862,1079.714;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;49;-1859.023,641.5837;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;59;-1673.77,1003.685;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;141;-1662.776,153.6188;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-1694.931,565.5546;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;93;-2019.672,371.4669;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1807.351,365.7855;Inherit;True;Property;_NoiseMap;_NoiseMap;2;0;Create;True;0;0;0;False;0;False;-1;42ebf2839c4c5194e814f3da24cf0693;42ebf2839c4c5194e814f3da24cf0693;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-1452.774,1118.059;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1473.933,679.9286;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;133;-1508.486,154.9445;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-1306.045,4.064702;Inherit;True;Property;_MainTex;_MainTex;1;0;Create;True;0;0;0;False;0;False;1717bb7fb0f200245ba16791cf0394f4;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SmoothstepOpNode;61;-1284.486,818.1516;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;47;-1287.847,394.6732;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1296.933,203.0405;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;54;-969.3013,555.3473;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;90;-899.2356,205.8835;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1033.382,3.830081;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;6effd8073b1a90d48b94d798abfcf343;6effd8073b1a90d48b94d798abfcf343;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;79;-749.1476,-100.2513;Inherit;False;Property;_Intensity;_Intensity;7;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-697.6574,395.4896;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-612.8715,61.73254;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-623.7365,278.3348;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-375.1103,36.83232;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;142;-444.7719,378.0894;Inherit;True;Property;_MaskTex;_MaskTex;3;0;Create;True;0;0;0;False;0;False;-1;None;8dbeed5d24cb4ed47a31f34c4ff338ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-84.2319,284.0355;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-96.42407,68.05981;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;111;106.7142,-11.71213;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;AS_RED_Sword_Trail_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;39;0
WireConnection;38;1;40;0
WireConnection;37;1;38;0
WireConnection;55;0;83;1
WireConnection;55;1;62;0
WireConnection;58;0;57;0
WireConnection;49;0;48;0
WireConnection;59;0;55;0
WireConnection;59;3;58;0
WireConnection;141;0;83;2
WireConnection;141;1;136;0
WireConnection;50;0;83;1
WireConnection;50;3;49;0
WireConnection;93;1;37;0
WireConnection;41;1;93;0
WireConnection;60;0;59;0
WireConnection;60;1;57;0
WireConnection;51;0;50;0
WireConnection;51;1;48;0
WireConnection;133;0;141;0
WireConnection;61;0;41;1
WireConnection;61;1;59;0
WireConnection;61;2;60;0
WireConnection;47;0;41;1
WireConnection;47;1;50;0
WireConnection;47;2;51;0
WireConnection;36;1;133;0
WireConnection;54;0;61;0
WireConnection;54;1;47;0
WireConnection;1;0;31;0
WireConnection;1;1;36;0
WireConnection;53;0;47;0
WireConnection;53;1;54;0
WireConnection;80;0;1;0
WireConnection;80;1;90;0
WireConnection;92;0;1;4
WireConnection;92;1;90;4
WireConnection;45;0;79;0
WireConnection;45;1;80;0
WireConnection;45;2;53;0
WireConnection;144;0;92;0
WireConnection;144;1;142;4
WireConnection;143;0;45;0
WireConnection;143;1;142;0
WireConnection;111;2;143;0
WireConnection;111;9;144;0
ASEEND*/
//CHKSM=0A2E5DCC1869E6847A2645D9452D13BCFCEBC65C