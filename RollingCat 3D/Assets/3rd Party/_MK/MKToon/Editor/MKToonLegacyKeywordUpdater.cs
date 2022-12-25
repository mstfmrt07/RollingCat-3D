using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

#if UNITY_EDITOR
namespace MK.Toon
{
    class AutoUpdateKeywords : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Contains("Assets/_MK/MKToon/Editor/MKToonLegacyKeywordUpdater.cs"))
            {
                MKToonLegacyKeywordUpdater.StartKeywordUpdater();
                //AssetDatabase.RenameAsset("Assets/_MK/MKToon/Editor/MKToonLegacyKeywordUpdater.cs", "MKToonLegacyKeywordUpdater_0");
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();
            }
        }
    }

    public class MKToonLegacyKeywordUpdater : EditorWindow
    {
       [MenuItem("Window/MK/Toon/Update Legacy Keywords")]
        public static void StartKeywordUpdater()
        {
            EditorUtility.DisplayProgressBar("MK Toon Legacy Keywords Updater", "Updating legacy keywords...", 100);
            UpdateKeywords();
            EditorUtility.ClearProgressBar();
        }

        static List<string> materials = new List<string>();

        static void OnAssetFileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            StartKeywordUpdater();
        }

        private static bool CheckIfAnyLegacyKeywordsOnMaterial(Material mat)
        {
            string[] kws = mat.shaderKeywords;
            if (kws.Contains(BLEND_OPAQUE_LEGACY) || kws.Contains(BLEND_TRANSPARENT_LEGACY) || kws.Contains(BLEND_CUTOUT_LEGACY) ||
                kws.Contains(BUMP_MAP_LEGACY) || kws.Contains(RIM_LEGACY) || kws.Contains(ANISOTROPIC_LEGACY) ||
                kws.Contains(LIGHTTYPE_DEFAULT_LEGACY) || kws.Contains(LIGHTTYPE_CEL_SHADE_SIMPLE_LEGACY) || kws.Contains(LIGHTTYPE_CEL_SHADE_MULTI_LEGACY) ||
                kws.Contains(LIGHTTYPE_RAMP_LEGACY) || kws.Contains(LIGHTMODEL_UNLIT_LEGACY) || kws.Contains(LIGHTMODEL_LAMBERT_LEGACY) ||
                kws.Contains(LIGHTMODEL_PHONG_LEGACY) || kws.Contains(LIGHTMODEL_BLINN_PHONG_LEGACY) || kws.Contains(LIGHTMODEL_MINNAERT_LEGACY) ||
                kws.Contains(LIGHTMODEL_OREN_NAYER_LEGACY) || kws.Contains(MODE_OPAQUE_LEGACY) || kws.Contains(MODE_TRANSPARENT_LEGACY) ||
                kws.Contains(MODE_CUTOUT_LEGACY) || kws.Contains(EMISSION_DEFAULT_LEGACY) || kws.Contains(EMISSION_MAP_LEGACY) ||
                kws.Contains(COLORSOURCE_MAP_LEGACY) || kws.Contains(REFLECTIVE_MAP_LEGACY) || kws.Contains(REFLECTIVE_DEFAULT_LEGACY) ||
                kws.Contains(DISSOLVE_DEFAULT_LEGACY) || kws.Contains(DISSOLVE_RAMP_LEGACY) || kws.Contains(TRANSLUCENT_DEFAULT_LEGACY) ||
                kws.Contains(TRANSLUCENT_MAP_LEGACY) || kws.Contains(SPECULAR_MAP_LEGACY))
            {
                return true;
            }
            return false;
        }

        //legacy
        private const string BLEND_OPAQUE_LEGACY = "_MODE_OPAQUE";
        private const string BLEND_TRANSPARENT_LEGACY = "_MODE_TRANSPARENT";
        private const string BLEND_CUTOUT_LEGACY = "_MODE_CUTOUT";

        private const string BUMP_MAP_LEGACY = "_MKTOON_BUMP_MAP";
        private const string RIM_LEGACY = "_MKTOON_RIM";
        private const string ANISOTROPIC_LEGACY = "_MKTOON_ANISOTROPIC_SPECULAR";

        private const string LIGHTTYPE_DEFAULT_LEGACY = "_LIGHTTYPE_DEFAULT";
        private const string LIGHTTYPE_CEL_SHADE_SIMPLE_LEGACY = "_LIGHTTYPE_CEL_SHADE_SIMPLE";
        private const string LIGHTTYPE_CEL_SHADE_MULTI_LEGACY = "_LIGHTTYPE_CEL_SHADE_MULTI";
        private const string LIGHTTYPE_RAMP_LEGACY = "_LIGHTTYPE_RAMP";

        private const string LIGHTMODEL_UNLIT_LEGACY = "_LIGHTMODEL_UNLIT";
        private const string LIGHTMODEL_LAMBERT_LEGACY = "_LIGHTMODEL_LAMBERT";
        private const string LIGHTMODEL_PHONG_LEGACY = "_LIGHTMODEL_PHONG";
        private const string LIGHTMODEL_BLINN_PHONG_LEGACY = "_LIGHTMODEL_BLINN_PHONG";
        private const string LIGHTMODEL_MINNAERT_LEGACY = "_LIGHTMODEL_MINNAERT";
        private const string LIGHTMODEL_OREN_NAYER_LEGACY = "_LIGHTMODEL_OREN_NAYER";

        private const string MODE_OPAQUE_LEGACY = "_MODE_OPAQUE";
        private const string MODE_TRANSPARENT_LEGACY = "_MODE_TRANSPARENT";
        private const string MODE_CUTOUT_LEGACY = "_MODE_CUTOUT";

        private const string EMISSION_DEFAULT_LEGACY = "_MKTOON_EMISSION_DEFAULT";
        private const string EMISSION_MAP_LEGACY = "_MKTOON_EMISSION_MAP";
        private const string COLORSOURCE_MAP_LEGACY = "_MKTOON_COLOR_SOURCE_MAP";
        private const string REFLECTIVE_MAP_LEGACY = "_MKTOON_REFLECTIVE_MAP";
        private const string REFLECTIVE_DEFAULT_LEGACY = "_MKTOON_REFLECTIVE_DEFAULT";
        private const string DISSOLVE_DEFAULT_LEGACY = "_MKTOON_DISSOLVE_DEFAULT";
        private const string DISSOLVE_RAMP_LEGACY = "_MKTOON_DISSOLVE_RAMP";
        private const string TRANSLUCENT_DEFAULT_LEGACY = "_MKTOON_TRANSLUCENT_DEFAULT";
        private const string TRANSLUCENT_MAP_LEGACY = "_MKTOON_TRANSLUCENT_MAP";
        private const string SPECULAR_MAP_LEGACY = "_MKTOON_SPECULAR_MAP";
        //legacy end

        static void UpdateKeywords()
        {
            string shaderPathDefault = AssetDatabase.GetAssetPath(Shader.Find("MK/Toon/Default"));
            string shaderPathOutline = AssetDatabase.GetAssetPath(Shader.Find("Hidden/MK/Toon/Default Outline"));
            string[] allMaterials = AssetDatabase.FindAssets("t:Material");
            materials.Clear();
            for (int i = 0; i < allMaterials.Length; i++)
            {
                allMaterials[i] = AssetDatabase.GUIDToAssetPath(allMaterials[i]);
                string[] dep = AssetDatabase.GetDependencies(allMaterials[i]);
                if (ArrayUtility.Contains(dep, shaderPathDefault) || ArrayUtility.Contains(dep, shaderPathOutline))
                    materials.Add(allMaterials[i]);
            }
            for (int i = 0; i < materials.Count; i++)
            {
                Material mat = (Material)AssetDatabase.LoadAssetAtPath(materials[i], typeof(Material));
                if (CheckIfAnyLegacyKeywordsOnMaterial(mat))
                {
                    //Disable old Keywords
                    SetKeyword(false, BLEND_OPAQUE_LEGACY, mat);
                    SetKeyword(false, BLEND_TRANSPARENT_LEGACY, mat);
                    SetKeyword(false, BLEND_CUTOUT_LEGACY, mat);

                    SetKeyword(false, BUMP_MAP_LEGACY, mat);
                    SetKeyword(false, RIM_LEGACY, mat);
                    SetKeyword(false, ANISOTROPIC_LEGACY, mat);

                    SetKeyword(false, LIGHTTYPE_DEFAULT_LEGACY, mat);
                    SetKeyword(false, LIGHTTYPE_CEL_SHADE_SIMPLE_LEGACY, mat);
                    SetKeyword(false, LIGHTTYPE_CEL_SHADE_MULTI_LEGACY, mat);
                    SetKeyword(false, LIGHTTYPE_RAMP_LEGACY, mat);

                    SetKeyword(false, LIGHTMODEL_UNLIT_LEGACY, mat);
                    SetKeyword(false, LIGHTMODEL_LAMBERT_LEGACY, mat);
                    SetKeyword(false, LIGHTMODEL_PHONG_LEGACY, mat);
                    SetKeyword(false, LIGHTMODEL_BLINN_PHONG_LEGACY, mat);
                    SetKeyword(false, LIGHTMODEL_MINNAERT_LEGACY, mat);
                    SetKeyword(false, LIGHTMODEL_OREN_NAYER_LEGACY, mat);

                    SetKeyword(false, MODE_OPAQUE_LEGACY, mat);
                    SetKeyword(false, MODE_TRANSPARENT_LEGACY, mat);
                    SetKeyword(false, MODE_CUTOUT_LEGACY, mat);

                    SetKeyword(false, EMISSION_DEFAULT_LEGACY, mat);
                    SetKeyword(false, EMISSION_MAP_LEGACY, mat);
                    SetKeyword(false, COLORSOURCE_MAP_LEGACY, mat);
                    SetKeyword(false, REFLECTIVE_MAP_LEGACY, mat);
                    SetKeyword(false, REFLECTIVE_DEFAULT_LEGACY, mat);
                    SetKeyword(false, DISSOLVE_DEFAULT_LEGACY, mat);
                    SetKeyword(false, DISSOLVE_RAMP_LEGACY, mat);

                    SetKeyword(false, TRANSLUCENT_DEFAULT_LEGACY, mat);
                    SetKeyword(false, TRANSLUCENT_MAP_LEGACY, mat);
                    SetKeyword(false, SPECULAR_MAP_LEGACY, mat);

                    //Blend
                    MKToonEditor.SetMaterialTags(mat, (MKToonEditor.BlendMode)mat.GetFloat(MKToonMaterialHelper.PropertyNames.BLEND_MODE));

                    //colorsource
                    SetKeyword(MKToonMaterialHelper.GetMainTexture(mat), MKToonEditor.ALBEDO_MAP, mat);

                    //lightmodel
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 1.0f, MKToonEditor.LIGHT_MODEL[1], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f, MKToonEditor.LIGHT_MODEL[2], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, MKToonEditor.LIGHT_MODEL[3], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 4.0f, MKToonEditor.LIGHT_MODEL[4], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 5.0f, MKToonEditor.LIGHT_MODEL[5], mat);

                    //lighttype
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 1.0f, MKToonEditor.LIGHT_TYPE[1], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 2.0f, MKToonEditor.LIGHT_TYPE[2], mat);
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_TYPE) == 3.0f, MKToonEditor.LIGHT_TYPE[3], mat);

                    //occlusion
                    //SetKeyword(MKToonMaterialHelper.GetOcclusionMap(mat), MKToonEditor.OCCLUSION, mat);

                    //Rim
                    SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_RIM) == 1.0f, MKToonEditor.RIM, mat);

                    //detail Albedo
                    //SetKeyword(MKToonMaterialHelper.GetDetailTexture(mat), MKToonEditor.DETAIL_MAP, mat);

                    //bumpdetail
                    SetKeyword(MKToonMaterialHelper.GetDetailNormalMap(mat), MKToonEditor.DETAIL_BUMP_MAP, mat);

                    //bumpmap
                    SetKeyword(MKToonMaterialHelper.GetBumpMap(mat), MKToonEditor.BUMP_MAP, mat);

                    //spec
                    SetKeyword(MKToonMaterialHelper.GetSpecularMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f || mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, MKToonEditor.SPECULAR_MAP, mat);

                    //aniso
                    SetKeyword(MKToonMaterialHelper.GetAnisoMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, MKToonEditor.ANISOTROPIC, mat);

                    //Emission
                    SetKeyword(MKToonMaterialHelper.GetEmissionMap(mat) != null && MKToonMaterialHelper.GetEmissionColor(mat) != Color.black, MKToonEditor.EMISSION_MAP, mat);
                    SetKeyword(MKToonMaterialHelper.GetEmissionMap(mat) == null && MKToonMaterialHelper.GetEmissionColor(mat) != Color.black, MKToonEditor.EMISSION_DEFAULT, mat);

                    //Reflective
                    SetKeyword(MKToonMaterialHelper.GetReflectMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, MKToonEditor.REFLECTIVE_MAP, mat);
                    SetKeyword(MKToonMaterialHelper.GetReflectMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, MKToonEditor.REFLECTIVE_DEFAULT, mat);

                    //translucent
                    SetKeyword(MKToonMaterialHelper.GetTranslucentMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, MKToonEditor.TRANSLUCENT_DEFAULT, mat);
                    SetKeyword(MKToonMaterialHelper.GetTranslucentMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, MKToonEditor.TRANSLUCENT_MAP, mat);

                    //Dissolve
                    SetKeyword(MKToonMaterialHelper.GetDissolveMap(mat) != null && MKToonMaterialHelper.GetDissolveRampMap(mat) == null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_DISSOLVE) == 1.0f, MKToonEditor.DISSOLVE_DEFAULT, mat);
                    SetKeyword(MKToonMaterialHelper.GetDissolveMap(mat) != null && MKToonMaterialHelper.GetDissolveRampMap(mat) != null && mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_DISSOLVE) == 1.0f, MKToonEditor.DISSOLVE_RAMP, mat);

                    //Sketch
                    //SetKeyword(mat.GetFloat(MKToonMaterialHelper.PropertyNames.USE_SKETCH) == 1.0f, MKToonEditor.SKETCH, mat);
                }
            }
            //window.Close();
        }

        private static void SetKeyword(bool enable, string keyword, Material mat)
        {
            if (enable)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
        }
    }
}
#endif