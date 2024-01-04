using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IresetTable {
    void Reset();
} 

public interface IUnitMethod {
    void Death();
    void TakeDamage(float damage);
    void Move();
}

public interface IUseItem {
    void Use();
}