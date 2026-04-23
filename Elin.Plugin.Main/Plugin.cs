using Elin.Plugin.Main.Models.Settings;
using System.Linq;

namespace Elin.Plugin.Main
{
    partial class Plugin
    {
        #region function

        /// <summary>
        /// 起動時のプラグイン独自処理。
        /// </summary>
        private void AwakePlugin()
        {
            var defaultSetting = new Setting();
            Setting.Instance = Setting.Bind(Config, defaultSetting);
        }

        /// <summary>
        /// 終了時のプラグイン独自処理。
        /// </summary>
        private void OnDestroyPlugin()
        {
            //NOP
        }

#if DEBUG
        public void PHL()
        {
            var w = EMono.ui.widgets.list.OfType<WidgetDate>().FirstOrDefault();
            if (w != null)
            {
                w.OnActivate();
            }
        }
#endif

        #endregion
    }
}
