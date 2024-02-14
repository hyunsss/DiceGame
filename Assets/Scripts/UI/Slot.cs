using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Image = UnityEngine.UI.Image;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemStatusobj;
    public ItemType Slotitemtype;

    public GameObject itemPrefab;
    public Image Item_Image;

    private void Awake()
    {
        ItemStatusobj = GameObject.FindWithTag("ItemStatus");
    }

    public void DeleteItem()
    {
        if (itemPrefab != null)
        {
            // GetComponentInChildren<Item>().Des;
        }
    }

    public void InsertItem(GameObject itemprefab) {
        if(transform.GetComponentInChildren<Item>() == false) {
            GameObject item = Instantiate(itemprefab);
            item.transform.SetParent(transform);
            item.name = itemprefab.name;
            item.GetComponent<Collider2D>().enabled = false;
            Debug.Log(item.GetComponent<Collider2D>().enabled);
            item.transform.localScale = new Vector3(1, 1, 1);
            itemPrefab = item;
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (itemPrefab != null) return;
        //드랍된 곳이 샵 아이템인 경우 리턴 
        if (Slotitemtype == ItemType.ShopItem) return;

        GameObject dropped = eventData.pointerDrag;

        if(dropped.transform.parent.GetComponent<Slot>() != null && dropped.transform.parent.GetComponent<Slot>().Slotitemtype == ItemType.ShopItem) {
            return;
        }

        if (dropped.GetComponent<Item>() == true)
        {
            DragController dragController = dropped.GetComponent<DragController>();
            ItemType DropItemtype = dragController.gameObject.GetComponent<Item>().type;

            if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.Weapon)
            {
                // 아이템 타입이 Weapon일 때
                dragController.parentAfterDrag = transform;
                Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                ItemManager.Instance.ChangeEquipItem(equipitem);

            }
            else if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.Equipment)
            {
                // 아이템 타입이 equipment일 때
                dragController.parentAfterDrag = transform;
                Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                ItemManager.Instance.ChangeEquipItem(equipitem);

            }
            else if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.PassiveSkill)
            {
                // 아이템 타입이 PassiveSkill일 때
                dragController.parentAfterDrag = transform;
            }
            else if (DropItemtype == Slotitemtype && Slotitemtype == ItemType.Activeskill)
            {
                // 아이템 타입이 PassiveSkill일 때
                dragController.parentAfterDrag = transform;
            }

            else if (Slotitemtype == ItemType.None)
            {

                //드래그 하기 전 슬롯이 equipment였다면 여기 로직은 장착을 해제하는 부분이므로 조건체크를 한 뒤 장비를 해체하는 작업을 함.
                if (dragController.parentAfterDrag.GetComponent<Slot>().Slotitemtype == ItemType.Weapon)
                {
                    Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                    ItemManager.Instance.UnEquipSprite(equipitem);
                }
                else if (dragController.parentAfterDrag.GetComponent<Slot>().Slotitemtype == ItemType.Equipment)
                {
                    Equipment equipitem = dragController.gameObject.GetComponent<Equipment>();
                    ItemManager.Instance.UnEquipSprite(equipitem);
                }
                dragController.parentAfterDrag = transform;
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemPrefab != null && itemPrefab.GetComponent<Item>().CheckItemTrue == true)
        {
            ItemStatusobj.SetActive(true);
            string name = itemPrefab.GetComponent<Item>()._itemname;
            string Desc = itemPrefab.GetComponent<Item>()._itemdesc;
            string Prize = itemPrefab.GetComponent<Item>()._itemprize.ToString();
            ItemStatusobj.GetComponent<ItemStatus>().SetItemStatus(name, Desc, Prize);

            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemStatusobj.GetComponent<RectTransform>().position = new Vector3(MousePos.x - 0.7f, MousePos.y - 0.5f, 0);

            if(InventoryManager.Instance != null)  InventoryManager.Instance.MouseEnterTarget = itemPrefab;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemStatusobj.GetComponent<RectTransform>().position = new Vector3(99f,99f,99f);
    }

}
