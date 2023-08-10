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
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerControllerCharacter : MonoBehaviour
    {
        private PlayerInput m_playerInput = null;
        private PlayerInputAction m_InputAction = null;
        private CharacterMovement m_CharacterMovement = null;

        private const string SchemeKeyboardMouse = "KeyboardMouse";


        private Vector2 m_InputDirection = Vector2.zero;
        private Vector2 m_InputLook = Vector2.zero;

        [Header("Movement")]
        [SerializeField]
        private float m_WalkSpeed = 2.5f;
  
        [SerializeField]
        private float m_RunSpeed = 5.0f;
        private bool m_IsRunning = false;
        private float m_TargetSpeed = 0.0f;
        [SerializeField]
        private float m_RotationSmoothTime = 0.12f;
        private const float m_SpeedChangeRate = 10.0f;

        [SerializeField]
        private float m_AnimationSpeedBlend = 0;

        private float m_targetRotation = 0.0f;
        private float m_rotationVelocity;

        [Header("Camera")]
        [SerializeField]
        private GameObject m_CinemachineCameraTarget = null;
        private float m_CameraAngleOverride = 0.0f;

        private float m_cinemachineTargetYaw;
        private float m_cinemachineTargetPitch;
        [SerializeField]
        private float m_TopClamp = 70.0f;
        [SerializeField]
        private float m_BottomClamp = -30.0f;
        
        private const float _threshold = 0.01f;

        private AnimaionCharacter m_AnimaionCharacter = null;

        private void Awake()
        {
            Initialized();
        }

        private void OnDestroy()
        {
            UnInitialized();
        }

        private void Update()
        {
            UpdateMovementDirection();
        }

        private void LateUpdate()
        {
            UpdateCameraLook();
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

        #region Input
        private void InitializeInputAction()
        {
            m_playerInput = this.GetComponent<PlayerInput>();
            m_InputAction = new PlayerInputAction();


            m_InputAction.PlayerMap.Movement.performed += OnMovementInput;
            m_InputAction.PlayerMap.Movement.canceled += OnMovementInput;

            m_InputAction.PlayerMap.Look.performed += OnLookInput;
            m_InputAction.PlayerMap.Look.canceled += OnLookInput;

            m_InputAction.PlayerMap.Run.performed += OnRunInput;
            m_InputAction.PlayerMap.Run.canceled += OnRunInput;

            //TODO : Remove
            StartOperation();
        }

        private void UnInitializeInputAction()
        {
            m_InputAction.PlayerMap.Movement.performed -= OnMovementInput;
            m_InputAction.PlayerMap.Movement.canceled -= OnMovementInput;

            m_InputAction.PlayerMap.Look.performed -= OnLookInput;
            m_InputAction.PlayerMap.Look.canceled -= OnLookInput;

            m_InputAction.PlayerMap.Run.performed -= OnRunInput;
            m_InputAction.PlayerMap.Run.canceled -= OnRunInput;

            StopOperation();
        }

        public void StartOperation()
        {
            m_InputAction.PlayerMap.Enable();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void StopOperation() 
        {
            m_InputAction.PlayerMap.Disable();
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            m_InputDirection = context.ReadValue<Vector2>();
        }

        private void OnLookInput(InputAction.CallbackContext context)
        {
            m_InputLook = context.ReadValue<Vector2>();
        }

        private void OnRunInput(InputAction.CallbackContext context)
        {
            //Debug.LogError(context.phase);
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_IsRunning = true;

                    break;

                case InputActionPhase.Canceled:
                    m_IsRunning = false;
                    break;
            }
            //m_MovementInput = context.ReadValue<Vector2>();
        }

        private bool IsCurrentMouseAndKeyBoard
        {
            get
            {
                return m_playerInput.currentControlScheme == SchemeKeyboardMouse;
            }
        }

        #endregion Input
        private void UpdateMovementDirection()
        {
            m_TargetSpeed = m_IsRunning ?  m_RunSpeed : m_WalkSpeed;
            if(m_InputDirection == Vector2.zero)
            {
                m_TargetSpeed = 0.0f;
            }

            m_AnimationSpeedBlend = Mathf.Lerp(m_AnimationSpeedBlend, m_TargetSpeed,Time.deltaTime * m_SpeedChangeRate);
            if(m_AnimationSpeedBlend < 0.01f)
            {
                m_AnimationSpeedBlend = 0.0f;
            }

            Vector3 inputDirection = new Vector3(m_InputDirection.x,0, m_InputDirection.y).normalized;
            if (m_InputDirection != Vector2.zero)
            {
                m_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y,m_targetRotation, ref m_rotationVelocity, m_RotationSmoothTime);
                Quaternion rot = Quaternion.Euler(0, rotation, 0);
                m_CharacterMovement.Rotation(rot);
            }
           
            Vector3 targetDirection = Quaternion.Euler(0, m_targetRotation,0) * Vector3.forward;

            m_CharacterMovement.Move(targetDirection.normalized *(m_TargetSpeed * Time.deltaTime));
            m_AnimaionCharacter.UpdateSpeedMovementAnimation(m_AnimationSpeedBlend);
        }

        private void UpdateCameraLook()
        {
            if (!m_CinemachineCameraTarget)
            {
                return;
            }

            if (m_InputLook.sqrMagnitude >= _threshold)
            {
                float deltaTime = IsCurrentMouseAndKeyBoard ? 1.0f : Time.deltaTime;
                m_cinemachineTargetYaw += m_InputLook.x * deltaTime;
                m_cinemachineTargetPitch += m_InputLook.y * deltaTime;
            }

            m_cinemachineTargetYaw = ClampAngle(m_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            m_cinemachineTargetPitch = ClampAngle(m_cinemachineTargetPitch, m_BottomClamp, m_TopClamp);

            m_CinemachineCameraTarget.transform.rotation = Quaternion.Euler(m_cinemachineTargetPitch + m_CameraAngleOverride,
              m_cinemachineTargetYaw, 0.0f);
        }


        private void InitializeCharacterMovement()
        {
            m_CharacterMovement = this.GetComponent<CharacterMovement>();
        }










        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
