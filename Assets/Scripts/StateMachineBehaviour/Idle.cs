using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
    }
}
