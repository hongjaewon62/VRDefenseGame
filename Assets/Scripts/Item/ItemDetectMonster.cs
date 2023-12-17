using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetectMonster : MonoBehaviour
{
    //trigger Collider�� Item�� �ڽ� ��ü�� �����ؼ� ����
    private float lullabyDuration;                      //Monster �̵�����ð�, �θ� ��ü���� �޾ƿ����� �� 
    private bool isActive = false;                      //������ Ȱ��ȭ ����, �θ� ��ü ��ũ��Ʈ���� ���� ���� ����
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
            if (other.CompareTag("Monster")) //�浹�� object�� Monster�̸�
            {
                other.GetComponent<Monster>().SetLullaby(lullabyDuration); //Monster ��ũ��Ʈ SetLullaby ȣ��
            }
            else
            {
                audioSource.Play();
            }
        }
    }

    public void SetIsActive(bool value)                 //isActive������ �ٸ� ��ũ��Ʈ���� ������ �� �ְ� �ϴ� �Լ�
    {
        isActive = value;
    }

    public void SetLullabyDuration(float duration)      //�θ� ��ü���� ȣ���� ���� �޾ƿ�
    {
        lullabyDuration = duration;
    }
}
