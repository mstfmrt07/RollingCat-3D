//Vertexshader Input and Output
#ifndef MK_TOON_IO_FORWARD
	#define MK_TOON_IO_FORWARD

	#include "UnityCG.cginc"
	#include "AutoLight.cginc"

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputForward
	{
		#if MKTOON_VERTCLR
			//use vertexcolors if enabled
			fixed4 color : COLOR;
		#endif
		//vertex position - always needed
		float4 vertex : POSITION;
		#if MKTOON_TC || MKTOON_TC_D
			//texcoords0 if needed
			float4 texcoord0 : TEXCOORD0;
		#endif
		#if MK_TOON_FWD_BASE_PASS
			//ambient and lightmap0 texcoords
			#if UNITY_SHOULD_SAMPLE_SH || LIGHTMAP_ON
				float4 texcoord1 : TEXCOORD1;
			#endif
			#if DYNAMICLIGHTMAP_ON
				//dynammic lightmap texcoords
				float4 texcoord2 : TEXCOORD2;
			#endif
		#endif
		#if MKTOON_LIT
			//use normals light is enabled
			half3 normal : NORMAL;
			//use tangents for special matrix calculation
			#if MKTOON_TBN || MKTOON_WN
				half4 tangent : TANGENT;
			#endif
		#endif
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputForward
	{
		float4 pos : SV_POSITION;
		#if MKTOON_TC && !MKTOON_TC_D
			float2 uv_Main : TEXCOORD0;
		#elif !MKTOON_TC && MKTOON_TC_D
			float2 uv_Main : TEXCOORD0;
		#elif !MKTOON_TC && !MKTOON_TC_D
			//no tc
		#else
			float4 uv_Main : TEXCOORD0;
		#endif

		#ifdef MKTOON_VERTCLR
			fixed4 color : COLOR0;
		#endif
		#if MKTOON_LIT
			#if MKTOON_SKETCH && MKTOON_TBN
				float4 posWorld : TEXCOORD1;
			#else
				float3 posWorld : TEXCOORD1;
			#endif
			#ifdef MKTOON_TBN
				#if MKTOON_SKETCH
					float4 normalWorld : TEXCOORD2;
					float4 tangentWorld : TEXCOORD3;
					float4 binormalWorld : TEXCOORD4;
				#else
					half4 normalWorld : TEXCOORD2;
					half4 tangentWorld : TEXCOORD3;
					half4 binormalWorld : TEXCOORD4;
				#endif
			#else
				#if MKTOON_SKETCH
					half3 normalWorld : TEXCOORD2;
					float4 uv_Screen : TEXCOORD3;
				#else
					half3 normalWorld : TEXCOORD2;
				#endif
			#endif
		#else
			#if MKTOON_SKETCH
				float4 uv_Screen : TEXCOORD3;
			#endif
		#endif

		#if MKTOON_LIT
			#ifdef MK_TOON_FWD_BASE_PASS
				fixed3 aLight : COLOR1;
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
					float4 uv_Lm : TEXCOORD5;
				#endif
				
			#endif
			UNITY_SHADOW_COORDS(6)
		#endif

		UNITY_FOG_COORDS(7)

		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};
#endif