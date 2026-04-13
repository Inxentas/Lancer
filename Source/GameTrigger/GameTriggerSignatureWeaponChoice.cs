using UnityEngine;
using UnityEngine.Events;

public class GameTriggerSignatureWeaponChoice : GameTrigger
{
    public static SignatureWeaponChoiceEvent choiceEvent = new SignatureWeaponChoiceEvent();

    protected override void Activate()
    {
        base.Activate();
        GameTriggerSignatureWeaponChoice.choiceEvent.Invoke(this);
        PauseManager.pause.Invoke(true);
    }
}
public class SignatureWeaponChoiceEvent : UnityEvent<GameTrigger> { }