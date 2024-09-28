Shader "Unlit/TestWater1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Distortion("Distortion Amt", Float) = 1
        _NoiseScale("Noise Scale", Float) = 1
        _NoiseScrollVelocity("Noise Scroll Velocity", Float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
                sampler2D _NoiseTex;
                float _Distortion; // Declaring the variables properly
                float _NoiseScale;
                float _NoiseScrollVelocity;
            CBUFFER_END

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // https://gamedev.stackexchange.com/questions/163689/how-to-animate-abstract-2d-top-down-water-texture as basis
                float2 waveUV = i.uv * _NoiseScale;             // Use a chosen scale value to generate our wave UV coords and distance
                float2 travel = _NoiseScrollVelocity * _Time.x;

                float2 uv = i.uv; // Take our initial UV coordinates...
                uv += _Distortion * (tex2D(_NoiseTex, waveUV + travel).rg - 0.5f);  // Add what is essentially a random noise modifier
                //waveUV += 0.2f;                                                     // Force an offset between the two samples
                //uv += _Distortion * (tex2D(_NoiseTex, waveUV - travel * .6f).rg - 0.5f);  // Add it again

                // sample the main texture, using our funky UV coords
                fixed4 col = tex2D(_MainTex, uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
