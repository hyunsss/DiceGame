using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    
    public float RecoveryHp;

    private void Awake() {
        
    }

    public override void Use()
    {
        Player.Instance.GetHp += RecoveryHp;
   
    }

    

}
