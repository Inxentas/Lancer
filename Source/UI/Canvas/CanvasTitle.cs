using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]

public class CanvasTitle : Entity
{
    private Canvas _canvas;
    private CanvasGroup _group;

    protected override void OnAwake()
    {
        base.OnAwake();
        this._canvas = GetComponent<Canvas>();
        this._group = GetComponent<CanvasGroup>();
    }

    public void OnClickStart()
    {
        SceneManager.sceneRequest.Invoke(2);
    }
}
