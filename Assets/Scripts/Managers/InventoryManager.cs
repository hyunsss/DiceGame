using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingleTon<InventoryManager>
{
    public Transform DropedItemObject;
    public List<Item> InTriggerItemObject = new List<Item>();

    public GameObject InventoryGameobject;
    public GameObject RootBoxGameobject;
    public Dictionary<SlotParentUIType, GameObject> SlotParentUI = new Dictionary<SlotParentUIType, GameObject>();

    public GameObject InventorySlotsPanel;
    public GameObject EquimentPanel;
    public GameObject PassivePanel;
    public GameObject RootBoxPanel;
    public GameObject CurrentSkillPanel;


    public Slot[] inventorySlots;
    public Slot[] equipSlots;
    public Slot[] passiveSlots;
    public Slot[] skillSlots;

    [HideInInspector] public GameObject MouseEnterTarget;

    protected override void Awake()
    {
        base.Awake();
        inventorySlots = InventorySlotsPanel.GetComponentsInChildren<Slot>();
        equipSlots = EquimentPanel.GetComponentsInChildren<Slot>();
        passiveSlots = PassivePanel.GetComponentsInChildren<Slot>();
        skillSlots = CurrentSkillPanel.GetComponentsInChildren<Slot>();
    }

    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();

        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Q))
        {
            if (MouseEnterTarget != null)
            {
                MouseEnterTarget.GetComponentInParent<Slot>().itemPrefab = null;
                MouseEnterTarget.transform.SetParent(DropedItemObject);
                Destroy(MouseEnterTarget.GetComponentInChildren<Image>());
                MouseEnterTarget.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                MouseEnterTarget.GetComponent<Collider2D>().enabled = true;
                float spriteWidth = MouseEnterTarget.GetComponent<Item>().spriteRenderer_Component.sprite.bounds.size.x;
                float spriteHeight = MouseEnterTarget.GetComponent<Item>().spriteRenderer_Component.sprite.bounds.size.y;

                // 스프라이트의 가로나 세로 중 더 큰 쪽을 기준으로 스케일 계산
                float largestSide = Mathf.Max(spriteWidth, spriteHeight);
                float scale = 0.6f / largestSide;

                MouseEnterTarget.GetComponent<Item>().spriteRenderer_Component.transform.localScale = new Vector3(scale, scale, 0);

                MouseEnterTarget.GetComponent<RectTransform>().transform.position = GameManager.Instance.GetPlayerPos;

                MouseEnterTarget = null;
            } else {
                return;
            }

        }


    }

    void LoadInventory() {

    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (SlotParentUI[SlotParentUIType.Inventory].activeSelf == true)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void ShiftMoveItem(Item item)
    {
        //target slot panel 설정
        GameObject target = SetTargetSlot(item);

        // panel의 slot들 배열에 저장
        Slot[] slots = target.GetComponentsInChildren<Slot>();
        item.transform.parent.GetComponent<Slot>().itemPrefab = null;
        // 순차적으로 탐색후 빈 곳에 저장
        AcquireItem(item, slots);

    }


    private GameObject SetTargetSlot(Item item)
    {
        GameObject currentSlot = item.transform.parent.parent.gameObject;

        GameObject target = null;
        if (currentSlot == InventorySlotsPanel)
        {
            if (RootBoxPanel.activeSelf == true)
            {
                target = RootBoxPanel;
            } //인벤토리 추가 될 때 else if문 추가하기
        }
        else if (currentSlot == RootBoxPanel)
        {
            target = InventorySlotsPanel;
        }

        return target;
    }



    public void CloseInventory()
    {
        InventoryGameobject.SetActive(false);
        Player.Instance.IsAttack = true;
        Player.Instance.IsMove = true;
    }

    public void OpenInventory()
    {
        InventoryGameobject.SetActive(true);
        Player.Instance.IsAttack = false;
        Player.Instance.IsMove = false;
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
        item.GetComponent<Collider2D>().enabled = false;
        item.transform.position = Vector3.zero;
        item.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<RectTransform>().position;
        item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }



}
