using PeterHan.PLib.UI;
using System.IO;
using UnityEngine;

namespace Blueprints {
    public sealed class UseBlueprintTool : InterfaceTool {
        public static UseBlueprintTool Instance { get; private set; }

        public UseBlueprintTool() {
            Instance = this;
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        public void CreateVisualizer() {
            if (visualizer != null) {
                Destroy(visualizer);
            }

            visualizer = new GameObject("UseBlueprintVisualizer");
            visualizer.SetActive(false);

            GameObject offsetObject = new GameObject();
            SpriteRenderer spriteRenderer = offsetObject.AddComponent<SpriteRenderer>();
            spriteRenderer.color = BlueprintsAssets.BLUEPRINTS_COLOR_BLUEPRINT_DRAG;
            spriteRenderer.sprite = BlueprintsAssets.BLUEPRINTS_USE_VISUALIZER_SPRITE;

            offsetObject.transform.SetParent(visualizer.transform);
            offsetObject.transform.localPosition = new Vector3(0, Grid.HalfCellSizeInMeters);
            offsetObject.transform.localScale = new Vector3(
                Grid.CellSizeInMeters / (spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit),
                Grid.CellSizeInMeters / (spriteRenderer.sprite.texture.height / spriteRenderer.sprite.pixelsPerUnit)
            );

            offsetObject.SetLayerRecursively(LayerMask.NameToLayer("Overlay"));
            visualizer.transform.SetParent(transform);

            OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();
            gameObject.AddComponent<UseBlueprintToolHoverCard>();
        }

        protected override void OnActivateTool() {
            base.OnActivateTool();

            ToolMenu.Instance.PriorityScreen.Show(true);

            if (BlueprintsState.HasBlueprints()) {
                GridCompositor.Instance.ToggleMajor(true);
            }

            if (BlueprintsState.HasBlueprints()) {
                BlueprintsState.VisualizeBlueprint(Grid.PosToXY(PlayerController.GetCursorPos(KInputManager.GetMousePos())), BlueprintsState.SelectedBlueprint);
                if (visualizer != null) {
                    Destroy(visualizer);
                    visualizer = null;
                }
            }

            else {
                CreateVisualizer();
            }
        }

        protected override void OnDeactivateTool(InterfaceTool newTool) {
            base.OnDeactivateTool(newTool);

            BlueprintsState.ClearVisuals();
            ToolMenu.Instance.PriorityScreen.Show(false);
            GridCompositor.Instance.ToggleMajor(false);
        }

        public override void OnLeftClickDown(Vector3 cursorPos) {
            base.OnLeftClickDown(cursorPos);

            if (hasFocus) {
                BlueprintsState.UseBlueprint(Grid.PosToXY(cursorPos));
            }
        }

        public override void OnMouseMove(Vector3 cursorPos) {
            base.OnMouseMove(cursorPos);

            if (hasFocus) {
                BlueprintsState.UpdateVisual(Grid.PosToXY(cursorPos));
            }
        }

        public override void OnKeyDown(KButtonEvent buttonEvent) {
            if (BlueprintsState.LoadedBlueprints.Count > 0) {
                bool blueprintChanged = false;

                if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_CREATEFOLDER.GetKAction())) {
                    static void onConfirmDelegate(string blueprintFolder, FileNameDialog parent) {
                        string newFolder = blueprintFolder.Trim(' ', '/', '\\', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                        if (newFolder == BlueprintsState.SelectedBlueprint.Folder) {
                            PopFXManager.Instance.SpawnFX(BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE, Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_USE_FOLDERBLUEPRINT_NA), null, PlayerController.GetCursorPos(KInputManager.GetMousePos()), BlueprintsAssets.Options.FXTime, false, false);
                        }

                        else {
                            string blueprintName = BlueprintsState.SelectedBlueprint.FriendlyName;
                            
                            BlueprintsState.SelectedBlueprint.SetFolder(newFolder);
                            PopFXManager.Instance.SpawnFX(BlueprintsAssets.BLUEPRINTS_CREATE_ICON_SPRITE, string.Format(Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_USE_MOVEDBLUEPRINT), blueprintName, newFolder), null, PlayerController.GetCursorPos(KInputManager.GetMousePos()), BlueprintsAssets.Options.FXTime, false, false);
                        }
                        
                        SpeedControlScreen.Instance.Unpause(false);
                        parent.Deactivate();
                    }

                    FileNameDialog blueprintFolderDialog = UIUtilities.CreateFolderDialog(onConfirmDelegate);
                    SpeedControlScreen.Instance.Pause(false);
                    blueprintFolderDialog.Activate();
                }

