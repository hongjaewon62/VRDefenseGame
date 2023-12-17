using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    [SerializeField]
    private MonsterScriptable monsterData;

    private bool isStop = false; // 이동 중인지 여부를 나타내는 변수
    private NavMeshAgent agent;                 // 길을 찾아서 이동할 에이전트

    public GameObject finalTarget;   // 몬스터의 최종 목표 지점
    public float distanceAhead = 20f; // 몬스터 위치에서 목적지 방향으로 생성할 거리
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
    private Vector3 randomTarget; // 랜덤 이동 목적지
    private void Awake()
    {
        finalTarget = GameObject.FindWithTag("Finish");
        thresholdDistance = Vector3.Distance(gameObject.transform.position, finalTarget.transform.position);
        agent = GetComponent<NavMeshAgent>();   // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        rigid = GetComponent<Rigidbody>();
        collid = GetComponent<Collider>();
        if (agent == null)
        {
            Debug.Assert(false, "Error (NavMeshAgent is Null) : 해당 객체에 NavMeshAgent가 존재하지 않습니다.");
            return;
        }

        if (rigid == null)
        {
            Debug.Assert(false, "Error (RigidBody is Null) : 해당 객체에 RigidBody가 존재하지 않습니다.");
            return;
        }

        if (collid == null)
        {
            Debug.Assert(false, "Error (NavMeshAgent is Null) : 해당 객체에 Collider가 존재하지 않습니다.");
            return;
        }

    }

    void Start()
    {
        currentTarget = finalTarget.transform.position;
        agent.SetDestination(currentTarget);   // 목적지 설정
        agent.speed = monsterData.moveSpeed;        // 몬스터 이동 속도 데이터에서 받아와서 설정
        originalSpeed = monsterData.moveSpeed;      //초기 이동속도 저장
        currentSpeed = monsterData.moveSpeed;
    }

    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, finalTarget.transform.position);

        if(distance <= thresholdDistance && enteredZone == false)
        {
            // 일정 거리값이 되면 랜덤한 목적지를 설정 및 거리값 줄이기
            SetRandomDestination();
            agent.SetDestination(currentTarget);
            thresholdDistance -= 10;
            enteredZone = true;

        }  else if (distance <=20)
        {
            currentTarget = finalTarget.transform.position;
            agent.SetDestination(currentTarget);
        }
        // 목적지와의 거리 
        if (agent.remainingDistance < 1f && agent.destination != finalTarget.transform.position && isCatSee == false) // 목적지에 도착하면 동작함
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

    // 현재 몬스터가 목적지 향해 이동하는지 검사하는 메소드
    public void InspectDestination()
    {
        // 미끄럼 방지
        rigid.velocity = Vector3.zero;

        // 현재 몬스터가 이동하지 않는다면 이후 스크립트는 실행하지 않음
        if (!isStop)
            return;

        // 몬스터가 이동중이지 않을 때 다음 위치로 이동하게 설정
        agent.isStopped = false;
        agent.SetDestination(finalTarget.transform.position);
        isStop = true; // 목적지로 이동 중으로 바꿔 agent.SetDestination(target.position); 이 함수 한번만 동작하게 함
    }

    // 몬스터를 멈추게 하는 메소드
    public void Stop()
    {
        isStop = false;
        agent.isStopped = true;             // agent 이동 멈추기
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
            currentSpeed -= originalSpeed / lullabyDuration * Time.deltaTime;   //duration동안 속도가 점점 0으로 줄어듦
            agent.speed = currentSpeed;

        }
        else                                                                    //속도가 0에 가까워지면 
        {
            Stop();                                                             //멈춤
            agent.speed = originalSpeed;                                        //속도 원상복구
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
        // 오브젝트의 현재 위치와 방향 벡터 얻기
        Vector3 objectPosition = transform.position;
        Vector3 direction = (finalTarget.transform.position - gameObject.transform.position).normalized; // 방향 벡터 계산

        // 무작위 방향 벡터 생성
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

        // 방향 벡터를 일정 범위로 스케일링
        Vector3 randomOffset = randomDirection * UnityEngine.Random.Range(0f, distanceAhead);

        // 몬스터에서 최종 목표지점 방향으로 distanceAhead 만큼 이동한 지점에서 랜덤 위치 생성하는 코드
        Vector3 randomPosition = objectPosition + direction * distanceAhead + randomOffset;


        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, distanceAhead, 1)) // 위치가 이동할 수 있는 위치인지 확인하는 코드 유호하진 않은 위치면 30f 범위내에서 다시 찾음
        {
            randomTarget = hit.position;
            currentTarget = randomTarget;
            
        }
        else
        {
            Debug.LogWarning("샘플링 실패 - 유효하지 않은 위치");
        }
    }
}