using Harmony;
using ModFramework;
using PeterHan.PLib.Options;
using System.Reflection;
using UnityEngine;

namespace AdvancedFilterMenu {
    public sealed class AdvancedFilterMenuCancelTool : MultiFilteredDragTool {
        public static AdvancedFilterMenuCancelTool Instance { get; private set; }

        public AdvancedFilterMenuCancelTool() {
            Instance = this;
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();

            visualizer = Util.KInstantiate(Traverse.Create(CancelTool.Instance).Field("visualizer").GetValue<GameObject>());

            visualizer.SetActive(false);
            visualizer.transform.SetParent(transform);

            FieldInfo areaVisualizerField = AccessTools.Field(typeof(DragTool), "areaVisualizer");
            FieldInfo areaVisualizerSpriteRendererField = AccessTools.Field(typeof(DragTool), "areaVisualizerSpriteRenderer");

            GameObject areaVisualizer = Util.KInstantiate(Traverse.Create(CancelTool.Instance).Field("areaVisualizer").GetValue<GameObject>());
            areaVisualizer.SetActive(false);

            areaVisualizer.name = "AdvancedFilterMenuCancelToolAreaVisualizer";
            areaVisualizerSpriteRendererField.SetValue(this, areaVisualizer.GetComponent<SpriteRenderer>());
            areaVisualizer.transform.SetParent(transform);

            areaVisualizerField.SetValue(this, areaVisualizer);
            gameObject.AddComponent<CancelToolHoverTextCard>();
        }

        protected override void OnDragTool(int cell, int distFromOrigin) {
            for (int index = 0; index < 40; ++index) {
                GameObject gameObject = Grid.Objects[cell, index];

                if (gameObject != null && MultiToolParameterMenu.Instance.IsActiveLayer(MultiToolParameterMenu.GetFilterLayerFromGameObject(gameObject))) {
                    gameObject.Trigger(2127324410, null);
                }
            }
        }

        protected override void OnDragComplete(Vector3 downPos, Vector3 upPos) {
            Vector2 regularizedPos1 = GetRegularizedPos(Vector2.Min(downPos, upPos), true);
            Vector2 regularizedPos2 = GetRegularizedPos(Vector2.Max(downPos, upPos), false);

            AttackTool.MarkForAttack(regularizedPos1, regularizedPos2, false);
            CaptureTool.MarkForCapture(regularizedPos1, regularizedPos2, false);
        }

        protected override void OnSyncChanged(bool synced) {
            base.OnSyncChanged(synced);

            AdvancedFiltrationAssets.Options.CancelToolSync = synced;
            POptions.WriteSettings(AdvancedFiltrationAssets.Options);
        }
    }
}