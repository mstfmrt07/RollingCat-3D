using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Toon
{
    public static class MKToonMaterialHelper
    {
        public static class PropertyNames
        {
            //Editor Properties
            public const string SHOW_MAIN_BEHAVIOR = "_MKEditorShowMainBehavior";
            public const string SHOW_DETAIL_BEHAVIOR = "_MKEditorShowDetailBehavior";
            public const string SHOW_LIGHT_BEHAVIOR = "_MKEditorShowLightBehavior";
            public const string SHOW_RENDER_BEHAVIOR = "_MKEditorShowRenderBehavior";
            public const string SHOW_SPECULAR_BEHAVIOR = "_MKEditorShowSpecularBehavior";
            public const string SHOW_TRANSLUCENT_BEHAVIOR = "_MKEditorShowTranslucentBehavior";
            public const string SHOW_RIM_BEHAVIOR = "_MKEditorShowRimBehavior";
            public const string SHOW_REFLECTION_BEHAVIOR = "_MKEditorShowReflectionBehavior";
            public const string SHOW_SHADOW_BEHAVIOR = "_MKEditorShowShadowBehavior";
            public const string SHOW_DISSOLVE_BEHAVIOR = "_MKEditorShowDissolveBehavior";
            public const string SHOW_OUTLINE_BEHAVIOR = "_MKEditorShowOutlineBehavior";
            public const string SHOW_SKETCH_BEHAVIOR = "_MKEditorShowSketchBehavior";

            //Main
            public const string MAIN_TEXTURE = "_MainTex";
            public const string CULL_MODE = "_CullMode";
            public const string Z_WRITE = "_ZWrite";
            public const string MAIN_COLOR = "_Color";
            public const string BLEND_0 = "_Blend0";
            public const string BLEND_1 = "_Blend1";
            public const string CUT_OFF = "_Cutoff";
            public const string BLEND_MODE = "_Mode";
            public const string USE_COLOR_SOURCE_MAP = "_AlbedoMap";

            //Detail
            public const string DETAIL_ALBEDO_MAP = "_DetailAlbedoMap";
            public const string DETAIL_BUMP_SCALE = "_DetailNormalMapScale";
            public const string DETAIL_BUMP_MAP = "_DetailNormalMap";
            public const string DETAIL_COLOR = "_DetailColor";
            public const string DETAIL_TINT = "_DetailTint";

            //Normalmap
            public const string USE_BUMP_MAP = "_UseBumpMap";
            public const string BUMP_MAP = "_BumpMap";
            public const string BUMP_SCALE = "_BumpScale";

            //Light
            public const string LIGHT_TYPE = "_LightType";
            public const string LIGHT_MODEL = "_LightModel";
            public const string Ramp = "_Ramp";
            public const string LIGHT_CUTS = "_LightCuts";
            public const string LIGHT_THRESHOLD = "_LightThreshold";
            public const string OCCLUSION_MAP = "_OcclusionMap";
            public const string OCCLUSION_STRENGTH = "_OcclusionStrength";

            //Render
            public const string LIGHT_SMOOTHNESS = "_LightSmoothness";
            public const string RIM_SMOOTHNESS = "_RimSmoothness";
            public const string REFLECT_SMOOTHNESS = "_ReflectSmoothness";
            public const string CONTRAST = "_Contrast";
            public const string SATURATION = "_Saturation";
            public const string BRIGHTNESS = "_Brightness";
            public const string ROUGHNESS = "_Roughness";

            //Custom shadow
            public const string SHADOW_COLOR = "_ShadowColor";
            public const string HIGHLIGHT_COLOR = "_HighlightColor";
            public const string SHADOW_INTENSITY = "_ShadowIntensity";

            //Outline
            public const string USE_OUTLINE = "_UseOutline";
            public const string OUTLINE_COLOR = "_OutlineColor";
            public const string OUTLINE_SIZE = "_OutlineSize";

            //Rim
            public const string USE_RIM = "_UseRim";
            public const string RIM_COLOR = "_RimColor";
            public const string RIM_SIZE = "_RimSize";
            public const string RIM_INTENSITY = "_RimIntensity";

            //Specular
            public const string SPECULAR_SHININESS = "_Shininess";
            public const string SPEC_COLOR = "_SpecColor";
            public const string SPEC_GLOSS_MAP = "_SpecGlossMap";
            public const string SPECULAR_INTENSITY = "_SpecularIntensity";
            public const string USE_ANISOTROPIC_SPECULAR = "_UseAnisotropicSpecular";
            public const string ANISO_MAP = "_AnisoMap";
            public const string ANISO_OFFSET = "_AnisoOffset";

            //Reflection
            public const string USE_REFLECTION = "_UseReflection";
            public const string REFLECT_COLOR = "_ReflectColor";
            public const string REFLECT_INTENSITY = "_ReflectIntensity";
            public const string REFLECT_MAP = "_ReflectMap";

            //Dissolve
            public const string USE_DISSOLVE = "_UseDissolve";
            public const string DISSOLVE_MAP = "_DissolveMap";
            public const string DISSOLVE_AMOUNT = "_DissolveAmount";
            public const string DISSOLVE_RAMP_SIZE = "_DissolveRampSize";
            public const string DISSOLVE_RAMP = "_DissolveRamp";
            public const string DISSOLVE_COLOR = "_DissolveColor";

            //Translucent
            public const string USE_TRANSLUCENT = "_UseTranslucent";
            public const string TRANSLUCENT_COLOR = "_TranslucentColor";
            public const string TRANSLUCENT_MAP = "_TranslucentMap";
            public const string TRANSLUCENT_INTENSITY = "_TranslucentIntensity";
            public const string TRANSLUCENT_SHININESS = "_TranslucentShininess";

            //Emission
            public const string EMISSION_COLOR = "_EmissionColor";
            public const string EMISSION_MAP = "_EmissionMap";
            public const string EMISSION = "_Emission";

            //Sketch
            public const string USE_SKETCH = "_UseSketch";
            public const string SKETCH_MAP = "_SketchMap";
            public const string SKETCH_TINT = "_SketchScale";
            public const string SKETCH_TONE_MIN = "_SketchToneMin";
            public const string SKETCH_TONE_MAX = "_SketchToneMax";
        }

        //Main
        public static void SetMainTexture(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.MAIN_TEXTURE, tex);
        }
        public static Texture GetMainTexture(Material material)
        {
            return material.GetTexture(PropertyNames.MAIN_TEXTURE);
        }

        public static void SetMainColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.MAIN_COLOR, color);
        }
        public static Color MainColor(Material material)
        {
            return material.GetColor(PropertyNames.MAIN_COLOR);
        }

        public static void SetZWrite(Material material, bool z)
        {
            if (z)
                material.SetFloat(PropertyNames.Z_WRITE, 1);
            else
                material.SetFloat(PropertyNames.Z_WRITE, 0);
        }
        public static bool GetZWrite(Material material)
        {
            if (material.GetFloat(PropertyNames.Z_WRITE) == 1.0f)
                return true;
            else
                return false;
        }

        public static void SetCullMode(Material material, UnityEngine.Rendering.CullMode cull)
        {
            material.SetFloat(PropertyNames.CULL_MODE, (int)cull);
        }
        public static UnityEngine.Rendering.CullMode GetCullMode(Material material)
        {
            return (UnityEngine.Rendering.CullMode)material.GetFloat(PropertyNames.CULL_MODE);
        }

        //Detail
        public static void SetDetailTint(Material material, float tint)
        {
            material.SetFloat(PropertyNames.DETAIL_TINT, tint);
        }
        public static float GeDetailTint(Material material)
        {
            return material.GetFloat(PropertyNames.DETAIL_TINT);
        }
        public static void SetDetailTexture(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DETAIL_ALBEDO_MAP, tex);
        }
        public static Texture GetDetailTexture(Material material)
        {
            return material.GetTexture(PropertyNames.DETAIL_ALBEDO_MAP);
        }
        public static void SetDetailNormalMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DETAIL_BUMP_MAP, tex);
        }
        public static Texture GetDetailNormalMap(Material material)
        {
            return material.GetTexture(PropertyNames.DETAIL_BUMP_MAP);
        }
        public static void SetDetailBumpScale(Material material, float bumpScale)
        {
            material.SetFloat(PropertyNames.DETAIL_BUMP_SCALE, bumpScale);
        }
        public static float GetDetailBumpScale(Material material)
        {
            return material.GetFloat(PropertyNames.DETAIL_BUMP_SCALE);
        }
        public static void SetDetailColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.DETAIL_COLOR, color);
        }
        public static Color GetDetailColor(Material material)
        {
            return material.GetColor(PropertyNames.DETAIL_COLOR);
        }

        //Normalmap
        public static void SetNormalmap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.BUMP_MAP, tex);
        }
        public static Texture GetBumpMap(Material material)
        {
            return material.GetTexture(PropertyNames.BUMP_MAP);
        }

        public static void SetBumpScale(Material material, float bumpScale)
        {
            material.SetFloat(PropertyNames.BUMP_SCALE, bumpScale);
        }
        public static float GetBumpScale(Material material)
        {
            return material.GetFloat(PropertyNames.BUMP_SCALE);
        }

        //Light
        public static void SetLightRampMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.Ramp, tex);
        }
        public static Texture GetLightRampMap(Material material)
        {
            return material.GetTexture(PropertyNames.Ramp);
        }

        public static void SetLightCuts(Material material, float cuts)
        {
            material.SetFloat(PropertyNames.LIGHT_CUTS, cuts);
        }
        public static float GetLightCuts(Material material)
        {
            return material.GetFloat(PropertyNames.LIGHT_CUTS);
        }

        public static void SetLightThreshold(Material material, float threshold)
        {
            material.SetFloat(PropertyNames.LIGHT_THRESHOLD, threshold);
        }
        public static float GetLightThreshold(Material material)
        {
            return material.GetFloat(PropertyNames.LIGHT_THRESHOLD);
        }

        //Occlusion
        public static void SetOcclusionMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.OCCLUSION_MAP, tex);
        }
        public static Texture GetOcclusionMap(Material material)
        {
            return material.GetTexture(PropertyNames.OCCLUSION_MAP);
        }
        public static void SetOcclusionStrength(Material material, float strength)
        {
            material.SetFloat(PropertyNames.OCCLUSION_STRENGTH, strength);
        }
        public static float GetOcclusionStrength(Material material)
        {
            return material.GetFloat(PropertyNames.OCCLUSION_STRENGTH);
        }

        //Render
        public static void SetBrightness(Material material, float brightness)
        {
            material.SetFloat(PropertyNames.BRIGHTNESS, brightness);
        }
        public static float GetBrightness(Material material)
        {
            return material.GetFloat(PropertyNames.BRIGHTNESS);
        }

        public static void SetContrast(Material material, float contrast)
        {
            material.SetFloat(PropertyNames.CONTRAST, contrast);
        }
        public static float GetContrast(Material material)
        {
            return material.GetFloat(PropertyNames.CONTRAST);
        }

        public static void SetSaturation(Material material, float saturation)
        {
            material.SetFloat(PropertyNames.SATURATION, saturation);
        }
        public static float GetSaturation(Material material)
        {
            return material.GetFloat(PropertyNames.SATURATION);
        }

        public static void SetLightSmoothness(Material material, float smoothness)
        {
            material.SetFloat(PropertyNames.LIGHT_SMOOTHNESS, smoothness);
        }
        public static float GetLightSmoothness(Material material)
        {
            return material.GetFloat(PropertyNames.LIGHT_SMOOTHNESS);
        }

        public static void SetRimSmoothness(Material material, float smoothness)
        {
            material.SetFloat(PropertyNames.RIM_SMOOTHNESS, smoothness);
        }
        public static float GetRimSmoothness(Material material)
        {
            return material.GetFloat(PropertyNames.RIM_SMOOTHNESS);
        }

        public static void SetReflectSmoothness(Material material, float smoothness)
        {
            material.SetFloat(PropertyNames.REFLECT_SMOOTHNESS, smoothness);
        }
        public static float GetReflectSmoothness(Material material)
        {
            return material.GetFloat(PropertyNames.REFLECT_SMOOTHNESS);
        }

        public static void SetRoughness(Material material, float roughness)
        {
            material.SetFloat(PropertyNames.ROUGHNESS, roughness);
        }
        public static float GetRoughness(Material material)
        {
            return material.GetFloat(PropertyNames.ROUGHNESS);
        }

        //Custom shadow
        public static void SetShadowColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.SHADOW_COLOR, color);
        }
        public static Color GetShadowColor(Material material)
        {
            return material.GetColor(PropertyNames.SHADOW_COLOR);
        }

        public static void SetHightlightColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.HIGHLIGHT_COLOR, color);
        }
        public static Color GetHightlightColor(Material material)
        {
            return material.GetColor(PropertyNames.HIGHLIGHT_COLOR);
        }

        public static void SetShadowIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.SHADOW_INTENSITY, intensity);
        }
        public static float GetShadowIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.SHADOW_INTENSITY);
        }

        //Outline
        public static void SetOutlineColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.OUTLINE_COLOR, color);
        }
        public static Color SetOutlineColor(Material material)
        {
            return material.GetColor(PropertyNames.OUTLINE_COLOR);
        }

        public static void SetOutlineSize(Material material, float size)
        {
            material.SetFloat(PropertyNames.OUTLINE_SIZE, size);
        }
        public static float GetOutlineSize(Material material)
        {
            return material.GetFloat(PropertyNames.OUTLINE_SIZE);
        }

        //Rim
        public static void SetRimColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.RIM_COLOR, color);
        }
        public static Color GetRimColor(Material material)
        {
            return material.GetColor(PropertyNames.RIM_COLOR);
        }

        public static void SetRimSize(Material material, float size)
        {
            material.SetFloat(PropertyNames.RIM_SIZE, size);
        }
        public static float GetRimSize(Material material)
        {
            return material.GetFloat(PropertyNames.RIM_SIZE);
        }

        public static void SetRimIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.RIM_INTENSITY, intensity);
        }
        public static float GetRimIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.RIM_INTENSITY);
        }

        //Specular
        public static void SetSpecularShininess(Material material, float shininess)
        {
            material.SetFloat(PropertyNames.SPECULAR_SHININESS, shininess);
        }
        public static float GetSpecularShininess(Material material)
        {
            return material.GetFloat(PropertyNames.SPECULAR_SHININESS);
        }

        public static void SetSpecularColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.SPEC_COLOR, color);
        }
        public static Color GetSpecularColor(Material material)
        {
            return material.GetColor(PropertyNames.SPEC_COLOR);
        }

        public static void SetSpecularMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.SPEC_GLOSS_MAP, tex);
        }
        public static Texture GetSpecularMap(Material material)
        {
            return material.GetTexture(PropertyNames.SPEC_GLOSS_MAP);
        }

        public static void SetSpecularIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.SPECULAR_INTENSITY, intensity);
        }
        public static float GetSpecularIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.SPECULAR_INTENSITY);
        }

        public static void SetAnisoMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.ANISO_MAP, tex);
        }
        public static Texture GetAnisoMap(Material material)
        {
            return material.GetTexture(PropertyNames.ANISO_MAP);
        }

        public static void SetAnisoOffset(Material material, float offset)
        {
            material.SetFloat(PropertyNames.ANISO_OFFSET, offset);
        }
        public static float GetAnisoOffset(Material material)
        {
            return material.GetFloat(PropertyNames.ANISO_OFFSET);
        }

        //Reflection
        public static void SetReflectMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.REFLECT_MAP, tex);
        }
        public static Texture GetReflectMap(Material material)
        {
            return material.GetTexture(PropertyNames.REFLECT_MAP);
        }

        public static void SetReflectColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.REFLECT_COLOR, color);
        }
        public static Color GetReflectColor(Material material)
        {
            return material.GetColor(PropertyNames.REFLECT_COLOR);
        }

        public static void SetReflectIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.REFLECT_INTENSITY, intensity);
        }
        public static float GetReflectIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.REFLECT_INTENSITY);
        }

        //Dissolve
        public static void SetDissolveMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DISSOLVE_MAP, tex);
        }
        public static Texture GetDissolveMap(Material material)
        {
            return material.GetTexture(PropertyNames.DISSOLVE_MAP);
        }

        public static void SetDissolveRampMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DISSOLVE_RAMP, tex);
        }
        public static Texture GetDissolveRampMap(Material material)
        {
            return material.GetTexture(PropertyNames.DISSOLVE_RAMP);
        }

        public static void SetDissolveColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.DISSOLVE_COLOR, color);
        }
        public static Color GetDissolveColor(Material material)
        {
            return material.GetColor(PropertyNames.DISSOLVE_COLOR);
        }

        public static void SetDissolveAmount(Material material, float amount)
        {
            material.SetFloat(PropertyNames.DISSOLVE_AMOUNT, amount);
        }
        public static float GetDissolveAmount(Material material)
        {
            return material.GetFloat(PropertyNames.DISSOLVE_AMOUNT);
        }

        public static void SetDissolveRampSize(Material material, float size)
        {
            material.SetFloat(PropertyNames.DISSOLVE_RAMP_SIZE, size);
        }
        public static float GetDissolveRampSize(Material material)
        {
            return material.GetFloat(PropertyNames.DISSOLVE_RAMP_SIZE);
        }

        //Translucent
        public static void SetTranslucentMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.TRANSLUCENT_MAP, tex);
        }
        public static Texture GetTranslucentMap(Material material)
        {
            return material.GetTexture(PropertyNames.TRANSLUCENT_MAP);
        }

        public static void SetTranslucentColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.TRANSLUCENT_COLOR, color);
        }
        public static Color GetTranslucentColor(Material material)
        {
            return material.GetColor(PropertyNames.TRANSLUCENT_COLOR);
        }

        public static void SetTranslucentIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.TRANSLUCENT_INTENSITY, intensity);
        }
        public static float GetTranslucentIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.TRANSLUCENT_INTENSITY);
        }

        public static void SetTranslucentShininess(Material material, float shininess)
        {
            material.SetFloat(PropertyNames.TRANSLUCENT_SHININESS, shininess);
        }
        public static float GetTranslucentShininess(Material material)
        {
            return material.GetFloat(PropertyNames.TRANSLUCENT_SHININESS);
        }

        //Emission
        public static void SetEmissionMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.EMISSION_MAP, tex);
        }
        public static Texture GetEmissionMap(Material material)
        {
            return material.GetTexture(PropertyNames.EMISSION_MAP);
        }

        public static void SetEmissionColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.EMISSION_COLOR, color);
        }
        public static Color GetEmissionColor(Material material)
        {
            return material.GetColor(PropertyNames.EMISSION_COLOR);
        }

        //Sketch
        public static void SetSketchMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.SKETCH_MAP, tex);
        }
        public static Texture GetSketchMap(Material material)
        {
            return material.GetTexture(PropertyNames.SKETCH_MAP);
        }

        public static void SetSketchTint(Material material, float tint)
        {
            material.SetFloat(PropertyNames.SKETCH_TINT, tint);
        }
        public static float GetSketchTint(Material material)
        {
            return material.GetFloat(PropertyNames.SKETCH_TINT);
        }

        public static void SetSketchToneMin(Material material, float toneMin)
        {
            material.SetFloat(PropertyNames.SKETCH_TONE_MIN, toneMin);
        }
        public static float GetSketchToneMin(Material material)
        {
            return material.GetFloat(PropertyNames.SKETCH_TONE_MIN);
        }
        public static void SetSketchToneMax(Material material, float toneMax)
        {
            material.SetFloat(PropertyNames.SKETCH_TONE_MAX, toneMax);
        }
        public static float GetSketchToneMax(Material material)
        {
            return material.GetFloat(PropertyNames.SKETCH_TONE_MAX);
        }

    }
}