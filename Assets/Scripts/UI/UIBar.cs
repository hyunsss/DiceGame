using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    #region HpBar
    public Image HpBar;
    public TextMeshProUGUI HpTextMesh;
    #endregion

    #region ExpBar
    public Image ExpBar;
    public TextMeshProUGUI ExpTextMesh;
    #endregion

    public TextMeshProUGUI LevelUpText;
    void Update()
    {
        if (HpBar != null && ExpBar != null)
        {
            HpBar.fillAmount = Player.Instance.HPAmount;
            HpTextMesh.text = $"{(int)(Player.Instance.HPAmount * 100)}%";

            ExpBar.fillAmount = Player.Instance.ExpAmount;
            ExpTextMesh.text = $"{(int)(Player.Instance.ExpAmount * 100)}%";

            LevelUpText.text = $"{Player.Instance.GetLevel}Lv.";
        }

        

    }


}
