using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAnimationController : MonoBehaviour
{
    [SerializeField]
    private RuntimeAnimatorController animatorController;

    public RuntimeAnimatorController AnimatorController
    {
        get { return animatorController; }
        set { animatorController = value; }
    }
}
