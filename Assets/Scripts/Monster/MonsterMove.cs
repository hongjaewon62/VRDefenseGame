using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    [SerializeField]
    private MonsterScriptable monsterData;

    private bool isStop = false; // �̵� ������ ���θ� ��Ÿ���� ����
    private NavMeshAgent agent;                 // ���� ã�Ƽ� �̵��� ������Ʈ

    public GameObject finalTarget;   // ������ ���� ��ǥ ����
    public float distanceAhead = 20f; // ���� ��ġ���� ������ �������� ������ �Ÿ�
    private Rigidbody rigid;
    private Collider collid;

    private Vector3 currentTarget;
    private float originalSpeed;
    private float currentSpeed;
    private bool isSlowingDown = false;
    public bool isCatSee = false;
    private float lullabyDuration;
    private float distance;
    private float thresholdDistance;
    private bool enteredZone = false;
    private Vector3 randomTarget; // ���� �̵� ������
    private void Awake()
    {
        finalTarget = GameObject.FindWithTag("Finish");
        thresholdDistance = Vector3.Distance(gameObject.transform.position, finalTarget.transform.position);
        agent = GetComponent<NavMeshAgent>();   // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        rigid = GetComponent<Rigidbody>();
        collid = GetComponent<Collider>();
        if (agent == null)
        {
            Debug.Assert(false, "Error (NavMeshAgent is Null) : �ش� ��ü�� NavMeshAgent�� �������� �ʽ��ϴ�.");
            return;
        }

        if (rigid == null)
        {
            Debug.Assert(false, "Error (RigidBody is Null) : �ش� ��ü�� RigidBody�� �������� �ʽ��ϴ�.");
            return;
        }

        if (collid == null)
        {
            Debug.Assert(false, "Error (NavMeshAgent is Null) : �ش� ��ü�� Collider�� �������� �ʽ��ϴ�.");
            return;
        }

    }

    void Start()
    {
        currentTarget = finalTarget.transform.position;
        agent.SetDestination(currentTarget);   // ������ ����
        agent.speed = monsterData.moveSpeed;        // ���� �̵� �ӵ� �����Ϳ��� �޾ƿͼ� ����
        originalSpeed = monsterData.moveSpeed;      //�ʱ� �̵��ӵ� ����
        currentSpeed = monsterData.moveSpeed;
    }

    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, finalTarget.transform.position);

        if(distance <= thresholdDistance && enteredZone == false)
        {
            // ���� �Ÿ����� �Ǹ� ������ �������� ���� �� �Ÿ��� ���̱�
            SetRandomDestination();
            agent.SetDestination(currentTarget);
            thresholdDistance -= 10;
            enteredZone = true;

        }  else if (distance <=20)
        {
            currentTarget = finalTarget.transform.position;
            agent.SetDestination(currentTarget);
        }
        // ���������� �Ÿ� 
        if (agent.remainingDistance < 1f && agent.destination != finalTarget.transform.position && isCatSee == false) // �������� �����ϸ� ������
        {
            currentTarget = finalTarget.transform.position;
            agent.SetDestination(currentTarget);
            enteredZone = false;
        }

        if (isSlowingDown) 
        {
            SlowingDown();
        }
        
        transform.rotation = Quaternion.LookRotation(agent.transform.forward);
    }

    // ���� ���Ͱ� ������ ���� �̵��ϴ��� �˻��ϴ� �޼ҵ�
    public void InspectDestination()
    {
        // �̲��� ����
        rigid.velocity = Vector3.zero;

        // ���� ���Ͱ� �̵����� �ʴ´ٸ� ���� ��ũ��Ʈ�� �������� ����
        if (!isStop)
            return;

        // ���Ͱ� �̵������� ���� �� ���� ��ġ�� �̵��ϰ� ����
        agent.isStopped = false;
        agent.SetDestination(finalTarget.transform.position);
        isStop = true; // �������� �̵� ������ �ٲ� agent.SetDestination(target.position); �� �Լ� �ѹ��� �����ϰ� ��
    }

    // ���͸� ���߰� �ϴ� �޼ҵ�
    public void Stop()
    {
        isStop = false;
        agent.isStopped = true;             // agent �̵� ���߱�
        rigid.velocity = Vector3.zero;
        collid.isTrigger = true;
    }

    public void Move()
    {
        isStop = true;
        agent.isStopped = false;
        collid.isTrigger = false;
        agent.SetDestination(finalTarget.transform.position);
    }

    public void SetIsSlowingDown(float duration)
    {
        isSlowingDown = true;           
        lullabyDuration = duration;
    }

    public void SlowingDown()
    {
        if (currentSpeed >= 0.001f)
        {
            currentSpeed -= originalSpeed / lullabyDuration * Time.deltaTime;   //duration���� �ӵ��� ���� 0���� �پ��
            agent.speed = currentSpeed;

        }
        else                                                                    //�ӵ��� 0�� ��������� 
        {
            Stop();                                                             //����
            agent.speed = originalSpeed;                                        //�ӵ� ���󺹱�
            isSlowingDown = false;
        }
    }

    public bool GetIsStop() 
    {
        return isStop;
    }

    public void SetMoveTimer(float timer) 
    {
        Invoke("Move", timer);
    }

    public void SetRandomDestination()
    {
        // ������Ʈ�� ���� ��ġ�� ���� ���� ���
        Vector3 objectPosition = transform.position;
        Vector3 direction = (finalTarget.transform.position - gameObject.transform.position).normalized; // ���� ���� ���

        // ������ ���� ���� ����
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

        // ���� ���͸� ���� ������ �����ϸ�
        Vector3 randomOffset = randomDirection * UnityEngine.Random.Range(0f, distanceAhead);

        // ���Ϳ��� ���� ��ǥ���� �������� distanceAhead ��ŭ �̵��� �������� ���� ��ġ �����ϴ� �ڵ�
        Vector3 randomPosition = objectPosition + direction * distanceAhead + randomOffset;


        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, distanceAhead, 1)) // ��ġ�� �̵��� �� �ִ� ��ġ���� Ȯ���ϴ� �ڵ� ��ȣ���� ���� ��ġ�� 30f ���������� �ٽ� ã��
        {
            randomTarget = hit.position;
            currentTarget = randomTarget;
            
        }
        else
        {
            Debug.LogWarning("���ø� ���� - ��ȿ���� ���� ��ġ");
        }
    }
}