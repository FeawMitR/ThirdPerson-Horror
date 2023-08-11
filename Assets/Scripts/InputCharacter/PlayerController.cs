using Cinemachine;
using TPSHorro.AnimaionCharacter;
using TPSHorror.Character;
using TPSHorror.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;
using TPSHorror.PerceptionSystem;

namespace TPSHorror.PlayerControllerCharacter
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(AnimaionCharacter))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PerceptionFieldOfViewSign))]
    public class PlayerController : MonoBehaviour
    {
        private bool m_IsOperating = false;

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

        [SerializeField]
        private float m_CrouchedSpeed = 1.5f;
        private bool m_IsCrouched = false;
        private System.Action<bool> onCrouchedHandler;

        [SerializeField]
        private float m_heightStand = 2.0f;
        [SerializeField]
        private float m_heightCrouched = 1.0f;



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
        private CinemachineVirtualCamera m_ThirdPerson = null;
        [SerializeField]
        private GameObject m_CameraTargetStand = null;
        [SerializeField]
        private GameObject m_CameraTargetCrouched = null;
        private GameObject m_CameraTarget = null;

        private float m_CameraAngleOverride = 0.0f;

        private float m_cinemachineTargetYaw;
        private float m_cinemachineTargetPitch;
        [SerializeField]
        private float m_TopClamp = 70.0f;
        [SerializeField]
        private float m_BottomClamp = -30.0f;  
        private const float _threshold = 0.01f;

        private AnimaionCharacter m_AnimaionCharacter = null;

        [Header("Interaction")]
        [SerializeField]
        private float m_RadiusFindInteraction = 1.0f;
        [SerializeField]
        private float m_MaxLegnthFindInteraction = 3.0f;
        private int m_InteractionIgnoreLayer = 6;
        private int m_InteractionAbleLayer = 7;
        //[SerializeField]
        private LayerMask m_InteractionLayerMask;
        //[SerializeField]
        private LayerMask m_InteractionAbleLayerMask;
        private IInteractAble m_CurrentInteraction = null;

        [Header("Perception FieldOfView Sign")]
        [SerializeField]
        private PerceptionFieldOfViewSign m_PerceptionFieldOfViewSign = null;

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
            if (!m_IsOperating)
            {
                return;
            }

            UpdateMovementDirection();
           
        }

        private void LateUpdate()
        {
            if (!m_IsOperating)
            {
                return;
            }

            UpdateCameraLook();
        }

        private void FixedUpdate()
        {
            if (!m_IsOperating)
            {
                return;
            }

            FindObjectToInteraction();
        }


        public void StartOperation()
        {
            m_InputAction.PlayerMap.Enable();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            m_IsOperating = true;
        }

        public void StopOperation()
        {
            m_InputAction.PlayerMap.Disable();
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            m_IsOperating = false;
        }


        private void Initialized()
        {
            InitializeCharacterMovement();
            InitializeInputAction();
            InitializeInteractionCheck();
            InitializPerseptionFieldOfViewSign();

            m_AnimaionCharacter = this.GetComponent<AnimaionCharacter>();
            m_CameraTarget = m_CameraTargetStand;
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
            m_InputAction.PlayerMap.Run.started += OnRunInput;
            m_InputAction.PlayerMap.Run.canceled += OnRunInput;

            m_InputAction.PlayerMap.Crouched.started += OnCrouchedInput;
            onCrouchedHandler += OnCrouched;

            m_InputAction.PlayerMap.Interaction.started += OnInteractionInput;

        }

        private void UnInitializeInputAction()
        {
            if(m_InputAction != null)
            {
                m_InputAction.PlayerMap.Movement.performed -= OnMovementInput;
                m_InputAction.PlayerMap.Movement.canceled -= OnMovementInput;

                m_InputAction.PlayerMap.Look.performed -= OnLookInput;
                m_InputAction.PlayerMap.Look.canceled -= OnLookInput;

                m_InputAction.PlayerMap.Run.performed -= OnRunInput;
                m_InputAction.PlayerMap.Run.started -= OnRunInput;
                m_InputAction.PlayerMap.Run.canceled -= OnRunInput;

                m_InputAction.PlayerMap.Crouched.started -= OnCrouchedInput;


                m_InputAction.PlayerMap.Interaction.started -= OnInteractionInput;
            }
        

            onCrouchedHandler -= OnCrouched;
        }

     


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            m_InputDirection = context.ReadValue<Vector2>();
            if(context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
            {
                if (m_IsRunning)
                {
                    m_IsRunning = false;
                }
            }
        }

        private void OnLookInput(InputAction.CallbackContext context)
        {
            m_InputLook = context.ReadValue<Vector2>();
        }

        private void OnRunInput(InputAction.CallbackContext context)
        {

            if (IsCurrentMouseAndKeyBoard)
            {
                switch (context.phase)
                {
                    case InputActionPhase.Performed:

                        m_IsRunning = true;

                        onCrouchedHandler?.Invoke(false);
                        break;

                    case InputActionPhase.Canceled:
                        m_IsRunning = false;
                        break;
                }
            }
            else
            {
                if (context.phase != InputActionPhase.Started)
                {
                    return;
                }

                m_IsRunning = !m_IsRunning;
                if (m_IsRunning)
                {
                    onCrouchedHandler?.Invoke(false);
                }
            }
           
        }

        private void OnCrouchedInput(InputAction.CallbackContext context)
        {
            if(context.phase != InputActionPhase.Started)
            {
                return;
            }

            bool isCrouched = !m_IsCrouched;
            if (m_IsRunning)
            {
                isCrouched = false;
            }

            onCrouchedHandler?.Invoke(isCrouched);
        }

        private void OnInteractionInput(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started)
            {
                return;
            }

            if (m_CurrentInteraction != null)
            {
                m_CurrentInteraction.StartInteract();
                m_CurrentInteraction = null;
            }
        }

        private bool IsCurrentMouseAndKeyBoard
        {
            get
            {
                return m_playerInput.currentControlScheme == SchemeKeyboardMouse;
            }
        }

        

        #endregion Input

        #region Movement,Look & Crouched
        private void UpdateMovementDirection()
        {
            m_TargetSpeed = GetTargetSpeed;

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

        private float GetTargetSpeed
        {
            get
            {
                if (m_IsCrouched)
                {
                    return m_CrouchedSpeed;
                }

                if (m_IsRunning)
                {
                    return m_RunSpeed;
                }

                return m_WalkSpeed;
            }
        }

        private void OnCrouched(bool isCrouhced)
        {
            m_IsCrouched = isCrouhced;
            m_AnimaionCharacter.UpdateIsCrouchedAnimation(m_IsCrouched);
            
          
            if (!m_IsCrouched)
            {
                m_CameraTarget = m_CameraTargetStand;
                m_CharacterMovement.CharacterController.height = m_heightStand;
                m_CharacterMovement.CharacterController.center = new Vector3(0, m_heightStand / 2, 0);

                ChangePerseptionFieldOfViewSign(m_CameraTargetStand.transform);
            }
            else
            {
                m_CameraTarget = m_CameraTargetCrouched;
                m_CharacterMovement.CharacterController.height = m_heightCrouched;
                m_CharacterMovement.CharacterController.center = new Vector3(0, m_heightCrouched / 2, 0);

                ChangePerseptionFieldOfViewSign(m_CameraTargetCrouched.transform);
            }
            m_ThirdPerson.m_Follow = m_CameraTarget.transform;
        }


        private void UpdateCameraLook()
        {
            if (m_InputLook.sqrMagnitude >= _threshold)
            {
                float deltaTime = IsCurrentMouseAndKeyBoard ? 1.0f : Time.deltaTime;
                m_cinemachineTargetYaw += m_InputLook.x * deltaTime;
                m_cinemachineTargetPitch += m_InputLook.y * deltaTime;
            }

            m_cinemachineTargetYaw = ClampAngle(m_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            m_cinemachineTargetPitch = ClampAngle(m_cinemachineTargetPitch, m_BottomClamp, m_TopClamp);

            if (m_CameraTargetStand)
            {
                m_CameraTargetStand.transform.rotation = Quaternion.Euler(m_cinemachineTargetPitch + m_CameraAngleOverride,m_cinemachineTargetYaw, 0.0f);
            }
           

            if (m_CameraTargetCrouched)
            {
                m_CameraTargetCrouched.transform.rotation = Quaternion.Euler(m_cinemachineTargetPitch + m_CameraAngleOverride,m_cinemachineTargetYaw, 0.0f);
            }
        }


        private void InitializeCharacterMovement()
        {
            m_CharacterMovement = this.GetComponent<CharacterMovement>();
        }

        #endregion Movement,Look & Crouched

        private void InitializeInteractionCheck()
        {
            m_InteractionLayerMask = ~(1 << m_InteractionIgnoreLayer);
            m_InteractionAbleLayerMask = 1 << m_InteractionAbleLayer;
            //m_InteractionLayerMask = 1 << 7;
        }


        private void FindObjectToInteraction()
        {
            Collider[] hitColliders = Physics.OverlapSphere(m_CameraTarget.transform.position , m_RadiusFindInteraction, m_InteractionLayerMask);
            if(hitColliders == null || hitColliders.Length <= 0)
            {
                InteractionManager.Instance.CloseUIInteract();
                m_CurrentInteraction = null;
            }
            else
            {
                IInteractAble interactAble = GetInteractAbleInArray(hitColliders);
                if (interactAble != null)
                {
                    //Debug.LogError($"interactAble {interactAble}");
    
                    if (Physics.Raycast(m_CameraTarget.transform.position, m_CameraTarget.transform.forward, out RaycastHit hit, m_MaxLegnthFindInteraction, m_InteractionAbleLayerMask))
                    {
                        //Debug.LogError($"interactAble C {hit.collider}");
                        IInteractAble hitInteractAble = hit.collider.GetComponent<IInteractAble>();
                        if(hitInteractAble != null && interactAble == hitInteractAble)
                        {
                            //Debug.LogError($"Found {hit.collider}");
                            m_CurrentInteraction = hitInteractAble;
                            var bindingIndex = m_InputAction.PlayerMap.Interaction.GetBindingIndex(InputBinding.MaskByGroup(m_playerInput.currentControlScheme));
                            //Debug.LogError($"{m_InputAction.PlayerMap.Interaction.GetBindingDisplayString(bindingIndex)}");
                            InteractionManager.Instance.ShowUIInteract(hitInteractAble, m_InputAction.PlayerMap.Interaction.GetBindingDisplayString(bindingIndex));
                        }
                        else
                        {
                            InteractionManager.Instance.CloseUIInteract();
                            m_CurrentInteraction = null;
                        }
                    }
                    else
                    {
                        InteractionManager.Instance.CloseUIInteract();
                        m_CurrentInteraction = null;
                    }
                }
                else
                {
                    InteractionManager.Instance.CloseUIInteract();
                    m_CurrentInteraction = null;
                }
            }
        }

        private IInteractAble GetInteractAbleInArray(Collider[] hitColliders)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                IInteractAble inputInteraction = hitColliders[i].gameObject.GetComponent<IInteractAble>();
                if (inputInteraction != null)
                {
                    return inputInteraction;
                }
            }

            return null;
        }


        #region Perseption FieldOfView Sign
        private void InitializPerseptionFieldOfViewSign()
        {
            m_PerceptionFieldOfViewSign = this.GetComponent<PerceptionFieldOfViewSign>();
            m_PerceptionFieldOfViewSign.SignTransform = m_CameraTargetStand.transform;
        }

        private void ChangePerseptionFieldOfViewSign(Transform signTransform) 
        {
            m_PerceptionFieldOfViewSign.SignTransform = signTransform;
        }
        #endregion Perseption FieldOfView Sign

        private void OnDrawGizmos()
        {
            if (!m_CameraTarget)
            {
                return;
            }

            Vector3 testOrigin = m_CameraTarget.transform.position;
            Vector3 testDirection = m_CameraTarget.transform.forward;

            Gizmos.color = Color.red;
            Debug.DrawLine(testOrigin, testOrigin + testDirection * m_MaxLegnthFindInteraction);
            Gizmos.DrawWireSphere(testOrigin  , m_RadiusFindInteraction);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        //private static float  
    }
}
