using UnityEngine;

/**
 * We define a seperate subclass to use as the required class in the editor.
 * We inherit from NavMeshEntityStateData to gain access to the entity/sensor.
 */
public class NavMeshEntityStateIdleDataVirtual : NavMeshEntityStateData
{
    public override void OnEnter()
    {
        base.OnEnter();
        this.entity.StopAgent(); /* TODO: may no longer be needed? */
    }
}
