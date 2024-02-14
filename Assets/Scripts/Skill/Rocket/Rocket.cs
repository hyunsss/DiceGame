using System.Collections;
using UnityEngine;

public class Rocket : Skill
{

    public int ExplosionDamage;
    public float AreaDamage;

    public float RocketSpeed;
    public GameObject missilePrefab;
    public GameObject ExplosionArea;
    
    IEnumerator Start()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(2f);
        }

    }
    public override void Create()
    {
        GameObject MissileObject = Instantiate(missilePrefab);
        MissileObject.transform.SetParent(gameObject.transform);
        MissileObject.transform.SetLocalPositionAndRotation(GameManager.Instance.GetPlayerPos, Quaternion.identity);
        MissileObject.GetComponent<Missile>().Shot(RocketSpeed);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx_Dic.Rocket);

        Destroy(MissileObject, 8f);

    }

}


