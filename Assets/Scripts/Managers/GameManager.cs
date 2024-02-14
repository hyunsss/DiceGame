using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class GameManager : SingleTon<GameManager>
{
    private Player player;
    private Vector2 PlayerPos;
    public GameObject[] MonsterPrefabs;

    public Vector2 GetPlayerPos { get { return PlayerPos; } }

    public int MobCount;

    [Header("KillMobCount")]
    public int KillMobCount;
    public TextMeshProUGUI KillCount;


    [Header("DeathPanel")]
    public UnityAction PlayerDeathAction;
    public GameObject DeathPanel;
    public TextMeshProUGUI DeathPanel_KillText;
    public TextMeshProUGUI DeathPanel_TimeText;
    


    [Header("A Star")]
    public int sizeX, sizeY;
    public Node[,] NodeArray;
    public Vector3Int originCellPosition;
    public bool allowDiagonal, dontCrossCorner;

    #region AreaType
    private Vector2 minBoxPos;
    private Vector2 maxBoxPos;
    private Vector2 minNoSpawnMob;
    private Vector2 maxNoSpawnMob;
    private Vector2 SpawnArea;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        PlayerData.Instance.PlaySceneLoadData();
        SkillManager.Instance.GameSceneStartLoadSkill();
    }

    private void Start()
    {   
        player = GameObject.Find("Player").GetComponent<Player>();
        player.transform.position = Vector3.zero;
        KillMobCount = 0;
        SetTilemapNode();
        PlayerDeathAction += PlayerDeath;

        _ = StartCoroutine(GenerateMonsterCoroutine());

    }

    public void SetTilemapNode()
    {
        Vector3Int minTilePos = TileMapManager.Instance.WallMap.cellBounds.min;
        Vector3Int maxTilePos = TileMapManager.Instance.WallMap.cellBounds.max;

        BoundsInt bounds = TileMapManager.Instance.WallMap.cellBounds;
        Vector3Int minCellPosition = bounds.min;

        // Tilemap의 왼쪽 하단을 (0, 0)으로 설정합니다.
        originCellPosition = new Vector3Int(-minCellPosition.x, -minCellPosition.y, 0);

        sizeX = maxTilePos.x - minTilePos.x + 1;
        sizeY = maxTilePos.y - minTilePos.y + 1;

        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 worldPosition = TileMapManager.Instance.WallMap.CellToWorld(new Vector3Int(i, j, 0));
                Collider2D collider = Physics2D.OverlapPoint(worldPosition - originCellPosition);
                bool isWall = false;
                if (collider != null)
                {
                    isWall = true;
                }

                NodeArray[i, j] = new Node(isWall, i, j);
            }
        }
    }

    private void FixedUpdate()
    {
        KillCount.text = $"{KillMobCount}";
        PlayerPos = Player.Instance.transform.position;

        if (TimeManager.Instance.IsNightTime())
        {
            MobCount = 15;
            foreach (EscapePoint escape_point in TileMapManager.Instance.EscapeList)
            {
                escape_point.GetComponent<Collider2D>().enabled = false;
            }
        }


        if (TimeManager.Instance.IsDayTime())
        {
            MobCount = 3;
            foreach (EscapePoint escape_point in TileMapManager.Instance.EscapeList)
            {
                escape_point.GetComponent<Collider2D>().enabled = true;
            }
        }

    }

    void PlayerDeath() {
        StopAllCoroutines();
        Time.timeScale = 0.2f;
        DeathPanel.SetActive(true);
        DeathPanel_KillText.text = $"몬스터 킬 수 : {KillMobCount}";
        DeathPanel_TimeText.text = $"생존한 시간 : {TimeManager.Instance.ElapsedTime()}";
    }

    public void MainMenuButton() {
        Time.timeScale = 1f;
        NextMainMenuScene();
    }

    public void NextMainMenuScene() {
        PlayerData.Instance.PlayerStatReset();
        StopCoroutine(GenerateMonsterCoroutine());
        PlayerData.Instance.PlaySceneSaveData();
        SceneManager.LoadScene("MainMenuScene");
    }

    #region Monster Generate
    public IEnumerator GenerateMonsterCoroutine()
    {
        if (ObjectPoolManager.Instance.GetSpawnMobList.Count < 70)
        {
            for (int i = 0; i < MobCount; i++)
            {
                SpawnMobArea(out Vector2 area);
                SpawnArea = area;
                if (!DontSpawnArea(SpawnArea, minNoSpawnMob, maxNoSpawnMob) && CheckOverlapCollider(SpawnArea))
                {
                    GameObject Unit = ObjectPoolManager.Instance.ActivePool(MonsterPrefabs);
                    Unit.transform.position = SpawnArea;

                }
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(GenerateMonsterCoroutine());

    }

    private bool CheckOverlapCollider(Vector2 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1f);

        return colliders.Length == 0;
    }

    private void SpawnMobArea(out Vector2 SpawnArea)
    {
        minBoxPos = AreaSetPos(-13f, -8f);
        maxBoxPos = AreaSetPos(13f, 8f);

        minNoSpawnMob = AreaSetPos(-6f, -3f);
        maxNoSpawnMob = AreaSetPos(6f, 3f);

        SpawnArea = new Vector2(Random.Range(minBoxPos.x, maxBoxPos.x), Random.Range(minBoxPos.y, maxBoxPos.y));
    }

    private bool DontSpawnArea(Vector2 position, Vector2 minNoSpawn, Vector2 maxNoSpawn)
    {
        return position.x >= minNoSpawn.x && position.x <= maxNoSpawn.x
        && position.y >= minNoSpawn.y && position.y <= maxNoSpawn.y;
    }


    private Vector2 AreaSetPos(float x, float y)
    {
        return new Vector2(PlayerPos.x + x, PlayerPos.y + y);
    }

    public GameObject RandomMonster()
    {
        int RandomIndex = Random.Range(0, MonsterPrefabs.Length);

        return MonsterPrefabs[RandomIndex];
    }
    #endregion

    /*
    어떤 리스트에 있는 값들을 인덱스로 접근하기 위해
    RandomIntList에 목표 리스트의 인덱스 값들을 할당함. 
    Random.Range에서 RandomIntList.Count 만큼의 수를 랜덤으로 뽑음
    랜덤으로 뽑은 숫자. RandomIntList(Random)을 대입하여 그 안에있던 인덱스 값을 꺼내어 반환함. 
    */
    public int[] GetRandomIndexesFromList<T>(List<T> list, int count)
    {

        if (list == null || list.Count < count)
        {
            Debug.Log("Error!! Out of Index --> list");
            return null;
        }

        List<int> RandomIntList = new List<int>();
        int[] ReturnIndex = new int[count];

        for (int i = 0; i < list.Count; i++)
        {
            RandomIntList.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, RandomIntList.Count);
            ReturnIndex[i] = RandomIntList[index];
            RandomIntList.RemoveAt(index);
        }

        return ReturnIndex;
    }

    public Stack<T> SortStack<T>(Stack<T> currentstack)
    {
        Stack<T> Tempstack = new Stack<T>(currentstack);
        currentstack.Clear();

        while (Tempstack.Count > 0)
        {
            currentstack.Push(Tempstack.Pop());
        }

        return currentstack;
    }




}
