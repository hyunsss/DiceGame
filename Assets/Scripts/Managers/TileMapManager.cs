using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : SingleTon<TileMapManager>
{

    public Tilemap WallMap;

    [Header("맵 리스트")]
    public List<GameObject> MapList = new List<GameObject>();
    public int MapIndex;

    [Header("해당 맵 인덱스의 Box가 소환 될 수 있는 Pos List")]
    [HideInInspector] Tilemap BoxGeneratePoint;
    public List<Vector2> BoxPos = new List<Vector2>();
    [Header("Box Prefabs")]
    public List<GameObject> BoxPrefabs = new List<GameObject>();
    public Transform ParentBoxObject;
    public ItemBox[] BoxList;

    [Header("탈출 리스트")]
    [HideInInspector] Tilemap EscapePoint;
    public GameObject EscapePrefab;
    public Transform ParentEscapeObject;
    public EscapePoint[] EscapeList;
    public List<Vector2> EscapePos = new List<Vector2>();
    public TextMeshProUGUI EscapeText;
    public GameObject EscapeUI;

    [Header("스폰 포인트")]
    public Tilemap SpawnPoint;
    public List<Vector2> SpawnPos = new List<Vector2>();


    private void Start() {
        BoxGeneratePoint = GameObject.FindWithTag("BoxGenerate").GetComponent<Tilemap>();
        EscapePoint = GameObject.FindWithTag("EscapeGenerate").GetComponent<Tilemap>();
        SpawnPoint = GameObject.FindWithTag("SpawnGenerate").GetComponent<Tilemap>();

         /*
        맵 박스 생성
        1. TileMapManager -> Box Pos 구하기
        2. Box Pos List에서 80% 만큼 랜덤적으로 번호 뽑기
        3. 랜덤한 포지션에 박스를 생성하기
        
        */
        GetTilePos(BoxPos, BoxGeneratePoint);
        GetTilePos(EscapePos, EscapePoint);
        GetTilePos(SpawnPos, SpawnPoint);
        GenerateMapBox();
        GenerateEscapePoint();
        BoxList = ParentBoxObject.GetComponentsInChildren<ItemBox>();
        EscapeList = ParentEscapeObject.GetComponentsInChildren<EscapePoint>();

        
        // int index = Random.Range(0, SpawnPos.Count);
        // Vector3Int StartPosition = WallMap.CellToWorld(new Vector3Int(SpawnPos[index].x, SpawnPos[index].y, 0));
        // Player.Instance.transform.position = new Vector3
    }

   
    void GetTilePos(List<Vector2> list, Tilemap targetTilemap) {
        for(int x = targetTilemap.cellBounds.xMin; x < targetTilemap.cellBounds.xMax; x++) {
            for(int y = targetTilemap.cellBounds.yMin; y < targetTilemap.cellBounds.yMax; y++) {
                Vector3Int localplace = new Vector3Int(x, y, 0);
                Vector3 place = targetTilemap.CellToWorld(localplace);

                if(targetTilemap.HasTile(localplace)) {
                    list.Add(place);
                } else {
                    continue;
                }
            }
        }
    }

    void GenerateMapBox() {

        int Count = (int)(BoxPos.Count * 0.8);
        int[] RandomList = GameManager.Instance.GetRandomIndexesFromList(BoxPos, Count);
        int BoxprefabIndex = Random.Range(0, BoxPrefabs.Count);

        for(int i = 0; i < RandomList.Length; i++) {
            GameObject Box = Instantiate(BoxPrefabs[BoxprefabIndex], new Vector2(BoxPos[RandomList[i]].x + 0.5f, BoxPos[RandomList[i]].y + 0.5f), Quaternion.identity);
            Box.transform.parent = ParentBoxObject;
            Box.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    void GenerateEscapePoint() {
        foreach(Vector2 point in EscapePos) {
            GameObject escape = Instantiate(EscapePrefab, new Vector2(point.x + 0.5f, point.y + 0.5f), Quaternion.identity);
            escape.transform.parent = ParentEscapeObject;
        }
        
    }

    





}
