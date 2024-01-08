using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public enum BoxType { NoneBox, EquipmentBox, SkillBox }
public enum SkillType {  None, Rocket, Rotate, Shuriken, Count }
public enum ItemType { None, Expendable, Equipment, Weapon, PassiveSkill, 
Activeskill, UpgradePart, EtcItem }


public class DATA : SingleTon<DATA>
{
    public List<Dictionary<string, object>> Testfile;


    public Dictionary<SkillType, string> Skill_name_Dic = new Dictionary<SkillType, string>();
    public Dictionary<SkillType, string> Skill_Desc_Dic = new Dictionary<SkillType, string>();
    public Dictionary<ItemType, string> Item_name_Dic = new Dictionary<ItemType, string>();
    public Dictionary<ItemType, string> Item_Desc_Dic = new Dictionary<ItemType, string>();

    #region SKill_name
    public string RotateKnife_name = "회전 낫";
    public string Rocket_name = "로켓";
    public string Shuriken_name = "수리검";
    #endregion

    #region Skill Description
    public string RotateKnife_Desc = "주변에 낫을 소환하여 적들을 공격합니다.";
    public string Rocket_Desc = "로켓을 발사하여 적에게 큰 데미지를 주고 일정영역에 지속 데미지를 입힙니다.";
    public string Shuriken_Desc = "플레이어 주변으로 수리검을 발사하여 데미지를 입힙니다.";
    #endregion

    #region Item_name
    public string Potion_name = "포션"; 

    #endregion

    #region Item Description
    public string Potion_Desc = "체력을 20%만큼 회복 시킵니다.";
    #endregion

    protected override void Awake() {
        base.Awake();
        SkillStringInit();
        Testfile = CSVReader.Read("itemCSV"); 
    }

    private void Start() {
        ItemStringInit();
    }

    public void SkillStringInit()
    {
        Skill_name_Dic.Add(SkillType.Rotate, RotateKnife_name);
        Skill_name_Dic.Add(SkillType.Rocket, Rocket_name);
        Skill_name_Dic.Add(SkillType.Shuriken, Shuriken_name);

        Skill_Desc_Dic.Add(SkillType.Rotate, RotateKnife_Desc);
        Skill_Desc_Dic.Add(SkillType.Rocket, Rocket_Desc);
        Skill_Desc_Dic.Add(SkillType.Shuriken, Shuriken_Desc);
    }

    void ItemStringInit() {
        foreach(var item in ItemManager.Instance.ItemList) {
            foreach(var data in Testfile) {
                if(data["ID"].ToString() == item.gameObject.name) {
                    item._itemname = data["Name"].ToString();
                    item._itemdesc = data["DESC"].ToString();
                    item.type = (ItemType)(int)data["TYPE"];
                    item._itemprize = (int)data["PRIZE"];
                    string ImagePath = data["ImagePath"].ToString();
                    item._itemimage.sprite = Resources.Load<Sprite>($"ItemTexture/{ImagePath}");
                    break;
                }
            }
        }
    }

}
