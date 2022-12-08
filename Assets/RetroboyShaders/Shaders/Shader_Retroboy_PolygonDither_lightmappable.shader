// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:True,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32638,y:32479|diff-211-OUT;n:type:ShaderForge.SFN_Tex2d,id:28,x:33828,y:31987,ptlb:Dither Texture 01,ptin:_DitherTexture01,ntxv:1,isnm:False|UVIN-102-OUT;n:type:ShaderForge.SFN_ScreenPos,id:101,x:34402,y:31881,sctp:1;n:type:ShaderForge.SFN_Multiply,id:102,x:34095,y:31945|A-101-UVOUT,B-103-OUT;n:type:ShaderForge.SFN_ValueProperty,id:103,x:34402,y:32059,ptlb:Dither Tile,ptin:_DitherTile,glob:False,v1:4;n:type:ShaderForge.SFN_ChannelBlend,id:191,x:33379,y:32701|M-193-OUT,R-28-RGB,G-215-RGB,B-217-RGB;n:type:ShaderForge.SFN_NormalVector,id:192,x:34584,y:32442,pt:False;n:type:ShaderForge.SFN_Posterize,id:193,x:33834,y:32529|IN-192-OUT,STPS-194-OUT;n:type:ShaderForge.SFN_ValueProperty,id:194,x:34087,y:32723,ptlb:Dither Blend,ptin:_DitherBlend,glob:False,v1:4;n:type:ShaderForge.SFN_Color,id:210,x:33210,y:32423,ptlb:Base Color,ptin:_BaseColor,glob:False,c1:0.141115,c2:0.4862745,c3:0.1603439,c4:1;n:type:ShaderForge.SFN_Lerp,id:211,x:33006,y:32482|A-213-RGB,B-210-RGB,T-191-OUT;n:type:ShaderForge.SFN_Color,id:213,x:33210,y:32262,ptlb:Secondary Color,ptin:_SecondaryColor,glob:False,c1:0.124506,c2:0.3607843,c3:0.1376691,c4:1;n:type:ShaderForge.SFN_Tex2d,id:215,x:33829,y:32172,ptlb:Dither Texture 02,ptin:_DitherTexture02,ntxv:1,isnm:False|UVIN-102-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:33829,y:32356,ptlb:Dither Texture 03,ptin:_DitherTexture03,ntxv:1,isnm:False|UVIN-102-OUT;proporder:210-213-28-215-217-103-194;pass:END;sub:END;*/

Shader "RetroboyFX/Retroboy PolygonDither lightmappable" {
    Properties {
        _BaseColor ("Base Color", Color) = (0.141115,0.4862745,0.1603439,1)
        _SecondaryColor ("Secondary Color", Color) = (0.124506,0.3607843,0.1376691,1)
        _DitherTexture01 ("Dither Texture 01", 2D) = "gray" {}
        _DitherTexture02 ("Dither Texture 02", 2D) = "gray" {}
        _DitherTexture03 ("Dither Texture 03", 2D) = "gray" {}
        _DitherTile ("Dither Tile", Float ) = 4
        _DitherBlend ("Dither Blend", Float ) = 4
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
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
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 binormalDir : TEXCOORD3;
                float4 screenPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
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
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                #ifndef LIGHTMAP_OFF
                    float3 diffuse = lightmap;
                #else
                    float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
                #endif
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float3 node_193 = floor(i.normalDir * _DitherBlend) / (_DitherBlend - 1);
                float2 node_102 = (float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg*_DitherTile);
                finalColor += diffuseLight * lerp(_SecondaryColor.rgb,_BaseColor.rgb,(node_193.r*tex2D(_DitherTexture01,TRANSFORM_TEX(node_102, _DitherTexture01)).rgb + node_193.g*tex2D(_DitherTexture02,TRANSFORM_TEX(node_102, _DitherTexture02)).rgb + node_193.b*tex2D(_DitherTexture03,TRANSFORM_TEX(node_102, _DitherTexture03)).rgb));
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
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
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 binormalDir : TEXCOORD3;
                float4 screenPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float3 node_193 = floor(i.normalDir * _DitherBlend) / (_DitherBlend - 1);
                float2 node_102 = (float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg*_DitherTile);
                finalColor += diffuseLight * lerp(_SecondaryColor.rgb,_BaseColor.rgb,(node_193.r*tex2D(_DitherTexture01,TRANSFORM_TEX(node_102, _DitherTexture01)).rgb + node_193.g*tex2D(_DitherTexture02,TRANSFORM_TEX(node_102, _DitherTexture02)).rgb + node_193.b*tex2D(_DitherTexture03,TRANSFORM_TEX(node_102, _DitherTexture03)).rgb));
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
