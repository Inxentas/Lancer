using UnityEngine;

public class CanvasGameTrigger : Entity
{
    private Canvas canvas;
    public RectTransform rect;
    private bool visible = false;
    private GameTrigger trigger;

    protected override void OnAwake()
    {
        base.OnAwake();
        this.canvas = GetComponent<Canvas>();
        GameTrigger.gameTriggerAvailable.AddListener(OnAvailable);
        GameTrigger.gameTriggerUnavailable.AddListener(OnUnavailable);
        GameTrigger.gameTriggerActivate.AddListener(OnActivate);
        visible = false;

        this.rect.localScale = new Vector3(0, 0, 0);
    }
    private void OnAvailable(GameTrigger trigger)
    {
        this.trigger = trigger;
        this.rect.localScale = new Vector3(0, 0, 0);
        this.AdjustPosition();
        visible = true;
    }
    private void OnUnavailable(GameTrigger trigger)
    {
        if(trigger == this.trigger)
        {
            visible = false;
        }
    }
    private void OnActivate(GameTrigger trigger)
    {
        this.visible = false;
    }
    private void AdjustPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.trigger.transform.position);
        this.rect.gameObject.transform.position = screenPos;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (visible)
        {
            this.AdjustPosition();
            this.rect.localScale = Vector3.Lerp(this.rect.localScale, new Vector3(1, 1, 1), 0.4f);
        }
        else
        {
            this.rect.localScale = Vector3.Lerp(this.rect.localScale, new Vector3(0, 0, 0), 0.4f);
        }
        if(transform.localScale == Vector3.zero)
        {
            canvas.enabled = false;
        } else
        {
            canvas.enabled = true;
        }
    }
}
