using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateRangeData", menuName = "ScriptableObjects/NavMeshEntityState/RangeData")]

public class NavMeshEntityStateRangeData : NavMeshEntityStateRangeDataVirtual
{
    public override void OnEnter()
    {
        Debug.Log("RANGED ATTACK!");
        base.OnEnter();
        this.entity.StopAgent();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        // TODO: Write basic attack logic.
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
