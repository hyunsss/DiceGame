using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ItemManager : SingleTon<ItemManager>
{
    public enum Operation { Add, Subract }

    public Transform ItemListObject;
    public List<Item> ItemList = new List<Item>();

    public SpriteRenderer Player_LeftArm;
    public SpriteRenderer Player_RightArm;
    public SpriteRenderer Player_Body;
    public SpriteRenderer Player_Helmat;

    private float Variance_Damage;
    private float Variance_Speed;
    private float Variance_FullHp;
    private float Variance_Hp;
    private float Variance_Hp_vampire;
    private float Variance_ArmorStat;

    public float GetVariance_Damage { get { return Variance_Damage;} }
    public float GetVariance_Speed { get { return Variance_Speed;} }
    public float GetVariance_FullHp { get { return Variance_FullHp;} }
    public float GetVariance_Hp { get { return Variance_Hp;} }
    public float GetVariance_Hp_vampire { get { return Variance_Hp_vampire;} }
    public float GetVariance_ArmorStat { get { return Variance_ArmorStat;} }

    void UpdateStatManagement(Equipment equipment, Operation operation) {
        if(operation == Operation.Add) {
            Variance_Damage += equipment.Item_Damage;
            Variance_Speed += equipment.Item_Speed;
            Variance_FullHp += equipment.Item_FullHp;
            Variance_Hp += equipment.Item_Hp;
            Variance_Hp_vampire += equipment.Item_Hp_vampire;
            Variance_ArmorStat += equipment.Item_ArmorStat;
        } else if(operation == Operation.Subract) {
            Variance_Damage -= equipment.Item_Damage;
            Variance_Speed -= equipment.Item_Speed;
            Variance_FullHp -= equipment.Item_FullHp;
            Variance_Hp -= equipment.Item_Hp;
            Variance_Hp_vampire -= equipment.Item_Hp_vampire;
            Variance_ArmorStat -= equipment.Item_ArmorStat;
        }
    }

    

    /*
    장착 아이템에 Equipment, Weapon의 무기가 들어갈 경우 화면으로 보이는 플레이어의 스프라이트를 변경하도록 함.
    */
    public void ChangeEquipItem(Equipment item) {
        //equipment item을 받고 EquipType을 만들어서 타입에 맞는 SpriteRenderer에 적용 시키자. 
        if( item.equipType == Equipment.EquipType.Weapon ) {
            Player_LeftArm.sprite = item.Player_sprite;
        } else if (item.equipType == Equipment.EquipType.Shield) {
            Player_RightArm.sprite = item.Player_sprite;
        } else if (item.equipType == Equipment.EquipType.Body) {
            Player_Body.sprite = item.Player_sprite;
        }else if (item.equipType == Equipment.EquipType.Helmat) {
            Player_Helmat.sprite = item.Player_sprite;
        }

        UpdateStatManagement(item, Operation.Add);
    }

    public void UnEquipSprite(Equipment item) {
        if( item.equipType == Equipment.EquipType.Weapon ) {
            Player_LeftArm.sprite = null;
        } else if (item.equipType == Equipment.EquipType.Shield) {
            Player_RightArm.sprite = null;
        } else if (item.equipType == Equipment.EquipType.Body) {
            Player_Body.sprite = null;
        }else if (item.equipType == Equipment.EquipType.Helmat) {
            Player_Helmat.sprite = null;
        }

        UpdateStatManagement(item, Operation.Subract);
    }


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
