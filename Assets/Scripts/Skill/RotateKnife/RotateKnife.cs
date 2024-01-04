using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateKnife : Skill
{
    public int KnifeCount;
    public float Speed;
    public int Damage;

    public GameObject KnifePrefab;
    private List<GameObject> Knifes = new List<GameObject>();

    private void Start()
    {


        gameObject.transform.Rotate(0, 0, 0);
        Create();
        SetKnifes();
        StartCoroutine(PlayRotateKnifes());
    }

    public override void Create()
    {
        for (int i = 0; i < KnifeCount; i++)
        {
            GameObject Knife = Instantiate(KnifePrefab);
            Knife.transform.SetParent(gameObject.transform);
            Knifes.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = Player.Instance.transform.position;
    }

    private void SetKnifes()
    {
        float RotateZ = 360 / Knifes.Count;
        float Temp = RotateZ;

        if (Knifes != null)
        {
            Knifes[0].transform.Rotate(0, 0, 0);

            for (int i = 1; i < Knifes.Count; i++)
            {
                Knifes[i].transform.Rotate(0, 0, Temp);
                Temp += RotateZ;
            }
        }
    }

    IEnumerator PlayRotateKnifes()
    {
        float cumulativeAngle = 0f; // 누적 각도

        while (true)
        {
            float rotationThisFrame = Time.deltaTime * Speed * 360; // 이번 프레임에서의 회전 각도
            cumulativeAngle += rotationThisFrame; // 각도 누적
            cumulativeAngle %= 360; // 360도를 넘어가면 0으로 초기화

            gameObject.transform.rotation = Quaternion.Euler(0, 0, cumulativeAngle);

            yield return null;
        }
    }
}
