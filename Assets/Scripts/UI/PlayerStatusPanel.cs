using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(SceneManager.GetActiveScene().name == "GamePlayScene") {
            Leveltext.text = $"{Player.Instance.Level}";
            HPtext.text = $"{Math.Ceiling(Player.Instance.GetHp)} / {Player.Instance.GetFullHp} +({ItemManager.Instance.GetVariance_FullHp})";
            Exptext.text = $"{Math.Ceiling(Player.Instance.GetCurrentExp)} / {Player.Instance.GetMaxExp}";
            Damagetext.text = $"{Player.Instance.GetDamage}";
            Speedtext.text = $"{Player.Instance.GetSpeed}";
        }
    }
}
