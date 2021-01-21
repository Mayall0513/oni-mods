using Harmony;
using UnityEngine;

namespace RemoveOverheatable {
    [HarmonyPatch(typeof(BuildingLoader), "CreateBuildingComplete")]
    public static class BuildingLoader_CreateBuildingComplete {
        public static void Postfix(GameObject __result) {
            if (__result.GetComponent<Overheatable>() != null) {
                Object.Destroy(__result.GetComponent<Overheatable>());
            }
        }
    }
}
