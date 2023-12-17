using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    public Animator headAnimator;
    public Animator bodyAnimator;
    private Animator hairAnimator;
    private Animator topAnimator;
    private Animator bottomAnimator;
    private Animator shoeAnimator;

    public void SetWalkSpeed(float speed)
    {
        headAnimator.SetFloat("moveSpeed", speed);
        bodyAnimator.SetFloat("moveSpeed", speed);
        hairAnimator.SetFloat("moveSpeed", speed);
        if (topAnimator != null)
        {
            topAnimator.SetFloat("moveSpeed", speed);
            bottomAnimator.SetFloat("moveSpeed", speed);
            shoeAnimator.SetFloat("moveSpeed", speed);
        }
    }
    public void SetKnockBack()
    {
        headAnimator.SetTrigger("knockBack");
        bodyAnimator.SetTrigger("knockBack");
        hairAnimator.SetTrigger("knockBack");
        if( topAnimator != null)
        {
            topAnimator.SetTrigger("knockBack");
            bottomAnimator.SetTrigger("knockBack");
            shoeAnimator.SetTrigger("knockBack");
        }



    }

    public void SetIsLogHit()
    {
        headAnimator.StopPlayback();
        headAnimator.SetTrigger("isLogHit");
        bodyAnimator.SetTrigger("isLogHit");
        hairAnimator.SetTrigger("isLogHit");
        if (topAnimator != null)
        {
            topAnimator.SetTrigger("isLogHit");
            bottomAnimator.SetTrigger("isLogHit");
            shoeAnimator.SetTrigger("isLogHit");
        }
    }

    public void SetisTrapped(bool input)
    {
        headAnimator.SetBool("isTrapped", input);
        bodyAnimator.SetBool("isTrapped", input);
        hairAnimator.SetBool("isTrapped", input);
        if (topAnimator != null)
        {
            topAnimator.SetBool("isTrapped", input);
            bottomAnimator.SetBool("isTrapped", input);
            shoeAnimator.SetBool("isTrapped", input);
        }

    }

    public void SetAivityKitty(bool input)
    {
        headAnimator.SetBool("isTrapped", input);
        bodyAnimator.SetBool("isTrapped", input);
        hairAnimator.SetBool("isTrapped", input);
        if (topAnimator != null)
        {
            topAnimator.SetBool("isTrapped", input);
            bottomAnimator.SetBool("isTrapped", input);
            shoeAnimator.SetBool("isTrapped", input);
        }
    }



    public void SetAnimator(int number, Animator inputAnimator)
    {
        if (number == 0)
        {
            hairAnimator = inputAnimator;
        }
        else if(number == 1)
        {
            topAnimator = inputAnimator;
        }
        else if (number == 2)
        {
            bottomAnimator = inputAnimator;
        }
        else if (number == 3)
        {
            shoeAnimator = inputAnimator;
        }
    }
}
