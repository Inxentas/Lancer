using UnityEngine;

public interface INavMeshEntity
{
    // agent
    public void StopAgent();
    public void Teleport(Vector3 position);
    public void DestroyEntity();
    public void MoveToTarget();
    public void RotateToTarget(float lerp);

    //sensor
    public ISensor GetISensor();
}
