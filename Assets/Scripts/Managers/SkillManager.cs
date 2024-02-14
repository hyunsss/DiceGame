using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Todo List   

SkillManagerScript 쳬계화
소스코드에서 냄새남


*/

public class SkillManager : SingleTon<SkillManager>
{
    public Transform SkillParent;
    public List<GameObject> CloneSkillObjects;

    private GameObject PrevObject;

    public List<Skill> SkillList = new List<Skill>();
    Dictionary<SkillType, List<Skill>> Skill_Dic = new Dictionary<SkillType, List<Skill>>();


    //모든 스킬을 타입으로 구분하여 딕셔너리에 저장.
    void AddSkillsDictionary()
    {
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            Skill_Dic.Add(type, new List<Skill>());
        }

        foreach (Skill skill in SkillList)
        {
            Skill_Dic[skill.type].Add(skill);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddSkillsDictionary();
    }

    public void GameSceneStartLoadSkill()
    {
        foreach (Slot slot in InventoryManager.Instance.skillSlots)
        {
            if (slot.GetComponentInChildren<SkillItem>() == null) continue;
            foreach (Skill skill in SkillList)
            {
                if (skill.name == slot.GetComponentInChildren<SkillItem>().SkillPrefab.name)
                {
                    GameObject SkillObject = slot.GetComponentInChildren<SkillItem>().SkillPrefab;
                    EnableMySkill(SkillObject);
                    break;
                }
            }
        }

        GetChildSkills();
    }

    //하이어라키에 Skill 게임 오브젝트의 클론된 스킬들을 모두 가져와 리스트에 저장
    void GetChildSkills()
    {
        CloneSkillObjects.Clear();
        for (int i = 0; i < SkillParent.childCount; i++)
        {
            CloneSkillObjects.Add(SkillParent.GetChild(i).gameObject);
        }
    }

    //start 구문에서 플레이어의 스킬리스트를 게임 시작당시 모두 가져와 시작함. 
    //매개변수로 받은 스킬을 Skill 게임 오브젝트에 추가하는 역할
    void EnableMySkill(GameObject skill)
    {
        GameObject PlayerSkill = Instantiate(skill);
        PlayerSkill.transform.parent = SkillParent;
        Player.Instance.MySkill.Add(skill);
        PlayerSkill.SetActive(true);
    }

    // 매개변수로 받은 스킬을 플레이어 스킬목록에서 제기하고 게임오브젝트를 파괴시킴 ==> 스킬 업그레이드 로직 
    void UnEnableMySkill(GameObject RemoveSkill)
    {
        foreach (GameObject obj in Player.Instance.MySkill)
        {
            if (obj.GetComponent<Skill>().type == RemoveSkill.GetComponent<Skill>().type)
            {
                Player.Instance.MySkill.Remove(obj);
                break;
            }
        }
        RemoveSkill.transform.SetParent(transform.root);
        RemoveSkill.GetComponent<Skill>().DestroyObject();

    }

    //스킬 업그레이드
    public void UpgradeSkill(SkillType type)
    {
        foreach (GameObject skillObject in CloneSkillObjects)
        {
            if (skillObject.GetComponent<Skill>().type == type)
            {
                PrevObject = skillObject;
            }
        }

        foreach (Skill skill in Skill_Dic[type])
        {
            if (PrevObject.GetComponent<Skill>().level + 1 == skill.level)
            {
                UnEnableMySkill(PrevObject);
                EnableMySkill(skill.gameObject);
                GetChildSkills();

                return;
            }
        }

    }

    //매개변수로 받은 게임오브젝트의 리스트 스킬들이 레벨이 최대인지 체크함.
    public List<Skill> IsSkillLevelMax(List<Skill> currentSkill)
    {
        List<Skill> NotMaxLevelSkills = new List<Skill>();

        foreach (Skill skill in currentSkill)
        {
            SkillType type = skill.GetComponent<Skill>().type;

            int Count = Skill_Dic[type].Count;

            if (Skill_Dic[type].IndexOf(skill.GetComponent<Skill>()) != Count - 1)
            {
                NotMaxLevelSkills.Add(skill);
            }
        }
        return NotMaxLevelSkills;
    }

    public List<Skill> IsSkillLevelMax(List<GameObject> currentSkill_obj)
    {

        List<Skill> currentSkill = new List<Skill>();
        foreach (GameObject obj in currentSkill_obj)
        {
            currentSkill.Add(obj.GetComponent<Skill>());
        }

        List<Skill> NotMaxLevelSkills = new List<Skill>();

        foreach (Skill skill in currentSkill)
        {
            SkillType type = skill.GetComponent<Skill>().type;

            int Count = Skill_Dic[type].Count;

            if (Skill_Dic[type].IndexOf(skill.GetComponent<Skill>()) != Count - 1)
            {
                NotMaxLevelSkills.Add(skill);
            }
        }
        return NotMaxLevelSkills;
    }

}
