using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Item : MonoBehaviour
{


    /*
        Todo List : 
        몬스터 드랍 아이템
        드랍아이템 상호작용시 인벤토리에 아이템 추가

        아이템박스를 만들고 박스를 클릭하면 UI 오픈
        박스 안에 랜덤적으로 아이템을 생성

    */
    public ItemType type;
    public string _itemname;
    public string _itemdesc;
    public int _itemprize;
    public Sprite itemSprite;
    [HideInInspector] public bool CheckItemTrue;

    public Image _itemimage_Component;
    public SpriteRenderer spriteRenderer_Component;
    
    [HideInInspector] public float Cumulative_Time;

    private void Awake() {
        CheckItemTrue = true;
        _itemimage_Component.sprite = itemSprite;
        spriteRenderer_Component.sprite = itemSprite;
    }

    private void Start() {
    }

    public virtual void Use() {}

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent(out Player player)) {
            InventoryManager.Instance.InTriggerItemObject.Add(this);
        } 
            
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.TryGetComponent(out Player player)) {
            InventoryManager.Instance.InTriggerItemObject.Remove(this);
        } 
    }

}


