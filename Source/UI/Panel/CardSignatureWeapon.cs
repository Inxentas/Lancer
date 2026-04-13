using UnityEngine;
using UnityEngine.EventSystems;

public class CardSignatureWeapon : Card, IPointerClickHandler
{
    [SerializeField] private SignatureWeaponData _data;

    public void ApplyData(SignatureWeaponData data)
    {
        this._data = data;
        if (this.textmeshHeading) { this.textmeshHeading.text = data.displayName; }
    }
    public void OnPointerClick(PointerEventData data)
    {
        PauseManager.pause.Invoke(false);
    }
}
