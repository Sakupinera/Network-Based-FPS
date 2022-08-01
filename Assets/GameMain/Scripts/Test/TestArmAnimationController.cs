using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArmAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public static Animator ArmAnimator
    {
        get;
        private set;
    }

    private void Awake()
    {
        ArmAnimator = animator;
    }
}
