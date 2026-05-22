using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshEntityStateIdleData", menuName = "ScriptableObjects/NavMeshEntityState/IdleData")]

public class NavMeshEntityStateIdleData : NavMeshEntityStateIdleDataVirtual
{
    public override void OnEnter()
    {
        Debug.Log("IDLE STATE OnEnter!");
        base.OnEnter();
        this.entity.StopAgent();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (this.sensor.hasLos)
        {
            if(this.sensor.distance < 50f) // detection range
            {
                if (this.sensor.distance > this.entity.getMaxRangedComponentRange())
                {
                    this.entity.RequestChase();
                }
                else if (this.sensor.distance < this.entity.getMaxRangedComponentRange())
                {
                    this.entity.RequestRangedAttack();
                }
                else if (this.sensor.distance < 3.0f)
                {
                    //this.entity.RequestMeleeAttack();
                }
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
