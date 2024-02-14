using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
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
    public bool IsDeath;

    Coroutine DamageCoroutine;
    bool PlayerDamage;

    public Vector3 targetPos1;

    public Image HPBarImage;
    public GameObject HPBar;

    private Player player;
    private Rigidbody2D rigid;

    private Animator _anim;

    public event Action DeathAction;
    public Action<GameObject> MonsterDeath;
    public bool UnActivetrue;

    #region A*
    public Vector2Int bottomLeft, topRight, startPos, targetPos, TemptargetPos;
    Vector3 cellCenterOffset = new Vector3(0.5f, 0.5f, 0f);
    Coroutine pathCoroutine;
    public float A_StarDelay;
    public Vector2 TempPlayerPos;

    Node StartNode, TargetNode, CurNode;
    public List<Node> FinalNodeList = new List<Node>();
    public List<Node> OpenList = new List<Node>(), ClosedList = new List<Node>();

    public void PathFinding()
    {
        Vector3Int StartPos = TileMapManager.Instance.WallMap.WorldToCell(transform.position) + GameManager.Instance.originCellPosition;
        startPos = new Vector2Int(StartPos.x, StartPos.y);
        Vector3Int PlayerCellPos = TileMapManager.Instance.WallMap.WorldToCell(Player.Instance.transform.position) + GameManager.Instance.originCellPosition;
        targetPos = new Vector2Int(PlayerCellPos.x, PlayerCellPos.y);

        StartNode = GameManager.Instance.NodeArray[startPos.x, startPos.y];
        TargetNode = GameManager.Instance.NodeArray[targetPos.x, targetPos.y];

        TempPlayerPos = GameManager.Instance.GetPlayerPos;

        OpenList.Clear();
        OpenList.Add(StartNode);
        ClosedList.Clear();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            if (CurNode == TargetNode)
            {
                FinalNodeList.Clear();
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                if (pathCoroutine != null)
                {
                    StopCoroutine(pathCoroutine);
                    rigid.velocity = Vector2.zero;
                }

                // 새 경로로 코루틴을 시작합니다.
                pathCoroutine = StartCoroutine(FollowPath());

                A_StarDelay = Time.time;
                return;
            }

            if (GameManager.Instance.allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }

    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (checkX >= 0 && checkX < GameManager.Instance.sizeX && checkY >= 0 && checkY < GameManager.Instance.sizeY
                                                            && !GameManager.Instance.NodeArray[checkX, checkY].isWall
                                                            && !ClosedList.Contains(GameManager.Instance.NodeArray[checkX, checkY]))
        {
            if (GameManager.Instance.allowDiagonal) if (GameManager.Instance.NodeArray[CurNode.x, checkY].isWall && GameManager.Instance.NodeArray[checkX, CurNode.y].isWall) return;

            if (GameManager.Instance.dontCrossCorner) if (GameManager.Instance.NodeArray[CurNode.x, checkY].isWall || GameManager.Instance.NodeArray[checkX, CurNode.y].isWall) return;

            Node NeighnorNode = GameManager.Instance.NodeArray[checkX, checkY];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);
            if (MoveCost < NeighnorNode.G || !OpenList.Contains(NeighnorNode))
            {
                NeighnorNode.G = MoveCost;
                NeighnorNode.H = (Mathf.Abs(NeighnorNode.x - TargetNode.x) + Mathf.Abs(NeighnorNode.y - TargetNode.y)) * 10;
                NeighnorNode.ParentNode = CurNode;

                OpenList.Add(NeighnorNode);
            }
        }
    }

    #endregion
    private void Awake() {
        player = GameObject.Find("Player").GetComponent<Player>();
        PlayerDamage = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _anim.Play("GhostIdle");
        UnActivetrue = false;
        TempPlayerPos = Vector2.zero;
    }

    private void FixedUpdate() {
        if(Vector2.Distance(player.gameObject.transform.position, transform.position) >= 15f) {
            ObjectPoolManager.Instance.UnActivePool(gameObject);
        }

        if(TimeManager.Instance.IsDayTime()) {
            if(Vector2.Distance(player.gameObject.transform.position, transform.position) <= 4f) {
                if(IsDeath == false) Move();
            } else {
                rigid.velocity = Vector2.zero;
            }
        }

        if(TimeManager.Instance.IsNightTime()) {
                if(IsDeath == false) Move();
        }

    }

    public void Reset()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        DamageCoroutine = null;
        MonsterDeath = null;
        IsDeath = false;
        IsMove = true;
        Hp = FullHp;
        HPAmount();
        DeathAction += Death;
        HPBar.SetActive(true);
    }

    public void Move()
    {
        Vector2 direction = (player.gameObject.transform.position - transform.position).normalized;
        rigid.velocity = direction * Speed;
    }

    public void TakeDamage(float Damage)
    {
        Hp -= Damage;
        HPAmount();
        _anim.SetTrigger("Hit");

        if (Hp <= 0 && IsDeath == false)
        {
            DeathAction.Invoke();
            IsDeath = true;
        }
    }

    void HPAmount()
    {
        HPBarImage.fillAmount = Hp / FullHp;
    }

    public void Death()
    {
        if(MonsterDeath != null) MonsterDeath(gameObject);
        HPBar.SetActive(false);
        if(DamageCoroutine != null) StopCoroutine(DamageCoroutine);
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DieEffect());
        DeathAction -= Death;
        _anim.SetTrigger("Death");
        Player.Instance.GetCurrentExp += ExpValue;
        DropItem();
        GameManager.Instance.KillMobCount++;
    }

    void DropItem()
    {
        if (ItemManager.Instance.RandomSystem(DropPer))
        {
            GameObject DropItem = Instantiate(DropItemList[0].gameObject, transform.position, Quaternion.identity);
            DropItem.transform.SetParent(ItemManager.Instance.ItemListobject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && PlayerDamage == false)
        {
            if(player.GetHp > 0) DamageCoroutine = StartCoroutine(PlayerDamageCoroutine(player));
            PlayerDamage = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out Player player))
        {   
            if(DamageCoroutine != null) StopCoroutine(DamageCoroutine);
            PlayerDamage = false;
        }
    }

    IEnumerator PlayerDamageCoroutine(Player player) {
        while(player != null && player.GetHp > 0) {

            player.TakeDamage(Damage);

            yield return new WaitForSeconds(0.5f);
        }
        PlayerDamage = false;
    }

    private void OnDestroy() {
        if( DamageCoroutine != null ) StopCoroutine(DamageCoroutine);
    }

    

    IEnumerator FollowPath()
    {
        foreach (Node node in FinalNodeList)
        {
            //if(node == FinalNodeList[0]) continue;
            
            // 다음 노드의 월드 좌표로 변환
            Vector3 targetPosition = TileMapManager.Instance.WallMap.CellToWorld(new Vector3Int(node.x, node.y, 0)) + cellCenterOffset - GameManager.Instance.originCellPosition;
            // 유닛을 해당 위치로 이동
            while (Vector2.Distance(transform.position, targetPosition ) > 0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
                yield return null;
            }
        }
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
