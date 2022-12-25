using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.Utils;
using UnityEditorInternal;

#if UNITY_EDITOR
namespace MK.Toon
{
    public class MKToonEditor : ShaderGUI
    {
        public static class GuiStyles
        {
            public static GUIStyle header = new GUIStyle("ShurikenModuleTitle")
            {
                font = (new GUIStyle("Label")).font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f),
            };

            public static GUIStyle headerCheckbox = new GUIStyle("ShurikenCheckMark");
            public static GUIStyle headerCheckboxMixed = new GUIStyle("ShurikenCheckMarkMixed");
        }

        private static class GUIContentCollection
        {
            public static GUIContent mainColor = new GUIContent("Color", "Basic color tint");
            public static GUIContent blendMode = new GUIContent("Blend", "Blending of pixels on the screen");
            public static GUIContent cullMode = new GUIContent("Cull", "Controls which sides of the polygon shuld not be drawn");
            public static GUIContent alphaCutoff = new GUIContent("Alpha cutoff", "Controls which fragments should be skipped");
            public static GUIContent lightModel = new GUIContent("Light model", "Change the lightmodel \n Unlit - No lights are calculated \n Lambert - Diffuse shading wihout specular \n Blinn-Phong - Diffuse and specular shading");
            public static GUIContent lightType = new GUIContent("Light type", "Default - Simple smooth light calculation \n Threshold - Cel shading with 2 cuts \n Cul shading with up to 6 cuts");
            public static GUIContent threshold = new GUIContent("Threshold", "Influence of the light attenuation");
            public static GUIContent lightLevels = new GUIContent("Cuts", "Amount of lightcuts");
            public static GUIContent highlightColor = new GUIContent("Highlight color", "Color tint of lit areas");
            public static GUIContent shadowColor = new GUIContent("Shadow color", "Color tint of shadowed areas");
            public static GUIContent lightSmoothness = new GUIContent("Light smoothness", "Smoothness of the light calculation");
            public static GUIContent rimSmoothness = new GUIContent("Rim smoothness", "Smoothness of the rim light");
            public static GUIContent reflectSmoothness = new GUIContent("Reflect smoothness", "Smoothness of the reflection the surface");
            public static GUIContent contrast = new GUIContent("Contrast", "Contrast intensity");
            public static GUIContent saturation = new GUIContent("Saturation", "Color saturation");
            public static GUIContent brightness = new GUIContent("Brightness", "Color brightness");
            public static GUIContent shadowIntensity = new GUIContent("intensity", "Intensity ajustment for the shadows");
            public static GUIContent specularShininess = new GUIContent("Shininess", "The level of blur for the specular highlight");
            public static GUIContent specularColor = new GUIContent("Color", "Color tint of specular highlights");
            public static GUIContent translucentShininess = new GUIContent("Shininess", "The level of blur for the translucent highlight");
            public static GUIContent translucentColor = new GUIContent("Color", "Color tint of translucent highlights");
            public static GUIContent rimSize = new GUIContent("Size", "Amount of highlighted areas by rim");
            public static GUIContent rimColor = new GUIContent("Color", "Color of the rim highlight");
            public static GUIContent rimIntensity = new GUIContent("Intensity", "Intensity of the rim highlight");
            public static GUIContent outlineColor = new GUIContent("Color", "Color of the outline");
            public static GUIContent outlineSize = new GUIContent("Size", "Size of the outline");
            public static GUIContent reflectColor = new GUIContent("Color", "Color tint of the reflection");
            public static GUIContent roughness = new GUIContent("Roughness", "Roughness of the minnaert lightmodel");
            public static GUIContent dissolveColor = new GUIContent("Dissolve color", "Color tint of the dissolve effect");
            public static GUIContent reflection = new GUIContent("Intensity", "Reflect Map (R)");
            public static GUIContent emission = new GUIContent("Emission", "Emission Map (RGB)");
            public static GUIContent specular = new GUIContent("intensity", "Spec (R) Gloss (G) Aniso (B)");
            public static GUIContent translucent = new GUIContent("intensity", "Power (R) Gloss (G)");
            public static GUIContent normalMap = new GUIContent("Normal map", "Normal map (Bump)");
            public static GUIContent dissolveMap = new GUIContent("Dissolve", "Dissolve (R)");
            public static GUIContent dissolveRamp = new GUIContent("Ramp", "Ramp (RGB)");
            public static GUIContent mainTex = new GUIContent("Albedo", "Albedo (RGB)");
            public static GUIContent rampTex = new GUIContent("Ramp", "Ramp (RGB)");
            public static GUIContent anisoMap = new GUIContent("Anisotropic", "Anisomap (Bump) - Offset");
            public static GUIContent occlusion = new GUIContent("Occlusion", "Occlusion map (G) and strength");
            public static GUIContent detailTex = new GUIContent("Detail", "Detail Albedo 1X, multiplied to the main albedo (RGB)");
            public static GUIContent detailNormalMap = new GUIContent("Normal map", "Detail normal map (Bump)");
            public static GUIContent detailColor = new GUIContent("Color", "Basic detail tint");
            public static GUIContent detailTint = new GUIContent("Tint", "detail opacity");
            public static GUIContent sketchmap = new GUIContent("Sketch", "Sketch - RGB");
            public static GUIContent sketchTint = new GUIContent("Tint", "");
            public static GUIContent sketchToneMin = new GUIContent("sketch tone min", "");
            public static GUIContent sketchToneMax = new GUIContent("sketch tone max", "");
        }

