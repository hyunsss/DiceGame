using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : Skill
{
    public int Damage;
    public float Speed;

    public GameObject ShurikenPrefab;
    public Transform shotPoints;
    //public Transform[] shotPoints;
    
    private IEnumerator Start()
    {
    
        while (true)
        {
            Create();
            yield return new WaitForSeconds(2f);
        }
    }

    public override void Create()
    {
        foreach (Transform shotPoint in shotPoints) //boltPrefab ������ �߻�
        {
            GameObject ShurikenPrefab = Instantiate(this.ShurikenPrefab);
            ShurikenPrefab.transform.SetParent(gameObject.transform);
            ShurikenPrefab.transform.SetPositionAndRotation(GameManager.Instance.GetPlayerPos, shotPoint.rotation);
            ShurikenPrefab.GetComponent<Knifes>().ShurikenShot(Speed);

            Destroy(ShurikenPrefab, 3f);
        }
    }

}
