using UnityEngine;

namespace Library.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public void PlaySE(AudioData data)
        {
            AudioManager.Instance.PlaySE(data);
        }
    }
}
