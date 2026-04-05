using Elin.Plugin.Main.Models.Impl;
using HarmonyLib;

namespace Elin.Plugin.Main.Models.Patches
{
    [HarmonyPatch(typeof(WidgetDate))]
    public static class WidgetDatePatch
    {
        #region function

        [HarmonyPatch(nameof(WidgetDate.OnActivate))]
        [HarmonyPostfix]
        public static void OnActivatePostfix(WidgetDate __instance)
        {
            WidgetDateImpl.OnActivatePostfix(__instance);
        }

        [HarmonyPatch(nameof(WidgetDate._Refresh))]
        [HarmonyPostfix]
        public static void _RefreshPostfix(WidgetDate __instance)
        {
            WidgetDateImpl._RefreshPostfix(__instance, EMono.game, EMono.world.date);
        }

        #endregion
    }
}
