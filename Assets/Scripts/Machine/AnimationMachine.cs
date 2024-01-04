using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationMachine : StateMachineBehaviour
{
    private GameObject gameObject;

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        gameObject = animator.gameObject;

        if(animatorStateInfo.IsName("GhostDeath")) {
            ObjectPoolManager.Instance.UnActivePool(gameObject);
        }
    }
}
