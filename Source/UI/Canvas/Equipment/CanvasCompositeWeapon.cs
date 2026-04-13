using UnityEngine;

public class CanvasCompositeWeapon : CanvasEquipment
{
    public CardCompositeWeapon card;

    private CompositeWeaponData[] _data;

    protected override void OnAwake()
    {
        base.OnAwake();
        GameTriggerCompositeWeaponChoice.choiceEvent.AddListener(RespondToGameTrigger);
        this._data = Resources.LoadAll<CompositeWeaponData>("CompositeWeaponData");
        foreach (CompositeWeaponData item in _data)
        {
            CardCompositeWeapon obj = Instantiate(card, cardParent);
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
