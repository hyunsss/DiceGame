using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class Player : SingleTon<Player>, IUnitMethod
{
    [SerializeField]
    private int Damage;
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float FullHp;
    [SerializeField]
    private float Hp;
    [SerializeField]
    public int Level;

    private float currentExp;
    [SerializeField]
    private float MaxExp;
    public List<GameObject> MySkill = new List<GameObject>();

    public GameObject AttackPrefab;
    public SPUM_Prefabs Prefabs;

    public SpriteRenderer render;
    private Rigidbody2D rigid;
    bool AnimTrue;
    bool isAttack;

    public int GetDamage { get { return Damage; } set { Damage = value; } }
    public float GetSpeed { get { return Speed; } set { Speed = value; } }
    public float GetHp { get { return Hp; } set { Hp = value; } }
    public float GetFullHp { get { return FullHp; } set { FullHp = value; } }
    public float HPAmount { get { return Hp / FullHp; } }
    public bool IsAttack { get { return isAttack;} set { isAttack = value;}}
    public float ExpAmount { get { return currentExp / MaxExp; } }
    public float GetCurrentExp { get { return currentExp; } set { currentExp = value; } }
    public float GetMaxExp { get { return MaxExp; } set { MaxExp = value; } }
    public int GetLevel { get { return Level; } set { Level = value; } }
    // Start is called before the first frame update= value

    void Start()
    {
        isAttack = true;
        transform.position = new Vector3(0, 0, 0);
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAttack == true)
        {
            Attack();
            StartCoroutine(AttackDelay());
        }
        CheckAnimation(out AnimTrue);

    }
    public void Move()
    {

        float x = Input.GetAxis("Horizontal") * Speed;
        float y = Input.GetAxis("Vertical") * Speed;

        Vector2 MoveDir = new Vector2(x, y);
        if (x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (x != 0 || y != 0)
        {
            Prefabs.PlayAnimation(1); // 달리는 애니메이션
        }
        else
        {
            Prefabs.PlayAnimation(0); // Idle 애니메이션
        }
        rigid.velocity = MoveDir;
    }
    private void FixedUpdate()
    {
        if (false == AnimTrue) Move();
        else { rigid.velocity = Vector2.zero; }
    }

    void Attack()
    {
        Prefabs.PlayAnimation(4);

        GameObject Attack = Instantiate(AttackPrefab);
        AttackMotion attackMotion = Attack.GetComponent<AttackMotion>();
        Vector2 CreateDir;
        //x > 0 왼쪽을 바라봄 x < 0 오른쪽을 바라봄
        if (transform.localScale.x < 0)
        {
            CreateDir = new Vector2(transform.position.x + 0.5f, transform.position.y);
        }
        else
        {
            CreateDir = new Vector2(transform.position.x - 0.5f, transform.position.y);
            Attack.transform.localScale = new Vector3(-1, 1, 1);
        }
        Attack.transform.Translate(CreateDir);
        attackMotion.PlayAnim();
    }

    void CheckAnimation(out bool PlayAnimation)
    {
        PlayAnimation = false;
        if (Prefabs._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState"))
        {
            PlayAnimation = true;
        }
        else
        {
            PlayAnimation = false;
        }
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
    }

    public void Death() {

    }


    IEnumerator AttackDelay() {
        isAttack = false;
        yield return new WaitForSeconds(0.5f);
        isAttack = true;
    }

}
