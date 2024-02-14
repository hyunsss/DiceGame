using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainInventory : SingleTon<MainInventory>
{
    [HideInInspector] public List<Slot[]> SlotsList = new List<Slot[]>();
    public TextMeshProUGUI StorageTotalValueText;
    public TextMeshProUGUI CurrentGoldValueText;

    public GameObject InventorySlotsPanel;
    public GameObject EquimentPanel;
    public GameObject PassivePanel;
    public GameObject CurrentSkillPanel;
    public GameObject StorageSlotsPanel;
    public GameObject ShopCellPanel;
    public GameObject ShopPanel;

    public Slot[] inventorySlots;
    public Slot[] equipSlots;
    public Slot[] passiveSlots;
    public Slot[] skillSlots;
    public Slot[] storageSlots;
    public Slot[] shopCellSlots;
    public Slot[] shopSlots;

    protected override void Awake() {
        base.Awake();

        inventorySlots = InventorySlotsPanel.GetComponentsInChildren<Slot>();
        equipSlots = EquimentPanel.GetComponentsInChildren<Slot>();
        passiveSlots = PassivePanel.GetComponentsInChildren<Slot>();
        skillSlots = CurrentSkillPanel.GetComponentsInChildren<Slot>();
        storageSlots = StorageSlotsPanel.GetComponentsInChildren<Slot>();
        shopCellSlots = ShopCellPanel.GetComponentsInChildren<Slot>();

        SlotsList.Add(inventorySlots);
        SlotsList.Add(equipSlots);
        SlotsList.Add(passiveSlots);
        SlotsList.Add(skillSlots);
        SlotsList.Add(storageSlots);
    }

    private void Update() {
        StorageTotalValueText.text = $"{TotalStorageValue()}";
        CurrentGoldValueText.text = $"{PlayerData.Instance.PlayerMoney}";
    }

    private GameObject SetTargetSlot(Item item)
    {
        GameObject currentSlotPanel = item.transform.parent.parent.gameObject;

        GameObject target = null;
        if (currentSlotPanel == InventorySlotsPanel)
        {
            if(StorageSlotsPanel.activeInHierarchy == true) target = StorageSlotsPanel;
        } else if (currentSlotPanel == StorageSlotsPanel) {
            if(InventorySlotsPanel.activeInHierarchy == true) target = InventorySlotsPanel;
            if(ShopCellPanel.activeInHierarchy == true) target = ShopCellPanel;
            Debug.Log(ShopCellPanel.activeInHierarchy);
        } else if (currentSlotPanel == ShopCellPanel) {
            if(StorageSlotsPanel.activeInHierarchy == true) target = StorageSlotsPanel;
        } else if (currentSlotPanel == EquimentPanel) {
            if(InventorySlotsPanel.activeInHierarchy == true) target = InventorySlotsPanel;
        } else if (currentSlotPanel == PassivePanel) {
            if(InventorySlotsPanel.activeInHierarchy == true) target = InventorySlotsPanel;
        } else if (currentSlotPanel == CurrentSkillPanel) {
            if(InventorySlotsPanel.activeInHierarchy == true) target = InventorySlotsPanel;
        } else if (currentSlotPanel == ShopPanel) target = null;

        return target;
    }

    public void ShiftMoveItem(Item item)
    {
        //target slot panel 설정
        GameObject target = SetTargetSlot(item);
        Debug.Log(target);

        if(target == null) return;

        // panel의 slot들 배열에 저장
        Slot[] slots = target.GetComponentsInChildren<Slot>();
        item.transform.parent.GetComponent<Slot>().itemPrefab = null;
        // 순차적으로 탐색후 빈 곳에 저장
        AcquireItem(item, slots);

    }

    public void AcquireItem(Item item, Slot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemPrefab == null)
            {
                AddItem(item, slots[i].transform);
                return;
            }
        }
    }

    public void AddItem(Item item, Transform transform)
    {
        transform.GetComponentInChildren<Slot>().itemPrefab = item.gameObject;
        item.transform.SetParent(transform);
        item.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<RectTransform>().position;
        item.GetComponent<Collider2D>().enabled = false;
        item.transform.position = Vector3.zero;
        item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    private int TotalStorageValue() {
        int TotalValue = 0;
        foreach(Slot[] slots in SlotsList) {
            for(int i = 0; i < slots.Length; i++) {
                if(slots[i].itemPrefab != null) {
                    TotalValue += slots[i].GetComponentInChildren<Item>()._itemprize;
                }
            }
        }
        return TotalValue;
    }

}
