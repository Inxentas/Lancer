using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateIdleData", menuName = "ScriptableObjects/NavMeshEntityState/IdleData")]

public class NavMeshEntityStateIdleData : NavMeshEntityStateIdleDataVirtual
{
    public override void OnEnter()
    {
        base.OnEnter();
        this.entity.StopAgent();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.sensor.hasLos)
        {
            if (this.sensor.distance > 8.0f)
            {
                this.entity.machine.SetState(this.entity.machine.chase);
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
