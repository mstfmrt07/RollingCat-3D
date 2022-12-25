//Light Calculations
#ifndef MK_TOON_LIGHT
	#define MK_TOON_LIGHT

	#include "../Common/MKToonDef.cginc"

	////////////
	// LIGHT
	////////////
	void MKToonLightMain(inout MKToonSurface mkts, in VertexOutputForward o)
	{
		#if MKTOON_LIT
			//Base light calculation
			fixed baseLightCalc;

			#if _MK_LIGHTMODEL_MINNAERT
				baseLightCalc = GetMinnaert(mkts.Pcp.NdotL, mkts.Pcp.VdotN, _Roughness*T_D);
			#elif _MK_LIGHTMODEL_OREN_NAYER
				baseLightCalc = GetOrenNayer(mkts.Pcp.NdotL, mkts.Pcp.VdotN, mkts.Pcp.VdotL, T_D - _Roughness);
			#else
				baseLightCalc = mkts.Pcp.NdotL;
			#endif


			#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
				mkts.Color_Diffuse = LevelLighting(baseLightCalc, _LightCuts, _LightSmoothness, _LightThreshold);
			#elif _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
				mkts.Color_Diffuse = TreshHoldLighting(_LightThreshold, _LightSmoothness, baseLightCalc);
			#elif _MK_LIGHTTYPE_RAMP
				mkts.Color_Diffuse = tex2D (_Ramp, float2(baseLightCalc, baseLightCalc)).rgb;
			#else
				mkts.Color_Diffuse = baseLightCalc;
			#endif

			mkts.Color_Diffuse *= mkts.Pcp.LightAttenuation;

			//Custom shadow
			#if MKTOON_SKETCH
				#ifdef USING_DIRECTIONAL_LIGHT
					mkts.Sketch_Color_T = smoothstep(mkts.Sketch_Color - T_H, mkts.Sketch_Color, clamp(mkts.Color_Diffuse, _SketchToneMin, _SketchToneMax));
					mkts.Color_Diffuse = mkts.Sketch_Color_T;
				#else
					mkts.Color_Diffuse *= (1.0 - mkts.Sketch_Color);
					mkts.Sketch_Color_T = lerp(0.0, smoothstep(mkts.Sketch_Color - T_H, mkts.Sketch_Color, clamp(mkts.Color_Diffuse, _SketchToneMin, _SketchToneMax)), mkts.Color_Diffuse);
					mkts.Color_Diffuse = mkts.Sketch_Color_T;
				#endif
			#endif

			#ifdef USING_DIRECTIONAL_LIGHT
				_ShadowColor.rgb = lerp(_HighlightColor * mkts.Pcp.LightColor, _ShadowColor, _ShadowIntensity);
				mkts.Color_Diffuse = lerp(_ShadowColor, _HighlightColor * mkts.Pcp.LightColor, mkts.Color_Diffuse);
			#else
				_ShadowColor.rgb = 0;
				//_ShadowColor.rgb = lerp(_HighlightColor * mkts.Pcp.LightColor, _ShadowColor, _ShadowIntensity);
				mkts.Color_Diffuse = lerp(_ShadowColor, _HighlightColor * mkts.Pcp.LightColor, mkts.Color_Diffuse);
			#endif

			fixed4 c;
			//Diffuse light
			c.rgb = mkts.Color_Albedo * mkts.Color_Diffuse;

			//Specular
			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				//halv vector or viewDir
				half spec;
				_Shininess *= mkts.Color_Specular.g;

				#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
					//_Shininess *= (_LightCuts / LL_MAX);
				#endif

				#if _MK_ANISOTROPIC_SPECULAR && _MK_LIGHTMODEL_BLINN_PHONG
						spec = GetSpecularAniso(mkts.Pcp.NormalDirection, mkts.Pcp.HV, mkts.Pcp.NdotHV, _Shininess, _AnisoOffset, fixed4(mkts.Pcp.AnisoDirection, mkts.Color_Specular.b), mkts.Pcp.NdotL);
					#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
						mkts.Color_Specular = LevelLighting(spec, _LightCuts, _LightSmoothness, _LightThreshold);
					#elif _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
						mkts.Color_Specular = TreshHoldLighting(_LightThreshold, _LightSmoothness, spec);
					#elif _MK_LIGHTTYPE_RAMP
						mkts.Color_Specular = spec * tex2D(_Ramp, float2(spec ,spec)).rgb;
					#endif
				#else
					#if _MK_LIGHTMODEL_BLINN_PHONG
						spec = GetSpecular(mkts.Pcp.NdotHV, _Shininess, mkts.Pcp.NdotL);
					#elif _MK_LIGHTMODEL_PHONG
						spec = GetSpecular(_Shininess, mkts.Pcp.NdotL, mkts.Pcp.MLrefNdotV);
					#endif
					#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
						mkts.Color_Specular = LevelLighting(spec, _LightCuts, _LightSmoothness, _LightThreshold);
					#elif _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
						mkts.Color_Specular = TreshHoldLighting(_LightThreshold, _LightSmoothness, spec);
					#elif _MK_LIGHTTYPE_RAMP
						mkts.Color_Specular = spec * tex2D(_Ramp, float2(spec, spec)).rgb;
					#else
						mkts.Color_Specular = spec;
					#endif
				#endif
					mkts.Color_Specular = mkts.Color_Specular * _SpecColor * (_SpecularIntensity *  mkts.Color_Specular.r);
					#if MKTOON_SKETCH
						#ifdef USING_DIRECTIONAL_LIGHT
							mkts.Color_Specular *= mkts.Sketch_Color_T;
						#else
							mkts.Color_Specular = lerp(mkts.Color_Specular, mkts.Color_Specular * mkts.Sketch_Color, _SketchToneMax);
						#endif
					#endif
			#endif

			//handle translucency
			#if MKTOON_TLD || MKTOON_TLM
				half translucent;
				_TranslucentShininess *= mkts.Color_Translucent.g;
				translucent = GetTranslucency(mkts.Pcp.MLdotV, mkts.Pcp.NormalDirection, _TranslucentShininess);
			#ifdef USING_DIRECTIONAL_LIGHT
				_TranslucentIntensity *= T_H;
			#endif

			#if _MK_LIGHTTYPE_CEL_SHADE_MULTI
				translucent = LevelLighting(translucent, _LightCuts, _LightSmoothness, _LightThreshold);
			#elif _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
				translucent = TreshHoldLighting(_LightThreshold, _LightSmoothness, translucent);
			#elif _MK_LIGHTTYPE_RAMP
				translucent = tex2D(_Ramp, float2(translucent, translucent)).rgb;
			#endif

			mkts.Color_Translucent = (translucent * _TranslucentColor * mkts.Pcp.LightColorXAttenuation * T_D * (_TranslucentIntensity * mkts.Color_Translucent.r));
			#if MKTOON_SKETCH
				#ifdef USING_DIRECTIONAL_LIGHT
					mkts.Color_Translucent *= mkts.Sketch_Color_T;
				#else
					mkts.Color_Translucent = lerp(mkts.Color_Translucent, mkts.Color_Translucent * mkts.Sketch_Color, _SketchToneMax);
				#endif
			#endif
		#endif

		#if _MKTOON_REFLECTIVE
			//reflection map based lightintensity
			#if _MK_REFLECTIVE_MAP
				_ReflectIntensity *= tex2D(_ReflectMap, o.uv_Main).r;
			#endif
			#if MKTOON_SKETCH
				#ifdef USING_DIRECTIONAL_LIGHT
					mkts.Color_Reflect = lerp(mkts.Color_Reflect*mkts.Sketch_Color_T, mkts.Color_Reflect, _ReflectIntensity);
				#else
					mkts.Color_Reflect = lerp(lerp(mkts.Color_Reflect, mkts.Color_Reflect * mkts.Sketch_Color, _SketchToneMax), mkts.Color_Reflect, _ReflectIntensity);
				#endif
			#endif
			//basic reflection
			c.rgb = lerp(c.rgb, mkts.Color_Reflect, _ReflectIntensity);
		#endif

			#if MKTOON_TLD || MKTOON_TLM
				//apply translucency
				c.rgb += mkts.Color_Translucent;
			#endif

			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				//apply specular
				c.rgb += mkts.Color_Specular * mkts.Pcp.LightColorXAttenuation;
			#endif

			//add occlusion
			#if MKTOON_OCCLUSION
				c.rgb *= mkts.Occlusion;
			#endif

			//apply alpha
			c.a = mkts.Alpha;

			mkts.Color_Out = c;
		#else
			//correct blending on unlit && fwd add pass

			#if MKTOON_SKETCH
				mkts.Color_Albedo *= smoothstep(mkts.Sketch_Color - T_H, mkts.Sketch_Color, clamp(1.0, 0.0, _SketchToneMax));
			#endif

			//TODO skip albedo calculation
			#if MK_TOON_FWD_ADD_PASS
				mkts.Color_Albedo.rgb = 0.0;
			#endif
			//non lit color output
			mkts.Color_Out = fixed4(mkts.Color_Albedo.rgb, mkts.Color_Out.a);
		#endif
	}

	void MKToonLightLMCombined(inout MKToonSurface mkts, in VertexOutputForward o)
	{
		//apply lighting to surface
		MKToonLightMain(mkts, o);

		#if MKTOON_LIT
			#ifdef MK_TOON_FWD_BASE_PASS
				//add ambient light
				fixed3 amb = mkts.Color_Albedo * o.aLight;
				#if _MKTOON_REFLECTIVE
					mkts.Color_Out.rgb = lerp(mkts.Color_Out.rgb + amb, mkts.Color_Out.rgb + amb * T_H, _ReflectIntensity);
				#else
					mkts.Color_Out.rgb += amb;
				#endif
			#endif

			#ifdef MK_TOON_FWD_BASE_PASS
				#if LIGHTMAP_ON || DYNAMICLIGHTMAP_ON
					half3 lm = 0;
					#ifdef LIGHTMAP_ON
						 fixed4 lmBCT = UNITY_SAMPLE_TEX2D(unity_Lightmap, o.uv_Lm.xy);
						 fixed3 bC = DecodeLightmap(lmBCT);
						 //handle directional lightmaps
						#if DIRLIGHTMAP_COMBINED
							// directional lightmaps
							fixed4 bDT = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, o.uv_Lm.xy);
							lm = DecodeDirectionalLightmap (bC, bDT, o.normalWorld);

							#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
								lm = SubtractMainLightWithRealtimeAttenuationFromLightmap (lm, mkts.Pcp.LightAttenuation, lmBCT, o.normalWorld);
							#endif
						//handle not directional lightmaps
						#else
							lm = bC;
							#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
								lm = SubtractMainLightWithRealtimeAttenuationFromLightmap(lm, mkts.Pcp.LightAttenuation, lmBCT, o.normalWorld);
							#endif
						#endif
					#endif

					//handle dynamic lightmaps
					#ifdef DYNAMICLIGHTMAP_ON
						fixed4 lmRTCT = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, o.uv_Lm.zw);
						half3 rTC = DecodeRealtimeLightmap (lmRTCT);

						#ifdef DIRLIGHTMAP_COMBINED
							half4 rDT = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, o.uv_Lm.zw);
							lm += DecodeDirectionalLightmap (rTC, rDT, o.normalWorld);
						#else
							lm += rTC;
						#endif
					#endif

					//add occlusion to lightmap
					#if MKTOON_OCCLUSION
						lm *= mkts.Occlusion;
					#endif

					//apply lightmap
					mkts.Color_Out.rgb += mkts.Color_Albedo * lm;
				#endif
			#endif
		#endif
	}

	void MKToonLightFinal(inout MKToonSurface mkts, in VertexOutputForward o)
	{
		#if MKTOON_LIT
			//apply emission
			#if _MKTOON_EMISSION
				#if _MKTOON_RIM
					//apply rim lighting
					mkts.Color_Emission += RimDefault(_RimSize, mkts.Pcp.VdotN, _RimColor.rgb, _RimIntensity, _RimSmoothness);
				#endif
				#if MKTOON_SKETCH
					#ifdef USING_DIRECTIONAL_LIGHT
						mkts.Color_Emission *= mkts.Sketch_Color_T;
					#else
						mkts.Color_Emission = lerp(mkts.Color_Emission, mkts.Color_Emission * mkts.Sketch_Color, _SketchToneMax);
					#endif
				#endif
				mkts.Color_Out.rgb += mkts.Color_Emission;
			#endif
		#else
			//return unlit color
			mkts.Color_Out = fixed4(mkts.Color_Out.rgb, mkts.Color_Out.a);
		#endif
		#if _MK_DISSOLVE_RAMP
			//apply color for dissolving
			mkts.Color_Out.rgb = DissolveRamp(mkts.Pcp.Sg, _DissolveRamp, _DissolveColor*T_D, _DissolveRampSize, _DissolveAmount, o.uv_Main, mkts.Color_Out.rgb);
		#endif
	}
#endif