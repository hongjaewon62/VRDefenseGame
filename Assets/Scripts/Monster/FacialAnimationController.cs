using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialAnimationController : MonoBehaviour
{
    public Animator eyeAnimator;
    public Animator eyebrowAnimator;
    public Animator mouthAnimator;
    public string eyeAnimationStateName = "Facial_Anim_eye";
    public string eyebrowAnimationStateName = "Facial_Anim_eyebrow";
    public string mouthAnimationStateName = "Facial_Anim_mouth";
    private float eyeAnimatorLength = 0;
    private float eyebrowAnimatorLength = 0;
    private float mouthAnimatorLength = 0;

    public void SetFacial(string monsterName, int facialNumber)
    {
        if (eyeAnimatorLength == 0) {
            eyeAnimatorLength = eyeAnimator.GetCurrentAnimatorStateInfo(0).length;
            eyebrowAnimatorLength = eyebrowAnimator.GetCurrentAnimatorStateInfo(0).length;
            mouthAnimatorLength = mouthAnimator.GetCurrentAnimatorStateInfo(0).length;
        }
        if (facialNumber >= 0 && facialNumber < 7)
        {
            eyeAnimator.speed = 0.0166666666666667f;
            eyebrowAnimator.speed = 0.0166666666666667f;
            mouthAnimator.speed = 0.0166666666666667f;
            if (monsterName == "NormalMonster")
            {
                eyeAnimator.Play(eyeAnimationStateName, 0, GameManager.gameManager.normalFacialData[facialNumber].eyeAnimationTime / eyeAnimatorLength);
                eyebrowAnimator.Play(eyebrowAnimationStateName, 0, GameManager.gameManager.normalFacialData[facialNumber].eyebrowAnimationTime / eyebrowAnimatorLength);
                mouthAnimator.Play(mouthAnimationStateName, 0, GameManager.gameManager.normalFacialData[facialNumber].mouthAnimationTime / mouthAnimatorLength);

            }
            if (monsterName == "TiredMonster")
            {
                eyeAnimator.Play(eyeAnimationStateName, 0, GameManager.gameManager.tiredFacialData[facialNumber].eyeAnimationTime / eyeAnimatorLength);
                eyebrowAnimator.Play(eyebrowAnimationStateName, 0, GameManager.gameManager.tiredFacialData[facialNumber].eyebrowAnimationTime / eyebrowAnimatorLength);
                mouthAnimator.Play(mouthAnimationStateName, 0, GameManager.gameManager.tiredFacialData[facialNumber].mouthAnimationTime / mouthAnimatorLength);
            }
            if (monsterName == "SpeedMonster")
            {
                eyeAnimator.Play(eyeAnimationStateName, 0, GameManager.gameManager.speedFacialData[facialNumber].eyeAnimationTime / eyeAnimatorLength);
                eyebrowAnimator.Play(eyebrowAnimationStateName, 0, GameManager.gameManager.speedFacialData[facialNumber].eyebrowAnimationTime / eyebrowAnimatorLength);
                mouthAnimator.Play(mouthAnimationStateName, 0, GameManager.gameManager.speedFacialData[facialNumber].mouthAnimationTime / mouthAnimatorLength);
            }
            if (monsterName == "TankerMonster")
            {
                eyeAnimator.Play(eyeAnimationStateName, 0, GameManager.gameManager.tankerFacialData[facialNumber].eyeAnimationTime / eyeAnimatorLength);
                eyebrowAnimator.Play(eyebrowAnimationStateName, 0, GameManager.gameManager.tankerFacialData[facialNumber].eyebrowAnimationTime / eyebrowAnimatorLength);
                mouthAnimator.Play(mouthAnimationStateName, 0, GameManager.gameManager.tankerFacialData[facialNumber].mouthAnimationTime / mouthAnimatorLength);

            }



            eyeAnimator.speed = 0f;
            eyebrowAnimator.speed = 0f;
            mouthAnimator.speed = 0f;
        }
    }
}
