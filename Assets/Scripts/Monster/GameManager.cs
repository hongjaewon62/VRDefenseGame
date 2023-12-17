using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int maxHealth = 45; // �ִ� ü��
    public int currentHealth; // ���� ü��
    public int currency; // ��ȭ
    public int weaponLevel = 1;
    public string weaponName = "None";
    public int[] itemNum = new int[] { 0, 0, 0, 0 }; 
    public int score;
    public string studentId;
    private int currentMonsterCount = 0;
    private bool spawnFinished = false;
    private int currentWave = 0;
    private bool weaponSwapEnabled = true;
    private AudioSource audioSource;
    public AudioClip[] clip;
    public SkillState[] logState;           // �볪�� ��ų�� �������� �� ����
    public SkillState[] paperState;         // ���� ��ų�� �������� �� ����
    private List<string> facialName = new List<string> { "Base1", "Base2", "Base3", "Pain1", "Pain2", "Smile", "Embarrassed" };
    private List<string> playerFacialName = new List<string> { "Base1", "Base2", "Base3", "Pain1", "Pain2", "Smile", "Embarrassed","Angry", "Wink1", "Wink2" };
    private List<string> monsterName = new List<string> { "Normal", "Tired", "Speed", "Tanker" };
    private List<int> playerEyebrowTimeList = new List<int> { 0, 0, 0, 2, 2, 0, 1, 1, 1, 1 };
    private List<float> eyebrowTimeList = new List<float> { 0.0f, 0.0f, 0.0f, 2.0f, 2.0f, 0.0f, 1.0f };
    private List<float> playerMouthTimeList = new List<float> { 0.0f, 0.0f, 3.0f, 7.0f, 8.0f, 4.0f, 5.0f, 6.0f, 5.0f, 5.0f };
    private List<float> mouthTimeList = new List<float> { 0.0f, 1.0f, 3.0f, 7.0f, 8.0f, 4.0f, 5.0f };
    public List<FacialExpressionData> PlayerFacialData = new List<FacialExpressionData>();
    public List<FacialExpressionData> normalFacialData = new List<FacialExpressionData>();
    public List<FacialExpressionData> tiredFacialData = new List<FacialExpressionData>();
    public List<FacialExpressionData> speedFacialData = new List<FacialExpressionData>();
    public List<FacialExpressionData> tankerFacialData = new List<FacialExpressionData>();
    private void Awake()
    {

        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();


        for (int i = 0; i < playerFacialName.Count; i++)
        {
            float eyeNumber = 0.1f + i;
            FacialExpressionData expression = new FacialExpressionData
            {
                expressionName = playerFacialName[i],
                eyeAnimationTime = eyeNumber + i,
                eyebrowAnimationTime = playerEyebrowTimeList[i],
                mouthAnimationTime = playerMouthTimeList[i]
            };
            PlayerFacialData.Add(expression);
        }
        for (int i = 0; i < monsterName.Count; i++)
        {
            float eyeNumber = 10.1f + i * 7;
            // ǥ�� �����͸� �� ���� ����Ʈ�� �߰��մϴ�.
            for (int k = 0; k < facialName.Count; k++)
            {
                FacialExpressionData expression = new FacialExpressionData
                {
                    expressionName = facialName[k],
                    eyeAnimationTime = eyeNumber+k,
                    eyebrowAnimationTime = eyebrowTimeList[k],
                    mouthAnimationTime = mouthTimeList[k]
                };

                // �� ���Ϳ� �´� ����Ʈ�� �߰��մϴ�.
                if (i == 0)
                {
                    normalFacialData.Add(expression);
                }
                else if (i == 1)
                {
                    tiredFacialData.Add(expression);
                }
                else if (i == 2)
                {
                    speedFacialData.Add(expression);
                }
                else
                {
                    tankerFacialData.Add(expression);
                }
            }
        }
    }



    private void Start()
    {
        currentHealth = maxHealth;  // ���� �� ��ü ü������ �ʱ�ȭ
        currency = 100;               // ���� �� ��ȭ�� �ʱ�ȭ

        // UiManager�� �����ϴ��� Ȯ�� ��
        if (UiManager.uiManager == null)
        {
            Debug.Assert(false, "Error (UiManager is Null) : UiManager�� ã�� �� �����ϴ�.");
            return;
        }

        UiManager.uiManager.UpdateHealthText(currentHealth, maxHealth);
        UiManager.uiManager.UpdateCurrencyText(currency);
    }

    public void DecreaseHealth(int amount) // ü�� ���� �޼���
    {
        currentHealth -= amount;
        if (currentHealth <= 0) // ü�� 0 �Ǹ� ���� ����
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Cine_E1_Hospital");
            Score.score.AddHighScoreEntry(score, studentId);
        }
        
    }


    public void AddCurrency(int amount)   // ��ȭ �߰�
    {
        currency += amount;
    }


    public bool SpendCurrency(int amount)   // ��ȭ ����
    {
        if (currency >= amount) // �� �ִ� ���
        {
            currency -= amount;
            return true;
        }
        else // �� ������ ���
        {
            return false;
        }
    }

    public void AddMonster()
    {
        currentMonsterCount++;
    }
    public void DieMonster()
    {
        

        currentMonsterCount--;
        if (spawnFinished == true && currentMonsterCount == 0 && currentWave <= 2)
        {
            StageClear();
        }
        else if (spawnFinished == true && currentMonsterCount == 0 && currentWave == 3)
        {
            GameClear(currentHealth);
        }
    }
    public void FinishSpawn()
    {
        spawnFinished = true;
    }

    public void StageClear()
    {
        // �������� Ŭ���� ���� �� �۵��� �͵� �ֱ� ��ų ��ȭ, �̵� ��Ż ����, �� �� ����Ʈ ��
        
        GameManager.gameManager.ChangeWeaponSwapEnabled();
        GameObject clearingHub = GameObject.Find("ClearingHub");
        clearingHub.transform.GetChild(0).gameObject.SetActive(true);
        spawnFinished = false;
    }

    private void GameClear(int health)
    {
        // ���� �ó׸�ƽ ��� ü���̳� ������ ����

        if (currentHealth>=40)
            SceneManager.LoadScene("Cine_E2_Graduation");
        else if(currentHealth < 40 && currentHealth >=15)
            SceneManager.LoadScene("Cine_E3_Take_a_Bus");
        else
            SceneManager.LoadScene("Cine_E4_Refusal");
        Score.score.AddHighScoreEntry(score, studentId);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
    public void PlusCurrentWave()
    {
        currentWave++;
    }

    public void ClickButtonSound()
    {
        audioSource.clip = clip[0];
        audioSource.Play();
    }

    public struct FacialExpressionData
    {
        public string expressionName;        // ǥ�� �̸� 
        public float eyeAnimationTime;       // �� �ִϸ��̼� �ð� (��)
        public float eyebrowAnimationTime;   // ���� �ִϸ��̼� �ð� (��)
        public float mouthAnimationTime;     // �� �ִϸ��̼� �ð�
    }

    public void ChangeWeaponSwapEnabled() // ���� ������ ��ü ���� ���� ����
    {
        weaponSwapEnabled = !weaponSwapEnabled;
    }
    public bool GetWeaponSwapEnabled()
    {
        return weaponSwapEnabled;
    }
}
