// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:Mobile/Unlit (Supports Lightmap),lico:0,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:False,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32590,y:32698|emission-7-OUT;n:type:ShaderForge.SFN_Color,id:4,x:33467,y:32457,ptlb:Colour 01,ptin:_Colour01,glob:False,c1:0.007352948,c2:0.5892495,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:6,x:33470,y:32275,ptlb:Colour 02,ptin:_Colour02,glob:False,c1:0.004865922,c2:0.1724883,c3:0.6617647,c4:1;n:type:ShaderForge.SFN_Lerp,id:7,x:33053,y:32498|A-6-RGB,B-4-RGB,T-9-OUT;n:type:ShaderForge.SFN_Posterize,id:9,x:33496,y:32950|IN-13-RGB,STPS-11-OUT;n:type:ShaderForge.SFN_ValueProperty,id:11,x:33676,y:33033,ptlb:Banding,ptin:_Banding,glob:False,v1:8;n:type:ShaderForge.SFN_Tex2d,id:13,x:33685,y:32611,ptlb:Gradient,ptin:_Gradient,tex:e4435b5b835da7b4296717bb3f2041a9,ntxv:0,isnm:False|UVIN-40-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:28,x:34592,y:32191;n:type:ShaderForge.SFN_ComponentMask,id:29,x:34383,y:32103,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-28-XYZ;n:type:ShaderForge.SFN_Add,id:40,x:33841,y:32378|A-42-OUT,B-46-OUT;n:type:ShaderForge.SFN_Multiply,id:42,x:34176,y:32342|A-29-OUT,B-44-OUT;n:type:ShaderForge.SFN_ValueProperty,id:44,x:34361,y:32535,ptlb:Global Tile,ptin:_GlobalTile,glob:False,v1:1;n:type:ShaderForge.SFN_Slider,id:46,x:34044,y:32762,ptlb:Offset,ptin:_Offset,min:0,cur:0,max:1;proporder:13-4-6-11-44-46;pass:END;sub:END;*/

Shader "RetroboyFX/Retroboy Simplesky" {
    Properties {
        _Gradient ("Gradient", 2D) = "white" {}
        _Colour01 ("Colour 01", Color) = (0.007352948,0.5892495,1,1)
        _Colour02 ("Colour 02", Color) = (0.004865922,0.1724883,0.6617647,1)
        _Banding ("Banding", Float ) = 8
        _GlobalTile ("Global Tile", Float ) = 1
        _Offset ("Offset", Range(0, 1)) = 0
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _Colour01;
            uniform float4 _Colour02;
            uniform float _Banding;
            uniform sampler2D _Gradient; uniform float4 _Gradient_ST;
            uniform float _GlobalTile;
            uniform float _Offset;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_40 = ((i.posWorld.rgb.rg*_GlobalTile)+_Offset);
                float3 emissive = lerp(_Colour02.rgb,_Colour01.rgb,floor(tex2D(_Gradient,TRANSFORM_TEX(node_40, _Gradient)).rgb * _Banding) / (_Banding - 1));
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Mobile/Unlit (Supports Lightmap)"
    CustomEditor "ShaderForgeMaterialInspector"
}