                else if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_RENAME.GetKAction())) {
                    static void onConfirmDelegate(string blueprintName, FileNameDialog parent) {
                        BlueprintsState.SelectedBlueprint.Rename(blueprintName);
  
                        SpeedControlScreen.Instance.Unpause(false);
                        parent.Deactivate();
                    }

                    FileNameDialog blueprintNameDialog = UIUtilities.CreateTextDialog(Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_NAMEBLUEPRINT_TITLE), false, onConfirmDelegate);
                    SpeedControlScreen.Instance.Pause(false);
                    blueprintNameDialog.Activate();
                }

                else if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_MULTI_DELETE.GetKAction())) {
                    static void onConfirmDelegate() {
                        BlueprintsState.SelectedBlueprint.DeleteFile();
                        BlueprintsState.SelectedFolder.RemoveBlueprint(BlueprintsState.SelectedBlueprint);

                        if (!BlueprintsState.HasBlueprints()) {
                            GridCompositor.Instance.ToggleMajor(false);
                        }

                        BlueprintsState.ClearVisuals();

                        if (BlueprintsState.HasBlueprints()) {
                            BlueprintsState.VisualizeBlueprint(Grid.PosToXY(PlayerController.GetCursorPos(KInputManager.GetMousePos())), BlueprintsState.SelectedBlueprint);
                        }

                        else {
                            Instance.CreateVisualizer();
                        }
                    }

                    PUIElements.ShowConfirmDialog(GameScreenManager.Instance.GetParent(GameScreenManager.UIRenderTarget.ScreenSpaceOverlay), "Are you sure you want to delete \"" + BlueprintsState.SelectedBlueprint.FriendlyName + "\"?", onConfirmDelegate, null, "YES", "NO");
                }

                else if (BlueprintsState.LoadedBlueprints.Count > 0) {
                    if (BlueprintsState.SelectedFolder.BlueprintCount > 1) {
                        if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_CYCLEBLUEPRINTS_NEXT.GetKAction())) {
                            blueprintChanged = BlueprintsState.SelectedFolder.NextBlueprint(); 
                        }

                        else if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_CYCLEBLUEPRINTS_PREVIOUS.GetKAction())) {
                            blueprintChanged = BlueprintsState.SelectedFolder.PreviousBlueprint();
                        }
                    }

                    if (!blueprintChanged && BlueprintsState.LoadedBlueprints.Count > 1) {
                        if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_CYCLEFOLDERS_NEXT.GetKAction())) {
                            if (++BlueprintsState.SelectedBlueprintFolderIndex >= BlueprintsState.LoadedBlueprints.Count) {
                                BlueprintsState.SelectedBlueprintFolderIndex = 0;
                            }

                            blueprintChanged = true;
                        }

                        else if (buttonEvent.TryConsume(BlueprintsAssets.BLUEPRINTS_USE_CYCLEFOLDERS_PREVIOUS.GetKAction())) {
                            if (--BlueprintsState.SelectedBlueprintFolderIndex < 0) {
                                BlueprintsState.SelectedBlueprintFolderIndex = BlueprintsState.LoadedBlueprints.Count - 1;
                            }

                            blueprintChanged = true;
                        }
                    }
                }

                if (blueprintChanged) {
                    BlueprintsState.ClearVisuals();

                    if (BlueprintsState.HasBlueprints()) {
                        BlueprintsState.VisualizeBlueprint(Grid.PosToXY(PlayerController.GetCursorPos(KInputManager.GetMousePos())), BlueprintsState.SelectedBlueprint);
                    }

                    else {
                        Instance.CreateVisualizer();
                    }
                }
            }

            base.OnKeyDown(buttonEvent);
        }
    }
}
