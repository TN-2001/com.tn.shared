using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Library.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Library.SceneManagement
{
    public class SceneTransitionManager : Singleton<SceneTransitionManager>
    {
        [SerializeField] private bool isFade = true;
        [SerializeField, ShowIf(nameof(isFade))] private SceneFader fader;
        [NonSerialized] public UnityEvent onFadeIn = new();
        [NonSerialized] public UnityEvent onFadeOut = new();

        private readonly Stack<string> history = new();
        private bool isTransitioning = false;


        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName).Forget();
        }

        public void Back()
        {
            if (history.Count == 0) return;

            string previousScene = history.Pop();
            LoadSceneAsync(previousScene, false).Forget();
        }

        private async UniTask LoadSceneAsync(string sceneName, bool addToHistory = true)
        {
            if (isTransitioning) return;
            
            isTransitioning = true;

            onFadeIn.Invoke();
            if (isFade) await fader.FadeIn();

            if (addToHistory)
            {
                history.Push(SceneManager.GetActiveScene().name);
            }
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();

            onFadeOut.Invoke();
            if (isFade) await fader.FadeOut();

            isTransitioning = false;
        }
    }
}
