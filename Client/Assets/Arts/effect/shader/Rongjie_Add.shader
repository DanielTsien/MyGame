// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33351,y:32625,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32599,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3574-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32951,y:32719,varname:node_2393,prsc:2|A-9643-OUT,B-3270-OUT,C-797-RGB,D-9248-OUT,E-4472-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32235,y:33081,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Tex2d,id:8350,x:32228,y:33355,ptovrint:False,ptlb:Rongjie_Tex,ptin:_Rongjie_Tex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6440-OUT;n:type:ShaderForge.SFN_TexCoord,id:5067,x:32228,y:33155,varname:node_5067,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Vector4Property,id:7277,x:31119,y:32523,ptovrint:False,ptlb:TextureUV,ptin:_TextureUV,varname:node_7277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:1084,x:31314,y:32544,varname:node_1084,prsc:2|A-7277-X,B-7277-Y;n:type:ShaderForge.SFN_Multiply,id:5225,x:31490,y:32484,varname:node_5225,prsc:2|A-3651-T,B-1084-OUT;n:type:ShaderForge.SFN_Time,id:3651,x:31314,y:32419,varname:node_3651,prsc:2;n:type:ShaderForge.SFN_Add,id:2128,x:31679,y:32674,varname:node_2128,prsc:2|A-5225-OUT,B-1949-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1949,x:31428,y:32771,varname:node_1949,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:423,x:31679,y:32850,varname:node_423,prsc:2|A-1949-UVOUT,B-4693-OUT;n:type:ShaderForge.SFN_TexCoord,id:4889,x:31252,y:32962,varname:node_4889,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Append,id:4693,x:31485,y:32984,varname:node_4693,prsc:2|A-4889-U,B-4889-V;n:type:ShaderForge.SFN_SwitchProperty,id:3574,x:31932,y:32778,ptovrint:False,ptlb:UV Custom Data？,ptin:_UVCustomData,varname:node_3574,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2128-OUT,B-423-OUT;n:type:ShaderForge.SFN_Vector4Property,id:8132,x:31452,y:33337,ptovrint:False,ptlb:RongjieUV,ptin:_RongjieUV,varname:_node_7277_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:9980,x:31647,y:33358,varname:node_9980,prsc:2|A-8132-X,B-8132-Y;n:type:ShaderForge.SFN_Multiply,id:785,x:31823,y:33298,varname:node_785,prsc:2|A-2895-T,B-9980-OUT;n:type:ShaderForge.SFN_Time,id:2895,x:31647,y:33233,varname:node_2895,prsc:2;n:type:ShaderForge.SFN_Add,id:6440,x:32017,y:33355,varname:node_6440,prsc:2|A-785-OUT,B-7438-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:7438,x:31823,y:33465,varname:node_7438,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:3270,x:32492,y:32765,varname:node_3270,prsc:2|A-2053-RGB,B-2053-A;n:type:ShaderForge.SFN_Multiply,id:8459,x:32502,y:32611,varname:node_8459,prsc:2|A-6074-RGB,B-6074-A;n:type:ShaderForge.SFN_SwitchProperty,id:9643,x:32749,y:32572,ptovrint:False,ptlb:Quse,ptin:_Quse,varname:node_9643,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-8459-OUT,B-8852-OUT;n:type:ShaderForge.SFN_Desaturate,id:8852,x:32542,y:32439,varname:node_8852,prsc:2|COL-6074-RGB;n:type:ShaderForge.SFN_SwitchProperty,id:4472,x:32773,y:33171,ptovrint:False,ptlb:Rongjie？,ptin:_Rongjie,varname:node_4472,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-8517-OUT,B-3148-OUT;n:type:ShaderForge.SFN_Vector1,id:8517,x:32515,y:33196,varname:node_8517,prsc:2,v1:1;n:type:ShaderForge.SFN_If,id:3148,x:32577,y:33345,varname:node_3148,prsc:2|A-8350-R,B-5067-W,GT-694-OUT,EQ-694-OUT,LT-2061-OUT;n:type:ShaderForge.SFN_Vector1,id:694,x:32387,y:33425,varname:node_694,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:2061,x:32387,y:33505,varname:node_2061,prsc:2,v1:0;proporder:9643-797-6074-3574-7277-8350-8132-4472;pass:END;sub:END;*/

Shader "Shader Forge/Rongjie_Add" {
    Properties {
        [MaterialToggle] _Quse ("Quse", Float ) = 0
        [HDR]_TintColor ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        [MaterialToggle] _UVCustomData ("UV Custom Data？", Float ) = 0
        _TextureUV ("TextureUV", Vector) = (0,0,0,0)
        _Rongjie_Tex ("Rongjie_Tex", 2D) = "white" {}
        _RongjieUV ("RongjieUV", Vector) = (0,0,0,0)
        [MaterialToggle] _Rongjie ("Rongjie？", Float ) = 1

			//特效裁切修改-------
	_MinX("MinX", Float) = -1000000
	_MaxX("MaxX", Float) = 1000000
	_MinY("MinY", Float) = -1000000
	_MaxY("MaxY", Float) = 1000000
			//------------------
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
            ColorMask RGB
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // #pragma multi_compile_fwdbase
            //#pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            CBUFFER_START(UnityPerMaterial)
                uniform float4 _MainTex_ST;
                uniform float4 _TintColor;
                uniform float4 _Rongjie_Tex_ST;
                uniform float4 _TextureUV;
                uniform half _UVCustomData;
                uniform float4 _RongjieUV;
                uniform half _Quse;
                uniform half _Rongjie;
                uniform float _MinX;
                uniform float _MaxX;
                uniform float _MinY;
                uniform float _MaxY;
            CBUFFER_END
            uniform sampler2D _Rongjie_Tex; 
            uniform sampler2D _MainTex; 

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
				//特效裁切修改-------
				float2 worldPos : TEXCOORD2;
				//------------------
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = TransformObjectToHClip( v.vertex.xyz );
				//获取世界坐标
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_3651 = _Time;
                float2 _UVCustomData_var = lerp( ((node_3651.g*float2(_TextureUV.r,_TextureUV.g))+i.uv0), (i.uv0+float2(i.uv1.r,i.uv1.g)), _UVCustomData );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(_UVCustomData_var, _MainTex));
                float4 node_2895 = _Time;
                float2 node_6440 = ((node_2895.g*float2(_RongjieUV.r,_RongjieUV.g))+i.uv0);
                float4 _Rongjie_Tex_var = tex2D(_Rongjie_Tex,TRANSFORM_TEX(node_6440, _Rongjie_Tex));
                float node_3148_if_leA = step(_Rongjie_Tex_var.r,i.uv1.a);
                float node_3148_if_leB = step(i.uv1.a,_Rongjie_Tex_var.r);
                float node_694 = 1.0;
                float3 emissive = (lerp( (_MainTex_var.rgb*_MainTex_var.a), dot(_MainTex_var.rgb,float3(0.3,0.59,0.11)), _Quse )*(i.vertexColor.rgb*i.vertexColor.a)*_TintColor.rgb*2.0*lerp( 1.0, lerp((node_3148_if_leA*0.0)+(node_3148_if_leB*node_694),node_694,node_3148_if_leA*node_3148_if_leB), _Rongjie ));
                float3 finalColor = emissive;
				//是否在区域内
				bool inArea = i.worldPos.x >= _MinX && i.worldPos.x <= _MaxX && i.worldPos.y >= _MinY && i.worldPos.y <= _MaxY;
				return  inArea ? float4(finalColor, 1) : float4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
    // CustomEditor "ShaderForgeMaterialInspector"
}
