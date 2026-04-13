using UnityEngine;
using UnityEngine.EventSystems;

public class CardHardsuit : Card, IPointerClickHandler
{
    [SerializeField] private HardsuitData _data;

    public void ApplyData(HardsuitData data)
    {
        this._data = data;
        if (this.textmeshHeading) { this.textmeshHeading.text = data.displayName; }
    }
    public void OnPointerClick(PointerEventData data)
    {
        PauseManager.pause.Invoke(false);
    }
}
