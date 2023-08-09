using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Character;
using UnityEngine.InputSystem;

namespace TPSHorror.InputCharacter
{
    [RequireComponent(typeof(CharacterMovement))]
    public class InputCharacter : MonoBehaviour
    {
        private PlayerInputAction m_InputAction = null;

        [SerializeField]
        private Vector2 m_MovementInput = Vector2.zero;

        private void Awake()
        {
            InitializeInputAction();
        }

        private void OnDestroy()
        {
            UnInitializeInputAction();
        }

        private void InitializeInputAction()
        {
            m_InputAction = new PlayerInputAction();

            m_InputAction.PlayerMap.Movement.performed += OnMovementInput;
            m_InputAction.PlayerMap.Movement.canceled += OnMovementInput;
            StartOperation();
        }

        private void UnInitializeInputAction()
        {
            m_InputAction.PlayerMap.Movement.performed -= OnMovementInput;
            m_InputAction.PlayerMap.Movement.canceled -= OnMovementInput;
            StopOperation();
        }

        public void StartOperation()
        {
            m_InputAction.PlayerMap.Enable();
        }

        public void StopOperation() 
        {
            m_InputAction.PlayerMap.Disable();
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            m_MovementInput = context.ReadValue<Vector2>();
            m_MovementInput = Vector2.ClampMagnitude(m_MovementInput,1);
        }
    }
}
