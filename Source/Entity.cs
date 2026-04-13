using UnityEngine;

public class Entity : MonoBehaviour
{
    virtual protected void OnAwake() { }
    virtual protected void OnStart() { }
    virtual protected void OnUpdate() { }

    private void Awake()
    {
        this.OnAwake();
    }
    private void Start()
    {
        this.OnStart();
    }
    private void Update()
    {
        this.OnUpdate();
    }
}
