using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : SingleTon<MainMenuManager>
{
    private List<GameObject> UIList = new List<GameObject>();
    public GameObject Inventory;
    public GameObject Storage;
    public GameObject Shop;
    public GameObject Setting;

    public GameObject MainMenu;

    protected override void Awake() {
        base.Awake();
        
        UIList.Add(Inventory);
        UIList.Add(Storage);
        UIList.Add(Shop);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            bool UIActive = false;
            foreach (GameObject ui in UIList) {
                if(ui.activeSelf == true) {
                    UIActive = true;
                    ui.SetActive(false);
                }
            }

            if(UIActive == true) {
                MainMenu.SetActive(true);
            }

            if(UIActive == false) {
                //게임 종료 UI 띄우기
            }

        }
    }

    private void Start() {
        PlayerData.Instance.MainMenuSceneLoadData();
    }

    public void OnClickGameStartTab() {

        PlayerData.Instance.MainMenuSceneSaveData();
        SceneManager.LoadScene("GamePlayScene");
    }

    public void OnClickInventoryTab() {
        Inventory.SetActive(true);
        Storage.SetActive(true);
    }

    public void OnClickShopTab() {
        Storage.SetActive(true);
        Shop.SetActive(true);
    }

    public void SettingTab() {
        //설정 창 켜기
    }

    
}
