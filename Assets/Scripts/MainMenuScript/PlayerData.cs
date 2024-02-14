using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PlayerData : SerializedMonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public List<GameObject> ItemPrefabs = new List<GameObject>();
    public List<Dictionary<string, object>> Testfile;

    public ScrollRect StorageScroll;

    public int PlayerMoney = 99999999;

    GameObject[] InventorySlotItems;
    GameObject[] EquipmentSlotItems;
    GameObject[] PassiveSlotItems;
    GameObject[] SkillSlotItems;
    GameObject[] StorageSlotItems;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
        EquipmentSlotItems = new GameObject[3];
        PassiveSlotItems = new GameObject[3];
        SkillSlotItems = new GameObject[6];
        InventorySlotItems = new GameObject[30];
        StorageSlotItems = new GameObject[MainInventory.Instance.storageSlots.Length];
        Testfile = CSVReader.Read("itemCSV"); 
    }

    public void PlayerStatReset() {
        Player.Instance.IsAttack = true;
        Player.Instance.GetHp = Player.Instance.GetFullHp;
        Player.Instance.GetCurrentExp = 0;
        Player.Instance.GetMaxExp = 100;
        Player.Instance.GetLevel = 1;
        Player.Instance.IsDeath = false;
    }

    private void Start() {
        ItemStringInit();
        StarterPack("S_Shuriken");
        StarterPack("S_Rocket");
        StarterPack("S_RotateKnife");
    }

    public void PlaySceneLoadData() {
        Debug.Log("플레이 씬 로드 데이터 실행");
        LoadItemData(InventorySlotItems, InventoryManager.Instance.inventorySlots);
        LoadItemData(EquipmentSlotItems, InventoryManager.Instance.equipSlots);
        LoadItemData(PassiveSlotItems, InventoryManager.Instance.passiveSlots);
        LoadItemData(SkillSlotItems, InventoryManager.Instance.skillSlots);

        AudioManager.Instance.BgmPlayer.clip = AudioManager.Instance.GamePlayAudioClip;
        AudioManager.Instance.BgmPlayer.Play();
    }

    public void MainMenuSceneLoadData() {
        Debug.Log("메인메뉴 로드 데이터 실행");
        LoadItemData(InventorySlotItems, MainInventory.Instance.inventorySlots);
        LoadItemData(EquipmentSlotItems, MainInventory.Instance.equipSlots);
        LoadItemData(PassiveSlotItems, MainInventory.Instance.passiveSlots);
        LoadItemData(SkillSlotItems, MainInventory.Instance.skillSlots);
        LoadItemData(StorageSlotItems, MainInventory.Instance.storageSlots);

        AudioManager.Instance.BgmPlayer.clip = AudioManager.Instance.MainmenuAudioClip;
        AudioManager.Instance.BgmPlayer.Play();
    }

    void LoadItemData(GameObject[] SlotItem, Slot[] targetSlots) {
        for(int i = 0; i < SlotItem.Length; i++) {
            targetSlots[i].itemPrefab = SlotItem[i];
            if(SlotItem[i] != null) {
                Debug.Log(SlotItem[i]);
                GameObject item = Instantiate(SlotItem[i]);
                item.name = SlotItem[i].name;
                item.GetComponent<Collider2D>().enabled = false;
                item.transform.parent = targetSlots[i].gameObject.transform;
                item.transform.localScale = new Vector3(1, 1, 1);
                
            }
        }
    }

    public void PlaySceneSaveData() {
        SaveItemData(InventorySlotItems, InventoryManager.Instance.inventorySlots);
        SaveItemData(EquipmentSlotItems, InventoryManager.Instance.equipSlots);
        SaveItemData(PassiveSlotItems, InventoryManager.Instance.passiveSlots);
        SaveItemData(SkillSlotItems, InventoryManager.Instance.skillSlots);
    }

    public void MainMenuSceneSaveData() {
        SaveItemData(InventorySlotItems, MainInventory.Instance.inventorySlots);
        SaveItemData(EquipmentSlotItems, MainInventory.Instance.equipSlots);
        SaveItemData(PassiveSlotItems, MainInventory.Instance.passiveSlots);
        SaveItemData(SkillSlotItems, MainInventory.Instance.skillSlots);
        SaveItemData(StorageSlotItems, MainInventory.Instance.storageSlots);

    }
    
    void SaveItemData(GameObject[] Slots, Slot[] TargetSlot) {
        for(int i = 0; i < Slots.Length; i++) {
            if(TargetSlot[i].itemPrefab != null) {
                Debug.Log(TargetSlot[i].itemPrefab);
                foreach(GameObject itemPrefab in ItemPrefabs) {
                    if(TargetSlot[i].itemPrefab.name == itemPrefab.name) {
                        Slots[i] = itemPrefab;
                        Debug.Log("name true" + Slots[i]);
                    }
                }
            } else {
                Slots[i] = null;
            }
        }
    }

    void ItemStringInit() {
        foreach(var item in ItemPrefabs) {
            foreach(var data in Testfile) {
                if(data["ID"].ToString() == item.name) {
                    item.GetComponent<Item>()._itemname = data["Name"].ToString();
                    item.GetComponent<Item>()._itemdesc = data["DESC"].ToString();
                    item.GetComponent<Item>().type = (ItemType)(int)data["TYPE"];
                    item.GetComponent<Item>()._itemprize = (int)data["PRIZE"];
                    string ImagePath = data["ImagePath"].ToString();
                    item.GetComponent<Item>().itemSprite = Resources.Load<Sprite>($"ItemTexture/{ImagePath}");
                    break;
                }
            }
        }
    }

    void StarterPack(string name) {
        GameObject go = Instantiate(GetFindItem(name));
        go.GetComponent<Collider2D>().enabled = false;
        go.name = GetFindItem(name).name;
        MainInventory.Instance.AcquireItem(go.GetComponent<Item>() ,MainInventory.Instance.storageSlots);
    }
    public GameObject GetFindItem(string name) {
        foreach(GameObject itemprefab in ItemPrefabs)
        {
            if(itemprefab.name == name) return itemprefab;
        }
        return null;
    }

    public GameObject GetFindItem(GameObject item) {
        foreach(GameObject itemprefab in ItemPrefabs)
        {
            if(itemprefab.name == item.name) return itemprefab;
        }
        return null;
    }
    
}
