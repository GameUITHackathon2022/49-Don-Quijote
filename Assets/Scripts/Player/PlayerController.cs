using FIMSpace;
using FIMSpace.Basics;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using DG.Tweening;
using Unity.VisualScripting;

namespace LittleFoxLite
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour,  iDamagetable
    {
        public delegate void VoidEvent();
        public VoidEvent jumpAction;
        [Header("Movement Setting")]
        [SerializeField] float MoveSpeed = 3f;
        [SerializeField] float SprintSpeedM = 1.8f;
        [SerializeField] float changeSpeedSmooth = 12f;
        [SerializeField] float JumpHeight;
        [SerializeField] float JumpCoolTime = 0.5f;
        [SerializeField] float FreeFallActiveTime = 0.15f;
        [SerializeField] float RotationSmooth = 0.12f;
        const float gravity = -14f;
        float deltaFreeFall;
        float deltaJump;
        float verticalVelocity = 0;
        CharacterController playerController;
        public InputManager _input;
        PlayerInput _playerInput;
        public Animator playerAnimator;
        GameObject _maincamera;
        public static PlayerController Instance;
        public HealthSystem<PlayerController> healthSystem;
        BullletPooling bullpool;
        [SerializeField] WeaponBase gunWeapon;
        public CurrentWeapon currentWeapon;
        [SerializeField] PlayerSound playerSound;
        /// <summary>
        /// Animation id
        /// </summary>
        int _aniIntSpeed;
        int _aniBoolGround;
        int _aniBoolFreeFall;
        int aniBoolJump;
        public int aniStateNormal;
        public int aniStateShoot;
        public int aniStateEvent;
        int aniFloatXdir;
        int aniFloatYdir;
        int aniBoolReload;
        public UIManager playerUI;

        void AssignAnimattionID()
        {
             aniBoolJump = Animator.StringToHash("Jump");
            _aniIntSpeed = Animator.StringToHash("Speed");
            _aniBoolGround = Animator.StringToHash("Grounded");
            _aniBoolFreeFall = Animator.StringToHash("FreeFall");
            aniStateShoot = Animator.StringToHash("IsShoot");
            aniStateNormal = Animator.StringToHash("IsNormal");
            aniStateEvent = Animator.StringToHash("IsEvent");
            aniFloatXdir = Animator.StringToHash("XDirection");
            aniFloatYdir = Animator.StringToHash("YDirection");
            aniBoolReload = Animator.StringToHash("Reloading");

        }
        [Header("State Manager")]
        [SerializeField] PlayerStateType startState;
        public StateManager stateManager;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            AssignAnimattionID();
            if (_maincamera == null)
                _maincamera = GameObject.FindGameObjectWithTag("MainCamera");
            stateManager = new StateManager(this, startState);
            playerAnimator = GetComponent<Animator>();
            _input = GetComponent<InputManager>();
            playerController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            bullpool = GetComponent<BullletPooling>();
            currentWeapon = GetComponent<CurrentWeapon>();
            currentWeapon.weapon = gunWeapon;
            playerUI = GetComponent<UIManager>();
        }
        public void AudioFootSepSound()
        {
            float curentVelocity = Mathf.Abs(verticalVelocity);
            float scale = curentVelocity / Mathf.Abs(gravity);
            scale = scale>1?1:scale;
            playerSound.FootStep(1+scale);
        }
        public void AudioReloadSound()
        {
            playerSound.ReloadSound();
        }

        [Header("PlayerStatus")]
        [SerializeField] int maxHealth;
        // Start is called before the first frame update
        void Start()
        {
            gunObject.SetActive(false);
            healthSystem = new HealthSystem<PlayerController>(this, maxHealth);
            healthSystem.takeDamageAction += DamageAction;
            healthSystem.dieAction += DieAction;
            healthSystem.changeUIHealth += OnUIHealthChange;
        }
        public void TakeDamage(int damge)
        {
            healthSystem.TakeDamage(damge);
        }
        void DamageAction()
        {

        }
        void  DieAction()
        {

        }
        public void ReloadAction(float value)
        {
            if (value == 1)
            {
                playerAnimator.SetLayerWeight(1, value);
                playerAnimator.SetBool(aniBoolReload, true);
            } else
            {
                playerAnimator.SetLayerWeight(1, value);
                playerAnimator.SetBool(aniBoolReload, false);
            }
        }
        void OnUIHealthChange(float value)
        {

        }
        public void Shoot()
        {
            if (_input.Fire1 && currentWeapon.ReadyToShoot)
            {
                bullpool.ShootAction();
                currentWeapon.ReadyToShoot = false;
                currentWeapon.ShootBulletCount();
                Invoke(nameof(ResetShootTime), gunWeapon.FireRate);
                playerSound.ShootSound();
            }
        }
        void ResetShootTime()
        {
            if (currentWeapon.onReload) return;
            currentWeapon.ReadyToShoot = true;
        }
        public void ChangeAnimatorState(int State, bool value)
        {
            playerAnimator.SetBool(State, value);
        }
        // Update is called once per frame
        void Update()
        {
            stateManager.Update();
        }
        private void LateUpdate()
        {
            //CameraRotation();
        }
        [Header("Camera Setup")]
        [SerializeField] float TopClamp = 70.0f;
        [SerializeField] float BottomClamp = -30.0f;
        [SerializeField] float CameraAngleOverride = 0.0f;
        [SerializeField] bool LockCameraPosition = true;
        [SerializeField] GameObject CinemachineCameraTarget;
        float _threshold = 0.01f;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                #if ENABLE_INPUT_SYSTEM
                                return _playerInput.currentControlScheme == "KeyboardMouse";
                #else
                                return false;
                #endif
            }
        }
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }
            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = MathL.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = MathL.ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Don't use in this project
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,_cinemachineTargetYaw, 0.0f);
        }
        [Header("Ground check Setting")]
        [SerializeField] Vector3 CheckOffset;
        [SerializeField] float GroundRadius;
        [SerializeField] LayerMask GroundLayer;
        bool isGrounded;
        void GroundCheck()
        {
            Vector3 position = transform.position + CheckOffset;
            isGrounded = Physics.CheckSphere(position, GroundRadius, GroundLayer);
            playerAnimator.SetBool(_aniBoolGround, isGrounded);
        }
        void CalculateVerticalVelocity()
        {
            if (isGrounded)
            {
                deltaFreeFall = FreeFallActiveTime;
                if (deltaJump > 0) deltaJump -= Time.deltaTime;
                playerAnimator.SetBool(_aniBoolFreeFall, false);
                if (!isJumping)
                {
                    playerAnimator.SetBool(aniBoolJump, false);
                }
                verticalVelocity = verticalVelocity < 0 ? -2 : verticalVelocity;
            } else
            {
                deltaJump = JumpCoolTime;
                if (deltaFreeFall >= 0) deltaFreeFall -= Time.deltaTime;
                if (deltaFreeFall < 0)
                {
                    playerAnimator.SetBool(_aniBoolFreeFall, true); 
                }
                verticalVelocity += gravity * Time.deltaTime;
            }
        }
        bool isJumping = false;
        [SerializeField] float ResetTimeDelta = 0.25f;
        public void HandleJump()
        {
            if (isGrounded && _input.jump && deltaJump <= 0 && !isJumping)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * gravity);
                playerAnimator.SetBool(aniBoolJump, true);
                isJumping = true;
                Invoke(nameof(ResetJumpmState), ResetTimeDelta);
            }
        }
        void ResetJumpmState()
        {
            isJumping = false;
        }
        [HideInInspector] public float CurrentSpeed = 0;
        [SerializeField]
        [Range(0, 1)] float MoveDirectionBlend;
        float targetAngle;
        [HideInInspector] public float RotationVelocity = 0.01f;
        [SerializeField] LeaningAnimator leaning;
        //NormalState
        public void HandleMovement()
        {
            GroundCheck();
            CalculateVerticalVelocity();
            float TargetSpeed = _input.sprint ? SprintSpeedM : MoveSpeed;
            TargetSpeed = _input.move == Vector2.zero ? 0 : TargetSpeed;
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, changeSpeedSmooth*Time.deltaTime);
            playerAnimator.SetFloat(_aniIntSpeed, CurrentSpeed / SprintSpeedM);
            if (_input.move != Vector2.zero) 
                HandleRotation();
            Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0)*Vector3.forward;
            Vector3 direcmovement = transform.forward;
            targetDirection = Vector3.Lerp(targetDirection, direcmovement, MoveDirectionBlend);
            playerController.Move(targetDirection.normalized * CurrentSpeed * Time.deltaTime);
            playerController.Move(Vector3.up * verticalVelocity * Time.deltaTime);

            leaning.User_DeliverIsAccelerating(_input.move != Vector2.zero);
            leaning.User_DeliverIsGrounded(isGrounded);
            leaning.User_DeliverAccelerationSpeed(CurrentSpeed);
        }
        public void HandleRotation()
        {
            Vector2 inputDirection = _input.move.normalized;
            targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _maincamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationVelocity, RotationSmooth);
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        //Shoot state
        [SerializeField] float JumpPowerShoot;
        public void HandleJumpInShoot()
        {
            if (isGrounded && _input.jump && deltaJump <= 0 && !isJumping)
            {
                Vector3 direcMovement = new Vector3(_input.move.x, 0, _input.move.y);
                direcMovement = Quaternion.Euler(0, transform.eulerAngles.y, 0) * direcMovement;
      
                transform.DOMove(transform.position + direcMovement.normalized * JumpPowerShoot, ResetTimeDelta).SetEase(Ease.OutSine);
                playerAnimator.SetBool(aniBoolJump, true);
                isJumping = true;
                Invoke(nameof(ResetJumpmState), ResetTimeDelta);
            }
        }
        public void HandleMovementShoot()
        {
            GroundCheck();
            CalculateVerticalVelocity();
            float TargetSpeed = MoveSpeed;
            TargetSpeed = _input.move == Vector2.zero ? 0 : TargetSpeed;
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, changeSpeedSmooth * Time.deltaTime);
            if (_input.move != Vector2.zero)
                HandleRotationShoot();
            if (isJumping) return;
            float RotationMove = MathF.Atan2(_input.move.x, _input.move.y) * Mathf.Rad2Deg + _maincamera.transform.eulerAngles.y;
            Vector3 direcMovement = Vector3.forward; 
            direcMovement = Quaternion.Euler(0, RotationMove, 0)*direcMovement;
            direcMovement.Normalize();
            Vector3 aniDir= new Vector3(_input.move.x, 0, _input.move.y);
            aniDir = Quaternion.Euler(0, targetAngle, 0) * aniDir;
           

            playerController.Move(direcMovement * CurrentSpeed * Time.deltaTime);
            playerController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
            //SetAnimator
            
            playerAnimator.SetFloat(_aniIntSpeed, CurrentSpeed / SprintSpeedM);
            playerAnimator.SetFloat(aniFloatYdir, aniDir.x*(CurrentSpeed/MoveSpeed));
            playerAnimator.SetFloat(aniFloatXdir, aniDir.z*(CurrentSpeed/MoveSpeed));
            //
            leaning.User_DeliverIsAccelerating(_input.move != Vector2.zero);
            leaning.User_DeliverIsGrounded(isGrounded);
            leaning.User_DeliverAccelerationSpeed(CurrentSpeed);
        }
        public Camera mainCamera;
        public float CamareRotationDutch;
        void HandleRotationShoot()
        {
            Vector2 inputDirection = _input.move.normalized;
            Ray screenRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!IsCurrentDeviceMouse)
            {
                Vector3 pos = _input.look;
                pos.Normalize();
                targetAngle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg + CamareRotationDutch;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationVelocity, RotationSmooth);
                transform.rotation = Quaternion.Euler(0, rotation, 0);
            } else
            if (Physics.Raycast(screenRay, out hit, Mathf.Infinity, GroundLayer))
            {
                Vector3 pos = hit.point;
                pos = pos - transform.position;
                pos.Normalize();
                targetAngle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg + CamareRotationDutch;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationVelocity, RotationSmooth);
                transform.rotation = Quaternion.Euler(0, rotation, 0);
            }

        }
        [SerializeField] GameObject gunObject;
        [SerializeField] float timeToHideGun = 0.2f; 
        public void ShowGun()
        {
            gunObject.SetActive(true);
        }
        public IEnumerator HideGun()
        {
            yield return new WaitForSeconds(timeToHideGun);
            gunObject.SetActive(false);
        }
        public void ChangeBetweenToState()
        {
            if (stateManager.CurrentType == PlayerStateType.Shoot)
            {
                stateManager.SwitchState(PlayerStateType.Normal);
            }
            else if (stateManager.CurrentType == PlayerStateType.Normal)
            {
                    stateManager.SwitchState(PlayerStateType.Shoot);

            }
        }
    }

}
