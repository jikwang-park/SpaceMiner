// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AS_panner_02_luke"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_Texture_01("Texture_01", 2D) = "white" {}
		_Texture_02("Texture_02", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 5)) = 1
		_Value_01("Value_01", Vector) = (0,0,0,0)
		_Value_02("Value_02", Vector) = (0,0,0,0)
		_Value_03("Value_03", Vector) = (0,0,0,0)
		_pow_01("pow_01", Float) = 1
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Color0("Color 0", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color0;
		uniform float _Float0;
		uniform sampler2D _MainTex;
		uniform float2 _Value_01;
		uniform sampler2D _Texture_01;
		uniform float2 _Value_02;
		uniform sampler2D _Texture_02;
		uniform float2 _Value_03;
		uniform float _pow_01;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner35 = ( 1.0 * _Time.y * _Value_01 + float2( 0,0 ));
			float4 tex2DNode2 = tex2D( _MainTex, ( i.uv_texcoord + panner35 ) );
			float2 panner71 = ( 1.0 * _Time.y * _Value_02 + float2( 0,0 ));
			float2 panner82 = ( 1.0 * _Time.y * _Value_03 + float2( 0,0 ));
			float4 temp_cast_0 = (_pow_01).xxxx;
			float2 uv_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			float4 tex2DNode78 = tex2D( _Mask_Tex, uv_Mask_Tex );
			o.Emission = ( ( _Color0 * ( _Float0 * ( pow( ( tex2DNode2 * ( tex2D( _Texture_01, ( i.uv_texcoord + panner71 ) ) * tex2D( _Texture_02, ( i.uv_texcoord + panner82 ) ) ) ) , temp_cast_0 ) * tex2DNode78 ) * i.vertexColor ) ) * ( ( tex2DNode2.a * tex2DNode78.a ) * i.vertexColor.a ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
2101;-1;1498;986;2085.311;830.8051;1.784038;True;True
Node;AmplifyShaderEditor.Vector2Node;81;-2032,656;Inherit;False;Property;_Value_03;Value_03;7;0;Create;True;0;0;0;False;0;False;0,0;1.2,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;70;-2032,384;Inherit;False;Property;_Value_02;Value_02;6;0;Create;True;0;0;0;False;0;False;0,0;0.8,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;36;-1856,80;Inherit;False;Property;_Value_01;Value_01;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-1872,512;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;71;-1840,368;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;82;-1840,640;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-1872,224;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-1600,288;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;35;-1648,64;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-1600,576;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1680,-64;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;74;-1440,240;Inherit;True;Property;_Texture_01;Texture_01;2;0;Create;True;0;0;0;False;0;False;-1;None;42ebf2839c4c5194e814f3da24cf0693;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;85;-1440,512;Inherit;True;Property;_Texture_02;Texture_02;3;0;Create;True;0;0;0;False;0;False;-1;None;9b33503794bd5c54086319a087de4a43;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-1424,0;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1072,384;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-1264,-64;Inherit;True;Property;_MainTex;_MainTex;1;0;Create;True;0;0;0;False;0;False;-1;None;121104dc3b1534f4fb06c27734255e63;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-896,-48;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-912,64;Inherit;False;Property;_pow_01;pow_01;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;76;-736,-48;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;78;-896,176;Inherit;True;Property;_Mask_Tex;Mask_Tex;9;0;Create;True;0;0;0;False;0;False;-1;None;810833af08895624c9b97274bb214747;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;68;-656,-144;Inherit;False;Property;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;1;1.5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-512,-48;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;3;-535.1403,181.999;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-528,64;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-348.059,-139.7735;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;87;-434.9642,-342.988;Inherit;False;Property;_Color0;Color 0;10;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-140.4274,-168.4227;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-114.9314,17.4297;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;57.93329,-105.076;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;39;211.5996,-155.8001;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;AS_panner_02_luke;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0;True;False;0;True;Transparent;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;71;2;70;0
WireConnection;82;2;81;0
WireConnection;73;0;72;0
WireConnection;73;1;71;0
WireConnection;35;2;36;0
WireConnection;84;0;83;0
WireConnection;84;1;82;0
WireConnection;74;1;73;0
WireConnection;85;1;84;0
WireConnection;69;0;11;0
WireConnection;69;1;35;0
WireConnection;86;0;74;0
WireConnection;86;1;85;0
WireConnection;2;1;69;0
WireConnection;75;0;2;0
WireConnection;75;1;86;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;79;0;76;0
WireConnection;79;1;78;0
WireConnection;80;0;2;4
WireConnection;80;1;78;4
WireConnection;4;0;68;0
WireConnection;4;1;79;0
WireConnection;4;2;3;0
WireConnection;88;0;87;0
WireConnection;88;1;4;0
WireConnection;41;0;80;0
WireConnection;41;1;3;4
WireConnection;43;0;88;0
WireConnection;43;1;41;0
WireConnection;39;2;43;0
ASEEND*/
//CHKSM=4DC25D57ADF4F65E03A78B052E0BCAB8F1DF8AAE