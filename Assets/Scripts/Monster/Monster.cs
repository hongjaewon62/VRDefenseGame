using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Monster : MonoBehaviour
{

    public event Action<GameObject> OnMonsterDeath;
    public bool isTrapped = false; // 트랩 걸렸는지 여부
    public float trapDuration;      // 트랩 지속 시간
    public bool isLullaby = false;  // 자장가 아이템의 영향을 받는지 여부
    public MonsterScriptable monsterData;               // 몬스터 데이터 스크립터블 객체
    public int currentHealth { get; private set; }      // 현재 체력 (외부에서 읽기 허용)
    public int score = 100;                             // 점수

    public AnimationController animController;
    public FacialAnimationController facialAnimationController;
    public string monsterType;
    private Slider hpSlider;                            // 체력 슬라이더
    public Animator monsterAnim;
    private int randomBaseFacial;
    // 외형 카테고리들을 저장할 배열의 배열
    public GameObject[][] appearanceOptions;
    // 외형 카테고리들
    public GameObject[] hairOptions;
    public GameObject[] topOptions;
    public GameObject[] bottomOptions;
    public GameObject[] shoeOptions;

    //메테리얼
    public Material[][] appearanceMaterials;
    public Material[] hairMaterials;
    public Material[] topMaterials;
    public Material[] bottomMaterials;
    public Material[] shoeMaterials;
    public Vector3 clothsScale = new Vector3(11.0f, 11.0f, 9.0f);

    public GameObject head;
    public GameObject toiletHead;

    // 움직임 관련 변수
    private MonsterMove moveComponent;

    // 넉백 변수
    Vector3 KnockBackPosition;


    private void Awake()
    {
        randomBaseFacial = UnityEngine.Random.Range(0, 2);
        hpSlider = GetComponentInChildren<Slider>();    // 몬스터에서 hp 슬라이더를 찾음
        moveComponent = GetComponent<MonsterMove>();
        monsterType = RemoveCloneFromName(gameObject.name);
        // 몬스터에 몬스터 슬라이더가 존재하는지 확인
        if (hpSlider == null)
            Debug.Assert(false, "Error (Monster Slider) : 몬스터에 체력 바가 존재하지 않습니다.");

        // 몬스터에 움직임관련 컴포넌트가 존재하는지 확인
        if (moveComponent == null)
            Debug.Assert(false, "Error (Monster Move) : 몬스터에 움직임에 관한 컴포넌트가 존재하지 않습니다.");
    }
    private void Start()
    {
        currentHealth = monsterData.maxHp; // 현재 체력 최대 체력으로 설정
        InitializeAppearanceOptions(); // 외형 카테고리 배열 초기화
        SetRandomAppearance(); // 랜덤하게 바디, 의상, 헤어 등을 선택하여 적용
        facialAnimationController.SetFacial(monsterType, randomBaseFacial);
        animController.SetWalkSpeed(monsterData.moveSpeed);
    }

    void Update()
    {
        if (isTrapped)
            return;
        moveComponent.InspectDestination();

        if (this.GetComponent<CapsuleCollider>().enabled == false) {
            this.GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    public void SetTrapped(float duration)
    {
        if (duration < 0)
        {
            Debug.Assert(false, "Error (Unacceptable Value) : 스턴 길이는 음수가 될 수 없습니다.");
            return;
        }

        if (!isTrapped)
        {
            isTrapped = true;
            
            moveComponent.Stop();
            trapDuration = duration;
            
            StartCoroutine(ReleaseFromTrap()); // 멈춤 상태를 표현하는 애니메이션 등을 추가하기 
            

        }
    }

    public void SetLullaby(float duration) 
    {
        if (duration < 0.0f)
        {
            Debug.Assert(false, "Error (Unacceptable Value) : 스턴 길이는 음수가 될 수 없습니다.");
            return;
        }

        if (!isLullaby) 
        {
            isLullaby = true;
            moveComponent.SetIsSlowingDown(duration);   //Monster의 속도가 느려짐
            float d = duration * 2.0f;                  //느려지는 시간 + 멈춰있는 시간
            StartCoroutine(ReleaseFromLullaby(d));      //다시 Monster가 움직임
        }
    }

    private IEnumerator ReleaseFromTrap()
    {
        animController.SetisTrapped(isTrapped);
        facialAnimationController.SetFacial(monsterType, 6);
        yield return new WaitForSeconds(trapDuration); //몬스터 정지
        isTrapped = false;
        animController.SetisTrapped(isTrapped);
        facialAnimationController.SetFacial(monsterType, randomBaseFacial);
        moveComponent.Move();
    }

    private IEnumerator ReleaseFromLullaby(float duration)
    {
        Debug.Log("Start Timer Release Lullaby");
        facialAnimationController.SetFacial(monsterType, 2);
        yield return new WaitForSeconds(duration);
        isLullaby = false;
        facialAnimationController.SetFacial(monsterType, randomBaseFacial);
        moveComponent.Move();                           //Monster 다시 이동
    }

    private IEnumerator KnockBack(Vector3 weaponpos, float knockback, bool isHitByLog)
    {
        //Lerp사용 밀려나는 느낌이 들게 만듬
        float flytime = 0.0f;
        if (isHitByLog == true)
        {
            animController.SetIsLogHit();
            facialAnimationController.SetFacial(monsterType, 6);
        }
        else
        {
            animController.SetKnockBack();
            facialAnimationController.SetFacial(monsterType, 3);
        }
        
        while (flytime < 0.125) //0.2초 동안 넉백
        {
            flytime += (Time.deltaTime);
            KnockBackPosition = transform.position + ((transform.position - weaponpos) * knockback);    //현재 위치 - 무기 위치에 밀려나는 정도를 곱하고 더해 밀려난 위치 얻음
            transform.position = Vector3.Lerp(transform.position, KnockBackPosition, flytime/ 0.125f);     //0.125초를 기준으로 날라감
            
            yield return null;
        }
        yield return null;
    }

    public void TakeDamage(int damage,Vector3 weaponpos,float knockback,bool isHitByLog) // 데미지 받는 코드
    {
        currentHealth -= damage;    // 현재 체력에서 데미지 만큼 빼는 코드
        StartCoroutine(KnockBack(weaponpos, knockback, isHitByLog));    //넉백 코루틴
        Invoke("CallBaseFacial", 2);
        // 체력 0 이하시 작동
        if (currentHealth <= 0)
        {
            hpSlider.value = 0;
            ItemDrop();
            Die(); 
        }

        // 만약 캐릭터가 체력이 남아있을 경우
        else
            hpSlider.value = ((float)currentHealth / monsterData.maxHp) * 100;  // 현재 체력을 슬라이더에 반영
    }

    public void HitByPaperTowel()
    {
        // 이동 멈추기 및 표정 변화 애니메이션 걷기 가속
        moveComponent.Stop();
        head.SetActive(false);
        toiletHead.SetActive(true);
        animController.SetWalkSpeed(3.0f);
    }
    public void CallBaseFacial()
    {
        facialAnimationController.SetFacial(monsterType, randomBaseFacial);
    }
    public void Die()// 몬스터가 죽었을 때 호출
    {
        if (OnMonsterDeath != null)
        {
            OnMonsterDeath(gameObject);
        }
        
        
        if(currentHealth <= 0)
        {
            GameManager.gameManager.AddCurrency(monsterData.coin); // 몬스터 coin 값 만큼 재화 증가
            GameManager.gameManager.score += score;
        }
        UiManager.uiManager.UpdateCurrencyText(GameManager.gameManager.currency);
    }

    private void InitializeAppearanceOptions()
    {
        // 외형 카테고리 배열 생성과 초기화
        appearanceOptions = new GameObject[4][];
        appearanceOptions[0] = hairOptions;
        appearanceOptions[1] = topOptions;
        appearanceOptions[2] = bottomOptions;
        appearanceOptions[3] = shoeOptions;
        appearanceMaterials = new Material[4][];
        appearanceMaterials[0] = hairMaterials;
        appearanceMaterials[1] = topMaterials;
        appearanceMaterials[2] = bottomMaterials;
        appearanceMaterials[3] = shoeMaterials;
    }

    private void SetRandomAppearance() // 랜덤 의상 생성 코드
    {
        for (int i = 0; i < appearanceOptions.Length; i++)
        {
            // 해당 카테고리의 배열 길이가 0 이상인 경우에만 랜덤한 인덱스 선택
            if (appearanceOptions[i].Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, appearanceOptions[i].Length);
                GameObject selectedAppearancePrefab = appearanceOptions[i][randomIndex];
                GameObject selectedAppearance = Instantiate(selectedAppearancePrefab, transform.position, transform.rotation, transform);
                selectedAppearance.transform.localScale = clothsScale;
                
                for(int k = 0; k< selectedAppearance.transform.childCount;k ++) // 랜덤 메테리얼 적용 코드
                {
                    if (selectedAppearance.transform.GetChild(k).GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        Renderer[] renderers = selectedAppearance.GetComponentsInChildren<Renderer>(); // 모든 하위 렌더러 컴포넌트를 가져옵니다.
                        int randomMaterialIndex = UnityEngine.Random.Range(0, appearanceMaterials[i].Length);
                        foreach (Renderer rend in renderers)
                        {
                            Material[] materials = new Material[rend.sharedMaterials.Length];
                            
                            for (int l = 0; l < materials.Length; l++)
                            {
                                materials[l] = appearanceMaterials[i][randomMaterialIndex]; // 새로운 메테리얼로 모든 메테리얼을 교체합니다.
                            }

                            rend.sharedMaterials = materials;
                        }
                    }
                }

                Animator appearanceAnim = selectedAppearance.GetComponent<Animator>();
                if (appearanceAnim == null)
                {
                    appearanceAnim = selectedAppearance.AddComponent<Animator>();
                    animController.SetAnimator(i, appearanceAnim);
                }
                
                appearanceAnim.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
                

            }
            
        }
    }
    string RemoveCloneFromName(string objectName)
    {
        if (objectName.EndsWith("(Clone)"))
        {
            // 이름이 "Clone"으로 끝나는 경우, "Clone"을 제외한 나머지 부분을 반환
            return objectName.Substring(0, objectName.Length - 7); 
        }
        else
        {
            // "Clone"이 포함되어 있지 않으면 원래 이름 그대로 반환
            return objectName;
        }
    }
    public void ItemDrop()
    {
        Choose(new float[2] { 25f, 75f });
        
        float Choose(float[] probs)
        {
            
            float total = 0;

            foreach (float elem in probs)
            {
                total += elem;
            }

            float randomPoint = UnityEngine.Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    switch(i)
                    {
                        case 0:
                            int rand = UnityEngine.Random.Range(0, monsterData.dropItem.Length);
                            Instantiate(monsterData.dropItem[rand], transform.position, Quaternion.identity);
                            break;
                        case 1:
                            break;
                    }
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }

    }
}