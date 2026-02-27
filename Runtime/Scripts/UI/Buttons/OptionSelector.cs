using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Library.UI
{
    public class OptionSelector : Selectable
    {
        [SerializeField] private TextMeshProUGUI optionText;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private string[] options = { "No.1", "No.2", "No.3" };

        private int currentIndex = 0;

        public string CurrentOption => options[currentIndex];
        public UnityEvent<string> OnChanged; // 選択変更イベント


        protected override void Awake()
        {
            base.Awake();
            leftButton.onClick.AddListener(PrevOption);
            rightButton.onClick.AddListener(NextOption);
            UpdateUI();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);

            if (eventData.moveDir == MoveDirection.Left)
            {
                PrevOption();
            }
            else if (eventData.moveDir == MoveDirection.Right)
            {
                NextOption();
            }
        }


        public void SetOptions(string[] options, int firstIndex)
        {
            this.options = options;
            SetCurrentIndex(firstIndex);
        }

        public void SetCurrentIndex(int index)
        {
            currentIndex = index;
            UpdateUI();
        }


        private void PrevOption()
        {
            currentIndex = (currentIndex - 1 + options.Length) % options.Length;
            UpdateUI();
        }

        private void NextOption()
        {
            currentIndex = (currentIndex + 1) % options.Length;
            UpdateUI();
        }

        private void UpdateUI()
        {
            optionText.text = options[currentIndex];
            OnChanged?.Invoke(options[currentIndex]);
        }
    }
}
