using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : Entity
{
    [SerializeField] private CinemachineCamera cameraMain;
    [SerializeField] private CinemachineCamera cameraAim;

    private InputAction aimAction;

    protected override void OnAwake()
    {
        base.OnAwake();
        aimAction = InputSystem.actions.FindAction("Aim");
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(this.aimAction.ReadValue<float>() > 0)
        {
            cameraMain.Priority = 0;
            cameraAim.Priority = 1;
        } else
        {
            cameraMain.Priority = 1;
            cameraAim.Priority = 0;
        }
    }
}
