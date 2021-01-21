using STRINGS;
using System.Collections.Generic;

namespace Blueprints {
    public sealed class SnapshotToolHoverCard : HoverTextConfiguration {
        public bool UsingSnapshot { get; set; } = false;

        public SnapshotToolHoverCard() {
            ToolName = Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_TOOLTIP_TITLE);
        }

        public override void UpdateHoverElements(List<KSelectable> hoveredObjects) {
            HoverTextScreen screenInstance = HoverTextScreen.Instance;
            HoverTextDrawer drawer = screenInstance.BeginDrawing();
            drawer.BeginShadowBar(false);

            DrawTitle(screenInstance, drawer);
            drawer.NewLine(26);

            drawer.DrawIcon(screenInstance.GetSprite("icon_mouse_left"), 20);
            drawer.DrawText(UsingSnapshot ? Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_ACTION_CLICK) : Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_CREATE_ACTION_DRAG), Styles_Instruction.Standard);
            drawer.AddIndent(8);

            drawer.DrawIcon(screenInstance.GetSprite("icon_mouse_right"), 20);
            drawer.DrawText(Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_ACTION_BACK), Styles_Instruction.Standard);

            if (UsingSnapshot) {
                drawer.NewLine(32);
                drawer.DrawText(string.Format(Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_SNAPSHOT_NEWSNAPSHOT), UI.FormatAsHotkey("[" + GameUtil.GetActionString(BlueprintsAssets.BLUEPRINTS_MULTI_DELETE.GetKAction()) + "]")), Styles_Instruction.Standard);
            }

            drawer.EndShadowBar();
            drawer.EndDrawing();
        }
    }
}
