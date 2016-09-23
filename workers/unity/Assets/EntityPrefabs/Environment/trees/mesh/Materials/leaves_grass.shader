// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|diff-4032-RGB,emission-4032-RGB,clip-4032-A,voffset-8775-OUT;n:type:ShaderForge.SFN_Tex2d,id:4032,x:32102,y:32552,ptovrint:False,ptlb:node_4032,ptin:_node_4032,varname:node_4032,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:12864adb4bd427a4a9f38a6473467982,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:5517,x:31582,y:32586,varname:node_5517,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4157,x:31490,y:33030,varname:node_4157,prsc:2|A-1205-OUT,B-6694-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1205,x:31193,y:32948,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_1205,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Time,id:978,x:30858,y:33332,varname:node_978,prsc:2;n:type:ShaderForge.SFN_Cos,id:626,x:31059,y:33243,varname:node_626,prsc:2|IN-978-T;n:type:ShaderForge.SFN_TexCoord,id:9234,x:30836,y:33146,varname:node_9234,prsc:2,uv:0;n:type:ShaderForge.SFN_OneMinus,id:1358,x:31059,y:33093,varname:node_1358,prsc:2|IN-9234-V;n:type:ShaderForge.SFN_Multiply,id:6694,x:31273,y:33141,varname:node_6694,prsc:2|A-1358-OUT,B-626-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:4168,x:31071,y:32599,varname:node_4168,prsc:2;n:type:ShaderForge.SFN_Sin,id:1708,x:31266,y:32637,varname:node_1708,prsc:2|IN-4168-X;n:type:ShaderForge.SFN_Multiply,id:8775,x:32196,y:32916,varname:node_8775,prsc:2|A-5517-R,B-8198-OUT;n:type:ShaderForge.SFN_Multiply,id:6923,x:31728,y:32976,varname:node_6923,prsc:2|A-1708-OUT,B-4157-OUT;n:type:ShaderForge.SFN_Append,id:8198,x:31938,y:32934,varname:node_8198,prsc:2|A-6923-OUT,B-4562-OUT,C-6923-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4562,x:31684,y:32807,ptovrint:False,ptlb:Direction,ptin:_Direction,varname:node_4562,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:1164,x:32332,y:32418,ptovrint:False,ptlb:node_1164,ptin:_node_1164,varname:node_1164,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:4032-1205-4562;pass:END;sub:END;*/

Shader "Savannah/sav_leaves" {
    Properties {
        _node_4032 ("node_4032", 2D) = "white" {}
        _Intensity ("Intensity", Float ) = 0.2
        _Direction ("Direction", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_4032; uniform float4 _node_4032_ST;
            uniform float _Intensity;
            uniform float _Direction;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_978 = _Time + _TimeEditor;
                float node_6923 = (sin(mul(_Object2World, v.vertex).r)*(_Intensity*((1.0 - o.uv0.g)*cos(node_978.g))));
                v.vertex.xyz += (o.vertexColor.r*float3(node_6923,_Direction,node_6923));
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _node_4032_var = tex2D(_node_4032,TRANSFORM_TEX(i.uv0, _node_4032));
                clip(_node_4032_var.a - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = _node_4032_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = _node_4032_var.rgb;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_4032; uniform float4 _node_4032_ST;
            uniform float _Intensity;
            uniform float _Direction;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_978 = _Time + _TimeEditor;
                float node_6923 = (sin(mul(_Object2World, v.vertex).r)*(_Intensity*((1.0 - o.uv0.g)*cos(node_978.g))));
                v.vertex.xyz += (o.vertexColor.r*float3(node_6923,_Direction,node_6923));
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _node_4032_var = tex2D(_node_4032,TRANSFORM_TEX(i.uv0, _node_4032));
                clip(_node_4032_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = _node_4032_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_4032; uniform float4 _node_4032_ST;
            uniform float _Intensity;
            uniform float _Direction;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                float4 node_978 = _Time + _TimeEditor;
                float node_6923 = (sin(mul(_Object2World, v.vertex).r)*(_Intensity*((1.0 - o.uv0.g)*cos(node_978.g))));
                v.vertex.xyz += (o.vertexColor.r*float3(node_6923,_Direction,node_6923));
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _node_4032_var = tex2D(_node_4032,TRANSFORM_TEX(i.uv0, _node_4032));
                clip(_node_4032_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
