using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Slot[] Slots;


    private void Awake() {
        Slots = GetComponentsInChildren<Slot>();
    }

    private void Start() {
        foreach (Slot slot in Slots)
        {
            //slot.RendererItem();
        }
    }
}
