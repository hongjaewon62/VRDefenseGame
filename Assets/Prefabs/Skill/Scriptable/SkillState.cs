using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skill", fileName = "SkillData")]

public class SkillState : ScriptableObject
{
    public float cooldown;
    public float force;

    public int damage;

    public float knockback;
    public float objSize;
}
