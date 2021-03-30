using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Model;
using Model.Skills;

public class SkillButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Skill CurrentSkill;

    public bool interactable = true;

    public Image Image;
    public Image Frame;
    public Button UpgradeButton;

    public RectTransform ImageTransfrom;

    public List<Sprite> ButtonSprite;
    public List<Sprite> PushedButtonSprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && interactable)
        {
            Debug.LogError("클릭 되었습니다.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && interactable)
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, -1);            
            Frame.sprite = PushedButtonSprite[CurrentSkill.Grade];
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && interactable)
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, 0);
            Frame.sprite = ButtonSprite[CurrentSkill.Grade];
        }
    }

    public void SetSkill(Skill skill)
    {
        CurrentSkill = skill;

        Image.sprite = CurrentSkill.Sprite;

        if (CurrentSkill.Grade == 0)
            SetInteractable(false);
        else
            SetInteractable(true);
    }

    public void SetInteractable(bool boolean)
    {
        interactable = boolean;

        if (interactable == false)
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, -1);
            Frame.sprite = PushedButtonSprite[CurrentSkill.Grade];
        }
        else
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, 0);
            Frame.sprite = ButtonSprite[CurrentSkill.Grade];
        }
    }

    private void Start()
    {
        UpgradeButton.onClick.AddListener(() =>
        {
            CurrentSkill?.Upgrade();
            SetSkill(CurrentSkill);
        });

        // 테스트용 코드
        // SetSkill(new Skill_000() { Grade = 1 });
    }
}
