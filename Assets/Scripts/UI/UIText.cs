
using TMPro;
using UnityEngine;

public class UIText : CustomUIComponent
{
    public UITextSO textData;
    public Style style;

    private TextMeshProUGUI textMeshProUGUI;


    public override void Setup()
    {   
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshProUGUI == null)
        {
            Debug.LogError("TMPGUI NULL");
        }
    }

    public override void Configure()
    {
        textMeshProUGUI.color = textData.theme.GetTextColor(style);
        textMeshProUGUI.font = textData.font;
        textMeshProUGUI.fontSize = textData.size;
    }
}
