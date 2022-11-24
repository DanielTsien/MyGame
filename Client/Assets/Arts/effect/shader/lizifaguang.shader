// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-4334-OUT;n:type:ShaderForge.SFN_Tex2d,id:1005,x:32635,y:32742,ptovrint:False,ptlb:Text,ptin:_Text,varname:node_1005,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7474,x:32807,y:32806,varname:node_7474,prsc:2|A-1005-RGB,B-1005-A;n:type:ShaderForge.SFN_Color,id:814,x:32567,y:32952,ptovrint:False,ptlb:color,ptin:_color,varname:node_814,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:2761,x:32946,y:32904,varname:node_2761,prsc:2|A-7474-OUT,B-814-RGB,C-2201-RGB,D-4288-OUT;n:type:ShaderForge.SFN_Vector1,id:4288,x:32524,y:33114,varname:node_4288,prsc:2,v1:2;n:type:ShaderForge.SFN_VertexColor,id:2201,x:32589,y:33238,varname:node_2201,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5954,x:32914,y:33115,varname:node_5954,prsc:2|A-814-A,B-2201-A;n:type:ShaderForge.SFN_Multiply,id:4334,x:33097,y:33212,varname:node_4334,prsc:2|A-2761-OUT,B-5954-OUT;proporder:814-1005;pass:END;sub:END;*/

Shader "Shader Forge/lizifaguang" {
    Properties {
        [HDR]_color ("color", Color) = (0.5,0.5,0.5,1)
        _Text ("Text", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalRenderPipeline"
        }
        Pass {
            Name "FORWARD"
            Tags {
                // "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // #include "UnityCG.cginc"
            // #pragma multi_compile_fwdbase
            //#pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            CBUFFER_START(UnityPerMaterial)
                uniform float4 _Text_ST;
                uniform float4 _color;
            CBUFFER_END
            uniform sampler2D _Text; 
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = TransformObjectToHClip( v.vertex.xyz );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
                float4 _Text_var = tex2D(_Text,TRANSFORM_TEX(i.uv0, _Text));
                float3 finalColor = (((_Text_var.rgb*_Text_var.a)*_color.rgb*i.vertexColor.rgb*2.0)*(_color.a*i.vertexColor.a));
                return float4(finalColor,1);
            }
            ENDHLSL
        }
        // Pass {
        //     Name "ShadowCaster"
        //     Tags {
        //         "LightMode"="ShadowCaster"
        //     }
        //     Offset 1, 1
        //     Cull Off
            
        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #include "UnityCG.cginc"
        //     #include "Lighting.cginc"
        //     #pragma fragmentoption ARB_precision_hint_fastest
        //     #pragma multi_compile_shadowcaster
        //     //#pragma only_renderers d3d9 d3d11 glcore gles gles3 
        //     #pragma target 3.0
        //     struct VertexInput {
        //         float4 vertex : POSITION;
        //     };
        //     struct VertexOutput {
        //         V2F_SHADOW_CASTER;
        //     };
        //     VertexOutput vert (VertexInput v) {
        //         VertexOutput o = (VertexOutput)0;
        //         o.pos = UnityObjectToClipPos( v.vertex );
        //         TRANSFER_SHADOW_CASTER(o)
        //         return o;
        //     }
        //     float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
        //         float isFrontFace = ( facing >= 0 ? 1 : 0 );
        //         float faceSign = ( facing >= 0 ? 1 : -1 );
        //         SHADOW_CASTER_FRAGMENT(i)
        //     }
        //     ENDCG
        // }
    }
    // FallBack "Diffuse"
    // CustomEditor "ShaderForgeMaterialInspector"
}
