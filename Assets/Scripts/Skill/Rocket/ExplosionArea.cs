using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    public Rocket ParentRocket;
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        ParentRocket = GetComponentInParent<Rocket>();
        Damage = ParentRocket.AreaDamage;

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Monster>(out Monster monster)) {
            monster.TakeDamage(Damage);
        }
    }
}
