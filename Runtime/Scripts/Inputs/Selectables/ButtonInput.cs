using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Library.Input
{
    [RequireComponent(typeof(Button))]
    public class ButtonInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private string actionName;

        private Button button;
        private InputAction inputAction;

        private void Awake()
        {
            button = GetComponent<Button>();
            if (inputActionAsset != null)
            {
                inputAction = inputActionAsset.FindAction(actionName);
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
