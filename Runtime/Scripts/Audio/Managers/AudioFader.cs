using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Library.Audio
{
    /// <summary>
    /// 音声フェード管理
    /// </summary>
    [System.Serializable]
    public class AudioFader
    {
        [SerializeField] private float fadeDuration = 0.1f;

        public async UniTask FadeIn(AudioSource source, float endVolume)
        {
            source.volume = 0f;
            source.DOKill(); // 既存のTweenを止める（安全対策）

            await DOTween
                .To(() => source.volume, x => source.volume = x, endVolume, fadeDuration)
                .SetUpdate(true) // Time.timeScaleの影響を受けない
                .AsyncWaitForCompletion();
        }

        public async UniTask FadeOut(AudioSource source)
        {
            source.DOKill();

            await DOTween
                .To(() => source.volume, x => source.volume = x, 0f, fadeDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}
