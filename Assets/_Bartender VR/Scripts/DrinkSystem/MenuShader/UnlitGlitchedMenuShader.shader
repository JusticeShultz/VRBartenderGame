Shader "Rays Custom/UnlitGlitchedMenuShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorPixel ("Color Pixel", 2D) = "white" {}
		_PixelScale("Pixel Scale", Float) = 1.0
		_Saturation("Saturation", Range(0.0,1.0)) = 0.0
	
		_GlitchMap("Glitch Map", 2D) = "white"{}
		_GlitchIntensity("Glitch Intesity", Range(-1.0,1.0)) = 0.5
	}
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
            };

            sampler2D _MainTex;
			float4 _MainTex_ST;


			sampler2D _ColorPixel;

			float _PixelScale;
			float _Saturation;

			sampler2D _GlitchMap;
			float _GlitchIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = {0,0,0,1};
				fixed4 colorCache;	
				
				float glitchValue = tex2D(_GlitchMap, i.uv).r;

				glitchValue = (glitchValue - 0.5f) * 2 * _GlitchIntensity;

				fixed4 main = tex2D(_MainTex, float2(i.uv.x + glitchValue, i.uv.y));

				col.a = 0;

				colorCache = tex2D(_ColorPixel, i.uv * _PixelScale);

				col = colorCache * main;
				
				col.rgb += _Saturation;
				return col;
            }
            ENDCG
        }
    }
}
