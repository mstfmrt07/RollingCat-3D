//Meta input and output
#ifndef MK_TOON_META_IO
	#define MK_TOON_META_IO

	#include "UnityCG.cginc"
	#include "UnityMetaPass.cginc"

	#ifndef UNITY_PASS_META
		#define UNITY_PASS_META 1
	#endif

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputMeta
	{
		#ifdef MKTOON_VERTCLR
			fixed4 color : COLOR;
		#endif
		float4 vertex	: POSITION;
		float2 uv0		: TEXCOORD0;
		float2 uv1		: TEXCOORD1;
		#if defined(DYNAMICLIGHTMAP_ON) || defined(UNITY_PASS_META)
			float2 uv2		: TEXCOORD2;
		#endif
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputMeta
	{
		float2 uv		: TEXCOORD0;
		float4 pos		: SV_POSITION;
		#ifdef MKTOON_VERTCLR
			fixed4 color : COLOR;
		#endif
	};

#endif