        #region const
        internal const string BUMP_MAP = "_MK_BUMP_MAP";
        internal const string ANISOTROPIC = "_MK_ANISOTROPIC_SPECULAR";
        internal const string EMISSION_DEFAULT = "_MK_EMISSION_DEFAULT";
        internal const string EMISSION_MAP = "_MK_EMISSION_MAP";
        internal const string COLORSOURCE_MAP = "_MK_ALBEDO_MAP";
        internal const string REFLECTIVE_MAP = "_MK_REFLECTIVE_MAP";
        internal const string REFLECTIVE_DEFAULT = "_MK_REFLECTIVE_DEFAULT";
        internal const string DISSOLVE_DEFAULT = "_MK_DISSOLVE_DEFAULT";
        internal const string DISSOLVE_RAMP = "_MK_DISSOLVE_RAMP";
        internal const string TRANSLUCENT_DEFAULT = "_MK_TRANSLUCENT_DEFAULT";
        internal const string TRANSLUCENT_MAP = "_MK_TRANSLUCENT_MAP";
        internal const string SPECULAR_MAP = "_MK_SPECULAR_MAP";
        internal const string SHADER_OUTLINE = "Outline";
        internal const string RIM = "_MK_RIM";
        internal const string OCCLUSION = "_MK_OCCLUSION";
        internal const string ALBEDO_MAP = "_MK_ALBEDO_MAP";
        internal const string REFLECTIVE_FRESNEL = "_MK_REFLECTIVE_FRESNEL";
        internal const string DETAIL_MAP = "_MK_DETAIL_MAP";
        internal const string DETAIL_BUMP_MAP = "_MK_DETAIL_BUMP_MAP";
        internal const string SKETCH = "_MK_SKETCH";
        internal static readonly string[] LIGHT_MODEL = new string[6] { "_MK_LIGHTMODEL_UNLIT", "_MK_LIGHTMODEL_LAMBERT", "_MK_LIGHTMODEL_PHONG", "_MK_LIGHTMODEL_BLINN_PHONG", "_MK_LIGHTMODEL_MINNAERT", "_MK_LIGHTMODEL_OREN_NAYER" };
        internal static readonly string[] LIGHT_TYPE = new string[4] { "_MK_LIGHTTYPE_DEFAULT", "_MK_LIGHTTYPE_CEL_SHADE_SIMPLE", "_MK_LIGHTTYPE_CEL_SHADE_MULTI", "_MK_LIGHTTYPE_RAMP" };
        internal static readonly string[] BLEND_MODE = new string[2] { "_MK_MODE_TRANSPARENT", "_MK_MODE_CUTOUT" };
        #endregion
        #region pe
        public enum ZWrite
        {
            On = 1,
            Off = 0
        }
        public enum LightType
        {
            Default = 0,
            Cel_Shade_Simple = 1,
            Cel_Shade_Multi = 2,
            Ramp = 3
        }
        public enum LightModel
        {
            Unlit = 0,
            Lambert = 1,
            Phong = 2,
            Blinn_Phong = 3,
            Minneart = 4,
            Oren_Nayer = 5
        }
        public enum BlendMode
        {
            Opaque = 0,
            Cutout = 1,
            Transparent = 2
        }

        private enum KeywordsToManage
        {
            COLOR_SOURCE,
            BUMP,
            LIGHT_TYPE,
            LIGHT_MODEL,
            RIM,
            DETAIL_ALBEDO,
            DETAIL_BUMP,
            ANISOTROPIC_SPECULAR,
            SPECULAR,
            EMISSION,
            REFLECTIVE,
            TRANSLUCENT,
            DISSOLVE,
            OCCLUSION,
            SKETCH,
            ALL
        };
        #endregion

        //hdr config
        private ColorPickerHDRConfig colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

        //shaders
        private static Shader defaultShader = Shader.Find("MK/Toon/Default");
        private static Shader defaultOutlineShader = Shader.Find("Hidden/MK/Toon/Default Outline");

        //Editor Properties
        private MaterialProperty showMainBehavior = null;
        private MaterialProperty showDetailBehavior = null;
        private MaterialProperty showLightBehavior = null;
        private MaterialProperty showRenderBehavior = null;
        private MaterialProperty showSpecularBehavior = null;
        private MaterialProperty showTranslucentBehavior = null;
        private MaterialProperty showRimBehavior = null;
        private MaterialProperty showReflectionBehavior = null;
        private MaterialProperty showShadowBehavior = null;
        private MaterialProperty showDissolveBehavior = null;
        private MaterialProperty showOutlineBehavior = null;
        private MaterialProperty showSketchBehavior = null;

        //Main
        private MaterialProperty blendMode = null;
        private MaterialProperty cutoff = null;
        private MaterialProperty mainColor = null;
        private MaterialProperty mainTex = null;
        private MaterialProperty cullMode = null;
        //private MaterialProperty useColorsourceMap = null;

        //detail
        private MaterialProperty detailAlbedoMap = null;
        private MaterialProperty detailNormalMapScale = null;
        private MaterialProperty detailNormalMap = null;
        private MaterialProperty detailColor = null;
        private MaterialProperty detailTint = null;

        //Normalmap
        private MaterialProperty normalMap = null;
        //private MaterialProperty useNormalMap = null;
        private MaterialProperty bumpScale = null;

        //Light
        private MaterialProperty lightType = null;
        private MaterialProperty lightModel = null;
        private MaterialProperty lightLevels = null;
        private MaterialProperty lTreshold = null;
        private MaterialProperty rampTex = null;
        private MaterialProperty occlusionMap = null;
        private MaterialProperty occlusionStrength = null;

        //Render
        private MaterialProperty contrast = null;
        private MaterialProperty saturation = null;
        private MaterialProperty brightness = null;
        private MaterialProperty lightSmoothness = null;
        private MaterialProperty rimSmoothness = null;
        private MaterialProperty reflectSmoothness = null;
        private MaterialProperty shadowIntensity = null;
        private MaterialProperty roughness = null;

        //Custom shadow
        private MaterialProperty shadowColor = null;
        private MaterialProperty highlightColor = null;

        //Outline
        private MaterialProperty useOutline = null;
        private MaterialProperty outlineColor = null;
        private MaterialProperty outlineSize = null;

        //Rim
        private MaterialProperty rimColor = null;
        private MaterialProperty rimSize = null;
        private MaterialProperty rimIntensity = null;
        private MaterialProperty useRim = null;

        //Specular
        private MaterialProperty specularIntensity = null;
        private MaterialProperty reflectIntensity = null;
        private MaterialProperty specularMap = null;
        private MaterialProperty shininess = null;
        //private MaterialProperty useAnisoSpec = null;
        private MaterialProperty anisoMap = null;
        private MaterialProperty anisoOffset = null;
        private MaterialProperty specularColor = null;

        //Reflection
        private MaterialProperty useReflection = null;
        private MaterialProperty reflectionColor = null;
        private MaterialProperty reflectMap = null;

        //Dissolve
        private MaterialProperty useDissolve = null;
        private MaterialProperty dissolveColor = null;
        private MaterialProperty dissolveMap = null;
        private MaterialProperty dissolveAmount = null;
        private MaterialProperty dissolveSize = null;
        private MaterialProperty dissolveRamp = null;

