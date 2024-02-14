using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knifes : MonoBehaviour{

    private Rigidbody2D rigid;

    private int Damage;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        GetSkillElement();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Monster>(out Monster monster) && monster.gameObject.activeSelf == true) {
            monster.TakeDamage(Damage);
            AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.TakeDamage);

        }
    }

    private void GetSkillElement() {
        GameObject ParentObject = transform.parent.gameObject;
        if(ParentObject.TryGetComponent(out RotateKnife rotateKnife)) {
            Damage = rotateKnife.Damage;
        } else if(ParentObject.TryGetComponent(out Shuriken shuriken)) {
            Damage = shuriken.Damage;
        }

    }

    public void ShurikenShot(float Speed) {
        rigid.velocity = transform.up * Speed;

    }

}
