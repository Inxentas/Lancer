using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class NavMeshEnemy : NavMeshEntity, IEnemy
{
    protected override void OnUpdate()
    {
        base.OnUpdate();

        foreach(RangedComponent r in rangedComponents)
        {
            r.cooldownRemaining -= (Time.deltaTime * Time.timeScale);
            r.cooldownRemaining = Mathf.Clamp(r.cooldownRemaining, 0, r.cooldownDuration);
        }
        foreach (MeleeComponent m in meleeComponents)
        {
            m.cooldownRemaining -= (Time.deltaTime * Time.timeScale);
            m.cooldownRemaining = Mathf.Clamp(m.cooldownRemaining, 0, m.cooldownDuration);
        }
    }
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
}
[Serializable]
public class MeleeComponent : CombatComponent
{
    public CompositeWeaponData SignatureWeaponData;
    
}