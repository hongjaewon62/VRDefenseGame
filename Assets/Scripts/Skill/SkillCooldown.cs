using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldown : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hideSkillButton;
    [SerializeField]
    private GameObject[] textPro;
    [SerializeField]
    private TextMeshProUGUI[] skillCooldownText;
    [SerializeField]
    private Image[] skillImage;
    private bool[] skillUse = { false, false };
    private float[] getSkillTime = { 0, 0 };

    private void Start()
    {
        for (int i = 0; i < textPro.Length; i++)
        {
            skillCooldownText[i] = textPro[i].GetComponent<TextMeshProUGUI>();
            hideSkillButton[i].SetActive(false);
        }
    }

    private void Update()
    {
        HideSkillCheck();
    }

    public void HideSkillSetting(int num)
    {
        hideSkillButton[num].SetActive(true);

        // 스테이지마다 쿨타임을 가져옴
        int level = GameManager.gameManager.GetCurrentWave();

        if (num == 0)
            getSkillTime[num] = GameManager.gameManager.logState[level].cooldown;

        else if (num == 1)
            getSkillTime[num] = GameManager.gameManager.paperState[level].cooldown;

        skillUse[num] = true;
    }

    private void HideSkillCheck()
    {
        if (skillUse[0])
        {
            StartCoroutine(SkillCooldownCoroutine(0));
        }

        if (skillUse[1])
        {
            StartCoroutine(SkillCooldownCoroutine(1));
        }
    }

    private IEnumerator SkillCooldownCoroutine(int num)
    {
        yield return null;

        if (getSkillTime[num] > 0)
        {
            getSkillTime[num] -= Time.deltaTime;

            if (getSkillTime[num] < 0)
            {
                getSkillTime[num] = 0;
                skillUse[num] = false;
                hideSkillButton[num].SetActive(false);
            }

            skillCooldownText[num].text = getSkillTime[num].ToString("00");

            // 해당 스킬에 맞는 쿨타임을 가져옴
            int level = GameManager.gameManager.GetCurrentWave();
            float time = 0.0f;
            if (num == 0)
                time = getSkillTime[num] / GameManager.gameManager.logState[level].cooldown;

            else if (num == 1)
                time = getSkillTime[num] / GameManager.gameManager.paperState[level].cooldown;

            skillImage[num].fillAmount = time;
        }
    }
    public void ReduceCooldown(float reductionAmount)
    {
        for (int i = 0; i < skillUse.Length; i++)
        {
            if (skillUse[i])
            {
                getSkillTime[i] -= reductionAmount;

                if (getSkillTime[i] < 0)
                {
                    getSkillTime[i] = 0;
                    skillUse[i] = false;
                    hideSkillButton[i].SetActive(false);
                }

                skillCooldownText[i].text = getSkillTime[i].ToString("00");

                int level = GameManager.gameManager.GetCurrentWave();
                float time = 0.0f;
                if (i == 0)
                    time = getSkillTime[i] / GameManager.gameManager.logState[level].cooldown;

                else if (i == 1)
                    time = getSkillTime[i] / GameManager.gameManager.paperState[level].cooldown;
                skillImage[i].fillAmount = time;
            }
        }
    }
}
