using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour, ISensor
{
    public bool calculateOnUpdate;

    public Transform target;
    public LayerMask layerMaskLOS;
    public LayerMask layerMaskNavMesh;

    [Header("Line of Sight")]
    [SerializeField] bool _hasLOS;

    [Header("3D Calculations")]
    [SerializeField] protected float _angle;
    [SerializeField] protected float _distance;
    [SerializeField] Vector3 _direction;

    [Header("2D Calculations")]
    [SerializeField] protected float _angle2D;
    [SerializeField] protected float _distance2D;
    [SerializeField] Vector3 _direction2D;

    [Header("NavMeshSurface Positions")]
    [SerializeField] protected bool _hasNavMeshPositionSelf;
    [SerializeField] protected bool _hasNavMeshPositionTarget;
    [SerializeField] protected Vector3 _navMeshPositionSelf;
    [SerializeField] protected Vector3 _navMeshPositionTarget;

    [Header("Radial NavMeshSurface Positions")]
    [SerializeField] public float navpointRadius = 6;
    [SerializeField] public int navPointQuantity = 8;
    [SerializeField] public float navpointOffset = 2;
    [SerializeField] protected List<Vector3> _navMeshPoints = new List<Vector3>();

    //public Vector3 NavMeshPositionSelf { get { return _navMeshPositionSelf; } }
    //public bool HasNavMeshPositionSelf { get { return _hasNavMeshPositionSelf; } }
    //public Vector3 NavMeshPositionTarget { get { return _navMeshPositionTarget; } }
    public bool hasNavMeshPositionTarget { get { return _hasNavMeshPositionTarget; } }

    public void CalculateAll()
    {
        // LOS
        this._hasLOS = RayCastLOS();
        // 3D
        this._angle = this.angle;
        this._distance = this.distance;
        this._direction = this.direction;
        // 2D
        this._angle2D = this.angle2D;
        this._distance2D = this.distance2D;
        this._direction2D = this.direction2D;
        // NavMesh
        SetNavPositionSelf();
        SetNavPositionTarget();
        SetNavMeshPoints(navMeshPositionTarget, navpointRadius, navPointQuantity);
    }

    private void Awake()
    {
        this.target = GameObject.FindFirstObjectByType<Character>().gameObject.transform;
    }

    private void Update()
    {
        if (this.calculateOnUpdate && this.target != null) { CalculateAll(); }
    }

    // getters
    public bool hasLos { get { return RayCastLOS(); }  }
    public float angle { get { return GetAngleTo(target.position); } }
    public float distance { get { return GetDistanceTo(target.position); } }
    public Vector3 direction { get { return GetDirectionTo(target.position); } }
    public Vector3 navMeshPositionSelf { get { return GetNavPositionSelf(); } }
    public Vector3 navMeshPositionTarget { get { return GetNavPositionTarget(); } }
    public float angle2D { get { return GetAngleTo2D(target.position); } }
    public float distance2D { get { return GetDistanceTo2D(target.position); } }
    public Vector3 direction2D { get { return GetDirectionTo2D(target.position); } }

    // 3D methods
    public float GetAngleTo(Vector3 p)
    {
        return Vector3.Angle(transform.forward, GetDirectionTo(p));
    }
    public float GetDistanceTo(Vector3 p)
    {
        return Vector3.Distance(transform.position, p);
    }
    public Vector3 GetDirectionTo(Vector3 p)
    {
        return p - this.transform.position;
    }
    public Vector3 SetNavPositionSelf()
    {
        this._hasNavMeshPositionSelf = false;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this.layerMaskNavMesh);
        if (hit.collider)
        {
            this._navMeshPositionSelf = hit.point;
            this._hasNavMeshPositionSelf = true;
        }
        return _navMeshPositionSelf;
    }
    public Vector3 GetNavPositionSelf()
    {
        return this._navMeshPositionSelf;
    }
    public Vector3 SetNavPositionTarget()
    {
        this._hasNavMeshPositionTarget = false;
        Ray ray = new Ray(target.position + new Vector3(0,1,0), Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this.layerMaskNavMesh);
        if (hit.collider)
        {
            this._navMeshPositionTarget = hit.point;
            this._hasNavMeshPositionTarget = true;
        }

        return _navMeshPositionTarget;
    }
    public Vector3 GetNavPositionTarget()
    {
        return this._navMeshPositionTarget;
    }

    // PUBLIC (TODO: because we needed arguments! can we fix that?)

    public Vector3 GetNavPositionTowards(float distance)
    {
        Vector3 self = GetNavPositionSelf();
        Vector3 target = GetNavPositionTarget();
        Vector3 position = self - target * distance;
        Ray ray = new Ray(position + new Vector3(0, 1, 0), Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this.layerMaskNavMesh);
        if (hit.collider)
        {
            return position;
        } else
        {
            return GetNavPositionSelf();
        }
    }
    private bool RayCastLOS()
    {
        // We raycast at a fixed height of 0.5 regardless of relative heights.
        Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
        Ray ray = new Ray(origin, direction2D);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this.layerMaskLOS);
        if (hit.collider)
        {
            Character character = hit.collider.gameObject.GetComponent<Character>();
            if (character)
            {
                //this.lastLOSpoint = hit.point;
                //Debug.DrawRay(ray.origin, ray.direction * this.distance2D, Color.blue);
                return true;
            }
        }
        //Debug.DrawRay(ray.origin, ray.direction * this.distance2D, Color.yellow);

        // fallback for when we're too close. should be based on SOMETHING other then a magic number. 
        if(this.distance < 2.0f)
        {
            return true;
        }
        return false;
    }

    // 2D methods

    public float GetAngleTo2D(Vector3 t)
    {
        t.y = 0;
        return Vector3.Angle(transform.forward, GetDirectionTo(t));
    }
    public float GetDistanceTo2D(Vector3 t)
    {
        Vector3 b = this.transform.position;
        t.y = 0;
        b.y = 0;
        return Vector3.Distance(b, t);
    }
    public Vector3 GetDirectionTo2D(Vector3 t)
    {
        Vector3 b = this.transform.position;
        t.y = 0;
        b.y = 0;
        return t - b;
    }

    // Radial detection
    public List<Vector3> SetNavMeshPoints(Vector3 position, float navpointRadius, int navPointQuantity)
    {
        this._navMeshPoints.Clear();
        List<Vector3> points = Rules.RadialNavMeshPoints(position, navpointRadius, navPointQuantity, navpointOffset, layerMaskNavMesh);
        this._navMeshPoints = points;
        return this._navMeshPoints;
    }
    public List<Vector3> GetNavMeshPoints(Vector3 position, float navpointRadius, int navPointQuantity)
    {
        return Rules.RadialNavMeshPoints(position, navpointRadius, navPointQuantity, navpointOffset, layerMaskNavMesh);
    }


    public void OnDrawGizmosSelected()
    {
        //Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
        //Debug.DrawRay(origin, _direction2D.normalized * _distance, Color.blue);

        foreach(Vector3 p in this._navMeshPoints)
        {
            Gizmos.DrawSphere(p, 0.2f);
        }
    }
}
