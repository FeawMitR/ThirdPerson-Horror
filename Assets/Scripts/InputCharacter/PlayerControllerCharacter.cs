using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Character;
using UnityEngine.InputSystem;
using TPSHorro.AnimaionCharacter;

namespace TPSHorror.PlayerControllerCharacter
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(AnimaionCharacter))]
    public class PlayerControllerCharacter : MonoBehaviour
    {
        private PlayerInputAction m_InputAction = null;
        private CharacterMovement m_CharacterMovement = null;

    
        private Vector2 m_MovementInput = Vector2.zero;
        private Vector3 m_MovementDirection = Vector3.zero;

        [SerializeField]
        private float m_WalkSpeed = 2.5f;
        [SerializeField]
        private float m_RunSpeed = 5.0f;

        [SerializeField]
        private bool m_IsRunning = false;
        [SerializeField]
        private float m_runLengthInput = 0;

        private AnimaionCharacter m_AnimaionCharacter = null;

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

            m_AnimaionCharacter = this.GetComponent<AnimaionCharacter>();
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

            m_InputAction.PlayerMap.Run.performed += OnRunInput;
            m_InputAction.PlayerMap.Run.canceled += OnRunInput;

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

        private void OnRunInput(InputAction.CallbackContext context)
        {
            //Debug.LogError(context.phase);
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    if(m_MovementInput != Vector2.zero)
                    {
                        m_IsRunning = true;
                    }
                    else
                    {
                        m_IsRunning = false;
                    }
                   
                    //m_runLengthInput = 1.0f;
                    //m_CharacterMovement.MoveSpeed = m_RunSpeed;
                    break;

                case InputActionPhase.Canceled:
                    m_IsRunning = false;
                    //m_runLengthInput = 0.0f;
                    //m_CharacterMovement.MoveSpeed = m_WalkSpeed;
                    break;
            }
            //m_MovementInput = context.ReadValue<Vector2>();
        }

        private void UpdateMovementDirection()
        {
        
            if (m_MovementInput != Vector2.zero)
            {
                Vector3 newInput = new Vector3(m_MovementInput.x,0, m_MovementInput.y);
                float targetRotation = Quaternion.LookRotation(newInput).eulerAngles.y + Camera.main.transform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0,targetRotation,0);
                m_CharacterMovement.Rotation(rotation);
                m_MovementDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
                //Debug.LogError($"m_MovementDirection : {m_MovementDirection}");
                //m_MovementDirection = Vector3.ClampMagnitude(m_MovementDirection, 1);
            }
            else
            {
                m_MovementDirection = Vector3.zero;
            }

            if (m_IsRunning)
            {
                m_CharacterMovement.MoveSpeed = m_RunSpeed;
                m_runLengthInput = 1.0f;
            }
            else
            {
                m_CharacterMovement.MoveSpeed = m_WalkSpeed;
                m_runLengthInput = 0.0f;
            }

            m_CharacterMovement.Move(m_MovementDirection);
            m_AnimaionCharacter.UpdateSpeedMovementAnimation(m_MovementDirection.magnitude + m_runLengthInput);
        }



        private void InitializeCharacterMovement()
        {
            m_CharacterMovement = this.GetComponent<CharacterMovement>();

            m_CharacterMovement.MoveSpeed = m_WalkSpeed;
        }
    }
}