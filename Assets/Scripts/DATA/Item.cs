using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public Image _itemimage;
    
    private void Start() {

    }

    public virtual void Use() {}

    protected void Destroy() {
        Destroy(gameObject);
    }


}


