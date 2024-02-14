using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public GameObject StoragePanel;
    public Slot[] StorageSlots;
    public GameObject ShopItemPanel;
    public Slot[] ShopSlots;

    [Header("ShopPanel")]
    public Button SelectCell;
    public Button SelectBuy;
    


    [Header("CellPanel")]
    public GameObject CellPanel;
    public Slot[] CellSlots;
    public TextMeshProUGUI TotalPrizeText_c;
    public Button Cellbutton;
    int CellPrize;

    [Header("Buy Panel")]
    public GameObject BuyPanel;
    public Button Buybutton;
    public TMP_InputField InputbuyCount;
    public TextMeshProUGUI TotalPrizetext_b;
    public TextMeshProUGUI InputBottomText;
    public TextMeshProUGUI ErrorMessage;
    int TotalPrize;
    public Slot buyslot;

    /*
    상점 아이템을 하나 누를 경우 BUYPanel 슬롯에 두고
    인풋 시스템에 작성된 갯수에 따라 
    인벤토리에 갯수만큼 구매하고 금액 차감하기

    Todo List 
    자고 일어나서 해야 될 것.
    1. buy버튼을 누르면 인벤토리에 아이템이 들어가게 할 것.
    2. buy버튼을 눌렀을 때 만약 사려던 아이템의 갯수가 현재 창고에 빈공간이 부족할 경우 구매하지 못하고 에러메세지를 남김

    3. 판매 창을 만듬
    4. 여기서 SHIFT 이동을 구현하고, 만약 파는 아이템에 등록이 되었다면 원래 있던 슬롯의 투명도를 올려 플레이어가 이 아이템을 팔고 있다는 듯한 모션을 취함
    5. 그리고 판매 슬롯에 등록하게 됨
    6. 등록 됐을 경우 판매를 눌렀을 때 총 골드가 플레이어한테 들어오게 되고 판매한 물건들은 전부 사라짐.
    여기 까지가 일단 오늘 내에 해야하는 메인메뉴 구성

    게임 씬에서 
    몬스터 에이스타 삭제 후 그냥 따라오게 만들고
    맵을 단순화하여 몬스터가 벽에 걸리는 것을 최소화하기
    타이머 UI제작하기
    스킬 추가로 더 제작하기
    스킬 ::
    부메랑 스킬 -> 부메랑이 왔다 갔다 하며 지속적으로 적에게 피해를 입힘
    스나이퍼 스킬 -> 쿨타임이 돌때마다 강력한 한방을 직선으로 꽂음
    폭격 스킬 -> 지정된 장소에 단시간동안 폭격이 떨어지는 스킬
    거인 스킬 -> 플레이어의 몸이 커져 몬스터를 밀치고 가며 데미지를 가하는 스킬 이 때 본인은 피해를 받지 않음

    아이템 박스 다양화
    아이템 다양화
    몬스터 리스폰 제구성 -> 벽 콜라이더가 있을 경우엔 스폰을 못하게 함.
    아이템 박스를 탐색 중일 때 플레이어가 밀리지 않게 조정

    스킬들 스프라이트 재 구성

    사운드 작업하기

    */



    void Start()
    {
        ShopSlots = ShopItemPanel.GetComponentsInChildren<Slot>();

        int index = 0;
        foreach (GameObject item in PlayerData.Instance.ItemPrefabs)
        {
            ShopSlots[index].InsertItem(item);
            index++;
        }

        buyslot = BuyPanel.GetComponentInChildren<Slot>();
        StorageSlots = StoragePanel.GetComponentsInChildren<Slot>();
        CellSlots = CellPanel.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(BuyPanel.activeSelf == true) {
            SelectBuy.interactable = false;
            SelectCell.interactable = true;
            BuybuttonSetEnable();
        }

        if(CellPanel.activeSelf == true) {
            SelectCell.interactable = false;
            SelectBuy.interactable = true;
            CellbuttonSetEnable();
        }

        
    }

    public void CellSelect() {
        BuyPanel.SetActive(false);
        CellPanel.SetActive(true);
        
    }

    public void BuySelect() {
        BuyPanel.SetActive(true);
        CellPanel.SetActive(false);
        
    }

    

    



    public void BuyButtonClick()
    {

        int Count = int.Parse(InputbuyCount.text);
        for (int i = 0; i < Count; i++)
        {
            GameObject buyitem = Instantiate(PlayerData.Instance.GetFindItem(buyslot.itemPrefab));
            buyitem.name = PlayerData.Instance.GetFindItem(buyslot.itemPrefab).name;
            MainInventory.Instance.AcquireItem(buyitem.GetComponent<Item>(), StorageSlots);
        }
        PlayerData.Instance.PlayerMoney -= TotalPrize;
        TotalPrize = 0;
        buyslot.itemPrefab = null;
        InputbuyCount.text = null;
        TotalPrizetext_b.text = null;
    }
    void BuybuttonSetEnable()
    {
        if (string.IsNullOrEmpty(InputbuyCount.text) == true)
        {
            TotalPrizetext_b.text = null;
            InputBottomText.text = "구매할 개수를 입력하세요.";
        }
        if (buyslot.itemPrefab != null && string.IsNullOrEmpty(InputbuyCount.text) == false)
        {
            TotalPrize = buyslot.itemPrefab.GetComponent<Item>()._itemprize * int.Parse(InputbuyCount.text);
            TotalPrizetext_b.text = $"총 골드 : {TotalPrize}";
            InputBottomText.text = $"해당 아이템을 {InputbuyCount.text}개 구매합니다.";

            if (NullStorageCount(int.Parse(InputbuyCount.text)) == false)
            {
                //빈 공간이 부족하다는 뜻
                ErrorMessage.text = "빈 공간이 부족합니다.";
                Buybutton.interactable = false;
                return;
            }
            else if (PlayerData.Instance.PlayerMoney < TotalPrize)
            {
                //돈이 부족함 
                ErrorMessage.text = "돈이 부족합니다.";
                Buybutton.interactable = false;
                return;
            }
            else {
                //빈 공간이 충분하고 돈이 충분하면 버튼 활성화
                ErrorMessage.text = null;
                Buybutton.interactable = true;
            }
        }
        else
        {
            Buybutton.interactable = false;
        }
    }

    public void CellButtonClick() {
        foreach(Slot slot in CellSlots) {
            if(slot.itemPrefab != null) {
                slot.itemPrefab = null;
                Destroy(slot.GetComponentInChildren<Item>().gameObject);
            }
        }

        PlayerData.Instance.PlayerMoney += CellPrize;
        CellPrize = 0;
    }

    void CellbuttonSetEnable() {
        CellPrize = 0;
        foreach(Slot slot in CellSlots) {
            if(slot.itemPrefab != null) {
                CellPrize += slot.itemPrefab.GetComponent<Item>()._itemprize;
            }
        }

        if(CellPrize > 0) {
            Cellbutton.interactable = true;
        } else {
            Cellbutton.interactable = false;
        }
        
        TotalPrizeText_c.text = $"총 골드 : {CellPrize}";
    }

    public bool NullStorageCount(int Count)
    {
        Slot[] slots = StoragePanel.GetComponentsInChildren<Slot>();
        int Null_Count = 0;
        foreach (Slot slot in slots)
        {
            if (slot.itemPrefab == null) Null_Count++;
        }
        return Null_Count >= Count ? true : false;
    }

   

    

}
