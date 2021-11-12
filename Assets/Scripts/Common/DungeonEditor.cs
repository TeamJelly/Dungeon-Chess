using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using View;
using Model.Managers;
using System;

public class DungeonEditor : MonoBehaviour
{
    public GameObject unitAttributeController;
    public Button button_saveUnit;
    public Button button_lvlUp;
    public Button button_resetLv;
    public Button button_HPUp;
    public Button button_HPDown;
    public Button button_STRUp;
    public Button button_STRDown;
    public Button button_MOVUp;
    public Button button_MOVDown;


    public GameObject infoBox;
    public RectTransform Skills;
    public RectTransform Artifacts;
    public RectTransform Effects;



    public static DungeonEditor instance;

    public static bool enabledEditMode = false;

    [Space(10)]
    Unit currentUnit = null;
    private void Awake()
    {
        instance = this;

        Init();
        button_lvlUp.onClick.AddListener(() =>
        {
            Common.Command.LevelUp(currentUnit);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        /*button_resetLv.onClick.AddListener(() =>
        {
            Common.Command.LevelUp(currentUnit);
        });*/
        button_HPUp.onClick.AddListener(() =>
        {
            Common.Command.Heal(currentUnit, 10);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_HPDown.onClick.AddListener(() =>
        {
            Common.Command.Damage(currentUnit, 10);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_STRUp.onClick.AddListener(() =>
        {
            currentUnit.Strength++;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_STRDown.onClick.AddListener(() =>
        {
            currentUnit.Strength--;
            if (currentUnit.Strength < 0) currentUnit.Strength = 0;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_MOVUp.onClick.AddListener(() =>
        {
            currentUnit.Mobility++;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_MOVDown.onClick.AddListener(() =>
        {
            currentUnit.Mobility--;
            if (currentUnit.Mobility < 0) currentUnit.Mobility = 0;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
    }
    public void SetUnit(Unit unit)
    {
        currentUnit = unit;
    }


    public void Init()
    {
        foreach (Skill skill in Common.Data.AllSkills)
        {
            GameObject gameObject = Instantiate(infoBox, Skills);

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = skill.Sprite;
            if (image.color != new Color(0, 0, 0, 0))
                image.color = skill.Color;

            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Skills.sizeDelta = Skills.rect.size + new Vector2(rect.width, 0);

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => InfoView.Show(currentUnit, skill));

            Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

            modifyButton.gameObject.SetActive(true);
            modifyButton.gameObject.GetComponentInChildren<Text>().text = "+";
            modifyButton.onClick.AddListener(() => 
            {
                Common.Command.AddSkill(currentUnit, skill);
                InfoView.instance.unitInfo.SetUnit(currentUnit);

                if(currentUnit == BattleManager.instance.thisTurnUnit)
                    BattleView.UnitControlView.UpdateSkillButtons(currentUnit);
            });
        }

        //Artifact �߰��ϱ� ������.
        //�׷����� ��� Artifact�� ������ �˾ƾ� ��.
        //
        foreach (Artifact artifact in Common.Data.AllArtifacts)
        {
            GameObject gameObject = Instantiate(infoBox, Artifacts);

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = artifact.Sprite;
            //if (image.color != new Color(0, 0, 0, 0))
            //image.color = Color.white;

            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Artifacts.sizeDelta = Artifacts.rect.size + new Vector2(rect.width, 0);

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => InfoView.Show((Effect)artifact));

            Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

            modifyButton.gameObject.SetActive(true);
            modifyButton.gameObject.GetComponentInChildren<Text>().text = "+";
            modifyButton.onClick.AddListener(() =>
            {
                Artifact copied = Activator.CreateInstance(artifact.GetType()) as Artifact;
                Common.Command.AddArtifact(currentUnit, copied);
                InfoView.instance.unitInfo.SetUnit(currentUnit);
            });
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            enabledEditMode = !enabledEditMode;
            unitAttributeController.gameObject.SetActive(enabledEditMode);

            if(InfoView.instance.infoPanel.activeSelf)
                InfoView.instance.unitInfo.SetUnit(currentUnit);

        }
    }
}
