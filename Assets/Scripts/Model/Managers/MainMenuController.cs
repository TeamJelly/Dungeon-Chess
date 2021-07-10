using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public RectTransform panel;
    public LinkedPanel selected;

    public RectTransform mainPanel;
    public RectTransform unitSelectionPanel;
    public RectTransform optionPanel;

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        LinkedPanel main = new LinkedPanel();
        LinkedPanel unit = new LinkedPanel();
        LinkedPanel option = new LinkedPanel();

        main.panel = mainPanel;
        main.Down = unit;
        main.Right = option;

        unit.panel = unitSelectionPanel;
        unit.Up = main;
        option.panel = optionPanel;
        option.Left = main;

        selected = main;
    }
    IEnumerator Show(LinkedPanel toShowPanel)
    {
        Vector3 from = panel.position;
        Vector3 to = from + selected.panel.position - toShowPanel.panel.position;
        float t = 1;
        while (t > 0.001f)
        {
            t *= 0.9f;
            panel.position = Vector3.Lerp(from, to,1 -  t);
            yield return null;
        }
        panel.position = to;
        selected = toShowPanel;
    }
    public void MoveLeft()
    {
        if (selected.Left != null)
        {
            StartCoroutine(Show(selected.Left));
        }
    }

    public void MoveRight()
    {
        if (selected.Right != null)
        {
            StartCoroutine(Show(selected.Right));
        }
    }

    public void MoveUp()
    {
        if (selected.Up != null)
        {
            StartCoroutine(Show(selected.Up));
        }
    }
    public void MoveDown()
    {
        if (selected.Down != null)
        {
            StartCoroutine(Show(selected.Down));
        }
    }
}


public class LinkedPanel
{

    public RectTransform panel;

    public LinkedPanel Left;
    public LinkedPanel Right;
    public LinkedPanel Up;
    public LinkedPanel Down;
}