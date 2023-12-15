using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    private Vector2 PlayerPos;
    public GameObject[] MonsterPrefabs;

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

    private void FixedUpdate()
    {
        PlayerPos = Player.Instance.transform.position;
    }

    IEnumerator GenerateMonsterCoroutine()
    {
        while (true)
        {
            do
            {
                SpawnMobArea(out Vector2 Area);
                SpawnArea = Area;
            }
            while (!DontSpawnArea(SpawnArea, minNoSpawnMob, maxNoSpawnMob));
            {
                for (int i = 0; i < MobCount; i++)
                {
                    GameObject Unit = ObjectPoolManager.Instance.ActivePool(MonsterPrefabs);
                    Unit.transform.position = SpawnArea;
                }
                yield return new WaitForSeconds(2f);
            }
        }

    }

    private void SpawnMobArea(out Vector2 SpawnArea)
    {
        minBoxPos = AreaSetPos(-13f, -8f);
        maxBoxPos = AreaSetPos(13f, 8f);

        minNoSpawnMob = AreaSetPos(-8f, -5f);
        maxNoSpawnMob = AreaSetPos(8f, 5f);

        SpawnArea = new Vector2(Random.Range(minBoxPos.x, maxBoxPos.x), Random.Range(minBoxPos.y, maxBoxPos.y));

    }


    private bool DontSpawnArea(Vector2 posiiton, Vector2 minNoSpawn, Vector2 maxNoSpawn)
    {
        return posiiton.x >= maxNoSpawn.x && posiiton.x <= minNoSpawn.x && posiiton.y >= maxNoSpawn.y && posiiton.y <= maxNoSpawn.y;
    }


    private Vector2 AreaSetPos(float x, float y)
    {

        return new Vector2(PlayerPos.x + x, PlayerPos.y + y);
    }

}
