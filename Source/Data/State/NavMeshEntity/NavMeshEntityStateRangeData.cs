using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateRangeData", menuName = "ScriptableObjects/NavMeshEntityState/RangeData")]

public class NavMeshEntityStateRangeData : NavMeshEntityStateRangeDataVirtual
{
    public override void OnEnter()
    {
        Debug.Log("RANGED ATTACK OnEnter!");
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
