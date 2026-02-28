using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Library.Input
{
    [RequireComponent(typeof(Button))]
    public class ButtonInput : MonoBehaviour
    {
        [SerializeField] private string actionName;

        private Button button;
        private InputAction inputAction;

        private void Awake()
        {
            button = GetComponent<Button>();

            try
            {
                if (actionName != "")
                {
                    inputAction = InputSystem.actions[actionName];
                }
            }
            catch
            {
                Debug.Log($"Input action '{actionName}' not found in the Input System.");
            }
        }

        private void OnEnable()
        {
            if (inputAction == null) return;
            inputAction.Enable();
            inputAction.performed += OnClick;
        }

        private void OnDisable()
        {
            if (inputAction == null) return;
            inputAction.performed -= OnClick;
            inputAction.Disable();
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            button.onClick?.Invoke();
        }
    }
}
