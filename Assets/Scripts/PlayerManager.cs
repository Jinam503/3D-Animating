using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    private Animator animator;
    private PlayerWeaponSlotManager playerWeaponSlotManager;

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
    public WeaponItem rifle_AK47;
    public WeaponItem rifle_M4A1;

    [Header("RIG")]
    public Rig leftHandAimingRig;
    public Rig leftHandMovingRig;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    private void Update()
    {
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleChangeWeapon();
        HandleJumpInput();
        HandleThrowGrenade();
        HandleAiming();
        HandleReloading();
        HandleFireing();
        HandleRigs();
    }

    private void HandleMovementInput()
    {
        moveInput = Mathf.Max(Input.GetAxis("Vertical"), 0);

        isSprinting = Input.GetKey(KeyCode.LeftShift) && moveInput > 0.5f;

        if (isSprinting && !isHandlingWeapon)
        {
            moveInput = 2;
        }

        animator.SetFloat("Vertical", moveInput, 0.1f, Time.deltaTime);
    }
    private void HandleChangeWeapon()
    {
        isHandlingWeapon = animator.GetBool("IsHandlingWeapon");

        if (isJumping) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeapon == CurrentWeapon.Rifle)
            {
                currentWeapon = CurrentWeapon.None;
                animator.CrossFade("Rifle Put Away", 0.1f);
            }
            else
            {
                currentWeapon = CurrentWeapon.Rifle;
                animator.CrossFade("Rifle Pull Out", 0.1f);
                playerWeaponSlotManager.LoadWeaponOnSlot(rifle_M4A1);
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

        animator.SetBool("IsHoldingRifle", isHoldingRifle); 
        //animator.SetBool("IsHoldingSword", isHoldingSword);
    }
    private void HandleJumpInput()
    {
        isJumping = animator.GetBool("IsJumping");

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isHandlingWeapon)
        {
            switch (currentWeapon)
            {
                case CurrentWeapon.None:
                    if (moveInput > 0.5f)
                    {
                        animator.CrossFade("Move Jump", 0.1f);
                    }
                    else
                    {
                        animator.CrossFade("Idle Jump", 0.1f);
                    }
                    break;
                case CurrentWeapon.Rifle:
                    if (moveInput > 0.5f)
                    {
                        animator.CrossFade("Rifle Move Jump", 0.1f);
                    }
                    else
                    {
                        animator.CrossFade("Rifle Idle Jump", 0.1f);
                    }
                    break;
                case CurrentWeapon.Sword:
                    break;
            }
        }
    }
    private void HandleThrowGrenade()
    {
        if (currentWeapon != CurrentWeapon.None  || isJumping || isHandlingWeapon) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.CrossFade("Throw Grenade", 0.1f);
        }
    }
    private void HandleReloading()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.CrossFade("Reload", 0.1f);
        }
    }
    private void HandleAiming()
    {
        if (currentWeapon == CurrentWeapon.Rifle && !isJumping && !isHandlingWeapon)
        {
            isAiming = Input.GetMouseButton(1);
        }
        else
        {
            isAiming = false;
        }

        animator.SetBool("IsAiming", isAiming);
    }
    private void HandleFireing()
    {
        if (currentWeapon != CurrentWeapon.Rifle || isJumping || isHandlingWeapon || !isAiming) return;

        if (Input.GetMouseButtonDown(0)) 
        { 
            isFiring = true;

            animator.CrossFade("Fire", 0.1f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }

        animator.SetBool("IsFiring", isFiring);
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