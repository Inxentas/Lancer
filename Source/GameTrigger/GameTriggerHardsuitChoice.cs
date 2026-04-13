using UnityEngine;
using UnityEngine.Events;

public class GameTriggerHardsuitChoice : GameTrigger
{
    public static HardSuitChoiceEvent choiceEvent = new HardSuitChoiceEvent();

    protected override void Activate()
    {
        base.Activate();
        GameTriggerHardsuitChoice.choiceEvent.Invoke(this);
        PauseManager.pause.Invoke(true);
    }
}
public class HardSuitChoiceEvent : UnityEvent<GameTrigger> { }