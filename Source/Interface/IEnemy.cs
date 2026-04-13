using UnityEngine;

public interface IEnemy
{
    public ISensor GetISensor();
    public void Teleport(Vector3 position);
}
