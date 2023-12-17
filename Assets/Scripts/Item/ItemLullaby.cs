using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLullaby : MonoBehaviour
{
    [SerializeField]
    private float objectDuration = 20.0f;                           //아이템 유효시간
    [SerializeField]
    private float lullabyDuration = 10.0f;                          //몬스터 이동제약시간(느려짐, 멈춤)
    
    private bool isObtained = false;                                //slot에 들어갔었는지 여부
    public bool isInstall = false;                                //아이템이 바닥에 설치됐는지의 여부
    private bool isWaitingToDestroy = false;                        //아이템이 삭제 대기 중인지의 여부
    
    private Rigidbody rigid;

    private ItemDetectMonster itemDetect = null;
    private Vector3 pos;

    private HoverItem2 hoverItem;

    private void Awake() 
    {
        rigid = GetComponent<Rigidbody>();
        hoverItem = GetComponent<HoverItem2>();
        if (rigid == null)
        {
            Debug.Assert(false, "Error (RigidBody is Null) : 해당 객체에 RigidBody가 존재하지 않습니다.");
            return;
        }

        Transform child = transform.GetChild(0);                   
        if (child == null) {
            Debug.Assert(false, "Error (There is no child) : 해당 객체에 child가 존재하지 않습니다.");
            return;
        }
        itemDetect = child.GetComponent<ItemDetectMonster>();

        if (itemDetect == null)
        {
            Debug.Assert(false, "Error (There is no Script) : 해당 객체에 해당 스크립트가 존재하지 않습니다.");
            return;
        }
    }

    private void Start() {
        pos = transform.position;
    }

    private void Update() 
    {
        if(hoverItem.itemRotation == false && transform.root.name.Equals("OVRPlayerController") == false)
        {
            isInstall = true;
        }
        if (isInstall)                                      //아이템이 설치된 상태일 때
        {
            transform.position = pos;                       
            if (!isWaitingToDestroy)                        //삭제 대기 중이 아니면
            {
                Debug.Log("자장가");
                itemDetect.SetLullabyDuration(lullabyDuration);     //Monster이동제약시간 넘김
                itemDetect.SetIsActive(true);                       //Monster감지 활성화
                StartCoroutine(DestroyLullaby());           //아이템 삭제 타이머 시작
                isWaitingToDestroy = true;                  //삭제 대기 중으로 변경
                rigid.isKinematic = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)      //아이템을 던져서 설치한다고 가정했을 때
    {      
        if (collision.gameObject.CompareTag("Floor"))       //바닥과 충돌하고
        {      
            if (!isInstall)                                 //아이템이 설치된 상태가 아닐 때
            {       
                pos = transform.position;
                isInstall = true;                           //설치됨으로 상태 변경
            }
        }
    }

    private IEnumerator DestroyLullaby()
    {
        yield return new WaitForSeconds(objectDuration);
        Destroy(gameObject);                                //일정시간 이후 아이템 삭제
    }

    public void SetIsObtain(bool value)
    {
        isObtained = value;
    }
}
