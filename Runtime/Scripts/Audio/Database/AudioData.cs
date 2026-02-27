using UnityEngine;

namespace Library.Audio
{
    /// <summary>
    /// 音声の種類
    /// </summary>
    public enum AudioType
    {
        UI,
        SE,
        BGM,
        Voice
    }

    /// <summary>
    /// 音声データ
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/AudioData", fileName = "AudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private new string name;            // 再生時のキー
        [SerializeField] private AudioClip clip;             // 実際の音
        [SerializeField] private AudioType type = AudioType.SE;     // UI / SE / BGM / Voice
        [SerializeField, Range(0f, 1f)] private float volum = 1f;   // デフォルト音量

        public string Name => name;
        public AudioClip Clip => clip;
        public AudioType Type => type;
        public float Volume => volum;
    }
}
