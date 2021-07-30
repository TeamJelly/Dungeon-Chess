using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.UI
{

    [RequireComponent(typeof(Button))]
    public class PixelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public RectTransform DownTransfrom;
        public Image FrameImage;
        public Image MainImage;

        public Sprite DefaultFrameSprite;
        public Sprite PushedFrameSprite;

        public Button properties;

        public float pushPixel = 6f;

        public bool hasImage = false;
        public bool toggleOption = false;
        public bool pressed = false;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!properties.interactable) return;

            if (toggleOption) pressed = !pressed;
            if (!toggleOption) pressed = true;

            if (pressed && eventData.button == PointerEventData.InputButton.Left)
            {
                ShowPushEffect();
            }

        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!properties.interactable) return;

            if (!toggleOption) pressed = false;

            if (!pressed && eventData.button == PointerEventData.InputButton.Left)
            {
                ShowPopEffect();
            }
        }


        public void ShowPushEffect()
        {
            DownTransfrom.anchoredPosition = DownTransfrom.anchoredPosition+ new Vector2(0, -pushPixel);
            FrameImage.sprite = PushedFrameSprite;
        }

        public void ShowPopEffect()
        {
            DownTransfrom.anchoredPosition = DownTransfrom.anchoredPosition + new Vector2(0, pushPixel);
            FrameImage.sprite = DefaultFrameSprite;
        }
        public virtual void SetInteractable(bool b)
        {
            properties.interactable = b;
            if (b)
                FrameImage.color = new Color(FrameImage.color.r,FrameImage.color.g,FrameImage.color.b,1);
            else
                FrameImage.color = new Color(FrameImage.color.r,FrameImage.color.g,FrameImage.color.b,0.75f);
        }

        private void OnValidate()
        {
            if (properties == null) properties = GetComponent<Button>();
            if (FrameImage == null) FrameImage = GetComponent<Image>();
            if (DownTransfrom == null) DownTransfrom = transform.GetComponentInChildren<RectTransform>();
            if (hasImage && MainImage == null) MainImage = DownTransfrom.GetChild(0).GetComponent<Image>();

            properties.transition = Selectable.Transition.None;

            FrameImage.sprite = DefaultFrameSprite;
            FrameImage.type = Image.Type.Sliced;
        }

    }
}