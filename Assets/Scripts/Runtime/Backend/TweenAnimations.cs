using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Runtime.Backend
{
    public static class TweenAnimations
    {
        public static void FadeText(CanvasGroup canvasGroup, TextMeshProUGUI textUGUI, string text, bool isInitial)
        {
            const float fadeDuration = 1f;
            
            if (isInitial)
            {
                textUGUI.text = text;
                canvasGroup.alpha = 0f;
                canvasGroup.DOFade(1f, fadeDuration);
                return;
            }
            
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                textUGUI.text = text;
                canvasGroup.DOFade(1f, fadeDuration);
            });
        }

        public static void RotateTransform(Transform transform, Quaternion targetQuaternion)
        {
            const float rotationDuration = 0.1f;

            transform.DORotateQuaternion(targetQuaternion, rotationDuration)
                .SetEase(Ease.InOutQuad);
        }
    }
}