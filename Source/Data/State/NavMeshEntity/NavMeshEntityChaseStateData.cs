using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateChaseData", menuName = "ScriptableObjects/NavMeshEntityState/ChaseData")]

public class NavMeshEntityStateChaseData : NavMeshEntityStateChaseDataVirtual
{
    public float meleeDistance = 2;

    public override void OnEnter()
    {
        Debug.Log("CHASE STATE OnEnter!");
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

        if (this.sensor.hasLos)
        {
            if (this.sensor.distance > this.meleeDistance && this.sensor.distance < this.entity.getMaxRangedComponentRange())
            {
                this.entity.RequestRangedAttack();
            }
            else if (this.sensor.distance < this.meleeDistance) // perhaps add range clause too?
            {
                this.entity.RequestMeleeAttack();
            }
            else
            {
                this.entity.MoveToTarget();
            }
        }
        else
        {
            this.entity.RequestIdle();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
