// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:0,limd:0,uamb:True,mssp:True,lmpd:True,lprd:True,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32638,y:32479|emission-211-OUT;n:type:ShaderForge.SFN_Tex2d,id:28,x:33908,y:31878,ptlb:Dither Texture 01,ptin:_DitherTexture01,ntxv:1,isnm:False|UVIN-102-OUT;n:type:ShaderForge.SFN_ScreenPos,id:101,x:34394,y:31761,sctp:1;n:type:ShaderForge.SFN_Multiply,id:102,x:34087,y:31825|A-101-UVOUT,B-103-OUT;n:type:ShaderForge.SFN_ValueProperty,id:103,x:34394,y:31939,ptlb:Dither Tile,ptin:_DitherTile,glob:False,v1:4;n:type:ShaderForge.SFN_ChannelBlend,id:191,x:33594,y:32177|M-193-OUT,R-28-RGB,G-215-RGB,B-217-RGB;n:type:ShaderForge.SFN_NormalVector,id:192,x:34594,y:32343,pt:False;n:type:ShaderForge.SFN_Posterize,id:193,x:33844,y:32430|IN-192-OUT,STPS-194-OUT;n:type:ShaderForge.SFN_ValueProperty,id:194,x:34097,y:32624,ptlb:Dither Blend,ptin:_DitherBlend,glob:False,v1:2;n:type:ShaderForge.SFN_Color,id:210,x:33379,y:32407,ptlb:Base Color,ptin:_BaseColor,glob:False,c1:0.141115,c2:0.4862745,c3:0.1603439,c4:1;n:type:ShaderForge.SFN_Lerp,id:211,x:33006,y:32482|A-213-RGB,B-210-RGB,T-191-OUT;n:type:ShaderForge.SFN_Color,id:213,x:33374,y:32584,ptlb:Secondary Color,ptin:_SecondaryColor,glob:False,c1:0.124506,c2:0.3607843,c3:0.1376691,c4:1;n:type:ShaderForge.SFN_Tex2d,id:215,x:33910,y:32053,ptlb:Dither Texture 02,ptin:_DitherTexture02,ntxv:1,isnm:False|UVIN-102-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:33912,y:32228,ptlb:Dither Texture 03,ptin:_DitherTexture03,ntxv:1,isnm:False|UVIN-102-OUT;proporder:210-213-28-215-217-103-194;pass:END;sub:END;*/

Shader "RetroboyFX/Retroboy Polygon Dither unlit" {
    Properties {
        _BaseColor ("Base Color", Color) = (0.141115,0.4862745,0.1603439,1)
        _SecondaryColor ("Secondary Color", Color) = (0.124506,0.3607843,0.1376691,1)
        _DitherTexture01 ("Dither Texture 01", 2D) = "gray" {}
        _DitherTexture02 ("Dither Texture 02", 2D) = "gray" {}
        _DitherTexture03 ("Dither Texture 03", 2D) = "gray" {}
        _DitherTile ("Dither Tile", Float ) = 4
        _DitherBlend ("Dither Blend", Float ) = 2
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                // sampler2D unity_Lightmap;
                // float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _DitherTexture01; uniform float4 _DitherTexture01_ST;
            uniform float _DitherTile;
            uniform float _DitherBlend;
            uniform float4 _BaseColor;
            uniform float4 _SecondaryColor;
            uniform sampler2D _DitherTexture02; uniform float4 _DitherTexture02_ST;
            uniform sampler2D _DitherTexture03; uniform float4 _DitherTexture03_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD0;
                float3 tangentDir : TEXCOORD1;
                float3 binormalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD4;
                #else
                    float3 shLight : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                #ifdef LIGHTMAP_OFF
                    o.shLight = ShadeSH9(float4(v.normal * 1.0,1)) * 0.5;
                #endif
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, float3(0,0,1)));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM));
                    #endif
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
////// Lighting:
////// Emissive:
                float3 node_193 = floor(i.normalDir * _DitherBlend) / (_DitherBlend - 1);
                float2 node_102 = (float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg*_DitherTile);
                float3 emissive = lerp(_SecondaryColor.rgb,_BaseColor.rgb,(node_193.r*tex2D(_DitherTexture01,TRANSFORM_TEX(node_102, _DitherTexture01)).rgb + node_193.g*tex2D(_DitherTexture02,TRANSFORM_TEX(node_102, _DitherTexture02)).rgb + node_193.b*tex2D(_DitherTexture03,TRANSFORM_TEX(node_102, _DitherTexture03)).rgb));
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
