using Harmony;
using PeterHan.PLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ModFramework {
    [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
    public static class ModsScreen_BuildDisplay {
        public static LocText versionLocText;

        public static void Postfix(Transform ___entryParent) {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            for (int i = 0; i < Global.Instance.modManager.mods.Count; ++i) {
                KMod.Mod mod = Global.Instance.modManager.mods[i];

                if ((mod.loaded_content & KMod.Content.DLL) == KMod.Content.DLL) {
                    if (currentPath.Contains(Path.GetFullPath(mod.label.install_path))) {
                        string modTitle = mod.label.title;

                        for (int j = 0; j < ___entryParent.childCount; ++j) {
                            Transform modSpecificTransform = ___entryParent.GetChild(j);
                            HierarchyReferences hierarchyReferences = modSpecificTransform.GetComponent<HierarchyReferences>();
                            LocText titleReference = hierarchyReferences.GetReference<LocText>("Title");

                            if (titleReference != null && titleReference.text == modTitle) {
                                titleReference.text = "<align=left>" + titleReference.text + " <line-height=0.000000001>\n<size=85%><align=right>(" + Assembly.GetExecutingAssembly().GetName().Version + ")";
                                titleReference.autoSizeTextContainer = false;

                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
