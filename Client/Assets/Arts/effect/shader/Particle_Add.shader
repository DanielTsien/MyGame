// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Shader "Custom/UI/Particles/Additive" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    _MinX("MinX", Float) = -1000000
    _MaxX("MaxX", Float) = 1000000 
    _MinY("MinY", Float) = -1000000
    _MaxY("MaxY", Float) = 1000000
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "RenderPipeline" = "UniversalRenderPipeline" }
    Blend SrcAlpha One
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off

    SubShader {
        Pass {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                real4 _TintColor;
                float _MinX;
                float _MaxX;
                float _MinY;
                float _MaxY;
                float _InvFade;
            CBUFFER_END

            sampler2D _MainTex;
			TEXTURE2D(_CameraDepthTexture);
			SAMPLER(sampler_CameraDepthTexture);

            struct appdata_t {
                float4 vertex : POSITION;
                real4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                // UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                real4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                float2 worldPos : TEXCOORD3;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                // UNITY_SETUP_INSTANCE_ID(v);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                    // COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            // UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            real4 frag (v2f i) : SV_Target
            {
                
				#ifdef SOFTPARTICLES_ON
					float partZ = i.projPos.z;
					float2 screenPos = i.projPos .xy / i.projPos .w;
					float sceneZ = LinearEyeDepth (SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, screenPos), _ZBufferParams);
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
				#endif

                real4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
                col.a = saturate(col.a); // alpha should not have double-brightness applied to it, but we can't fix that legacy behavior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)

				bool inArea = i.worldPos.x >= _MinX && i.worldPos.x <= _MaxX && i.worldPos.y >= _MinY && i.worldPos.y <= _MaxY;
				return  inArea ? col : real4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
}
}
