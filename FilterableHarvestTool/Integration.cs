using Harmony;
using ModFramework;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FilterableHarvestTool {
    public static class Mod_OnLoad {
        public static void OnLoad() {
            ModLocalization.DefaultLocalization = new string[] {
                FilterableHarvestToolStrings.FILTERABLEHARVESTTOOL_WILD_ENABLEHARVEST, "Enable Wild Harvest",
                FilterableHarvestToolStrings.FILTERABLEHARVESTTOOL_WILD_DISABLEHARVEST, "Disable Wild Harvest",
                FilterableHarvestToolStrings.FILTERABLEHARVESTTOOL_FARM_ENABLEHARVEST, "Enable Farm Harvest",
                FilterableHarvestToolStrings.FILTERABLEHARVESTTOOL_FARM_DISABLEHARVEST, "Disable Farm Harvest"
            };

            Debug.Log("Filterable Harvest Tool Loaded: Version " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }

    namespace Patches {
        [HarmonyPatch(typeof(HarvestTool), "OnPrefabInit")]
        public static class HarvestTool_OnPrefabInit {
            public static void Postfix(Dictionary<string, ToolParameterMenu.ToggleState> ___options) {
                ___options.Add("ENABLEHARVEST_WILD", ToolParameterMenu.ToggleState.Off);
                ___options.Add("DISABLEHARVEST_WILD", ToolParameterMenu.ToggleState.Off);
                ___options.Add("ENABLEHARVEST_FARM", ToolParameterMenu.ToggleState.Off);
                ___options.Add("DISABLEHARVEST_FARM", ToolParameterMenu.ToggleState.Off);
            }
        }

        [HarmonyPatch(typeof(HarvestTool), "Update")]
        public static class HarvestTool_Update {
            public static void Postfix(Dictionary<string, ToolParameterMenu.ToggleState> ___options) {
                MeshRenderer meshRenderer = HarvestTool.Instance.visualizer.GetComponentInChildren<MeshRenderer>();

                if (meshRenderer == null) {
                    return;
                }

                if (___options["ENABLEHARVEST_WILD"] == ToolParameterMenu.ToggleState.On || ___options["ENABLEHARVEST_FARM"] == ToolParameterMenu.ToggleState.On) {
                    meshRenderer.material.mainTexture = HarvestTool.Instance.visualizerTextures[0];
                }

                else if (___options["DISABLEHARVEST_WILD"] == ToolParameterMenu.ToggleState.On || ___options["DISABLEHARVEST_FARM"] == ToolParameterMenu.ToggleState.On) {
                    meshRenderer.material.mainTexture = HarvestTool.Instance.visualizerTextures[1];
                }
            }
        }

        [HarmonyPatch(typeof(HarvestTool), "OnDragTool")]
        public static class HarvestTool_OnDragTool {
            public static bool Prefix(int cell, Dictionary<string, ToolParameterMenu.ToggleState> ___options) {
                if (!Grid.IsValidCell(cell)) {
                    return false;
                }

                foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items) {
                    if (Grid.PosToCell(harvestDesignatable) == cell || harvestDesignatable.area != null && harvestDesignatable.area.CheckIsOccupying(cell)) {
                        if (___options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On) {
                            harvestDesignatable.SetHarvestWhenReady(true);
                        }

                        else if (___options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On) {
                            harvestDesignatable.SetHarvestWhenReady(false);
                        }

                        else {
                            if (harvestDesignatable.InPlanterBox) {
                                if (___options["ENABLEHARVEST_FARM"] == ToolParameterMenu.ToggleState.On) {
                                    harvestDesignatable.SetHarvestWhenReady(true);
                                }

                                else if (___options["DISABLEHARVEST_FARM"] == ToolParameterMenu.ToggleState.On) {
                                    harvestDesignatable.SetHarvestWhenReady(false);
                                }
                            }

                            else {
                                if (___options["ENABLEHARVEST_WILD"] == ToolParameterMenu.ToggleState.On) {
                                    harvestDesignatable.SetHarvestWhenReady(true);
                                }

                                else if (___options["DISABLEHARVEST_WILD"] == ToolParameterMenu.ToggleState.On) {
                                    harvestDesignatable.SetHarvestWhenReady(false);
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}