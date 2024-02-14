using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Button RootBoxButton;


    private void Start() {
        RootBoxButton.onClick.AddListener(ItemManager.Instance.SearchItem);
    }

}
