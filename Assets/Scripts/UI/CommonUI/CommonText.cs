using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommonText : MonoBehaviour
{
    private TMP_Text textMesh;
    private Image image;
    private Button button;

    // inspector
    [SerializeField]
    private CommonData.Size size;
    [SerializeField]
    private bool isButton = true;
    [SerializeField]
    private bool usableButton = true;
    [SerializeField]
    private bool hasFrame = true;
    [SerializeField]
    private bool hasText = true;
    [SerializeField]
    private CommonData.SizeField[] sizeDefinition =
    {
        new CommonData.SizeField { width = 180, height = 90 },
        new CommonData.SizeField { width = 256, height = 142 },
        new CommonData.SizeField { width = 320, height = 180 },
        new CommonData.SizeField { width = 416, height = 234 },
        new CommonData.SizeField { width = 512, height = 288 },
    };

    public TMP_Text TextMesh => textMesh;
    
    public string Text
    {
        get => textMesh.text;
        set => textMesh.text = value;
    }

    public float TextAlpha
    {
        get => textMesh.color.a;
        set
        {
            if (value < 0f || value > 1f)
            {
                Debug.LogError("value is not valid");
            }
            var color = textMesh.color;
            color.a = value;
            textMesh.color = color;
        }
    }

    public float FrameAlpha
    {
        get => image.color.a;
        set
        {
            if (value < 0f || value > 1f)
            {
                Debug.LogError("value is not valid");
            }
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }

    public CommonData.Size Size
    {
        get => size;
        set
        {
            var v = (int) value;
            if (v < 0 || v >=  sizeDefinition.Length)
            {
                Debug.LogError("the size is not defined.");
            }
            size = value;
            image.rectTransform.sizeDelta = new Vector2(sizeDefinition[v].width, sizeDefinition[v].height);
        }
    }

    public bool IsButton
    {
        get => isButton;
        set
        {
            isButton = value;
            image.color = GetComponent<Button>().colors.normalColor;
            GetComponent<Button>().enabled = value;
        }
    }


    public bool UsableButton
    {
        get => usableButton;
        set
        {
            usableButton = value;
            button.interactable = value;
        }
    }

    private void OnValidate()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
        Size = size;
        IsButton = isButton;
        UsableButton = usableButton;
        TextAlpha = hasText ? 1f : 0f;
        FrameAlpha = hasFrame ? 1f : 0f;
    }

    public void ChangeSize(CommonData.Size size)
    {
        Size = size;
    }

    public void ShowFrame()
    {
        hasFrame = true;
    }

    public void HideFrame()
    {
        hasFrame = false;
    }

    public void ShowText()
    {
        hasText = true;
    }

    public void HideText()
    {
        hasText = false;
    }

    public void EnableButton()
    {
        UsableButton = true;
    }

    public void DisableButton()
    {
        UsableButton = false;
    }

    public void Hello()
    {
        Debug.Log("Hello");
    }
}
