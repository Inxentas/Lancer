using UnityEngine;
using UnityEngine.Events;

public class GameTriggerEvent : GameTrigger
{
    public UnityEvent onActivate;

    protected override void Activate()
    {
        onActivate.Invoke();
        base.Activate();
    }
    public void Test()
    {
        Debug.Log("Test");
    }
}
