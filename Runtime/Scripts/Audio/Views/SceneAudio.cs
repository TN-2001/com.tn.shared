using Library.SceneManagement;
using UnityEngine;

namespace Library.Audio
{
    /// <summary>
    /// シーンごとの音声管理
    /// </summary>
    public class SceneAudio : MonoBehaviour
    {
        [SerializeField] private AudioData bgm;

        private void Start()
        {
            SceneTransitionManager.Instance.onFadeIn.AddListener(AudioManager.Instance.FadeOutBGM);

            if (bgm != null)
            {
                AudioManager.Instance.PlayBGM(bgm);
            }
        }
    }
}
