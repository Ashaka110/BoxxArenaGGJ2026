Shader "Unlit/Body"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GradientDistStart;
            float _GradientDistEnd;
            float4 _GradStartColor;
            float4 _GradEndColor;
            float _DamageAnim;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 c = _Color;
                
                float3 N = normalize(i.normal);
                float3 L =normalize( _WorldSpaceCameraPos-i.worldPos) ;
                float3 diffuseLight = (dot(N,L)*.5 +.6) *  c.xyz;
                
                
                return lerp( float4(diffuseLight,0), float4(1,0,0,0), _DamageAnim);
                float3 camPos = _WorldSpaceCameraPos;
                float d = log( length(i.worldPos - camPos));
                float l = (d-_GradientDistStart)/(_GradientDistEnd-_GradientDistStart);
                
                float g = .05f;
                if (abs(i.worldPos.x)%1 < g || abs(i.worldPos.z)%1 < g ||  abs(i.worldPos.y)%1 < g)
                {
                    return 0;
                }
                return lerp(_GradStartColor,_GradEndColor, l);
                
            }
            ENDCG
        }
    }
}
