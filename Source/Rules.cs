using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rules : MonoBehaviour
{
    public enum ProjectileOrigin { Player, Enemy }

    // TODO: Rename these methods, or make resetting Y an option or something!
    public static List<Vector3> GeneratePointsOnCircleKeepOrigin(Vector3 origin, float radius, int quantity)
    {
        List<Vector3> points = new List<Vector3>();
        float radPerSection = 360f / quantity * Mathf.Deg2Rad;
        for (int i = 0; i < quantity; i++)
        {
            float radians = radPerSection * i;
            float x = origin.x + radius * Mathf.Cos(radians);
            float z = origin.z + radius * Mathf.Sin(radians);
            points.Add(new Vector3(x, origin.y, z));
        }
        return points;
    }
    /**
     * This method generates a bunch of Vector3 positions based on an origin, radius and quantity. These
     * are NOT checked for anything and thus UNSAFE to use as NavMeshPositions without filtering them.
     */
    public static List<Vector3> GeneratePointsOnCircle(Vector3 origin, float radius, int quantity)
    {
        List<Vector3> points = new List<Vector3>();
        float radPerSection = 360f / quantity * Mathf.Deg2Rad;
        for (int i = 0; i < quantity; i++)
        {
            float radians = radPerSection * i;
            float x = origin.x + radius * Mathf.Cos(radians);
            float z = origin.z + radius * Mathf.Sin(radians);
            points.Add(new Vector3(x, 0, z));
        }
        return points;
    }
    public static List<Vector3> GenerateDirectionsOnCirle(Vector3 origin, int quantity)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < quantity; i++)
        {
            float angle = (360f / quantity) * i;
            float radian = Mathf.Deg2Rad * angle;
            Vector3 direction = new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian));

            // Normalize it (in case you want it explicitly as a unit vector)
            //direction.Normalize();

            // Add the direction vector to the list
            points.Add(direction);

        }
        return points;
    }
    /**
     * This method should be fed with the result of GeneratePointsOnCircle(). It uses a raycast to check
     * for a valid navmesh position with a fixed LayerMask. By default we use NavMesh.AllAreas but this
     * can be overridden.
     */
    public static List<Vector3> FilterPointsByNavMesh(List<Vector3> points, float offset, LayerMask mask, int navmesh = NavMesh.AllAreas)
    {
        List<Vector3> navpoints = new List<Vector3>();
        for (int i = 0; i < points.Count; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(points[i] + new Vector3(0, offset, 0), Vector3.down);
            bool collide = Physics.Raycast(ray, out hit, offset * 2, mask);
            if (collide)
            {
                NavMeshHit sample;
                bool nav = NavMesh.SamplePosition(hit.point, out sample, 0.5f, navmesh);
                if (nav)
                {
                    navpoints.Add(points[i]);
                    //Debug.Log("hit:" + hit.collider.gameObject.name);
                }
                else
                {
                    //Debug.Log("no hit:"+hit.collider.gameObject.name);
                }
            }
            else
            {
                //Debug.Log("no collider at: " + points[i] + " OFFSET: " + offset + " to " + (0-offset));
                //Debug.DrawLine(ray.origin, points[i] - new Vector3(0, offset, 0), Color.magenta);
                //Debug.DrawLine(ray.origin, points[i] - new Vector3(0, 0-offset, 0), Color.green);
            }
        }
        return navpoints;
    }

    /**
     * This method returns a safe List using other internal methods, based on a central point.
     */
    public static List<Vector3> RadialNavMeshPoints(Vector3 position, float radius, int qty, float offset, LayerMask mask)
    {
        List<Vector3> points = Rules.GeneratePointsOnCircleKeepOrigin(position, radius, qty);
        List<Vector3> navPoints = Rules.FilterPointsByNavMesh(points, offset, mask);
        return navPoints;
    }

    public static Vector3 PointClosestTo(Vector3 position, List<Vector3> points)
    {
        Vector3 value = points[0];
        if (points.Count == 0) {
            value =  Vector3.zero;
        }  else {
            foreach (Vector3 p in points) {
                if (Vector3.Distance(position, p) < Vector3.Distance(position, value)) {
                    value = position;
                }   
            }
        }
        return value;
    }
    public static Vector3 PointFurthestFrom(Vector3 position, List<Vector3> points)
    {
        Vector3 value = points[0];
        if (points.Count == 0)
        {
            value = Vector3.zero;
        } else {
            foreach (Vector3 p in points) {
                if (Vector3.Distance(position, p) > Vector3.Distance(position, value)) {
                    value = position;
                }
            }
        }
        return value;
    }
}
