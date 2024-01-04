using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : SingleTon<PlayerStatManager>
{
    
    public UnityEvent LevelUp;

    void Update()
    {
        if (Player.Instance.ExpAmount >= 1) CheckPlayerLevelUp();
        if (Player.Instance.GetHp > Player.Instance.GetFullHp) MaximumHp();

    }

    void CheckPlayerLevelUp()
    {
        Player.Instance.GetLevel++;
        Player.Instance.GetMaxExp *= 1.5f;
        Player.Instance.GetCurrentExp = 0;

        LevelUp?.Invoke();
    }

    void MaximumHp()
    {
        Player.Instance.GetHp = Player.Instance.GetFullHp;
    }


}