        //Translucent
        private MaterialProperty translucentMap = null;
        private MaterialProperty translucentColor = null;
        private MaterialProperty useTranslucent = null;
        private MaterialProperty translucentIntensity = null;
        private MaterialProperty translucentShininess = null;

        //Emission
        private MaterialProperty emissionColor = null;
        private MaterialProperty emissionTex = null;

        //Sketch
        private MaterialProperty useSketch = null;
        private MaterialProperty sketchMap = null;
        private MaterialProperty sketchTint = null;
        private MaterialProperty sketchToneMin = null;
        private MaterialProperty sketchToneMax = null;

        private bool showGIField = false;

        public void FindProperties(MaterialProperty[] props, Material mat)
        {
            defaultShader = Shader.Find("MK/Toon/Default");
            defaultOutlineShader = Shader.Find("Hidden/MK/Toon/Default Outline");

            //Editor Properties
            showMainBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_MAIN_BEHAVIOR, props);
            showDetailBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_DETAIL_BEHAVIOR, props);
            showLightBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_LIGHT_BEHAVIOR, props);
            showRenderBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_RENDER_BEHAVIOR, props);
            showSpecularBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_SPECULAR_BEHAVIOR, props);
            showTranslucentBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_TRANSLUCENT_BEHAVIOR, props);
            showRimBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_RIM_BEHAVIOR, props);
            showReflectionBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_REFLECTION_BEHAVIOR, props);
            showShadowBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_SHADOW_BEHAVIOR, props);
            showDissolveBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_DISSOLVE_BEHAVIOR, props);
            showOutlineBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_OUTLINE_BEHAVIOR, props);
            showSketchBehavior = FindProperty(MKToonMaterialHelper.PropertyNames.SHOW_SKETCH_BEHAVIOR, props);

            //Main
            mainColor = FindProperty(MKToonMaterialHelper.PropertyNames.MAIN_COLOR, props);
            mainTex = FindProperty(MKToonMaterialHelper.PropertyNames.MAIN_TEXTURE, props);
            blendMode = FindProperty(MKToonMaterialHelper.PropertyNames.BLEND_MODE, props);
            cullMode = FindProperty(MKToonMaterialHelper.PropertyNames.CULL_MODE, props);
            cutoff = FindProperty(MKToonMaterialHelper.PropertyNames.CUT_OFF, props);
            //useColorsourceMap = FindProperty(MKToonMaterialHelper.PropertyNames.USE_COLOR_SOURCE_MAP, props);

            //detail
            detailAlbedoMap = FindProperty(MKToonMaterialHelper.PropertyNames.DETAIL_ALBEDO_MAP, props);
            detailNormalMapScale = FindProperty(MKToonMaterialHelper.PropertyNames.DETAIL_BUMP_SCALE, props);
            detailNormalMap = FindProperty(MKToonMaterialHelper.PropertyNames.DETAIL_BUMP_MAP, props);
            detailColor = FindProperty(MKToonMaterialHelper.PropertyNames.DETAIL_COLOR, props);
            detailTint = FindProperty(MKToonMaterialHelper.PropertyNames.DETAIL_TINT, props);


            //Normalmap
            //useNormalMap = FindProperty(MKToonMaterialHelper.PropertyNames.USE_BUMP_MAP, props);
            bumpScale = FindProperty(MKToonMaterialHelper.PropertyNames.BUMP_SCALE, props);
            normalMap = FindProperty(MKToonMaterialHelper.PropertyNames.BUMP_MAP, props);

            //Light
            lightType = FindProperty(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE, props);
            lightModel = FindProperty(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL, props);
            lightLevels = FindProperty(MKToonMaterialHelper.PropertyNames.LIGHT_CUTS, props);
            lTreshold = FindProperty(MKToonMaterialHelper.PropertyNames.LIGHT_THRESHOLD, props);
            rampTex = FindProperty(MKToonMaterialHelper.PropertyNames.Ramp, props);
            occlusionMap = FindProperty(MKToonMaterialHelper.PropertyNames.OCCLUSION_MAP, props);
            occlusionStrength = FindProperty(MKToonMaterialHelper.PropertyNames.OCCLUSION_STRENGTH, props);

            //Render
            contrast = FindProperty(MKToonMaterialHelper.PropertyNames.CONTRAST, props);
            saturation = FindProperty(MKToonMaterialHelper.PropertyNames.SATURATION, props);
            brightness = FindProperty(MKToonMaterialHelper.PropertyNames.BRIGHTNESS, props);
            lightSmoothness = FindProperty(MKToonMaterialHelper.PropertyNames.LIGHT_SMOOTHNESS, props);
            rimSmoothness = FindProperty(MKToonMaterialHelper.PropertyNames.RIM_SMOOTHNESS, props);
            reflectSmoothness = FindProperty(MKToonMaterialHelper.PropertyNames.REFLECT_SMOOTHNESS, props);
            roughness = FindProperty(MKToonMaterialHelper.PropertyNames.ROUGHNESS, props);

            //Custom shadow
            shadowIntensity = FindProperty(MKToonMaterialHelper.PropertyNames.SHADOW_INTENSITY, props);
            shadowColor = FindProperty(MKToonMaterialHelper.PropertyNames.SHADOW_COLOR, props);
            highlightColor = FindProperty(MKToonMaterialHelper.PropertyNames.HIGHLIGHT_COLOR, props);

            //Outline
            useOutline = FindProperty(MKToonMaterialHelper.PropertyNames.USE_OUTLINE, props);
            outlineColor = FindProperty(MKToonMaterialHelper.PropertyNames.OUTLINE_COLOR, props);
            outlineSize = FindProperty(MKToonMaterialHelper.PropertyNames.OUTLINE_SIZE, props);

            //Rim
            rimColor = FindProperty(MKToonMaterialHelper.PropertyNames.RIM_COLOR, props);
            rimSize = FindProperty(MKToonMaterialHelper.PropertyNames.RIM_SIZE, props);
            useRim = FindProperty(MKToonMaterialHelper.PropertyNames.USE_RIM, props);
            rimIntensity = FindProperty(MKToonMaterialHelper.PropertyNames.RIM_INTENSITY, props);

            //Specular
            shininess = FindProperty(MKToonMaterialHelper.PropertyNames.SPECULAR_SHININESS, props);
            specularColor = FindProperty(MKToonMaterialHelper.PropertyNames.SPEC_COLOR, props);
            specularMap = FindProperty(MKToonMaterialHelper.PropertyNames.SPEC_GLOSS_MAP, props);
            specularIntensity = FindProperty(MKToonMaterialHelper.PropertyNames.SPECULAR_INTENSITY, props);
            //useAnisoSpec = FindProperty(MKToonMaterialHelper.PropertyNames.USE_ANISOTROPIC_SPECULAR, props);
            anisoMap = FindProperty(MKToonMaterialHelper.PropertyNames.ANISO_MAP, props);
            anisoOffset = FindProperty(MKToonMaterialHelper.PropertyNames.ANISO_OFFSET, props);

            //Reflection
            useReflection = FindProperty(MKToonMaterialHelper.PropertyNames.USE_REFLECTION, props);
            reflectionColor = FindProperty(MKToonMaterialHelper.PropertyNames.REFLECT_COLOR, props);
            reflectMap = FindProperty(MKToonMaterialHelper.PropertyNames.REFLECT_MAP, props);
            reflectIntensity = FindProperty(MKToonMaterialHelper.PropertyNames.REFLECT_INTENSITY, props);

            //Emission
            emissionColor = FindProperty(MKToonMaterialHelper.PropertyNames.EMISSION_COLOR, props);
            emissionTex = FindProperty(MKToonMaterialHelper.PropertyNames.EMISSION_MAP, props);

            //Dissolve
            useDissolve = FindProperty(MKToonMaterialHelper.PropertyNames.USE_DISSOLVE, props);
            dissolveColor = FindProperty(MKToonMaterialHelper.PropertyNames.DISSOLVE_COLOR, props);
            dissolveMap = FindProperty(MKToonMaterialHelper.PropertyNames.DISSOLVE_MAP, props);
            dissolveAmount = FindProperty(MKToonMaterialHelper.PropertyNames.DISSOLVE_AMOUNT, props);
            dissolveSize = FindProperty(MKToonMaterialHelper.PropertyNames.DISSOLVE_RAMP_SIZE, props);
            dissolveRamp = FindProperty(MKToonMaterialHelper.PropertyNames.DISSOLVE_RAMP, props);

            //Translucent
            translucentColor = FindProperty(MKToonMaterialHelper.PropertyNames.TRANSLUCENT_COLOR, props);
            translucentMap = FindProperty(MKToonMaterialHelper.PropertyNames.TRANSLUCENT_MAP, props);
            useTranslucent = FindProperty(MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT, props);
            translucentIntensity = FindProperty(MKToonMaterialHelper.PropertyNames.TRANSLUCENT_INTENSITY, props);
            translucentShininess = FindProperty(MKToonMaterialHelper.PropertyNames.TRANSLUCENT_SHININESS, props);

            //Sketch
            sketchMap = FindProperty(MKToonMaterialHelper.PropertyNames.SKETCH_MAP, props);
            sketchTint = FindProperty(MKToonMaterialHelper.PropertyNames.SKETCH_TINT, props);
            sketchToneMin = FindProperty(MKToonMaterialHelper.PropertyNames.SKETCH_TONE_MIN, props);
            sketchToneMax = FindProperty(MKToonMaterialHelper.PropertyNames.SKETCH_TONE_MAX, props);
            useSketch = FindProperty(MKToonMaterialHelper.PropertyNames.USE_SKETCH, props);
        }

        [MenuItem("CONTEXT/Material/Reset", false, 2100)]
        static void Reset(MenuCommand command)
        {
            try
            {
                defaultShader = Shader.Find("MK/Toon/Default");
                defaultOutlineShader = Shader.Find("Hidden/MK/Toon/Default Outline");

                Material mat = null;
                mat = (Material)command.context;
                Undo.RecordObject(mat, "Reset Material");
                string[] kws = mat.shaderKeywords;
                Material tmp_mat = new Material(mat.shader);
                mat.CopyPropertiesFromMaterial(tmp_mat);

                if (mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_OUTLINE) == 1.0f)
                {
                    if (!mat.shader.name.Contains(SHADER_OUTLINE))
                        mat.shader = defaultOutlineShader;
                }
                else
                {
                    if (mat.shader.name.Contains(SHADER_OUTLINE))
                        mat.shader = defaultShader;
                }
                mat.shaderKeywords = kws;
            }
            catch { }
        }

        //Colorfield
        private void ColorProperty(MaterialProperty prop, bool showAlpha, bool hdrEnabled, GUIContent label)
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            Color c = EditorGUILayout.ColorField(label, prop.colorValue, false, showAlpha, hdrEnabled, colorPickerHDRConfig);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                prop.colorValue = c;
        }

        //Setup GI emission
        private void SetGIFlags()
        {
            foreach (Material obj in lightModel.targets)
            {
                bool emissive = true;
                if(MKToonMaterialHelper.GetEmissionColor(obj) == Color.black)
                {
                    emissive = false;
                }
                MaterialGlobalIlluminationFlags flags = obj.globalIlluminationFlags;
                if ((flags & (MaterialGlobalIlluminationFlags.BakedEmissive | MaterialGlobalIlluminationFlags.RealtimeEmissive)) != 0)
                {
                    flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                    if (!emissive)
                        flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                    obj.globalIlluminationFlags = flags;
                }
            }
        }

        internal static void SetMaterialTags(Material material, BlendMode blend)
        {
            material.DisableKeyword(BLEND_MODE[0]);
            material.DisableKeyword(BLEND_MODE[1]);
            switch (blend)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", "Opaque");
                    material.SetOverrideTag("Queue", "Geometry");
                    material.SetOverrideTag("IgnoreProjector", "false");
                    material.SetInt(MKToonMaterialHelper.PropertyNames.Z_WRITE, 1);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetOverrideTag("Queue", "Transparent");
                    material.SetOverrideTag("IgnoreProjector", "true");
                    material.SetInt(MKToonMaterialHelper.PropertyNames.Z_WRITE, 1);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.EnableKeyword(BLEND_MODE[0]);
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetOverrideTag("Queue", "AlphaTest");
                    material.SetOverrideTag("IgnoreProjector", "true");
                    material.SetInt(MKToonMaterialHelper.PropertyNames.Z_WRITE, 1);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    material.EnableKeyword(BLEND_MODE[1]);
                    break;
            }
        }

        private static void SetBlendMode(Material material, BlendMode mode)
        {
            switch (mode)
            {
                case BlendMode.Opaque:
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_0, (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_1, (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_MODE, (int)BlendMode.Opaque);
                    break;
                case BlendMode.Transparent:
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_0, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_1, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_MODE, (int)BlendMode.Transparent);
                    break;
                case BlendMode.Cutout:
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_0, (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_1, (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt(MKToonMaterialHelper.PropertyNames.BLEND_MODE, (int)BlendMode.Cutout);
                    break;
            }
            SetMaterialTags(material, mode);
        }

        //BoldToggle
        private void ToggleBold(MaterialEditor materialEditor, MaterialProperty prop)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(prop.displayName, EditorStyles.boldLabel, GUILayout.Width(100));
            materialEditor.ShaderProperty(prop, "");
            EditorGUILayout.EndHorizontal();
        }

        //Lightmodel
        private void LightModelPopup(MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = lightModel.hasMixedValue;
            LightModel lm = new LightModel();
            lm = (LightModel)lightModel.floatValue;
            EditorGUI.BeginChangeCheck();
            lm = (LightModel)EditorGUILayout.EnumPopup(GUIContentCollection.lightModel, lm);
            if (EditorGUI.EndChangeCheck())
            {
                lightModel.floatValue = (float)lm;
                materialEditor.RegisterPropertyChangeUndo("Lightmodel");
                foreach (Material mat in lightModel.targets)
                {
                    mat.SetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL, lightModel.floatValue);
                }
                UpdateKeywords(KeywordsToManage.LIGHT_MODEL);
            }
            EditorGUI.showMixedValue = false;
        }

        //LightType
        private void LightTypePopup(MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = lightType.hasMixedValue;
            LightType lt = new LightType();
            lt = (LightType)lightType.floatValue;
            EditorGUI.BeginChangeCheck();
            lt = (LightType)EditorGUILayout.EnumPopup(GUIContentCollection.lightType, lt);
            if (EditorGUI.EndChangeCheck())
            {
                lightType.floatValue = (float)lt;
                materialEditor.RegisterPropertyChangeUndo("Light type");
                foreach (Material mat in lightType.targets)
                {
                    mat.SetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE, lightType.floatValue);
                }
                UpdateKeywords(KeywordsToManage.LIGHT_TYPE);
            }
            EditorGUI.showMixedValue = false;
        }

        //Blendmode
        private void BlendModePopup(MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = blendMode.hasMixedValue;

            EditorGUI.BeginChangeCheck();
            materialEditor.ShaderProperty(blendMode, GUIContentCollection.blendMode);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                foreach (var obj in blendMode.targets)
                {
                    SetBlendMode((Material)obj, (BlendMode)blendMode.floatValue);
                }
            }
            EditorGUI.showMixedValue = false;
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material.HasProperty(MKToonMaterialHelper.PropertyNames.EMISSION))
            {
                MKToonMaterialHelper.SetEmissionColor(material, material.GetColor(MKToonMaterialHelper.PropertyNames.EMISSION));
            }
            BlendMode newBlend;
            if (material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.AlphaTest)
            {
                newBlend = BlendMode.Cutout;
            }
            else if (material.renderQueue == (int)UnityEngine.Rendering.RenderQueue.Transparent)
            {
                newBlend = BlendMode.Transparent;
            }
            else
            {
                newBlend = BlendMode.Opaque;
            }
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            MaterialProperty[] properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
            FindProperties(properties, material);

            SetBlendMode(material, newBlend);
            UpdateKeywords(KeywordsToManage.ALL);
            SetGIFlags();
        }

        private bool HandleBehavior(string title, ref MaterialProperty behavior, MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = behavior.hasMixedValue;
            var rect = GUILayoutUtility.GetRect(16f, 22f, GuiStyles.header);
            rect.x -= 10;
            rect.width += 10;
            var e = Event.current;

            GUI.Box(rect, title, GuiStyles.header);

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y + 2, 13f, 13f);
            if (behavior.hasMixedValue)
            {
                foldoutRect.x -= 13;
                foldoutRect.y -= 2;
            }

            EditorGUI.BeginChangeCheck();
            if (e.type == EventType.MouseDown)
            {
                if (rect.Contains(e.mousePosition))
                {
                    if (behavior.hasMixedValue)
                        behavior.floatValue = 0.0f;
                    else
                        behavior.floatValue = Convert.ToSingle(!Convert.ToBoolean(behavior.floatValue));
                    e.Use();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Convert.ToBoolean(behavior.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Show");
                else
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Hide");
            }

            EditorGUI.showMixedValue = false;

            if (e.type == EventType.Repaint && behavior.hasMixedValue)
                EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, Convert.ToBoolean(behavior.floatValue), "");

            if (behavior.hasMixedValue)
                return true;
            else
                return Convert.ToBoolean(behavior.floatValue);
        }

        private bool HandleBehavior(string title, ref MaterialProperty behavior, ref MaterialProperty feature, MaterialEditor materialEditor, string featureName)
        {
            var rect = GUILayoutUtility.GetRect(16f, 22f, GuiStyles.header);
            rect.x -= 10;
            rect.width += 10;
            var e = Event.current;

            GUI.Box(rect, title, GuiStyles.header);

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y + 2, 13f, 13f);
            if (behavior.hasMixedValue)
            {
                foldoutRect.x -= 13;
                foldoutRect.y -= 2;
            }

            EditorGUI.showMixedValue = feature.hasMixedValue;
            var toggleRect = new Rect(rect.x + 4f, rect.y + ((feature.hasMixedValue) ? 0.0f : 4.0f), 13f, 13f);
            bool fn = Convert.ToBoolean(feature.floatValue);
            EditorGUI.BeginChangeCheck();

            fn = EditorGUI.Toggle(toggleRect, "", fn, GuiStyles.headerCheckbox);

            if (EditorGUI.EndChangeCheck())
            {
                feature.floatValue = Convert.ToSingle(fn);
                if (Convert.ToBoolean(feature.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(feature.displayName + " enabled");
                else
                    materialEditor.RegisterPropertyChangeUndo(feature.displayName + " disabled");
                foreach (Material mat in feature.targets)
                {
                    mat.SetFloat(featureName, feature.floatValue);
                }
            }
            EditorGUI.showMixedValue = false;

            EditorGUI.showMixedValue = behavior.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            if (e.type == EventType.MouseDown)
            {
                if (rect.Contains(e.mousePosition))
                {
                    if (behavior.hasMixedValue)
                        behavior.floatValue = 0.0f;
                    else
                        behavior.floatValue = Convert.ToSingle(!Convert.ToBoolean(behavior.floatValue));
                    e.Use();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Convert.ToBoolean(behavior.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " show");
                else
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " hide");
            }

            EditorGUI.showMixedValue = false;

            if (e.type == EventType.Repaint && behavior.hasMixedValue)
                EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, Convert.ToBoolean(behavior.floatValue), "");

            if (behavior.hasMixedValue)
                return true;
            else
                return Convert.ToBoolean(behavior.floatValue);
        }

        override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;
            //get properties
            FindProperties(properties, targetMat);

            if (emissionColor.colorValue != Color.black)
                showGIField = true;
            else
                showGIField = false;

            EditorGUI.BeginChangeCheck();
            //main settings
            if (HandleBehavior("Main", ref showMainBehavior, materialEditor))
            {
                EditorGUI.BeginChangeCheck();

                if (blendMode.floatValue == (int)BlendMode.Opaque)
                    ColorProperty(mainColor, false, false, GUIContentCollection.mainColor);
                else
                    ColorProperty(mainColor, true, false, GUIContentCollection.mainColor);

                EditorGUI.BeginChangeCheck();
                materialEditor.TexturePropertySingleLine(GUIContentCollection.mainTex, mainTex);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.COLOR_SOURCE);
                }

                if (blendMode.floatValue == (int)BlendMode.Cutout)
                    materialEditor.ShaderProperty(cutoff, GUIContentCollection.alphaCutoff);

                EditorGUI.BeginChangeCheck();
                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    if (normalMap.textureValue == null)
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.normalMap, normalMap);
                    else
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.normalMap, normalMap, bumpScale);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.BUMP);
                }

                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    EditorGUI.BeginChangeCheck();
                    materialEditor.TexturePropertyWithHDRColor(GUIContentCollection.emission, emissionTex, emissionColor, colorPickerHDRConfig, false);
                    if (showGIField)
                        materialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdateKeywords(KeywordsToManage.EMISSION);
                        SetGIFlags();
                    }
                }
                materialEditor.TextureScaleOffsetProperty(mainTex);
            }

            //Detail
            if (HandleBehavior("Detail", ref showDetailBehavior, materialEditor))
            {
                EditorGUI.BeginChangeCheck();
                if (detailAlbedoMap.textureValue != null)
                    ColorProperty(detailColor, false, false, GUIContentCollection.detailColor);
                if (detailAlbedoMap.textureValue == null)
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailTex, detailAlbedoMap);
                else
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailTex, detailAlbedoMap, detailTint);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.DETAIL_ALBEDO);
                }
                EditorGUI.BeginChangeCheck();
                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    if (detailNormalMap.textureValue == null)
                    {
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.detailNormalMap, detailNormalMap);
                    }
                    else
                    {
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.detailNormalMap, detailNormalMap, detailNormalMapScale);
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.DETAIL_BUMP);
                }
                materialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
            }

            //light settings
            if (HandleBehavior("Light", ref showLightBehavior, materialEditor))
            {
                LightModelPopup(materialEditor);
                if (lightModel.floatValue == (int)(LightModel.Unlit))
                {
                    EditorGUILayout.HelpBox("To completly unlit, please disable shadow casting & recieving on the MeshRenderer", MessageType.Info);
                }
                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    LightTypePopup(materialEditor);
                    if (lightType.floatValue == (int)(LightType.Cel_Shade_Simple))
                        materialEditor.ShaderProperty(lTreshold, GUIContentCollection.threshold);
                    if (lightType.floatValue == (int)(LightType.Cel_Shade_Multi))
                    {
                        materialEditor.ShaderProperty(lightLevels, GUIContentCollection.lightLevels);
                        materialEditor.ShaderProperty(lTreshold, GUIContentCollection.threshold);
                    }
                    if (lightType.floatValue == (int)(LightType.Ramp))
                    {
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.rampTex, rampTex);
                    }
                    if (lightModel.floatValue == (int)(LightModel.Minneart) || lightModel.floatValue == (int)(LightModel.Oren_Nayer))
                        materialEditor.ShaderProperty(roughness, GUIContentCollection.roughness);
                    if (lightType.floatValue != (int)(LightType.Ramp))
                        materialEditor.ShaderProperty(lightSmoothness, GUIContentCollection.lightSmoothness);
                    EditorGUI.BeginChangeCheck();
                    if (occlusionMap.textureValue == null)
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.occlusion, occlusionMap);
                    else
                    {
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.occlusion, occlusionMap, occlusionStrength);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdateKeywords(KeywordsToManage.OCCLUSION);
                    }
                }
            }

            //custom shadow settings
            if (lightModel.floatValue != (int)(LightModel.Unlit))
            {
                if (HandleBehavior("Shadow", ref showShadowBehavior, materialEditor))
                {
                    ColorProperty(highlightColor, false, false, GUIContentCollection.highlightColor);
                    ColorProperty(shadowColor, false, false, GUIContentCollection.shadowColor);
                    materialEditor.ShaderProperty(shadowIntensity, GUIContentCollection.shadowIntensity);
                }
            }

            //render settings
            if (HandleBehavior("Render", ref showRenderBehavior, materialEditor))
            {
                materialEditor.EnableInstancingField();
                BlendModePopup(materialEditor);
                materialEditor.ShaderProperty(cullMode, GUIContentCollection.cullMode);
                materialEditor.ShaderProperty(contrast, GUIContentCollection.contrast);
                materialEditor.ShaderProperty(saturation, GUIContentCollection.saturation);
                materialEditor.ShaderProperty(brightness, GUIContentCollection.brightness);
            }

            //specular settings
            if (lightModel.floatValue == (int)(LightModel.Blinn_Phong) || lightModel.floatValue == (int)(LightModel.Phong))
            {
                if (HandleBehavior("Specular", ref showSpecularBehavior, materialEditor))
                {
                    ColorProperty(specularColor, false, false, GUIContentCollection.specularColor);
                    materialEditor.ShaderProperty(shininess, GUIContentCollection.specularShininess);
                    EditorGUI.BeginChangeCheck();
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.specular, specularMap, specularIntensity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdateKeywords(KeywordsToManage.SPECULAR);
                    }

                    if (lightModel.floatValue == (int)(LightModel.Blinn_Phong))
                    {
                        EditorGUI.BeginChangeCheck();
                        if (anisoMap.textureValue == null)
                            materialEditor.TexturePropertySingleLine(GUIContentCollection.anisoMap, anisoMap);
                        else
                            materialEditor.TexturePropertySingleLine(GUIContentCollection.anisoMap, anisoMap, anisoOffset);
                        if (EditorGUI.EndChangeCheck())
                        {
                            UpdateKeywords(KeywordsToManage.ANISOTROPIC_SPECULAR);
                        }
                    }
                }
            }

            //translucent settings
            if (lightModel.floatValue == (int)(LightModel.Blinn_Phong) || lightModel.floatValue == (int)(LightModel.Phong))
            {
                EditorGUI.BeginChangeCheck();
                if (HandleBehavior("Translucent", ref showTranslucentBehavior, ref useTranslucent, materialEditor, MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT))
                {
                    ColorProperty(translucentColor, false, false, GUIContentCollection.translucentColor);
                    materialEditor.ShaderProperty(translucentShininess, GUIContentCollection.translucentShininess);
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.translucent, translucentMap, translucentIntensity);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.TRANSLUCENT);
                }
            }

            //rim settings
            if (lightModel.floatValue != (int)(LightModel.Unlit))
            {
                EditorGUI.BeginChangeCheck();
                if (HandleBehavior("Rim", ref showRimBehavior, ref useRim, materialEditor, MKToonMaterialHelper.PropertyNames.USE_RIM))
                {
                    ColorProperty(rimColor, false, false, GUIContentCollection.rimColor);
                    materialEditor.ShaderProperty(rimSize, GUIContentCollection.rimSize);
                    materialEditor.ShaderProperty(rimIntensity, GUIContentCollection.rimIntensity);
                    materialEditor.ShaderProperty(rimSmoothness, GUIContentCollection.rimSmoothness);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.RIM);
                }
            }

            //reflection settings
            if (lightModel.floatValue != (int)(LightModel.Unlit))
            {
                EditorGUI.BeginChangeCheck();
                if (HandleBehavior("Reflection", ref showReflectionBehavior, ref useReflection, materialEditor, MKToonMaterialHelper.PropertyNames.USE_REFLECTION))
                {
                    ColorProperty(reflectionColor, false, false, GUIContentCollection.reflectColor);
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.reflection, reflectMap, reflectIntensity);
                    materialEditor.ShaderProperty(reflectSmoothness, GUIContentCollection.reflectSmoothness);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateKeywords(KeywordsToManage.REFLECTIVE);
                }
            }

            //Dissolve settings
            EditorGUI.BeginChangeCheck();
            if (HandleBehavior("Dissolve", ref showDissolveBehavior, ref useDissolve, materialEditor, MKToonMaterialHelper.PropertyNames.USE_DISSOLVE))
            {
                ColorProperty(dissolveColor, false, false, GUIContentCollection.dissolveColor);
                if (dissolveMap.textureValue == null)
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.dissolveMap, dissolveMap);
                else
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.dissolveMap, dissolveMap, dissolveAmount);
                if (dissolveMap.textureValue != null)
                {
                    if (dissolveRamp.textureValue == null)
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.dissolveRamp, dissolveRamp);
                    else
                        materialEditor.TexturePropertySingleLine(GUIContentCollection.dissolveRamp, dissolveRamp, dissolveSize);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                UpdateKeywords(KeywordsToManage.DISSOLVE);
            }

            EditorGUI.BeginChangeCheck();
            if (HandleBehavior("Sketch", ref showSketchBehavior, ref useSketch, materialEditor, MKToonMaterialHelper.PropertyNames.USE_SKETCH))
            {
                if (sketchMap.textureValue == null)
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.sketchmap, sketchMap);
                else
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.sketchmap, sketchMap, sketchTint);
                if (sketchMap.textureValue != null)
                {
                    if (lightModel.floatValue != (int)(LightModel.Unlit))
                        materialEditor.ShaderProperty(sketchToneMin, GUIContentCollection.sketchToneMin);
                    materialEditor.ShaderProperty(sketchToneMax, GUIContentCollection.sketchToneMax);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                UpdateKeywords(KeywordsToManage.SKETCH);
            }

            //set outline
            EditorGUI.BeginChangeCheck();
            if (HandleBehavior("Outline", ref showOutlineBehavior, ref useOutline, materialEditor, MKToonMaterialHelper.PropertyNames.USE_OUTLINE))
            {
                if (blendMode.floatValue == (int)BlendMode.Opaque)
                    ColorProperty(outlineColor, false, false, GUIContentCollection.outlineColor);
                else
                    ColorProperty(outlineColor, true, false, GUIContentCollection.outlineColor);
                materialEditor.ShaderProperty(outlineSize, GUIContentCollection.outlineSize);
            }
            if (EditorGUI.EndChangeCheck())
            {
                ManageVariantsInternal();
            }

            EditorGUI.EndChangeCheck();
        }

        private void ManageVariantsInternal()
        {
            //ManageOutline
            foreach (Material mat in useOutline.targets)
            {
                if (mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_OUTLINE) == 1.0f)
                {
                    if (!mat.shader.name.Contains(SHADER_OUTLINE))
                    {
                        AssignNewShaderToMaterial(mat, defaultShader, defaultOutlineShader);
                    }
                }
                else
                {
                    if (mat.shader.name.Contains(SHADER_OUTLINE))
                    {
                        AssignNewShaderToMaterial(mat, defaultOutlineShader, defaultShader);
                    }
                }
            }
        }

        private void ManageKeywordsColorSource()
        {
            //Colorsource
            foreach (Material mat in mainTex.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetMainTexture(mat), ALBEDO_MAP, mat);
            }
        }

        private void ManageKeywordsLightModel()
        {
            //Lightmodel
            foreach (Material mat in lightModel.targets)
            {
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 1.0f, LIGHT_MODEL[1], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f, LIGHT_MODEL[2], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, LIGHT_MODEL[3], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 4.0f, LIGHT_MODEL[4], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 5.0f, LIGHT_MODEL[5], mat);
            }
        }

        private void ManageKeywordsLightType()
        {
            //Lighttype
            foreach (Material mat in lightType.targets)
            {
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 1.0f, LIGHT_TYPE[1], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 2.0f, LIGHT_TYPE[2], mat);
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 3.0f, LIGHT_TYPE[3], mat);
            }
        }

        private void ManageKeywordsOcclusion()
        {
            //Occlusion
            foreach (Material mat in occlusionMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetOcclusionMap(mat), OCCLUSION, mat);
            }
        }

        private void ManageKeywordsRim()
        {
            //Rim
            foreach (Material mat in useRim.targets)
            {
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_RIM) == 1.0f, RIM, mat);
            }
        }

        private void ManageKeywordsDetailAlbedo()
        {
            //detail albedo
            foreach (Material mat in detailAlbedoMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetDetailTexture(mat), DETAIL_MAP, mat);
            }
        }

        private void ManageKeywordsDetailBump()
        {
            //detail bump
            foreach (Material mat in detailNormalMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetDetailNormalMap(mat), DETAIL_BUMP_MAP, mat);
            }
        }

        private void ManageKeywordsBump()
        {
            //Bumpmap
            foreach (Material mat in normalMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetBumpMap(mat), BUMP_MAP, mat);
            }
        }

        private void ManageKeywordsSpecular()
        {
            //spec
            foreach (Material mat in specularMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetSpecularMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f || mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, SPECULAR_MAP, mat);
            }
        }

        private void ManageKeywordsAnisoSpecular()
        {
            //aniso
            foreach (Material mat in anisoMap.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetAnisoMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, ANISOTROPIC, mat);
            }
        }

        private void ManageKeywordsEmission()
        {
            //emission
            foreach (Material mat in emissionColor.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetEmissionMap(mat) != null && MKToonMaterialHelper.GetEmissionColor(mat) != Color.black, EMISSION_MAP, mat);
            }
            foreach (Material mat in emissionColor.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetEmissionMap(mat) == null && MKToonMaterialHelper.GetEmissionColor(mat) != Color.black, EMISSION_DEFAULT, mat);
            }
        }

        private void ManageKeywordsReflective()
        {
            //Reflective
            foreach (Material mat in useReflection.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetReflectMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, REFLECTIVE_MAP, mat);
            }
            foreach (Material mat in useReflection.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetReflectMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, REFLECTIVE_DEFAULT, mat);
            }
        }

        private void ManageKeywordsTranslucent()
        {
            //Translucent
            foreach (Material mat in useTranslucent.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetTranslucentMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, TRANSLUCENT_DEFAULT, mat);
            }
            foreach (Material mat in useTranslucent.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetTranslucentMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, TRANSLUCENT_MAP, mat);
            }
        }

        private void ManageKeywordsDissolve()
        {
            //Dissolve
            foreach (Material mat in useDissolve.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetDissolveMap(mat) != null && MKToonMaterialHelper.GetDissolveRampMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_DISSOLVE) == 1.0f, DISSOLVE_DEFAULT, mat);
            }
            foreach (Material mat in useDissolve.targets)
            {
                SetKeyword(MKToonMaterialHelper.GetDissolveMap(mat) != null && MKToonMaterialHelper.GetDissolveRampMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_DISSOLVE) == 1.0f, DISSOLVE_RAMP, mat);
            }
        }

        private void ManageKeywordsSketch()
        {
            //Sketch
            foreach (Material mat in useSketch.targets)
            {
                SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_SKETCH) == 1.0f, SKETCH, mat);
            }
        }

        private void UpdateKeywords(KeywordsToManage kw)
        {
            switch (kw)
            {
                case KeywordsToManage.ALL:
                    ManageKeywordsBump();
                    ManageKeywordsColorSource();
                    ManageKeywordsDetailAlbedo();
                    ManageKeywordsDetailBump();
                    ManageKeywordsEmission();
                    ManageKeywordsLightModel();
                    ManageKeywordsLightType();
                    ManageKeywordsReflective();
                    ManageKeywordsRim();
                    ManageKeywordsSpecular();
                    ManageKeywordsAnisoSpecular();
                    ManageKeywordsTranslucent();
                    ManageKeywordsDissolve();
                    ManageKeywordsOcclusion();
                    ManageKeywordsSketch();
                    ManageVariantsInternal();
                    break;
                case KeywordsToManage.BUMP:
                    ManageKeywordsBump();
                    break;
                case KeywordsToManage.COLOR_SOURCE:
                    ManageKeywordsColorSource();
                    break;
                case KeywordsToManage.DETAIL_ALBEDO:
                    ManageKeywordsDetailAlbedo();
                    break;
                case KeywordsToManage.DETAIL_BUMP:
                    ManageKeywordsDetailBump();
                    break;
                case KeywordsToManage.EMISSION:
                    ManageKeywordsEmission();
                    break;
                case KeywordsToManage.LIGHT_MODEL:
                    ManageKeywordsLightModel();
                    break;
                case KeywordsToManage.LIGHT_TYPE:
                    ManageKeywordsLightType();
                    break;
                case KeywordsToManage.REFLECTIVE:
                    ManageKeywordsReflective();
                    break;
                case KeywordsToManage.RIM:
                    ManageKeywordsRim();
                    break;
                case KeywordsToManage.SPECULAR:
                    ManageKeywordsSpecular();
                    break;
                case KeywordsToManage.ANISOTROPIC_SPECULAR:
                    ManageKeywordsAnisoSpecular();
                    break;
                case KeywordsToManage.TRANSLUCENT:
                    ManageKeywordsTranslucent();
                    break;
                case KeywordsToManage.DISSOLVE:
                    ManageKeywordsDissolve();
                    break;
                case KeywordsToManage.OCCLUSION:
                    ManageKeywordsOcclusion();
                    break;
                case KeywordsToManage.SKETCH:
                    ManageKeywordsSketch();
                    break;
            }
        }

        private static void SetKeyword(bool enable, string keyword, Material mat)
        {
            if(enable)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
        }

        private void Divider()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }
    }
}
#endif