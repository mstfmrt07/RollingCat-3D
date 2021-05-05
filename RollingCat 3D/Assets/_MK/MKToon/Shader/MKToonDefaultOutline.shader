Shader "Hidden/MK/Toon/Default Outline"
{
	Properties
	{
		//Main
		[Enum(BlendMode)] _Mode ("BlendMode", int) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Culling", int) = 2
		_Blend0 ("Blend mode 0", int) = 1
		_Blend1 ("Blend mode 1", int) = 0
		[Enum(ZWrite)] _ZWrite ("Z-Buffer", int) = 1.0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Color (RGB)", 2D) = "white" {}
		[Toggle] _AlbedoMap ("Color source map", int) = 0

		//Detail
		_DetailTint("Detail Tint", Range (0, 1)) = 0.5
		_DetailAlbedoMap("Detail Albedo", 2D) = "white" {}
		_DetailNormalMapScale("Detail Normal Map Scale", Float) = 1.0
		_DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
		_DetailColor ("Detail Color", Color) = (1,1,1,1)

		//Normalmap
		[Toggle] _UseBumpMap ("NormalDirection Map", int) = 0
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpScale("Scale", Float) = 1

		//Light
		[Enum(LightType)] _LightType ("Type", int) = 1
		[Enum(LightModel)] _LightModel ("Model", int) = 1
		_Ramp ("Ramp (RGB)", 2D) = "white" {}
		_LightCuts("LightCuts", Range (1.0, 6)) = 3
		_LightThreshold("LightThreshold", Range (0.01, 1)) = 0.3
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		//Render
		_LightSmoothness ("Light Smoothness", Range(0,1)) = 0
		_RimSmoothness ("Rim Smoothness", Range(0,1)) = 0.5
		_ReflectSmoothness ("Reflect Smoothness", Range(0,1)) = 0
		_Contrast ("Contrast", Range(0.0,2.0)) = 1.0
		_Saturation ("Saturation", Range(0,2)) = 1.0
		_Brightness("Brightness", Range (0.5, 1.5)) = 1
		_Roughness("Roughness", Range (0.0, 2.0)) = 1.0

		//Custom shadow
		_ShadowColor ("Shadow Color", Color) = (00,0.0,0.0,1.0)
		_HighlightColor ("Highlight Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ShadowIntensity("Shadow Intensity", Range (0.0, 2.0)) = 1.0

		//Outline
		[Toggle]_UseOutline ("Outline", int) = 0
		_OutlineColor ("Outline Color", Color) = (0,0,0,1.0)
		_OutlineSize ("Outline Size", Float) = 0.02

		//Rim
		[Toggle] _UseRim ("Rim", int) = 0
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimSize ("Rim Size", Range(0.0,3.0)) = 1.5
		_RimIntensity("Intensity", Range (0, 1)) = 0.5

		//Specular
		_Shininess ("Shininess",  Range (0.01, 1)) = 0.275
		_SpecColor ("Specular Color", Color) = (1,1,1,0.5)
		_SpecGlossMap("Spec (R) Gloss (G) Aniso (B)", 2D) = "white" {}
		_SpecularIntensity("Intensity", Range (0, 1)) = 0.5
		[Toggle] _UseAnisotropicSpecular ("Anisotropic", int) = 0
		_AnisoMap ("Anisotropic Direction (NormalDirection)", 2D) = "bump" {}
		_AnisoOffset ("Anisotropic Highlight Offset", Range(-1,1)) = -0.2
		
		//Reflection
		[Toggle] _UseReflection ("Reflection", int) = 0
		_ReflectColor ("Reflection Color", Color) = (1, 1, 1, 0.5)
		_ReflectIntensity("Intensity", Range (0, 1)) = 0.5
		_ReflectMap ("Reflectivity (R)", 2D) = "white" {}

		//Dissolve 
		[Toggle] _UseDissolve ("Dissolve", int) = 0
	    _DissolveMap ("Dissolve (R)", 2D) = "white" {}
		_DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.5
		_DissolveRampSize ("Dissolve Ramp Size", Range(0.0, 1.0)) = 0.25
		_DissolveRamp ("Ramp (RGB)", 2D) = "white" {}
		_DissolveColor ("Dissolve Color", Color) = (1, 1, 1, 0.5)

		//Translucent
		[Toggle] _UseTranslucent ("Translucent", int) = 0
		_TranslucentColor ("Translucent Color", Color) = (1,1,1,0.5)
		_TranslucentMap ("Power (R)", 2D) = "white" {}
		_TranslucentIntensity("Intensity", Range (0, 1)) = 0.5
		_TranslucentShininess ("Shininess",  Range (0.01, 1)) = 0.275

		//Emission
		_EmissionColor("Emission Color", Color) = (0,0,0)
		_EmissionMap("Emission (RGB)", 2D) = "white" {}

		//Sketch
		[Toggle] _UseSketch ("Sketch", int) = 0
		_SketchMap ("Sketch (RGB)", 2D) = "white" {}
		_SketchScale("Intensity", Range (0, 32)) = 16.0
		_SketchToneMin("Sketch Tone Min", Range (0, 1)) = 0.5
		_SketchToneMax("Sketch Tone Max", Range (0, 1)) = 1.0

		//Editor
		[HideInInspector] _MKEditorShowMainBehavior ("Main Behavior", int) = 1
		[HideInInspector] _MKEditorShowDetailBehavior ("Detail Behavior", int) = 0
		[HideInInspector] _MKEditorShowLightBehavior ("Light Behavior", int) = 0
		[HideInInspector] _MKEditorShowShadowBehavior ("Shadow Behavior", int) = 0
		[HideInInspector] _MKEditorShowRenderBehavior ("Render Behavior", int) = 0
		[HideInInspector] _MKEditorShowSpecularBehavior ("Specular Behavior", int) = 0
		[HideInInspector] _MKEditorShowTranslucentBehavior ("Translucent Behavior", int) = 0
		[HideInInspector] _MKEditorShowRimBehavior ("Rim Behavior", int) = 0
		[HideInInspector] _MKEditorShowReflectionBehavior ("Reflection Behavior", int) = 0
		[HideInInspector] _MKEditorShowDissolveBehavior ("Dissolve Behavior", int) = 0
		[HideInInspector] _MKEditorShowOutlineBehavior ("Outline Behavior", int) = 0
		[HideInInspector] _MKEditorShowSketchBehavior ("Sketch Behavior", int) = 0
	}
	SubShader
	{
		LOD 300
		Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull [_CullMode]
			Blend [_Blend0] [_Blend1]
			ZWrite [_ZWrite]
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature __ _MK_BUMP_MAP
			#pragma shader_feature __ _MK_RIM
			#pragma shader_feature __ _MK_ANISOTROPIC_SPECULAR
			#pragma shader_feature __ _MK_LIGHTTYPE_CEL_SHADE_SIMPLE _MK_LIGHTTYPE_CEL_SHADE_MULTI _MK_LIGHTTYPE_RAMP
			#pragma shader_feature __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG _MK_LIGHTMODEL_MINNAERT _MK_LIGHTMODEL_OREN_NAYER
			#pragma shader_feature __ _MK_MODE_TRANSPARENT _MK_MODE_CUTOUT
			#pragma shader_feature __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_RAMP
			#pragma shader_feature __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature __ _MK_ALBEDO_MAP
			#pragma shader_feature __ _MK_SPECULAR_MAP
			#pragma shader_feature __ _MK_DETAIL_MAP
			#pragma shader_feature __ _MK_DETAIL_BUMP_MAP
			#pragma shader_feature __ _MK_OCCLUSION
			#pragma shader_feature __ _MK_SKETCH

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma multi_compile_instancing

			#include "Inc/Forward/MKToonForwardBaseSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" } 
			Name "FORWARDADD"
			Cull [_CullMode]
			Blend [_Blend0] One
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature __ _MK_BUMP_MAP
			#pragma shader_feature __ _MK_RIM
			#pragma shader_feature __ _MK_ANISOTROPIC_SPECULAR
			#pragma shader_feature __ _MK_LIGHTTYPE_CEL_SHADE_SIMPLE _MK_LIGHTTYPE_CEL_SHADE_MULTI _MK_LIGHTTYPE_RAMP
			#pragma shader_feature __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG _MK_LIGHTMODEL_MINNAERT _MK_LIGHTMODEL_OREN_NAYER
			#pragma shader_feature __ _MK_MODE_TRANSPARENT _MK_MODE_CUTOUT
			#pragma shader_feature __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_RAMP
			#pragma shader_feature __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature __ _MK_ALBEDO_MAP
			#pragma shader_feature __ _MK_SPECULAR_MAP
			#pragma shader_feature __ _MK_DETAIL_MAP
			#pragma shader_feature __ _MK_DETAIL_BUMP_MAP
			#pragma shader_feature __ _MK_OCCLUSION
			#pragma shader_feature __ _MK_SKETCH

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#include "Inc/Forward/MKToonForwardAddSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}

		//TODO deferred shading pass
		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			Fog { Mode Off }
			ZWrite On ZTest Less Cull Off
			Offset 1, 1


			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature __ _MK_MODE_TRANSPARENT _MK_MODE_CUTOUT
			#pragma shader_feature __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG _MK_LIGHTMODEL_MINNAERT _MK_LIGHTMODEL_OREN_NAYER
			#pragma shader_feature __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_RAMP
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#pragma multi_compile_instancing

			#include "Inc/ShadowCaster/MKToonShadowCasterSetup.cginc"
			#include "Inc/ShadowCaster/MKToonShadowCaster.cginc"

			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" }
			Name "META" 

			Cull Off

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex metavert
			#pragma fragment metafrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma shader_feature __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG _MK_LIGHTMODEL_MINNAERT _MK_LIGHTMODEL_OREN_NAYER
			#pragma shader_feature __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature __ _MK_ALBEDO_MAP

			#include "Inc/Meta/MKToonMetaSetup.cginc"
			#include "Inc/Meta/MKToonMeta.cginc"
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// OUTLINE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			LOD 300
			Tags {"LightMode" = "Always"}
			Name "OUTLINE_SM_3_0"
			Cull Front
			Blend [_Blend0] [_Blend1]
			ZWrite [_ZWrite]
			ZTest LEqual

			CGPROGRAM 
			#pragma target 3.0
			#pragma vertex outlinevert 
			#pragma fragment outlinefrag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature __ _MK_MODE_TRANSPARENT _MK_MODE_CUTOUT
			#pragma shader_feature __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_RAMP

			#pragma multi_compile_fog

			#pragma multi_compile_instancing

			#include "/Inc/Outline/MKToonOutlineOnlySetup.cginc"
			#include "/Inc/Outline/MKToonOutlineOnlyBase.cginc"
			ENDCG 
		}
    }
	FallBack "Hidden/MK/Toon/Mobile Outline"
	CustomEditor "MK.Toon.MKToonEditor"
}
