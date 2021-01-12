using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//유닛 누르면 팝업 형식으로 뜨는 ui
public class UnitDescriptionUI : MonoBehaviour
{
    public CommonText text;
    public static UnitDescriptionUI instance;
    //여기에 유물정보 있으면 좋을 것 같음.

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Enable(Unit unit)
    {
        text.Text =
            "이름: " + unit.name +
            "\nstrength: " + unit.strength +
            "\ndefense: " + unit.defense;
        //text.text = "A";
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
