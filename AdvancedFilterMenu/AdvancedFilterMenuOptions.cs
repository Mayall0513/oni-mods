using Newtonsoft.Json;
using PeterHan.PLib;

namespace AdvancedFilterMenu {
    public enum DefaultSelections {
        All, None
    }

    [JsonObject]
    public class AdvancedFilterMenuOptions {
        [Option("Default Menu Selections", "The default selections made when an advanced filter menu is opened.")]
        public DefaultSelections DefaultMenuSelections { get; set; } = DefaultSelections.All;

        [Option("Cancel Tool Overlay Sync", "Whether the Cancel Tool syncs with the current overlay. (configurable in game too)")]
        public bool CancelToolSync { get; set; } = true;

        [Option("Deconstruct Tool Overlay Sync", "Whether the Deconstruct Tool syncs with the current overlay. (configurable in game too)")]
        public bool DeconstructToolSync { get; set; } = true;

        [Option("Prioritize Tool Overlay Sync", "Whether the Prioritize Tool syncs with the current overlay. (configurable in game too)")]
        public bool PrioritizeSync { get; set; } = true;

        [Option("Empty Pipe Tool Overlay Sync", "Whether the Empty Pipe Tool syncs with the current overlay. (configurable in game too)")]
        public bool EmptyPipeSync { get; set; } = true;
    }
}
