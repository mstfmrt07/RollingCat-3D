// Upgrade NOTE: upgraded instancing buffer 'MK_TOON_PROPERTIES' to new syntax.

//uniform variables
#ifndef MK_TOON_V
	#define MK_TOON_V

	/////////////////////////////////////////////////////////////////////////////////////////////
	// UNIFORM VARIABLES
	/////////////////////////////////////////////////////////////////////////////////////////////

	//enabled uniform variables only if needed

	//UNITY_INSTANCING_BUFFER_START(MK_TOON_PROPERTIES)

	//UNITY_INSTANCING_BUFFER_END(MK_TOON_PROPERTIES)

	//Main
	#if _MK_MODE_CUTOUT || (MK_TOON_SHADOWCASTER && _MK_MODE_TRANSPARENT)
		half _Cutoff;
	#endif
	#ifndef UNITY_STANDARD_INPUT_INCLUDED
		#ifndef MKTOON_OUTLINE_PASS_ONLY
			uniform fixed4 _Color;
		#endif
	#endif
	#ifdef MKTOON_TEXCLR
		#ifndef UNITY_STANDARD_INPUT_INCLUDED
			#if !MKTOON_OUTLINE_PASS_ONLY
				uniform sampler2D _MainTex;
			#endif
		#endif
	#endif
	#ifdef MKTOON_TC
		#ifndef UNITY_STANDARD_INPUT_INCLUDED
			uniform float4 _MainTex_ST;
		#endif
	#endif

	//Detail
	#if _MK_DETAIL_MAP || _MK_DETAIL_BUMP_MAP
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform half _DetailTint;
		uniform fixed3 _DetailColor;
	#endif

	#if _MK_DETAIL_BUMP_MAP
		uniform half _DetailNormalMapScale;
		uniform sampler2D _DetailNormalMap;
	#endif

	//Normalmap
	#if MKTOON_LIT
		#if _MK_BUMP_MAP
			uniform sampler2D _BumpMap;
			uniform half _BumpScale;
		#endif
	#endif

	#if MKTOON_OCCLUSION
		uniform sampler2D _OcclusionMap;
		uniform half _OcclusionStrength;
	#endif

	//Light
	#if MKTOON_LIT
		#ifndef UNITY_LIGHTING_COMMON_INCLUDED
			uniform fixed4 _LightColor0;
		#endif
		#if _MK_LIGHTTYPE_RAMP
			uniform sampler2D _Ramp;
		#endif
		#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
			uniform half _LightCuts;
		#endif
		#if _MK_LIGHTTYPE_CEL_SHADE_SIMPLE || _MK_LIGHTTYPE_CEL_SHADE_MULTI
			uniform half _LightThreshold;
		#endif
	#endif

	//Render
	#if !MKTOON_OUTLINE_PASS_ONLY
		uniform half _Contrast;
		uniform half _Saturation;
		uniform half _Brightness;
	#endif
	#if MKTOON_LIT
		#if !_MK_LIGHTTYPE_RAMP || MKTOON_VD
			uniform half _LightSmoothness;
		#endif
		#if _MKTOON_REFLECTIVE
			uniform half _ReflectSmoothness;
		#endif
		#if _MKTOON_RIM && MK_TOON_FWD_BASE_PASS
			uniform half _RimSmoothness;
		#endif
		#if _MK_LIGHTMODEL_MINNAERT || _MK_LIGHTMODEL_OREN_NAYER
			uniform half _Roughness;
		#endif
	#endif

	//Custom shadow
	#if MKTOON_LIT
		uniform fixed3 _ShadowColor;
		uniform fixed3 _HighlightColor;
		uniform fixed _ShadowIntensity;
	#endif

	//Rim
	#if _MKTOON_RIM && MK_TOON_FWD_BASE_PASS
		uniform fixed3 _RimColor;
		uniform half _RimSize;
		uniform fixed _RimIntensity;
	#endif

	//Specular
	#if MKTOON_LIT
		#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
			uniform sampler2D _SpecGlossMap;
			uniform half _Shininess;
			#ifndef UNITY_LIGHTING_COMMON_INCLUDED
				uniform fixed3 _SpecColor;
			#endif
			uniform fixed _SpecularIntensity;
			#if _MK_ANISOTROPIC_SPECULAR && _MK_LIGHTMODEL_BLINN_PHONG
				uniform fixed _AnisoOffset;
				uniform sampler2D _AnisoMap;
			#endif
		#endif
	#endif

	//Reflection
	#if MKTOON_LIT
		#if _MK_REFLECTIVE_MAP
			uniform sampler2D _ReflectMap;
		#endif
		#if _MKTOON_REFLECTIVE
			uniform fixed3 _ReflectColor;
			uniform fixed _ReflectIntensity;
		#endif
	#endif

	//Dissolve
	#if _MKTOON_DISSOLVE
		uniform fixed3 _DissolveColor;
		uniform sampler2D _DissolveMap;
		#if MKTOON_OUTLINE_PASS_ONLY || MK_TOON_SHADOWCASTER_PASS
			uniform float4 _DissolveMap_ST;
		#endif
			uniform half _DissolveAmount;
		#if _MK_DISSOLVE_RAMP
			uniform sampler2D _DissolveRamp;
			uniform half _DissolveRampSize;
		#endif
	#endif

	//Translucent
	#if MKTOON_LIT
		#if MKTOON_TLD || MKTOON_TLM
			uniform fixed3 _TranslucentColor;
			uniform fixed _TranslucentIntensity;
			uniform fixed _TranslucentShininess;
		#endif
		#if MKTOON_TLM
			uniform sampler2D _TranslucentMap;
		#endif
	#endif

	//Emission
	#if MKTOON_LIT
		#if _MKTOON_EMISSION
			uniform fixed3 _EmissionColor;

			#if _MK_EMISSION_MAP
			uniform sampler2D _EmissionMap;
			#endif
		#endif
	#endif

	//Outline
	#ifdef MKTOON_OUTLINE_PASS_ONLY
		uniform fixed4 _OutlineColor;
		uniform half _OutlineSize;
	#endif

	//Sketch
	#ifdef MKTOON_SKETCH
		uniform sampler2D _SketchMap;
		uniform float4 _SketchMap_ST;
		uniform half _SketchScale;
		uniform half _SketchToneMin;
		uniform half _SketchToneMax;
	#endif

	//Other
	#if MK_TOON_SHADOWCASTER_PASS && MKTOON_DITHER_MASK
		sampler3D _DitherMaskLOD;
	#endif
#endif