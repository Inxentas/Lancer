using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]

public class CanvasDialogue : Entity
{
    private Canvas canvas;
    private CanvasGroup group;

    public GameObject dialogueParent;
    public GameObject responseParent;
    public TextMeshProUGUI speakerTextMesh;

    protected override void OnAwake()
    {
        base.OnAwake();
        this.canvas = GetComponent<Canvas>();
        this.group = GetComponent<CanvasGroup>();
        //this.Clear();
    }
    private void ClearDialogue()
    {
        if (this.dialogueParent)
        {
            foreach (Transform t in dialogueParent.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
    private void ClearResponse()
    {
        if (this.responseParent)
        {
            foreach (Transform t in responseParent.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
    private void ClearSpeaker()
    {
        if (this.speakerTextMesh)
        {
            speakerTextMesh.text = "";
        }
    }
    private void Clear()
    {
        this.ClearDialogue();
        this.ClearResponse();
        this.ClearSpeaker();
    }
}
