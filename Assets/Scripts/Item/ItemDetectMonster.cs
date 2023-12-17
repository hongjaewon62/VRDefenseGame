using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetectMonster : MonoBehaviour
{
    //trigger Collider를 Item의 자식 개체에 생성해서 적용
    private float lullabyDuration;                      //Monster 이동제약시간, 부모 객체에서 받아오도록 함 
    private bool isActive = false;                      //아이템 활성화 여부, 부모 객체 스크립트에서 변수 상태 변경
    private AudioSource audioSource;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(isActive)
        {
            sphereCollider.radius = 7f;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (isActive)
        {
            if (other.CompareTag("Monster")) //충돌한 object가 Monster이면
            {
                other.GetComponent<Monster>().SetLullaby(lullabyDuration); //Monster 스크립트 SetLullaby 호출
            }
            else
            {
                audioSource.Play();
            }
        }
    }

    public void SetIsActive(bool value)                 //isActive변수를 다른 스크립트에서 변경할 수 있게 하는 함수
    {
        isActive = value;
    }

    public void SetLullabyDuration(float duration)      //부모 객체에서 호출해 값을 받아옴
    {
        lullabyDuration = duration;
    }
}
