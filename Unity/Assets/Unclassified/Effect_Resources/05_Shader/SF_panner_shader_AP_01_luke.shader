// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-5734-OUT;n:type:ShaderForge.SFN_Tex2d,id:8757,x:32112,y:32766,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4b8326bcc54290141981e19239b8b611,ntxv:0,isnm:False|UVIN-1769-OUT;n:type:ShaderForge.SFN_Multiply,id:5734,x:32515,y:32766,varname:node_5734,prsc:2|A-5128-OUT,B-6913-RGB,C-4674-RGB,D-6805-OUT;n:type:ShaderForge.SFN_VertexColor,id:6913,x:32323,y:32892,varname:node_6913,prsc:2;n:type:ShaderForge.SFN_Color,id:4674,x:32323,y:33063,ptovrint:True,ptlb:Color_01,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:6805,x:32340,y:33220,varname:node_6805,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:7436,x:31668,y:32764,varname:node_7436,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:5128,x:32323,y:32766,varname:node_5128,prsc:2|A-8757-RGB,B-942-OUT;n:type:ShaderForge.SFN_Tex2d,id:2027,x:31820,y:33063,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_9518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0ad836bc7685cb74aae9777609f58301,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:7974,x:31947,y:32914,varname:node_7974,prsc:2|A-825-OUT,B-7325-OUT;n:type:ShaderForge.SFN_Add,id:1769,x:31896,y:32764,varname:node_1769,prsc:2|A-7436-UVOUT,B-7974-OUT;n:type:ShaderForge.SFN_Vector1,id:825,x:31752,y:32914,varname:node_825,prsc:2,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7924,x:31418,y:33023,ptovrint:False,ptlb:ap_add,ptin:_ap_add,varname:node_2122,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.1;n:type:ShaderForge.SFN_Add,id:7325,x:31648,y:33023,varname:node_7325,prsc:2|A-7924-OUT,B-6913-A;n:type:ShaderForge.SFN_TexCoord,id:3806,x:31376,y:33247,varname:node_3806,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:9857,x:31445,y:33397,varname:node_9857,prsc:2|A-2886-T,B-2111-OUT;n:type:ShaderForge.SFN_Append,id:1533,x:31656,y:33397,varname:node_1533,prsc:2|A-9857-OUT,B-2028-OUT;n:type:ShaderForge.SFN_Time,id:2886,x:31220,y:33377,varname:node_2886,prsc:2;n:type:ShaderForge.SFN_Add,id:6704,x:31604,y:33247,varname:node_6704,prsc:2|A-3806-UVOUT,B-1533-OUT;n:type:ShaderForge.SFN_Slider,id:1591,x:31138,y:33823,ptovrint:False,ptlb:V_sped_02,ptin:_V_sped_02,varname:_V_sped_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:2028,x:31467,y:33689,varname:node_2028,prsc:2|A-5726-T,B-1591-OUT;n:type:ShaderForge.SFN_Time,id:5726,x:31242,y:33669,varname:node_5726,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:3922,x:31820,y:33249,ptovrint:False,ptlb:Main_Tex_02,ptin:_Main_Tex_02,varname:_Main_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:626a7ae973a445949945d6223455bdb7,ntxv:0,isnm:False|UVIN-6704-OUT;n:type:ShaderForge.SFN_Multiply,id:5289,x:31990,y:33063,varname:node_5289,prsc:2|A-2027-RGB,B-3922-RGB;n:type:ShaderForge.SFN_Slider,id:2111,x:31027,y:33594,ptovrint:False,ptlb:U_sped_02,ptin:_U_sped_02,varname:node_2938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Color,id:6561,x:31990,y:33249,ptovrint:False,ptlb:color_02,ptin:_color_02,varname:node_1079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:942,x:32161,y:33063,varname:node_942,prsc:2|A-5289-OUT,B-6561-RGB;proporder:8757-4674-2027-1591-3922-2111-6561-7924;pass:END;sub:END;*/

Shader "Shader Forge/SF_panner_shader_AP_01_luke" {
    Properties {
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _TintColor ("Color_01", Color) = (0.5,0.5,0.5,1)
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _V_sped_02 ("V_sped_02", Range(-5, 5)) = 0
        _Main_Tex_02 ("Main_Tex_02", 2D) = "white" {}
        _U_sped_02 ("U_sped_02", Range(-5, 5)) = 0
        _color_02 ("color_02", Color) = (0.5,0.5,0.5,1)
        _ap_add ("ap_add", Float ) = -0.1
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
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Main_Tex; uniform float4 _Main_Tex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
            uniform float _ap_add;
            uniform float _V_sped_02;
            uniform sampler2D _Main_Tex_02; uniform float4 _Main_Tex_02_ST;
            uniform float _U_sped_02;
            uniform float4 _color_02;
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
                float2 node_1769 = (i.uv0+float2(0.0,(_ap_add+i.vertexColor.a)));
                float4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_1769, _Main_Tex));
                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));
                float4 node_2886 = _Time + _TimeEditor;
                float4 node_5726 = _Time + _TimeEditor;
                float2 node_6704 = (i.uv0+float2((node_2886.g*_U_sped_02),(node_5726.g*_V_sped_02)));
                float4 _Main_Tex_02_var = tex2D(_Main_Tex_02,TRANSFORM_TEX(node_6704, _Main_Tex_02));
                float3 emissive = ((_Main_Tex_var.rgb*((_Mask_Tex_var.rgb*_Main_Tex_02_var.rgb)*_color_02.rgb))*i.vertexColor.rgb*_TintColor.rgb*2.0);
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
