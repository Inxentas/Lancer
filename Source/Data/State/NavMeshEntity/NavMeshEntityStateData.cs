using UnityEngine;

public class NavMeshEntityStateData : ScriptableObject
{
    protected NavMeshEntity entity;
    protected ISensor sensor;

    virtual public void Initialize(NavMeshEntity entity)
    {
        this.entity = entity;
        this.sensor = entity.GetISensor();
    }
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { this.Clear(); }
    public virtual void Clear() 
    { 
        //
    }
}
