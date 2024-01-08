using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public enum EquipType { Weapon, Shield, Body, Helmat }

    public EquipType equipType;
    public Sprite Player_sprite;


    [Header("Item Stat")] 
    public float Item_Damage;
    public float Item_Speed;
    public float Item_FullHp;
    public float Item_Hp;
    public float Item_Hp_vampire;
    public float Item_ArmorStat;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
