using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePoint : MonoBehaviour
{

    public string Pointname;

    private float currentTime;
    private float StartTime;

    public GameObject EscapeUIobject;
    public TextMeshProUGUI text;

    bool AreaInPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            currentTime = 8f;
            AreaInPlayer = true;
        }
    }

    private void Start()
    {
        EscapeUIobject = TileMapManager.Instance.EscapeUI;
        text = TileMapManager.Instance.EscapeText;
    }

    private void Update()
    {
        if (TimeManager.Instance.IsDayTime()) {
            if (AreaInPlayer == true)
            {
                EscapeUIobject.SetActive(true);

                currentTime -= Time.deltaTime;
                string str = currentTime.ToString("F2");
                text.text = $"탈출까지 : {str}";

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    GetComponent<Collider2D>().enabled = false;
                    GameManager.Instance.NextMainMenuScene();
                }
            }
        } else {
            currentTime = 0;
            EscapeUIobject.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (SceneManager.GetActiveScene().name == "GamePlayScene")
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                AreaInPlayer = false;
                EscapeUIobject.SetActive(false);
                currentTime = 8f;
            }
        }
    }
}
