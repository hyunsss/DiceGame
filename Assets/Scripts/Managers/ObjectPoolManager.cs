using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager : SingleTon<ObjectPoolManager>
{

    [SerializeField]private List<GameObject> UsePrefabs = new List<GameObject>();
    [SerializeField]private List<GameObject> UnUsePrefabs = new List<GameObject>();

    public int InitPrefabCount;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for(int i = 0; i < InitPrefabCount; i++) {
            CreatePrefabs(GameManager.Instance.MonsterPrefabs[0], out GameObject Unit);
            Unit.transform.parent = transform;
            Unit.SetActive(false);
            UnUsePrefabs.Add(Unit);
        }
    }
    //특정 프리팹을 받아 생성하고 반환
    public void CreatePrefabs(GameObject Prefab, out GameObject Unit) {
        Unit = Instantiate(Prefab);
    }

    //게임 오브젝트를 활성화
    public GameObject ActivePool(GameObject[] Prefabs)
    {
        if (UnUsePrefabs.Count > 0)
        {
            GameObject Unit = UnUsePrefabs[0];
            if (UnUsePrefabs.Remove(Unit))
            {
                UsePrefabs.Add(Unit);
            }
            
            IresetTable resetTable = Unit.GetComponent<IresetTable>(); // 몬스터의 경우 체력이 0인 상태이므로 초기화 인터페이스 작성
            if(resetTable != null) {
                resetTable.Reset(); // 스텟 초기화 구문
            }

            Unit.transform.parent = null;
            Unit.SetActive(true);
            return Unit;
        }
        else
        {
            int RandomIndex = Random.Range(0, Prefabs.Length);
            GameObject Unit = Instantiate(Prefabs[RandomIndex]);

            UsePrefabs.Add(Unit);
            return Unit;
        }
    }

    

    //게임 오브젝트 비 활성화
    public void UnActivePool(GameObject Prefab)
    {
        
        if (UsePrefabs != null && UsePrefabs.Remove(Prefab))
        {
            Prefab.transform.parent = transform;
            Prefab.SetActive(false);
            UnUsePrefabs.Add(Prefab);
        }

    }

    
}
