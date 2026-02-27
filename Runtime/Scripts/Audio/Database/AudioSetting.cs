using UnityEngine;

namespace Library.Audio
{
    /// <summary>
    /// 音声設定
    /// </summary>
    [System.Serializable]
    public class AudioSetting
    {
        [SerializeField] private int seVolume = 100;
        [SerializeField] private int bgmVolume = 100;

        public int SEVolume => seVolume;
        public int BGMVolume => bgmVolume;


        public void SetSEVolume(int volume)
        {
            seVolume = Mathf.Clamp(volume, 0, 100);
        }

        public void SetBGMVolume(int volume)
        {
            bgmVolume = Mathf.Clamp(volume, 0, 100);
        }
    }
}
