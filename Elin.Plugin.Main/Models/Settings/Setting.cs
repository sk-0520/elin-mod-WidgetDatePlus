using Elin.Plugin.Generated;

namespace Elin.Plugin.Main.Models.Settings
{
    [GeneratePluginConfig]
    public partial class Setting
    {
        #region property

        internal static Setting Instance { get; set; } = new Setting();

        /// <summary>
        /// アニメーションを有効にするか。
        /// </summary>
        public virtual bool IsAnimationEnabled { get; set; } = true;

        /// <summary>
        /// 時針の角度を分に合わせて変化させるか。
        /// </summary>
        public virtual bool IsHourHandFollowingMinutes { get; set; } = true;

        #endregion
    }
}
