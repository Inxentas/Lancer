using UnityEngine;
using UnityEngine.EventSystems;

public class CardCompositeWeapon : Card, IPointerClickHandler
{
    [SerializeField] private CompositeWeaponData _data;

    public void ApplyData(CompositeWeaponData data)
    {
        this._data = data;
        if (this.textmeshHeading) { this.textmeshHeading.text = data.displayName; }
    }
    public void OnPointerClick(PointerEventData data)
    {
        PauseManager.pause.Invoke(false);
    }
}