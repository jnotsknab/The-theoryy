using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Updates the ui of animated / filled world elements such as a battery on an item, or a meter on a computer screen etc.
/// will be invoked in the <see cref="Player"/> class.
/// 
/// </summary>
public class PlayerWorldUI : MonoBehaviour
{
    [Header("Battery Fill Meter Image")]
    public Image batteryFillIMG;
    public TimefluxLogic timefluxLogic;

    void Update()
    {
        UpdateBatteryUI();
    }

    private void UpdateBatteryUI()
    {
        if (timefluxLogic != null)
        {
            float normalizedCharge = timefluxLogic.batteryCharge / timefluxLogic.maxCharge;
            batteryFillIMG.fillAmount = Mathf.Clamp01(normalizedCharge);
        }
    }
}
