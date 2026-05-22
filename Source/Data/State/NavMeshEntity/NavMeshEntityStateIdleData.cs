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
            if (this.sensor.distance < 20.0f)
            {
                this.entity.RequestChase();
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
