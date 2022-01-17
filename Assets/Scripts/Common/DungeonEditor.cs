using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using View;
using Model.Managers;
using System;
using UI.Battle;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class DungeonEditor : MonoBehaviour
{
    public GameObject unitAttributeController;
    public GameObject battleFieldController;

    [Header("Save Panel")]
    public GameObject savePanel;
    public TMP_InputField inputField;

    [Space(5)]
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

    public GameObject infoBox;
    public RectTransform Skills;
    public RectTransform Artifacts;
    public RectTransform Effects;


    [Space(5)]

    [Header("Battle Editor")]
    public RectTransform ListBar;

    public RectTransform ListSelectedCursor;

    public int selectedIndex;

    public object selectedObject;


    public static DungeonEditor instance;

    public static bool enabledEditMode = false;

    Tile currentTile = null;
    Unit currentUnit = null;

    public TextMeshProUGUI BattleButtonText;

    public TextMeshProUGUI AutoButtonText;

    private void Awake()
    {
        instance = this;
        unitAttributeController.SetActive(false);
        battleFieldController.SetActive(false);

        InitUnitInfoController();

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
    }
    private void Start()
    {
        InitTilesInData();
    }
    void InitTilesInData()
    {
        var Tiles = Common.Data.AllTiles;
        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].position = new Vector2Int(-10, i);
            FieldManager.instance.tileMap.SetTile(new Vector3Int(-10, i, 0), Tiles[i].TileBase);
        }
    }

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        currentUnit = tile?.GetUnit();
    }

    public void InitUnitInfoController()
    {
        foreach (Skill skill in Common.Data.AllSkills)
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

                if (currentUnit == BattleManager.instance.thisTurnUnit)
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
                Artifact copied = artifact.Clone();
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

                Effect copied = effect.Clone();
                Common.Command.AddEffect(currentUnit, copied);
                InfoView.instance.unitInfo.SetUnit(currentUnit);
            });
        }
    }

    public void SaveUnit()
    {
        if (currentUnit == null)
        {
            Debug.LogError("저장할 유닛이 없습니다.");
        }
        else
        {
            Debug.LogError(currentUnit.Name + "저장");
            Common.Data.Save_Unit_Serializable_Data(currentUnit);
        }

    }

    public void SaveScene()
    {
        Common.Data.SaveScene(inputField.text);
        savePanel.gameObject.SetActive(false);
    }
    public void SetCursor(int num)
    {
        if (num < 0 || ListBar.childCount <= num)
        {
            selectedIndex = -1;
            ListSelectedCursor.gameObject.SetActive(false);
        }
        else
        {
            selectedIndex = num;
            ListSelectedCursor.gameObject.SetActive(true);
            ListSelectedCursor.position = ListBar.GetChild(num).GetComponent<RectTransform>().position;
        }
    }

    public void SetSceneList()
    {
        Transform[] childList = ListBar.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
            Destroy(childList[i].gameObject);
        ListBar.sizeDelta = Vector2.zero;

        string[] filters = new[] { "*.json" };
        string[] ScenePaths = filters.SelectMany(f => System.IO.Directory.GetFiles(Application.dataPath + "/Resources/Data/Scene/", f)).ToArray();


        selectedIndex = -1;
        ListSelectedCursor.gameObject.SetActive(false);

        for (int i = 0; i < ScenePaths.Length; i++)
        {
            GameObject gameObject = Instantiate(infoBox, ListBar);

            TMPro.TextMeshProUGUI TMP = gameObject.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMP.gameObject.SetActive(true);
            TMP.text = Path.GetFileNameWithoutExtension(ScenePaths[i]);

            // Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            // image.sprite = Units[i].Sprite;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            ListBar.sizeDelta = ListBar.rect.size + new Vector2(150, 0);

            Button button = gameObject.GetComponent<Button>();

            Image Background = gameObject.GetComponent<Image>();
            Color oldColor = Background.color;

            int temp = i;

            button.onClick.AddListener(() =>
            {
                if (selectedIndex == temp)
                {
                    SetCursor(-1);
                }
                else
                {
                    SetCursor(temp);
                    Common.Data.LoadScene(ScenePaths[temp]);
                    inputField.text = Path.GetFileNameWithoutExtension(ScenePaths[temp]);
                }
            });
        }
    }
    public void SetUnitList()
    {
        Transform[] childList = ListBar.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
            Destroy(childList[i].gameObject);
        ListBar.sizeDelta = Vector2.zero;

        string[] filters = new[] { "*.json" };
        string[] UnitPaths = filters.SelectMany(f => System.IO.Directory.GetFiles(Application.dataPath + "/Resources/Data/Unit/", f)).ToArray();


        selectedIndex = -1;
        ListSelectedCursor.gameObject.SetActive(false);

        for (int i = 0; i < UnitPaths.Length; i++)
        {
            GameObject gameObject = Instantiate(infoBox, ListBar);

            TMPro.TextMeshProUGUI TMP = gameObject.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMP.gameObject.SetActive(true);
            TMP.text = Path.GetFileNameWithoutExtension(UnitPaths[i]);

            // Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            // image.sprite = Units[i].Sprite;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            ListBar.sizeDelta = ListBar.rect.size + new Vector2(150, 0);

            Button button = gameObject.GetComponent<Button>();

            Image Background = gameObject.GetComponent<Image>();
            Color oldColor = Background.color;

            int temp = i;

            button.onClick.AddListener(() =>
            {
                if (selectedIndex == temp)
                {
                    SetCursor(-1);
                }
                else
                {
                    SetCursor(temp);
                    selectedObject = UnitPaths[temp];
                }
            });
        }
    }
    public void SetTileList()
    {
        Transform[] childList = ListBar.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
            Destroy(childList[i].gameObject);
        ListBar.sizeDelta = Vector2.zero;

        var Tiles = Common.Data.AllTiles;
        selectedIndex = -1;
        ListSelectedCursor.gameObject.SetActive(false);

        for (int i = 0; i < Tiles.Count; i++)
        {
            GameObject gameObject = Instantiate(infoBox, ListBar);

            TMPro.TextMeshProUGUI TMP = gameObject.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMP.gameObject.SetActive(true);
            TMP.text = Tiles[i].Name;

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = Tiles[i].Sprite;


            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            ListBar.sizeDelta = ListBar.rect.size + new Vector2(150, 0);

            Button button = gameObject.GetComponent<Button>();

            Image Background = gameObject.GetComponent<Image>();
            Color oldColor = Background.color;

            int temp = i;

            button.onClick.AddListener(() =>
            {
                if (selectedIndex == temp)
                {
                    SetCursor(-1);
                }
                else
                {
                    SetCursor(temp);
                    selectedObject = Tiles[temp];
                }
            });
        }
    }

    public void SetArtifactList()
    {
        Transform[] childList = ListBar.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
            Destroy(childList[i].gameObject);
        ListBar.sizeDelta = Vector2.zero;

        var Artifacts = Common.Data.AllArtifacts;
        selectedIndex = -1;
        ListSelectedCursor.gameObject.SetActive(false);

        for (int i = 0; i < Artifacts.Count; i++)
        {
            GameObject gameObject = Instantiate(infoBox, ListBar);

            TMPro.TextMeshProUGUI TMP = gameObject.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMP.gameObject.SetActive(true);
            TMP.text = Artifacts[i].Name;

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = Artifacts[i].Sprite;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            ListBar.sizeDelta = ListBar.rect.size + new Vector2(150, 0);

            Button button = gameObject.GetComponent<Button>();

            Image Background = gameObject.GetComponent<Image>();
            Color oldColor = Background.color;

            int temp = i;

            button.onClick.AddListener(() =>
            {
                if (selectedIndex == temp)
                {
                    SetCursor(-1);
                }
                else
                {
                    SetCursor(temp);
                    selectedObject = Artifacts[temp];
                }
            });
        }
    }

    public void SetItemList()
    {
        Transform[] childList = ListBar.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
            Destroy(childList[i].gameObject);
        ListBar.sizeDelta = Vector2.zero;

        var Items = Common.Data.AllItems;
        selectedIndex = -1;
        ListSelectedCursor.gameObject.SetActive(false);

        for (int i = 0; i < Items.Count; i++)
        {
            GameObject gameObject = Instantiate(infoBox, ListBar);

            TMPro.TextMeshProUGUI TMP = gameObject.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMP.gameObject.SetActive(true);
            TMP.text = Items[i].Name;

            Image image = gameObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = Items[i].Sprite;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            ListBar.sizeDelta = ListBar.rect.size + new Vector2(150, 0);

            Button button = gameObject.GetComponent<Button>();

            Image Background = gameObject.GetComponent<Image>();
            Color oldColor = Background.color;

            int temp = i;

            button.onClick.AddListener(() =>
            {
                if (selectedIndex == temp)
                {
                    SetCursor(-1);
                }
                else
                {
                    SetCursor(temp);
                    selectedObject = Items[temp];
                }
            });
        }
    }

    /// <summary>
    /// 배틀 모드를 변환한다.
    /// </summary>
    public void BattleToggleButton()
    {
        BattleController.SetBattleMode(!GameManager.InBattle);

        // 배틀 모드가 On이 되었다.
        if (GameManager.InBattle)
        {
            Debug.LogWarning("Battle mode ON");
            BattleController.instance.NextTurnStart();
            BattleButtonText.text = "Battle\nON";
        }
        // 배틀 모드가 Off가 되었다.
        else
        {
            Debug.LogWarning("Battle mode OFF");
            BattleButtonText.text = "Battle\nOff";
        }
    }

    public void AutoToggleButtton()
    {
        if (GameManager.InAuto)
        {
            Debug.LogWarning("Auto mode OFF");
            GameManager.InAuto = false;
            AutoButtonText.text = "Auto\nOff";

            if (GameManager.InBattle)
                BattleController.instance.NextTurnStart();
        }
        else
        {
            Debug.LogWarning("Auto mode ON");
            GameManager.InAuto = true;
            AutoButtonText.text = "Auto\nOn";

            if (GameManager.InBattle)
                BattleController.instance.ThisTurnEnd();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            enabledEditMode = !enabledEditMode;
            unitAttributeController.gameObject.SetActive(enabledEditMode);
            battleFieldController.gameObject.SetActive(enabledEditMode);

            if (currentUnit != null)
                InfoView.instance.unitInfo.SetUnit(currentUnit);

        }
    }
}
