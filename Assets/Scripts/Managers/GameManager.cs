using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingleTon<GameManager>
{
    private Vector2 PlayerPos;
    public GameObject[] MonsterPrefabs;

    public Vector2 GetPlayerPos { get { return PlayerPos;} }
    public int MobCount;
    

    

    #region AreaType
    private Vector2 minBoxPos;
    private Vector2 maxBoxPos;
    private Vector2 minNoSpawnMob;
    private Vector2 maxNoSpawnMob;
    private Vector2 SpawnArea;
    #endregion
    private void Start()
    {
        _ = StartCoroutine(GenerateMonsterCoroutine());
    }

    private void Update()
    {
        PlayerPos = Player.Instance.transform.position;
        
    }

    IEnumerator GenerateMonsterCoroutine()
    {
            for(int i = 0; i < MobCount; i++) {
                SpawnMobArea(out Vector2 area);
                SpawnArea = area;
                if(!DontSpawnArea(SpawnArea, minNoSpawnMob, maxNoSpawnMob)) {
                    GameObject Unit = ObjectPoolManager.Instance.ActivePool(MonsterPrefabs);
                    Unit.transform.position = SpawnArea;
                }
            }
            yield return new WaitForSeconds(2f);
            StartCoroutine(GenerateMonsterCoroutine());

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

    public GameObject RandomMonster() {
        int RandomIndex = Random.Range(0, MonsterPrefabs.Length);

        return MonsterPrefabs[RandomIndex];
    }

    /*
    어떤 리스트에 있는 값들을 인덱스로 접근하기 위해
    RandomIntList에 목표 리스트의 인덱스 값들을 할당함. 
    Random.Range에서 RandomIntList.Count 만큼의 수를 랜덤으로 뽑음
    랜덤으로 뽑은 숫자. RandomIntList(Random)을 대입하여 그 안에있던 인덱스 값을 꺼내어 반환함. 
    */
    public int[] GenerateRandomListIndex<T>(List<T> list, int count) {
        
        if(list == null && list.Count < count) {
            Debug.Log("Error!! Out of Index --> list");
            return null;
        }

        List<int> RandomIntList = new List<int>(); 
        int[] ReturnIndex = new int[count];

        for(int i = 0; i < list.Count; i++) {
            RandomIntList.Add(i);
        }
        
        for(int i = 0; i < count; i++) {
            int index = Random.Range(0, RandomIntList.Count);
            ReturnIndex[i] = RandomIntList[index];
            RandomIntList.Remove(index);
        }

        return ReturnIndex;
    }

    
}
