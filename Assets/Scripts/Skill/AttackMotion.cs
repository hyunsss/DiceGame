using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    Animator _anim;
    float Hp_vampireStat;

    private void Start() {
        _anim = GetComponent<Animator>();
        Hp_vampireStat = Player.Instance.GetHpVampire;
    }

    public void PlayAnim() {
        Destroy(gameObject, 0.3f);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent(out Monster monster) && monster.gameObject.activeSelf == true) {
            monster.TakeDamage(Player.Instance.GetDamage);
            Debug.Log("attack!");

            Player.Instance.GetHp += Player.Instance.GetDamage;
        }
    }
}
