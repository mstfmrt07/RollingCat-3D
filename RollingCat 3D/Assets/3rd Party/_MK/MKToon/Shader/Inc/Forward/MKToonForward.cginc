// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//vertex and fragment shader
#ifndef MK_TOON_FORWARD
	#define MK_TOON_FORWARD

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputForward vertfwd (VertexInputForward v)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		VertexOutputForward o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputForward, o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		//vertex positions
		#if MKTOON_LIT
			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.pos =  UnityObjectToClipPos(v.vertex);
		#else
			o.pos =  UnityObjectToClipPos(v.vertex);
		#endif
		//texcoords
		#if MKTOON_TC && !MKTOON_TC_D
			o.uv_Main.xy = TRANSFORM_TEX(v.texcoord0, _MainTex);
		#elif !MKTOON_TC && MKTOON_TC_D
			o.uv_Main.xy = TRANSFORM_TEX(v.texcoord0, _DetailAlbedoMap);
		#elif !MKTOON_TC && !MKTOON_TC_D
			//no tc
		#else
			o.uv_Main.xy = TRANSFORM_TEX(v.texcoord0, _MainTex);
			o.uv_Main.zw = TRANSFORM_TEX(v.texcoord0, _DetailAlbedoMap);
		#endif

		#if MKTOON_LIT
			//normal binormal tangent
			#ifdef MKTOON_TBN
				//create tangent space matrix
				o.tangentWorld.xyz = normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz, 0.0)).xyz);
				o.normalWorld.xyz = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
				o.binormalWorld.xyz = normalize(cross(o.normalWorld.xyz, o.tangentWorld.xyz) * v.tangent.w);
			#else
				//if tangent space matrix not needed calculate only world normal
				o.normalWorld.xyz = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
			#endif
		#endif
		#ifdef MKTOON_VERTCLR
			o.color = v.color;
		#endif

		#if MKTOON_LIT
			#ifdef MK_TOON_FWD_BASE_PASS
				//lightmaps and ambient
				#ifdef DYNAMICLIGHTMAP_ON
					o.uv_Lm.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					o.uv_Lm.xy = 1;
				#endif
				#ifdef LIGHTMAP_ON
					o.uv_Lm.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					o.uv_Lm.zw = 1;
				#endif

				#ifdef MK_TOON_FWD_BASE_PASS
					#if UNITY_SHOULD_SAMPLE_SH
					//unity ambient light
						o.aLight = ShadeSH9 (half4(o.normalWorld.xyz,1.0));
					#else
						o.aLight = 0.0;
					#endif
					#ifdef VERTEXLIGHT_ON
						//vertexlight
						o.aLight += Shade4PointLights (
						unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
						unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
						unity_4LightAtten0, o.posWorld, 
						#ifdef MKTOON_TBN
							o.normalWorld.xyz);
						#else
							o.normalWorld.xyz);
						#endif
					#endif
				#endif
			#endif
		#endif

		#if MKTOON_LIT
			//vertex shadow
			UNITY_TRANSFER_SHADOW(o,v.texcoord0.xy); 
		#endif

		//vertex fog
		UNITY_TRANSFER_FOG(o,o.pos);

		#if MKTOON_SKETCH
			float4 uvScreen = 0;
			//use default texure mapping instead of screen uv
			uvScreen.xy = TRANSFORM_TEX(v.texcoord0, _SketchMap);

			#if MKTOON_LIT
				#if MKTOON_TBN
					o.normalWorld.w = uvScreen.x;
					o.tangentWorld.w = uvScreen.y;
					o.binormalWorld.w = uvScreen.z;
					o.posWorld.w = uvScreen.w;
				#else
					o.uv_Screen = uvScreen;
				#endif
			#else
				o.uv_Screen = uvScreen;
			#endif
		#endif
		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 fragfwd (VertexOutputForward o) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(o);

		//init surface struct for rendering
		MKToonSurface mkts = InitSurface(o);

		//apply lights, ambient and lightmap
		MKToonLightLMCombined(mkts, o);

		//finalize the output
		MKToonLightFinal(mkts, o);

		mkts.Color_Out.rgb = CSBControl(mkts.Color_Out.rgb, _Brightness, _Saturation, _Contrast);

		//if enabled add some fog - forward rendering only
		UNITY_APPLY_FOG(o.fogCoord, mkts.Color_Out);

		return mkts.Color_Out;
	}
#endif