Shader "SupGames/FastBloomUrp"
{
	Properties
	{
		[HideInInspector]_MainTex("Base (RGB)", 2D) = "white" {}
	}
	HLSLINCLUDE

	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

	TEXTURE2D_X(_MainTex);
	SAMPLER(sampler_MainTex);
	TEXTURE2D_X(_BlurTex);
	SAMPLER(sampler_BlurTex);
	half4 _MainTex_TexelSize;
	uniform half4 _BloomColor;
	uniform half _BloomDiffuse;
	uniform half4 _BloomData;

	struct appdata {
		half4 pos : POSITION;
		half2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2fb {
		half4 pos : SV_POSITION;
		half4 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv  : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f vert(appdata i)
	{
		v2f o = (v2f)0;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_TRANSFER_INSTANCE_ID(i, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, half4(i.pos.xyz, 1.0h)));
		o.uv = i.uv;
		return o;
	}

	v2fb vertBloom(appdata i)
	{
		v2fb o = (v2fb)0;
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_TRANSFER_INSTANCE_ID(i, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, half4(i.pos.xyz, 1.0h)));
		half2 offset = _MainTex_TexelSize.xy * _BloomDiffuse;
		o.uv = half4(i.uv - offset, i.uv + offset);
		return o;
	}

	half4 fragBloom(v2fb i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		half4 c = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.xy));
		c += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.xw));
		c += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.zy));
		c += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.zw));
		return c * 0.25h;;
	}

	half4 frag(v2f i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		half4 c = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv));
		half4 b = SAMPLE_TEXTURE2D_X(_BlurTex, sampler_BlurTex, UnityStereoTransformScreenSpaceTex(i.uv));
		half br = max(b.r, max(b.g, b.b));
		half soft = clamp(br - _BloomData.y, 0.0h, _BloomData.z);
		half a = max(soft * soft * _BloomData.w, br - _BloomData.x) / max(br, 0.00001h);
		return	c + b * a * _BloomColor;
	}

	ENDHLSL

	Subshader
	{
		Pass //0
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }
		  HLSLPROGRAM
		  #pragma vertex vertBloom
		  #pragma fragment fragBloom
		  ENDHLSL
		}

		Pass //1
		{
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }
		  HLSLPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
		  ENDHLSL
		}
	}
	Fallback off
}