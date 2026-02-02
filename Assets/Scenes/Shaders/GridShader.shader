Shader "Unlit/GridShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GradientDistStart ("Gradient Distance Start", float) = 1
        _GradientDistEnd ("Gradient Distance End", float) = 10
        _GradStartColor("StartColor", Color) = (1,1,1,1)
        _GradEndColor("EndColor", Color) = (1,1,1,1)
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GradientDistStart;
            float _GradientDistEnd;
            float4 _GradStartColor;
            float4 _GradEndColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 camPos = _WorldSpaceCameraPos;
                float d = log( length(i.worldPos - camPos));
                float l = (d-_GradientDistStart)/(_GradientDistEnd-_GradientDistStart);
                
                float g = .05f;
                float s = 1.5f;
                if ((abs(i.worldPos.x)+g*.5)%s < g || (abs(i.worldPos.z)+g*.5)%s < g ||  (abs(i.worldPos.y)+g*.5)%s < g)
                {
                    return 0;
                return lerp(_GradStartColor,_GradEndColor, 1-l);
                }
                return lerp(_GradStartColor,_GradEndColor, l);
                
            }
            ENDCG
        }
    }
}
