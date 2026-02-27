using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Library.Audio.Views
{
    /// <summary>
    /// ボタン音再生
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class ButtonAudio : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
    {
        [SerializeField] private AudioData selectAudioData;
        [SerializeField] private AudioData clickAudioData;

        private Button button;
        private bool isIgnoreFrame = true;
        private static GameObject lastSelected;


        private void Awake()
        {
            if (TryGetComponent<Button>(out button))
            {
                button.onClick.AddListener(OnClick);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(InitializeCoroutine());
        }
        private IEnumerator InitializeCoroutine()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }
            isIgnoreFrame = false;

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                lastSelected = gameObject;
            }
        }

        private void OnDisable()
        {
            isIgnoreFrame = true;
        }


        public void OnSelect(BaseEventData eventData)
        {
            if (selectAudioData == null) return;

            if (isIgnoreFrame)
            {
                lastSelected = gameObject;
            }

            if (lastSelected == gameObject) return;

            AudioManager.Instance.PlaySE(selectAudioData);
            lastSelected = gameObject;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            StartCoroutine(OnDeselectCoroutine());
        }
        private IEnumerator OnDeselectCoroutine()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }
            if (EventSystem.current.currentSelectedGameObject != lastSelected)
            {
                lastSelected = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isIgnoreFrame || button != null) return;
            OnClick();
        }
        private void OnClick()
        {
            if (clickAudioData == null) return;

            AudioManager.Instance.PlaySE(clickAudioData);
        }
    }
}
