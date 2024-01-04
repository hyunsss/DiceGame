using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusPanel : MonoBehaviour
{
    public TextMeshProUGUI Leveltext;
    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI Exptext;
    public TextMeshProUGUI Damagetext;
    public TextMeshProUGUI Speedtext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Leveltext.text = $"{Player.Instance.Level}";
        HPtext.text = $"{Player.Instance.GetHp} / {Player.Instance.GetFullHp}";
        Exptext.text = $"{Player.Instance.GetCurrentExp} / {Player.Instance.GetMaxExp}";
        Damagetext.text = $"{Player.Instance.GetDamage}";
        Speedtext.text = $"{Player.Instance.GetSpeed}";
    }
}
