using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SingleTon<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour 
{
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        Instance = this as T;
    }
        
    
}
