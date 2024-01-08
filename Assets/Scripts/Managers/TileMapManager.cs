using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : SingleTon<TileMapManager>
{
    [Header("맵 리스트")]
    public List<Tilemap> MapList = new List<Tilemap>();
    public int MapIndex;
    [Header("해당 맵 인덱스의 Box가 소환 될 수 있는 Pos List")]
    public List<Vector2> BoxPos = new List<Vector2>();
    [Header("Box Prefabs")]
    public List<GameObject> BoxPrefab = new List<GameObject>();

    private void Start() {
        GetBoxPos(MapList[MapIndex]);
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

        int Count = (int)(TileMapManager.Instance.BoxPos.Count * 0.8);

        int[] RandomList = GameManager.Instance.GenerateRandomListIndex(TileMapManager.Instance.BoxPos, Count);

        
    }






}
