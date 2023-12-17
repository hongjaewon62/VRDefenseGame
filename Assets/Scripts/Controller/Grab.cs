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
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))       //��ư�� ������ ��
        {
            isGrabbing = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.A) )        //A ��ư�� ������ ���´�
        {
            isGrabbing = false;
            if (!isGrabbing && rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == true && GameManager.gameManager.GetWeaponSwapEnabled() == true)        //���� ���´ٸ� ��� ���⸦ ��Ȱ�� �Ѵ�.
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
            if (this.name == "RightControllerGrabposition")                                         // this�� ������ ��Ʈ�ѷ��� �Ǹ� PickUp�Լ��� ������, ���� �� ������ǹǷ� this üũ
            {
                return;
            }
            else
            {
                Debug.Log("B");       // ������ ���
                itemData.GetComponent<ItemUse>().Use();
            }
        }
    }

    private void OnTriggerEnter(Collider other)     //Enter�̿��� ��ư���� ���·� ������Ʈ�� ����������
    {
        Grabbable = other.gameObject;


        if (other.CompareTag("Grabbable"))
        {
            if (isGrabbing)      //���� ��´ٸ�
            {
                //������ Grabbable�� ��ġ ���� �տ� ����
                Grabbable.transform.position = rightGrabPosition.transform.position;
                Grabbable.transform.rotation = rightGrabPosition.transform.rotation;

                //Grabbable�� ���� ���� ��ü�� ����
                Grabbable.transform.SetParent(rightGrabPosition);

                //�տ� �����ϱ� ���� iskinematic Ȱ��
                Grabbable.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Grabbable = other.gameObject;



        if (other.CompareTag("Grabbable"))
        {
            if(!isGrabbing)        //���´ٸ�
            {
                //Grabbable ���� ���� ��ü���� ����
                Grabbable.transform.SetParent(null);

                //�տ� ���� ���� iskinematic ��Ȱ��
                Grabbable.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        if(other.CompareTag("Weapon"))
        {
            if(isGrabbing && rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == false)
            {
                weaponName = other.name;
                GameManager.gameManager.weaponName = weaponName;
                ChangeWeapon(weaponName);       //���� Ȱ��ȭ
            }
        }
    }


    public void ChangeWeapon(string weaponName)
    {
        weaponLevel = GameManager.gameManager.weaponLevel;
        // ���� ��Ȱ��ȭ�� Ȱ��
        if (rightGrabPosition.transform.Find("WP_Bundle").gameObject.activeSelf == false)
        {
            rightGrabPosition.transform.Find("WP_Bundle").gameObject.SetActive(true);
        }
        //1���� �� 1�� ���� Ȱ��
        if (weaponLevel == 1)
        {

            rightGrabPosition.transform.Find("WP_Bundle").transform.Find(weaponName).transform.Find(weaponName + weaponLevel).gameObject.SetActive(true);
        }
        //1�� �ʰ��� �� ���ܰ蹫�� ��Ȱ�� �� ���� ����Ȱ��
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

        // ���� ������
        lineRenderer.SetPosition(0, leftGrabPositon.position);
        // ���� ������
        lineRenderer.SetPosition(1, leftGrabPositon.position + (leftGrabPositon.forward * 10));
        lineRenderer.enabled = true;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.tag == "Item")
            {
                if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))        // ��ư ������
                {
                    if (this.name == "RightControllerGrabposition")                                         // this�� ������ ��Ʈ�ѷ��� �Ǹ� PickUp�Լ��� ������, ���� �� ������ǹǷ� this üũ
                    {
                        return;
                    }
                    else
                    {
                        HoverItem2 hoverItem = hit.transform.GetComponent<HoverItem2>();
                        hit.transform.GetComponent<HoverItem2>().itemRotation = false;                          // ������ ȸ�� ����
                        hit.transform.position = leftGrabPositon.transform.position;    // ������ �� ��ġ��
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
            Item.ItemType item = hit.transform.GetComponent<ItemPickUp>().item.itemType;        //  itemType�� �Ҵ�
            if (item == Item.ItemType.SportsDrink)
            {
                audioSource.clip = clip[1];
                audioSource.Play();
                hit.transform.GetComponent<ItemSportsDrink>().SpeedIncrease();
                Destroy(hit.transform.gameObject);                  // ����
            }
            else
            {
                audioSource.clip = clip[0];
                audioSource.Play();
                Debug.Log(hit.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ��");
                itemSlot.AcquireItem(hit.transform.GetComponent<ItemPickUp>().item);
                Destroy(hit.transform.gameObject, 1f);                  // ����
            }
        }
    }
}
