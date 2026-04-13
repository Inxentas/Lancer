using System.Collections.Generic;
using UnityEngine;

public interface ISensor
{
    // 3D methods
    public float GetAngleTo(Vector3 p);
    public float GetDistanceTo(Vector3 p);
    public Vector3 GetDirectionTo(Vector3 p);
    public Vector3 GetNavPositionSelf();
    public Vector3 GetNavPositionTarget();
    public Vector3 GetNavPositionTowards(float distance);

    // 2D methods
    public float GetAngleTo2D(Vector3 t);
    public float GetDistanceTo2D(Vector3 t);
    public Vector3 GetDirectionTo2D(Vector3 t);

    // getters
    public bool hasLos { get; } 
    public float angle { get; } 
    public float distance { get; } 
    public Vector3 direction { get; } 
    public Vector3 navMeshPositionSelf { get; } 
    public Vector3 navMeshPositionTarget { get; }
    public float angle2D { get; }
    public float distance2D { get; }
    public Vector3 direction2D { get; }

    // Radial detection
    public List<Vector3> GetNavMeshPoints(Vector3 position, float navpointRadius, int navPointQuantity);
    public List<Vector3> SetNavMeshPoints(Vector3 position, float navpointRadius, int navPointQuantity);
}
