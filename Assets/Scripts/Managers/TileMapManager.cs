using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : SingleTon<TileMapManager>
{
    [Header("맵 리스트")]
    public List<GameObject> MapList = new List<GameObject>();
    public int MapIndex;

    [Header("해당 맵 인덱스의 Box가 소환 될 수 있는 Pos List")]
    [HideInInspector] Tilemap BoxGenerateMap;
    public List<Vector2> BoxPos = new List<Vector2>();
    [Header("Box Prefabs")]
    public List<GameObject> BoxPrefabs = new List<GameObject>();
    public Transform ParentBoxObject;

    protected override void Awake() {
        base.Awake();
        BoxGenerateMap = GameObject.FindWithTag("BoxGenerate").GetComponent<Tilemap>();
    }

    private void Start() {
         /*
        맵 박스 생성
        1. TileMapManager -> Box Pos 구하기
        2. Box Pos List에서 80% 만큼 랜덤적으로 번호 뽑기
        3. 랜덤한 포지션에 박스를 생성하기
        
        */
        GetBoxPos(BoxGenerateMap);
        GenerateMapBox();
    }

    void GetBoxPos(Tilemap targetTilemap) {
        for(int x = targetTilemap.cellBounds.xMin; x < targetTilemap.cellBounds.xMax; x++) {
            for(int y = targetTilemap.cellBounds.yMin; y < targetTilemap.cellBounds.yMax; y++) {
                Vector3Int localplace = new Vector3Int(x, y, 0);
                Vector3 place = targetTilemap.CellToWorld(localplace);

                if(targetTilemap.HasTile(localplace)) {
                    BoxPos.Add(place);
                } else {
                    continue;
                }
            }
        }
    }

    void GenerateMapBox() {

        int Count = (int)(BoxPos.Count * 0.8);
        Debug.Log(Count);
        int[] RandomList = GameManager.Instance.GenerateRandomListIndex(BoxPos, Count);
        Debug.Log(RandomList);
        int BoxprefabIndex = Random.Range(0, BoxPrefabs.Count);

        for(int i = 0; i < RandomList.Length; i++) {
            GameObject Box = Instantiate(BoxPrefabs[BoxprefabIndex], new Vector2(BoxPos[RandomList[i]].x + 0.5f, BoxPos[RandomList[i]].y + 0.5f), Quaternion.identity);
            Box.transform.parent = ParentBoxObject;
        }

    }






}
