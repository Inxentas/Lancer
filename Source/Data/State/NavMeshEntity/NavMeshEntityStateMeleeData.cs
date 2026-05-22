using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateMeleeData", menuName = "ScriptableObjects/NavMeshEntityState/MeleeData")]

public class NavMeshEntityStateMeleeData : NavMeshEntityStateMeleeDataVirtual
{
    public override void OnEnter()
    {
        Debug.Log("MELEE ATTACK OnEnter!");
        base.OnEnter();
        this.entity.StopAgent();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.progress >= 1) { this.entity.RequestIdle(); }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
