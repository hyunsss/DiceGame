using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Rocket ParentRocket;
    public int Damage;

    private Rigidbody2D rigid;
    // Start is called before the first frame update

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        ParentRocket = GetComponentInParent<Rocket>();
        Damage = ParentRocket.ExplosionDamage;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Monster>(out Monster monster)) {
            monster.TakeDamage(Damage);
        GameObject ExplosionAreaObject = Instantiate(ParentRocket.ExplosionArea, this.transform.position, Quaternion.identity);
        ExplosionAreaObject.transform.SetParent(ParentRocket.transform);
        Destroy(gameObject, 0.01f);
        }

        
    }

    public void Shot(float Speed) {
        rigid.velocity = Vector2.up * Speed;
    }
}
