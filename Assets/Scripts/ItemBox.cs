using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public BoxType boxType;

    private GameObject[] currentItemList;
    private Slot[] RootBoxslots;


    private bool CheckItem;
    public bool InPlayer { get { return CheckItem; } set { CheckItem = value; } }

    private void Awake()
    {
        RootBoxslots = InventoryManager.Instance.RootBoxPanel.GetComponentsInChildren<Slot>();
        currentItemList = new GameObject[RootBoxslots.Length];
    }

    void Start()
    {
        SetItemList();
    }

    void SetItemList()
    {
        int item_Count = Random.Range(1, 4);

        List<GameObject> itemList = ItemManager.Instance.ItemList_dic[boxType];
        for (int i = 0; i < item_Count; i++)
        {
            int itemList_index = Random.Range(0, itemList.Count);
            GameObject Item = Instantiate(itemList[itemList_index]);
            Item.name = itemList[itemList_index].name;
            Item.GetComponent<Item>().CheckItemTrue = false;
            Item.GetComponent<Item>()._itemimage_Component.sprite = ItemManager.Instance.UnknownSprite;
            Item.GetComponent<Item>()._itemimage_Component.raycastTarget = false;
            Item.GetComponent<RectTransform>().SetParent(transform);
            Item.SetActive(false);
            currentItemList[i] = Item;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {

            for (int i = 0; i < currentItemList.Length; i++)
            {
                RootBoxslots[i].itemPrefab = currentItemList[i]?.gameObject;



                if (currentItemList[i] != null)
                {
                    currentItemList[i].transform.SetParent(RootBoxslots[i].transform);
                    currentItemList[i].GetComponent<RectTransform>().anchoredPosition = RootBoxslots[i].GetComponent<RectTransform>().position;
                    currentItemList[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    currentItemList[i].SetActive(true);

                    if (currentItemList[i].GetComponent<Item>().CheckItemTrue == false)
                    {
                        ItemManager.Instance.RoadItemStack.Add(currentItemList[i]);
                        Debug.Log("enqueue item + " + currentItemList[i].name);
                    }
                }

            }
            //ItemManager.Instance.RoadItemStack.Reverse();
            player.InItemBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            //루트 박스에 있던 오브젝트 전부 해당 오브젝트 자식으로 몰아 넣기
            //오브젝트 자식에 있는 오브젝트들로 currentList 갱신

            for (int i = 0; i < RootBoxslots.Length; i++)
            {
                currentItemList[i] = RootBoxslots[i]?.itemPrefab;
            }

            for (int i = 0; i < currentItemList.Length; i++)
            {
                if (currentItemList[i] != null)
                {
                    currentItemList[i].transform.SetParent(transform);
                }
            }

            ItemManager.Instance.RoadItemStack.Clear();
            ItemManager.Instance.isRunning_SearchItemCoroutine = false;
            player.InItemBox = false;
        }
    }



}



