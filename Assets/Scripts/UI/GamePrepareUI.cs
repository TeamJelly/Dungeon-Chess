using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrepareUI : MonoBehaviour
{
    public static GamePrepareUI instance;
    public CommonText startGameButton;

    public CommonText GotoMainButton;

    List<CommonImage> UnitsImageList;

    public CommonImage showNextUnitButton;
    public CommonImage showPrevUnitButton;

    public CommonImage currentUnitImage;

    private void Awake()
    {
        instance = this;
    }
    public void EnableStartButton()
    {

    }

    public void DIsableStartButton()
    {

    }

    public void UpdateCurrentUnitImage(Sprite sprite)
    {
        currentUnitImage.Sprite = sprite;
    }
}
