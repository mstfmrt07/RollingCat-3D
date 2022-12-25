//meta vertex and fragment shader
#ifndef MK_TOON_META
#define MK_TOON_META

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputMeta metavert (VertexInputMeta v)
	{
		VertexOutputMeta o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputMeta, o);
		//vertexposition
		o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
		//texcoords
		o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
		#ifdef MKTOON_VERTCLR
			o.color = v.color;
		#endif
		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 metafrag (VertexOutputMeta o) : SV_Target
	{
		UnityMetaInput umi;
		fixed alpha;
		UNITY_INITIALIZE_OUTPUT(UnityMetaInput, umi);

		//modified meta albedo
		#if MKTOON_TEXCLR
			SurfaceColor(umi.Albedo, alpha, o.uv);
		#elif MKTOON_VERTCLR
			SurfaceColor(umi.Albedo, alpha, o.color);
		#endif
		umi.Albedo = CSBControl(umi.Albedo, _Brightness, _Saturation, _Contrast);

		//apply emission color
		#if MKTOON_LIT
			#if _MKTOON_EMISSION
				#if _MK_EMISSION_DEFAULT
					umi.Emission = _EmissionColor * umi.Albedo;
				#elif _MK_EMISSION_MAP
					umi.Emission = _EmissionColor * tex2D(_EmissionMap, o.uv).rgb * umi.Albedo;
				#endif
			#endif
		#endif

		//unity meta macro to apply gi
		return UnityMetaFragment(umi);
	}

#endif