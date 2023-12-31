using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    Animator _anim;

    private void Start() {
        _anim = GetComponent<Animator>();
    }

    public void PlayAnim() {
        Destroy(gameObject, 0.5f);
        _anim.Play("Attack");
    }

    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent(out Monster monster)) {
            monster.TakeDamage(Player.Instance.GetDamage);
            Debug.Log("attack!");
        }
    }
}
