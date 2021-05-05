//surface input and output
#ifndef MK_TOON_SURFACE_IO
	#define MK_TOON_SURFACE_IO

	/////////////////////////////////////////////////////////////////////////////////////////////
	// MKTOON SURFACE
	/////////////////////////////////////////////////////////////////////////////////////////////

	//Dynamic precalc struct
	#if MKTOON_PRECALC
	struct MKToonPCP
	{
		#if MKTOON_V_DOT_N
			half VdotN;
		#endif
		#if MKTOON_V_DOT_L
			half VdotL;
		#endif
		#if MKTOON_N_DOT_L
			half NdotL;
		#endif
		#if MKTOON_HV
			half3 HV;
		#endif
		#if MKTOON_N_DOT_HV
			half NdotHV;
		#endif
		#if MKTOON_ML_REF_N
			half3 MLrefN;
		#endif
		#if MKTOON_ML_DOT_V
			half MLdotV;
		#endif
		#if MKTOON_ML_REF_N_DOT_V
			half MLrefNdotV;
		#endif

		#if MKTOON_LIT
			half3 LightDirection;
			half3 LightColor;
			half LightAttenuation;
			half3 LightColorXAttenuation;
			half3 NormalDirection;
			#if _MK_DETAIL_BUMP_MAP
				half3 DetailNormalDirection;
			#endif
			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				#if _MK_ANISOTROPIC_SPECULAR && _MK_LIGHTMODEL_BLINN_PHONG
					half3 AnisoDirection;
				#endif
			#endif
			#if MKTOON_MV_REF_N
				half3 MVrefN;
			#endif
			#if MKTOON_VD
				half3 ViewDirection;
			#endif
		#endif
		#if _MKTOON_DISSOLVE
			half Sg;
		#endif
		#if MKTOON_TC_D
			float2 UvDetail;
		#endif
	};
	#endif

	//dynamic surface struct
	struct MKToonSurface
	{
		#if MKTOON_PRECALC
			MKToonPCP Pcp;
		#endif
		#if MKTOON_SKETCH
			float4 UV_Screen;
			fixed3 Sketch_Color_T;
			fixed3 Sketch_Color;
		#endif
		fixed4 Color_Out;
		fixed3 Color_Albedo;
		#if MKTOON_OCCLUSION
			fixed Occlusion;
		#endif
		fixed Alpha;
		#if MKTOON_LIT
			#if _MKTOON_EMISSION
				half3 Color_Emission;
			#endif
			fixed3 Color_Diffuse;
			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				fixed3 Color_Specular;
			#endif
			#if MKTOON_TLD || MKTOON_TLM
				fixed3 Color_Translucent;
			#endif
			#if _MKTOON_REFLECTIVE
				fixed3 Color_Reflect;
			#endif
		#endif
	};
#endif