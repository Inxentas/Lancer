using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : Entity
{
    public static PauseEvent pause = new PauseEvent();

    public Character character;

    [SerializeField] private CinemachineOrbitalFollow[] _orbitalFollows;
    [SerializeField] private CinemachineRotationComposer[] _rotationComposers;
    [SerializeField] private CinemachineInputAxisController[] _inputAxisControllers;

    protected override void OnAwake()
    {
        base.OnAwake();
        PauseManager.pause.AddListener(OnPause);
        Cursor.visible = false;
        FindObjectsSortMode mode = FindObjectsSortMode.InstanceID;
        this._orbitalFollows = GameObject.FindObjectsByType<CinemachineOrbitalFollow>(mode);
        this._rotationComposers = GameObject.FindObjectsByType<CinemachineRotationComposer>(mode);
        this._inputAxisControllers = GameObject.FindObjectsByType<CinemachineInputAxisController>(mode);
    }
    private void OnPause(bool pause)
    {
        this.pauseCamera(pause);
        this.PauseCharacter(pause);
        Cursor.visible = pause;
    }
    private void pauseCamera(bool pause)
    {
        foreach(CinemachineOrbitalFollow f in this._orbitalFollows)
        {
            f.enabled = !pause;
        }
        foreach (CinemachineRotationComposer r in this._rotationComposers)
        {
            r.enabled = !pause;
        }
        foreach (CinemachineInputAxisController i in this._inputAxisControllers)
        {
            i.enabled = !pause;
        }
    }
    private void PauseCharacter(bool pause)
    {
        this.character.suspendMotion = pause;
        this.character.suspendLookDirection = pause;
    }
}
public class PauseEvent : UnityEvent<bool> { }