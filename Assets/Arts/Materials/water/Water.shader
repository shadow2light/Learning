Shader "Unlit/Water"
{
    Properties
    {
        _Albedo("Albedo", Color)=(1,1,1, 0.5)
        [HideInInspector] _IoR("Index of Refraction", Float)=1.4
        _Distortion("Refraction Distortion", Float)=0.5
        _Gloss("Gloss", Range(1, 255))=20
        _WaveLen1("Wave Length", Float) = 1
        _WaveSpeed1("Wave Speed", Float) = 1
        _WaveAmplitude1("Wave Amplitude", Float) = 1
        _WaveDirection1("Wave Direction", Vector) = (0, 1, 0, 0)
        _WaveLen2("Wave Length", Float) = 1
        _WaveSpeed2("Wave Speed", Float) = 1
        _WaveAmplitude2("Wave Amplitude", Float) = 1
        _WaveDirection2("Wave Direction", Vector) = (0, 1, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="AlphaTest+51"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                half4 vertex : POSITION;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                half3 normalWS : TEXCOORD0;
                half2 posSS : TEXCOORD1; //Screen Space
                half4 posWS:TEXCOORD2;
            };

            half4 _Albedo;
            half _IoR;
            half _Distortion;
            half _Gloss;
            half _WaveLen1;
            half _WaveSpeed1;
            half _WaveAmplitude1;
            half3 _WaveDirection1;
            half _WaveLen2;
            half _WaveSpeed2;
            half _WaveAmplitude2;
            half3 _WaveDirection2;
            //sampler2D _CameraOpaqueTexture;
            SAMPLER(_CameraOpaqueTexture);

            half calculate_height(half len, half speed, half amp, half3 dir, half3 pos)
            {
                // return amp * sin(dot(dir.xy, pos.xz) * 2 / len);
                return amp * sin(dot(dir.xy, pos.xz) * 2 / len + _Time * speed * 2 / len);
            }

            half calculate_dx(half len, half speed, half amp, half3 dir, half3 pos)
            {
                // return 2 / len * dir.x * amp * cos(dot(dir.xy, pos.xz) * 2 / len);
                return 2 / len * dir.x * amp * cos(dot(dir.xy, pos.xz) * 2 / len + _Time * speed * 2 / len);
            }

            half calculate_dy(half len, half speed, half amp, half3 dir, half3 pos)
            {
                // return 2 / len * dir.y * amp * cos(dot(dir.xy, pos.xz) * 2 / len);
                return 2 / len * dir.y * amp * cos(dot(dir.xy, pos.xz) * 2 / len + _Time * speed * 2 / len);
            }

            v2f vert(appdata v)
            {
                //计算顶点高度
                half3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                positionWS.y += calculate_height(_WaveLen1, _WaveSpeed1, _WaveAmplitude1, _WaveDirection1, positionWS) +
                    calculate_height(_WaveLen2, _WaveSpeed2, _WaveAmplitude2, _WaveDirection2, positionWS);

                //计算法线
                half dx = calculate_dx(_WaveLen1, _WaveSpeed1, _WaveAmplitude1, _WaveDirection1, positionWS) +
                    calculate_dx(_WaveLen2, _WaveSpeed2, _WaveAmplitude2, _WaveDirection2, positionWS);
                half3 binormal = half3(1, 0, dx);
                half dy = calculate_dy(_WaveLen1, _WaveSpeed1, _WaveAmplitude1, _WaveDirection1, positionWS) +
                    calculate_dy(_WaveLen2, _WaveSpeed2, _WaveAmplitude2, _WaveDirection2, positionWS);
                half3 tangent = half3(0, 1, dy);
                half3 normalWS = normalize(half3(-binormal.z, 1, -tangent.z));

                v2f o;
                o.vertex = TransformWorldToHClip(positionWS);
                o.normalWS = normalWS;
                half2 posSS=ComputeScreenPos(o.vertex/o.vertex.w).xy;
                o.posSS = posSS; // _ScreenParams.xy
                o.posWS = half4(positionWS, 1);

                return o;
            }

            //镜面反射，菲涅尔反射，折射
            //根据深度获取本体色，暂时来不及做了，下个版本研究一下
            //泡沫效果，下个版本
            half4 frag(v2f i) : SV_Target
            {
                Light light = GetMainLight();

                //specular
                const half3 viewDir = normalize(GetCameraPositionWS() - i.posWS);
                half3 h = normalize(light.direction + viewDir);
                half4 specular = half4(half3(1, 1, 1) * pow(0.92 * max(0, dot(i.normalWS, h)), _Gloss), 1);

                //fresnel
                half fresnelStrength = dot(i.normalWS, viewDir);
                fresnelStrength = pow(1 - max(0, fresnelStrength), 10);
                half4 fresnel = half4(fresnelStrength, fresnelStrength, fresnelStrength, 1);

                //refraction
                half distortion=pow(1-dot(viewDir, i.normalWS),_Distortion)/_IoR;
                i.posSS+=half2(distortion, distortion);
                half4 refraction = tex2D(_CameraOpaqueTexture, i.posSS);

                //albedo
                //half4 depth=LinearEyeDepth(tex2D(_CameraDepthTexture, i.posSS.xy));

                half4 color = specular + fresnel + refraction; 
                color = color * _Albedo;
                return color;
            }
            ENDHLSL
        }
    }
}