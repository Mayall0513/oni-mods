using Harmony;
using ModFramework;
using PeterHan.PLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Pliers {
    public static class Mod_OnLoad {
        public static void OnLoad() {
            PUtil.InitLibrary(false);

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string currentAssemblyDirectory = Path.GetDirectoryName(currentAssembly.Location);

            PliersAssets.PLIERS_PATH_CONFIGFOLDER = Path.Combine(currentAssemblyDirectory, "config");
            PliersAssets.PLIERS_PATH_CONFIGFILE = Path.Combine(PliersAssets.PLIERS_PATH_CONFIGFOLDER, "config.json");
            PliersAssets.PLIERS_PATH_KEYCODESFILE = Path.Combine(PliersAssets.PLIERS_PATH_CONFIGFOLDER, "keycodes.txt");

            PliersAssets.PLIERS_ICON_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Pliers.image_wirecutter_button.dds"), 32, 32);
            PliersAssets.PLIERS_ICON_SPRITE.name = PliersAssets.PLIERS_ICON_NAME;
            PliersAssets.PLIERS_VISUALIZER_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Pliers.image_wirecutter_visualizer.dds"), 256, 256);

            PliersAssets.PLIERS_OPENTOOL = PAction.Register("Pliers.opentool", "Pliers", new PKeyBinding(KKeyCode.None, Modifier.None));

            ModLocalization.LocalizationCompleteEvent += ModLocalizedHandler;
            ModLocalization.DefaultLocalization = new string[] {
                PliersStrings.STRING_PLIERS_NAME, "Pliers",
                PliersStrings.STRING_PLIERS_TOOLTIP, "Disconnect utility networks {0}",
                PliersStrings.STRING_PLIERS_TOOLTIP_TITLE, "PLIERS",
                PliersStrings.STRING_PLIERS_ACTION_DRAG, "DRAG",
                PliersStrings.STRING_PLIERS_ACTION_BACK, "BACK"
            };

            Debug.Log("Pliers Loaded: Version " + currentAssembly.GetName().Version);
        }

        private static void ModLocalizedHandler(string languageCode) {
            PliersAssets.PLIERS_TOOLCOLLECTION = ToolMenu.CreateToolCollection(
                (string) Strings.Get(PliersStrings.STRING_PLIERS_NAME),
                PliersAssets.PLIERS_ICON_NAME,
                PliersAssets.PLIERS_OPENTOOL.GetKAction(),
                PliersAssets.PLIERS_TOOLNAME,
                string.Format(Strings.Get(PliersStrings.STRING_PLIERS_TOOLTIP).String, "{Hotkey}"),
                false
           );
        }
    }

    namespace Patches {
        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerController_OnPrefabInit {
            public static void Postfix(PlayerController __instance) {
                List<InterfaceTool> interfaceTools = new List<InterfaceTool>(__instance.tools);


                GameObject pliersTool = new GameObject(PliersAssets.PLIERS_TOOLNAME);
                pliersTool.AddComponent<PliersTool>();

                pliersTool.transform.SetParent(__instance.gameObject.transform);
                pliersTool.gameObject.SetActive(true);
                pliersTool.gameObject.SetActive(false);

                interfaceTools.Add(pliersTool.GetComponent<InterfaceTool>());


                __instance.tools = interfaceTools.ToArray();
            }
        }

        [HarmonyPatch(typeof(ToolMenu), "OnPrefabInit")]
        public static class ToolMenu_OnPrefabInit {
            public static void Postfix(ToolMenu __instance, List<Sprite> ___icons)
            {
                ___icons.Add(PliersAssets.PLIERS_ICON_SPRITE);
            }
        }

        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public static class ToolMenu_CreateBasicTools {
            public static void Prefix(ToolMenu __instance) {
                __instance.basicTools.Add(PliersAssets.PLIERS_TOOLCOLLECTION);
            }
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public static class Game_DestroyInstances {
            public static void Postfix() {
                PliersTool.DestroyInstance();
            }
        }
    }
}