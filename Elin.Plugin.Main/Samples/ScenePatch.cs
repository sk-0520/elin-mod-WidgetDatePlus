using Elin.Plugin.Main.PluginHelpers;
using HarmonyLib;

#if DEBUG
namespace Elin.Plugin.Main.Samples
{
    [HarmonyPatch(typeof(Scene))]
    public class ScenePatch
    {
        #region function

        [HarmonyPatch(nameof(Scene.Init), new[] { typeof(Scene.Mode) })]
        [HarmonyPostfix]
        public static void InitPostfix(Scene.Mode newMode)
        {
            switch (newMode)
            {
                case Scene.Mode.Title:
                    ModHelper.WriteDebug("DEBUG START!");
                    break;

                case Scene.Mode.StartGame:
                    ModHelper.LogDebug(ModHelper.Lang.General.HelloWorld);
                    ModHelper.LogDebug(ModHelper.Lang.Formatter.FormatHelloFormat(a: 999, b: 1, c: 1000));
                    ModHelper.LogDebug(ModHelper.ToStringFromInformation());
                    break;

                default:
                    break;
            }
        }

        #endregion
    }

}
#endif
