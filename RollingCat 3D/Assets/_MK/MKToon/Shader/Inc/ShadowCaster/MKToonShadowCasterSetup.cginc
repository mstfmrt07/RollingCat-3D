//Shadowcaster setup
#ifndef MK_TOON_SHADOWCASTER_SETUP
	#define MK_TOON_SHADOWCASTER_SETUP

	#ifndef MK_TOON_SHADOWCASTER_PASS
		#define MK_TOON_SHADOWCASTER_PASS 1
	#endif

	#include "UnityCG.cginc"
	#include "../Common/MKToonDef.cginc"

	#ifndef MKTOON_TEXCLR
		#define MKTOON_TEXCLR 1
	#endif
	#ifndef MKTOON_TC
		#define MKTOON_TC 1
	#endif

	#if _MK_MODE_TRANSPARENT && SHADER_TARGET >= 25
		#ifndef MKTOON_DITHER_MASK
			#define MKTOON_DITHER_MASK 1
		#endif
	#endif

	//if blending enabled use uv´s
	#if _MK_MODE_CUTOUT || _MK_MODE_TRANSPARENT
		#ifndef MKTOON_SHADOW_UVS
			#define MKTOON_SHADOW_UVS 1
		#endif
	#endif

	#include "../Common/MKToonV.cginc"
	#include "../Common/MKToonInc.cginc"
	#include "../ShadowCaster/MKToonShadowCasterIO.cginc"
#endif