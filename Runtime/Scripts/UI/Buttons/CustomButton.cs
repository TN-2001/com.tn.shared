using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Library.UI
{
    [RequireComponent(typeof(Selectable))]
    public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler
    {
        [SerializeField] private Selectable selectable;
        public Selectable Selectable => selectable ??= GetComponent<Selectable>();
        public Button Button => Selectable as Button;
        public Toggle Toggle => Selectable as Toggle;
        public Slider Slider => Selectable as Slider;
        public OptionSelector OptionSelector => Selectable as OptionSelector;
        
        [Header("Text Components")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private TextMeshProUGUI countText;

        [Header("Image Components")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;


        private void Awake()
        {
            if (Slider)
            {
                Slider.onValueChanged.AddListener(OnValueChanged);
                OnValueChanged(Slider.value);
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

        public void OnMove(AxisEventData eventData)
        {
            if (Slider)
            {
                if (Slider.gameObject == gameObject) return;

                switch (eventData.moveDir)
                {
                    case MoveDirection.Left:
                        Slider.value -= Slider.wholeNumbers ? 1 : Slider.maxValue * 0.05f;
                        break;
                    case MoveDirection.Right:
                        Slider.value += Slider.wholeNumbers ? 1 : Slider.maxValue * 0.05f;
                        break;
                }
            }
            else if (OptionSelector)
            {
                if (OptionSelector.gameObject == gameObject) return;

                OptionSelector.OnMove(eventData);
            }
        }


        private void OnValueChanged(float value)
        {
            SetView(count: value.ToString());
        }


        public void SetView(string tiltle = null, string subtitle = null, string count = null, Sprite icon = null, Sprite background = null)
        {
            if (titleText != null && tiltle != null)
                titleText.text = tiltle;
            if (subtitleText != null && subtitle != null)
                subtitleText.text = subtitle;
            if (countText != null && count != null)
                countText.text = count;
            if (iconImage != null && icon != null)
                iconImage.sprite = icon;
            if (backgroundImage != null && background != null)
                backgroundImage.sprite = background;
        }

    }
}
