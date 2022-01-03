using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using View;
using Model.Managers;
using System;
using UI.Battle;

public class DungeonEditor : MonoBehaviour
{
    public GameObject unitAttributeController;
    [Header("Unit Editor")]
    public Button button_saveUnit;
    public Button button_lvlUp;
    public Button button_resetLv;
    public Button button_HPUp;
    public Button button_HPDown;
    public Button button_STRUp;
    public Button button_STRDown;
    public Button button_MOVUp;
    public Button button_MOVDown;

    public Button button_summonUnit;
    public Button button_unsummonUnit;

    public GameObject infoBox;
    public RectTransform Skills;
    public RectTransform Artifacts;
    public RectTransform Effects;

    public static DungeonEditor instance;

    public static bool enabledEditMode = false;

    [Space(10)]

    Tile currentTile = null;
    Unit currentUnit = null;
    
    private void Awake()
    {
        instance = this;
        unitAttributeController.SetActive(false);

        Init();
        button_lvlUp.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            Common.Command.LevelUp(currentUnit);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        /*button_resetLv.onClick.AddListener(() =>
        {
            Common.Command.LevelUp(currentUnit);
        });*/
        button_HPUp.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            Common.Command.Heal(currentUnit, 10);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_HPDown.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            Common.Command.Damage(currentUnit, 10);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_STRUp.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            currentUnit.Strength++;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_STRDown.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            currentUnit.Strength--;
            if (currentUnit.Strength < 0) currentUnit.Strength = 0;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_MOVUp.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            currentUnit.Mobility++;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });
        button_MOVDown.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            currentUnit.Mobility--;
            if (currentUnit.Mobility < 0) currentUnit.Mobility = 0;
            InfoView.instance.unitInfo.SetUnit(currentUnit);
        });

        /*button_saveUnit.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            Common.Data.SaveUnitData(currentUnit);
        });
        button_summonUnit.onClick.AddListener(() =>
        {
            if(currentUnit != null)
            {
                Common.Command.UnSummon(currentUnit);
                GameManager.RemovePartyUnit(currentUnit); //죽으면 파티유닛에서 박탈.
            }
            //currentUnit = Common.Data.LoadUnitData();
            Common.Command.Summon(currentUnit, currentTile.position);
            InfoView.instance.unitInfo.SetUnit(currentUnit);
            BattleManager.instance.InitializeUnitBuffer();

        });*/

        //소환 해제 함수 정리 필요.
        button_unsummonUnit.onClick.AddListener(() =>
        {
            if (currentUnit == null) return;
            Common.Command.UnSummon(currentUnit);

            currentUnit = null;
            InfoView.instance.unitInfo.SetNone();
        });
    }

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        currentUnit = tile?.GetUnit();
    }

    public void Init()
    {
        foreach (Skill skill in Common.Data.AllSkills.Values)
        {
            GameObject gameObject = Instantiate(infoBox, Skills);

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = skill.Sprite;

            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Skills.sizeDelta = Skills.rect.size + new Vector2(rect.width, 0);

            Button button = gameObject.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                if (currentUnit == null) InfoView.Show(skill);
                else InfoView.Show(currentUnit, skill);
            });

            Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

            modifyButton.gameObject.SetActive(true);
            modifyButton.gameObject.GetComponentInChildren<Text>().text = "+";
            modifyButton.onClick.AddListener(() => 
            {
                if (currentUnit == null) return;
                Common.Command.AddSkill(currentUnit, skill);
                InfoView.instance.unitInfo.SetUnit(currentUnit);

                if(currentUnit == BattleManager.instance.thisTurnUnit)
                    BattleView.UnitControlView.UpdateSkillButtons(currentUnit);
            });
        }

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
                if (currentUnit == null) return;
                Artifact copied = artifact.Clone() as Artifact;
                Common.Command.AddArtifact(currentUnit, copied);
                InfoView.instance.unitInfo.SetUnit(currentUnit);
            });
        }

        foreach (Effect effect in Common.Data.AllEffects)
        {
            GameObject gameObject = Instantiate(infoBox, Effects);

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = effect.Sprite;
            //if (image.color != new Color(0, 0, 0, 0))
            //image.color = Color.white;

            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Effects.sizeDelta = Effects.rect.size + new Vector2(rect.width, 0);

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => InfoView.Show(effect));

            Button modifyButton = gameObject.transform.Find("ModifyButton").GetComponent<Button>();

            modifyButton.gameObject.SetActive(true);
            modifyButton.gameObject.GetComponentInChildren<Text>().text = "+";
            modifyButton.onClick.AddListener(() =>
            {
                if (currentUnit == null)
                    return;

                Effect copied = effect.Clone() as Effect;
                Common.Command.AddEffect(currentUnit, copied);
                InfoView.instance.unitInfo.SetUnit(currentUnit);
            });
        }
    }

    public void SaveUnit()
    {
        Common.Data.Save_Unit_Serializable_Data(currentUnit);
    }
    public void ShowUnitList()
    {
        List<Common.ScrollData> list = Common.ScrollUI.MakeDataList(Application.dataPath + "/Resources/Data/Unit/", new[] { "*.json" }, (filePath) =>
        {
            Unit_Serializable u = Common.Data.Load_Unit_Serializable_Data(filePath);

            Vector2Int position = currentTile.position;
            if (currentUnit != null) Common.Command.UnSummon(currentUnit);
            currentUnit = new Unit(u);
            currentUnit.Position = position;
            Common.Command.Summon(currentUnit, currentUnit.Position);

            //이후에 ui 및 hp 바 업데이트, 등 해줘야 함.
        });
        Common.ScrollUI.instance.EnableUI(list);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            enabledEditMode = !enabledEditMode;
            unitAttributeController.gameObject.SetActive(enabledEditMode);

            if(currentUnit != null)
                InfoView.instance.unitInfo.SetUnit(currentUnit);

        }
    }
}
