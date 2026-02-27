using System.Collections.Generic;
using UnityEngine;

namespace Library.Audio
{
    /// <summary>
    /// 音声データベース
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/AudioDatabase", fileName = "AudioDatabase")]
    public class AudioDatabase : ScriptableObject
    {
        [SerializeField] private List<AudioData> audioList = new List<AudioData>();
        private Dictionary<string, AudioData> audioDict;

        public AudioData GetAudio(string name)
        {
            if (audioDict == null)
            {
                BuildDictionary();
            }


            if (audioDict.TryGetValue(name, out AudioData data))
            {
                return data;
            }
            return null;
        }

        private void BuildDictionary()
        {
            audioDict = new Dictionary<string, AudioData>();
            foreach (var audio in audioList)
            {
                if (!string.IsNullOrEmpty(audio.name))
                {
                    audioDict[audio.name] = audio;
                }
            }
        }
    }
}