using Harmony;
using ModFramework;
using PeterHan.PLib.Options;
using System.Reflection;
using UnityEngine;

namespace AdvancedFilterMenu {
    public sealed class AdvancedFilterMenuEmptyPipeTool : MultiFilteredDragTool {
        public static AdvancedFilterMenuEmptyPipeTool Instance { get; private set; }

        public AdvancedFilterMenuEmptyPipeTool() {
            Instance = this;
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();

            visualizer = Util.KInstantiate(Traverse.Create(EmptyPipeTool.Instance).Field("visualizer").GetValue<GameObject>());

            visualizer.SetActive(false);
            visualizer.transform.SetParent(transform);

            FieldInfo areaVisualizerField = AccessTools.Field(typeof(DragTool), "areaVisualizer");
            FieldInfo areaVisualizerSpriteRendererField = AccessTools.Field(typeof(DragTool), "areaVisualizerSpriteRenderer");

            GameObject areaVisualizer = Util.KInstantiate(Traverse.Create(EmptyPipeTool.Instance).Field("areaVisualizer").GetValue<GameObject>());
            areaVisualizer.SetActive(false);

            areaVisualizer.name = "AdvancedFilterMenuEmptyPipeToolAreaVisualizer";
            areaVisualizerSpriteRendererField.SetValue(this, areaVisualizer.GetComponent<SpriteRenderer>());
            areaVisualizer.transform.SetParent(transform);

            areaVisualizerField.SetValue(this, areaVisualizer);
            gameObject.AddComponent<CancelToolHoverTextCard>();
        }

        protected override void OnDragTool(int cell, int distFromOrigin) {
            for (int index = 0; index < 40; ++index) {
                if (MultiToolParameterMenu.Instance.IsActiveLayer((ObjectLayer) index)) {
                    GameObject gameObject = Grid.Objects[cell, index];

                    if (gameObject != null) {
                        EmptyConduitWorkable conduitWorkable = gameObject.GetComponent<EmptyConduitWorkable>();

                        if (conduitWorkable != null) {
                            if (DebugHandler.InstantBuildMode) {
                                conduitWorkable.EmptyPipeContents();
                            }

                            else {
                                conduitWorkable.MarkForEmptying();

                                Prioritizable prioritizable = gameObject.GetComponent<Prioritizable>();
                                if (prioritizable != null) {
                                    prioritizable.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnActivateTool() {
            base.OnActivateTool();

            ToolMenu.Instance.PriorityScreen.Show(true);
        }

        protected override void OnDeactivateTool(InterfaceTool newTool) {
            base.OnDeactivateTool(newTool);

            ToolMenu.Instance.PriorityScreen.Show(false);
        }

        protected override void OnSyncChanged(bool synced) {
            base.OnSyncChanged(synced);

            AdvancedFiltrationAssets.Options.EmptyPipeSync = synced;
            POptions.WriteSettings(AdvancedFiltrationAssets.Options);
        }
    }
}