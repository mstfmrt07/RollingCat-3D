//include file for important calculations during rendering
#ifndef MK_TOON_INC
	#define MK_TOON_INC

	#include "../Common/MKToonDef.cginc"

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INC
	/////////////////////////////////////////////////////////////////////////////////////////////
	//normal in world space
	#ifdef MKTOON_TBN
	inline half3 WorldNormal(sampler2D normalMap, float2 uv, half bumpiness, half3x3 tbn)
	{
		half4 encode = tex2D(normalMap, uv);
		#if defined(UNITY_NO_DXT5nm)
			half3 local = encode.rgb * 2.0 - 1.0;
		#else
			half3 local = half3(2.0 * encode.a - 1.0, 2.0 * encode.g - 1.0, 0.0);
		#endif
		local.xy *= bumpiness;
		//local.z = sqrt(1.0 - dot(local, local));
		#if !defined(UNITY_NO_DXT5nm)
			local.z = 1.0 - 0.5 * dot(local.xy, local.xy); //approximation
		#endif
		return normalize(mul(local, tbn));
	}
	#endif

	//Minnaert lighting
	inline half GetMinnaert(half ndl, half vdn, half roughness)
	{
		return saturate(ndl * pow(ndl*vdn, roughness));
	}

	//OrenNayer lighting
	inline half GetOrenNayer(half ndl, half vdn, half ldv, half roughness)
	{
		half srf = ldv - ndl * vdn;
		half roughnessSquare = roughness * roughness;
		return ndl*ndl * ((1.0 + roughnessSquare * (ndl / (roughnessSquare + 0.13) + 0.5 / (roughnessSquare + 0.33))) + (0.45 * roughnessSquare / (roughnessSquare + 0.09)) * srf / lerp(1.0, max(ndl+0.001, vdn), step(0.0, srf))) / PI;
	}

	//aniso direction
	inline half3 UnpackAniso(sampler2D anisoMap, float2 uv)
	{
		half3 unpackedAniso = half3(tex2D(anisoMap, uv).wy * 2.0 - 1.0, 0);
		unpackedAniso.z = sqrt(1.0 - saturate(dot(unpackedAniso, unpackedAniso)));
		return unpackedAniso;
	}

	#if _MK_LIGHTMODEL_BLINN_PHONG
		//aniso specular blinn phong
		inline half GetSpecularAniso(half3 normal, half3 halfV, half ndhv, half shine, half offset, half4 aDir, half ndl)
		{
			return (ndl > 0.0) ?  pow(lerp(ndhv, max(0.0, sin(radians((dot(normalize(normal + aDir.rgb), halfV) + offset) * 180.0))), aDir.a), shine * SHINE_MULT) : 0.0;
		}
		//specular blinn phong
		inline half GetSpecular(half ndhv, half shine, half ndl)
		{
			return (ndl > 0.0) ? pow(ndhv, shine * SHINE_MULT) : 0.0;
		}
	#elif _MK_LIGHTMODEL_PHONG
		//specular phong
		inline half GetSpecular(half shine, half ndl, half mlrefndotv)
		{
			return (ndl > 0.0) ? pow(mlrefndotv, shine * SHINE_MULT) : 0.0;
		}
	#endif

	//translucency
	inline half GetTranslucency(half mldotv, half3 normal, half shine)
	{
		return pow (mldotv, shine * SHINE_MULT);
	}

	//level based lighting type
	inline half LevelLighting(half brightness, half levels, half smoothness, half lThreshold)
	{
		half levelStep = 1.0 / levels;
		half light = 0;
		for(int i = 0; i < levels; i++)
		{
			light += smoothstep(levelStep*i+lThreshold-smoothness*T_H, levelStep*i+lThreshold+smoothness*T_H, brightness);
		}
		return light*levelStep;
	}

	//threshold based lighting type
	inline half TreshHoldLighting(half lThreshold, half smoothness, half v)
	{
		return smoothstep(lThreshold-smoothness*T_H, lThreshold+smoothness*T_H, v);
	}

	//Rim with smooth interpolation
	inline half3 RimDefault(half size, half3 vdn, fixed3 col, fixed intensity, half smoothness)
	{
		fixed r = pow ((1.0 - saturate(vdn)), size);
		r = smoothstep(r - smoothness, r + smoothness, vdn);
		return (1.0-r) * intensity * col.rgb;
	}

	//Rampcolor when dissolving
	inline half3 DissolveRamp(fixed dissolveValue, sampler2D dissolveRampTex, fixed3 dissolveColor, half dissolveRampSize, half dissolveAmount, half2 uv, fixed3 baseCol)
	{
		half sv = dissolveRampSize * dissolveAmount;
		return lerp(baseCol, dissolveColor * tex2D(dissolveRampTex, half2(dissolveValue * (1.0/sv), 0)), step(dissolveValue, sv));
	}

	//modified clip function for a complete discard when input equal zero oder smaller
	inline void Clip0(half c)
	{
		//if(any(c < 0.0)) discard;
		clip(c);
	}

	//Contrast - Saturation - Brightness
	inline fixed3 CSBControl( fixed3 color, half brightness, half saturation, half contrast)
	{
		/*
		fixed3 luminace = fixed3(0.2126,0.7152,0.0722);
		fixed3 bc = color * brightness;
		fixed i = dot(bc, luminace);
		return lerp(fixed3(0.5, 0.5, 0.5), lerp(fixed3(i, i, i), bc, saturation), contrast);
		*/
		fixed3 luminace = fixed3(0.2126,0.7152,0.0722);
		fixed3 bc = color * brightness;
		fixed i = dot(bc, luminace);
		#if MK_TOON_FWD_ADD_PASS
			return lerp(fixed3(0.0, 0.0, 0.0), lerp(fixed3(i, i, i), bc, saturation), contrast);
		#else
			return lerp(fixed3(0.5, 0.5, 0.5), lerp(fixed3(i, i, i), bc, saturation), contrast);
		#endif
	}

	#if _MKTOON_REFLECTIVE
		//get environmentcube
		//based on unity implementation
		inline float3 BoxProjection (
			float3 dir, float3 posW,
			float4 cubePos, float3 boxMin, float3 boxMax
			) 
		{
			#if UNITY_SPECCUBE_BOX_PROJECTION
				UNITY_BRANCH
				if (cubePos.w > 0) 
				{
					float3 factors = ((dir > 0 ? boxMax : boxMin) - posW) / dir;
					float scalar = min(min(factors.x, factors.y), factors.z);
					dir = dir * scalar + (posW - cubePos);
				}
			#endif
			return dir;
		}

		inline half3 GetReflection(half3 reflectionDir, half3 worldPos)
		{
			Unity_GlossyEnvironmentData environment;
			environment.roughness = _ReflectSmoothness;
			environment.reflUVW = BoxProjection(
				reflectionDir, worldPos,
				unity_SpecCube0_ProbePosition,
				unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax
			);
			half3 probe0 = Unity_GlossyEnvironment(
				UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, environment
			);
			environment.reflUVW = BoxProjection(
				reflectionDir, worldPos,
				unity_SpecCube1_ProbePosition,
				unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax
			);
			#if UNITY_SPECCUBE_BLENDING
				UNITY_BRANCH
				if (unity_SpecCube0_BoxMin.w < 0.99999) 
				{
					half3 probe1 = Unity_GlossyEnvironment(
						UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0),
						unity_SpecCube0_HDR, environment
					);
					probe0 = lerp(probe1, probe0, unity_SpecCube0_BoxMin.w);
				}
			#endif
			return probe0;
		}
	#endif
#endif