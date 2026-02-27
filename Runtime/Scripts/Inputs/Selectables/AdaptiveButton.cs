using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Library.Input
{
    /// <summary>
    /// 入力モードに応じて動作するボタンコンポーネント
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class AdaptiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        private Selectable selectable;
        public Selectable Selectable => selectable ??= GetComponent<Selectable>();
        public Button Button => Selectable as Button;
        public Toggle Toggle => Selectable as Toggle;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;


        private void Start()
        {
            // InputModeManagerのイベントを購読
            InputManager.OnInputModeChanged += OnInputModeChanged;
        }

        private void OnDestroy()
        {
            InputManager.OnInputModeChanged -= OnInputModeChanged;
        }


        // マウスイベント（マウスモードでのみ有効）
        public void OnPointerEnter(PointerEventData eventData)
        {
            Selectable.Select();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!InputManager.Instance.IsAlwaysSelectButton)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect.Invoke();
        }


        private void OnInputModeChanged(InputManager.InputMode newMode)
        {
            if (newMode == InputManager.InputMode.MouseKeyboard)
            {
                // マウス・キーボードモード：選択状態をリセット
                if (EventSystem.current.currentSelectedGameObject == gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }


        // 外部から呼び出し可能な選択メソッド
        public void SelectButton()
        {
            if (Selectable.interactable)
            {
                Selectable.Select();
            }
        }

        public bool IsInteractable()
        {
            return Selectable.interactable;
        }
    }
}