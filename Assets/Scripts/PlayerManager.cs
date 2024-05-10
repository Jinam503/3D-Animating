using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputReader input;
    #region PRIVATE_VARIABLES

    private Animator _animator;
    private PlayerWeaponSlotManager _playerWeaponSlotManager;
    
    #endregion
    
    #region PUBLIC_VARIABLES
    
    [Header("MOVEMENT")]
    public float moveInputValue;
    private Vector2 _movementInput;


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
    private void Start()
    {
        input.MoveEvent += SetMovementValue;
        input.JumpEvent += Jump;
        input.StartRunEvent += StartRun;
        input.StopRunEvent += StopRun;

        input.StartAimEvent += StartAiming;
        input.StopAimEvent += StopAiming;
        input.StartFireEvent += StartFire;
        input.StopFireEvent += StopFire;
        
        input.ReloadEvent += Reload;
        input.ThrowGrenadeEvent += ThrowGrenade;

        input.MainWeaponEvent += ChangeMainWeapon;
    }
    private void Update()
    {
        Move();
        HandleFlags();
        HandleRigs();
    }

    private void HandleFlags()
    {
        isHandlingWeapon = _animator.GetBool(IsHandlingWeapon);
        isJumping = _animator.GetBool(IsJumping);
    }
    
    private void Move()
    {
        moveInputValue = Mathf.Max(_movementInput.y, 0);
        
        if (isSprinting && !isHandlingWeapon)
        {
            moveInputValue = 2;
        }
        
        _animator.SetFloat(Vertical, moveInputValue, 0.1f, Time.deltaTime);
    }
    private void SetMovementValue(Vector2 movementInput)
    {
        _movementInput = movementInput;
    }

    private void StartRun()
    {
        isSprinting = true;
    }
    private void StopRun()
    {
        isSprinting = false;
    }
    private void Jump()
    {
        if (isJumping || isHandlingWeapon) return;

        switch (currentWeapon)
        {
            case CurrentWeapon.None:
                _animator.CrossFade(moveInputValue > 0.5f ? "Move Jump" : "Idle Jump", 0.1f);
                break;
            case CurrentWeapon.Rifle:
                _animator.CrossFade(moveInputValue > 0.5f ? "Rifle Move Jump" : "Rifle Idle Jump", 0.1f);
                break;
            case CurrentWeapon.Sword:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void ChangeMainWeapon()
    {
        if (isJumping) return;

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

        isHoldingRifle = currentWeapon == CurrentWeapon.Rifle;

        _animator.SetBool(IsHoldingRifle, isHoldingRifle); 
    }
    private void ThrowGrenade()
    {
        if (currentWeapon != CurrentWeapon.None  || isJumping || isHandlingWeapon) return;

        _animator.CrossFade("Throw Grenade", 0.1f);
    }
    private void Reload()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon) return;

        _animator.CrossFade("Reload", 0.1f);
    }
    
    private void StartAiming()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon) return;
        
        isAiming = true;
        _animator.SetBool(IsAiming, isAiming);
    }
    private void StopAiming()
    {
        isAiming = false;
        _animator.SetBool(IsAiming, isAiming);
    }
    private void StartFire()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon || !isAiming) return;
        
        isFiring = true;
        _animator.CrossFade("Fire", 0.1f);
        
        _animator.SetBool(IsFiring, isFiring);
    }
    private void StopFire()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon || !isAiming) return;

        isFiring = false;
        
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