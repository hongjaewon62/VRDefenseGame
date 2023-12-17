using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkill : MonoBehaviour
{
    private SkillPaperTowel paperTowel;
    private SkillLog log;
    [SerializeField]
    private SkillCooldown skillCooldown;

    private void Awake()
    {
        paperTowel = GetComponent<SkillPaperTowel>();
        log = GetComponent<SkillLog>();
    }
    private void FixedUpdate()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.X))       //버튼을 눌렀을 때
        {
            if (paperTowel.isCooldown == false)
            {
                paperTowel.StartCoroutine(paperTowel.PaperTowel());
                skillCooldown.HideSkillSetting(1);
            }
        }

        if (OVRInput.GetUp(OVRInput.RawButton.Y))       //버튼을 눌렀을 때
        {
            if (log.isCooldown == false)
            {
                log.StartCoroutine(log.Log());
                skillCooldown.HideSkillSetting(0);
            }
        }
    }
}
