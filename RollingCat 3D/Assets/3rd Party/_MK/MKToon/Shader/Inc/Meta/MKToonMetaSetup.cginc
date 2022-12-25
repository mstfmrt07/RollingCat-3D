//Meta setup
#ifndef MK_TOON_META_SETUP
	#define MK_TOON_META_SETUP

	#ifndef _EMISSION
		#define _EMISSION 1
	#endif

	#ifndef MK_TOON_META_PASS
		#define MK_TOON_META_PASS 1
	#endif

	#include "UnityCG.cginc"
	#include "../Common/MKToonDef.cginc"
	#ifndef MKTOON_TC
		#define MKTOON_TC 1
	#endif
	#include "../Common/MKToonV.cginc"
	#include "../Common/MKToonInc.cginc"
	#include "../Surface/MKToonSurfaceIO.cginc"
	#include "../Surface/MKToonSurface.cginc"
	#include "MKToonMetaIO.cginc"
#endif