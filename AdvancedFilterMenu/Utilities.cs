using System.Collections.Generic;
using System.Linq;

namespace AdvancedFilterMenu {
    public static class Utilities {
        public static void CleanVanillaFilters(Dictionary<string, ToolParameterMenu.ToggleState> vanillaFilter) {
            vanillaFilter.Remove(ToolParameterMenu.FILTERLAYERS.ALL);

            ToolParameterMenu.ToggleState overrideState = AdvancedFiltrationAssets.Options.DefaultMenuSelections == DefaultSelections.All ? ToolParameterMenu.ToggleState.On : ToolParameterMenu.ToggleState.Off;
            foreach (string key in vanillaFilter.Keys.ToArray()) {
                vanillaFilter[key] = overrideState;
            }
        }
    }
}
