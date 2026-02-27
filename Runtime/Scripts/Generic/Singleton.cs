using UnityEngine;

namespace Library.Generic
{
    /// <summary>
    /// シングルトンの基底クラス
    /// </summary>
    /// <typeparam name="T">シングルトンにしたいクラス</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
