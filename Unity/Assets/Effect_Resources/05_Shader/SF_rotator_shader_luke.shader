// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-4672-OUT;n:type:ShaderForge.SFN_Tex2d,id:5208,x:31566,y:32682,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:626a7ae973a445949945d6223455bdb7,ntxv:0,isnm:False|UVIN-9616-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4672,x:32509,y:32781,varname:node_4672,prsc:2|A-2904-OUT,B-8144-RGB,C-5114-RGB,D-8475-OUT;n:type:ShaderForge.SFN_VertexColor,id:8144,x:32317,y:32907,varname:node_8144,prsc:2;n:type:ShaderForge.SFN_Color,id:5114,x:32317,y:33071,ptovrint:True,ptlb:Color_copy,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:8475,x:32317,y:33216,varname:node_8475,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:8408,x:31122,y:32680,varname:node_8408,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2904,x:32317,y:32781,varname:node_2904,prsc:2|A-7814-OUT,B-2753-RGB;n:type:ShaderForge.SFN_Tex2d,id:2753,x:32045,y:32914,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6b6e26ed2663f904aaa7cdd6e76dd7b1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:2888,x:30965,y:32863,ptovrint:False,ptlb:sped,ptin:_sped,varname:_U_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_TexCoord,id:7068,x:31111,y:32425,varname:node_7068,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:6817,x:31566,y:32483,ptovrint:False,ptlb:Main_Tex_02,ptin:_Main_Tex_02,varname:_Main_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:626a7ae973a445949945d6223455bdb7,ntxv:0,isnm:False|UVIN-7873-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9221,x:31828,y:32591,varname:node_9221,prsc:2|A-6817-RGB,B-5208-RGB;n:type:ShaderForge.SFN_Slider,id:7136,x:30947,y:32594,ptovrint:False,ptlb:sped_02,ptin:_sped_02,varname:node_2938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Rotator,id:7873,x:31336,y:32425,varname:node_7873,prsc:2|UVIN-7068-UVOUT,SPD-7136-OUT;n:type:ShaderForge.SFN_Rotator,id:9616,x:31346,y:32680,varname:node_9616,prsc:2|UVIN-8408-UVOUT,SPD-2888-OUT;n:type:ShaderForge.SFN_Multiply,id:7814,x:32066,y:32700,varname:node_7814,prsc:2|A-9221-OUT,B-2564-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2564,x:31845,y:32796,ptovrint:False,ptlb:pow,ptin:_pow,varname:node_2564,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:5208-5114-2753-6817-7136-2888-2564;pass:END;sub:END;*/

Shader "Shader Forge/SF_rotator_shader_luke" {
    Properties {
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _TintColor ("Color_copy", Color) = (0.5,0.5,0.5,1)
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _Main_Tex_02 ("Main_Tex_02", 2D) = "white" {}
        _sped_02 ("sped_02", Range(-5, 5)) = 0
        _sped ("sped", Range(-5, 5)) = 0
        _pow ("pow", Float ) = 0
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
            uniform float _sped;
            uniform sampler2D _Main_Tex_02; uniform float4 _Main_Tex_02_ST;
            uniform float _sped_02;
            uniform float _pow;
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
                float4 node_501 = _Time + _TimeEditor;
                float node_7873_ang = node_501.g;
                float node_7873_spd = _sped_02;
                float node_7873_cos = cos(node_7873_spd*node_7873_ang);
                float node_7873_sin = sin(node_7873_spd*node_7873_ang);
                float2 node_7873_piv = float2(0.5,0.5);
                float2 node_7873 = (mul(i.uv0-node_7873_piv,float2x2( node_7873_cos, -node_7873_sin, node_7873_sin, node_7873_cos))+node_7873_piv);
                float4 _Main_Tex_02_var = tex2D(_Main_Tex_02,TRANSFORM_TEX(node_7873, _Main_Tex_02));
                float node_9616_ang = node_501.g;
                float node_9616_spd = _sped;
                float node_9616_cos = cos(node_9616_spd*node_9616_ang);
                float node_9616_sin = sin(node_9616_spd*node_9616_ang);
                float2 node_9616_piv = float2(0.5,0.5);
                float2 node_9616 = (mul(i.uv0-node_9616_piv,float2x2( node_9616_cos, -node_9616_sin, node_9616_sin, node_9616_cos))+node_9616_piv);
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_9616, _Main_Tex));
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));
                float3 emissive = ((((_Main_Tex_02_var.rgb*_Main_Tex_var.rgb)*_pow)*_Mask_Tex_var.rgb)*i.vertexColor.rgb*_TintColor.rgb*2.0);
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
