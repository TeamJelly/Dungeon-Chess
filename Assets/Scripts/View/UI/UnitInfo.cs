using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Model;
using Model.Managers;

namespace View.UI
{
    public class UnitInfo : MonoBehaviour
    {
        public TextMeshProUGUI Name;
        public Image Portrait;
        public TextMeshProUGUI Status;

        public RectTransform Skills;
        public RectTransform Belongings;
        public RectTransform Effects;

        public GameObject infoBox;

        public void SetNone()
        {
            Name.text = "No Unit";
            Portrait.sprite = null;
            Status.text = "";

            Transform[] childList = Skills.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Skills.sizeDelta = Vector2.zero;
            childList = Belongings.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            childList = Effects.transform.GetComponentsInChildren<Transform>();
            Belongings.sizeDelta = Vector2.zero;
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Effects.sizeDelta = Vector2.zero;
        }

        public void SetUnit(Unit unit)
        {
            Name.text = $"{unit.Modifier} {unit.Name}";
            Portrait.sprite = unit.Sprite;
            Status.text = $"{unit.Level}\n{unit.CurHP}/{unit.MaxHP}\n{unit.CurEXP}/{unit.NextEXP}\n{unit.Strength}\n{unit.Agility}\n{unit.Mobility}";

            Transform[] childList = Skills.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Skills.sizeDelta = Vector2.zero;
            childList = Belongings.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            childList = Effects.transform.GetComponentsInChildren<Transform>();
            Belongings.sizeDelta = Vector2.zero;
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Effects.sizeDelta = Vector2.zero;


            // 이동 스킬
            GameObject gameObject = Instantiate(infoBox, Skills);

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = unit.MoveSkill.Sprite;

            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Skills.sizeDelta = Skills.rect.size + new Vector2(rect.width, 0);
            gameObject.transform.SetParent(Skills);

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => InfoView.Show(unit, unit.MoveSkill));

            // 이동 스킬 삭제버튼
            if (DungeonEditor.enabledEditMode)
            {
                Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

                modifyButton.gameObject.SetActive(true);
                modifyButton.gameObject.GetComponentInChildren<Text>().text = "-";
                modifyButton.onClick.AddListener(() =>
                {
                    Common.Command.RemoveSkill(unit, unit.MoveSkill);
                    InfoView.instance.unitInfo.SetUnit(unit);                    
                    if(unit == BattleManager.instance.thisTurnUnit)
                        BattleView.UnitControlView.UpdateSkillButtons(unit);
                });
            }

            // 공격 스킬
            foreach (Skill skill in unit.Skills)
            {
                gameObject = Instantiate(infoBox, Skills);

                image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = skill.Sprite;
                // if (image.color != new Color(0, 0, 0, 0))
                //     image.color = skill.Color;

                rect = gameObject.GetComponent<RectTransform>().rect;
                Skills.sizeDelta = Skills.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Skills);

                button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(unit, skill));

                if (DungeonEditor.enabledEditMode)
                {
                    Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

                    modifyButton.gameObject.SetActive(true);
                    modifyButton.gameObject.GetComponentInChildren<Text>().text = "-";
                    modifyButton.onClick.AddListener(() =>
                    {
                        Common.Command.RemoveSkill(unit, skill);
                        InfoView.instance.unitInfo.SetUnit(unit);
                        if(unit == BattleManager.instance.thisTurnUnit)
                            BattleView.UnitControlView.UpdateSkillButtons(unit);
                    });
                }
            }

            foreach (Obtainable obtainable in unit.Belongings)
            {
                gameObject = Instantiate(infoBox, Belongings);

                image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = obtainable.Sprite;

                rect = gameObject.GetComponent<RectTransform>().rect;
                Belongings.sizeDelta = Belongings.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Belongings);

                button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(obtainable));

                if (DungeonEditor.enabledEditMode)
                {
                    Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

                    modifyButton.gameObject.SetActive(true);
                    modifyButton.gameObject.GetComponentInChildren<Text>().text = "-";
                    modifyButton.onClick.AddListener(() =>
                    {
                        Common.Command.RemoveArtifact(unit, (Artifact)obtainable);
                        InfoView.instance.unitInfo.SetUnit(unit);
                        // if(unit == BattleManager.instance.thisTurnUnit)
                        //     BattleView.UnitControlView.UpdateSkillButtons(unit);
                    });
                }

            }

            foreach (Effect effect in unit.StateEffects)
            {
                gameObject = Instantiate(infoBox, Effects);

                image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = effect.Sprite;

                rect = gameObject.GetComponent<RectTransform>().rect;
                Effects.sizeDelta = Effects.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Effects);

                button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(effect));

                
                if (DungeonEditor.enabledEditMode)
                {
                    Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

                    modifyButton.gameObject.SetActive(true);
                    modifyButton.gameObject.GetComponentInChildren<Text>().text = "-";
                    modifyButton.onClick.AddListener(() =>
                    {
                        Common.Command.RemoveEffect(unit, effect);
                        InfoView.instance.unitInfo.SetUnit(unit);
                    });
                }
            }
        }




        private void OnValidate()
        {
            Name = transform.Find("Name/Text").GetComponent<TextMeshProUGUI>();
            Status = transform.Find("Status/Parameter").GetComponent<TextMeshProUGUI>();
            Portrait = transform.Find("Portrait/Image").GetComponent<Image>();

            Skills = transform.Find("Skills/Viewport/Content").GetComponent<RectTransform>();
            Skills.sizeDelta = Vector2.zero;

            Belongings = transform.Find("Belongings/Viewport/Content").GetComponent<RectTransform>();
            Belongings.sizeDelta = Vector2.zero;

            Effects = transform.Find("Effects/Viewport/Content").GetComponent<RectTransform>();
            Effects.sizeDelta = Vector2.zero;

            infoBox = Resources.Load<GameObject>("Prefabs/UI/InfoBox");
        }

    }

}
