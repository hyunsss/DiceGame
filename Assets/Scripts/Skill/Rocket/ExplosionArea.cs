using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
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

    private void OnEnable() {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Monster monster) && monster.gameObject.activeSelf == true)
        {
            monster.MonsterDeath += StopDamageCoroutine;
            Coroutine damagecoroutine = StartCoroutine(OnDamageCoroutine(monster.gameObject));
            activeCoroutines[monster.gameObject] = damagecoroutine;
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Monster monster) && activeCoroutines.ContainsKey(other.gameObject))
        {
            if(activeCoroutines != null ) StopCoroutine(activeCoroutines[other.gameObject]);
            activeCoroutines.Remove(other.gameObject);
        }
    }

    private void StopDamageCoroutine(GameObject monster)
    {
        if (activeCoroutines.ContainsKey(monster))
        {
            StopCoroutine(activeCoroutines[monster]);
            activeCoroutines.Remove(monster);
            monster.GetComponent<Monster>().MonsterDeath -= StopDamageCoroutine;
        }
    }

    IEnumerator OnDamageCoroutine(GameObject monster)
    {
        while (monster != null)
        {
            monster.GetComponent<Monster>().TakeDamage(Damage);

            yield return new WaitForSeconds(1.5f);
        }
    }
}
