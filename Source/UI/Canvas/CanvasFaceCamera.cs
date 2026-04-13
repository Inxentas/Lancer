using UnityEngine;

[RequireComponent(typeof(Canvas))]

public class CanvasFaceCamera : Entity
{
    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
