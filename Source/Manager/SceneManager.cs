using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager : Entity
{
    private StateMachineSceneManager machine;
    private int _index = 1;
    public int index { get { return this._index; } }

    [Header("Public Settings")]
    public Camera cam;
    
    [Tooltip("The standard duration of the pre- and post load states.")]
    public float duration = 0.6f;

    [Tooltip("The scene that will initially load.")]
    public int initialIndex = 1;

    public static SceneLoadInitEvent sceneLoadInit = new SceneLoadInitEvent();
    public static ScenePreLoadProgressEvent scenePreLoadProgress = new ScenePreLoadProgressEvent();
    public static ScenePostLoadProgressEvent scenePostLoadProgress = new ScenePostLoadProgressEvent();
    public static SceneLoadCompleteEvent sceneLoadComplete = new SceneLoadCompleteEvent();
    public static SceneRequestEvent sceneRequest = new SceneRequestEvent();

    #region # Manager
    protected override void OnAwake()
    {
        this._index = this.initialIndex;
        this.machine = new StateMachineSceneManager(this);
        this.machine.SetState(new StateSceneManager(this.machine));
        SceneManager.sceneRequest.AddListener(OnSceneRequest);
        base.OnAwake();
    }
    protected override void OnStart()
    {
        base.OnStart();
        //this.machine.SetState(new ScenePreLoad(this.machine, this.index)); /* Title Screen */

        AsyncOperation load = this.LoadSceneByIndex(this.index);
        load.completed += this.OnSceneLoaded;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (this.machine != null) { machine.Run(); }
    }
    #endregion

    #region # EventHandlers

    /* Sets the active Scene to the last one in the stack. */
    private void OnSceneLoaded(AsyncOperation op)
    {
        // The last loaded scene by this operation is always set as the active Scene.
        int count = UnityEngine.SceneManagement.SceneManager.loadedSceneCount - 1;
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneAt(count));
    }
    /* Sets the active Scene to the last one in the stack. */
    private void OnSceneUnloaded(AsyncOperation op)
    {
        // perhaps use this as a que to remove loading icons and such?
        // we should also manage Camera and AudioListeners!
        int count = UnityEngine.SceneManagement.SceneManager.loadedSceneCount - 1;
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneAt(count));
    }
    /* Handles incoming requests to load scenes. */
    private void OnSceneRequest(int index)
    {
        Time.timeScale = 1;
        this.machine.SetState(new ScenePreLoad(this.machine, index));
    }

    #endregion

    #region # AsyncOperations
    public AsyncOperation LoadSceneByIndex(int index)
    {
        this._index = index;
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }
    public AsyncOperation UnloadScene()
    {
        AsyncOperation op = this.UnloadSceneByIndex(this._index);
        op.completed += this.OnSceneUnloaded;
        return op;
    }
    public AsyncOperation UnloadSceneByIndex(int index)
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(index);
        op.completed += this.OnSceneLoaded;
        return op;
    }

    #endregion

    #region # State Machine

    public void LoadSceneByStateIndex(int index)
    {
        this.machine.SetState(new ScenePreLoad(this.machine, index));
    }
    #endregion

    #region # Testing

    public void LoadTest()
    {
        AsyncOperation op = this.LoadSceneByIndex(index);
        op.completed += this.OnSceneLoaded;
    }
    public void UnloadTest()
    {
        AsyncOperation op = this.UnloadScene();
        op.completed += this.OnSceneUnloaded;
    }
    public void LoadSceneByStateIndexTest()
    {
        this._index = 1;
        this.LoadSceneByStateIndex(1);
    }

    #endregion
}

#region # StateMachine
public class StateMachineSceneManager : StateMachine
{
    public SceneManager manager;
    public StateMachineSceneManager(SceneManager manager) { this.manager = manager; }
}
public class StateSceneManager : State
{
    protected float duration = 1.0f; /* This gets overridden in the constructor. */
    protected float timer = 0;
    protected int index;

