using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameTrigger : MonoBehaviour
{
    public static GameTriggerAvailableEvent gameTriggerAvailable = new GameTriggerAvailableEvent();
    public static GameTriggerUnavailableEvent gameTriggerUnavailable = new GameTriggerUnavailableEvent();
    public static GameTriggerActivateEvent gameTriggerActivate = new GameTriggerActivateEvent();

    protected Character _character;

    [Header("Public Settings")]
    public float minAngle = 0f;
    public float maxAngle = 45f;
    public float minDistance = 0.0f;
    public float maxDistance = 1.5f;
    public bool singleUse = false;

    [Header("Serialized Private Fields")]
    [SerializeField] private float _angle;
    [SerializeField] private float _cross;
    [SerializeField] private float _distance;
    [SerializeField] private bool _isLeft;
    [SerializeField] private bool _isRight;
    [SerializeField] private bool _isAvailable;
    [SerializeField] private bool _isUsed = false;

    private InputAction interactAction;

    private void Awake()
    {
        this.OnAwake();
    }
    private void Update()
    {
        this.OnUpdate();
    }
    
    private void Calculate()
    {
        if (this._character)
        {
            // we reset the y values because we use an orthographic camera
            Vector3 pointOther = _character.transform.position;
            //pointOther.y = 0;
            Vector3 pointSelf = this.transform.position;
            //pointSelf.y = 0;
            Vector3 direction = pointOther - pointSelf;

            this._angle = Vector3.Angle(this.transform.forward, direction);
            this._cross = Vector3.Cross(this.transform.forward, direction).y;
            this._distance = Vector3.Distance(pointOther, pointSelf);
            this._isRight = this._cross > 0 ? true : false;
            this._isLeft = this._cross < 0 ? true : false;

            bool available = (this._distance > this.minDistance && this._distance < this.maxDistance && this._angle > this.minAngle && this._angle < this.maxAngle && !_isUsed) ? true : false;

            if(available && !this._isAvailable)
            {
                this.SetAvailable();
            } else if (!available && this._isAvailable)
            {
                this.SetUnavailable();
            }

            this._isAvailable = available;
        }
    }
    private void GetInput()
    {
        if (this._character)
        {
            if (this._isAvailable)
            {
                if (interactAction.ReadValue<float>() > 0)
                {
                    this.Activate();
                }
            }
        }   
    }

    protected virtual void OnAwake()
    {
        this._character = GameObject.FindFirstObjectByType<Character>();
        this.interactAction = InputSystem.actions.FindAction("Interact");
    }
    protected virtual void OnUpdate()
    {
        this.Calculate();
        this.GetInput();
    }
    protected virtual void SetAvailable()
    {
        GameTrigger.gameTriggerAvailable.Invoke(this);
    }
    protected virtual void SetUnavailable()
    {
        GameTrigger.gameTriggerUnavailable.Invoke(this);
    }
    protected virtual void Activate()
    {
        Debug.Log(this + " has been Activated!");
        if (this.singleUse)
        {
            this._isUsed = true;
        }
        GameTrigger.gameTriggerActivate.Invoke(this);
    }
}
public class GameTriggerAvailableEvent : UnityEvent<GameTrigger> { };
public class GameTriggerUnavailableEvent : UnityEvent<GameTrigger> { };
public class GameTriggerActivateEvent : UnityEvent<GameTrigger> { };