using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform mainPanel; 
    public TextMeshProUGUI textmeshHeading;

    [SerializeField] private bool _hover;

    public void OnPointerEnter(PointerEventData data)
    {
        this._hover = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        this._hover = false;
    }

    private void Update()
    {
        switch (this._hover)
        {
            case true:
                mainPanel.localScale = Vector3.Lerp(mainPanel.localScale, new Vector3(1.05f, 1.05f, 1.05f), 0.2f);
                break;
            case false:
                mainPanel.localScale = Vector3.Lerp(mainPanel.localScale, new Vector3(1, 1, 1), 0.2f);
                break;
        }
    }
}
