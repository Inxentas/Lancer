using UnityEngine;

public class CanvasHardsuit : CanvasEquipment
{
    public CardHardsuit card;

    private HardsuitData[] _data;

    protected override void OnAwake()
    {
        base.OnAwake();
        GameTriggerHardsuitChoice.choiceEvent.AddListener(RespondToGameTrigger);
        this._data = Resources.LoadAll<HardsuitData>("HardsuitData");
        foreach (HardsuitData item in _data)
        {
            CardHardsuit obj = Instantiate(card, cardParent);
            obj.name = "Card - " + item.displayName;
            obj.ApplyData(item);
        }
    }
    protected override void RespondToGameTrigger(GameTrigger trigger)
    {
        base.RespondToGameTrigger(trigger);
        this._machine.SetState(this._machine.show);
    }
}
