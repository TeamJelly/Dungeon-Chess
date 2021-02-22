using Common.UI;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UI.Popup;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI instance;
    public GameObject panel;

    private void Awake()
    {
        instance = this;
    }
    public void GotoMain()
    {
        MenuManager.instance.GotoMain();
    }

    public void GiveUp()
    {
        BattleResultUI.instance.EnableDeafeatUI();
        Disable();
    }
    public void ExitGame()
    {
        MenuManager.instance.ExitGame();
    }
    public void Enable()
    {
        UIEffect.FadeInPanel(panel);
    }

    public void Disable()
    {
        UIEffect.FadeOutPanel(panel);
    }
}
