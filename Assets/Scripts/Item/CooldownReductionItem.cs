using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownReductionItem : MonoBehaviour
{
    [SerializeField]
    private float cooldownReductionAmount = 2.0f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            SkillCooldown skillCooldown = other.GetComponent<SkillCooldown>();
            if (skillCooldown != null)
            {
                skillCooldown.ReduceCooldown(cooldownReductionAmount);
                Destroy(gameObject); 
            }
        }
    }
}
