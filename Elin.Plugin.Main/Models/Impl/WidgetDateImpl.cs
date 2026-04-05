using Elin.Plugin.Generated;
using Elin.Plugin.Main.PluginHelpers;
using UnityEngine.UI;

namespace Elin.Plugin.Main.Models.Impl
{
    public static class WidgetDateImpl
    {
        #region property

        private static Image? ImageMinutes { get; set; }

        #endregion

        #region WidgetDate

        public static void OnActivatePostfix(WidgetDate instance)
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

            // 時針に対する分針
            // TODO: お尻が伸びるのだっせぇなぁ、原点に合わせて後ろ縮める楽な方法がぱっと思いつかない
            var scale = 1.3f;

            // 何でもかんでもコピーしとけば安心の精神
            var srcRt = imageHour.rectTransform;
            var dstRt = clonedImage.rectTransform;
            dstRt.anchorMin = srcRt.anchorMin;
            dstRt.anchorMax = srcRt.anchorMax;
            dstRt.anchoredPosition = srcRt.anchoredPosition;
            dstRt.sizeDelta = new Vector2(srcRt.sizeDelta.x * scale, srcRt.sizeDelta.y);
            dstRt.pivot = srcRt.pivot;
            dstRt.localRotation = srcRt.localRotation;
            dstRt.localScale = srcRt.localScale;
            // 全部コピーしとけの心
            clonedImage.color = imageHour.color;
            clonedImage.material = imageHour.material;
            clonedImage.raycastTarget = imageHour.raycastTarget;
            clonedImage.type = imageHour.type;
            clonedImage.preserveAspect = imageHour.preserveAspect;
            clonedImage.sprite = imageHour.sprite;

            // 前へ
            clonedGameObject.transform.SetSiblingIndex(imageHour.transform.GetSiblingIndex() + 1);
            clonedGameObject.SetActive(true);

            ImageMinutes = clonedImage;
        }

        public static void _RefreshPostfix(WidgetDate instance, Game game, GameDate date)
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
                // OnActivate の時点で _Refresh が呼び出されるが、
                // OnActivate は Postfix で実装しているので ImageMinutes はまだ作られていない可能性あり。
                // 次回更新は 0.1 秒後なので待っとけばいい。
                // もともと OnActivate は Prefix で実装していたが、リソースコピー処理するので Postfix に変更した。
                // Unity の仕組み知らんから分からんけど DI コンテナ的なもので依存性注入されてそうだから問題ないと思うけどさ。
                return;
            }

            var minute = date.min;
            var angle = -minute * 6f + 90f;

            ImageMinutes.rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }

        #endregion
    }
}
