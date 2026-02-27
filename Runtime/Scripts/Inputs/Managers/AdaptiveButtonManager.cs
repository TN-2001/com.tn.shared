using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Library.Input
{
    /// <summary>
    /// AdaptiveButtonを管理し、入力モードに応じて適切なボタンを選択するマネージャークラス
    /// </summary>
    public class AdaptiveButtonManager : MonoBehaviour
    {
        [SerializeField] private List<AdaptiveButton> registeredButtons = new List<AdaptiveButton>();
        [SerializeField] private AdaptiveButton currentAdaptiveButton;
        public AdaptiveButton CurrentAdaptiveButton => currentAdaptiveButton;


        private void Start()
        {
            // 入力モード変更イベントを購読
            InputManager.OnInputModeChanged += OnInputModeChanged;

            // 既存のAdaptiveButtonを自動登録
            RegisterAllButtons();

            OnEnable();
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                if (InputManager.Instance.IsAlwaysSelectButton)
                {
                    // デフォルトボタンを選択
                    SelectDefaultSelectButton();
                }
            }
        }

        private void Update()
        {
            if (InputManager.Instance.IsAlwaysSelectButton)
            {
                // 選択されているボタンがない場合、最初の使用可能なボタンを選択
                foreach (AdaptiveButton button in registeredButtons)
                {
                    if (button != null && button.IsInteractable())
                    {
                        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
                        {
                            return;
                        }
                    }
                }
                SelectDefaultSelectButton();
            }
        }

        private void OnDestroy()
        {
            InputManager.OnInputModeChanged -= OnInputModeChanged;
        }


        private void OnInputModeChanged(InputManager.InputMode newMode)
        {
            if (InputManager.Instance.IsAlwaysSelectButton)
            {
                // デフォルトボタンを選択
                SelectDefaultSelectButton();
            }
            else
            {
                // 選択を解除
                EventSystem.current.SetSelectedGameObject(null);
            }
        }


        // すべての子のAdaptiveButtonを登録
        public void RegisterAllButtons()
        {
            registeredButtons = new List<AdaptiveButton>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<AdaptiveButton>(out AdaptiveButton button))
                {
                    button.onSelect.AddListener(() => { currentAdaptiveButton = button; });
                    registeredButtons.Add(button);
                }
            }
        }

        // 最初の使用可能なボタンを選択
        private void SelectDefaultSelectButton()
        {
            AdaptiveButton buttonToSelect = null;

            // デフォルトボタンが設定されていて使用可能な場合
            if (currentAdaptiveButton != null && currentAdaptiveButton.IsInteractable())
            {
                buttonToSelect = currentAdaptiveButton;
            }
            else
            {
                // 最初の使用可能なボタンを探す
                foreach (AdaptiveButton button in registeredButtons)
                {
                    if (button != null)
                    {
                        if (button.IsInteractable())
                        {
                            buttonToSelect = button;
                            break;
                        }
                    }
                }
            }

            if (buttonToSelect != null)
            {
                buttonToSelect.SelectButton();
            }
        }
    }
}