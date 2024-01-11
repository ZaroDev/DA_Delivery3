Shader "Unlit/HeatMapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Opacity("Opacity", Range(0, 1)) = 0.25
        
        _Color0 ("Color 0", Color) = (0,0,0,0)
        _Color1 ("Color 1", Color) = (0, .9, .2, 1)
        _Color2 ("Color 2", Color) = (.9, 1, .3, 1)
        _Color3 ("Color 3", Color) = (.9, .7, .1, 1)
        _Color4 ("Color 4", Color) = (1, 0, 0, 1)

        _Range0 ("Range 0", Range(0, 1)) = 0
        _Range1 ("Range 1", Range(0, 1)) = 0.25
        _Range2 ("Range 2", Range(0, 1)) = 0.5
        _Range3 ("Range 3", Range(0, 1)) = 0.75
        _Range4 ("Range 4", Range(0, 1)) = 1

        _Diameter("Diameter", Range(0, 1)) = 1.0
        _Strength("Strenght", Range(.1, 4)) = 1.0

    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float4 colors[5];
            float pointRanges[5];


            
            float _Grid[3 * 32];
            int _Count = 0;

            float4 _Color0;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            float _Range0;
            float _Range1;
            float _Range2;
            float _Range3;
            float _Range4;
            float _Diameter;
            float _Strength;

            float _Opacity;
            
            void init()
            {
                colors[0] = _Color0;
                colors[1] = _Color1;
                colors[2] = _Color2;
                colors[3] = _Color3;
                colors[4] = _Color4;

                pointRanges[0] = _Range0;
                pointRanges[1] = _Range1;
                pointRanges[2] = _Range2;
                pointRanges[3] = _Range3;
                pointRanges[4] = _Range4;
            }

            float distSq(float2 a, float2 b)
            {
                return pow(max(0.0, 1.0 - distance(a, b) / _Diameter), 2);
            }

            float3 getHeatForPixel(float weight)
            {
                if (weight <= pointRanges[0])
                {
                    return colors[0];
                }
                if (weight >= pointRanges[4])
                {
                    return colors[4];
                }

                for (int i = 1; i < 5; i++)
                {
                    if (weight < pointRanges[i])
                    {
                        const float distFromLowerPoint = weight - pointRanges[i - 1];
                        const float sizeOfPointRange = pointRanges[i] - pointRanges[i - 1];

                        const float ratioOverLowerPoint = distFromLowerPoint / sizeOfPointRange;

                        const float3 colorRange = colors[i] - colors[i - 1];
                        const float3 colorContribution = colorRange * ratioOverLowerPoint;

                        return colors[i - 1] + colorContribution;
                    }
                }
                return colors[0];
            }

            fixed4 frag(v2f i) : SV_Target
            {
                init();

                fixed4 col = tex2D(_MainTex, i.uv);

                // Change uv coordinate range to -2 - 2
                float2 uv = i.uv;
                uv = uv * 4.0 - float2(2.0, 2.0);

                float totalWeight = 0;
                for (int it = 0; it < _Count; it++)
                {
                    const float2 workPt = float2(_Grid[it * 3], _Grid[it * 3 + 1]) * _Grid[it * 3 + 2];
                    const float ptIntensity = _Grid[it * 3 + 2];

                    totalWeight += 0.5 * distSq(uv, workPt) * ptIntensity * _Strength;
                }

                const float3 heat = getHeatForPixel(totalWeight);
                return float4(heat + col.xyz, 0);
            }
            ENDCG
        }
    }
}