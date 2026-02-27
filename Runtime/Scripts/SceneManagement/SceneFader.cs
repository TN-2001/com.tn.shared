using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Library.SceneManagement
{
    [System.Serializable]
    public class SceneFader
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 0.1f;

        public async UniTask FadeIn()
        {
            if (fadeImage == null) return;

            fadeImage.DOKill();
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);

            await fadeImage
                .DOFade(1f, fadeDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        public async UniTask FadeOut()
        {
            if (fadeImage == null) return;

            fadeImage.DOKill();
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

            await fadeImage
                .DOFade(0f, fadeDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}
