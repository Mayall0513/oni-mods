using Harmony;
using ModFramework;
using PeterHan.PLib.Options;
using System.Reflection;
using UnityEngine;

namespace AdvancedFilterMenu {
    public sealed class AdvancedFilterMenuPrioritizeTool : MultiFilteredDragTool {
        public static AdvancedFilterMenuPrioritizeTool Instance { get; private set; }

        public AdvancedFilterMenuPrioritizeTool() {
            Instance = this;
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();

            visualizer = Util.KInstantiate(Traverse.Create(PrioritizeTool.Instance).Field("visualizer").GetValue<GameObject>());
            visualizer.SetActive(false);

            FieldInfo areaVisualizerField = AccessTools.Field(typeof(DragTool), "areaVisualizer");
            FieldInfo areaVisualizerSpriteRendererField = AccessTools.Field(typeof(DragTool), "areaVisualizerSpriteRenderer");

            GameObject areaVisualizer = Util.KInstantiate(Traverse.Create(PrioritizeTool.Instance).Field("areaVisualizer").GetValue<GameObject>());
            areaVisualizer.SetActive(false);

            areaVisualizer.name = "AdvancedFilterMenuPrioritizeToolToolAreaVisualizer";
            areaVisualizerSpriteRendererField.SetValue(this, areaVisualizer.GetComponent<SpriteRenderer>());
            areaVisualizer.transform.SetParent(transform);

            areaVisualizerField.SetValue(this, areaVisualizer);
            gameObject.AddComponent<CancelToolHoverTextCard>();

            viewMode = OverlayModes.Priorities.ID;
            interceptNumberKeysForPriority = true;
        }

        private bool TryPrioritizeGameObject(GameObject target, PrioritySetting priority) {
            if (MultiToolParameterMenu.Instance.IsActiveLayer(GetFilterLayerFromGameObject(target))) {
                Prioritizable prioritizable = target.GetComponent<Prioritizable>();

                if (prioritizable != null && prioritizable.showIcon && prioritizable.IsPrioritizable()) {
                    prioritizable.SetMasterPriority(priority);
                    return true;
                }
            }
            return false;
        }

        private static string GetFilterLayerFromGameObject(GameObject input) {
            if (input.GetComponent<Constructable>() != null || input.GetComponent<Deconstructable>() != null && input.GetComponent<Deconstructable>().IsMarkedForDeconstruction()) {
                return ToolParameterMenu.FILTERLAYERS.CONSTRUCTION;
            }
                
            if (input.GetComponent<Diggable>() != null) {
                return ToolParameterMenu.FILTERLAYERS.DIG;
            }

            if (input.GetComponent<Clearable>() != null || input.GetComponent<Moppable>() != null || input.GetComponent<StorageLocker>() != null) {
                return ToolParameterMenu.FILTERLAYERS.CLEAN;
            }
                
            return ToolParameterMenu.FILTERLAYERS.OPERATE;
        }

        protected override void OnDragTool(int cell, int distFromOrigin) {
            PrioritySetting selectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
            int prioritiesChanged = 0;

            for (int index = 0; index < 40; ++index) {
                GameObject gameObject = Grid.Objects[cell, index];

                if (gameObject != null) {
                    if (gameObject.GetComponent<Pickupable>()) {
                        ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;

                        while (objectLayerListItem != null) {
                            GameObject currentObject = objectLayerListItem.gameObject;
                            objectLayerListItem = objectLayerListItem.nextItem;

                            if (currentObject != null && !(currentObject.GetComponent<MinionIdentity>() != null && TryPrioritizeGameObject(currentObject, selectedPriority))) {
                                ++prioritiesChanged;
                            }
                        }
                    }

                    else if (TryPrioritizeGameObject(gameObject, selectedPriority)) {
                        ++prioritiesChanged;
                    }
                }
            }

            if (prioritiesChanged > 0) {
                PriorityScreen.PlayPriorityConfirmSound(selectedPriority);
            }
        }

        protected override void OnActivateTool() {
            base.OnActivateTool();

            ToolMenu.Instance.PriorityScreen.ShowDiagram(true);
            ToolMenu.Instance.PriorityScreen.Show(true);

            ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1.35F, 1.35F, 1.35F);
        }

        protected override void OnDeactivateTool(InterfaceTool newTool) {
            base.OnDeactivateTool(newTool);

            ToolMenu.Instance.PriorityScreen.Show(false);
            ToolMenu.Instance.PriorityScreen.ShowDiagram(false);

            ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1, 1, 1);
        }

        public void Update() {
            PrioritySetting selectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();

            int offset = 0;
            if (selectedPriority.priority_class >= PriorityScreen.PriorityClass.high) {
                offset += 9;
            }
                
            Texture2D cursor = PrioritizeTool.Instance.cursors[offset + selectedPriority.priority_value - 1];
            MeshRenderer meshRenderer =  visualizer.GetComponentInChildren<MeshRenderer>();

            if (meshRenderer != null) {
                meshRenderer.material.mainTexture = cursor;
            }
        }

        protected override void OnSyncChanged(bool synced) {
            base.OnSyncChanged(synced);

            AdvancedFiltrationAssets.Options.PrioritizeSync = synced;
            POptions.WriteSettings(AdvancedFiltrationAssets.Options);
        }
    }
}