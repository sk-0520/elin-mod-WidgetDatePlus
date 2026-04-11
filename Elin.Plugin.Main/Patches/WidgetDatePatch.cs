using Elin.Plugin.Main.Models.Impl;
using Elin.Plugin.Main.Models.Settings;
using HarmonyLib;

namespace Elin.Plugin.Main.Patches
{
    [HarmonyPatch(typeof(WidgetDate))]
    public static class WidgetDatePatch
    {
        #region function

        [HarmonyPatch(nameof(WidgetDate.OnActivate))]
        [HarmonyPrefix]
        public static void OnActivatePrefix(WidgetDate __instance)
        {
            WidgetDateImpl.OnActivatePrefix(__instance);
        }

        [HarmonyPatch(nameof(WidgetDate.OnActivate))]
        [HarmonyPostfix]
        public static void OnActivatePostfix(WidgetDate __instance)
        {
            WidgetDateImpl.OnActivatePostfix(__instance, EMono.world.date);
        }

        [HarmonyPatch(nameof(WidgetDate._Refresh))]
        [HarmonyPostfix]
        public static void _RefreshPostfix(WidgetDate __instance)
        {
            WidgetDateImpl._RefreshPostfix(__instance, EMono.game, EMono.world.date, Setting.Instance);
        }

        #endregion
    }
}
