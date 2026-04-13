using UnityEngine;

public class CanvasSignatureWeapon : CanvasEquipment
{
    public CardSignatureWeapon card;

    private SignatureWeaponData[] _data;

    protected override void OnAwake()
    {
        base.OnAwake();
        GameTriggerSignatureWeaponChoice.choiceEvent.AddListener(RespondToGameTrigger);
        this._data = Resources.LoadAll<SignatureWeaponData>("SignatureWeaponData");
        foreach (SignatureWeaponData item in _data)
        {
            CardSignatureWeapon obj = Instantiate(card, cardParent);
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
