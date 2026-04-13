using UnityEngine;
using UnityEngine.Events;

public class GameTriggerCompositeWeaponChoice : GameTrigger
{
    public static CompositeWeaponChoiceEvent choiceEvent = new CompositeWeaponChoiceEvent();

    protected override void Activate()
    {
        base.Activate();
        GameTriggerCompositeWeaponChoice.choiceEvent.Invoke(this);
        PauseManager.pause.Invoke(true);
    }
}
public class CompositeWeaponChoiceEvent : UnityEvent<GameTrigger> { }