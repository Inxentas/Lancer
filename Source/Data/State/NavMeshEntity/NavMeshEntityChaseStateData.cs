using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateChaseData", menuName = "ScriptableObjects/NavMeshEntityState/ChaseData")]

public class NavMeshEntityStateChaseData : NavMeshEntityStateChaseDataVirtual
{
    public float meleeDistance = 2;

    public override void OnEnter()
    {
        base.OnEnter();
        if (this.sensor.navMeshPositionTarget != Vector3.zero)
        {
            if (this.sensor.hasLos)
            {
                this.entity.MoveToTarget();
            }
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (this.sensor.navMeshPositionTarget != Vector3.zero)
        {
            if (this.sensor.hasLos)
            {
                this.entity.MoveToTarget();
            }
        }

        if (this.sensor.distance < this.meleeDistance)
        {
            this.entity.machine.SetState(this.entity.machine.melee);
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
