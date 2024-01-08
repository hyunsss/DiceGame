using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Image = UnityEngine.UI.Image;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemStatusobj;
    public ItemType Slotitemtype;

    public Item item;
    public Image Item_Image;

    private void Awake()
    {
        ItemStatusobj = GameObject.FindWithTag("ItemStatus");


    }

    private void Start() {
        
        if (GetComponentInChildren<Item>() == null && item != null)
        {
            GameObject itemObject = Instantiate(item.gameObject);
            itemObject.name = item.gameObject.name;
            itemObject.transform.SetParent(this.gameObject.transform);
            itemObject.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().position;
            itemObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    public void AddItem(Item item)
    {
        this.item = item;
        item.transform.SetParent(this.transform);
    }

    public void DeleteItem()
    {
        this.item = null;
        Item_Image = null;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (item != null) return;
        
        GameObject dropped = eventData.pointerDrag;
        DragController dragController = dropped.GetComponent<DragController>();
        ItemType DropItemtype = dragController.gameObject.GetComponent<Item>().type;

        if(DropItemtype == Slotitemtype && Slotitemtype == ItemType.Weapon) {
            // 아이템 타입이 Weapon일 때
            dragController.parentAfterDrag = transform;
            Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
            ItemManager.Instance.ChangeEquipItem(equipitem);

        } else if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.Equipment) {
            // 아이템 타입이 equipment일 때
            dragController.parentAfterDrag = transform;
            Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
            ItemManager.Instance.ChangeEquipItem(equipitem);

        } else if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.PassiveSkill) {
            // 아이템 타입이 PassiveSkill일 때
            dragController.parentAfterDrag = transform;
        }

        else if(Slotitemtype == ItemType.None) {

            //드래그 하기 전 슬롯이 equipment였다면 여기 로직은 장착을 해제하는 부분이므로 조건체크를 한 뒤 장비를 해체하는 작업을 함.
            if(dragController.parentAfterDrag.GetComponent<Slot>().Slotitemtype == ItemType.Weapon) {
                Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                ItemManager.Instance.UnEquipSprite(equipitem);
            } else if(dragController.parentAfterDrag.GetComponent<Slot>().Slotitemtype == ItemType.Equipment) {
                Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                ItemManager.Instance.UnEquipSprite(equipitem);
            }


            dragController.parentAfterDrag = transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemStatusobj.SetActive(true);
            string name = item._itemname;
            string Desc = item._itemdesc;
            ItemStatusobj.GetComponent<ItemStatus>().SetItemStatus(name, Desc);

            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemStatusobj.GetComponent<RectTransform>().position = new Vector3(MousePos.x - 0.7f, MousePos.y - 0.5f, 0);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemStatusobj.SetActive(false);
    }

}
