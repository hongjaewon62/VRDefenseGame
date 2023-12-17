using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public WeaponScriptable WeaponData;
    public GameObject hitEffects;
    public Transform EffectPos;
    public AudioSource HitSound;




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.gameObject.GetComponent<Monster>().TakeDamage(WeaponData.AttackDamage, transform.position, WeaponData.KnockBack,false);
            GameObject spawnedHit = Instantiate(hitEffects, EffectPos);
            HitSound.Play();
          
            
        }
    }
}
