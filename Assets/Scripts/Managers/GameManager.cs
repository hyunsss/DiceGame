using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public GameObject[] MonsterPrefabs;

    private void Start() {
        _= StartCoroutine(GenerateMonsterCoroutine());
    }


    IEnumerator GenerateMonsterCoroutine() {
        while(true) {
            for(int i = 0; i < 4; i++) {
                Vector3 randomPos = Random.insideUnitSphere * 5;

				GameObject Unit = ObjectPoolManager.Instance.ActivePool(GameManager.Instance.MonsterPrefabs);

				Unit.transform.position = randomPos;
                //Todo : 플레이어 기준 사각형 외각에 몬스터가 스폰되도록 설정

            }
        yield return new WaitForSeconds(2f);
        }

    }
}
