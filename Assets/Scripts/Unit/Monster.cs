using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IUnitMethod, IresetTable
{
    public List<Item> DropItemList = new List<Item>();
    public int DropPer;

    public float Damage;
    public float Speed;
    private float FullHp = 100;
    public float Hp;
    public float ExpValue;
    private bool IsMove = true;

    public Vector3 targetPos;

    public Image HPBarImage;
    public GameObject HPBar;
    
    private Player player;
    private Rigidbody2D rigid;

    private Animator _anim;

    public event Action DeathAction;
    // Start is called before the first frame update
    void Start()
    {
        DeathAction += Death;
        player = FindObjectOfType<Player>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _=StartCoroutine(MonsterMoveCoroutine());
        _anim.Play("GhostIdle");
    }

    public void Reset() {
        GetComponent<BoxCollider2D>().enabled = true;
        IsMove = true;
        Hp = FullHp;
        HPAmount();
        DeathAction += Death;
        HPBar.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Move() {
        if (IsMove == true) {
        Transform target = player.transform;
        Vector2 moveDir = ((Vector2)(target.position - transform.position)).normalized;

        rigid.velocity = moveDir * Speed;
        targetPos = player.transform.position;
        }
    }

    public void TakeDamage(float Damage) {
        Hp -= Damage;
        HPAmount();
        _anim.SetTrigger("Hit");

        if(Hp <= 0 && DeathAction != null) {
            DeathAction.Invoke();
        }
    }

    void HPAmount() {
        HPBarImage.fillAmount = Hp / FullHp;
    }

    public void Death() {
        GetComponent<BoxCollider2D>().enabled = false;
        _anim.SetTrigger("Death");
        StartCoroutine(DieEffect());
        Player.Instance.GetCurrentExp += ExpValue;
        DropItem();

        HPBar.SetActive(false);

        DeathAction -= Death;
    }

    void DropItem() {
        if(ItemManager.Instance.RandomSystem(DropPer)) {
            GameObject DropItem = Instantiate(DropItemList[0].gameObject, transform.position, Quaternion.identity);
            DropItem.transform.SetParent(ItemManager.Instance.ItemListObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.TryGetComponent(out Player player)) {
            player.TakeDamage(Damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Wall") {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Wall") {
            GetComponent<Collider2D>().isTrigger = false;
        }
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
