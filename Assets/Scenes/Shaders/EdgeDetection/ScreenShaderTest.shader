Shader "Unlit/ScreenShaderTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RewSprite ("Texture", 2D) = "white" {}
        _PauseSprite ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Speed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _RewSprite;
            sampler2D _PauseSprite;
            float4 _RewSprite_ST;
            float4 _MainTex_ST;
            float _Speed;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal( v.normals);
                o.uv = v.uv;
                o.uv2 = TRANSFORM_TEX(v.uv, _RewSprite);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }

            
            fixed4 frag (Interpolators i) : SV_Target
            {
                
                // sample the texture
                fixed4 rewCol;
                if(_Speed == 0)
                {
                    rewCol = tex2D(_PauseSprite, i.uv2 * 5 );
                }else {
                    rewCol = tex2D(_RewSprite, i.uv2 * 5 )* step(frac(_Time.y*.4), .4 );
                }
                
                //return rewCol;

                float2 sample = i.uv;
                float speed = sin(_Time.y * .5) * (_Speed+.1f);

                sample.x += .002 * sin(100 * sample.y + _Time.y * speed * 5) * _Speed;
                //sample.x += .1 * sin(_Time.y * sample.x);
                
                fixed4 col = tex2D(_MainTex, sample);

                float noise = random(i.uv + _Time.y * _Speed) * .3;

                
                return col * .5 + noise+ rewCol *.5;
                
                //return float4(sample,0 , 1);//_Color;//float4(1,0,0,1);//col;
            }
            ENDCG
        }
    }
}
