using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    public Transform ItemListObject;

    //드랍율을 할당하면 드랍율에 따라 아이템을 드랍해주는 시스템
    /*
    Random.Range(0, Max ==> 100) 100을 최대로 두고 float 매개변수를 하나를 받는다
    float는 나올 확률을 결정함. 
    float 값 보다 아래일 경우 drop true
    아니면 drop false
    */
    public bool RandomSystem(int percent) {
        int index = Random.Range(0, 100);

        if(index >= percent) return true;
        else return false;
    }



}
