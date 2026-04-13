using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockableList : Entity
{
    [SerializeField] private List<Lockable> _list = new List<Lockable>();

    [SerializeField] private List<Lockable> _testList = new List<Lockable>();

    protected override void OnAwake()
    {
        base.OnAwake();
        this._list.Clear();
        Lockable.subscribeEvent.AddListener(OnSubscribe);
        Lockable.unsubscribeEvent.AddListener(OnUnsubscribe);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        this._testList = LockableList.OrderByViewportDistance(
            LockableList.FilterByViewportDistance(this._list, 0.5f), 
            transform.position
        );
    }

    private void OnSubscribe(Lockable lockable)
    {
        if (!_list.Contains(lockable))
        {
            _list.Add(lockable);
        }
    }
    private void OnUnsubscribe(Lockable lockable)
    {
        if (_list.Contains(lockable))
        {
            _list.Remove(lockable);
        }
    }

    // order
    private static List<Lockable> OrderByDistance(List<Lockable> list, Vector3 position)
    {
        return list.OrderBy((d) => (d.transform.position - position).sqrMagnitude).ToList();
    }
    private static List<Lockable> OrderByViewportDistance(List<Lockable> list, Vector3 position)
    {
        return list.OrderBy(d => (d.viewportPosition).sqrMagnitude).ToList();
    }
    
    // filter
    public static List<Lockable> FilterByDistance(List<Lockable> list, Vector3 position, float maxDistance)
    {
        return list.Where(d => (d.transform.position - position).sqrMagnitude <= maxDistance * maxDistance).ToList();
    }
    public static List<Lockable> FilterByViewportDistance(List<Lockable> list, float maxDistance)
    {
        return list.Where(d => d.viewportDistance <= maxDistance).ToList();
    }
}
