//Basic definitions for the rendering
#ifndef MK_TOON_DEF
	#define MK_TOON_DEF

	/////////////////////////////////////////////////////////////////////////////////////////////
	// DEF
	/////////////////////////////////////////////////////////////////////////////////////////////
	#ifndef T_H
		#define T_H 0.25
	#endif
	#ifndef T_V
		#define T_V 0.5
	#endif
	#ifndef T_O
		#define T_O 1.0
	#endif
	#ifndef T_G
		#define T_G 1.75
	#endif
	#ifndef T_D
		#define T_D 2.0
	#endif
	#ifndef T_DD
		#define T_DD 4.0
	#endif
	#ifndef T_T
		#define T_T 10.0
	#endif

	#ifndef LL_MAX
		#define LL_MAX 6.0
	#endif

	#ifndef SHINE_MULT
		#define SHINE_MULT 64
	#endif

	#ifndef PI
		#define PI 3.14
	#endif

	//Lit
	#if _MK_LIGHTMODEL_LAMBERT || _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_MINNAERT || _MK_LIGHTMODEL_OREN_NAYER
		#ifndef MKTOON_LIT
			#define MKTOON_LIT 1
		#endif
	#else
		#ifndef MKTOON_UNLIT
			#define MKTOON_UNLIT 1
		#endif
	#endif

	//Dissolve
	#if _MK_DISSOLVE_DEFAULT || _MK_DISSOLVE_RAMP
		#ifndef _MKTOON_DISSOLVE
			#define _MKTOON_DISSOLVE 1
		#endif
	#endif

	//Rim
	#if MKTOON_LIT && MK_TOON_FWD_BASE_PASS
		#if _MK_RIM
			#ifndef _MKTOON_RIM
				#define _MKTOON_RIM 1
			#endif
		#endif
	#endif

	//Emission
	#if MKTOON_LIT
		#if (_MK_EMISSION_DEFAULT || _MK_EMISSION_MAP || _MKTOON_RIM) && (MK_TOON_META_PASS || MK_TOON_FWD_BASE_PASS)
			#ifndef _MKTOON_EMISSION
				#define _MKTOON_EMISSION 1
			#endif
		#endif
	#endif

	//Reflective
	#if (_MK_REFLECTIVE_MAP || _MK_REFLECTIVE_DEFAULT) && MK_TOON_FWD_BASE_PASS && MKTOON_LIT
		#ifndef _MKTOON_REFLECTIVE
			#define _MKTOON_REFLECTIVE 1
		#endif
	#endif

	//Translucent
	#if _MK_TRANSLUCENT_DEFAULT && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)
		#ifndef MKTOON_TLD
			#define MKTOON_TLD 1
		#endif
	#elif _MK_TRANSLUCENT_MAP && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)
		#ifndef MKTOON_TLM
			#define MKTOON_TLM 1
		#endif
	#endif

	//Color src
	#if _MK_ALBEDO_MAP || _MK_DETAIL_MAP
		#define MKTOON_TEXCLR 1
	#else
		#define MKTOON_VERTCLR 1
	#endif

	//Occlusion
	#if MKTOON_LIT
		#if _MK_OCCLUSION
			#ifndef MKTOON_OCCLUSION
				#define MKTOON_OCCLUSION 1
			#endif
		#endif
	#endif

	#if _MK_SKETCH
		#ifndef MKTOON_SKETCH
			#define MKTOON_SKETCH 1
		#endif
	#endif

	//Texcoords
	#if MKTOON_LIT || MKTOON_TEXCLR || (_MK_SPECULAR_MAP && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)) || _MK_BUMP_MAP || _MK_DETAIL_BUMP_MAP || _MKTOON_DISSOLVE || MKTOON_TLD || MKTOON_TLM ||_MK_ANISOTROPIC_SPECULAR || _MK_REFLECTIVE_MAP || _MK_EMISSION_MAP || _MK_ALBEDO_MAP || MKTOON_OCCLUSION || _MK_DETAIL_MAP || MKTOON_SKETCH
		#ifndef MKTOON_TC
			#define MKTOON_TC 1
		#endif
	#endif

	//Texcoord detail
	#if _MK_DETAIL_MAP || _MK_DETAIL_BUMP_MAP
		#ifndef MKTOON_TC_D
			#define MKTOON_TC_D 1
		#endif
	#endif

	//Normals
	#if _MK_BUMP_MAP || _MK_DETAIL_BUMP_MAP || DIRLIGHTMAP_COMBINED || UNITY_SHOULD_SAMPLE_SH 
		#ifndef	MKTOON_TBN	
			#define MKTOON_TBN 1
		#endif
	#else
		#ifndef MKTOON_WN
			#define MKTOON_WN 1
		#endif
	#endif

	//VD
	#if DIRLIGHTMAP_COMBINED || _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG || (_MKTOON_RIM && MK_TOON_FWD_BASE_PASS) || _MKTOON_REFLECTIVE || MKTOON_TLM || _MK_LIGHTMODEL_MINNAERT || _MK_LIGHTMODEL_OREN_NAYER
		#ifndef MKTOON_VD
			#define MKTOON_VD 1
		#endif
	#endif

	//Pcp enable
	#if MKTOON_LIT
		#if _MK_LIGHTMODEL_MINNAERT || _MK_LIGHTMODEL_OREN_NAYER || (_MKTOON_RIM && MK_TOON_FWD_BASE_PASS)
			#ifndef MKTOON_V_DOT_N
				#define MKTOON_V_DOT_N 1
			#endif
		#endif
		#if _MK_LIGHTMODEL_OREN_NAYER
			#ifndef MKTOON_V_DOT_L
				#define MKTOON_V_DOT_L 1
			#endif
		#endif
		#if MKTOON_LIT
			#ifndef MKTOON_N_DOT_L
				#define MKTOON_N_DOT_L 1
			#endif
		#endif
		#if _MK_LIGHTMODEL_BLINN_PHONG
			#ifndef MKTOON_HV
				#define MKTOON_HV 1
			#endif
			#ifndef MKTOON_N_DOT_HV
				#define MKTOON_N_DOT_HV 1 
			#endif
		#endif
		#if _MK_LIGHTMODEL_PHONG
			#ifndef MKTOON_ML_REF_N
				#define MKTOON_ML_REF_N 1
			#endif
		#endif
		#if _MKTOON_REFLECTIVE
			#ifndef MKTOON_MV_REF_N
				#define MKTOON_MV_REF_N 1
			#endif
		#endif
		#if MKTOON_TLD || MKTOON_TLM
			#ifndef MKTOON_ML_DOT_V
				#define MKTOON_ML_DOT_V 1
			#endif
		#endif
		#if _MK_LIGHTMODEL_PHONG
			#ifndef MKTOON_ML_REF_N_DOT_V
				#define MKTOON_ML_REF_N_DOT_V 1
			#endif
		#endif
	#endif

	#if (MKTOON_ML_REF_N_DOT_V || MKTOON_ML_DOT_V || MKTOON_MV_REF_N || MKTOON_ML_REF_N || MKTOON_N_DOT_HV || MKTOON_HV || MKTOON_N_DOT_L || MKTOON_V_DOT_L || MKTOON_V_DOT_N) || MKTOON_LIT || _MKTOON_DISSOLVE || MKTOON_TC_D
		#ifndef MKTOON_PRECALC
			#define MKTOON_PRECALC 1
		#endif
	#endif
#endif