    public StateMachineSceneManager sceneMachine { get { return this.machine as StateMachineSceneManager; } }
    public SceneManager manager { get { return this.sceneMachine.manager; } }
    public StateSceneManager(StateMachineSceneManager machine) : base(machine) { this.duration = this.manager.duration; }
    public override void OnStateEnter()
    {
        this.timer = 0;
        base.OnStateEnter();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        //this.timer += (Time.deltaTime * Time.timeScale);
    }
    public override void OnStateExit()
    {
        this.timer = 0;
        base.OnStateExit();
    }
}
public class ScenePreLoad : StateSceneManager
{
    public ScenePreLoad(StateMachineSceneManager machine, int index) : base(machine) { this.index = index; }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //Debug.Log("State Loading Starting...");
        SceneManager.sceneLoadInit.Invoke();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (timer >= duration)
        {
            this.sceneMachine.SetState(new SceneLoad(this.sceneMachine, this.index));
        }
        else
        {
            float progress = timer / duration;
            //Debug.Log("Progress: " + progress);
            SceneManager.scenePreLoadProgress.Invoke(progress);
            this.timer += (Time.deltaTime * Time.timeScale);
        }
    }
}
public class SceneLoad : StateSceneManager
{
    public SceneLoad(StateMachineSceneManager machine, int index) : base(machine) { this.index = index; }

    public override void OnStateEnter()
    {
        //Debug.Log("State Loading Actual Scene Load!");
        base.OnStateEnter();
        int count = UnityEngine.SceneManagement.SceneManager.loadedSceneCount;
        if (count > 1)
        {
            AsyncOperation unload = this.manager.UnloadScene();
            unload.completed += this.OnUnloadComplete;
        }
        else
        {
            AsyncOperation load = this.manager.LoadSceneByIndex(this.index);
            load.completed += this.OnLoadComplete;
        }
    }
    protected void OnUnloadComplete(AsyncOperation op)
    {
        op.completed -= OnUnloadComplete;
        AsyncOperation load = this.manager.LoadSceneByIndex(this.index);
        load.completed += this.OnLoadComplete;
    }
    protected void OnLoadComplete(AsyncOperation op)
    {
        op.completed -= OnLoadComplete;
        //Debug.Log("State Loading Actual Scene Complete!");
        if (this.manager.index == 1) /* Title Screen */
        {
            this.manager.cam.enabled = true;
        }
        else
        {
            this.manager.cam.enabled = false;
        }
        this.sceneMachine.SetState(new ScenePostLoad(this.sceneMachine));
    }
}
public class ScenePostLoad : StateSceneManager
{
    public ScenePostLoad(StateMachineSceneManager machine) : base(machine) { }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (timer >= duration)
        {
            //Debug.Log("State Loading Complete!");
            this.sceneMachine.SetState(new StateSceneManager(this.sceneMachine));
            SceneManager.sceneLoadComplete.Invoke();
        }
        else
        {
            float progress = timer / duration;
            //Debug.Log("Progress: " + progress);
            SceneManager.scenePostLoadProgress.Invoke(progress);
            this.timer += (Time.deltaTime * Time.timeScale);
        }
    }
}
#endregion

public class LoadTitle : StateSceneManager
{
    public LoadTitle(StateMachineSceneManager machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        this.manager.LoadSceneByIndex(1);
    }
}
public class LoadSample : StateSceneManager
{
    public LoadSample(StateMachineSceneManager machine) : base(machine) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        this.manager.LoadSceneByIndex(2);
    }
}


// Events
public class SceneLoadInitEvent : UnityEvent { }
public class ScenePreLoadProgressEvent : UnityEvent<float> { }
public class ScenePostLoadProgressEvent : UnityEvent<float> { }
public class SceneLoadCompleteEvent : UnityEvent { }
public class SceneRequestEvent : UnityEvent<int> { }