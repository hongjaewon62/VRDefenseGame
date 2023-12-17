using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManhole : MonoBehaviour
{
    [SerializeField]
    private float trapDuration = 5.0f;
    [SerializeField]
    private float objectDuration = 10.0f;
    private HoverItem2 hoverItem;
    private GameObject cover;
    private Vector3 vector;
    private AudioSource audioSource;

    private void Awake()
    {
        hoverItem = GetComponent<HoverItem2>();
        cover = gameObject.transform.GetChild(1).gameObject;
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hoverItem.itemRotation == false && transform.root.name.Equals("OVRPlayerController") == false)
        {
            StartCoroutine(DestroyManhole());
            //cover.transform.localPosition = new Vector3(0, 0, 1);
            StartCoroutine(MoveCover());
            if (other.CompareTag("Monster"))
            {
                if (transform.root.name.Equals("OVRPlayerController") == false)
                {
                    Trap(other);
                }
            }
            else {
                audioSource.Play();
            }
        }
    }

    private void Trap(Collider monster)
    {
        monster.GetComponent<Monster>().trapDuration = trapDuration;
        //monster.GetComponent<Monster>().isTrapped = true;
        monster.GetComponent<Monster>().SetTrapped(trapDuration);
    }

    private IEnumerator DestroyManhole()
    {
        yield return new WaitForSeconds(objectDuration);
        Destroy(gameObject);
    }

    private IEnumerator MoveCover()             // ¶Ñ²± ¿òÁ÷ÀÓ
    {
        float moveTime = 0.0f;
        while (moveTime < 1.0f)
        {
            Debug.Log(moveTime);
            cover.transform.Translate(new Vector3(0.1f, 0, 0) * 0.1f);
            moveTime += Time.deltaTime;
            yield return null;
        }
    }
}
