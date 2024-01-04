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

        if (GetComponentInChildren<Item>() == null && item != null)
        {
            GameObject itemObject = Instantiate(item.gameObject);
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

        if(dragController.gameObject.GetComponent<Item>().type == Slotitemtype) {
            dragController.parentAfterDrag = transform;
        } else if(Slotitemtype == ItemType.None) {
            dragController.parentAfterDrag = transform;
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemStatusobj.SetActive(true);
            DATA.Instance.Item_name_Dic.TryGetValue(item.type, out string name);
            DATA.Instance.Item_Desc_Dic.TryGetValue(item.type, out string Desc);
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
