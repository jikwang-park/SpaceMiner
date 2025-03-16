// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32839,y:32695,varname:node_4795,prsc:2|emission-1372-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31870,y:33090,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1435-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32495,y:32793,varname:node_2393,prsc:2|A-3039-OUT,B-2053-RGB,C-797-RGB,D-9248-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32303,y:32919,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32303,y:33083,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32320,y:33247,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:4476,x:31426,y:33088,varname:node_4476,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:3039,x:32303,y:32793,varname:node_3039,prsc:2|A-6265-OUT,B-9518-RGB;n:type:ShaderForge.SFN_Tex2d,id:9518,x:32031,y:32929,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:8137,x:31166,y:33372,ptovrint:False,ptlb:U_sped,ptin:_U_sped,varname:node_8137,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:1174,x:31495,y:33238,varname:node_1174,prsc:2|A-145-T,B-8137-OUT;n:type:ShaderForge.SFN_Append,id:5727,x:31706,y:33238,varname:node_5727,prsc:2|A-1174-OUT,B-2901-OUT;n:type:ShaderForge.SFN_Time,id:145,x:31270,y:33218,varname:node_145,prsc:2;n:type:ShaderForge.SFN_Add,id:1435,x:31654,y:33088,varname:node_1435,prsc:2|A-4476-UVOUT,B-5727-OUT;n:type:ShaderForge.SFN_Slider,id:8589,x:31188,y:33664,ptovrint:False,ptlb:V_sped,ptin:_V_sped,varname:_U_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:2901,x:31517,y:33530,varname:node_2901,prsc:2|A-8959-T,B-8589-OUT;n:type:ShaderForge.SFN_Time,id:8959,x:31292,y:33510,varname:node_8959,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:1905,x:31345,y:32444,varname:node_1905,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:5771,x:31414,y:32594,varname:node_5771,prsc:2|A-2875-T,B-2938-OUT;n:type:ShaderForge.SFN_Append,id:5742,x:31625,y:32594,varname:node_5742,prsc:2|A-5771-OUT,B-3998-OUT;n:type:ShaderForge.SFN_Time,id:2875,x:31189,y:32574,varname:node_2875,prsc:2;n:type:ShaderForge.SFN_Add,id:7734,x:31573,y:32444,varname:node_7734,prsc:2|A-1905-UVOUT,B-5742-OUT;n:type:ShaderForge.SFN_Slider,id:2388,x:31107,y:33020,ptovrint:False,ptlb:V_sped_02,ptin:_V_sped_02,varname:_V_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:3998,x:31436,y:32886,varname:node_3998,prsc:2|A-2315-T,B-2388-OUT;n:type:ShaderForge.SFN_Time,id:2315,x:31211,y:32866,varname:node_2315,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:6120,x:31789,y:32446,ptovrint:False,ptlb:Main_Tex_02,ptin:_Main_Tex_02,varname:_Main_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7734-OUT;n:type:ShaderForge.SFN_Multiply,id:6265,x:32031,y:32765,varname:node_6265,prsc:2|A-6120-RGB,B-6074-RGB;n:type:ShaderForge.SFN_Slider,id:2938,x:31032,y:32729,ptovrint:False,ptlb:U_sped_02,ptin:_U_sped_02,varname:node_2938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:8378,x:32495,y:32965,varname:node_8378,prsc:2|A-2053-A,B-797-A;n:type:ShaderForge.SFN_Multiply,id:1372,x:32672,y:32859,varname:node_1372,prsc:2|A-2393-OUT,B-8378-OUT;proporder:6074-797-9518-8137-8589-2388-6120-2938;pass:END;sub:END;*/

Shader "Shader Forge/SF_panner_shader_luke" {
    Properties {
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _U_sped ("U_sped", Range(-5, 5)) = 0
        _V_sped ("V_sped", Range(-5, 5)) = 0
        _V_sped_02 ("V_sped_02", Range(-5, 5)) = 0
        _Main_Tex_02 ("Main_Tex_02", 2D) = "white" {}
        _U_sped_02 ("U_sped_02", Range(-5, 5)) = 0
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
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
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_2875 = _Time;
                float4 node_2315 = _Time;
                float2 node_7734 = (i.uv0+float2((node_2875.g*_U_sped_02),(node_2315.g*_V_sped_02)));
                float4 _Main_Tex_02_var = tex2D(_Main_Tex_02,TRANSFORM_TEX(node_7734, _Main_Tex_02));
                float4 node_145 = _Time;
                float4 node_8959 = _Time;
                float2 node_1435 = (i.uv0+float2((node_145.g*_U_sped),(node_8959.g*_V_sped)));
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_1435, _Main_Tex));
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
