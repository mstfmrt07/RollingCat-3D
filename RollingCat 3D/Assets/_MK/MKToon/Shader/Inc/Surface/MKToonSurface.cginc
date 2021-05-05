//surface calculations
#ifndef MK_TOON_SURFACE
	#define MK_TOON_SURFACE

	#if MKTOON_PRECALC
	void PreCalcParameters(inout MKToonSurface mkts)
	{
		#if MKTOON_V_DOT_N
			mkts.Pcp.VdotN = max(0.0 , dot(mkts.Pcp.ViewDirection, mkts.Pcp.NormalDirection));
		#endif
		#if MKTOON_V_DOT_L
			mkts.Pcp.VdotL = max(0.0 ,dot(mkts.Pcp.LightDirection, mkts.Pcp.ViewDirection));
		#endif
		#if MKTOON_N_DOT_L
			#if _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
				mkts.Pcp.NdotL = max(0.0 , dot(mkts.Pcp.NormalDirection, mkts.Pcp.LightDirection)*T_V + T_V);
			#elif _MK_LIGHTTYPE_CEL_SHADE_MULTI || _MK_LIGHTTYPE_RAMP
				mkts.Pcp.NdotL = max(0.0 , dot(mkts.Pcp.NormalDirection, mkts.Pcp.LightDirection));
			#else
				mkts.Pcp.NdotL = dot(mkts.Pcp.NormalDirection, mkts.Pcp.LightDirection);
				mkts.Pcp.NdotL = max(0.0, lerp(mkts.Pcp.NdotL / T_V - T_V, mkts.Pcp.NdotL, _LightSmoothness));
			#endif
		#endif
		#if MKTOON_HV
			mkts.Pcp.HV = normalize(mkts.Pcp.LightDirection + mkts.Pcp.ViewDirection);
		#endif
		#if MKTOON_N_DOT_HV
			mkts.Pcp.NdotHV = max(0.0, dot(mkts.Pcp.NormalDirection, mkts.Pcp.HV));
		#endif
		#if MKTOON_ML_REF_N
			mkts.Pcp.MLrefN = reflect(-mkts.Pcp.LightDirection, mkts.Pcp.NormalDirection);
		#endif
		#ifdef MKTOON_MV_REF_N
			mkts.Pcp.MVrefN = reflect(-mkts.Pcp.ViewDirection, mkts.Pcp.NormalDirection);
		#endif
		#if MKTOON_ML_DOT_V
			 mkts.Pcp.MLdotV = max(0.0, dot (-mkts.Pcp.LightDirection, mkts.Pcp.ViewDirection));
		#endif
		#if MKTOON_ML_REF_N_DOT_V
			 mkts.Pcp.MLrefNdotV = max(0.0, dot(mkts.Pcp.MLrefN, mkts.Pcp.ViewDirection));
		#endif
	}
	#endif

	//get surface color based on blendmode and color source
	#if MKTOON_TEXCLR
		void SurfaceColor(out fixed3 albedo, out fixed alpha, float2 uv)
		{
			#if _MK_MODE_CUTOUT || _MK_MODE_TRANSPARENT
				fixed4 c = tex2D(_MainTex, uv) * _Color;
				albedo = c.rgb;
				alpha = c.a;
			#else
				albedo = tex2D(_MainTex, uv) * _Color;
				alpha = 1.0;
			#endif
		}
	#elif MKTOON_VERTCLR
		void SurfaceColor(out fixed3 albedo, out fixed alpha, fixed4 vrtColor)
		{
			#if _MK_MODE_CUTOUT || _MK_MODE_TRANSPARENT
				fixed4 c = vrtColor * _Color;
				albedo = c.rgb;
				alpha = c.a;
			#else
				albedo = vrtColor * _Color;
				alpha = 1.0;
			#endif
		}
	#endif

	//only include initsurface when not meta pass
	#ifndef MK_TOON_META_PASS
		/////////////////////////////////////////////////////////////////////////////////////////////
		// INITIALIZE SURFACE
		/////////////////////////////////////////////////////////////////////////////////////////////
		MKToonSurface InitSurface(
			#if defined(MK_TOON_FWD_BASE_PASS) || defined(MK_TOON_FWD_ADD_PASS)
				in VertexOutputForward o
			#endif
		)
		{
			//Init Surface
			MKToonSurface mkts;
			UNITY_INITIALIZE_OUTPUT(MKToonSurface,mkts);

			//manage dissolve and skip fragment
			#if _MKTOON_DISSOLVE
				mkts.Pcp.Sg =  tex2D (_DissolveMap, o.uv_Main).r - _DissolveAmount;
				Clip0(mkts.Pcp.Sg);
			#endif

			#if MKTOON_TC_D
				#if !MKTOON_TC
					mkts.Pcp.UvDetail = o.uv_Main.xy;
				#else
					mkts.Pcp.UvDetail = o.uv_Main.zw;
				#endif
			#endif

			#if MKTOON_LIT
				//modified brightness based on light type
				#if _MK_LIGHTTYPE_CEL_SHADE_SIMPLE
					_Brightness -= T_H;
					#if !_MK_LIGHTMODEL_MINNAERT && !_MK_LIGHTMODEL_OREN_NAYER
						_LightThreshold = _LightThreshold * T_V + T_V;
					#endif
				#elif _MK_LIGHTTYPE_CEL_SHADE_MULTI
					//_LightThreshold = _LightThreshold * T_V + T_V;
				#endif
			#endif

			//reconstruct screen uv
			#if MKTOON_SKETCH
				#if MKTOON_LIT
					#if MKTOON_TBN
						mkts.UV_Screen.x = o.normalWorld.w;
						mkts.UV_Screen.y = o.tangentWorld.w;
						mkts.UV_Screen.z = o.binormalWorld.w;
						mkts.UV_Screen.w = o.posWorld.w;
					#else
						mkts.UV_Screen = o.uv_Screen;
					#endif
				#else
					mkts.UV_Screen = o.uv_Screen;
				#endif
				//mkts.UV_Screen.xy = o.pos.xy / mkts.UV_Screen.z;
				mkts.UV_Screen.xy *= (_SketchScale);
				mkts.Sketch_Color = 1.0 - tex2D(_SketchMap, mkts.UV_Screen.xy).rgb;
			#endif

			//init surface color
			#if MKTOON_TEXCLR
				SurfaceColor(mkts.Color_Albedo, mkts.Alpha, o.uv_Main);
			#elif MKTOON_VERTCLR
				SurfaceColor(mkts.Color_Albedo, mkts.Alpha, o.color);
			#endif

			//add detail
			#if _MK_DETAIL_MAP
				mkts.Color_Albedo = lerp(mkts.Color_Albedo, mkts.Color_Albedo * (tex2D(_DetailAlbedoMap, mkts.Pcp.UvDetail).rgb * _DetailColor), _DetailTint);
			#endif

			//Unpack Occlusion
			#if MKTOON_OCCLUSION
				half oms = 1.0 - _OcclusionStrength;
				mkts.Occlusion = oms + tex2D(_OcclusionMap, o.uv_Main.xy).g * _OcclusionStrength;
			#endif

			//skip fragment if cutoff enabled
			#if _MK_MODE_CUTOUT
				if(mkts.Alpha < _Cutoff) discard;
			//apply alpha if transparent blendmode
			#elif _MK_MODE_TRANSPARENT
				mkts.Color_Out.a = mkts.Alpha;
			#endif

			#if MKTOON_LIT
				//get normal direction
				#if MKTOON_TBN
					//Normalmap extraction
					#if _MK_BUMP_MAP
						mkts.Pcp.NormalDirection = WorldNormal(_BumpMap, o.uv_Main.xy, _BumpScale, half3x3(o.tangentWorld.xyz, o.binormalWorld.xyz, o.normalWorld.xyz));
					#endif
					#if _MK_DETAIL_BUMP_MAP
						mkts.Pcp.DetailNormalDirection = WorldNormal(_DetailNormalMap, mkts.Pcp.UvDetail, _DetailNormalMapScale, half3x3(o.tangentWorld.xyz, o.binormalWorld.xyz, o.normalWorld.xyz));
					#endif

					#if _MK_BUMP_MAP && _MK_DETAIL_BUMP_MAP
						mkts.Pcp.NormalDirection = lerp(mkts.Pcp.NormalDirection, mkts.Pcp.DetailNormalDirection, 0.5);
					#elif !_MK_BUMP_MAP && _MK_DETAIL_BUMP_MAP
						mkts.Pcp.NormalDirection = mkts.Pcp.DetailNormalDirection;
					#elif _MK_BUMP_MAP && !_MK_DETAIL_BUMP_MAP
						//already done
					#else
						mkts.Pcp.NormalDirection = normalize(o.normalWorld.xyz);
					#endif
				#else
					//basic normal input
					mkts.Pcp.NormalDirection = normalize(o.normalWorld.xyz);
				#endif
			#endif

			#if MKTOON_LIT
				#if MKTOON_VD
					//view direction
					mkts.Pcp.ViewDirection = normalize(_WorldSpaceCameraPos - o.posWorld).xyz;
				#endif

				//lightdirection and attenuation
				#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
					mkts.Pcp.LightDirection  = normalize(_WorldSpaceLightPos0.xyz - o.posWorld.xyz);
				#else
					mkts.Pcp.LightDirection = normalize(_WorldSpaceLightPos0.xyz);
				#endif

				UNITY_LIGHT_ATTENUATION(atten, o, o.posWorld.xyz);
				mkts.Pcp.LightAttenuation = atten;

				mkts.Pcp.LightColor = _LightColor0.rgb;
				mkts.Pcp.LightColorXAttenuation = mkts.Pcp.LightColor * mkts.Pcp.LightAttenuation;

				//init precalc
				#if MKTOON_PRECALC
					PreCalcParameters(mkts);
				#endif
			#endif

			#if _MK_ANISOTROPIC_SPECULAR && _MK_LIGHTMODEL_BLINN_PHONG
				//aniso specular direction
				mkts.Pcp.AnisoDirection = UnpackAniso(_AnisoMap, o.uv_Main);
			#endif

			#if MKTOON_LIT
				#if MKTOON_TLM
					//get translucent intensity using map
					mkts.Color_Translucent = tex2D(_TranslucentMap, o.uv_Main).r;
				#elif MKTOON_TLD
					mkts.Color_Translucent = 1.0;
				#endif

				#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
					// get specular gloss, intensity, aniso value using map
					#if _MK_SPECULAR_MAP
						mkts.Color_Specular = tex2D(_SpecGlossMap, o.uv_Main).rgb;
					#else
						mkts.Color_Specular = 1.0;
					#endif
				#endif

				#if _MKTOON_REFLECTIVE
					//get reflection color
					mkts.Color_Reflect = GetReflection(mkts.Pcp.MVrefN, o.posWorld.xyz) * _ReflectColor.rgb;
				#endif

				//apply emission color using a map
				#if MKTOON_LIT
					#if _MKTOON_EMISSION
						#if _MK_EMISSION_DEFAULT
							mkts.Color_Emission = _EmissionColor;
						#elif _MK_EMISSION_MAP
							mkts.Color_Emission = _EmissionColor * tex2D(_EmissionMap, o.uv_Main).rgb;
						#endif
					#endif
				#endif
			#endif
			return mkts;
		}
	#endif
#endif