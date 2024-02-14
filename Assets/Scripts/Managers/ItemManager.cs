using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;


public class ItemManager : SerializedMonoBehaviour
{
    public enum Operation { Add, Subract }
    public static ItemManager Instance { get; private set; }
    public Dictionary<BoxType, List<GameObject>> ItemList_dic = new Dictionary<BoxType, List<GameObject>>();

    public Transform ItemListobject;

    public List<GameObject> RoadItemStack = new List<GameObject>();
    public bool isRunning_SearchItemCoroutine;
    public float StartTime;
    public float RoadTime = 3f;
    public Sprite RoadSprite;
    public Sprite UnknownSprite;

    public List<Item> ItemList = new List<Item>();
    public SpriteRenderer Player_LeftArm;
    public SpriteRenderer Player_RightArm;
    public SpriteRenderer Player_Body;
    public SpriteRenderer Player_Helmat;

    private float Variance_Damage;
    private float Variance_Speed;
    private float Variance_FullHp;
    private float Variance_Hp;
    private float Variance_Hp_vampire;
    private float Variance_ArmorStat;

    public float GetVariance_Damage { get { return Variance_Damage; } }
    public float GetVariance_Speed { get { return Variance_Speed; } }
    public float GetVariance_FullHp { get { return Variance_FullHp; } }
    public float GetVariance_Hp { get { return Variance_Hp; } }
    public float GetVariance_Hp_vampire { get { return Variance_Hp_vampire; } }
    public float GetVariance_ArmorStat { get { return Variance_ArmorStat; } }

     private void Awake() {

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        Player_LeftArm = GameObject.Find("L_Weapon").GetComponent<SpriteRenderer>();
        Player_RightArm = GameObject.Find("R_Weapon").GetComponent<SpriteRenderer>();
        Player_Body = GameObject.Find("BodyArmor").GetComponent<SpriteRenderer>();
        Player_Helmat = GameObject.Find("11_Helmet1").GetComponent<SpriteRenderer>();
    }

    public void GamePlaySceneInit() {
        ItemListobject = GameObject.Find("ItemList").transform;
    }

    void UpdateStatManagement(Equipment equipment, Operation operation)
    {
        if (operation == Operation.Add)
        {
            Variance_Damage += equipment.Item_Damage;
            Variance_Speed += equipment.Item_Speed;
            Variance_FullHp += equipment.Item_FullHp;
            Variance_Hp += equipment.Item_Hp;
            Variance_Hp_vampire += equipment.Item_Hp_vampire;
            Variance_ArmorStat += equipment.Item_ArmorStat;
        }
        else if (operation == Operation.Subract)
        {
            Variance_Damage -= equipment.Item_Damage;
            Variance_Speed -= equipment.Item_Speed;
            Variance_FullHp -= equipment.Item_FullHp;
            Variance_Hp -= equipment.Item_Hp;
            Variance_Hp_vampire -= equipment.Item_Hp_vampire;
            Variance_ArmorStat -= equipment.Item_ArmorStat;
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "GamePlayScene") {

            //해당 아이템 박스를 오픈 했으면 먼저 어떤 아이템이 있든 아이템의 정보를 알 수 없음 -> 알수없는 스프라이트로 이미지 변경, 드래그 불가능하게 Image에 raycast target == false
            //아이템을 찾고자 어떤 버튼을 누르면 아이템을 찾기 시작
            //아이템 찾는 함수를 실행 후 일정 시간이 되면 끄고 아이템을 보여줌 스프라이트로 이미지 변경, 드래그 불가능하게 Image에 raycast target == true

            if (isRunning_SearchItemCoroutine == true)
            {
                if (RoadItemStack.Count != 0) ItemRoad(RoadItemStack[0].GetComponent<Item>());
            }
        }


    }


    /*
    장착 아이템에 Equipment, Weapon의 무기가 들어갈 경우 화면으로 보이는 플레이어의 스프라이트를 변경하도록 함.
    */
    public void ChangeEquipItem(Equipment item)
    {
        //equipment item을 받고 EquipType을 만들어서 타입에 맞는 SpriteRenderer에 적용 시키자. 
        if (item.equipType == Equipment.EquipType.Weapon)
        {
            Player_LeftArm.sprite = item.Player_sprite;
        }
        else if (item.equipType == Equipment.EquipType.Shield)
        {
            Player_RightArm.sprite = item.Player_sprite;
        }
        else if (item.equipType == Equipment.EquipType.Body)
        {
            Player_Body.sprite = item.Player_sprite;
        }
        else if (item.equipType == Equipment.EquipType.Helmat)
        {
            Player_Helmat.sprite = item.Player_sprite;
        }

        UpdateStatManagement(item, Operation.Add);
    }

    public void UnEquipSprite(Equipment item)
    {
        if (item.equipType == Equipment.EquipType.Weapon)
        {
            Player_LeftArm.sprite = null;
        }
        else if (item.equipType == Equipment.EquipType.Shield)
        {
            Player_RightArm.sprite = null;
        }
        else if (item.equipType == Equipment.EquipType.Body)
        {
            Player_Body.sprite = null;
        }
        else if (item.equipType == Equipment.EquipType.Helmat)
        {
            Player_Helmat.sprite = null;
        }

        UpdateStatManagement(item, Operation.Subract);
    }


    //드랍율을 할당하면 드랍율에 따라 아이템을 드랍해주는 시스템
    /*
    Random.Range(0, Max ==> 100) 100을 최대로 두고 float 매개변수를 하나를 받는다
    float는 나올 확률을 결정함. 
    float 값 보다 아래일 경우 drop true
    아니면 drop false
    */
    public bool RandomSystem(int percent)
    {
        int index = Random.Range(0, 100);

        if (index >= percent) return true;
        else return false;
    }

    public void SearchItem()
    {
        if (RoadItemStack.Count != 0)
        {
            isRunning_SearchItemCoroutine = true;
            StartCoroutine(PlayRoadAnimation(RoadItemStack[0]));
        }


    }

    public void ItemRoad(Item item)
    {
        item.Cumulative_Time = Time.time - StartTime;
        item._itemimage_Component.sprite = RoadSprite;
        

        if (item.Cumulative_Time >= RoadTime)
        {
            StopCoroutine(PlayRoadAnimation(item.gameObject));
            item._itemimage_Component.sprite = item.itemSprite;
            item._itemimage_Component.raycastTarget = true;
            item._itemimage_Component.transform.rotation = Quaternion.identity;
            item.CheckItemTrue = true;
            AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.FindItem);

            RoadItemStack.RemoveAt(0);
            if (RoadItemStack.Count != 0)
            {
                StartCoroutine(PlayRoadAnimation(RoadItemStack[0]));

                ItemRoad(RoadItemStack[0].GetComponent<Item>());
            }

        }
    }


    IEnumerator PlayRoadAnimation(GameObject item)
    {
        float cumulativeAngle = 0f; // 누적 각도
        StartTime = Time.time;
        AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.SearchItem);
        while (item.GetComponent<Item>().Cumulative_Time <= RoadTime)
        {
            float rotationThisFrame = Time.deltaTime * -1.5f * 360; // 이번 프레임에서의 회전 각도
            cumulativeAngle += rotationThisFrame; // 각도 누적
            cumulativeAngle %= 360; // 360도를 넘어가면 0으로 초기화

            item.gameObject.transform.rotation = Quaternion.Euler(0, 0, cumulativeAngle);

            yield return null;
        }
    }


}
