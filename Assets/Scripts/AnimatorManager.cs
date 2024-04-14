using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimatorValues(float verticalInput, bool isHoldingRifle, bool isHoldingSword)
    {
        animator.SetFloat("Vertical", verticalInput, 0.1f, Time.deltaTime);
        animator.SetBool("IsHoldingRifle", isHoldingRifle);
        animator.SetBool("IsHoldingSword", isHoldingSword);
    }
    public void PlayOverrideAnimation(string animation)
    {
        animator.CrossFade(animation, 0.2f);
    }
}
