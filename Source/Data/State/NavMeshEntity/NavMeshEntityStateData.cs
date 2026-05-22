using UnityEngine;

public class NavMeshEntityStateData : ScriptableObject
{
    protected NavMeshEntity entity;
    protected ISensor sensor;

    public float duration = 1.0f;
    public float timer = 0.0f;
    public float progress = 0.0f;

    virtual public void Initialize(NavMeshEntity entity)
    {
        this.entity = entity;
        this.sensor = entity.GetISensor();
    }
    public virtual void OnEnter() { Debug.Log(this.entity + " entered " + this.name);  }
    public virtual void OnUpdate() 
    {
        this.timer += (Time.deltaTime * Time.timeScale);
        this.progress = this.timer / this.duration;
    }
    public virtual void OnExit() { this.Clear(); }
    public virtual void Clear() 
    { 
        //
    }
}
