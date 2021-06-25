using Harmony;
using ModFramework;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;
using PeterHan.PLib.Options;
using Rendering;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Blueprints {
    public static class Integration {
        public static void OnLoad() {
            PUtil.InitLibrary(false);
            PLocalization.Register();
            Localization.RegisterForTranslation(typeof(BlueprintsStrings));

            POptions.RegisterOptions(typeof(BlueprintsOptions));

            BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_createblueprint_button.dds"), 32, 32);
            BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE.name = BlueprintsAssets.BLUEPRINTS_CREATE_ICON_NAME;
            BlueprintsAssets.BLUEPRINTS_CREATE_OPENTOOL = PAction.Register("Blueprints.create.opentool", "Create Blueprint", new PKeyBinding(KKeyCode.None, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_CREATE_VISUALIZER_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_createblueprint_visualizer.dds"), 256, 256);

            BlueprintsAssets.BLUEPRINTS_USE_ICON_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_useblueprint_button.dds"), 32, 32);
            BlueprintsAssets.BLUEPRINTS_USE_ICON_SPRITE.name = BlueprintsAssets.BLUEPRINTS_USE_ICON_NAME;
            BlueprintsAssets.BLUEPRINTS_USE_OPENTOOL = PAction.Register("Blueprints.use.opentool", "Use Blueprint", new PKeyBinding(KKeyCode.None, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_CREATEFOLDER = PAction.Register("Blueprints.use.assignfolder", "Assign Folder", new PKeyBinding(KKeyCode.Home, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_RENAME = PAction.Register("Blueprints.use.rename", "Rename Blueprint", new PKeyBinding(KKeyCode.End, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_CYCLEFOLDERS_NEXT = PAction.Register("Blueprints.use.cyclefolders.next", "Next Folder", new PKeyBinding(KKeyCode.UpArrow, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_CYCLEFOLDERS_PREVIOUS = PAction.Register("Blueprints.use.cyclefolders.previous", "Previous Folder", new PKeyBinding(KKeyCode.DownArrow, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_CYCLEBLUEPRINTS_NEXT = PAction.Register("Blueprints.use.cycleblueprints.next", "Next Blueprint", new PKeyBinding(KKeyCode.RightArrow, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_CYCLEBLUEPRINTS_PREVIOUS = PAction.Register("Blueprints.use.cycleblueprints.previous", "Previous Blueprint", new PKeyBinding(KKeyCode.LeftArrow, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_USE_VISUALIZER_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_useblueprint_visualizer.dds"), 256, 256);

            BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_snapshot_button.dds"), 32, 32);
            BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_SPRITE.name = BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_NAME;
            BlueprintsAssets.BLUEPRINTS_SNAPSHOT_OPENTOOL = PAction.Register("Blueprints.snapshot.opentool", "Take Snapshot", new PKeyBinding(KKeyCode.None, Modifier.None));
            BlueprintsAssets.BLUEPRINTS_SNAPSHOT_VISUALIZER_SPRITE = Utilities.CreateSpriteDXT5(Assembly.GetExecutingAssembly().GetManifestResourceStream("Blueprints.image_snapshot_visualizer.dds"), 256, 256);

            BlueprintsAssets.BLUEPRINTS_MULTI_DELETE = PAction.Register("Blueprints.multi.delete", "Delete Blueprint/Snapshot", new PKeyBinding(KKeyCode.Delete, Modifier.None));

            Utilities.AttachFileWatcher();
            Debug.Log("Blueprints Loaded: Version " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }

    [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
    public static class PlayerController_OnPrefabInit {
        public static void Postfix(PlayerController __instance) {
            List<InterfaceTool> interfaceTools = new List<InterfaceTool>(__instance.tools);

            GameObject createBlueprintTool = new GameObject(BlueprintsAssets.BLUEPRINTS_CREATE_TOOLNAME, typeof(CreateBlueprintTool));
            createBlueprintTool.transform.SetParent(__instance.gameObject.transform);
            createBlueprintTool.gameObject.SetActive(true);
            createBlueprintTool.gameObject.SetActive(false);

            interfaceTools.Add(createBlueprintTool.GetComponent<InterfaceTool>());

            GameObject useBlueprintTool = new GameObject(BlueprintsAssets.BLUEPRINTS_USE_TOOLNAME, typeof(UseBlueprintTool));
            useBlueprintTool.transform.SetParent(__instance.gameObject.transform);
            useBlueprintTool.gameObject.SetActive(true);
            useBlueprintTool.gameObject.SetActive(false);

            interfaceTools.Add(useBlueprintTool.GetComponent<InterfaceTool>());

            GameObject snapshotTool = new GameObject(BlueprintsAssets.BLUEPRINTS_SNAPSHOT_TOOLNAME, typeof(SnapshotTool));
            snapshotTool.transform.SetParent(__instance.gameObject.transform);
            snapshotTool.gameObject.SetActive(true);
            snapshotTool.gameObject.SetActive(false);

            interfaceTools.Add(snapshotTool.GetComponent<InterfaceTool>());

            __instance.tools = interfaceTools.ToArray();

            BlueprintsAssets.Options = POptions.ReadSettings<BlueprintsOptions>() ?? new BlueprintsOptions();
            CreateBlueprintTool.Instance.OverlaySynced = BlueprintsAssets.Options.CreateBlueprintToolSync;
            SnapshotTool.Instance.OverlaySynced = BlueprintsAssets.Options.SnapshotToolSync;
        }
    }

    [HarmonyPatch(typeof(ToolMenu), "OnPrefabInit")]
    public static class ToolMenu_OnPrefabInit {
        public static void Postfix(List<Sprite> ___icons)
        {
            if (___icons.Contains(BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE))
            {
                ___icons.Remove(BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE);
            }
            ___icons.Add(BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE);
            if (___icons.Contains(BlueprintsAssets.BLUEPRINTS_USE_ICON_SPRITE))
            {
                ___icons.Remove(BlueprintsAssets.BLUEPRINTS_USE_ICON_SPRITE);
            }
            ___icons.Add(BlueprintsAssets.BLUEPRINTS_USE_ICON_SPRITE);
            if (___icons.Contains(BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_SPRITE))
            {
                ___icons.Remove(BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_SPRITE);
            }
            ___icons.Add(BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_SPRITE);

            MultiToolParameterMenu.CreateInstance();
            ToolParameterMenu.ToggleState defaultSelection = BlueprintsAssets.Options.DefaultMenuSelections == DefaultSelections.All ? ToolParameterMenu.ToggleState.On : ToolParameterMenu.ToggleState.Off;

            SnapshotTool.Instance.DefaultParameters = 
            CreateBlueprintTool.Instance.DefaultParameters = new Dictionary<string, ToolParameterMenu.ToggleState> {
                { ToolParameterMenu.FILTERLAYERS.WIRES, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.GASCONDUIT, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.BUILDINGS, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.LOGIC, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.BACKWALL, defaultSelection },
                { ToolParameterMenu.FILTERLAYERS.DIGPLACER, defaultSelection},
                //{ BlueprintsStrings.STRING_BLUEPRINTS_MULTIFILTER_GASTILES, defaultSelection },
            };
        }
    }

    [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
    public static class ToolMenu_CreateBasicTools {
        public static void Prefix(ToolMenu __instance) {
            __instance.basicTools.Add(ToolMenu.CreateToolCollection(
                    BlueprintsStrings.STRING_BLUEPRINTS_CREATE_NAME,
                    BlueprintsAssets.BLUEPRINTS_CREATE_ICON_NAME,
                    BlueprintsAssets.BLUEPRINTS_CREATE_OPENTOOL.GetKAction(),
                    BlueprintsAssets.BLUEPRINTS_CREATE_TOOLNAME,
                    string.Format(BlueprintsStrings.STRING_BLUEPRINTS_CREATE_TOOLTIP, "{Hotkey}"),
                    true
                ));
            __instance.basicTools.Add(ToolMenu.CreateToolCollection(
                    BlueprintsStrings.STRING_BLUEPRINTS_USE_NAME,
                    BlueprintsAssets.BLUEPRINTS_USE_ICON_NAME,
                    BlueprintsAssets.BLUEPRINTS_USE_OPENTOOL.GetKAction(),
                    BlueprintsAssets.BLUEPRINTS_USE_TOOLNAME,
                    string.Format(BlueprintsStrings.STRING_BLUEPRINTS_USE_TOOLTIP, "{Hotkey}"),
                    true
                ));
            __instance.basicTools.Add(ToolMenu.CreateToolCollection(
                    BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_NAME,
                    BlueprintsAssets.BLUEPRINTS_SNAPSHOT_ICON_NAME,
                    BlueprintsAssets.BLUEPRINTS_SNAPSHOT_OPENTOOL.GetKAction(),
                    BlueprintsAssets.BLUEPRINTS_SNAPSHOT_TOOLNAME,
                    string.Format(BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_TOOLTIP, "{Hotkey}"),
                    false
                ));

            Utilities.ReloadBlueprints(false);
        }
    }

    [HarmonyPatch(typeof(FileNameDialog), "OnSpawn")]
    public static class FileNameDialog_OnSpawn {
        public static void Postfix(FileNameDialog __instance, TMP_InputField ___inputField) {
            if (__instance.name.StartsWith("BlueprintsMod_")) {
                ___inputField.onValueChanged.RemoveAllListeners();
                ___inputField.onEndEdit.RemoveAllListeners();

                if (__instance.name.StartsWith("BlueprintsMod_FolderDialog_")) {
                    ___inputField.onValueChanged.AddListener(delegate (string text) {
                        for (int i = text.Length - 1; i >= 0; --i) {
                            if (i < text.Length && BlueprintsAssets.BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.Contains(text[i])) {
                                text = text.Remove(i, 1);
                            }
                        }

                        ___inputField.text = text;
                    });
                }
            }
        }
    }

    [HarmonyPatch(typeof(Game), "DestroyInstances")]
    public static class Game_DestroyInstances {
        public static void Postfix() {
            CreateBlueprintTool.DestroyInstance();
            UseBlueprintTool.DestroyInstance();
            SnapshotTool.DestroyInstance();
            MultiToolParameterMenu.DestroyInstance();

            BlueprintsAssets.BLUEPRINTS_AUTOFILE_WATCHER.Dispose();
        }
    }

    [HarmonyPatch(typeof(BlockTileRenderer), "GetCellColour")]
    public static class BlockTileRenderer_GetCellColour {
        public static void Postfix(int cell, SimHashes element, ref Color __result) {
            if (__result != Color.red && element == SimHashes.Void && BlueprintsState.ColoredCells.ContainsKey(cell)) {
                __result = BlueprintsState.ColoredCells[cell].Color;
            }
        }
    }
}