using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerAnimationContorller : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public static Animator PlayerAnimator
    {
        get;
        private set;
    }

    private void Awake()
    {
        PlayerAnimator = animator;
    }
}
