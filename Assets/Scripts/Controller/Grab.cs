using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grab : MonoBehaviour
{
    private bool isGrabbing = false;
    public Transform leftGrabPositon;
    public Transform rightGrabPosition;
    public GameObject itemData;
    public ItemSlot itemSlot;
    private int weaponLevel;
    private string weaponName;
    private GameObject Grabbable;
    private LineRenderer lineRenderer;
    private RaycastHit hit;
    public AudioClip[] clip;
    private AudioSource audioSource;
    private void Awake()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(GameManager.gameManager.GetCurrentWave() > 0)
        {
            ChangeWeapon(GameManager.gameManager.weaponName);
        }
        
    }

    void FixedUpdate()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))       //버튼을 눌렀을 때
        {
            isGrabbing = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.A) )        //A 버튼을 누르면 놓는다
        {
            isGrabbing = false;
            if (!isGrabbing && rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == true && GameManager.gameManager.GetWeaponSwapEnabled() == true)        //만약 놓는다면 모든 무기를 비활성 한다.
            {
                weaponLevel = GameManager.gameManager.weaponLevel;
                weaponName = GameManager.gameManager.weaponName;
                rightGrabPosition.transform.Find("WP_Bundle").transform.Find(weaponName).transform.Find(weaponName + weaponLevel).gameObject.SetActive(false);
                rightGrabPosition.transform.Find("WP_Bundle").gameObject.SetActive(false);
            }
        }

        

        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) || OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            ItemGrab();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            lineRenderer.enabled = false;
        }
        else if(!OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            if(lineRenderer.enabled == true)
            {
                lineRenderer.enabled = false;
            }
        }

        if(OVRInput.GetUp(OVRInput.RawButton.B))
        {
            if (this.name == "RightControllerGrabposition")                                         // this가 오른쪽 컨트롤러가 되면 PickUp함수가 오른쪽, 왼쪽 두 번실행되므로 this 체크
            {
                return;
            }
            else
            {
                Debug.Log("B");       // 아이템 사용
                itemData.GetComponent<ItemUse>().Use();
            }
        }
    }

    private void OnTriggerEnter(Collider other)     //Enter이여서 버튼누른 상태로 오브젝트에 가져가야함
    {
        Grabbable = other.gameObject;


        if (other.CompareTag("Grabbable"))
        {
            if (isGrabbing)      //만약 잡는다면
            {
                //잡히는 Grabbable의 위치 값을 손에 맞춤
                Grabbable.transform.position = rightGrabPosition.transform.position;
                Grabbable.transform.rotation = rightGrabPosition.transform.rotation;

                //Grabbable을 손의 하위 객체로 지정
                Grabbable.transform.SetParent(rightGrabPosition);

                //손에 고정하기 위해 iskinematic 활성
                Grabbable.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Grabbable = other.gameObject;



        if (other.CompareTag("Grabbable"))
        {
            if(!isGrabbing)        //놓는다면
            {
                //Grabbable 손의 하위 객체에서 해제
                Grabbable.transform.SetParent(null);

                //손에 놓기 위해 iskinematic 비활성
                Grabbable.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        if(other.CompareTag("Weapon"))
        {
            if(isGrabbing && rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == false)
            {
                weaponName = other.name;
                GameManager.gameManager.weaponName = weaponName;
                ChangeWeapon(weaponName);       //무기 활성화
            }
        }
    }


    public void ChangeWeapon(string weaponName)
    {
        weaponLevel = GameManager.gameManager.weaponLevel;
        // 무기 비활성화시 활성
        if (rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == false)
        {
            rightGrabPosition.transform.Find("WP_Bundle").gameObject.SetActive(true);
        }
        //1랩일 때 1랩 무기 활성
        if (weaponLevel == 1)
        {

            rightGrabPosition.transform.Find("WP_Bundle").transform.Find(weaponName).transform.Find(weaponName + weaponLevel).gameObject.SetActive(true);
        }
        //1랩 초과일 때 전단계무기 비활성 및 현재 무기활성
        else if (1 < weaponLevel && weaponLevel <= 3)
        {
            rightGrabPosition.transform.Find("WP_Bundle").transform.Find(weaponName).transform.Find(weaponName + weaponLevel).gameObject.SetActive(true);
            rightGrabPosition.transform.Find("WP_Bundle").transform.Find(weaponName).transform.Find(weaponName + (weaponLevel - 1)).gameObject.SetActive(false);
        }
        else if(weaponLevel > 3)
        {
            Debug.Log("err");
        }
    }

    private void ItemGrab()
    {
        Debug.DrawRay(leftGrabPositon.position, leftGrabPositon.forward * 10, Color.red);
        //Debug.DrawRay(GrabPositon.position, GrabPositon.forward * 10, Color.blue);

        // 라인 시작점
        lineRenderer.SetPosition(0, leftGrabPositon.position);
        // 라인 종료점
        lineRenderer.SetPosition(1, leftGrabPositon.position + (leftGrabPositon.forward * 10));
        lineRenderer.enabled = true;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.tag == "Item")
            {
                if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))        // 버튼 뗐을때
                {
                    if (this.name == "RightControllerGrabposition")                                         // this가 오른쪽 컨트롤러가 되면 PickUp함수가 오른쪽, 왼쪽 두 번실행되므로 this 체크
                    {
                        return;
                    }
                    else
                    {
                        HoverItem2 hoverItem = hit.transform.GetComponent<HoverItem2>();
                        hit.transform.GetComponent<HoverItem2>().itemRotation = false;                          // 아이템 회전 중지
                        hit.transform.position = leftGrabPositon.transform.position;    // 아이템 팔 위치로
                        hit.transform.SetParent(leftGrabPositon);
                        hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                        Debug.Log(this.name);
                        PickUp();
                    }
                }
            }
        }
    }

    private void PickUp()
    {
        Debug.Log("PickUp");
        if(hit.collider.tag == "Item")
        {
            Item.ItemType item = hit.transform.GetComponent<ItemPickUp>().item.itemType;        //  itemType을 할당
            if (item == Item.ItemType.SportsDrink)
            {
                audioSource.clip = clip[1];
                audioSource.Play();
                hit.transform.GetComponent<ItemSportsDrink>().SpeedIncrease();
                Destroy(hit.transform.gameObject);                  // 삭제
            }
            else
            {
                audioSource.clip = clip[0];
                audioSource.Play();
                Debug.Log(hit.transform.GetComponent<ItemPickUp>().item.itemName + "획득");
                itemSlot.AcquireItem(hit.transform.GetComponent<ItemPickUp>().item);
                Destroy(hit.transform.gameObject, 1f);                  // 삭제
            }
        }
    }
}
