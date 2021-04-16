using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopHandgun : StateMachineBehaviour
{
    private float timePassed = 0;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.deltaTime;
        if (timePassed < stateInfo.length) return;
        animator.SetBool("Idle",true);
    }
}
