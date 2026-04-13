using UnityEngine;
using UnityEngine.Events;

public class Lockable : Entity, ILockable
{
    // events
    public static LockableSubscribeEvent subscribeEvent = new LockableSubscribeEvent();
    public static LockableUnsubscribeEvent unsubscribeEvent = new LockableUnsubscribeEvent();

    // vectors
    [Header("Vectors")]
    [SerializeField] private Vector2 _screenCenter;
    [SerializeField] private Vector2 _screenPosition;
    [SerializeField] private Vector2 _viewportCenter = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 _viewportPosition;

    // derived
    [Header("Derived")]
    [SerializeField] float _screenDistance;
    [SerializeField] float _viewportDistance;
    [SerializeField] bool _onScreen;

    // getters
    public Vector2 screenCenter { get { return this._screenCenter; } }
    public Vector2 screenPosition { get { return this._screenPosition; } }
    public Vector2 viewportCenter { get { return this._viewportCenter; } }
    public Vector2 viewportPosition { get { return this._viewportPosition; } }
    public float screenDistance { get { return this._screenDistance; } }
    public float viewportDistance { get { return this._viewportDistance; } }
    public bool onScreen { get { return this._onScreen; } }

    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected override void OnStart()
    {
        base.OnStart();
        this.Subscribe();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        // vectors
        this._screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        this._screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        this._viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);
        
        //derived
        this._screenDistance = Vector2.Distance(this._screenPosition, this._screenCenter);
        this._viewportDistance = Vector2.Distance(this._viewportPosition, this._viewportCenter);

        this._onScreen =
            //_viewportPosition.z > 0f &&
            _viewportPosition.x >= 0f && _viewportPosition.x <= 1f &&
            _viewportPosition.y >= 0f && _viewportPosition.y <= 1f;
    }

    // subscription
    protected void Subscribe()
    {
        Lockable.subscribeEvent.Invoke(this);
    }
    public void Unsubscribe()
    {
        Lockable.unsubscribeEvent.Invoke(this);
    }
}
public class LockableSubscribeEvent : UnityEvent<Lockable> { }
public class LockableUnsubscribeEvent : UnityEvent<Lockable> { }