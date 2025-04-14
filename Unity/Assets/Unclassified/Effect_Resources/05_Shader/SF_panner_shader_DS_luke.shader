// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-5181-OUT;n:type:ShaderForge.SFN_Tex2d,id:1645,x:31747,y:33028,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5920-OUT;n:type:ShaderForge.SFN_Multiply,id:2593,x:32372,y:32731,varname:node_2593,prsc:2|A-407-OUT,B-187-RGB,C-9115-RGB,D-5970-OUT;n:type:ShaderForge.SFN_VertexColor,id:187,x:32180,y:32857,varname:node_187,prsc:2;n:type:ShaderForge.SFN_Color,id:9115,x:32180,y:33021,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:5970,x:32197,y:33185,varname:node_5970,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:7067,x:31303,y:33026,varname:node_7067,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:407,x:32180,y:32731,varname:node_407,prsc:2|A-7849-OUT,B-8328-RGB;n:type:ShaderForge.SFN_Tex2d,id:8328,x:31908,y:32867,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:3546,x:31043,y:33310,ptovrint:False,ptlb:U_sped,ptin:_U_sped,varname:node_8137,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:1065,x:31372,y:33176,varname:node_1065,prsc:2|A-7495-T,B-3546-OUT;n:type:ShaderForge.SFN_Append,id:5673,x:31583,y:33176,varname:node_5673,prsc:2|A-1065-OUT,B-5396-OUT;n:type:ShaderForge.SFN_Time,id:7495,x:31147,y:33156,varname:node_7495,prsc:2;n:type:ShaderForge.SFN_Add,id:5920,x:31531,y:33026,varname:node_5920,prsc:2|A-7067-UVOUT,B-5673-OUT;n:type:ShaderForge.SFN_Slider,id:2304,x:31065,y:33602,ptovrint:False,ptlb:V_sped,ptin:_V_sped,varname:_U_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:5396,x:31394,y:33468,varname:node_5396,prsc:2|A-4511-T,B-2304-OUT;n:type:ShaderForge.SFN_Time,id:4511,x:31169,y:33448,varname:node_4511,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:1347,x:31222,y:32382,varname:node_1347,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:9048,x:31291,y:32532,varname:node_9048,prsc:2|A-4426-T,B-5761-OUT;n:type:ShaderForge.SFN_Append,id:7104,x:31502,y:32532,varname:node_7104,prsc:2|A-9048-OUT,B-286-OUT;n:type:ShaderForge.SFN_Time,id:4426,x:31066,y:32512,varname:node_4426,prsc:2;n:type:ShaderForge.SFN_Add,id:950,x:31450,y:32382,varname:node_950,prsc:2|A-1347-UVOUT,B-7104-OUT;n:type:ShaderForge.SFN_Slider,id:6298,x:30984,y:32958,ptovrint:False,ptlb:V_sped_02,ptin:_V_sped_02,varname:_V_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:286,x:31313,y:32824,varname:node_286,prsc:2|A-8470-T,B-6298-OUT;n:type:ShaderForge.SFN_Time,id:8470,x:31088,y:32804,varname:node_8470,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:6069,x:31666,y:32384,ptovrint:False,ptlb:Main_Tex_02,ptin:_Main_Tex_02,varname:_Main_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-950-OUT;n:type:ShaderForge.SFN_Multiply,id:7849,x:31908,y:32703,varname:node_7849,prsc:2|A-6069-RGB,B-1645-RGB;n:type:ShaderForge.SFN_Slider,id:5761,x:30909,y:32667,ptovrint:False,ptlb:U_sped_02,ptin:_U_sped_02,varname:node_2938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:5055,x:32372,y:32903,varname:node_5055,prsc:2|A-187-A,B-9115-A;n:type:ShaderForge.SFN_Multiply,id:5181,x:32549,y:32797,varname:node_5181,prsc:2|A-2593-OUT,B-5055-OUT;proporder:1645-9115-8328-6298-6069-5761-3546-2304;pass:END;sub:END;*/

Shader "Shader Forge/SF_panner_shader_DS_luke" {
    Properties {
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _V_sped_02 ("V_sped_02", Range(-5, 5)) = 0
        _Main_Tex_02 ("Main_Tex_02", 2D) = "white" {}
        _U_sped_02 ("U_sped_02", Range(-5, 5)) = 0
        _U_sped ("U_sped", Range(-5, 5)) = 0
        _V_sped ("V_sped", Range(-5, 5)) = 0
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
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Main_Tex; uniform float4 _Main_Tex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
            uniform float _U_sped;
            uniform float _V_sped;
            uniform float _V_sped_02;
            uniform sampler2D _Main_Tex_02; uniform float4 _Main_Tex_02_ST;
            uniform float _U_sped_02;
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
                float4 node_4426 = _Time;
                float4 node_8470 = _Time;
                float2 node_950 = (i.uv0+float2((node_4426.g*_U_sped_02),(node_8470.g*_V_sped_02)));
                float4 _Main_Tex_02_var = tex2D(_Main_Tex_02,TRANSFORM_TEX(node_950, _Main_Tex_02));
                float4 node_7495 = _Time;
                float4 node_4511 = _Time;
                float2 node_5920 = (i.uv0+float2((node_7495.g*_U_sped),(node_4511.g*_V_sped)));
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_5920, _Main_Tex));
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));
                float3 emissive = ((((_Main_Tex_02_var.rgb*_Main_Tex_var.rgb)*_Mask_Tex_var.rgb)*i.vertexColor.rgb*_TintColor.rgb*2.0)*(i.vertexColor.a*_TintColor.a));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
