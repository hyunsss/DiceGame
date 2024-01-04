using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    
    public float RecoveryHp;

    private void Awake() {
        type = ItemType.Expendable;
    }

    public override void Use()
    {
        Player.Instance.GetHp += RecoveryHp;
        Destroy();
    }

    

}
