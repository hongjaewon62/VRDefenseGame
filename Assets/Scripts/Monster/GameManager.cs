using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int maxHealth = 45; // 최대 체력
    public int currentHealth; // 현재 체력
    public int currency; // 재화
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
    public SkillState[] logState;           // 통나무 스킬의 스테이지 당 정보
    public SkillState[] paperState;         // 휴지 스킬의 스테이지 당 정보
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
            // 표정 데이터를 각 몬스터 리스트에 추가합니다.
            for (int k = 0; k < facialName.Count; k++)
            {
                FacialExpressionData expression = new FacialExpressionData
                {
                    expressionName = facialName[k],
                    eyeAnimationTime = eyeNumber+k,
                    eyebrowAnimationTime = eyebrowTimeList[k],
                    mouthAnimationTime = mouthTimeList[k]
                };

                // 각 몬스터에 맞는 리스트에 추가합니다.
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
        currentHealth = maxHealth;  // 시작 시 전체 체력으로 초기화
        currency = 100;               // 시작 시 재화를 초기화

        // UiManager가 존재하는지 확인 함
        if (UiManager.uiManager == null)
        {
            Debug.Assert(false, "Error (UiManager is Null) : UiManager를 찾을 수 없습니다.");
            return;
        }

        UiManager.uiManager.UpdateHealthText(currentHealth, maxHealth);
        UiManager.uiManager.UpdateCurrencyText(currency);
    }

    public void DecreaseHealth(int amount) // 체력 감소 메서드
    {
        currentHealth -= amount;
        if (currentHealth <= 0) // 체력 0 되면 게임 오버
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Cine_E1_Hospital");
            Score.score.AddHighScoreEntry(score, studentId);
        }
        
    }


    public void AddCurrency(int amount)   // 재화 추가
    {
        currency += amount;
    }


    public bool SpendCurrency(int amount)   // 재화 차감
    {
        if (currency >= amount) // 돈 있는 경우
        {
            currency -= amount;
            return true;
        }
        else // 돈 부족한 경우
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
        // 스테이지 클리어 했을 때 작동할 것들 넣기 스킬 강화, 이동 포탈 생성, 그 외 이펙트 등
        
        GameManager.gameManager.ChangeWeaponSwapEnabled();
        GameObject clearingHub = GameObject.Find("ClearingHub");
        clearingHub.transform.GetChild(0).gameObject.SetActive(true);
        spawnFinished = false;
    }

    private void GameClear(int health)
    {
        // 엔딩 시네마틱 재생 체력이나 점수에 따라

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
        public string expressionName;        // 표정 이름 
        public float eyeAnimationTime;       // 눈 애니메이션 시간 (초)
        public float eyebrowAnimationTime;   // 눈썹 애니메이션 시간 (초)
        public float mouthAnimationTime;     // 입 애니메이션 시간
    }

    public void ChangeWeaponSwapEnabled() // 무기 버려서 교체 가능 여부 변경
    {
        weaponSwapEnabled = !weaponSwapEnabled;
    }
    public bool GetWeaponSwapEnabled()
    {
        return weaponSwapEnabled;
    }
}
