using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//유닛 누르면 팝업 형식으로 뜨는 ui
public class UnitDescriptionUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI text;

    public Button closeButton;
    public static UnitDescriptionUI instance;
    //여기에 유물정보 있으면 좋을 것 같음.

    private void Awake()
    {
        instance = this;
        closeButton.onClick.AddListener(Disable);
    }

    public void Enable(Unit unit)
    {
        text.text =
            "이름: " + unit.name +
            "\nstrength: " + unit.strength +
            "\ndefense: " + unit.defense;
        panel.SetActive(true);
    }

    public void Disable()
    {
        panel.SetActive(false);
    }
}
