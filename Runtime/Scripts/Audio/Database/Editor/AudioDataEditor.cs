using UnityEngine;
using UnityEditor;

namespace Library.Audio
{
    [CustomEditor(typeof(AudioData))]
    public class AudioDataEditor : Editor
    {
        private AudioSource previewSource;

        public override void OnInspectorGUI()
        {
            // 既存のインスペクタ
            base.OnInspectorGUI();

            AudioData data = (AudioData)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
            
            // ★ 再生中かどうかを判定
            bool isPlaying = previewSource != null && previewSource.isPlaying;

            EditorGUILayout.BeginHorizontal();
            {
                if (!isPlaying)
                {
                    // 停止中 → Play だけ表示
                    if (GUILayout.Button("▶ Play", GUILayout.Width(80)))
                    {
                        PlayClip(data);
                    }
                }
                else
                {
                    // 再生中 → Stop だけ表示
                    if (GUILayout.Button("⏹ Stop", GUILayout.Width(80)))
                    {
                        StopClip();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnDisable()
        {
            StopClip();
        }


        private void PlayClip(AudioData data)
        {
            if (data.Clip == null) return;

            StopClip(); // 多重再生防止

            // プレビュー用の AudioSource
            GameObject go = new GameObject("AudioPreview", typeof(AudioSource));
            go.hideFlags = HideFlags.HideAndDontSave;
            previewSource = go.GetComponent<AudioSource>();
            previewSource.clip = data.Clip;
            previewSource.volume = data.Volume;
            previewSource.Play();
        }

        private void StopClip()
        {
            if (previewSource != null)
            {
                previewSource.Stop();
                DestroyImmediate(previewSource.gameObject);
                previewSource = null;
            }
        }
    }
}
