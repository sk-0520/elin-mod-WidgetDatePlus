using Elin.Plugin.Generated;
using Elin.Plugin.Main.Models.Settings;
using Elin.Plugin.Main.PluginHelpers;
using UnityEngine.UI;

namespace Elin.Plugin.Main.Models.Impl
{
    public static class WidgetDateImpl
    {
        #region define

        // 補間時間（秒）。小さくすると速く追従する
        private const float SmoothTime = 0.05f;

        #endregion

        #region property

        private static Image? ImageMinutes { get; set; }
        private static Vector2? SourceScale { get; set; }

        // 角度補間用状態
        private static float CurrentHourAngle { get; set; }
        private static float CurrentMinuteAngle { get; set; }
        private static float hourVelocity;
        private static float minuteVelocity;

        #endregion

        #region function

        // 常に current 以下（時計回り）
        private static float CalculateClockwiseTargetAngle(float currentUnwrappedAngle, float targetWrappedAngle)
        {
            var delta = Mathf.Repeat(currentUnwrappedAngle - targetWrappedAngle, 360f); // 0..360
            return currentUnwrappedAngle - delta;
        }

        #endregion

        #region WidgetDate

        public static void OnActivatePrefix(WidgetDate instance)
        {
            ImageMinutes = null;
            if (SourceScale != null)
            {
                var rt = instance.imageHour.rectTransform;
                rt.sizeDelta = SourceScale.Value;
            }
            else
            {
                SourceScale = instance.imageHour.rectTransform.sizeDelta;
            }
        }

        public static void OnActivatePostfix(WidgetDate instance, GameDate date)
        {
            var imageHour = instance.imageHour;

            // imageHour の GameObject を複製して分針用にする
            var clonedGameObject = Object.Instantiate(imageHour.gameObject, imageHour.transform.parent);
            clonedGameObject.name = $"{Package.Id}_Minute";

            var clonedImage = clonedGameObject.GetComponent<Image>();
            if (clonedImage == null)
            {
                ModHelper.LogNotExpected("複製に Image コンポーネントが見つかりません。");
                Object.Destroy(clonedGameObject);
                return;
            }

            var minuteScale = (x: 1.3f, y: 0.8f);

            // 何でもかんでもコピーしとけば安心の精神
            var srcRt = imageHour.rectTransform;
            var dstRt = clonedImage.rectTransform;
            dstRt.anchorMin = srcRt.anchorMin;
            dstRt.anchorMax = srcRt.anchorMax;
            dstRt.anchoredPosition = srcRt.anchoredPosition;
            dstRt.sizeDelta = new Vector2(srcRt.sizeDelta.x * minuteScale.x, srcRt.sizeDelta.y * minuteScale.y);
            dstRt.pivot = srcRt.pivot;
            dstRt.localRotation = srcRt.localRotation;
            dstRt.localScale = srcRt.localScale;

            clonedImage.color = imageHour.color;
            clonedImage.material = imageHour.material;
            clonedImage.raycastTarget = imageHour.raycastTarget;
            clonedImage.type = imageHour.type;
            clonedImage.preserveAspect = imageHour.preserveAspect;
            clonedImage.sprite = imageHour.sprite;

            clonedGameObject.transform.SetSiblingIndex(imageHour.transform.GetSiblingIndex() + 1);
            clonedGameObject.SetActive(true);

            ImageMinutes = clonedImage;

            // 初期化：現在角度をその場の角度に合わせる（急なジャンプを防ぐ）
            var minuteAngle = -date.min * 6f + 90f;
            var hour12 = date.hour % 12;
            var hourWithMinute = hour12 + date.min / 60f;
            var hourAngle = -hourWithMinute * 30f + 90f;

            CurrentMinuteAngle = minuteAngle;
            CurrentHourAngle = hourAngle;
            hourVelocity = 0f;
            minuteVelocity = 0f;

            // やけくそで時針を太らす
            var hourScale = (x: 0.8f, y: 1.2f);
            srcRt.sizeDelta = new Vector2(SourceScale!.Value.x * hourScale.x, SourceScale!.Value.y * hourScale.y);
        }

        public static void _RefreshPostfix(WidgetDate instance, Game game, GameDate date, Setting setting)
        {
            // [ELIN:WidgetDate._Refresh]
            // -> if (EMono.game.activeZone == null)
            if (game.activeZone == null)
            {
                return;
            }

            if (!instance.extra.clock)
            {
                return;
            }

            if (ImageMinutes == null)
            {
                return;
            }

            // 目標角度(設定に関係なく時針も計算する)
            var minuteTarget = -date.min * 6f + 90f;
            var hourTarget = -((date.hour % 12) + date.min / 60f) * 30f + 90f;

            if (setting.IsAnimationEnabled)
            {
                var minuteTargetUnwrapped = CalculateClockwiseTargetAngle(CurrentMinuteAngle, minuteTarget);
                var hourTargetUnwrapped = CalculateClockwiseTargetAngle(CurrentHourAngle, hourTarget);

                // SmoothDampAngle は最短経路を取るため使わない
                CurrentMinuteAngle = Mathf.SmoothDamp(CurrentMinuteAngle, minuteTargetUnwrapped, ref minuteVelocity, SmoothTime, Mathf.Infinity, Time.deltaTime);
                CurrentHourAngle = Mathf.SmoothDamp(CurrentHourAngle, hourTargetUnwrapped, ref hourVelocity, SmoothTime, Mathf.Infinity, Time.deltaTime);
            }
            else
            {
                CurrentMinuteAngle = minuteTarget;
                CurrentHourAngle = hourTarget;
            }

            ImageMinutes.rectTransform.localEulerAngles = new Vector3(0f, 0f, Mathf.Repeat(CurrentMinuteAngle, 360f));
            if (setting.IsHourHandFollowingMinutes)
            {
                instance.imageHour.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Repeat(CurrentHourAngle, 360f));
            }
        }

        #endregion
    }
}
