using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasEquipment : Entity
{
    public static CanvasEquipmentCloseEvent close = new CanvasEquipmentCloseEvent();

    public RectTransform cardParent;

    protected Canvas _canvas;
    protected CanvasGroup _group;
    protected CanvasEquipmentStateMachine _machine;

    private InputAction _cancelAction;

    public Canvas canvas { get { return _canvas; } }
    public CanvasGroup group { get { return _group; } }
    public InputAction cancelAction { get { return _cancelAction; } }

    protected override void OnAwake()
    {
        base.OnAwake();
        this._canvas = GetComponent<Canvas>();
        this._group = GetComponent<CanvasGroup>();
        this._machine = new CanvasEquipmentStateMachine(this);
        this._machine.SetState(this._machine.idle);
        this._cancelAction = InputSystem.actions.FindAction("Cancel");
        PauseManager.pause.AddListener(OnPause);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(this._machine != null)
        {
            this._machine.Run();
        }
    }
    virtual protected void RespondToGameTrigger(GameTrigger trigger) { }
    public void OnPause(bool pause)
    {
        if (pause == false)
        {
            this._machine.SetState(this._machine.hide);
        }
    }

    public void Activate()
    {
        this.group.interactable = true;
        this.group.blocksRaycasts = true;
    }
    public void Deactivate()
    {
        this.group.interactable = false;
        this.group.blocksRaycasts = false;
    }
}

// state machine

public class CanvasEquipmentStateMachine : StateMachine
{
    protected CanvasEquipment _canvas;
    public CanvasEquipment canvas { get { return _canvas; } }

    public CanvasEquipmentStateIdle idle;
    public CanvasEquipmentStateShow show;
    public CanvasEquipmentStateHide hide;

    public CanvasEquipmentStateMachine(CanvasEquipment canvas) : base()
    { 
        this._canvas = canvas;
        this.idle = new CanvasEquipmentStateIdle(this);
        this.show = new CanvasEquipmentStateShow(this);
        this.hide = new CanvasEquipmentStateHide(this);
    }
}
public class CanvasEquipmentState : State
{
    protected CanvasEquipmentStateMachine stateMachine { get { return this.machine as CanvasEquipmentStateMachine; } }
    public CanvasEquipment canvas { get { return this.stateMachine.canvas; } }
    public CanvasEquipmentState(CanvasEquipmentStateMachine machine) : base(machine) { }
}
public class CanvasEquipmentStateIdle : CanvasEquipmentState
{
    public CanvasEquipmentStateIdle(CanvasEquipmentStateMachine machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        this.canvas.group.alpha = 0;
        this.canvas.group.interactable = true;
        this.canvas.group.blocksRaycasts = true;
    }
}
public class CanvasEquipmentStateShow : CanvasEquipmentState
{
    public CanvasEquipmentStateShow(CanvasEquipmentStateMachine machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateUpdate();
        this.canvas.group.alpha = 1;
        this.canvas.Activate();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (this.stateMachine.canvas.cancelAction.IsPressed())
        {
            this.stateMachine.SetState(this.stateMachine.hide);
            PauseManager.pause.Invoke(false);
        }
    }
}
public class CanvasEquipmentStateHide : CanvasEquipmentState
{
    public CanvasEquipmentStateHide(CanvasEquipmentStateMachine machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateExit();
        CanvasEquipment.close.Invoke();
        this.canvas.group.alpha = 0;
        this.canvas.Deactivate();
    }
}

// events

public class CanvasEquipmentCloseEvent : UnityEvent { }