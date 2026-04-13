using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]

public class CanvasLoadingScene : Entity
{
    private Canvas _canvas;
    private CanvasGroup _group;

    protected override void OnAwake()
    {
        base.OnAwake();
        this._canvas = GetComponent<Canvas>();
        this._group = GetComponent<CanvasGroup>();

        this._canvas.sortingOrder = 100;

        this._group.alpha = 0;
        this._group.blocksRaycasts = false;
        this._group.interactable = false;

        // Scene loading and unloading handling
        SceneManager.sceneLoadInit.AddListener(OnSceneLoadInit);
        SceneManager.sceneLoadComplete.AddListener(OnSceneLoadComplete);

        // Pre- and Post Loading animation handling
        SceneManager.scenePreLoadProgress.AddListener(OnScenePreLoadProgress);
        SceneManager.scenePostLoadProgress.AddListener(OnScenePostLoadProgress);
    }

    private void OnSceneLoadInit()
    {
        this._group.alpha = 0;
    }

    private void OnSceneLoadComplete()
    {
        this._group.alpha = 0;
    }

    private void OnScenePreLoadProgress(float progress)
    {
        this._group.alpha = progress;
    }

    private void OnScenePostLoadProgress(float progress)
    {
        this._group.alpha = 1 - progress;
    }
}
