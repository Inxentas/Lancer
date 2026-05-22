using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Sensor))]

public class NavMeshEntity : Entity, INavMeshEntity
{
    private NavMeshAgent _agent;
    private Sensor _sensor;
    private StateMachineNavMeshEntity _machine;

    // getters
    public NavMeshAgent agent { get { return this._agent; } }
    public Sensor sensor { get { return this._sensor; } }
    public StateMachineNavMeshEntity machine { get { return this._machine; } }

    // fields
    [SerializeField] private Animator animator;                                     /* Set in inspector */
    [SerializeField] private NavMeshEntityStateIdleDataVirtual idleData;            /* Set in inspector */
    [SerializeField] private NavMeshEntityStateChaseDataVirtual chaseData;          /* Set in inspector */
    [SerializeField] private NavMeshEntityStateMeleeDataVirtual meleeData;          /* Set in inspector */
    [SerializeField] private NavMeshEntityStateRangeDataVirtual rangeData;          /* Set in inspector */
    public List<RangedComponent> rangedComponents = new List<RangedComponent>();    /* Set in inspector */
    public List<MeleeComponent> meleeComponents = new List<MeleeComponent>();       /* Set in inspector */
    // states
    private NavMeshEntityStateIdleDataVirtual _idleDataInstance;
    private NavMeshEntityStateChaseDataVirtual _chaseDataInstance;
    private NavMeshEntityStateMeleeDataVirtual _meleeDataInstance;
    private NavMeshEntityStateRangeDataVirtual _rangeDataInstance;
    // getters
    public NavMeshEntityStateIdleDataVirtual idle { get { return this._idleDataInstance; } }
    public NavMeshEntityStateChaseDataVirtual chase { get { return this._chaseDataInstance; } }
    public NavMeshEntityStateMeleeDataVirtual melee { get { return this._meleeDataInstance; } }
    public NavMeshEntityStateRangeDataVirtual range { get { return this._rangeDataInstance; } }

    // entity
    override protected void OnAwake()
    {
        // set components
        this._agent = this.GetComponent<NavMeshAgent>();
        this._sensor = this.GetComponent<Sensor>();

        // copy over all SO data so we have unique instances.
        this._idleDataInstance = Instantiate(this.idleData);
        this._chaseDataInstance = Instantiate(this.chaseData);
        this._meleeDataInstance = Instantiate(this.meleeData);
        this._rangeDataInstance = Instantiate(this.rangeData);

        // initialize these states with their dependencies.
        this._idleDataInstance.Initialize(this);
        this._chaseDataInstance.Initialize(this);
        this._meleeDataInstance.Initialize(this);
        this._rangeDataInstance.Initialize(this);

        // initialize the actual state machine.
        this._machine = new StateMachineNavMeshEntity(this);
        this._machine.SetState(this._machine.idle);
    }
    override protected void OnStart()
    {
        //
    }
    override protected void OnUpdate()
    {
        if (this._machine != null)
        {
            this._machine.Run();
        }

        //NavMeshPath path = new NavMeshPath();
        //agent.CalculatePath(sensor.navMeshPositionTarget, path);
        //Debug.Log(path.status);
        //Debug.Log(agent.isOnNavMesh);
    }

    // agent
    public bool validPath()
    {
        bool validPath = false;
        NavMeshPath path = new NavMeshPath();
        this.agent.CalculatePath(sensor.navMeshPositionTarget, path);
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            if (agent.isOnNavMesh)
            {
                validPath = true;
            }
        }
        return validPath;
    }
    public void StopAgent()
    {
        if (this._agent)
        {
            this._agent.isStopped = true;
            this._agent.velocity = new Vector3(0, 0, 0);
        }
        if (this.animator)
        {
            this.animator.SetFloat("moveBlend", 0);
        }
    }
    public void Teleport(Vector3 position)
    {
        this._agent.isStopped = true;
        this.transform.position = position;
        this._agent.nextPosition = position;
    }
    public void DestroyEntity()
    {
        Destroy(this.gameObject);
    }
    public void MoveToTarget()
    {
        if (this.validPath() && this.sensor.hasNavMeshPositionTarget)
        {
            this.agent.isStopped = false;
            this.agent.SetDestination(this.sensor.navMeshPositionTarget);
            if (this.animator)
            {
                this.animator.SetFloat("moveBlend", 0.5f);
            }
        } 
    }
    public void RotateToTarget(float lerp)
    {
        Vector3 direction = sensor.direction;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this._agent.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerp);
        }
    }

    // sensor
    public ISensor GetISensor()
    {
        return this.sensor as ISensor;
    }

    // combat components
    public List<RangedComponent> RangedComponentsWithin(float distance)
    {
        return this.RangedComponentsCooledDown().Where(c => c.signatureWeaponData.range <= distance).ToList();
    }
    public List<RangedComponent> RangedComponentsCooledDown()
    {
        return this.rangedComponents.Where(c => c.cooldownRemaining == 0).ToList();
    }
    public float getMaxRangedComponentRange()
    {
        return rangedComponents.Max(c => c.signatureWeaponData.range);
    }
    public RangedComponent WeightRangedComponents(List<RangedComponent> components)
    {
        float totalWeight = 0f;
        foreach (RangedComponent r in components) { totalWeight += r.weight; }
        float random = UnityEngine.Random.value * totalWeight;
        foreach (var comp in components)
        {
            random -= comp.weight;
            if (random <= 0f)
            {
                return comp;
            }
        }
        return components[^1];
    }
    public MeleeComponent WeightMeleeComponents(List<MeleeComponent> components)
    {
        float totalWeight = 0f;
        foreach (MeleeComponent m in components) { totalWeight += m.weight; }
        float random = UnityEngine.Random.value * totalWeight;
        foreach (var comp in components)
        {
            random -= comp.weight;
            if (random <= 0f)
            {
                return comp;
            }
        }
        return components[^1];
    }


    public void RequestIdle()
    {
        machine.SetState(this.machine.idle);
    }
    public void RequestChase()
    {
        machine.SetState(this.machine.chase);
    }
    public void RequestRangedAttack()
    {
        List<RangedComponent> valid = this.RangedComponentsWithin(this.sensor.distance);
        RangedComponent weighted = this.WeightRangedComponents(valid);
        
        Debug.Log(weighted.signatureWeaponData.displayName + " being fired!");
        weighted.cooldownRemaining = weighted.cooldownDuration;

        machine.SetState(this.machine.range);
    }
    public void RequestMeleeAttack()
    {
        machine.SetState(this.machine.melee);
    }

    //public override void Harm()
    //{
    //this.RespondToSignal(); // because we are not so stupid as to allow ourselves to be aware of attacks!
    //GameEvents.navMeshEntityHarm.Invoke(this); // and everyone else needs to know as well.
    //base.Harm();
    //}

    //override public void Kill()
    //{
    //this.StopAgent();
    //GameEvents.navMeshEntityKill.Invoke(this);
    //}

    // events
    private void OnNavMeshEntityHarm(NavMeshEntity entity)
    {
        
        //
    }

    private void OnNavMeshEntityKill(NavMeshEntity entity)
    {
        //
    }
}
