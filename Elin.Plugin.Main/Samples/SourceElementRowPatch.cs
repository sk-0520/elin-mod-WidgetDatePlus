using Elin.Plugin.Main.PluginHelpers;

namespace Elin.Plugin.Main.Samples
{
    internal class SourceElementRowPatch
    {
        #region function

        public static bool GetTextPrefix(SourceElement.Row __instance, ref string __result, string id, bool returnNull)
        {
            if (id == "textPhase")
            {
                __result = ModHelper.Lang.General.ModSampleLabel + __instance.GetText("name", returnNull);
                return false;
            }

            return true;
        }

        #endregion
    }
}
