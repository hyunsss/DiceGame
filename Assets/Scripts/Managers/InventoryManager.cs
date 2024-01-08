using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleTon<InventoryManager>
{
    private bool InventoryActive = false;
    public GameObject InventoryGameobject;

    public GameObject InventorySlotsPanel;
    public GameObject EquimentPanel;
    public GameObject PassivePanel;
    public GameObject RootBoxPanel;
    public GameObject CurrentSkillPanel;

    public Slot[] inventorySlots;
    public Slot[] equipSlots;



    // Start is called before the first frame update
    void Start()
    {
        inventorySlots = InventorySlotsPanel.GetComponentsInChildren<Slot>();
        equipSlots = EquimentPanel.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory() {
        if(Input.GetKeyDown(KeyCode.I)) {
            if(InventoryActive == true) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }

    public void ShiftMoveItem(Item item) {
        //target slot panel 설정
        GameObject target = SetTargetSlot(item);

        // panel의 slot들 배열에 저장
        Slot[] slots = target.GetComponentsInChildren<Slot>();
        item.transform.parent.GetComponent<Slot>().item = null;
        // 순차적으로 탐색후 빈 곳에 저장
        AcquireItem(item, slots);
    }


    private GameObject SetTargetSlot(Item item) {
        GameObject currentSlot = item.transform.parent.parent.gameObject;

        GameObject target = null;
        if(currentSlot == InventorySlotsPanel) {
            if(RootBoxPanel.activeSelf == true) {
                target = RootBoxPanel;
            } //인벤토리 추가 될 때 else if문 추가하기
        } else if(currentSlot == RootBoxPanel) {
            target = InventorySlotsPanel;
        }

        return target;
    }



    public void CloseInventory() {
        InventoryGameobject.SetActive(false);
        InventoryActive = false;
    }

    public void OpenInventory() {
        InventoryGameobject.SetActive(true);
        InventoryActive = true;
    }

    public void AcquireItem(Item item, Slot[] slots) {
        for(int i = 0; i < slots.Length; i++) {
            if(slots[i].item == null) {
                slots[i].AddItem(item);
                return;
            }
        }
    }

}
