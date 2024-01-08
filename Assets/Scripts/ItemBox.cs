using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public BoxType boxType;

    List<Item> currentItemList = new List<Item>();
    private Slot[] RootBoxslots;

    private void Awake() {
        RootBoxslots = InventoryManager.Instance.RootBoxPanel.GetComponentsInChildren<Slot>();
    }
    void Start()
    {
        SetItemList();
    }

    void SetItemList() {
        int item_Count = Random.Range(1, 4);

        List<Item> itemList = ItemManager.Instance.ItemList_dic[boxType];

        for(int i = 0; i < item_Count; i++) {
            int itemList_index = Random.Range(0, itemList.Count);
            currentItemList.Add(itemList[itemList_index]);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.TryGetComponent(out Player player)) {
            if (Input.GetKeyDown(KeyCode.E)) {
                for(int i = 0; i < currentItemList.Count; i++) {
                    InventoryManager.Instance.AcquireItem(currentItemList[i], RootBoxslots);
                }
                InventoryManager.Instance.OpenInventory();
                InventoryManager.Instance.RootBoxPanel.SetActive(true);

            }
        }
    }


}
