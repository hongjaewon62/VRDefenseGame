using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapon", fileName = "WeaponData")]

public class WeaponScriptable : ScriptableObject
{
    public int AttackDamage = 0;
    public float KnockBack = 0;
    public float AttackSpeed = 0;
}