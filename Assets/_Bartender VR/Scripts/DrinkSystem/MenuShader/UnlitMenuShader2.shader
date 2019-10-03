Shader "Rays Custom/UnlitMenuShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _RedPixel ("Red Pixel", 2D) = "white" {}
        _GreenPixel ("Green Pixel", 2D) = "white" {}
        _BluePixel ("Blue Pixel", 2D) = "white" {}
	
		_PixelScale("Pixel Scale", Float) = 1.0
		//_BaseBrightness
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


			sampler2D _RedPixel;
			sampler2D _GreenPixel;
			sampler2D _BluePixel;



			float _PixelScale;

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
				
				if (i.uv.x < 1.0f / _PixelScale / 3.0f / _PixelScale)
				{
					col.r = 1;
				}
				else if (i.uv.x > 2.0f / 3.0f)
				{
					col.b = 1;
				}
				else
				{
					col.g = 1;
				}

				if(i.uv.x )

				//col.r =  1.0f - floor((i.uv.x * 3));
				//col.r = 0.0f + ( floor((i.uv.x * 3.0f)) / 3.0f );
			

				
				//col.r = floor((i.uv.x - (2.0f / 3.0f))) + 1.0f;
				//col.g = floor((i.uv.x - (1.0f / 3.0f))) + 1.0f;
				//col.b = floor((i.uv.x - (2.0f / 3.0f))) + 1.0f;
				


				//col.r = floor( 0.6f )  ;


				/*
				fixed4 col = {0,0,0,1};
				fixed4 colorCache;

				fixed4 test = tex2D(_MainTex, i.uv);

				col.a = 0;

				colorCache = tex2D(_ColorPixel, i.uv * _PixelScale);

				col = colorCache *  test;
				*/
				return col;
            }
            ENDCG
        }
    }
}
