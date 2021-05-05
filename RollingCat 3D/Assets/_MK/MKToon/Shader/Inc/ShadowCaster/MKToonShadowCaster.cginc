//shadow rendering input and output
#ifndef MK_TOON_SHADOWCASTER
	#define MK_TOON_SHADOWCASTER

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	void vertShadowCaster (
		 VertexInputShadowCaster v,
		 out VertexOutputShadowCaster o
		 #ifdef UNITY_STEREO_INSTANCING_ENABLED
			,out VertexOutputStereoShadowCaster os
		 #endif
		 ,out float4 pos : SV_POSITION
		)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(VertexOutputShadowCaster, o);
		#ifdef UNITY_STEREO_INSTANCING_ENABLED
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(os);
		#endif

		#if defined(MKTOON_TC)
			//texcoords using untiy macro
			o.uv = TRANSFORM_TEX(v.texcoord0, _MainTex);
		#endif
		/*
		#ifdef SHADOWS_CUBE //point light shadows
			pos = UnityObjectToClipPos(v.vertex);
			o.sv = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
		#else //other shadows
			//pos with unity macros
			pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);
			pos = UnityApplyLinearShadowBias(pos);
		#endif
		*/
		TRANSFER_SHADOW_CASTER_NOPOS(o, pos)
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 fragShadowCaster 
		(
			VertexOutputShadowCaster o
			#if UNITY_VERSION >= 20171
				,UNITY_POSITION(vpos)
			#else
				,UNITY_VPOS_TYPE vpos : VPOS
			#endif
		) : SV_Target
	{	
		#if _MKTOON_DISSOLVE
			fixed3 sg =  tex2D (_DissolveMap, o.uv).r - _DissolveAmount;
			Clip0(sg);
		#endif
		#if defined(MKTOON_SHADOW_UVS)
			half alpha = tex2D(_MainTex, o.uv).a * _Color.a;
			#if defined(_MK_MODE_CUTOUT)
				//cutout shadow alpha
				clip (alpha - _Cutoff);
			#endif
			#if _MK_MODE_TRANSPARENT
				#if MKTOON_DITHER_MASK
					// dither mask alpha blending
					half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
					clip (alphaRef - 0.01);
				#else
					clip (alpha - 1.0);
				#endif
			#endif
		#endif

		/*
		#ifdef SHADOWS_CUBE
			return UnityEncodeCubeShadowDepth ((length(o.sv) + unity_LightShadowBias.x) * _LightPositionRange.w);
		#else
			return 0;
		#endif*/
		SHADOW_CASTER_FRAGMENT(o)
	}			
#endif