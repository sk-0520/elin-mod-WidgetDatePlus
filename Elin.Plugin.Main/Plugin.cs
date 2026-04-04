using BepInEx;
using Elin.Plugin.Generated;
using Elin.Plugin.Main.PluginHelpers;
#if DEBUG
using Elin.Plugin.Main.Samples;
#endif
using HarmonyLib;
using System;
using System.Reflection;

// Mod 用テンプレート組み込み想定

namespace Elin.Plugin.Main
{
    [BepInPlugin(Package.Id, Mod.Name, Mod.Version)]
    public class Plugin : BaseUnityPlugin
    {
        #region function

        /// <summary>
        /// 起動時のプラグイン独自処理。
        /// </summary>
        /// <param name="harmony"></param>
        private void AwakePlugin(Harmony harmony)
        {
#if DEBUG
            // サンプル用パッチ処理のため削除してください
            PatchSample(harmony);
#endif
            //NOP
        }

        /// <summary>
        /// 終了時のプラグイン独自処理。
        /// </summary>
        private void OnDestroyPlugin()
        {
            //NOP
        }

        /// <summary>
        /// 起動。
        /// </summary>
        /// <remarks>本メソッドではインフラ面の構築も行っているため、プラグインとしての起動処理は <see cref="AwakePlugin(Harmony)"/> で実施する。</remarks>
        public void Awake()
        {
            ModHelper.Initialize(this, Logger);

            var harmony = new Harmony(Package.Id);

            AwakePlugin(harmony);

            harmony.PatchAll();
        }

        public void OnDestroy()
        {
            OnDestroyPlugin();
            ModHelper.Destroy();
        }

        #endregion


#if DEBUG
        void PatchSample(Harmony harmony)
        {
            // ネストクラスのフル名（例: Namespace.SourceElement+Row）
            var nestedType = AccessTools.TypeByName($"{typeof(SourceElement).FullName}+{nameof(SourceElement.Row)}");

            // オリジナルメソッド（シグネチャを正確に指定）
            var original = AccessTools.Method(nestedType, nameof(SourceElement.Row.GetText), new Type[] { typeof(string), typeof(bool) });

            // Prefix の MethodInfo を取得（このクラス内に static メソッドを用意）
            var prefix = typeof(SourceElementRowPatch).GetMethod(nameof(SourceElementRowPatch.GetTextPrefix),
                BindingFlags.Static | BindingFlags.Public);

            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }
#endif

    }
}
