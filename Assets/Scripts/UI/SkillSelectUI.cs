using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    public GameObject SelectUI;
    public GameObject[] SelectPanels;
    /*
    플레이어가 가지고 있는 스킬목록들을 가져와 랜덤적으로 UI에 표시한다.
    1.3가지를 랜덤으로 추출하여 화면에 보여지게 한다.
        1-1. 3가지를 랜덤으로 추출하되, 추출한 스킬들 중 중복이 있으면 안된다.
        1-2. 추출하고자 하는 스킬이 레벨이 만렙일 경우 제외한다.
            1-2-1. 기본 스텟을 강화하는 리스트를 만들어 가지고 있는 스킬이 모두 만렙일 때 기본 체력, 공격력, 이동속도 등을 올릴 수 있도록 하자.
        1-3. 스킬에 맞는 이름과 설명을 붙이는 방법 
    
    
    */


    //랜덤 배정된 스킬을 출력
    //업그레이트 창에서 총 3개의 스킬을 랜덤으로 뽑아옴
    public void OnUpgradeSkillEnable()
    {
        List<Skill> NotMaxLevelSkills = SkillManager.Instance.IsSkillLevelMax(Player.Instance.MySkill);

        int RandomCount = NotMaxLevelSkills.Count >= 3 ? 3 : NotMaxLevelSkills.Count; 

        int[] RandomIndex = GameManager.Instance.GetRandomIndexesFromList(NotMaxLevelSkills, RandomCount);

        for(int i = 0; i < RandomIndex.Length; i++) {
            SelectPanels[i].GetComponent<SkillSelectButton>().currenttype = Player.Instance.MySkill[RandomIndex[i]].GetComponent<Skill>().type;
            SelectPanels[i].GetComponent<SkillSelectButton>().Text();
        }

        
        SelectUI.SetActive(true);
        Player.Instance.IsAttack = false;
        Time.timeScale = 0;
    }

}
