using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Character;
using UnityEngine.InputSystem;

namespace TPSHorror.PlayerControllerCharacter
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerControllerCharacter : MonoBehaviour
    {
        private PlayerInputAction m_InputAction = null;
        private CharacterMovement m_CharacterMovement = null;

    
        private Vector2 m_MovementInput = Vector2.zero;
        private Vector3 m_MovementDirection = Vector3.zero;
  
        private void Awake()
        {
            Initialized();
        }

        private void OnDestroy()
        {
            UnInitialized();
        }

        private void FixedUpdate()
        {
            UpdateMovementDirection();
        }


        private void Initialized()
        {
            InitializeCharacterMovement();
            InitializeInputAction();
        }

        private void UnInitialized()
        {
            UnInitializeInputAction();
        }


        private void InitializeInputAction()
        {
            m_InputAction = new PlayerInputAction();


            m_InputAction.PlayerMap.Movement.performed += OnMovementInput;
            m_InputAction.PlayerMap.Movement.canceled += OnMovementInput;

            //TODO : Remove
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
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void StopOperation() 
        {
            m_InputAction.PlayerMap.Disable();
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            m_MovementInput = context.ReadValue<Vector2>();
        }

        private void UpdateMovementDirection()
        {
            if(m_MovementInput != Vector2.zero)
            {
                Vector3 newInput = new Vector3(m_MovementInput.x,0, m_MovementInput.y);
                float targetRotation = Quaternion.LookRotation(newInput).eulerAngles.y + Camera.main.transform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0,targetRotation,0);
                m_CharacterMovement.Rotation(rotation);
                m_MovementDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            }
            else
            {
                m_MovementDirection = Vector3.zero;
            }

            m_CharacterMovement.Move(m_MovementDirection);
        }



        private void InitializeCharacterMovement()
        {
            m_CharacterMovement = this.GetComponent<CharacterMovement>();
        }
    }
}
