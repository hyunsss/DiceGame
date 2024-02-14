using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sirenix.OdinInspector;


public class Player : SerializedMonoBehaviour, IUnitMethod
{
    public static Player Instance { get; private set; }
    [SerializeField] private int Damage;
    [SerializeField] private float Speed;
    [SerializeField] private float FullHp;
    [SerializeField] private float Hp;
    [SerializeField] public int Level;
    [SerializeField] private float Hp_vampireStat;
    [SerializeField] private float ArmorStat;

    private float currentExp;
    [SerializeField]
    private float MaxExp;
    public List<GameObject> MySkill = new List<GameObject>();
    private List<Ray2D> raysToDraw = new List<Ray2D>();

    public GameObject AttackPrefab;
    public SPUM_Prefabs Prefabs;
    private Rigidbody2D rigid;
    public Light2D PlayerspotLight;

    bool AnimTrue;
    bool isAttack;
    bool inItemBox;
    bool isMove;
    bool isDeath;
    bool HasActiveUI;
    public int GetDamage { get { return Damage + (int)ItemManager.Instance.GetVariance_Damage; } set { Damage = value; } }
    public float GetSpeed { get { return Speed + ItemManager.Instance.GetVariance_Speed; } set { Speed = value; } }
    public float GetHpVampire { get { return Damage * (Hp_vampireStat + +ItemManager.Instance.GetVariance_Hp_vampire / 300); } set { Hp_vampireStat = value; } }
    public float GetArmor { get { return ArmorStat + ItemManager.Instance.GetVariance_ArmorStat; } set { ArmorStat = value; } }
    public float GetHp { get { return Hp; } set { Hp = value; } }
    public float GetFullHp { get { return FullHp + ItemManager.Instance.GetVariance_FullHp; } set { FullHp = value; } }
    public float HPAmount { get { return GetHp / GetFullHp; } }
    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }
    public bool IsMove { get { return isMove; } set { isMove = value; } }
    public bool IsDeath { get { return isDeath; } set { isDeath = value; } }
    public bool InItemBox { get { return inItemBox; } set { inItemBox = value; } }
    public float ExpAmount { get { return currentExp / MaxExp; } }
    public float GetCurrentExp { get { return currentExp; } set { currentExp = value; } }
    public float GetMaxExp { get { return MaxExp; } set { MaxExp = value; } }
    public int GetLevel { get { return Level; } set { Level = value; } }
    // Start is called before the first frame update= value

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isDeath = false;
        isAttack = true;
        isMove = true;
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlayScene")
        {
            //마우스 왼쪽 버튼 공격
            if (Input.GetMouseButtonDown(0) && isAttack == true)
            {
                Attack();
                StartCoroutine(AttackDelay());
            }

            //아이템 상자 상호작용
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (inItemBox == true)
                {
                    isAttack = false;
                    isMove = false;
                    InventoryManager.Instance.OpenInventory();
                    InventoryManager.Instance.RootBoxGameobject.SetActive(true);
                    Prefabs._anim.SetFloat("RunState", 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (InventoryManager.Instance.InTriggerItemObject.Count != 0)
                {
                    float min_distance = float.MaxValue;
                    Item target = null;

                    foreach (Item itemobj in InventoryManager.Instance.InTriggerItemObject)
                    {
                        float distance = Vector2.Distance(transform.position, itemobj.gameObject.GetComponent<RectTransform>().transform.position);

                        if (min_distance > distance)
                        {
                            min_distance = distance;
                            target = itemobj;
                        }
                    }

                    target._itemimage_Component = target.transform.Find("Image").AddComponent<Image>();
                    target._itemimage_Component.sprite = target.itemSprite;
                    Slot[] targetslots = InventoryManager.Instance.InventorySlotsPanel.GetComponentsInChildren<Slot>();
                    InventoryManager.Instance.AcquireItem(target, targetslots);


                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (var UI in InventoryManager.Instance.SlotParentUI)
                {
                    if (UI.Value.activeSelf == true)
                    {
                        UI.Value.SetActive(false);
                        isAttack = true;
                        isMove = true;
                        //HasActiveUI가 true일 경우 현재 띄워져 있는 UI요소가 하나 이상이라는 것. 
                        HasActiveUI = true;
                        ItemManager.Instance.isRunning_SearchItemCoroutine = false;
                    }
                }

                //HasActiveUI가 false면 현재 ESC를 눌렀을 때 활성화된 UI 요소가 없다는 뜻이므로 설정창을 띄움.
                if (HasActiveUI == false)
                {
                    //설정 UI 띄우기 
                }

                HasActiveUI = false;
            }



            CheckAnimation(out AnimTrue);
            FindItemBox();
        }

        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.Click);
            }
        }
        if (false == AnimTrue && true == isMove) Move();
        else { rigid.velocity = Vector2.zero; }

        if (Hp <= 0 && isDeath == false)
        {
            Death();
            isDeath = true;
        }
    }


    void FindItemBox()
    {
        foreach (ItemBox this_box in TileMapManager.Instance.BoxList)
        {
            if (Vector2.Distance(this_box.transform.position, transform.position) <= 8f)
            {
                Vector2 direction = this_box.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector2.Distance(this_box.transform.position, transform.position));
                SpriteRenderer sp = this_box.GetComponent<SpriteRenderer>();
                ;

                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Wall"))
                    {
                        sp.enabled = false;
                    }
                    else
                    {
                        sp.enabled = true;
                    }

                }
                else
                {
                    sp.enabled = true;
                }


                raysToDraw.Add(new Ray2D(transform.position, direction));
            }

        }
    }

    public void Move()
    {

        float x = Input.GetAxis("Horizontal") * Speed;
        float y = Input.GetAxis("Vertical") * Speed;

        Vector2 MoveDir = new Vector2(x, y);
        if (x < 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }

        if (x != 0 || y != 0)
        {
            Prefabs._anim.SetFloat("RunState", 0.5f); // 달리는 애니메이션
        }
        else
        {
            Prefabs._anim.SetFloat("RunState", 0); // Idle 애니메이션
        }
        rigid.velocity = MoveDir;

    }

    void Attack()
    {
        Prefabs._anim.SetTrigger("Attack");

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

        AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.Attack);
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
        AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.PlayerTakeDamage);
    }

    public void Death()
    {
        isAttack = false;

        GameManager.Instance.PlayerDeathAction.Invoke();
    }


    IEnumerator AttackDelay()
    {
        isAttack = false;
        yield return new WaitForSeconds(0.5f);
        isAttack = true;
    }

}
