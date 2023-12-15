using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMachine : StateMachineBehaviour
{
    private GameObject gameObject;

    private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        gameObject = animator.gameObject;

        if(animatorStateInfo.IsName("GhostDeath")) {
            ObjectPoolManager.Instance.UnActivePool(gameObject);
        }
    }
}
