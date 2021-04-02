using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PixelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool interactable = true;

    public RectTransform DownTransfrom;
    public Image FrameImage;

    public Sprite DefaultFrameSprite;
    public Sprite PushedFrameSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && interactable)
        {
            DownTransfrom.anchoredPosition = new Vector2(0, -1);
            FrameImage.sprite = PushedFrameSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && interactable)
        {
            DownTransfrom.anchoredPosition = new Vector2(0, 0);
            FrameImage.sprite = DefaultFrameSprite;
        }
    }

    private void OnValidate()
    {
        GetComponent<Button>().interactable = interactable;
        GetComponent<Button>().transition = Selectable.Transition.None;
        FrameImage = GetComponent<Image>();
        FrameImage.sprite = DefaultFrameSprite;
    }

}
