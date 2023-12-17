using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Monster : MonoBehaviour
{

    public event Action<GameObject> OnMonsterDeath;
    public bool isTrapped = false; // Ʈ�� �ɷȴ��� ����
    public float trapDuration;      // Ʈ�� ���� �ð�
    public bool isLullaby = false;  // ���尡 �������� ������ �޴��� ����
    public MonsterScriptable monsterData;               // ���� ������ ��ũ���ͺ� ��ü
    public int currentHealth { get; private set; }      // ���� ü�� (�ܺο��� �б� ���)
    public int score = 100;                             // ����

    public AnimationController animController;
    public FacialAnimationController facialAnimationController;
    public string monsterType;
    private Slider hpSlider;                            // ü�� �����̴�
    public Animator monsterAnim;
    private int randomBaseFacial;
    // ���� ī�װ����� ������ �迭�� �迭
    public GameObject[][] appearanceOptions;
    // ���� ī�װ���
    public GameObject[] hairOptions;
    public GameObject[] topOptions;
    public GameObject[] bottomOptions;
    public GameObject[] shoeOptions;

    //���׸���
    public Material[][] appearanceMaterials;
    public Material[] hairMaterials;
    public Material[] topMaterials;
    public Material[] bottomMaterials;
    public Material[] shoeMaterials;
    public Vector3 clothsScale = new Vector3(11.0f, 11.0f, 9.0f);

    public GameObject head;
    public GameObject toiletHead;

    // ������ ���� ����
    private MonsterMove moveComponent;

    // �˹� ����
    Vector3 KnockBackPosition;


    private void Awake()
    {
        randomBaseFacial = UnityEngine.Random.Range(0, 2);
        hpSlider = GetComponentInChildren<Slider>();    // ���Ϳ��� hp �����̴��� ã��
        moveComponent = GetComponent<MonsterMove>();
        monsterType = RemoveCloneFromName(gameObject.name);
        // ���Ϳ� ���� �����̴��� �����ϴ��� Ȯ��
        if (hpSlider == null)
            Debug.Assert(false, "Error (Monster Slider) : ���Ϳ� ü�� �ٰ� �������� �ʽ��ϴ�.");

        // ���Ϳ� �����Ӱ��� ������Ʈ�� �����ϴ��� Ȯ��
        if (moveComponent == null)
            Debug.Assert(false, "Error (Monster Move) : ���Ϳ� �����ӿ� ���� ������Ʈ�� �������� �ʽ��ϴ�.");
    }
    private void Start()
    {
        currentHealth = monsterData.maxHp; // ���� ü�� �ִ� ü������ ����
        InitializeAppearanceOptions(); // ���� ī�װ� �迭 �ʱ�ȭ
        SetRandomAppearance(); // �����ϰ� �ٵ�, �ǻ�, ��� ���� �����Ͽ� ����
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
            Debug.Assert(false, "Error (Unacceptable Value) : ���� ���̴� ������ �� �� �����ϴ�.");
            return;
        }

        if (!isTrapped)
        {
            isTrapped = true;
            
            moveComponent.Stop();
            trapDuration = duration;
            
            StartCoroutine(ReleaseFromTrap()); // ���� ���¸� ǥ���ϴ� �ִϸ��̼� ���� �߰��ϱ� 
            

        }
    }

    public void SetLullaby(float duration) 
    {
        if (duration < 0.0f)
        {
            Debug.Assert(false, "Error (Unacceptable Value) : ���� ���̴� ������ �� �� �����ϴ�.");
            return;
        }

        if (!isLullaby) 
        {
            isLullaby = true;
            moveComponent.SetIsSlowingDown(duration);   //Monster�� �ӵ��� ������
            float d = duration * 2.0f;                  //�������� �ð� + �����ִ� �ð�
            StartCoroutine(ReleaseFromLullaby(d));      //�ٽ� Monster�� ������
        }
    }

    private IEnumerator ReleaseFromTrap()
    {
        animController.SetisTrapped(isTrapped);
        facialAnimationController.SetFacial(monsterType, 6);
        yield return new WaitForSeconds(trapDuration); //���� ����
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
        moveComponent.Move();                           //Monster �ٽ� �̵�
    }

    private IEnumerator KnockBack(Vector3 weaponpos, float knockback, bool isHitByLog)
    {
        //Lerp��� �з����� ������ ��� ����
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
        
        while (flytime < 0.125) //0.2�� ���� �˹�
        {
            flytime += (Time.deltaTime);
            KnockBackPosition = transform.position + ((transform.position - weaponpos) * knockback);    //���� ��ġ - ���� ��ġ�� �з����� ������ ���ϰ� ���� �з��� ��ġ ����
            transform.position = Vector3.Lerp(transform.position, KnockBackPosition, flytime/ 0.125f);     //0.125�ʸ� �������� ����
            
            yield return null;
        }
        yield return null;
    }

    public void TakeDamage(int damage,Vector3 weaponpos,float knockback,bool isHitByLog) // ������ �޴� �ڵ�
    {
        currentHealth -= damage;    // ���� ü�¿��� ������ ��ŭ ���� �ڵ�
        StartCoroutine(KnockBack(weaponpos, knockback, isHitByLog));    //�˹� �ڷ�ƾ
        Invoke("CallBaseFacial", 2);
        // ü�� 0 ���Ͻ� �۵�
        if (currentHealth <= 0)
        {
            hpSlider.value = 0;
            ItemDrop();
            Die(); 
        }

        // ���� ĳ���Ͱ� ü���� �������� ���
        else
            hpSlider.value = ((float)currentHealth / monsterData.maxHp) * 100;  // ���� ü���� �����̴��� �ݿ�
    }

    public void HitByPaperTowel()
    {
        // �̵� ���߱� �� ǥ�� ��ȭ �ִϸ��̼� �ȱ� ����
        moveComponent.Stop();
        head.SetActive(false);
        toiletHead.SetActive(true);
        animController.SetWalkSpeed(3.0f);
    }
    public void CallBaseFacial()
    {
        facialAnimationController.SetFacial(monsterType, randomBaseFacial);
    }
    public void Die()// ���Ͱ� �׾��� �� ȣ��
    {
        if (OnMonsterDeath != null)
        {
            OnMonsterDeath(gameObject);
        }
        
        
        if(currentHealth <= 0)
        {
            GameManager.gameManager.AddCurrency(monsterData.coin); // ���� coin �� ��ŭ ��ȭ ����
            GameManager.gameManager.score += score;
        }
        UiManager.uiManager.UpdateCurrencyText(GameManager.gameManager.currency);
    }

    private void InitializeAppearanceOptions()
    {
        // ���� ī�װ� �迭 ������ �ʱ�ȭ
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

    private void SetRandomAppearance() // ���� �ǻ� ���� �ڵ�
    {
        for (int i = 0; i < appearanceOptions.Length; i++)
        {
            // �ش� ī�װ��� �迭 ���̰� 0 �̻��� ��쿡�� ������ �ε��� ����
            if (appearanceOptions[i].Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, appearanceOptions[i].Length);
                GameObject selectedAppearancePrefab = appearanceOptions[i][randomIndex];
                GameObject selectedAppearance = Instantiate(selectedAppearancePrefab, transform.position, transform.rotation, transform);
                selectedAppearance.transform.localScale = clothsScale;
                
                for(int k = 0; k< selectedAppearance.transform.childCount;k ++) // ���� ���׸��� ���� �ڵ�
                {
                    if (selectedAppearance.transform.GetChild(k).GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        Renderer[] renderers = selectedAppearance.GetComponentsInChildren<Renderer>(); // ��� ���� ������ ������Ʈ�� �����ɴϴ�.
                        int randomMaterialIndex = UnityEngine.Random.Range(0, appearanceMaterials[i].Length);
                        foreach (Renderer rend in renderers)
                        {
                            Material[] materials = new Material[rend.sharedMaterials.Length];
                            
                            for (int l = 0; l < materials.Length; l++)
                            {
                                materials[l] = appearanceMaterials[i][randomMaterialIndex]; // ���ο� ���׸���� ��� ���׸����� ��ü�մϴ�.
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
            // �̸��� "Clone"���� ������ ���, "Clone"�� ������ ������ �κ��� ��ȯ
            return objectName.Substring(0, objectName.Length - 7); 
        }
        else
        {
            // "Clone"�� ���ԵǾ� ���� ������ ���� �̸� �״�� ��ȯ
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