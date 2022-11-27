using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Scriptable/Weapon/Gun")]
public class WeaponBase : ScriptableObject
{
    public string WeaponName;
    [TextArea]
    public string Description;
    public Sprite Image;
    public float FireRate;
    public int Damage;
    public int BullperRound;
    public int MaxRound;
    public float ReloadTime;
    [Range(0f, 1f)] float CriticalRate;
}
