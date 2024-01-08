using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;
    private RectTransform rect;
    [HideInInspector] public Transform parentAfterDrag;

    /*
    Todo List 
    1. Drop 하는 곳에 아이템이 이미 있을 경우 원래자리로 되돌리기                   clear
    2. Slot마다 ItemType을 사용하여 타입이 맞지 않으면 슬롯에 장착 불가능           clear
    3. 장비 슬롯이나 패시브 슬롯에 들어간 아이템들은 플레이어 스테이터스에 활성화 
    4. 쉬프트 클릭을 할 경우 인벤토리 빈 슬롯에 들어가 되, 장비, 무기들은 각 칸이 비어 있다면 우선순위로 설정되어 바로 장착 할 수 있도록 설정 --> shift 이동 성공
    5. 플레이어 스테이터스 창 구현하기  
        추가되는 스텟들은 +() 이런식으로 띄울 수 있도록 하기
    6. DATA관리 방법 생각하기 .txt로 보관하는 방식 알아 볼 것. csvReader를 활용한 데이터 읽어오기 
    7. 시계 오브젝트를 개발하고 낮과 밤이 되면 몬스터의 로직이 변경 되도록 수정하기. 
    8. 아이템, 장비, 무기, 패시브 등등 스킬들 다양화 시켜주고 등록하기.
    9. 드랍 아이템 생성 아이템과 상호작용 시 아이템을 먹음
    10. 맵 프리팹 만들고 랜덤 맵 구현하기 
    11. 아이템 등급 enum을 설계 하고 등급에 따른 값어치 및 능력치 변경하기 
    12. 아이템 장착 타입에 따른 능력치 적용 status에 추가하기
    13. GUI 팩을 이용해 디자인 가꾸기
    14. 몬스터 프리팹을 활용하여 몬스터 추가하기
    게임 플레이 Scene End
    ==========================================================
    메인 Scene 
    1. 상점
    2. UI 구성
    
    */


    private void Awake() {
        rect = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        parentAfterDrag.GetComponent<Slot>().item = null;
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {   
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        parentAfterDrag.GetComponent<Slot>().ItemStatusobj.SetActive(false);
        rect.position = new Vector3(MousePos.x, MousePos.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        parentAfterDrag.GetComponent<Slot>().item = this.gameObject.GetComponent<Item>();
        transform.SetParent(parentAfterDrag);

        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Input.GetKey(KeyCode.LeftShift)) {
            InventoryManager.Instance.ShiftMoveItem(gameObject.GetComponent<Item>());
        }
    }
}
