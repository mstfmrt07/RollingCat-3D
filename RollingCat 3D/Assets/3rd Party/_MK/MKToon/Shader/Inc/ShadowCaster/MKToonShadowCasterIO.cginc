//shadow input and output
#ifndef MK_TOON_SHADOWCASTER_IO
	#define MK_TOON_SHADOWCASTER_IO

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputShadowCaster
	{
		float4 vertex : POSITION;
		//use normals for cubemapped shadows (point lights)
		//#ifndef SHADOWS_CUBE
		float3 normal : NORMAL;
		//#endif
		#ifdef MKTOON_TC
			//texcoords0 if needed
			float2 texcoord0 : TEXCOORD0;
		#endif
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputShadowCaster
	{	
		V2F_SHADOW_CASTER_NOPOS
		//float3 sv : TEXCOORD7;
		//enable texcoords for blended shadows (dither)
		#if MKTOON_TC
			float2 uv : TEXCOORD6;
		#endif
		fixed deb : TEXCOORD7;
	};

	#ifdef UNITY_STEREO_INSTANCING_ENABLED
	struct VertexOutputStereoShadowCaster
	{
		UNITY_VERTEX_OUTPUT_STEREO
	};
	#endif
#endif