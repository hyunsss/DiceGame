using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour, IresetTable, IDeath
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

    public void Death() {
        _anim.SetTrigger("Death");
        StartCoroutine(DieEffect());

        
    }

    IEnumerator MonsterMoveCoroutine() {

        IsMove = false;
        yield return new WaitForSeconds(1.5f);
        IsMove = true;
        yield return new WaitForSeconds(1f);

        StartCoroutine(MonsterMoveCoroutine());
    }

    IEnumerator DieEffect()
    {
        float riseDuration = 0.15f;
        float riseSpeed = 0.15f;
        float timer = 0;

        // 초기 위치 저장
        Vector3 startPosition = transform.position;
        // 최종 목표 위치
        Vector3 endPosition = startPosition + new Vector3(0, 1f, 0); // 1미터 위로 상승

        while (timer < riseDuration)
        {
            // 시간에 따라 위치를 부드럽게 이동시킴
            transform.position = Vector3.Lerp(startPosition, endPosition, timer / riseDuration);
            timer += Time.deltaTime * riseSpeed;
            yield return null;
        }

        // 최종 위치에 도달
        transform.position = endPosition;

        // 필요한 경우, 오브젝트 파괴 또는 다른 로직 실행
        // Destroy(gameObject);
    }
}
