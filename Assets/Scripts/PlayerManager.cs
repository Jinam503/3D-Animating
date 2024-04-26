using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    #region PRIVATE_VARIABLES

    private Animator _animator;
    private PlayerWeaponSlotManager _playerWeaponSlotManager;

    private bool _inputAlpha1;
    private bool _inputF;
    private bool _inputR;
    private bool _inputSpace;
    
    private bool _inputLeftMouseDown;
    private bool _inputLeftMouseUp;
    private bool _inputLeftMouseStay;
    
    #endregion
    
    #region PUBLIC_VARIABLES
    
    [Header("MOVEMENT")]
    public float moveInput;

    

    [Header("FLAGS")]
    public bool isSprinting;
    public bool isHoldingRifle;
    public bool isHoldingSword;
    public bool isJumping;
    public bool isHandlingWeapon;
    public bool isAiming;
    public bool isFiring;

    [Header("WEAPON")]
    public CurrentWeapon currentWeapon;
    public WeaponItem rifleAk47;
    public WeaponItem rifleM4A1;

    [Header("RIG")]
    public Rig leftHandAimingRig;
    public Rig leftHandMovingRig;
    
    #endregion

    #region READONLY_VARIABLES

    private static readonly int IsHandlingWeapon = Animator.StringToHash("IsHandlingWeapon");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsHoldingRifle = Animator.StringToHash("IsHoldingRifle");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");
    private static readonly int IsFiring = Animator.StringToHash("IsFiring");

    #endregion
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    private void Update()
    {
        HandleAllPlayerActions();
    }

    private void HandleAllPlayerActions()
    {
        HandleAllInputsAndFlags();

        HandleMovement();
        HandleJumpInput();

        HandleChangeWeapon();
        HandleThrowGrenade();

        HandleAiming();
        HandleReloading();
        HandleFiring();

        HandleRigs();
    }
    private void HandleAllInputsAndFlags()
    {
        moveInput = Mathf.Max(Input.GetAxis("Vertical"), 0);
        isSprinting = Input.GetKey(KeyCode.LeftShift) && moveInput > 0.5f;


        isHandlingWeapon = _animator.GetBool(IsHandlingWeapon);
        isJumping = _animator.GetBool(IsJumping);

        _inputAlpha1 = Input.GetKeyDown(KeyCode.Alpha1);
        
        _inputF = Input.GetKeyDown(KeyCode.R);
        _inputR = Input.GetKeyDown(KeyCode.R);
        _inputSpace = Input.GetKeyDown(KeyCode.Space);

        _inputLeftMouseUp = Input.GetMouseButtonUp(1);
        _inputLeftMouseDown = Input.GetMouseButtonDown(1);
        _inputLeftMouseStay = Input.GetMouseButton(1);
    }
    private void HandleMovement()
    {
        if (isSprinting && !isHandlingWeapon)
        {
            moveInput = 2;
        }

        _animator.SetFloat(Vertical, moveInput, 0.1f, Time.deltaTime);
    }
    private void HandleChangeWeapon()
    {
        if (isJumping) return;

        if (_inputAlpha1)
        {
            if (currentWeapon == CurrentWeapon.Rifle)
            {
                currentWeapon = CurrentWeapon.None;
                _animator.CrossFade("Rifle Put Away", 0.1f);
            }
            else
            {
                currentWeapon = CurrentWeapon.Rifle;
                _animator.CrossFade("Rifle Pull Out", 0.1f);
                _playerWeaponSlotManager.LoadWeaponOnSlot(rifleM4A1);
            }

        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    if (currentWeapon == CurrentWeapon.Sword)
        //    {
        //        currentWeapon = CurrentWeapon.None;
        //    }
        //    else
        //    {
        //        currentWeapon = CurrentWeapon.Sword;
        //    }
        //}

        isHoldingRifle = currentWeapon == CurrentWeapon.Rifle;
        //isHoldingSword = currentWeapon == CurrentWeapon.Sword;

        _animator.SetBool(IsHoldingRifle, isHoldingRifle); 
        //animator.SetBool("IsHoldingSword", isHoldingSword);
    }
    private void HandleJumpInput()
    {
        if (!_inputSpace || isJumping || isHandlingWeapon) return;
        
        switch (currentWeapon)
        {
            case CurrentWeapon.None:
                _animator.CrossFade(moveInput > 0.5f ? "Move Jump" : "Idle Jump", 0.1f);
                break;
            case CurrentWeapon.Rifle:
                _animator.CrossFade(moveInput > 0.5f ? "Rifle Move Jump" : "Rifle Idle Jump", 0.1f);
                break;
            case CurrentWeapon.Sword:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void HandleThrowGrenade()
    {
        if (currentWeapon != CurrentWeapon.None  || isJumping || isHandlingWeapon) return;

        if (_inputR)
        {
            _animator.CrossFade("Throw Grenade", 0.1f);
        }
    }
    private void HandleReloading()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon) return;

        if (_inputF)
        {
            _animator.CrossFade("Reload", 0.1f);
        }
    }
    private void HandleAiming()
    {
        if (currentWeapon == CurrentWeapon.Rifle && !isJumping && !isHandlingWeapon)
        {
            isAiming = _inputLeftMouseDown;
        }
        else
        {
            isAiming = false;
        }

        _animator.SetBool(IsAiming, isAiming);
    }
    private void HandleFiring()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon || !isAiming) return;

        if (_inputLeftMouseDown) 
        { 
            isFiring = true;

            _animator.CrossFade("Fire", 0.1f);
        }
        if (_inputLeftMouseUp)
        {
            isFiring = false;
        }

        _animator.SetBool(IsFiring, isFiring);
    }
    private void HandleRigs()
    {
        if (!isHandlingWeapon && isHoldingRifle)
        {
            leftHandAimingRig.weight = Mathf.Lerp(leftHandAimingRig.weight, isAiming || isJumping ? 1 : 0, Time.deltaTime * 10);
        }
        else
        {
            leftHandAimingRig.weight = Mathf.Lerp(leftHandAimingRig.weight, 0, Time.deltaTime * 10);
        }
    }
}

public enum CurrentWeapon
{
    None,
    Rifle,
    Sword,
}