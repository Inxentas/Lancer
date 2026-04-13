using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateMeleeData", menuName = "ScriptableObjects/NavMeshEntityState/MeleeData")]

public class NavMeshEntityStateMeleeData : NavMeshEntityStateMeleeDataVirtual
{
    public override void OnEnter()
    {
        //Debug.Log("Actual melee enter.");
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //Debug.Log("ACTUAL MELEE UPDATE!");
        // TODO: Write basic attack logic.
        this.entity.machine.SetState(this.entity.machine.idle);
    }
    public override void OnExit()
    {
        base.OnExit();
        //Debug.Log("ACTUAL MELEE EXIT!");
    }
}
