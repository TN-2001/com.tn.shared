using Cysharp.Threading.Tasks;
using Library.Attributes;
using Library.Generic;
using Library.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace Library.Audio
{
    /// <summary>
    /// 音声管理
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioDatabase database;
        [SerializeField, ReadOnly] private AudioSetting setting;
        [SerializeField] private AudioSource seSource;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioFader fader;

        public int SEVolume => setting.SEVolume;
        public int BGMVolume => setting.BGMVolume;


        protected override void Awake()
        {
            base.Awake();
            Load();
            SetSEVolume(setting.SEVolume);
            SetBGMVolume(setting.BGMVolume);
        }

        private void OnDisable()
        {
            if (Instance != this) return;

            Save();
        }


        // セーブ・ロード
        public void Load()
        {
            if (SaveLoadUtility.TryLoad("AudioSetting.json", out AudioSetting result))
            {
                setting = result;
            }
            else
            {
                setting = new AudioSetting();
                Save();
            }
        }
        public void Save()
        {
            SaveLoadUtility.Save(setting, "AudioSetting.json");
        }


        // 再生管理
        public void PlaySE(AudioData data)
        {
            if (data == null || data.Type == AudioType.BGM) return;

            seSource.PlayOneShot(data.Clip, data.Volume);
        }
        public void PlayBGM(AudioData data)
        {
            if (data == null || data.Type != AudioType.BGM) return;

            bgmSource.clip = data.Clip;
            bgmSource.loop = true;
            bgmSource.volume = data.Volume;
            bgmSource.Play();
        }

        //  音量管理
        public void SetSEVolume(float value)
        {
            setting.SetSEVolume(Mathf.RoundToInt(value));
            float dB = Mathf.Log10(Mathf.Clamp(value/100, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("SE", dB);
        }
        public void SetBGMVolume(float value)
        {
            setting.SetBGMVolume(Mathf.RoundToInt(value));
            float dB = Mathf.Log10(Mathf.Clamp(value/100, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("BGM", dB);
        }

        // フェード管理
        public void FadeInBGM(float endVolume = 1f)
        {
            fader.FadeIn(bgmSource, endVolume).Forget();
        }
        public void FadeOutBGM()
        {
            fader.FadeOut(bgmSource).Forget();
        }

        // ミュート管理
        public void MuteSE(bool mute)
        {
            seSource.mute = mute;
        }
        public void MuteBGM(bool mute)
        {
            bgmSource.mute = mute;
        }
    }
}
