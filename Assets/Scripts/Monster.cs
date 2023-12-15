using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour, IresetTable
{
    public float Speed;
    public int Hp;
    public int Damage;
    public Vector3 targetPos;

    private int MaxHp = 100;
    private Player player;
    private Rigidbody2D rigid;

    private Animator _anim;
    private bool IsMove = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _=StartCoroutine(MonsterMoveCoroutine());
        _anim.Play("GhostIdle");
    }

    public void Reset() {
        IsMove = true;
        Hp = MaxHp;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        if(Hp <= 0) {
            Death();
        }
    }

    void Move() {
        if (IsMove == true) {
        Transform target = player.transform;
        Vector2 moveDir = ((Vector2)(target.position - transform.position)).normalized;

        rigid.velocity = moveDir * Speed;
        targetPos = player.transform.position;
        }
    }

    public void TakeDamage(int Damage) {
        Hp -= Damage;
        _anim.SetTrigger("Hit");

        if(Hp <= 0) {
            Death();
        }
    }

    void Death() {
        ObjectPoolManager.Instance.UnActivePool(gameObject);
    }

    IEnumerator MonsterMoveCoroutine() {

        IsMove = false;
        yield return new WaitForSeconds(1.5f);
        IsMove = true;
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(MonsterMoveCoroutine());
    }
}
