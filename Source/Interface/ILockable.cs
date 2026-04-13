using UnityEngine;

public interface ILockable
{
    public Vector2 screenCenter { get; }
    public Vector2 screenPosition { get; }
    public Vector2 viewportPosition { get; }
    public float screenDistance { get; }
    public bool onScreen { get; }
    protected void Subscribe() { }
    public void Unsubscribe() { }
}
