using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatus : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemname;
    [SerializeField] TextMeshProUGUI itemdesc;
    [SerializeField] TextMeshProUGUI itemprize;
    

    public void SetItemStatus(string name, string desc, string prize) {
        this.itemname.text = name;
        this.itemdesc.text = desc;
        this.itemprize.text = prize;
    }
}
