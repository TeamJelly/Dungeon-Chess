using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OXView : MonoBehaviour
{
    public GameObject oxUI;
    static OXView instance;

    public TextMeshProUGUI title;

    public Button yesButton;
    public Button noButton;
    private void Awake()
    {
        instance = this;
    }

    public static void Enable(string title, UnityAction Confirm, UnityAction Discard)
    {
        instance.title.text = title;

        instance.yesButton.onClick.RemoveAllListeners();
        instance.noButton.onClick.RemoveAllListeners();

        instance.yesButton.onClick.AddListener(Confirm);
        instance.noButton.onClick.AddListener(Discard);

        //Debug.Log("OXView Enabled");
        Common.UIEffect.FadeInPanel(instance.oxUI);
    }

    public static void Disable()
    {
        Common.UIEffect.FadeOutPanel(instance.oxUI);
    }
}
