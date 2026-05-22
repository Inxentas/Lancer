using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateMeleeData", menuName = "ScriptableObjects/NavMeshEntityState/MeleeData")]

public class NavMeshEntityStateMeleeData : NavMeshEntityStateMeleeDataVirtual
{
    public override void OnEnter()
    {
        Debug.Log("MELEE STATE OnEnter!");
        base.OnEnter();
        this.entity.StopAgent();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.progress >= 1) { this.entity.RequestChase(); }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
