using System.IO;
using UnityEngine;

namespace Library.Utilities
{
    public static class SaveLoadUtility
    {
        /// <summary>
        /// Application.persistentDataPathのPath
        /// Windows Editor: C:\Users\<ユーザー名>\AppData\LocalLow\<会社名>\<プロジェクト名>
        /// </summary>

        public static bool TryLoad<T>(string fileName, out T result)
        {
            try
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                if (!PlayerPrefs.HasKey(fileName))
                {
                    result = default;
                    return false;
                }
                string json = PlayerPrefs.GetString(fileName);
#else
                string path = Path.Combine(Application.persistentDataPath, fileName);
                if (!File.Exists(path))
                {
                    result = default;
                    return false;
                }
                string json = File.ReadAllText(path);
#endif

                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "{}")
                {
                    result = default;
                    return false;
                }

                result = JsonUtility.FromJson<T>(json);
                return result != null;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load {typeof(T).Name}: {e}");
                result = default;
                return false;
            }

        }
        

        public static void Save<T>(T data, string fileName)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);

#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.SetString(fileName, json);
                PlayerPrefs.Save();
#else
                string path = Path.Combine(Application.persistentDataPath, fileName);
                File.WriteAllText(path, json);
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save {typeof(T).Name}: {e}");
            }
        }
    }
}
