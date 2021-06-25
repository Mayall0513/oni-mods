using Harmony;
using PeterHan.PLib;
using UnityEngine;

namespace Pliers {
    static class PliersStrings {
        public static LocString STRING_PLIERS_NAME = "Pliers";
        public static LocString STRING_PLIERS_TOOLTIP = "Disconnect utility networks {0}";
        public static LocString STRING_PLIERS_TOOLTIP_TITLE = "PLIERS";
        public static LocString STRING_PLIERS_ACTION_DRAG = "DRAG";
        public static LocString STRING_PLIERS_ACTION_BACK = "BACK";
    }

    public static class PliersAssets {
        public static string PLIERS_TOOLNAME = "PliersTool";
        public static string PLIERS_ICON_NAME = "PLIERS.TOOL.ICON";
        public static Sprite PLIERS_ICON_SPRITE;
        public static Sprite PLIERS_VISUALIZER_SPRITE;
        public static PAction PLIERS_OPENTOOL;
        public static ToolMenu.ToolCollection PLIERS_TOOLCOLLECTION;

        public static Color PLIERS_COLOR_DRAG = new Color32(255, 140, 105, 255);

        public static string PLIERS_PATH_CONFIGFOLDER;
        public static string PLIERS_PATH_CONFIGFILE;
        public static string PLIERS_PATH_KEYCODESFILE;
    }
}