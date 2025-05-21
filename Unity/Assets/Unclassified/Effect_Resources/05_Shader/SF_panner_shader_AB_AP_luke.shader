// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-8310-OUT,alpha-798-OUT;n:type:ShaderForge.SFN_Multiply,id:798,x:32495,y:32923,varname:node_798,prsc:2|A-4763-OUT,B-6689-A,C-3917-A;n:type:ShaderForge.SFN_Tex2d,id:9490,x:31366,y:32603,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6249b54ca0c8dad40b4177288b1a560e,ntxv:0,isnm:False|UVIN-1524-OUT;n:type:ShaderForge.SFN_Multiply,id:8310,x:32032,y:32605,varname:node_8310,prsc:2|A-2472-OUT,B-6689-RGB,C-3917-RGB,D-9795-OUT;n:type:ShaderForge.SFN_VertexColor,id:6689,x:31840,y:32731,varname:node_6689,prsc:2;n:type:ShaderForge.SFN_Color,id:3917,x:31840,y:32886,ptovrint:True,ptlb:Color_copy,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9795,x:31857,y:33059,varname:node_9795,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:3884,x:30922,y:32601,varname:node_3884,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2472,x:31840,y:32605,varname:node_2472,prsc:2|A-9490-RGB,B-6305-OUT;n:type:ShaderForge.SFN_Tex2d,id:16,x:31204,y:33142,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0ad836bc7685cb74aae9777609f58301,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:8527,x:31201,y:32751,varname:node_8527,prsc:2|A-5338-OUT,B-8478-OUT;n:type:ShaderForge.SFN_Add,id:1524,x:31150,y:32601,varname:node_1524,prsc:2|A-3884-UVOUT,B-8527-OUT;n:type:ShaderForge.SFN_Vector1,id:5338,x:31006,y:32751,varname:node_5338,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:9099,x:30753,y:33035,varname:node_9099,prsc:2|A-4835-OUT,B-6689-A;n:type:ShaderForge.SFN_ValueProperty,id:4835,x:30672,y:32860,ptovrint:False,ptlb:node_2122,ptin:_node_2122,varname:node_2122,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:8478,x:30902,y:32860,varname:node_8478,prsc:2|A-4835-OUT,B-6689-A;n:type:ShaderForge.SFN_TexCoord,id:6607,x:30602,y:33572,varname:node_6607,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:9862,x:30671,y:33722,varname:node_9862,prsc:2|A-3341-T,B-6194-OUT;n:type:ShaderForge.SFN_Append,id:1301,x:30882,y:33722,varname:node_1301,prsc:2|A-9862-OUT,B-2681-OUT;n:type:ShaderForge.SFN_Time,id:3341,x:30446,y:33702,varname:node_3341,prsc:2;n:type:ShaderForge.SFN_Add,id:8166,x:30830,y:33572,varname:node_8166,prsc:2|A-6607-UVOUT,B-1301-OUT;n:type:ShaderForge.SFN_Slider,id:7419,x:30364,y:34148,ptovrint:False,ptlb:V_sped_02,ptin:_V_sped_02,varname:_V_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:2681,x:30693,y:34014,varname:node_2681,prsc:2|A-6546-T,B-7419-OUT;n:type:ShaderForge.SFN_Time,id:6546,x:30468,y:33994,varname:node_6546,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:7425,x:31046,y:33574,ptovrint:False,ptlb:Main_Tex_02,ptin:_Main_Tex_02,varname:_Main_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:626a7ae973a445949945d6223455bdb7,ntxv:0,isnm:False|UVIN-8166-OUT;n:type:ShaderForge.SFN_Multiply,id:1206,x:31507,y:33143,varname:node_1206,prsc:2|A-16-RGB,B-7425-RGB;n:type:ShaderForge.SFN_Slider,id:6194,x:30253,y:33919,ptovrint:False,ptlb:U_sped_02,ptin:_U_sped_02,varname:node_2938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Color,id:5583,x:31507,y:33329,ptovrint:False,ptlb:node_1079,ptin:_node_1079,varname:node_1079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6305,x:31678,y:33143,varname:node_6305,prsc:2|A-1206-OUT,B-5583-RGB;n:type:ShaderForge.SFN_Multiply,id:4763,x:31650,y:32718,varname:node_4763,prsc:2|A-9490-A,B-16-A,C-7425-A;proporder:3917-9490-16-4835-7419-7425-6194-5583;pass:END;sub:END;*/

Shader "Shader Forge/SF_panner_shader_AB_AP_luke" {
    Properties {
        _TintColor ("Color_copy", Color) = (0.5,0.5,0.5,1)
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _node_2122 ("node_2122", Float ) = 1
        _V_sped_02 ("V_sped_02", Range(-5, 5)) = 0
        _Main_Tex_02 ("Main_Tex_02", 2D) = "white" {}
        _U_sped_02 ("U_sped_02", Range(-5, 5)) = 0
        _node_1079 ("node_1079", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Main_Tex; uniform float4 _Main_Tex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
            uniform float _node_2122;
            uniform float _V_sped_02;
            uniform sampler2D _Main_Tex_02; uniform float4 _Main_Tex_02_ST;
            uniform float _U_sped_02;
            uniform float4 _node_1079;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float2 node_1524 = (i.uv0+float2(0.0,(_node_2122+i.vertexColor.a)));
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_1524, _Main_Tex));
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));
                float4 node_3341 = _Time + _TimeEditor;
                float4 node_6546 = _Time + _TimeEditor;
                float2 node_8166 = (i.uv0+float2((node_3341.g*_U_sped_02),(node_6546.g*_V_sped_02)));
                float4 _Main_Tex_02_var = tex2D(_Main_Tex_02,TRANSFORM_TEX(node_8166, _Main_Tex_02));
                float3 emissive = ((_Main_Tex_var.rgb*((_Mask_Tex_var.rgb*_Main_Tex_02_var.rgb)*_node_1079.rgb))*i.vertexColor.rgb*_TintColor.rgb*2.0);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,((_Main_Tex_var.a*_Mask_Tex_var.a*_Main_Tex_02_var.a)*i.vertexColor.a*_TintColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
