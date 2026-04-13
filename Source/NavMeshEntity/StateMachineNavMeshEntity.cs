using UnityEngine;

public class StateMachineNavMeshEntity : StateMachine
{
    public NavMeshEntity entity;
    public ISensor sensor;

    public StateNavMeshEntityIdle idle;
    public StateNavMeshEntityChase chase;
    public StateNavMeshEntityMelee melee;

    public StateMachineNavMeshEntity(NavMeshEntity entity)
    {
        // entity
        this.entity = entity;
        this.sensor = entity.GetISensor();
        // states
        this.idle = new StateNavMeshEntityIdle(this);
        this.chase = new StateNavMeshEntityChase(this);
        this.melee = new StateNavMeshEntityMelee(this);
    }
}
public class StateNavMeshEntity : State
{
    public StateMachineNavMeshEntity stateMachine { get { return this.machine as StateMachineNavMeshEntity; } }
    public NavMeshEntity entity { get { return this.stateMachine.entity; } }
    public ISensor sensor { get { return this.stateMachine.sensor; } }
    public StateNavMeshEntity(StateMachineNavMeshEntity machine) : base(machine) { }
}
public class StateNavMeshEntityIdle : StateNavMeshEntity
{
    public StateNavMeshEntityIdle(StateMachineNavMeshEntity machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        entity.idle.OnEnter();
    }
    public override void OnStateUpdate()
    {
        base.OnStateEnter();
        entity.idle.OnUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        entity.idle.OnExit();
    }
}
public class StateNavMeshEntityChase : StateNavMeshEntity
{
    public StateNavMeshEntityChase(StateMachineNavMeshEntity machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        entity.chase.OnEnter();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        entity.chase.OnUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        entity.chase.OnExit();
    }
}
public class StateNavMeshEntityMelee : StateNavMeshEntity
{
    public StateNavMeshEntityMelee(StateMachineNavMeshEntity machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        entity.melee.OnUpdate();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        entity.melee.OnUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        entity.melee.OnExit();
    }
}