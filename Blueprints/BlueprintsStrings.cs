using HarmonyLib;
using ModFramework;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Blueprints {
    static class BlueprintsStrings
    {
        // Action strings
        public static string ACTION_CREATE_KEY = "Blueprints.create.opentool";
        public static LocString ACTION_CREATE_TITLE = "Create Blueprint";

        public static string ACTION_USE_KEY = "Blueprints.use.opentool";
        public static LocString ACTION_USE_TITLE = "Use Blueprint";

        public static string ACTION_CREATEFOLDER_KEY = "Blueprints.use.assignfolder";
        public static LocString ACTION_CREATEFOLDER_TITLE = "Create Folder";

        public static string ACTION_RENAME_KEY = "Blueprints.use.rename";
        public static LocString ACTION_RENAME_TITLE = "Rename Blueprint";

        public static string ACTION_CYCLEFOLDERS_NEXT_KEY = "Blueprints.use.cyclefolders.next";
        public static LocString ACTION_CYCLEFOLDERS_NEXT_TITLE = "Next Folder";

        public static string ACTION_CYCLEFOLDERS_PREV_KEY = "Blueprints.use.cyclefolders.previous";
        public static LocString ACTION_CYCLEFOLDERS_PREV_TITLE = "Previous Folder";

        public static string ACTION_CYCLEBLUEPRINTS_NEXT_KEY = "Blueprints.use.cycleblueprints.next";
        public static LocString ACTION_CYCLEBLUEPRINTS_NEXT_TITLE = "Next Blueprint";

        public static string ACTION_CYCLEBLUEPRINTS_PREV_KEY = "Blueprints.use.cycleblueprints.previous";
        public static LocString ACTION_CYCLEBLUEPRINTS_PREV_TITLE = "Previous Blueprint";

        public static string ACTION_SNAPSHOT_KEY = "Blueprints.snapshot.opentool";
        public static LocString ACTION_SNAPSHOT_TITLE = "Take Snapshot";

        public static string ACTION_DELETE_KEY = "Blueprints.multi.delete";
        public static LocString ACTION_DELETE_TITLE = "Delete Blueprint/Snapshot";


        // Tool strings
        public static LocString STRING_BLUEPRINTS_CREATE_NAME = "New Blueprint";
        public static LocString STRING_BLUEPRINTS_CREATE_TOOLTIP = "Create blueprint {0}";
        public static LocString STRING_BLUEPRINTS_CREATE_EMPTY = "Blueprint would have been empty!";
        public static LocString STRING_BLUEPRINTS_CREATE_CREATED = "Created blueprint!";
        public static LocString STRING_BLUEPRINTS_CREATE_CANCELLED = "Cancelled blueprint!";
        public static LocString STRING_BLUEPRINTS_CREATE_TOOLTIP_TITLE = "CREATE BLUEPRINT TOOL";
        public static LocString STRING_BLUEPRINTS_CREATE_ACTION_DRAG = "DRAG";
        public static LocString STRING_BLUEPRINTS_CREATE_ACTION_BACK = "BACK";

        public static LocString STRING_BLUEPRINTS_USE_NAME = "Use Blueprint";
        public static LocString STRING_BLUEPRINTS_USE_TOOLTIP = "Use blueprint {0}";
        public static LocString STRING_BLUEPRINTS_USE_LOADEDBLUEPRINTS = "Loaded {0} blueprints! ({1} total)";
        public static LocString STRING_BLUEPRINTS_USE_LOADEDBLUEPRINTS_ADDITIONAL = "additional";
        public static LocString STRING_BLUEPRINTS_USE_LOADEDBLUEPRINTS_FEWER = "fewer";
        public static LocString STRING_BLUEPRINTS_USE_TOOLTIP_TITLE = "USE BLUEPRINT TOOL";
        public static LocString STRING_BLUEPRINTS_USE_ACTION_CLICK = "CLICK";
        public static LocString STRING_BLUEPRINTS_USE_ACTION_BACK = "BACK";
        public static LocString STRING_BLUEPRINTS_USE_CYCLEFOLDERS = "Use {0} and {1} to cycle folders.";
        public static LocString STRING_BLUEPRINTS_USE_CYCLEBLUEPRINTS = "Use {0} and {1} to cycle blueprints.";
        public static LocString STRING_BLUEPRINTS_USE_FOLDERBLUEPRINT = "Press {0} to assign folder.";
        public static LocString STRING_BLUEPRINTS_USE_FOLDERBLUEPRINT_NA = "Same folder provided - no change made.";
        public static LocString STRING_BLUEPRINTS_USE_MOVEDBLUEPRINT = "Moved \"{0}\" to \"{1}\"";
        public static LocString STRING_BLUEPRINTS_USE_NAMEBLUEPRINT = "Press {0} to rename blueprint.";
        public static LocString STRING_BLUEPRINTS_USE_DELETEBLUEPRINT = "Press {0} to delete blueprint.";
        public static LocString STRING_BLUEPRINTS_USE_ERRORMESSAGE = "This blueprint contained {0} misconfigured or missing prefabs which have been omitted!";
        public static LocString STRING_BLUEPRINTS_USE_SELECTEDBLUEPRINT = "Selected \"{0}\" ({1}/{2}) from \"{3}\" ({4}/{5})";
        public static LocString STRING_BLUEPRINTS_USE_FOLDEREMPTY = "Selected folder \"{0}\" is empty!";
        public static LocString STRING_BLUEPRINTS_USE_NOBLUEPRINTS = "No blueprints loaded!";

        public static LocString STRING_BLUEPRINTS_SNAPSHOT_NAME = "Take Snapshot";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_TOOLTIP = "Take snapshot {0} \n\nCreate a blueprint and quickly place it elsewhere while not cluttering your blueprint collection! \nSnapshots do not persist between games or worlds.";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_EMPTY = "Snapshot would have been empty!";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_TAKEN = "Snapshot taken!";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_TOOLTIP_TITLE = "SNAPSHOT TOOL";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_ACTION_CLICK = "CLICK";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_ACTION_DRAG = "DRAG";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_ACTION_BACK = "BACK";
        public static LocString STRING_BLUEPRINTS_SNAPSHOT_NEWSNAPSHOT = "Press {0} to take new snapshot.";

        public static LocString STRING_BLUEPRINTS_NAMEBLUEPRINT_TITLE = "NAME BLUEPRINT";
        public static LocString STRING_BLUEPRINTS_FOLDERBLUEPRINT_TITLE = "ASSIGN FOLDER";

        public static LocString STRING_BLUEPRINTS_MULTIFILTER_GASTILES = "Gas Tiles";
        public static LocString STRING_BLUEPRINTS_MULTIFILTER_ALL = "All";
        public static LocString STRING_BLUEPRINTS_MULTIFILTER_NONE = "None";
    }

    public static class BlueprintsAssets {
        public static BlueprintsOptions Options { get; set; } = new();

        public static string BLUEPRINTS_CREATE_ICON_NAME = "BLUEPRINTS.TOOL.CREATE_BLUEPRINT.ICON";
        public static Sprite BLUEPRINTS_CREATE_ICON_SPRITE;
        public static Sprite BLUEPRINTS_CREATE_VISUALIZER_SPRITE;

        public static string BLUEPRINTS_USE_ICON_NAME = "BLUEPRINTS.TOOL.USE_BLUEPRINT.ICON";
        public static Sprite BLUEPRINTS_USE_ICON_SPRITE;
        public static Sprite BLUEPRINTS_USE_VISUALIZER_SPRITE;

        public static string BLUEPRINTS_SNAPSHOT_ICON_NAME = "BLUEPRINTS.TOOL.SNAPSHOT.ICON";
        public static Sprite BLUEPRINTS_SNAPSHOT_ICON_SPRITE;
        public static Sprite BLUEPRINTS_SNAPSHOT_VISUALIZER_SPRITE;

        public static Color BLUEPRINTS_COLOR_VALIDPLACEMENT = Color.white;
        public static Color BLUEPRINTS_COLOR_INVALIDPLACEMENT = Color.red;
        public static Color BLUEPRINTS_COLOR_NOTECH = new Color32(30, 144, 255, 255);
        public static Color BLUEPRINTS_COLOR_BLUEPRINT_DRAG = new Color32(0, 119, 145, 255);

        public static HashSet<char> BLUEPRINTS_FILE_DISALLOWEDCHARACTERS;
        public static HashSet<char> BLUEPRINTS_PATH_DISALLOWEDCHARACTERS;

        public static HashSet<string> BLUEPRINTS_AUTOFILE_IGNORE = new();
        public static FileSystemWatcher BLUEPRINTS_AUTOFILE_WATCHER;

        static BlueprintsAssets() {
            BLUEPRINTS_FILE_DISALLOWEDCHARACTERS = new HashSet<char>();
            BLUEPRINTS_FILE_DISALLOWEDCHARACTERS.UnionWith(Path.GetInvalidFileNameChars());

            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS = new HashSet<char>();
            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.UnionWith(Path.GetInvalidFileNameChars());
            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.UnionWith(Path.GetInvalidPathChars());

            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.Remove('/');
            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.Remove('\\');
            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.Remove(Path.DirectorySeparatorChar);
            BLUEPRINTS_PATH_DISALLOWEDCHARACTERS.Remove(Path.AltDirectorySeparatorChar);

            BLUEPRINTS_CREATE_ICON_SPRITE = PUIUtils.LoadSprite("Blueprints.images.createblueprint_button.png");
            BLUEPRINTS_CREATE_ICON_SPRITE.name = BLUEPRINTS_CREATE_ICON_NAME;
            BLUEPRINTS_CREATE_VISUALIZER_SPRITE = PUIUtils.LoadSprite("Blueprints.images.createblueprint_visualizer.png");

            BLUEPRINTS_USE_ICON_SPRITE = PUIUtils.LoadSprite("Blueprints.images.useblueprint_button.png");
            BLUEPRINTS_USE_ICON_SPRITE.name = BLUEPRINTS_USE_ICON_NAME;
            BLUEPRINTS_USE_VISUALIZER_SPRITE = PUIUtils.LoadSprite("Blueprints.images.useblueprint_visualizer.png");

            BLUEPRINTS_SNAPSHOT_ICON_SPRITE = PUIUtils.LoadSprite("Blueprints.images.snapshot_button.png");
            BLUEPRINTS_SNAPSHOT_ICON_SPRITE.name = BLUEPRINTS_SNAPSHOT_ICON_NAME;
            BLUEPRINTS_SNAPSHOT_VISUALIZER_SPRITE = PUIUtils.LoadSprite("Blueprints.images.snapshot_visualizer.png");
        }
        public static void AddSpriteToCollection(Sprite sprite) {
            if (Assets.Sprites.ContainsKey(sprite.name))
                Assets.Sprites.Remove(sprite.name);
            Assets.Sprites.Add(sprite.name, sprite);
        }
    }

    public static class BlueprintsState {
        private static int _selectedBlueprintFolderIndex;
        public static int SelectedBlueprintFolderIndex {
            get => _selectedBlueprintFolderIndex;

            set => _selectedBlueprintFolderIndex = Mathf.Clamp(value, 0, LoadedBlueprints.Count - 1);
        }

        public static List<BlueprintFolder> LoadedBlueprints { get; } = new();
        public static BlueprintFolder SelectedFolder => LoadedBlueprints[SelectedBlueprintFolderIndex];
        public static Blueprint SelectedBlueprint => SelectedFolder.SelectedBlueprint;

        public static bool InstantBuild => DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild;

        private static readonly List<IVisual> FoundationVisuals = new();
        private static readonly List<IVisual> DependentVisuals = new();
        private static readonly List<ICleanableVisual> CleanableVisuals = new();

        public static readonly Dictionary<int, CellColorPayload> ColoredCells = new();

        public static bool HasBlueprints() {
            if (LoadedBlueprints.Count == 0) {
                return false;
            }

            foreach (BlueprintFolder blueprintFolder in LoadedBlueprints) {
                if (blueprintFolder.BlueprintCount > 0) {
                    return true;
                }
            }

            return false;
        }

        public static Blueprint CreateBlueprint(Vector2I topLeft, Vector2I bottomRight, MultiToolParameterMenu filter = null) {
            Blueprint blueprint = new Blueprint("unnamed", "");

            int blueprintHeight = (topLeft.y - bottomRight.y);
            bool collectingGasTiles = false;// filter.IsActiveLayer(BlueprintsStrings.STRING_BLUEPRINTS_MULTIFILTER_GASTILES);

            for (int x = topLeft.x; x <= bottomRight.x; ++x) {
                for (int y = bottomRight.y; y <= topLeft.y; ++y) {
                    int cell = Grid.XYToCell(x, y);

                    if (Grid.IsVisible(cell)) {
                        bool emptyCell = true;

                        for (int layer = 0; layer < Grid.ObjectLayers.Length; ++layer) {
                            if (layer == 7) {
                                continue;
                            }

                            GameObject gameObject = Grid.Objects[cell, layer];

                            if (gameObject != null && (gameObject.GetComponent<Constructable>() != null || gameObject.GetComponent<Deconstructable>() != null)) {
                                Building building;

                                bool validBuilding = (building = gameObject.GetComponent<Building>()) != null;
                                if (!validBuilding && (building = gameObject.GetComponent<BuildingUnderConstruction>()) != null) {
                                    validBuilding = true;
                                }

                                if (gameObject != null && validBuilding && building.Def.IsBuildable() && (filter == null || filter.IsActiveLayer(MultiToolParameterMenu.GetFilterLayerFromGameObject(gameObject)))) {
                                    Vector2I centre = Grid.CellToXY(GameUtil.NaturalBuildingCell(building));

                                    BuildingConfig buildingConfig = new BuildingConfig {
                                        Offset = new Vector2I(centre.x - topLeft.x, blueprintHeight - (topLeft.y - centre.y)),
                                        BuildingDef = building.Def,
                                        Orientation = building.Orientation
                                    };

                                    if (gameObject.GetComponent<Deconstructable>() != null) {
                                        buildingConfig.SelectedElements.AddRange(gameObject.GetComponent<Deconstructable>().constructionElements);
                                    }

                                    else {
                                        buildingConfig.SelectedElements.AddRange(Traverse.Create(gameObject.GetComponent<Constructable>()).Field("selectedElementsTags").GetValue<Tag[]>());
                                        
                                    }
                                    
                                    if (building.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>() != null) {
                                        buildingConfig.Flags = (int)building.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager()?.GetConnections(cell, false);
                                    }

                                    if (!blueprint.BuildingConfiguration.Contains(buildingConfig)) {
                                        blueprint.BuildingConfiguration.Add(buildingConfig);
                                    }

                                    emptyCell = false;
                                }
                            }
                        }

                        if (emptyCell && (collectingGasTiles && !Grid.IsSolidCell(cell) || filter.IsActiveLayer(ToolParameterMenu.FILTERLAYERS.DIGPLACER) && Grid.Objects[cell, 7] != null && Grid.Objects[cell, 7].name == "DigPlacer")) {
                            Vector2I digLocation = new Vector2I(x - topLeft.x, blueprintHeight - (topLeft.y - y));

                            if (!blueprint.DigLocations.Contains(digLocation)) {
                                blueprint.DigLocations.Add(digLocation);
                            }
                        }
                    }
                }
            }

            blueprint.CacheCost();
            return blueprint;
        }

        public static void VisualizeBlueprint(Vector2I topLeft, Blueprint blueprint) {
            if (blueprint == null) {
                return;
            }

            int errors = 0;
            ClearVisuals();

            foreach (BuildingConfig buildingConfig in blueprint.BuildingConfiguration) {
                if (buildingConfig.BuildingDef == null || buildingConfig.SelectedElements.Count == 0) {
                    ++errors;
                    continue;
                }

                if (buildingConfig.BuildingDef.BuildingPreview != null) {
                    int cell = Grid.XYToCell(topLeft.x + buildingConfig.Offset.x, topLeft.y + buildingConfig.Offset.y);

                    if (buildingConfig.BuildingDef.IsTilePiece) {
                        if (buildingConfig.BuildingDef.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>() != null) {
                            AddVisual(new UtilityVisual(buildingConfig, cell), buildingConfig.BuildingDef);
                        }

                        else {
                            AddVisual(new TileVisual(buildingConfig, cell), buildingConfig.BuildingDef);
                        }
                    }

                    else {
                        AddVisual(new BuildingVisual(buildingConfig, cell), buildingConfig.BuildingDef);
                    }
                }
            }

            foreach (Vector2I digLocation in blueprint.DigLocations) {
                FoundationVisuals.Add(new DigVisual(Grid.XYToCell(topLeft.x + digLocation.x, topLeft.y + digLocation.y), digLocation));
            }

            if (UseBlueprintTool.Instance.GetComponent<UseBlueprintToolHoverCard>() != null) {
                UseBlueprintTool.Instance.GetComponent<UseBlueprintToolHoverCard>().prefabErrorCount = errors;
            }
        }

        private static void AddVisual(IVisual visual, BuildingDef buildingDef) {
            if (buildingDef.IsFoundation) {
                FoundationVisuals.Add(visual);
            }

            else {
                DependentVisuals.Add(visual);
            }

            if (visual is ICleanableVisual) {
                CleanableVisuals.Add((ICleanableVisual) visual);
            }
        }

        public static void UpdateVisual(Vector2I topLeft) {
            CleanDirtyVisuals();

            FoundationVisuals.ForEach(foundationVisual => foundationVisual.MoveVisualizer(Grid.XYToCell(topLeft.x + foundationVisual.Offset.x, topLeft.y + foundationVisual.Offset.y)));
            DependentVisuals.ForEach(dependentVisual => dependentVisual.MoveVisualizer(Grid.XYToCell(topLeft.x + dependentVisual.Offset.x, topLeft.y + dependentVisual.Offset.y)));
        }

        public static void UseBlueprint(Vector2I topLeft) {
            CleanDirtyVisuals();

            FoundationVisuals.ForEach(foundationVisual => foundationVisual.TryUse(Grid.XYToCell(topLeft.x + foundationVisual.Offset.x, topLeft.y + foundationVisual.Offset.y)));
            DependentVisuals.ForEach(dependentVisual => dependentVisual.TryUse(Grid.XYToCell(topLeft.x + dependentVisual.Offset.x, topLeft.y + dependentVisual.Offset.y)));
        }

        public static void ClearVisuals() {
            CleanDirtyVisuals();
            CleanableVisuals.Clear();

            FoundationVisuals.ForEach(foundationVisual => Object.DestroyImmediate(foundationVisual.Visualizer));
            FoundationVisuals.Clear();

            DependentVisuals.ForEach(dependantVisual => Object.DestroyImmediate(dependantVisual.Visualizer));
            DependentVisuals.Clear();
        }

        public static void CleanDirtyVisuals() {
            foreach (int cell in ColoredCells.Keys) {
                CellColorPayload cellColorPayload = ColoredCells[cell];
                TileVisualizer.RefreshCell(cell, cellColorPayload.TileLayer, cellColorPayload.ReplacementLayer);
            }

            ColoredCells.Clear();
            CleanableVisuals.ForEach(cleanableVisual => cleanableVisual.Clean());
        }
    }

    public struct CellColorPayload {
        public Color Color { get; private set; }
        public ObjectLayer TileLayer { get; private set; }
        public ObjectLayer ReplacementLayer { get; private set; }

        public CellColorPayload(Color color, ObjectLayer tileLayer, ObjectLayer replacementLayer) {
            Color = color;
            TileLayer = tileLayer;
            ReplacementLayer = replacementLayer;
        }
    }
}