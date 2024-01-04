using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public SkillType type;
	public int level;
    
    public abstract void Create();

    public void DestroyObject() {
        Destroy(this.gameObject, 0f);
    }
}
