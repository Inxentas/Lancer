using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class NavMeshEnemy : NavMeshEntity, IEnemy
{

}
public class CombatComponent
{
    public string name = "New Combat Component";
    public int weight = 1;
    public float cooldownDuration = 1f;
    public float cooldownRemaining = 1f;
}
[Serializable]
public class RangedComponent : CombatComponent
{
    public SignatureWeaponData signatureWeaponData;
    public float animationDuration = 1.0f;
}
[Serializable]
public class MeleeComponent : CombatComponent
{
    public CompositeWeaponData compositeWeaponData;
    public float animationDuration = 1.0f;
}