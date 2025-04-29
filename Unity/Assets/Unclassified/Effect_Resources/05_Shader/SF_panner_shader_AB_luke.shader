// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-2393-OUT,alpha-798-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32495,y:32777,varname:node_2393,prsc:2|A-9586-OUT,B-2053-RGB,C-797-RGB,D-9248-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32235,y:33081,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:798,x:32495,y:32937,varname:node_798,prsc:2|A-1174-OUT,B-2053-A,C-797-A;n:type:ShaderForge.SFN_Tex2d,id:7177,x:31740,y:32552,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7115-OUT;n:type:ShaderForge.SFN_TexCoord,id:8696,x:31296,y:32550,varname:node_8696,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:8168,x:31740,y:32752,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:3974,x:31036,y:32834,ptovrint:False,ptlb:U_sped,ptin:_U_sped,varname:node_8137,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:5447,x:31365,y:32700,varname:node_5447,prsc:2|A-5098-T,B-3974-OUT;n:type:ShaderForge.SFN_Append,id:8989,x:31576,y:32700,varname:node_8989,prsc:2|A-5447-OUT,B-4775-OUT;n:type:ShaderForge.SFN_Time,id:5098,x:31140,y:32680,varname:node_5098,prsc:2;n:type:ShaderForge.SFN_Add,id:7115,x:31524,y:32550,varname:node_7115,prsc:2|A-8696-UVOUT,B-8989-OUT;n:type:ShaderForge.SFN_Slider,id:3106,x:31058,y:33126,ptovrint:False,ptlb:V_sped,ptin:_V_sped,varname:_U_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:4775,x:31387,y:32992,varname:node_4775,prsc:2|A-7698-T,B-3106-OUT;n:type:ShaderForge.SFN_Time,id:7698,x:31162,y:32972,varname:node_7698,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9586,x:32235,y:32651,varname:node_9586,prsc:2|A-7177-RGB,B-8168-RGB;n:type:ShaderForge.SFN_Multiply,id:1174,x:31989,y:32872,varname:node_1174,prsc:2|A-7177-A,B-8168-A;proporder:797-7177-8168-3974-3106;pass:END;sub:END;*/

Shader "Shader Forge/SF_panner_shader_AB_luke" {
    Properties {
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _U_sped ("U_sped", Range(-5, 5)) = 0
        _V_sped ("V_sped", Range(-5, 5)) = 0
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _TintColor;
            uniform sampler2D _Main_Tex; uniform float4 _Main_Tex_ST;
            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
            uniform float _U_sped;
            uniform float _V_sped;
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
                float4 node_5098 = _Time;
                float4 node_7698 = _Time;
                float2 node_7115 = (i.uv0+float2((node_5098.g*_U_sped),(node_7698.g*_V_sped)));
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_7115, _Main_Tex));
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));
                float3 emissive = ((_Main_Tex_var.rgb*_Mask_Tex_var.rgb)*i.vertexColor.rgb*_TintColor.rgb*2.0);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,((_Main_Tex_var.a*_Mask_Tex_var.a)*i.vertexColor.a*_TintColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
