using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSelectButton : MonoBehaviour
{
    public GameObject SelectUI;
    public SkillType currenttype;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Desc;

    //SkillSelectUI에서 랜덤으로 스킬의 타입을 배정함

    public void Text() {
        string name = DATA.Instance.Skill_name_Dic[currenttype];
        string desc = DATA.Instance.Skill_Desc_Dic[currenttype];

        Title.text = $"{name}";
        Desc.text = $"{desc}";
    }

    //버튼을 눌러 스킬 타입을 인자로 넘겨서 UpgradeSkills에서 판별하고 스킬을 강화함.
    public void PushButton()
    {
        SkillManager.Instance.UpgradeSkill(currenttype);

        Time.timeScale = 1;
        SelectUI.gameObject.SetActive(false);
        Player.Instance.IsAttack = true;
    }
}
