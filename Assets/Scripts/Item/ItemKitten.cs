using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemKitten : MonoBehaviour
{
    [SerializeField]
    private float duration = 5.0f;
    [SerializeField]
    private float range = 0f;
    private SphereCollider kittenCollider;
    private HoverItem2 hoverItem;
    private AudioSource audioSource;

    private void Awake()
    {
        kittenCollider = GetComponent<SphereCollider>();
        hoverItem = GetComponent<HoverItem2>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hoverItem.itemRotation == false && transform.root.name.Equals("OVRPlayerController") == false)
        {
            StartCoroutine(DestroyKitten());
            kittenCollider.radius = range;
            if (other.CompareTag("Monster"))
            {
                if (transform.root.name.Equals("OVRPlayerController") == false)
                {
                    //itemFunction.StartCoroutine(itemFunction.SeeKitten(other, this.gameObject, duration));
                    StartCoroutine(SeeKitten(other));
                }
            }
            else
            {
                audioSource.Play();
            }
        }
    }

    private IEnumerator DestroyKitten()
    {
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = new Vector3(1000, 100, 1000);

        time = 0.0f;
        while (time < 20.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator SeeKitten(Collider monster)
    {
        float moveTime = 0.0f;
        MonsterMove monsterMove = monster.GetComponent<MonsterMove>();
        monsterMove.isCatSee = true;
        Monster monsterScript = monster.GetComponent<Monster>();
        NavMeshAgent agent = monster.GetComponent<NavMeshAgent>();
        agent.SetDestination(gameObject.transform.position);
        monsterScript.facialAnimationController.SetFacial(monsterScript.monsterType, 4);
        monsterScript.Invoke("CallBaseFacial", duration);
        while (moveTime < duration)
        {
            moveTime += Time.deltaTime;
            yield return null;
        }
        agent.SetDestination(monsterMove.finalTarget.transform.position);
        monsterMove.isCatSee = false;
        gameObject.transform.position = new Vector3(1000, 100, 1000);

        moveTime = 0.0f;
        while(moveTime < 20.0f)
        {
            moveTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
