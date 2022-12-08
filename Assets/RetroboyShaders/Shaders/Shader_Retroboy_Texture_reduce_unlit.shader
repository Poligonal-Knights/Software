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
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:True,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32545,y:32714|emission-213-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:34078,y:32623,ptlb:Diffuse,ptin:_Diffuse,tex:1ddd8424b9882034da74098b4d1afc44,ntxv:0,isnm:False|MIP-51-OUT;n:type:ShaderForge.SFN_Posterize,id:32,x:33459,y:32925|IN-278-OUT,STPS-34-OUT;n:type:ShaderForge.SFN_ValueProperty,id:34,x:33570,y:32814,ptlb:Colour Reduce,ptin:_ColourReduce,glob:False,v1:4;n:type:ShaderForge.SFN_ValueProperty,id:51,x:34345,y:32675,ptlb:Pixelate,ptin:_Pixelate,glob:False,v1:2;n:type:ShaderForge.SFN_Desaturate,id:63,x:33837,y:32754|COL-2-RGB;n:type:ShaderForge.SFN_Color,id:71,x:33155,y:32549,ptlb:Color 01,ptin:_Color01,glob:False,c1:0.2232356,c2:0.3602941,c3:0.03443987,c4:1;n:type:ShaderForge.SFN_Slider,id:93,x:33990,y:33142,ptlb:Contrast,ptin:_Contrast,min:0,cur:1,max:3;n:type:ShaderForge.SFN_Clamp01,id:189,x:33153,y:32970|IN-32-OUT;n:type:ShaderForge.SFN_Color,id:212,x:33153,y:32731,ptlb:Color 02,ptin:_Color02,glob:False,c1:0.5708668,c2:0.9852941,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:213,x:32904,y:32664|A-71-RGB,B-212-RGB,T-189-OUT;n:type:ShaderForge.SFN_Power,id:278,x:33756,y:33008|VAL-63-OUT,EXP-93-OUT;proporder:2-71-212-34-51-93;pass:END;sub:END;*/

Shader "RetroboyFX/Retroboy Texture reduce unlit" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color01 ("Color 01", Color) = (0.2232356,0.3602941,0.03443987,1)
        _Color02 ("Color 02", Color) = (0.5708668,0.9852941,0,1)
        _ColourReduce ("Colour Reduce", Float ) = 4
        _Pixelate ("Pixelate", Float ) = 2
        _Contrast ("Contrast", Range(0, 3)) = 1
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
            #pragma glsl
            #ifndef LIGHTMAP_OFF
                // sampler2D unity_Lightmap;
                // float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _ColourReduce;
            uniform float _Pixelate;
            uniform float4 _Color01;
            uniform float _Contrast;
            uniform float4 _Color02;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 binormalDir : TEXCOORD3;
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos(v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
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
////// Lighting:
////// Emissive:
                float2 node_286 = i.uv0;
                float3 emissive = lerp(_Color01.rgb,_Color02.rgb,saturate(floor(pow(dot(tex2Dlod(_Diffuse,float4(TRANSFORM_TEX(node_286.rg, _Diffuse),0.0,_Pixelate)).rgb,float3(0.3,0.59,0.11)),_Contrast) * _ColourReduce) / (_ColourReduce - 1)));
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
