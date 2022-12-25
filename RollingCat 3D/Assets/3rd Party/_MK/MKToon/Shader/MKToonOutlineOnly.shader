//Shared outline shader for rendering
Shader "Hidden/MK/Toon/OutlineOnly" 
{
	Properties
	{
		//Main
		[KeywordEnum(Opaque, Cutout, Transparent)] _Mode ("BlendMode", int) = 0

		//Outline
		_OutlineColor ("Outline Color", Color) = (0,0,0,1.0)
		_OutlineSize ("Outline Size", Float) = 0.02

		//Dissolve 
		[Toggle] _UseDissolve ("Dissolve", int) = 0
	    _DissolveMap ("Dissolve (R)", 2D) = "white" {}
		_DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.5
		_DissolveRampSize ("Dissolve Ramp Size", Range(0.0, 1.0)) = 0.25
		_DissolveRamp ("Ramp (RGB)", 2D) = "white" {}
		_DissolveColor ("Dissolve Color", Color) = (1, 1, 1, 0.5)
	}
	/////////////////////////////////////////////////////////////////////////////////////////////
	// SHADER MODEL 3
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader 
	{ 
		Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}
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

			#include "/Inc/Outline/MKToonOutlineOnlySetup.cginc"
			#include "/Inc/Outline/MKToonOutlineOnlyBase.cginc"
			ENDCG 
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// SHADER MODEL 2
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader 
	{ 
		Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}
		Pass
		{
			LOD 150
			Tags {"LightMode" = "Always"}
			Name "OUTLINE_SM_2_5"
			Cull Front
			Blend [_Blend0] [_Blend1]
			ZWrite [_ZWrite]
			ZTest LEqual

			CGPROGRAM 
			#pragma target 2.5
			#pragma vertex outlinevert 
			#pragma fragment outlinefrag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature __ _MK_MODE_TRANSPARENT _MK_MODE_CUTOUT
			#pragma shader_feature __ _MK_DISSOLVE_DEFAULT _MK_DISSOLVE_RAMP

			#pragma multi_compile_fog

			#include "/Inc/Outline/MKToonOutlineOnlySetup.cginc"
			#include "/Inc/Outline/MKToonOutlineOnlyBase.cginc"
			ENDCG 
		}
	}
	CustomEditor "MK.Toon.MKToonEditor"
	FallBack Off
}