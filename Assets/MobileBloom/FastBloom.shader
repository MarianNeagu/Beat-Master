Shader "SupGames/FastBloom"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
	UNITY_DECLARE_SCREENSPACE_TEXTURE(_BloomTex);
	fixed4 _MainTex_TexelSize;
	uniform fixed4 _BloomColor;
	uniform fixed _BloomDiffuse;
	uniform fixed4 _BloomData;

	struct appdata {
		fixed4 pos : POSITION;
		fixed2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2fb {
		fixed4 pos : SV_POSITION;
		fixed4 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct v2f {
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2fb vertBlur(appdata i)
	{
		v2fb o;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_INITIALIZE_OUTPUT(v2fb, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = UnityObjectToClipPos(i.pos);
		fixed2 offset = _MainTex_TexelSize.xy * _BloomDiffuse;
		o.uv = fixed4(i.uv - offset, i.uv + offset);
		return o;
	}

	v2f vert(appdata i)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = UnityObjectToClipPos(i.pos);
		o.uv = i.uv;
		return o;
	}

	fixed4 fragBloom(v2fb i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 c = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.xy);
		c += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.xw);
		c += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.zy);
		c += UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv.zw);
		return c * 0.25h;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		fixed4 c = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, UnityStereoTransformScreenSpaceTex(i.uv));
		fixed4 b = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_BloomTex, UnityStereoTransformScreenSpaceTex(i.uv));
		fixed br = max(b.r, max(b.g, b.b));
		fixed soft = clamp(br - _BloomData.y, 0.0h, _BloomData.z);
		fixed a = max(soft * soft * _BloomData.w, br - _BloomData.x) / max(br, 0.00001h) ;
		return	c + b * a * _BloomColor;
	}
	ENDCG 
		
	Subshader 
	{
		Pass //0
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }
		  CGPROGRAM
		  #pragma vertex vertBlur
		  #pragma fragment fragBloom
		  #pragma fragmentoption ARB_precision_hint_fastest
		  ENDCG
		}
		
		Pass //1
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }
		  CGPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
		  #pragma fragmentoption ARB_precision_hint_fastest
		  ENDCG
		}
	}
	Fallback off
}